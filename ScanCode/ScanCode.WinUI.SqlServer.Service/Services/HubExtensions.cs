using Microsoft.AspNetCore.SignalR.Client;
using ScanCode.Share;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace ScanCode.WinUI.Service
{
    public static class HubExtensions
    {
        private static string HubUrl;// = ConfigurationManager.ConnectionStrings["HubUrl"].ConnectionString;
        private static string LoginUrl;// = ConfigurationManager.ConnectionStrings["LoginUrl"].ConnectionString;

        private static string _username;
        private static string _password;
        private static User User { set; get; }

        private static WtdlSqlService WtdlSqlService { set; get; }

        public static HubConnection TryInitialize(this HubConnection hubConnection,
            string huburl = "",
            string loginurl = "",
            string username = null,
            string password = null)
        {
            if (!string.IsNullOrWhiteSpace(huburl))
            {
                HubUrl = huburl;
            }
            if (!string.IsNullOrWhiteSpace(loginurl))
            {
                LoginUrl = loginurl;
            }

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
                    //.WithAutomaticReconnect()
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
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
            catch (Exception e)
            {
                Console.WriteLine(e);

                return "";
                // throw;
            }
        }

        /// <summary>
        /// 初始化 WtdlSqlService
        /// </summary>
        /// <param name="wtdlSqlService"></param>
        /// <returns></returns>
        //public static WtdlSqlService TryGetSqlService(this WtdlSqlService wtdlSqlService)
        //{
        //    if (WtdlSqlService is null)
        //    {
        //        WtdlSqlService = new WtdlSqlService();
        //    }

        //    return WtdlSqlService;
        //}
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}