namespace Wtdl.Mvc.Models
{
    public class RedPacketResult
    {
        /// <summary>
        /// 领取红包状态
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 领取红包消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 领取红包的金额
        /// </summary>
        public string CashAmount { get; set; }
    }
}