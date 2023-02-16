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

        public int Code { get; set; }
        public string TotalAmount { get; set; }
    }
}