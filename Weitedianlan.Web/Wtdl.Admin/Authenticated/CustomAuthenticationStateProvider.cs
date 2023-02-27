using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Wtdl.Admin.Authenticated.IdentityModel;

namespace Wtdl.Admin.Authenticated
{
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
}