using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using Wtdl.Controller.SignalRHub;
using Wtdl.Share;
using Wtdl.Share.SignalR;

namespace Wtdl.Controller.Services
{
    public class HubService
    {
        private static HubConnection _connection;

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HubService> _logger;

        private static string Token;
        private static string HubUrl;

        public HubService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<HubService> logger)
        {
            //   _connection = connection;
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _ = TryInitializeHub();
            Token = GetSignalRAppToken().Result;
            HubUrl = configuration.GetValue<string>("SignalR:HubUrl");

            //  _connection.TryInitialize(HubUrl, Token);
        }

        private async Task TryInitializeHub()
        {
            _connection = _connection.TryInitialize(HubUrl, Token);
            await _connection.StartAsync();
        }

        /// <summary>
        /// 返回授权token
        /// </summary>
        /// <returns></returns>
        private async Task<string?> GetSignalRAppToken()
        {
            ///http调用，返回token
            var tokenurl = _configuration.GetValue<string>("SignalR:TokenUrl");

            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                tokenurl)
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/json" },
                    { HeaderNames.UserAgent, "HttpRequestsSample" }
                }
            };

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream =
                   await httpResponseMessage.Content.ReadAsStringAsync();

                var LoginResult = JsonSerializer.Deserialize<LoginResult>(contentStream);
                return LoginResult.Token;
                //GitHubBranches = await JsonSerializer.DeserializeAsync
                //    <IEnumerable<GitHubBranch>>(contentStream);
            }

            return ""; //await _httpClient.GetStringAsync(tokenurl);
        }

        public async Task SendLotteryCountAsync()
        {
            try
            {
                _connection = _connection.TryInitialize(HubUrl, Token);
                if (_connection.State != HubConnectionState.Connected)
                {
                    await _connection.StartAsync();
                }

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
                _logger.LogError($"SendSendRedpacketTotalAmountAsync()-signalR连接错误：,{e.Message}");
                // throw;
            }
        }
    }
}