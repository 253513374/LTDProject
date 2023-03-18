using System.ComponentModel.DataAnnotations;

namespace Wtdl.Model.Entity
{
    public class AccountModel
    {
        [Required(ErrorMessage = "请输入用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        public string UserPwd { get; set; }
    }
}