using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Wtdl.Admin.Pages.Authentication.ViewModel
{
    public class UpdatePassword
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 当前密码a
        /// </summary>
        [Required]
        public string CurrentPassword { get; set; }

        public ClaimsPrincipal User { get; set; }
    }
}