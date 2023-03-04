using Microsoft.AspNetCore.SignalR.Client;
using Wtdl.Controller.SignalRHub;
using Wtdl.Share.SignalR;

namespace Wtdl.Controller.Services
{
    public class HubService
    {
        private HubConnection _connection;

        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILogger<HubService> _logger;

        public HubService(IConfiguration configuration, HttpClient client, ILogger<HubService> logger)
        {
            //   _connection = connection;
            _logger = logger;
            _configuration = configuration;

            _ = TryInitializeHub();
        }

        private async Task TryInitializeHub()
        {
            if (_connection is null)
            {
                var hubUrl = _configuration.GetValue<string>("SignalR:HubUrl");
                _connection = new HubConnectionBuilder()
                    .WithUrl(hubUrl, options =>
                    {
                        options.AccessTokenProvider = async () =>
                        {
                            return await GetSignalRAppToken();
                        };
                    })
                    .WithAutomaticReconnect()
                    .Build();
                _connection.StartAsync();
            }
        }

        /// <summary>
        /// 返回授权token
        /// </summary>
        /// <returns></returns>
        private async Task<string?> GetSignalRAppToken()
        {
            ///http调用，返回token
            var tokenurl = _configuration.GetValue<string>("SignalR:TokenUrl");
            return await _httpClient.GetStringAsync(tokenurl);
        }

        public async Task SendLotteryCountAsync()
        {
            try
            {
                await TryInitializeHub();
                await _connection.InvokeAsync(HubServerMethods.SendLotteryCount);
            }
            catch (Exception e)
            {
                _logger.LogError($"signalR连接错误：,{e.Message}");
                // Console.WriteLine(e);
                // throw;
            }

            //  throw new NotImplementedException();
        }

        public async Task SendLotteryWinCountAsync()
        {
            try
            {
                await TryInitializeHub();
                await _connection.InvokeAsync(HubServerMethods.SendLotteryWinCount);
            }
            catch (Exception e)
            {
                _logger.LogError($"signalR连接错误：,{e.Message}");
                // Console.WriteLine(e);
                // throw;
            }
        }

        public async Task SendSendRedPackedCountAsync()
        {
            try
            {
                await TryInitializeHub();
                await _connection.InvokeAsync(HubServerMethods.SendRedPackedCount);
            }
            catch (Exception e)
            {
                _logger.LogError($"signalR连接错误：,{e.Message}");
                // Console.WriteLine(e);
                // throw;
            }
        }

        /// <summary>
        /// 通知发放红包成功
        /// </summary>
        /// <param name="totalamount"></param>
        /// <returns></returns>
        public async Task SendSendRedpacketTotalAmountAsync(string totalamount)
        {
            try
            {
                await TryInitializeHub();
                await _connection.InvokeAsync(HubServerMethods.SendRedpacketTotalAmount, totalamount);
            }
            catch (Exception e)
            {
                _logger.LogError($"signalR连接错误：,{e.Message}");
                // throw;
            }
        }
    }
}