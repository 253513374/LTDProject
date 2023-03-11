using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client.Extensions.Msal;
using Senparc.Weixin.TenPay.V3;
using Weitedianlan.Model.Entity;
using Wtdl.Admin.Data;
using Wtdl.Repository;
using Wtdl.Share;
using Wtdl.Share.SignalR;

namespace Wtdl.Admin.SignalRHub
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

        private ILogger<APPHub> _logger;

        //构造函数
        public APPHub(IMemoryCache cache, ILogger<APPHub> logger, WLabelStorageRepository repository, AgentRepository agentRepository)
        {
            _storageRepository = repository;
            _logger = logger;
            _cache = cache;
            _agentRepository = agentRepository;
        }

        #region 发送信息

        /// <summary>
        /// 实时更新当日出库数据量
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendOutStorageDayCount(bool ationtype)
        {
            var now = DateTime.Now;
            var timeToMidnight = new TimeSpan(24, 0, 0) - now.TimeOfDay;
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(timeToMidnight);
            int newValue;
            lock (_cacheLock)
            {
                int counter;
                if (!_cache.TryGetValue(CacheKeys.DayCacheKey, out counter))
                {
                    _logger.LogInformation("从数据库获取数据");
                    //获取当前数据
                    counter = _storageRepository.GetCount();
                    _cache.Set(CacheKeys.DayCacheKey, counter);
                    _logger.LogInformation("从数据库获取数据完成");
                }

                if (ationtype)
                {
                    newValue = Interlocked.Increment(ref counter);
                }
                else
                {
                    //自减操作
                    newValue = Interlocked.Decrement(ref counter);
                }

                _cache.Set(CacheKeys.DayCacheKey, newValue, options);
            }

            //_logger.LogInformation("缓存更新完成");

            //Context.User.Claims.ToList().ForEach(x =>
            //{
            //    _logger.LogInformation($"{x.Type} : {x.Value}");
            //});
            ///调用所有客户端的方法【SendReportFormsNever】
            await Clients.All.SendAsync(HubClientMethods.OnOutStorageDayCount, newValue);
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

        public async Task<OutStorageResult> SendOutStorageBatchAsync(List<W_LabelStorage> labelStorages)
        {
            // var username = Context.User.Identity.Name;
            // storage.Adminaccount = username;
            await _storageRepository.BulkInsertAsync(labelStorages);

            return OutStorageResult.Success(DateTime.Now, labelStorages.Count, "BatchInsert");
        }

        /// <summary>
        /// 实时出库。数据写入数据库
        /// </summary>
        /// <param name="storage"></param>
        /// <returns></returns>
        public async Task<OutStorageResult> SendOutStorageAsync(W_LabelStorage storage)
        {
            //var username = Context.User.Identity.Name;
            //storage.Adminaccount = username;
            var result = await _storageRepository.AddAsync(storage);

            if (result > 0)
            {
                await Clients.All.SendAsync(HubClientMethods.OnOutStorageDayCount, 0);
                return OutStorageResult.Success(DateTime.Now, result, storage.QRCode);
            }
            //发货失败
            return OutStorageResult.Fail("发货失败", storage.QRCode);
        }

        public async Task SendDeleteSynchronizationDataAsync(SynchState state)
        {
            await Clients.All.SendAsync(HubClientMethods.OnDeleteSynchronizationData, state.SynchDataKey);
        }

        #endregion 发送信息

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