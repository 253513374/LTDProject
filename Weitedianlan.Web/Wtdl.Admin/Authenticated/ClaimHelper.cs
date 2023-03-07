using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;
using Wtdl.Admin.Authenticated.IdentityModel;
using Wtdl.Admin.Pages.Authentication.ViewModel;

namespace Wtdl.Admin.Authenticated
{
    public static class ClaimHelper
    {
        /// <summary>
        /// 返回所有由RolePermissions 类定义的权限
        /// </summary>
        /// <param name="allPermissions"></param>
        public static void GetAllPermissions(this List<RoleClaimModel> allPermissions)
        {
            //获取分组类型节点
            var modules = typeof(Permissions).GetNestedTypes();

            foreach (var module in modules)
            {
                var moduleName = string.Empty;
                var moduleDescription = string.Empty;

                if (module.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                    .FirstOrDefault() is DisplayNameAttribute displayNameAttribute)
                    moduleName = displayNameAttribute.DisplayName;

                if (module.GetCustomAttributes(typeof(DescriptionAttribute), true)
                    .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                    moduleDescription = descriptionAttribute.Description;

                //获取类型节点的所有字段信息
                var fields = module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                //循环获取字段或者属性值
                foreach (var fi in fields)
                {
                    var propertyValue = fi.GetValue(null);

                    //字段或者属性值不为空 就创建新的声明RoleClaim
                    if (propertyValue is not null)
                        allPermissions.Add(new RoleClaimModel { Value = propertyValue.ToString(), Type = CustomClaimTypes.Permission, Group = moduleName, Description = moduleDescription });
                }
            }
        }

        public static async Task<IdentityResult> AddPermissionClaim(this RoleManager<WtdlRole> roleManager, WtdlRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == CustomClaimTypes.Permission && a.Value == permission))
            {
                return await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
            }

            return IdentityResult.Failed();
        }
    }
}