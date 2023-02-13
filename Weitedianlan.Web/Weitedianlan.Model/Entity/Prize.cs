using System;
using Weitedianlan.Model.Enum;

namespace Weitedianlan.Model.Entity
{
    /// <summary>
    /// 奖品池信息。
    /// </summary>
    public class Prize : IEntityBase
    {
        public Prize()
        {
            PrizeNumber = Guid.NewGuid().ToString().Replace("_", "");
        }

        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 中奖号码
        /// </summary>
        public string UniqueNumber { get; set; }

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
        /// 奖品数量
        /// </summary>
       // public int Amount { get; set; }//奖品数量

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
        public string ImageUrl { get; set; }//奖品图片

        ///// <summary>
        ///// 开始时间
        ///// </summary>
        //public DateTime? StartTime { get; set; }//开始时间

        ///// <summary>
        ///// 结束时间
        ///// </summary>
        //public DateTime? EndTime { get; set; }//结束时间

        /// <summary>
        /// 奖品类型
        /// </summary>
        public PrizeType Type { get; set; }

        ///// <summary>
        ///// 用户限制
        ///// </summary>
        //public int UserLimit { get; set; }//用户限制

        ///// <summary>
        ///// 总限制
        ///// </summary>
        //public int TotalLimit { get; set; }//总限制

        ///// <summary>
        ///// 中奖人数
        ///// </summary>
        //public int WinnerCount { get; set; }//中奖人数

        /// <summary>
        /// 金额
        /// </summary>
        public int CashValue { get; set; }

        ///// <summary>
        ///// 最小金额限制
        ///// </summary>
        //public decimal MinCashValue { get; set; } //新增最小金额属性

        ///// <summary>
        ///// 最大金额限制
        ///// </summary>
        //public decimal MaxCashValue { get; set; } //新增最大金额属性

        /// <summary>
        /// 是否参加活动
        /// </summary>
        public bool IsJoinActivity { get; set; }

        ///// <summary>
        ///// 参加活动的ID
        ///// </summary>
        //public int? LotteryActivityId { get; set; }

        ///// <summary>
        ///// 参加活动信息
        ///// </summary>
        //public virtual LotteryActivity LotteryActivity { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Identifier { set; get; }
    }
}