namespace Wtdl.Admin.Pages.Authentication.ViewModel
{
    public class UserRoleModel
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string RoleDescription { get; set; }

        /// <summary>
        /// 当前用户是否拥有这个角色
        /// </summary>
        public bool Selected { get; set; }
    }
}