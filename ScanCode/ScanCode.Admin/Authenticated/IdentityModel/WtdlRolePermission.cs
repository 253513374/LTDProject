using Microsoft.AspNetCore.Identity;

namespace ScanCode.Web.Admin.Authenticated.IdentityModel
{
    public class WtdlRolePermission : IdentityRoleClaim<string>
    {
        /// <summary>
        /// 权限描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 权限分组名称
        /// </summary>
        public string? Group { get; set; }

        /// <summary>
        /// 权限创建人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 权限创建时间
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// 权限最后修改人
        /// </summary>
        public string? LastModifiedBy { get; set; }

        /// <summary>
        /// 权限最后修改时间
        /// </summary>
        public DateTime? LastModifiedOn { get; set; }

        /// <summary>
        /// 权限所属角色
        /// </summary>
        public virtual WtdlRole Role { get; set; }

        public WtdlRolePermission() : base()
        {
        }

        public WtdlRolePermission(string roleClaimDescription = null, string roleClaimGroup = null) : base()
        {
            Description = roleClaimDescription;
            Group = roleClaimGroup;
        }
    }
}