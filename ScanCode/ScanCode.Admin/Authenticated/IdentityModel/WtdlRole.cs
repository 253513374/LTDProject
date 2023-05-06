using Microsoft.AspNetCore.Identity;

namespace ScanCode.Web.Admin.Authenticated.IdentityModel
{
    public class WtdlRole : IdentityRole<string>
    {
        /// <summary>
        /// 角色描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string? LastModifiedBy { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModifiedOn { get; set; }

        /// <summary>
        /// 角色权限列表
        /// </summary>
        public virtual ICollection<WtdlRolePermission> RoleClaims { get; set; }

        public WtdlRole() : base()
        {
            Id = Guid.NewGuid().ToString();
            CreatedOn = DateTime.UtcNow;
            RoleClaims = new HashSet<WtdlRolePermission>();
        }

        public WtdlRole(string roleName, string roleDescription = null) : base(roleName)
        {
            Id = Guid.NewGuid().ToString();
            CreatedOn = DateTime.UtcNow;
            RoleClaims = new HashSet<WtdlRolePermission>();
            Description = roleDescription;
        }
    }
}