using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weitedianlan.Model.Enum;

namespace Weitedianlan.Model.Entity
{
    /// <summary>
    /// 抽奖记录
    /// </summary>
    public class LotteryRecord : IEntityBase
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }// ID

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }// 用户ID

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }// 用户名

        /// <summary>
        /// 抽奖二维码编号
        /// </summary>
        public string QRCode { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string UserPhoneNumber { get; set; }// 用户手机号

        /// <summary>
        /// 中奖时间
        /// </summary>
        public DateTime Time { get; set; }// 中奖时间

        /// <summary>
        /// 是否已经领取
        /// </summary>
        public bool IsClaimed { get; set; }// 是否已领取

        /// <summary>
        /// 领取时间
        /// </summary>
        public DateTime? ClaimTime { get; set; }// 领取时间

        /// <summary>
        /// 活动ID
        /// </summary>
        public string ActivityId { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }//  活动名称

        /// <summary>
        /// 奖品ID
        /// </summary>
        public int PrizeId { get; set; } //奖品ID

        /// <summary>
        /// 奖品类型
        /// </summary>
        public PrizeType Type { get; set; }//奖品类型

        /// <summary>
        /// 中奖金额
        /// </summary>
        public decimal CashValue { get; set; }

        /// <summary>
        /// 奖品名称
        /// </summary>
        public string PrizeName { get; set; } // 奖品名称

        /// <summary>
        /// 奖品描述
        /// </summary>
        public string PrizeDescription { get; set; }// 奖品描述
    }
}