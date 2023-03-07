using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Weitedianlan.Model.Enum;

namespace Weitedianlan.Model.Entity
{
    /// <summary>
    /// 这个类与Prize一样。这里保存的是参加抽奖的奖品信息
    /// </summary>
    public class ActivityPrize : IEntityBase
    {
        public ActivityPrize()
        {
            PrizeNumber = Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// 奖品编号。
        /// </summary>
        public int Id { get; set; }//奖品ID

        /// <summary>
        /// 中奖编号
        /// </summary>
        public int UniqueNumber { get; set; }

        /// <summary>
        /// 奖品唯一编号
        /// </summary>
        public string PrizeNumber { get; set; }

        /// <summary>
        /// 奖品名称
        /// </summary>
        public string Name { get; set; }//奖品名称

        /// <summary>
        /// 奖品描述
        /// </summary>
        public string Description { get; set; }//奖品描述

        /// <summary>
        /// 奖品总数量
        /// </summary>
        public int Amount { get; set; }//奖品数量

        /// <summary>
        /// 中奖数量，该值不能大于总数量，否则会出现奖品数量不足的情况
        /// </summary>
        public int Unredeemed { get; set; }

        /// <summary>
        /// 中奖概率
        /// </summary>
        public double Probability { get; set; }//中奖概率

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }//是否启用

        /// <summary>
        /// 奖品图片
        /// </summary>
        [JsonIgnore]
        public string ImageUrl { get; set; }//奖品图片

        /// <summary>
        /// 奖品类型
        /// </summary>
        public PrizeType Type { get; set; }//奖品类型

        /// <summary>
        /// 金额
        /// </summary>
        public int CashValue { get; set; }

        ///// <summary>
        ///// 最小金额限制
        ///// </summary>
        //public int MinCashValue { get; set; } //新增最小金额属性

        ///// <summary>
        ///// 最大金额限制
        ///// </summary>
        //public int MaxCashValue { get; set; } //新增最大金额属性

        /// <summary>
        /// 是否参加活动
        /// </summary>
        public bool IsJoinActivity { get; set; }

        /// <summary>
        /// 参加活动的ID
        /// </summary>
        public int? LotteryActivityId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// 参加活动信息
        /// </summary>
        public virtual LotteryActivity LotteryActivity { get; set; }

        /// <summary>
        /// 辅助属性，不作为表字段使用
        /// </summary>
        public string Identifier { set; get; }
    }
}