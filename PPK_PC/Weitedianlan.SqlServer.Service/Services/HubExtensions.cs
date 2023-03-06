using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wtdl.Share;

namespace Weitedianlan.SqlServer.Service
{
    public static class HubExtensions
    {
        private static string HubUrl = ConfigurationManager.ConnectionStrings["HubUrl"].ConnectionString;
        private static string LoginUrl = ConfigurationManager.ConnectionStrings["LoginUrl"].ConnectionString;

        private static string _username;
        private static string _password;
        private static User User { set; get; }

        private static WtdlSqlService WtdlSqlService { set; get; }

        public static HubConnection TryInitialize(this HubConnection hubConnection,
            string username = null,
            string password = null)
        {
            if (!string.IsNullOrEmpty(username))
            {
                _username = username;
            }
            if (!string.IsNullOrEmpty(username))
            {
                _password = password;
            }

            if (hubConnection == null)
            {
                hubConnection = new HubConnectionBuilder()
                    .WithUrl(HubUrl,
                        options => { options.AccessTokenProvider = async () => await LoginAsync(_username, _password); })
                    .WithAutomaticReconnect()
                    .Build();
            }

            return hubConnection;
        }

        public static User TryGetUser(this HubConnection hubConnection)
        {
            return User;
        }

        //public static HubConnection TryInitialize(this HubConnection hubConnection, NavigationManager navigationManager)
        //{
        //    if (hubConnection == null)
        //    {
        //        hubConnection = new HubConnectionBuilder()
        //                          .WithUrl(navigationManager.ToAbsoluteUri(ApplicationConstants.SignalR.HubUrl))
        //                          .Build();
        //    }
        //    return hubConnection;
        //}

        private static async Task<string> LoginAsync(string username, string password)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //var content = new FormUrlEncodedContent(new Dictionary<string, string>
                //{
                //    { "username", username },
                //    { "password", password }
                //});
                // 构造请求内容
                //var contents = new FormUrlEncodedContent(new[]
                //{
                //    new KeyValuePair<string, string>("username", username),
                //    new KeyValuePair<string, string>("password", password)
                //});
                HttpContent content = new StringContent(JsonSerializer.Serialize(new LoginModel { Username = username, Password = password }));

                var url = $"{LoginUrl}?username={username}&password={password}";

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                //var responseContent = await response.Content.ReadAsStringAsync();

                var result = await response.Content.ReadAsStringAsync();

                var loginResult = JsonSerializer.Deserialize<LoginResult>(result); //await JsonSerializer.DeserializeAsync<LoginResult>(result);

                if (loginResult.Succeeded)
                {
                    User = new User();
                    User.UserName = loginResult.Username;
                    User.UserID = loginResult.UserId;
                    return loginResult.Token;
                }

                return loginResult.Token;
            }
        }

        /// <summary>
        /// 初始化 WtdlSqlService
        /// </summary>
        /// <param name="wtdlSqlService"></param>
        /// <returns></returns>
        public static WtdlSqlService TryGetSqlService(this WtdlSqlService wtdlSqlService)
        {
            if (WtdlSqlService is null)
            {
                WtdlSqlService = new WtdlSqlService();
            }

            return WtdlSqlService;
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}