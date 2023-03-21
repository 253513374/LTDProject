namespace Wtdl.Controller.Models.ResponseModel
{
    public class RedStatusResult
    {
        /// <summary>
        /// 领取红包状态
        /// </summary>
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        /// <summary>
        /// QRCODE:需要二维码标签 领取红包
        /// CAPTCHA：需要验证码 领取红包
        /// NOTSTARTED :活动还未开始
        /// NOTACTIVATED ：活动还未激活
        /// NOTINSTOCK : 数据还未出库
        /// NOTOUTTIME : 数据还未到出库时间
        /// MAXIMUMLIMIT ：标签达到领取红包数量上限
        /// MAXUSERLIMIT : 用户领取红包数量达到上限
        /// NOTFOLLOWED ：还未关注微信公众号
        /// NOTIMPORTDATA : 红包数据还没有导入
        /// INVALIDCAPTCHA : 无效的验证码
        /// CAPTCHAUSED :验证已经使用过
        /// NOT: 禁止领取现金红包
        /// </summary>
        public string StuteCode { get; set; }

        public static RedStatusResult Fail(string message)
        {
            return new RedStatusResult
            {
                IsSuccess = false,
                Message = message
            };
        }

        public static RedStatusResult FailNotStarted(string message)
        {
            return new RedStatusResult
            {
                IsSuccess = false,
                Message = message,
                StuteCode = "NOTSTARTED"
            };
        }

        public static RedStatusResult FailNotFollowed(string message)
        {
            return new RedStatusResult
            {
                IsSuccess = false,
                Message = message,
                StuteCode = "NOTFOLLOWED"
            };
        }

        public static RedStatusResult FailNotActivated(string message)
        {
            return new RedStatusResult
            {
                IsSuccess = false,
                Message = message,
                StuteCode = "NOTACTIVATED"
            };
        }

        public static RedStatusResult FailMaximumLimit(string message)
        {
            return new RedStatusResult
            {
                IsSuccess = false,
                Message = message,
                StuteCode = "MAXIMUMLIMIT"
            };
        }

        public static RedStatusResult FailMaxUserLimit(string message)
        {
            return new RedStatusResult
            {
                IsSuccess = false,
                Message = message,
                StuteCode = "MAXUSERLIMIT"
            };
        }

        public static RedStatusResult FailNotImportData(string message)
        {
            return new RedStatusResult
            {
                IsSuccess = false,
                Message = message,
                StuteCode = "NOTIMPORTDATA"
            };
        }

        public static RedStatusResult FailNotOutTime(string message)
        {
            return new RedStatusResult
            {
                IsSuccess = false,
                Message = message,
                StuteCode = "NOTOUTTIME"
            };
        }

        public static RedStatusResult FailInvalidCaptcha(string message)
        {
            return new RedStatusResult
            {
                IsSuccess = false,
                Message = message,
                StuteCode = "INVALIDCAPTCHA"
            };
        }

        public static RedStatusResult FailCaptchaUsed(string message)
        {
            return new RedStatusResult
            {
                IsSuccess = false,
                Message = message,
                StuteCode = "CAPTCHAUSED"
            };
        }

        public static RedStatusResult FailNot(string message)
        {
            return new RedStatusResult
            {
                IsSuccess = false,
                Message = message,
                StuteCode = "NOT"
            };
        }

        public static RedStatusResult Success(string message)
        {
            return new RedStatusResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        public static RedStatusResult SuccessQrCode(string message)
        {
            return new RedStatusResult
            {
                IsSuccess = true,
                Message = message,
                StuteCode = "QRCODE"
            };
        }

        public static RedStatusResult SuccessCaptcha(string message)
        {
            return new RedStatusResult
            {
                IsSuccess = true,
                Message = message,
                StuteCode = "CAPTCHA"
            };
        }
    }
}