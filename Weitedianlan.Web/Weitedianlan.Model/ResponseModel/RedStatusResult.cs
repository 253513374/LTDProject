namespace   Wtdl.Controller.Models.ResponseModel
{
    public class RedStatusResult
    {
        /// <summary>
        /// 领取红包状态
        /// </summary>
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        /// <summary>
        /// QRCODE:第一次领现金红包
        /// CAPTCHA：第二次领取现金红包
        /// NOT: 禁止领取现金红包
        /// </summary>
        public string StuteCode { get; set; }
    }
}