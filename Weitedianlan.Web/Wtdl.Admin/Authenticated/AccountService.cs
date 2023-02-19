using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Security.Claims;
using Wtdl.Admin.Authenticated.IdentityModel;
using Wtdl.Admin.Pages.Authentication;
using Wtdl.Admin.Pages.Authentication.ViewModel;

namespace Wtdl.Admin.Authenticated
{
    public class AccountService
    {
        private readonly UserManager<WtdlUser> userManager;
        private readonly RoleManager<WtdlRole> roleManager;
        private readonly SignInManager<WtdlUser> signInManager;

        // private readonly CustomAuthenticationStateProvider authentication;
        private readonly IDbContextFactory<CustomIdentityDbContext> dbContextFactory;

        public AccountService(UserManager<WtdlUser> _userManager,
            RoleManager<WtdlRole> _roleManager,
            SignInManager<WtdlUser> _signInManager,
        IDbContextFactory<CustomIdentityDbContext> _dbContextFactory)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            dbContextFactory = _dbContextFactory;
        }

        /// <summary>
        /// 新建账户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> CreateUserAsync(WtdlUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        /// <summary>
        /// 返回所有用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<WtdlUser>> GetAllUserAsync()
        {
            var user = await userManager.Users.ToListAsync();
            return user;
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<List<WtdlRole>>> GetAllRolesAsync()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return BaseResponse<List<WtdlRole>>.Success(roles);
        }

        /// <summary>
        /// 返回角色所属权限
        /// </summary>
        /// <returns></returns>
        public async Task<List<Claim>> GetRolePermissionAsync(WtdlRole role)
        {
            var roles = await roleManager.GetClaimsAsync(role);
            return roles.ToList();
        }

        public async Task<SignInWResult> LoginUserAsync(LoginModel model)
        {
            // using var dbContext = dbContextFactory.CreateDbContext();
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName || u.Email == model.UserName || u.PhoneNumber == model.UserName);

            //验证用户密码是否正确

            if (user == null)
            {
                return SignInWResult.Failure("账号不存在"); // Result.Failure("Invalid username or password.");
            }
            var result = await userManager.CheckPasswordAsync(user, model.Password);

            if (result)
            {
                return SignInWResult.Failure("密码不正确");
            }

            var roleclaims = await GetClaimsAsync(user);

            //   await authentication.UpdateAuthenticationStateAsync(user, roleclaims.ToList());

            return SignInWResult.Success(roleclaims.ToList());
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(WtdlUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            var permissionClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
                var thisRole = await roleManager.FindByNameAsync(role);
                var allPermissionsForThisRoles = await roleManager.GetClaimsAsync(thisRole);
                permissionClaims.AddRange(allPermissionsForThisRoles);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
            };
            if (userClaims is not null)
            {
                claims = claims.Union(userClaims).ToList();
            }

            if (userClaims is not null)
            {
                claims = claims.Union(roleClaims).ToList();
            }

            if (permissionClaims is not null)
            {
                claims = claims.Union(permissionClaims).ToList();
            }

            return claims;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteRoleAsync(WtdlRole role)
        {
            var result = await roleManager.DeleteAsync(role);

            return result.Succeeded;
            // throw new NotImplementedException();
        }

        ///更新角色
        public async Task<bool> UpdateRoleAsync(WtdlRole role)
        {
            try
            {
                var result = await roleManager.UpdateAsync(role);
                return result.Succeeded;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        //添加角色
        public async Task<bool> AddRoleAsync(WtdlRole role)
        {
            var result = await roleManager.CreateAsync(role);
            return result.Succeeded;
        }
    }
}