using System.Security.Claims;

namespace ScanCode.Web.Admin.Authenticated
{
    public class CustomAuthenticationService
    {
        public event Action<ClaimsPrincipal>? UserChanged;

        private ClaimsPrincipal? currentUser;

        public ClaimsPrincipal CurrentUser
        {
            get { return currentUser ?? new(); }
            set
            {
                currentUser = value;

                if (UserChanged is not null)
                {
                    UserChanged(currentUser);
                }
            }
        }

        public Task LogOut()
        {
            CurrentUser = new ClaimsPrincipal(new ClaimsIdentity());
            return Task.CompletedTask;
        }

        public Task<bool> LogIn(List<Claim> claims)
        {
            CurrentUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "CustomAuthentication"));
            return Task.FromResult(true);
        }
    }
}