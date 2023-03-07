using Microsoft.AspNetCore.SignalR.Client;

namespace Wtdl.Controller.SignalRHub
{
    public static class APPHubExtensions
    {
        /// <summary>
        /// 初始化连接
        /// </summary>
        /// <param name="hubConnection"></param>
        /// <param name="navigation"></param>
        /// <param name="accesstoken"></param>
        /// <returns></returns>
        public static HubConnection TryInitialize(this HubConnection hubConnection, string hubUrl, string accesstoken)
        {
            if (hubConnection == null)
            {
                hubConnection = new HubConnectionBuilder()
                                  .WithUrl(hubUrl, options =>
                                  {
                                      options.AccessTokenProvider = () => Task.FromResult(accesstoken);
                                  })
                                  .WithAutomaticReconnect()
                                  .Build();
            }
            return hubConnection;
        }
    }
}