namespace Wtdl.Model.ResponseModel
{
    public class LoginResult
    {
        /// <summary>
        /// 授权token
        /// </summary>
        public string Token { set; get; }

        /// <summary>
        /// 订阅
        /// </summary>
        public string IsSubscribe { set; get; }
    }
}