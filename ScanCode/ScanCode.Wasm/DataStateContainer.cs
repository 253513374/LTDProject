using ScanCode.Model.ResponseModel;

namespace ScanCode.Web.Wasm
{
    public class DataStateContainer
    {
        /// <summary>
        /// 标签二维码
        /// </summary>
        public string? QrCode { get; set; }

        /// <summary>
        /// 微信用户openid
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 标签状态
        /// </summary>
        public RedStatusResult? UserStatus { get; set; }

        /// <summary>
        /// 活动信息
        /// </summary>
        public ActivityResult? Activity { get; set; }

        /// <summary>
        /// 访问信息
        /// </summary>
        public AntiFakeResult? AntiFakeResult { get; set; }

        /// <summary>
        /// 溯源信息
        /// </summary>
        public TraceabilityResult? TraceabilityResult { get; set; }

        public LotteryResult LotteryResult { get; set; }

        /// <summary>
        /// 订阅方法
        /// </summary>

        public event Action? OnChanged;

        public Task SetCode(string qrcode, string openid)
        {
            QrCode = qrcode;
            OpenId = openid;
            NotifyCodeChanged();
            return Task.CompletedTask;
        }

        public Task SetActivity(ActivityResult activity)
        {
            Activity = activity;
            NotifyCodeChanged();
            return Task.CompletedTask;
        }

        private void NotifyCodeChanged() => OnChanged?.Invoke();

        public bool IsVerify(string prizenumber)
        {
            if (string.IsNullOrEmpty(QrCode) || string.IsNullOrEmpty(OpenId) || string.IsNullOrEmpty(prizenumber))
            {
                return false;
            }
            return true;
        }
    }
}