namespace Wtdl.Mvc.Models
{
    /// <summary>
    /// 抽奖结果
    /// </summary>
    public class LotteryViewModel
    {
        /// <summary>
        /// 抽奖状态
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 抽奖信息
        /// </summary>
        public string Msg { get; set; }

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
    }
}