namespace ScanCode.Web.Api.Controllers
{
    /// <summary>
    /// 生成token的表单对象
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
    }
}