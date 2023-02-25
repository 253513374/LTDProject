using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Security.Claims;
using Wtdl.Admin.Authenticated.IdentityModel;
using Wtdl.Admin.Authenticated.Services;
using Wtdl.Admin.Pages.Authentication;
using Wtdl.Admin.Pages.Authentication.ViewModel;

namespace Wtdl.Admin.Authenticated
{
    public class AccountService
    {
        private readonly UserManager<WtdlUser> userManager;
        // private readonly RoleCl

        private readonly RoleManager<WtdlRole> roleManager;
        private readonly SignInManager<WtdlUser> signInManager;

        private readonly RoleClaimService roleClaimService;

        // private readonly CustomAuthenticationStateProvider authentication;
        private readonly IDbContextFactory<CustomIdentityDbContext> dbContextFactory;

        public AccountService(UserManager<WtdlUser> _userManager,
            RoleManager<WtdlRole> _roleManager,
            SignInManager<WtdlUser> _signInManager,
        IDbContextFactory<CustomIdentityDbContext> _dbContextFactory,
        RoleClaimService claimService)
        {
            roleClaimService = claimService;
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
        public async Task<IdentityResult> CreateUserAsync(WtdlUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);
            return result;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteUserAsync(WtdlUser uesr)
        {
            var result = await userManager.DeleteAsync(uesr);

            return result.Succeeded;
            // throw new NotImplementedException();
        }

        /// <summary>
        /// 返回所有用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<List<WtdlUser>>> GetAllUserAsync()
        {
            var user = await userManager.Users.ToListAsync();
            return BaseResponse<List<WtdlUser>>.Success(user);
        }

        /// <summary>
        /// 返回指定账号信息
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WtdlUser> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await userManager.GetUserAsync(claimsPrincipal);//etUserAsync(claimsPrincipal);
            //throw new NotImplementedException();
        }

        public async Task<WtdlUser> GetUserByIdAsync(string userid)
        {
            return await userManager.FindByIdAsync(userid);
        }

        /// <summary>
        /// 返回指定账号所属角色信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<UserRoleModel>> GetRolesByUserIdAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var userrole = await roleManager.Roles.ToListAsync();

            List<UserRoleModel> roleModels = new();
            for (int i = 0; i < userrole.Count(); i++)
            {
                if (await userManager.IsInRoleAsync(user, userrole[i].Name))
                {
                    roleModels.Add(new UserRoleModel()
                    {
                        RoleName = userrole[i].Name,
                        RoleDescription = userrole[i].Description,
                        Selected = true,
                    });
                    // userrole[i].IsSelected = true;
                }
                else
                {
                    roleModels.Add(new UserRoleModel()
                    {
                        RoleName = userrole[i].Name,
                        RoleDescription = userrole[i].Description,
                        Selected = false,
                    });
                }
                //await roleManager.Roles.ToListAsync()
            }

            return roleModels;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 更新指定账号所属角色信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IdentityResult> UpdateUserByRolesAsync(UpdateUserRoles request)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            var userrole = await roleManager.Roles.ToListAsync();

            for (int i = 0; i < userrole.Count(); i++)
            {
                if (await userManager.IsInRoleAsync(user, userrole[i].Name))
                {
                    await userManager.RemoveFromRoleAsync(user, userrole[i].Name);
                }
            }
            var uproles = request.UserRoles.Where(w => w.Selected).Select(s => s.RoleName);
            if (uproles is null && uproles.Count() == 0)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "1", Description = "未选择角色" });
            }

            //IEnumerable<UserRoleModel> enumerable = uproles.AsEnumerable();
            return await userManager.AddToRolesAsync(user, uproles);
            //throw new NotImplementedException();
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
        public async Task<BaseResponse<IList<Claim>>> GetRolePermissionAsync(string roleid)
        {
            ///根据角色ID 返回角色信息
            var role = await roleManager.FindByIdAsync(roleid);
            var roles = await roleManager.GetClaimsAsync(role);

            RolePermission rolePermission = new RolePermission();

            rolePermission.RoleId = role.Id;
            rolePermission.RoleName = role.Name;

            foreach (var item in roles)
            {
                rolePermission.RoleClaims.Add(new RoleClaimModel() { });
            }

            return BaseResponse<IList<Claim>>.Success(roles);
        }

        /// <summary>
        /// 登录信息验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SignInWResult> LoginUserAsync(LoginModel model)
        {
#if DEBUG
            try
            {
                //生成默认账号与默认角色
                var defuluser = await userManager.FindByEmailAsync("admin@wt.com");
                if (defuluser is null)
                {
                    var createuser = Activator.CreateInstance<WtdlUser>();
                    createuser.UserName = "admin";
                    createuser.Email = "admin@wt.com";
                    createuser.CreatedOn = DateTime.Now;
                    createuser.CreatedBy = "t";
                    createuser.IsActive = true;

                    await userManager.CreateAsync(createuser);
                    //设置默认密码
                    await userManager.AddPasswordAsync(createuser, "88888888");

                    var createrole = Activator.CreateInstance<WtdlRole>();
                    {
                        createrole.Name = BaseRole.Aministrator;
                        createrole.Description = "超级管理员角色，拥有系统最高权限，不可删除";
                        createrole.CreatedOn = DateTime.Now;
                        createrole.CreatedBy = "t";
                    };
                    await roleManager.CreateAsync(createrole);

                    //用户添加角色
                    await userManager.AddToRoleAsync(createuser, BaseRole.Aministrator);

                    List<RoleClaimModel> roleClaims = new();
                    roleClaims.GetAllPermissions();

                    var addrole = await roleManager.FindByNameAsync(BaseRole.Aministrator);
                    foreach (var item in roleClaims)
                    {
                        await roleManager.AddPermissionClaim(addrole, item.Value);
                    }
                    //  await roleManager.AddClaimAsync();
                }
            }
            catch (Exception e)
            {
                return SignInWResult.Failure(e.Message);
            }

#endif

            // using var dbContext = dbContextFactory.CreateDbContext();
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName || u.Email == model.UserName || u.PhoneNumber == model.UserName);

            //验证用户密码是否正确

            if (user == null)
            {
                return SignInWResult.Failure("账号不存在"); // Result.Failure("Invalid username or password.");
            }
            var result = await userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return SignInWResult.Failure("密码不正确");
            }

            if (!user.IsActive.Value)
            {
                return SignInWResult.Failure("账号没有激活");
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
                new(ClaimTypes.Surname, user.LastName??string.Empty),
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

        public async Task<IdentityResult> UpdateAsync(WtdlUser user)
        {
            var result = await userManager.UpdateAsync(user);
            return result;
            // throw new NotImplementedException();
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="passwordModel"></param>
        /// <returns></returns>
        public async Task<IdentityResult> ChangePasswordAsync(UpdatePassword passwordModel)
        {
            var user = await userManager.GetUserAsync(passwordModel.User);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "用户不存在" });
            }

            //校验密码
            var result = await userManager.CheckPasswordAsync(user, passwordModel.ConfirmPassword);

            if (result)
            {
                return await userManager.ChangePasswordAsync(user, passwordModel.CurrentPassword, passwordModel.Password);
            }

            return IdentityResult.Failed(new IdentityError { Description = "密码不正确" });
            // return result;
        }

        public async Task<IdentityResult> UpdateUserStatusAsync(bool active, string id)
        {
            //根据ID 查找用户信息
            var user = await userManager.FindByIdAsync(id);
            user.IsActive = active;
            ///更新用户
            return await userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdateRolePermission(RolePermission model)
        {
            throw new NotImplementedException();
        }
    }
}