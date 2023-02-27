using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Wtdl.Admin.SignalRHub
{
    public static class HubExtensions
    {
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