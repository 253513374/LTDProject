using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using MudBlazor;
using Weitedianlan.Model.Entity;
using Wtdl.Admin.Authenticated;
using Wtdl.Admin.Authenticated.IdentityModel;
using Wtdl.Admin.Data;
using Wtdl.Admin.Pages.Authentication.ViewModel;
using Wtdl.Repository;

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

        private ILogger<APPHub> _logger;

        //构造函数
        public APPHub(IMemoryCache cache, ILogger<APPHub> logger, WLabelStorageRepository repository)
        {
            _storageRepository = repository;
            _logger = logger;
            _cache = cache;
        }

        #region 发送信息

        public async Task SendMessage(string user, string message)
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
                newValue = Interlocked.Increment(ref counter);
                _cache.Set(CacheKeys.DayCacheKey, newValue, options);
            }

            //_logger.LogInformation("缓存更新完成");

            Context.User.Claims.ToList().ForEach(x =>
            {
                _logger.LogInformation($"{x.Type} : {x.Value}");
            });
            ///调用ReceiveMessage 更新所有客户端
            await Clients.All.SendAsync("OnReportFormsNever", newValue);
        }

        #endregion 发送信息

        //private readonly AccountService _accountService;

        //public APPHub(AccountService service)
        //{
        //    _accountService = service;
        //}

        //public async Task<string> Login(string username, string password)
        //{
        //    var loginmodel = new LoginModel()
        //    {
        //        UserName = username,
        //        Password = password
        //    };
        //    var loginresult = await _accountService.LoginUserAsync(loginmodel);

        //    if (loginresult.Succeeded)
        //    {
        //        // 将用户信息存储在 Context.User 属性中
        //        // = loginresult.Claims;

        //        var claimsprincipal = new ClaimsPrincipal(new ClaimsIdentity(loginresult.Claims, "CustomAuthentication"));

        //        // Context.
        //        //  Context. = loginresult.UserIdentifier;
        //    }
        //}
    }
}