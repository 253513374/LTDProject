using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ScanCode.Model.Entity;
using ScanCode.Model.Entity.ERP;
using ScanCode.Model.ResponseModel;
using ScanCode.RedisCache;
using ScanCode.Repository;
using ScanCode.Share;
using ScanCode.Share.SignalR;
using Senparc.Weixin.TenPay.V2;
using System.Reflection.Emit;

namespace ScanCode.Web.Admin.SignalRHub
{
    /// <summary>
    /// 指定策略才能访问
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class APPHub : Hub
    {
        private readonly IMemoryCache _cache;
        private readonly object _cacheLock = new object();

        private readonly WLabelStorageRepository _storageRepository;
        private readonly AgentRepository _agentRepository;
        private readonly BdxOrderRepository _bdxOrderRepository;

        private ILogger<APPHub> _logger;

        private readonly IRedisCache _redisCache;

        //构造函数
        public APPHub(IMemoryCache cache, IRedisCache redisCache,
            ILogger<APPHub> logger,
            WLabelStorageRepository repository,
            AgentRepository agentRepository,
            BdxOrderRepository bdxOrderRepository)
        {
            _bdxOrderRepository = bdxOrderRepository;
            _redisCache = redisCache;
            _storageRepository = repository;
            _logger = logger;
            _cache = cache;
            _agentRepository = agentRepository;
        }

        public async Task SendAgentAsync(Agent agent)
        {
            //判断Agent 中是否存在
            var result = await _agentRepository.AnyAsync(f => f.AID == agent.AID);
            if (!result)
            {
                var resultint = await _agentRepository.AddAsync(agent);
            }
            return;
        }

        /// <summary>
        /// 扫码退货
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public async Task<ReturnsStorageResult> ReturnsOutStorage(string qrcode)
        {
            var isOut = await _redisCache.GetBitAsync(qrcode);
            if (isOut)
            {
                //已经发货，可以退货
                var result = await _storageRepository.DeleteAsync(qrcode);

                if (result)
                {
                    //退货成功
                    await _redisCache.SetBitAsync(qrcode, false);//设置出库缓存状态
                    await Clients.All.SendAsync(HubClientMethods.OnOutStorageDayCount, false);
                    return ReturnsStorageResult.Success(qrcode);
                }

                return ReturnsStorageResult.Fail(qrcode);
            }

            return ReturnsStorageResult.NotOutFail(qrcode);
        }

        /// <summary>
        /// 订单退货,这是一个批量退货操作
        /// </summary>
        /// <param name="ddno"></param>
        /// <returns></returns>
        public async Task<ReturnsStorageResult> ReturnsOutStorageDdnoAsync(string ddno)
        {
            var anyOrder = await _storageRepository.AnyAsync(a => a.OrderNumbels == ddno);

            if (!anyOrder)
            {
                _logger.LogWarning(ddno, "找不到订单号");
                return ReturnsStorageResult.NotOutFail(ddno, "找不到订单号");
            }
            //已经发货，可以退货
            var result = await _storageRepository.DeleteDdnoAsync(ddno);

            _logger.LogInformation($"删除信息数量：{result.Item1.Count}");
            _logger.LogInformation($"删除信息：{result.Item1}");

            if (result.Item1.Count > 0)
            {
                //退货成功
                await _redisCache.SetBulkBitAsync(result.Item1, false);//设置出库缓存状态
                //await Clients.All.SendAsync(HubClientMethods.OnOutStorageDayCount, false);
                return ReturnsStorageResult.SuccessList(result.Item1);
            }

            return ReturnsStorageResult.FailList(result.Item1);
        }

        /// <summary>
        /// 批量发货
        /// </summary>
        /// <param name="labelStorages"></param>
        /// <returns></returns>
        //public async Task<OutStorageResult> SendOutStorageBatchAsync(List<W_LabelStorage> labelStorages)
        //{
        //    // var username = Context.User.Identity.Name;
        //    // storage.Adminaccount = username;
        //    try
        //    {
        //        await _storageRepository.BulkInsertAsync(labelStorages);
        //        await _redisCache.SetBulkBitAsync(labelStorages.Select(s => s.QRCode).ToList());

        //        return OutStorageResult.Success(DateTime.Now, labelStorages.Count, "BatchInsert");
        //    }
        //    catch (Exception e)
        //    {
        //        return OutStorageResult.FailList($"发货异常:{e.Message}", labelStorages);
        //    }
        //}

        /// <summary>
        /// 实时出库。数据写入数据库
        /// </summary>
        /// <param name="storage"></param>
        /// <returns></returns>
        public async Task<OutStorageResult> SendOutStorageAsync(W_LabelStorage storage)
        {
            var isOut = await _redisCache.GetBitAsync(storage.QRCode);
            if (isOut)
            {
                var result = OutStorageResult.Fail("重复发货", storage.QRCode);
                _logger.LogError($"重复发货：{result.ToString()}");
                return result;
            }

            try
            {
                var result = await _storageRepository.AddAsync(storage);

                if (result > 0)
                {
                    await _redisCache.SetBitAsync(storage.QRCode);
                    await Clients.All.SendAsync(HubClientMethods.OnOutStorageDayCount, true);
                    return OutStorageResult.Success(DateTime.Now, result, storage.QRCode);
                }
                var errorresult = OutStorageResult.Fail("未知错误", storage.QRCode);

                _logger.LogError($"未知错误：{errorresult.ToString()}");
                return errorresult;
            }
            catch (Exception e)
            {
                _logger.LogError($"发货异常：{e.Message}");
                //发货失败
                return OutStorageResult.Fail($"发货异常:{e.Message}", storage.QRCode);
            }
        }

        /// <summary>
        /// 获取时间范围内 ERP 出库单列表
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<List<GroupedBdxOrder>> SendGroupedBdxOrderAsync()
        {
            var ssss = await _bdxOrderRepository.GetGroupedBdxOrdersAsync();

            _logger.LogInformation($"获取时间范围内 ERP 出库单列表：{ssss.Count}");
            return ssss;
            // var list = await _storageRepository.GetOutStorageListAsync(state.SynchDataKey);
            //await Clients.All.SendAsync(HubClientMethods.OnOutStorageList, list);
        }

        /// <summary>
        /// 返回模糊查询订单号分组
        /// </summary>
        /// <returns></returns>
        public async Task<List<GroupedBdxOrder>> SendGroupedBdxOrdersDDNOAsync(string ddno)
        {
            return await _bdxOrderRepository.GetGroupedBdxOrdersDDNOAsync(ddno);
        }

        /// <summary>
        /// 获取订单号的实际出库数量
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<int> SendOrderCountByDDNOAsync(string ddno)
        {
            //根据订单号返回实际数量
            var list = await _storageRepository.GetOrderCountByDDNOAsync(ddno);
            // await Clients.All.SendAsync(HubClientMethods., list);
            return list;
        }

        /// <summary>
        /// 返回二维码详细出库信息
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public async Task<TraceabilityResult?> SendGetWLabelStorageAsync(string qrcode)
        {
            var lisQRCode = await _storageRepository.GetWLabelStorageAsync(qrcode);

            if (lisQRCode is null)
            {
                _logger.LogWarning($"查询二维码{qrcode}，返回null");
                return null;
            }
            var outqrcode = await _agentRepository.FindSingleAgentAsync(lisQRCode.OrderNumbels.Trim()).ConfigureAwait(false);

            return new TraceabilityResult
            {
                Status = true,
                AgentName = outqrcode.AName,
                OrderNumbels = lisQRCode.OrderNumbels,
                OutTime = lisQRCode.OutTime,
                QRCode = lisQRCode.QRCode
            };
            // await Clients.All.SendAsync(HubClientMethods., list);
            // return lisQRCode;
        }

        /// <summary>
        /// 返回订单号详细列表
        /// </summary>
        /// <param name="ddno"></param>
        /// <returns></returns>
        public async Task<List<BdxOrder>> SendBdxOrderListAsync(string ddno)
        {
            var list = await _bdxOrderRepository.GetBdxOrderListAsync(ddno);
            // await Clients.All.SendAsync(HubClientMethods., list);
            return list;
        }

        public async Task SendDeleteSynchronizationDataAsync(SynchState state)
        {
            await Clients.All.SendAsync(HubClientMethods.OnDeleteSynchronizationData, state.SynchDataKey);
        }

        public async Task SendLotteryWinCount()
        {
            await Clients.All.SendAsync(HubClientMethods.OnLotteryWinCount, 11);
            return;
        }

        public async Task SendLotteryCount()
        {
            await Clients.All.SendAsync(HubClientMethods.OnLotteryCount, 11);
            return;
        }

        public async Task SendRedPackedCount()
        {
            await Clients.All.SendAsync(HubClientMethods.OnRedPackedCount, 11);
            return;
        }

        public async Task SendRedpacketTotalAmount(int totalamount)
        {
            await Clients.All.SendAsync(HubClientMethods.OnRedpacketTotalAmount, totalamount);
            return;
        }
    }
}