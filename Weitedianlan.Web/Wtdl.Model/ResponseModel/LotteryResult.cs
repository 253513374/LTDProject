namespace Wtdl.Model.ResponseModel
{
    /// <summary>
    /// 抽奖结果
    /// </summary>
    public class LotteryResult : TResult
    {
        /// <summary>
        /// 抽奖状态
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 抽奖信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 奖品名称
        /// </summary>
        public string PrizeName { get; set; }

        /// <summary>
        /// 奖品类型
        /// </summary>
        public string PrizeType { get; set; }

        /// <summary>
        /// 奖品图片
        /// </summary>
        public string PrizeImage { get; set; }

        /// <summary>
        /// 奖品描述
        /// </summary>
        public string PrizeDescription { get; set; }

        /// <summary>
        /// 奖品编号
        /// </summary>
        public string PrizeNumber { get; set; }

        public static LotteryResult Fail(string message)
        {
            return new LotteryResult
            {
                IsSuccess = false,
                Message = message
            };
        }

        public static LotteryResult Success(string prizeName, string prizeType, string prizeImage,
            string prizeDescription,string prizenumber, string message = "")
        {
            return new LotteryResult
            {
                IsSuccess = true,
                Message = message,
                PrizeName = prizeName,
                PrizeType = prizeType,
                PrizeImage = prizeImage,
                PrizeDescription = prizeDescription,
                PrizeNumber = prizenumber
                
            };
        }
    }
}