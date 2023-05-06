using ScanCode.Model.Enum;
using System;

namespace ScanCode.Model.Entity
{
    /// <summary>
    /// 抽奖记录
    /// </summary>
    public class LotteryRecord : IEntityBase
    {
        public LotteryRecord()
        {
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }// ID

        /// <summary>
        /// 是否中奖
        /// </summary>
        public bool IsSuccessPrize { get; set; }

        /// <summary>
        /// 微信用户openid
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 微信用户名称
        /// </summary>
        public string UserName { get; set; }// 用户名

        /// <summary>
        /// 抽奖二维码编号
        /// </summary>
        public string QRCode { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string UserPhoneNumber { get; set; }

        /// <summary>
        /// 抽奖时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 领取奖品状态 是否已领取
        /// </summary>
        public ClaimedStatus Claimed { get; set; }

        /// <summary>
        /// 领取时间
        /// </summary>
        public DateTime? ClaimTime { get; set; }

        /// <summary>
        /// 活动编号
        /// </summary>
        public string ActivityNumber { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 活动描述
        /// </summary>
        public string ActivityDescription { get; set; }

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

        /// <summary>
        /// 中奖金额
        /// </summary>
        public int CashValue { get; set; }

        /// <summary>
        /// 奖品描述
        /// </summary>
        public string PrizeDescription { get; set; }
    }
}