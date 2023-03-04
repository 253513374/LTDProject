using Senparc.Weixin.TenPay.V3;
using Weitedianlan.Model.Entity;
using Weitedianlan.Model.Enum;

namespace Wtdl.Mvc.Models
{
    /// <summary>
    /// 户抽奖信息
    /// <summary>
    public class UserItemsRecordResult
    {
        /// <summary>
        /// 是否请求成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 信息说明
        /// </summary>
        public string Message { get; set; }

        ///<summary>
        /// 用户的抽奖次数
        /// </summary>
        public int LotteryCount { get; set; }

        /// <summary>
        /// 用户的中奖次数
        /// </summary>
        public int LotteryWinCount { get; set; }

        /// <summary>
        /// 红包数量（个）
        /// </summary>
        public int RedpacktCount { get; set; }

        /// <summary>
        /// 红包总金额
        /// </summary>
        public int RedpacketTotalAmount { get; set; }

        /// <summary>
        /// 用户的中奖记录
        /// </summary>
        public IEnumerable<LotteryRecord> LotteryInfos { get; set; }

        /// <summary>
        /// 用户的红包记录
        /// </summary>
        public IEnumerable<RedPacketRecord> RedPacketInfos { get; set; }

        public static UserItemsRecordResult SuccessResult(
            IEnumerable<LotteryRecord> lotteryInfos, IEnumerable<RedPacketRecord> redPacketInfos)
        {
            var loteryCount = lotteryInfos.Count();
            var looteryWinCount = lotteryInfos.Where(w => w.IsSuccessPrize == true).Count();

            var redpacktCount = redPacketInfos.Count();
            var redpacketTotalAmount = redPacketInfos.Sum(s => Convert.ToInt32(s.TotalAmount));
            return new UserItemsRecordResult
            {
                Success = true,
                LotteryCount = loteryCount,
                LotteryWinCount = looteryWinCount,

                RedpacktCount = redpacktCount,
                RedpacketTotalAmount = redpacketTotalAmount,
                LotteryInfos = lotteryInfos,
                RedPacketInfos = redPacketInfos
            };
        }

        public static UserItemsRecordResult FailResult(string message)
        {
            return new UserItemsRecordResult
            {
                Success = false,
                Message = message
            };
        }
    }

    public class LotteryRecordViewModel
    {
        /// <summary>
        /// 是否中奖
        /// </summary>
        public bool IsSuccessPrize { get; set; }

        /// <summary>
        /// 抽奖二维码编号
        /// </summary>
        public string QRCode { get; set; }

        /// <summary>
        /// 抽奖时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 活动编号
        /// </summary>
        public string ActivityNumber { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 奖品编号
        /// </summary>
        public string PrizeNumber { get; set; }

        /// <summary>
        /// 奖品名称
        /// </summary>
        public string PrizeName { get; set; } // 奖品名称

        /// <summary>
        /// 奖品类型
        /// </summary>
        public PrizeType Type { get; set; }//奖品类型
    }
}