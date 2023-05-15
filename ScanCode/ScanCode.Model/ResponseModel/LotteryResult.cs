using ScanCode.Model.Entity;
using System;

namespace ScanCode.Model.ResponseModel
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
            string prizeDescription, string prizenumber, string message = "")
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

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static LotteryResult ServerError(Exception e)
        {
            return new LotteryResult { IsSuccess = false, Message = $"服务器错误：{e.Message}" };
        }

        /// <summary>
        /// 校验失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static LotteryResult VerificationFailed(string message)
        {
            return new LotteryResult { IsSuccess = false, Message = message };
        }

        /// <summary>
        /// 奖品已经抽完,请重新抽奖
        /// </summary>
        /// <returns></returns>
        public static LotteryResult NoPrizeAvailable()
        {
            return new LotteryResult { IsSuccess = false, Message = "奖品已经抽完,请重新抽奖" };
        }

        /// <summary>
        /// 中奖
        /// </summary>
        /// <param name="prize"></param>
        /// <returns></returns>
        public static LotteryResult PrizeWon(ActivityPrize prize)
        {
            return new LotteryResult
            {
                IsSuccess = true,
                Message = $"恭喜你，中大奖啦",
                PrizeType = prize.Type.ToString(),
                PrizeName = prize.Name,
                PrizeDescription = prize.Description,
                PrizeImage = prize.ImageUrl,
                PrizeNumber = prize.PrizeNumber
            };
        }

        /// <summary>
        /// 没有中奖
        /// </summary>
        /// <param name="prize"></param>
        /// <returns></returns>
        public static LotteryResult PrizeNotWon(ActivityPrize prize)
        {
            return new LotteryResult
            {
                IsSuccess = false,
                Message = $"很遗憾！没有中奖。",
                PrizeType = prize.Type.ToString(),
            };
        }
    }
}