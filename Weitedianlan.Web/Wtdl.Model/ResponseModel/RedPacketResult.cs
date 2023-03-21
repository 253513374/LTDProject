namespace Wtdl.Model.ResponseModel
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

        public static RedPacketResult Fail(string message)
        {
            return new RedPacketResult
            {
                IsSuccess = false,
                Message = message
            };
        }

        public static RedPacketResult Success(string totalamount)
        {
            return new RedPacketResult
            {
                IsSuccess = true,
                TotalAmount = totalamount
            };
        }
    }
}