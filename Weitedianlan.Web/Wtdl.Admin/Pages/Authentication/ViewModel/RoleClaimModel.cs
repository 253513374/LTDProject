namespace Wtdl.Admin.Pages.Authentication.ViewModel
{
    public class RoleClaimModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 权限类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 权限值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 权限描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 权限分组
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 是否拥有权限
        /// </summary>
        public bool Selected { get; set; }
    }
}