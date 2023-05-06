using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScanCode.Web.Admin.Authenticated.IdentityModel;
using ScanCode.Web.Admin.Pages.Authentication.ViewModel;

namespace ScanCode.Web.Admin.Authenticated.Services
{
    public class RoleClaimService : IService
    {
        private readonly UserManager<WtdlUser> _userManager;
        private readonly RoleManager<WtdlRole> _roleManager;
        private readonly IDbContextFactory<CustomIdentityDbContext> _dbContextFactory;

        private CustomAuthenticationService _authenticationService;

        public RoleClaimService(CustomAuthenticationService service, UserManager<WtdlUser> userManager, RoleManager<WtdlRole> roleManager, IDbContextFactory<CustomIdentityDbContext> dbContextFactory)
        {
            _authenticationService = service;
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContextFactory = dbContextFactory;
        }

        public Task AddRoleClaimAsync()
        {
            return Task.CompletedTask;
        }

        public async Task<int> DeleteRoleClaimAsync(int roleclaimid)
        {
            //删除角色声明
            CustomIdentityDbContext db = _dbContextFactory.CreateDbContext();
            var roleClaim = await db.RoleClaims.FirstOrDefaultAsync(x => x.Id == roleclaimid);
            db.RoleClaims.Remove(roleClaim);
            return await db.SaveChangesAsync();

            //  _roleManager.AddPermissionClaim();

            // return Task.CompletedTask;
        }

        public async Task<BaseResponse<List<RoleClaimModel>>> GetRoleClaimAsync(string roleid)
        {
            try
            {
                List<RoleClaimModel> roleClaims = new();
                roleClaims.GetAllPermissions();

                using var db = _dbContextFactory.CreateDbContext();

                var userroleClaims = await db.RoleClaims.Include(i => i.Role).Where(x => x.RoleId == roleid).ToListAsync();

                ///循环判断用户是否拥有权限，用户的以及拥有的权限设置状态Selected 为true
                foreach (var roleClaim in roleClaims)
                {
                    var dbclaim = userroleClaims.SingleOrDefault(x => x.ClaimValue == roleClaim.Value && x.ClaimType == roleClaim.Type);
                    if (dbclaim is not null)
                    {
                        //拥有权限
                        roleClaim.Selected = true;
                        roleClaim.Description = dbclaim.Description;
                    }
                }

                return BaseResponse<List<RoleClaimModel>>.Success(roleClaims);
            }
            catch (Exception e)
            {
                return BaseResponse<List<RoleClaimModel>>.Fail(e.Message);
            }
        }

        public async Task<BaseResponse<string>> UpdateRoleClaimAsync(RolePermission roleclaims)
        {
            //  roleclaims = roleclaims.
            //获取当前角色的声明
            var role = await _roleManager.FindByIdAsync(roleclaims.RoleId);

            if (role == null) return BaseResponse<string>.Fail("当前角色不存在，无法更新");
            if (role.Name == BaseRole.Aministrator)
            {
                return BaseResponse<string>.Fail($"不允许修改角色{BaseRole.Aministrator}的权限");
            }

            //  获取当前角色的所有声明
            var claims = await _roleManager.GetClaimsAsync(role);
            //删除现有声明
            if (claims is not null)
            {
                foreach (var claim in claims)
                {
                    //当前没有被选择的被视为删除
                    var resultAny = roleclaims.RoleClaims.Any(a => a.Type == claim.Type && a.Value == claim.Value && a.RoleId == role.Id);
                    if (resultAny)
                    {
                        await _roleManager.RemoveClaimAsync(role, claim);
                    }
                }
            }

            //重新添加
            foreach (var roleclaim in roleclaims.RoleClaims)
            {
                if (roleclaim.Selected)
                {
                    //添加权限
                    var identityresult = await _roleManager.AddPermissionClaim(role, roleclaim.Value);

                    if (identityresult.Succeeded)
                    {
                    }
                }
            }

            var result = await UpdateRoleclaim(roleclaims);
            if (result.IsSuccess)
            {
                return BaseResponse<string>.Success(result.Message);
            }
            return BaseResponse<string>.Fail($"UpdateRoleclaim 更新失败{result.Message}");
        }

        private async Task<BaseResponse<string>> UpdateRoleclaim(RolePermission roleclaims)
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                var user = await _userManager.GetUserAsync(_authenticationService.CurrentUser);
                var claims = roleclaims.RoleClaims;
                var roleid = roleclaims.RoleId;

                foreach (var claim in claims)
                {
                    var result = context.RoleClaims.SingleOrDefault(a => a.ClaimType == claim.Type && a.ClaimValue == claim.Value && a.RoleId == roleid);

                    if (result is not null)
                    {
                        result.Description = claim.Description;
                        result.LastModifiedOn = DateTime.Now;
                        result.LastModifiedBy = user.UserName;
                        result.Group = claim.Group;
                        context.RoleClaims.Update(result);
                        context.SaveChanges();
                    }
                }
                return BaseResponse<string>.Success("Success");
            }
            catch (Exception e)
            {
                return BaseResponse<string>.Fail(e.Message);
            }
        }
    }
}