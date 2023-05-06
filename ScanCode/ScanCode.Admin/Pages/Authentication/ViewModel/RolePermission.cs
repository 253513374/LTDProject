namespace ScanCode.Web.Admin.Pages.Authentication.ViewModel
{
    /// <summary>
    /// 角色用有的权限
    /// </summary>
    public class RolePermission
    {
        public RolePermission()
        {
            RoleClaims = new();
        }

        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色权限
        /// </summary>
        public List<RoleClaimModel> RoleClaims { get; set; }
    }
}