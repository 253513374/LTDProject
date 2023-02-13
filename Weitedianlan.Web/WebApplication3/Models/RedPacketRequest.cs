namespace Wtdl.Mvc.Models
{
    public class RedPacketRequest
    {
        /// <summary>
        /// 微信用户openid
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 二维码标签序号
        /// </summary>
        public string QRCode { get; set; }

        /// <summary>
        /// 标签序号对应的4位数的验证码
        /// </summary>
        public string Captcha { get; set; }
    }
}