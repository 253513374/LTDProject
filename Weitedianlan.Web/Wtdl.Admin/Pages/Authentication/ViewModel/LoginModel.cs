namespace Wtdl.Admin.Pages.Authentication.ViewModel
{
    /// <summary>
    /// 登录表单
    /// </summary>
    public class LoginModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}