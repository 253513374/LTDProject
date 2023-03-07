using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Wtdl.Admin.SignalRHub
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
        public static HubConnection TryInitialize(this HubConnection hubConnection, NavigationManager navigation, string accesstoken)
        {
            if (hubConnection == null)
            {
                hubConnection = new HubConnectionBuilder()
                                  .WithUrl(navigation.ToAbsoluteUri("/APPHub"), options =>
                                  {
                                      options.AccessTokenProvider = () => Task.FromResult(accesstoken);
                                  })
                                  .Build();
            }
            return hubConnection;
        }
    }
}