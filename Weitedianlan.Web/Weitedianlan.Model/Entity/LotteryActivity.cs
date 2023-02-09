using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
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
            ActivityNumber = Guid.NewGuid().ToString().Replace("-", "");
        }

        public int Id { get; set; }

        /// <summary>
        /// 活动唯一编号
        /// </summary>
        public string ActivityNumber { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 活动描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 活动状态
        /// </summary>
        public ActivityStatus Status { get; set; }

        /// <summary>
        /// 活动启用或者禁用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 活动参与人数
        /// </summary>
        public int TotalParticipant { get; set; }

        /// <summary>
        /// 总中奖人数
        /// </summary>
        public int TotalWinner { get; set; }

        /// <summary>
        /// 重复中奖：true 允许，false 不允许
        /// </summary>
        public bool AllowDuplicate { get; set; }

        /// <summary>
        /// 多次中奖 true：允许，false：不允许
        /// </summary>

        public bool AllowMultipleWinning { get; set; }

        /// <summary>
        /// 活动图片
        /// </summary>
        [JsonIgnore]
        public string ActivityImage { get; set; }

        /// <summary>
        /// 参与活动的奖品列表
        /// </summary>
        public virtual ICollection<ActivityPrize> Prizes { get; set; }

        /// <summary>
        /// 辅助数据显示的属性。
        /// </summary>
        public bool ShowPrizes { get; set; }

        /// <summary>
        /// 最后领取奖品的时间。
        /// </summary>
        //public DateTime LiveLastTime { get; set; }

        //public virtual ICollection<Child> Children { get; set; }
        /// <summary>
        /// 参与活动的客户列表
        /// </summary>
        //public virtual ICollection<ActivityParticipant> ActivityParticipants { get; set; }
    }
}