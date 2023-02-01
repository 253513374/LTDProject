using System;
using System.Collections.Generic;
using Weitedianlan.Model.Enum;

namespace Weitedianlan.Model.Entity
{
    /// <summary>
    /// 抽奖活动
    /// </summary>
    public class LotteryActivity : IEntityBase
    {
        public LotteryActivity()
        {
            Prizes = new List<ActivityPrize>();
        }

        public int Id { get; set; }//活动ID
        public string Name { get; set; }//活动名称
        public string Description { get; set; }//活动描述
        public DateTime StartTime { get; set; }//开始时间
        public DateTime EndTime { get; set; }//结束时间
        public ActivityStatus Status { get; set; }//活动状态
        public bool IsActive { get; set; } //是否激活

        public int TotalParticipant { get; set; }//总参与人数
        public int TotalWinner { get; set; }//总中奖人数

        public bool AllowDuplicate { get; set; }//是否允许重复中奖

        public bool AllowMultipleWinning { get; set; }//是否允许多次中奖

        /// <summary>
        /// 参与活动的奖品列表
        /// </summary>
        public virtual ICollection<ActivityPrize> Prizes { get; set; }

        //public virtual ICollection<Child> Children { get; set; }
        /// <summary>
        /// 参与活动的客户列表
        /// </summary>
        //public virtual ICollection<ActivityParticipant> ActivityParticipants { get; set; }
    }
}