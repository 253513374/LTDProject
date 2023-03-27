using Microsoft.AspNetCore.Identity;

namespace Wtdl.Admin.Authenticated.IdentityModel
{
    public class WtdlUser : IdentityUser<string>
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// 第二个名称
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// 谁创建的
        /// </summary>
        public string? CreatedBy { get; set; }

        public string? ProfilePictureDataUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string? LastModifiedBy { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModifiedOn { get; set; }

        /// <summary>
        /// 是否软删除
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletedOn { get; set; }

        /// <summary>
        /// 账号是否激活
        /// </summary>
        public bool? IsActive { get; set; }
    }
}