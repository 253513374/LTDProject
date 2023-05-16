namespace ScanCode.Controller.Models
{
    /// <summary>
    /// 用户红包状态
    /// </summary>
    public enum RedPacketStatus
    {
        /// <summary>
        /// 明确表示红包未被领取
        /// </summary>
        PacketNotReceived = 0,

        /// <summary>
        /// 明确表示红包已被领取 1次
        /// </summary>
        PacketAlreadyReceived = 1,

        /// <summary>
        /// 明确表示红包已经被领完。
        /// </summary>
        StockExhausted = 2,

        /// <summary>
        /// 明确红包记录无效或者异常。
        /// </summary>
        Invalid = 3,

        /// <summary>
        /// 明确表示用户已经达到了每日领取红包的上限。
        /// </summary>
        UserDailyLimitReached = 10,
    }

    /// <summary>
    /// 状态信息说明
    /// </summary>
    public static class RedPacketMessages
    {
        /// <summary>
        /// 已经领过1次，还可以再领1次
        /// </summary>
        public const string AlreadyReceived = "红包已经领过啦!，请查看威特五金公众号红包信息。";

        public const string OutOfStock = "当前标签序号红包已经领取完毕";
        public const string UserLimitReached = "今日已经领取10个红包上限，请明日再参与扫码领现金红包活动";
        public const string Success = "可以参加扫码得现金活动";
    }
}