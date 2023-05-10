using Microsoft.AspNetCore.SignalR.Client;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ScanCode.Share;
using ScanCode.Share.SignalR;
using ScanCode.WPF.HubServer.Entity;
using ScanCode.WPF.HubServer.Model;
using ScanCode.WPF.HubServer.ReQuest;
using ScanCode.WPF.HubServer.Response;
using ScanCode.WPF.HubServer.ViewModels;

namespace ScanCode.WPF.HubServer.Services
{
    public class HubClientService
    {
        // public static User _usernnfo = new User();
        // private WTDLContext DbEntities { set; get; }

        private readonly string HubUrl; // ConfigurationManager.ConnectionStrings["HubUrl"].ConnectionString;
        private readonly string LoginUrl; //= ConfigurationManager.ConnectionStrings["LoginUrl"].ConnectionString;

        // private string applicationName = app.Configuration.GetSection("AppSettings:ApplicationName").Value;
        // private string applicationVersion = app.Configuration.GetSection("AppSettings:ApplicationVersion").Value;

        private HubConnection hubConnection;

        private List<Claim> Claims = new List<Claim>();

        // private const string NotPut = "NotPut";

        /// <summary>
        /// 默认在线 true,l离线为false
        /// </summary>
        private static bool OnlineOrOffline = true;

        public HubClientService(string huburl, string loginurl)
        {
            HubUrl = huburl;
            LoginUrl = loginurl;
            // DbEntities = new WTDLContext();
        }

        //}

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="addtLabelx"></param>
        /// <returns></returns>
        public async Task<OutStorageResult> AddScanCodeAsync(W_LabelStorage storage)
        {
            try
            {
                //判断是网络是Online还是Offline
                if (hubConnection.State == HubConnectionState.Connected)
                {
                    try
                    {
                        hubConnection = hubConnection.TryInitialize();

                        storage.Adminaccount = hubConnection.TryGetUser().UserName;
                        var result = await hubConnection.InvokeAsync<OutStorageResult>(HubServerMethods.SendOutStorage, storage);
                        return result;
                    }
                    catch (Exception e)
                    {
                        return OutStorageResult.Fail($"{e.Message}", storage.QRCode);
                    }
                }
                else
                {
                    return OutStorageResult.Fail($"网络断开，请检查网络是否正常", storage.QRCode);
                }
            }
            catch (Exception ex)
            {
                string strinfo = ex.Message;
                return OutStorageResult.Fail($"出现异常：{strinfo}", storage.QRCode);
            }
        }

        private async Task<tLabelsxModel> TLabelsxModelOffline(AddtLabelx addtLabelx, W_LabelStorage tlabelx)
        {
            ///网络不通的情况下，数据保存到本地数据库
            //using (var context = new WTDLContext())
            //{
            //    tlabelx.ExtensionOrder = $"{DateTime.Now.Day.ToString("yyyyMMdd")}";
            //    context.W_LabelStorages.Add(tlabelx);
            //    int i = await context.SaveChangesAsync();
            //    if (i > 0)
            //    {
            //        return UPdateAddtLabelsxModel(addtLabelx, 200, "出库成功");
            //    }
            //    return UPdateAddtLabelsxModel(addtLabelx, 400, "出库失败");
            //}
            return UPdateAddtLabelsxModel(addtLabelx, 400, "出库失败");
        }

        /// <summary>
        /// 根据返回的数据状态 更新UI状态
        /// </summary>W_LabelStorage w_LabelStorage
        /// <param name="w_LabelStorage"></param>
        /// <param name="addtLabelx"></param>
        /// <param name="ResulCode"></param>
        /// <param name="ResultStatus"></param>
        /// <param name="Errorinfo"></param>
        /// <returns></returns>
        public tLabelsxModel UPdateAddtLabelsxModel(AddtLabelx addtLabelx, int ResulCode, string ResultStatus,
            string Errorinfo = "")
        {
            tLabelsxModel tLabelsxModel = new tLabelsxModel();
            tLabelsxModel.QRCode = addtLabelx.QRCode;
            tLabelsxModel.Aname = addtLabelx.DealersName;
            tLabelsxModel.OrderNumbel = addtLabelx.OrderNumbels;
            tLabelsxModel.ResulCode = ResulCode;
            tLabelsxModel.ResultStatus = ResultStatus;
            tLabelsxModel.Errorinfo = Errorinfo;
            // tLabelsxModel.tLabels_X = w_LabelStorage;
            return tLabelsxModel;
        }

        /// <summary>
        /// 退货，更新数据库
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public async Task<DBResult<int>> DeeletetLabelX(string qrcode)
        {
            try
            {
                await hubConnection.InvokeAsync(HubServerMethods.SendOutStorageDayCount, false);
                return DBResult<int>.Success(1);
                //using (var context = new WTDLContext())
                //{
                //    var w_LabelStorage = await context.W_LabelStorages.FirstOrDefaultAsync(x =>
                //        x.OutTime.Year == DateTime.Now.Year && x.OutTime.Month == DateTime.Now.Month &&
                //        x.OutTime.Day == DateTime.Now.Day && x.QRCode.Contains(qrcode));
                //    if (w_LabelStorage != null)
                //    {
                //        context.W_LabelStorages.Remove(w_LabelStorage);
                //        var delete = await context.SaveChangesAsync();

                //        if (delete > 0)
                //        {
                //            await hubConnection.InvokeAsync(HubServerMethods.SendOutStorageDayCount, false);
                //            return DBResult<int>.Success(delete);
                //        }
                //    }
                //    else
                //    {
                //        return DBResult<int>.Fail("数据还没有出库");
                //    }
                //}
            }
            catch (Exception e)
            {
                return DBResult<int>.Fail($"出现异常：{e.Message}");
            }

            return DBResult<int>.Fail($"失败");
        }

        /// <summary>
        /// 记录已经完成得出库单
        /// </summary>
        public async void AddOrder(W_OrderTable orderTable)
        {
            try
            {
                //if (orderTable != null)
                //{
                //    W_OrderTable w_OrderTable = DbEntities.W_OrderTables.AsNoTracking()
                //        .Where(w => w.OrderId.Trim().Contains(orderTable.OrderId.Trim())).FirstOrDefault();

                //    if (w_OrderTable == null)
                //    {
                //        using (var context = new WTDLContext())
                //        {
                //            context.W_OrderTables.Add(orderTable);
                //            int ret = await context.SaveChangesAsync();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                //  throw;
            }
            //你的代码
        }

        public async Task<ResponseModel> AddAgent(AddAgent addAgent)
        {
            if (hubConnection.State == HubConnectionState.Connected)
            {
                hubConnection = hubConnection.TryInitialize();
                await hubConnection.InvokeAsync(HubServerMethods.SendAgent, addAgent);
                return new ResponseModel();
            }

            return new ResponseModel();
        }

        /// <summary>
        /// 删除tLabelX   用于退货操作
        /// </summary>
        public async Task<ReturnsStorageResult> ReturnsLabelX(string qrcode)
        {
            if (hubConnection.State == HubConnectionState.Connected)
            {
                try
                {
                    hubConnection = hubConnection.TryInitialize();
                    // _ = hubConnection.InvokeAsync(HubServerMethods.SendOutStorageDayCount, true);

                    //退货
                    return await hubConnection.InvokeAsync<ReturnsStorageResult>(HubServerMethods.Returns_OutStorage, qrcode);
                }
                catch (Exception e)
                {
                    return ReturnsStorageResult.Exception(qrcode, e.Message);
                    //return await TLabelsxModelOffline(addtLabelx, tlabelx);
                }
            }
            else
            {
                return ReturnsStorageResult.Offline(qrcode);
                //var tLabelxcode = DbEntities.W_LabelStorages.AsNoTracking().Where(o => o.QRCode == tLabelxId||o.OrderNumbels.Trim()== tLabelxId.Trim()).FirstOrDefault();
                //using (var context = new WTDLContext())
                //{
                //    var tLabelxcode = context.W_LabelStorages.AsNoTracking()
                //        .Where(o => o.QRCode == tLabelxId || o.OrderNumbels == tLabelxId).ToList();

                //    try
                //    {
                //        if (tLabelxcode != null && tLabelxcode.Count == 0)
                //        {
                //            return new +
                //                { QrCode = tLabelxId, ReCount = "0", ResulCode = 0, ResultStatus = "还未发货" };
                //        }

                //        context.W_LabelStorages.RemoveRange(tLabelxcode);
                //        int i = await context.SaveChangesAsync();
                //        if (i > 0)
                //        {
                //            // await hubConnection.InvokeAsync(HubServerMethods.SendOutStorageDayCount, false);
                //            return new ReturnsStorageResult
                //            { QrCode = tLabelxId, ReCount = i.ToString(), ResulCode = 200, ResultStatus = "退货成功" };
                //        }

                //        return new ReturnsStorageResult
                //        { QrCode = tLabelxId, ReCount = i.ToString(), ResulCode = 400, ResultStatus = "退货失败" };
                //    }
                //    catch (Exception ex)
                //    {
                //        return ReturnsStorageResult.Exception(tLabelxId, ex.Message);
                //    }
                //}
            }
        }

        /// <summary>
        /// 获取tLabelX  出库单实际已经出库数量集合
        /// </summary>
        //public async Task<ResponseModel> GettLabelXList(string beingTime, string ordernumbel = "")
        //{
        //    using (var context = new WTDLContext())
        //    {
        //        // DateTime dateTime = DateTime.Now.AddDays(-30);
        //        string Commandtext = "";
        //        if (ordernumbel != "")
        //        {
        //            Commandtext =
        //                string.Format(
        //                    @"select OrderNumbels ,count(OrderNumbels) as OrderCount from W_LabelStorage where OutTime> '{0}' and OrderNumbels='{1}'  GROUP BY  OrderNumbels",
        //                    beingTime, ordernumbel);
        //        }
        //        else
        //        {
        //            Commandtext =
        //                string.Format(
        //                    @"select OrderNumbels ,count(OrderNumbels) as OrderCount from W_LabelStorage where OutTime> '{0}'  GROUP BY  OrderNumbels",
        //                    beingTime);
        //        }

        //        var Ordercounts = await context.Database.SqlQuery<tLabels_OrderCount>(Commandtext).ToListAsync();

        //        //  var banners = DbEntities.tLabels_X.Where(w=>w.OrderNumbels,w).ToList().OrderByDescending(c => c.fhDate1);
        //        var response = new ResponseModel();
        //        response.code = 200;
        //        response.result = "Labels集合获取成功";
        //        response.data = new List<tLabels_OrderCount>();

        //        response.data = Ordercounts;

        //        return response;
        //    }
        //}

        /// <summary>
        /// 返回单个订单的实际扫码数量
        /// </summary>
        /// <param name="ddno"></param>
        /// <returns></returns>
        public async Task<int> GetBdxOrderTotalCountAsync(string ddno)
        {
            if (hubConnection.State == HubConnectionState.Connected)
            {
                hubConnection = hubConnection.TryInitialize();
                return await hubConnection.InvokeAsync<int>(HubServerMethods.BDXORDER_TOTAL_COUNT, ddno);
            }
            return 0;
        }

        /// <summary>
        /// 返回单个订单的详细信息
        /// </summary>
        /// <param name="ddno"></param>
        /// <returns></returns>
        public async Task<List<T_BDX_ORDER>> GetBdxOrdersAsync(string ddno)
        {
            try
            {
                if (hubConnection.State == HubConnectionState.Connected)
                {
                    hubConnection = hubConnection.TryInitialize();
                    return await hubConnection.InvokeAsync<List<T_BDX_ORDER>>(HubServerMethods.BDXORDER_LIST, ddno);
                }
                return new List<T_BDX_ORDER>();
            }
            catch (Exception e)
            {
                return new List<T_BDX_ORDER>();
            }
        }

        /// <summary>
        /// 用户登录系统
        /// </summary>
        /// <param name="loginData"></param>
        /// <returns></returns>
        public async Task<UserResult> UserLoging(LoginData loginData, bool isonline)
        {
            hubConnection = hubConnection.TryInitialize(HubUrl, LoginUrl, loginData.Username, loginData.Password);
            hubConnection.On<string>(HubClientMethods.OnDeleteSynchronizationData, async (deleteid) =>
            {
                // await DeleteSynchronizationData(deleteid);
                //服务器后台通过这里控制PC端是否可以使用
                //this.Dispatcher.Invoke(() =>
                //{
                //    var newMessage = $"{user}: {message}";
                //    messagesList.Items.Add(newMessage);
                //});
            });
            hubConnection.Closed += async (error) =>
            {
                //OnlineOrOffline = false;
                // 网络断开的处理逻辑，一直循环重新连接上为止.
                await Task.Delay(new Random().Next(0, 3) * 1000);
                await ConnectWithRetryAsync();
            };

            hubConnection.Reconnected += (connectionId) =>
            {
                //OnlineOrOffline = true;
                // 重新连接成功的处理逻辑，重新上传保存在本地的数据
                Console.WriteLine($"重新连接成功！:{connectionId}");
                return Task.CompletedTask;
            };

            hubConnection.Reconnecting += (connectionId) =>
            {
                //进入重连状态
                return Task.CompletedTask;
            };

            try
            {
                await hubConnection.StartAsync();
                var usernnfo = hubConnection.TryGetUser();
                return UserResult.Success(usernnfo);
            }
            catch (Exception ex)
            {
                //if (!isonline)
                //{
                //    _ = ConnectWithRetryAsync();
                //    return UserResult.Success(new User() { UserName = loginData.Username, UserID = "OfflineUserId" });
                //}

                return UserResult.Failed(ex.Message);
            }
        }

        public ResponseModel GetOrdersFinish(string beingDatePickers)
        {
            DateTime dateTime = DateTime.Now.AddDays(-7);
            List<W_OrderTable>
                listfinish = null; //DbEntities.W_OrderTables.AsNoTracking().Where(T=>T.FinishTime> dateTime).ToList();
            var response = new ResponseModel();
            if (listfinish != null)
            {
                response.code = 200;
                response.result = "Labels集合获取成功";
                response.data = new List<W_OrderTable>();

                response.data = listfinish;
            }
            else
            {
                response.code = 400;
                response.result = "完成出库单集合获取失败";
            }

            return response;
            //  throw new NotImplementedException();
        }

        private async Task ConnectWithRetryAsync()
        {
            while (true)
            {
                try
                {
                    hubConnection = hubConnection.TryInitialize();
                    await hubConnection.StartAsync();
                    Console.WriteLine("循环重试网络连接成功！");
                    break; //连接成功 退出循环
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("401"))
                    {
                        Console.WriteLine($"SignalR连接失败！:{ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 把还未同步的列表数据分割成大小为1000的数据块，分批上传,只同步30天以内的离线数据
        /// </summary>
        /// <returns></returns>
        //private async Task Synchronization()
        //{
        //    using (var context = new WTDLContext())
        //    {
        //        var resultList = await context.W_LabelStorages.AsNoTracking()
        //            .Where(e => e.ExtensionOrder != null && e.OutTime >= DateTime.Now.Date.AddDays(-14)).ToListAsync();

        //        var groupbyList = resultList.GroupBy(e => e.ExtensionOrder).ToDictionary(g => g.Key, g => g.ToList());
        //        foreach (var variable in groupbyList)
        //        {
        //            // 将数据分割成大小为1000的数据块
        //            var dataChunks = ChunkList(variable.Value, 1000);
        //            foreach (var chunk in dataChunks)
        //            {
        //                if (hubConnection.State == HubConnectionState.Connected)
        //                {
        //                    var result = await hubConnection.InvokeAsync<OutStorageResult>(HubServerMethods.SendOutStorageBatch, chunk);
        //                }
        //            }

        //            //上传完一组数据， 就删除一组数据
        //            await hubConnection.InvokeAsync(HubServerMethods.SendDeleteSynchronizationData, SynchState.SendCompleted(variable.Key));
        //        }
        //    }
        //}

        //public static List<List<T>> ChunkList<T>(List<T> list, int chunkSize)
        //{
        //    List<List<T>> chunks = new List<List<T>>();

        //    for (int i = 0; i < list.Count; i += chunkSize)
        //    {
        //        chunks.Add(list.GetRange(i, Math.Min(chunkSize, list.Count - i)));
        //    }

        //    return chunks;
        //}

        ///离线数据已经全部上传，需要完全删除
        //private async Task DeleteSynchronizationData(string notput)
        //{
        //    using (var context = new WTDLContext())
        //    {
        //        context.W_LabelStorages.RemoveRange(context.W_LabelStorages.Where(w => w.ExtensionOrder.Contains(notput)));
        //        await context.SaveChangesAsync();
        //    }
        //}

        /// <summary>
        /// 返回ERP 订单分组集合数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<GroupedBdxOrder>> GetGroupedBdxOrdersAsync(string ddno = "")
        {
            if (hubConnection.State == HubConnectionState.Connected)
            {
                hubConnection = hubConnection.TryInitialize();

                // List < GroupedBdxOrder > groupe_orders = new List<GroupedBdxOrder>();
                if (string.IsNullOrWhiteSpace(ddno))
                {
                    var groupe_orders = await hubConnection.InvokeAsync<List<GroupedBdxOrder>>(HubServerMethods.GROUPED_ORDERS);

                    return groupe_orders;
                }

                var groupe_ddnos = await hubConnection.InvokeAsync<List<GroupedBdxOrder>>(HubServerMethods.Grouped_DDNO, ddno);

                return groupe_ddnos;
            }
            return new List<GroupedBdxOrder>();
        }

        public async Task<IEnumerable<T_BDX_ORDER>> GetOrderDetailAsync(string ddno)
        {
            try
            {
                if (hubConnection.State == HubConnectionState.Connected)
                {
                    hubConnection = hubConnection.TryInitialize();
                    return await hubConnection.InvokeAsync<List<T_BDX_ORDER>>(HubServerMethods.BDXORDER_LIST, ddno);
                }
                return new List<T_BDX_ORDER>();
            }
            catch (Exception e)
            {
                return new List<T_BDX_ORDER>();
            }
        }
    }
}