using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Wtdl.Admin.Authenticated.IdentityModel;

namespace Wtdl.Admin.Authenticated
{
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        // private readonly IHttpContextAccessor httpContextAccessor;
        // private readonly IDbContextFactory<CustomIdentityDbContext> _dbContextFactory;

        // private readonly UserManager<WtdlUser> _userManager;
        private readonly CustomAuthenticationService service;

        // private readonly SignInManager<WtdlUser> _signInManager;

        private AuthenticationState authenticationState;

        public CustomAuthenticationStateProvider(CustomAuthenticationService authentication
           //  IDbContextFactory<CustomIdentityDbContext> dbContext,
           // SignInManager<WtdlUser> signInManager
           )
        {
            // this.httpContextAccessor = httpContextAccessor;
            // this._dbContextFactory = dbContext;
            service = authentication;
            //  _signInManager = signInManager;

            authenticationState = new AuthenticationState(service.CurrentUser);
            service.UserChanged += (newUser) =>
            {
                NotifyAuthenticationStateChanged(
                    Task.FromResult(new AuthenticationState(newUser)));
            };
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
            Task.FromResult(authenticationState);
    }

    /// <summary>
    /// 登录结果
    /// </summary>
    public class SignInWResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string ErrorDescription { get; set; }

        public List<Claim> Claims { get; set; }

        public SignInWResult()
        {
            Claims = new List<Claim>();
        }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        /// <returns></returns>
        public static SignInWResult Success(List<Claim> claims = null)
        {
            return new SignInWResult()
            {
                Claims = claims,
                Succeeded = true,
            };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="errorDescription">错误描述</param>
        /// <returns></returns>
        public static SignInWResult Failure(string error, string errorDescription = null)
        {
            return new SignInWResult { Error = error, ErrorDescription = errorDescription };
        }
    }
}