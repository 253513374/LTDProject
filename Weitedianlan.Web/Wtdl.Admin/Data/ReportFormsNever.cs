using Wtdl.Model.Entity.Analysis;

namespace Wtdl.Admin.Data
{
    /// <summary>
    /// 往年数据统计表
    /// </summary>
    public class ReportFormsNever
    {
        public ReportFormsNever()
        {
            GroupByOrderCounts = new List<OutStorageAnalysis>();
            GroupByYearCounts = new List<OutStorageAnalysis>();
        }

        /// <summary>
        /// 抽奖数量
        /// </summary>
        public int LotteryCount { set; get; }

        /// <summary>
        /// 中奖数量
        /// </summary>
        public int LotteryWinPrizeCount { set; get; }

        /// <summary>
        /// 红包发放数量
        /// </summary>
        public int RedPacketCount { set; get; }

        /// <summary>
        /// 红包发放金额
        /// </summary>
        public decimal TotalAmountSum { set; get; }

        /// <summary>
        /// 年订单量
        /// </summary>
        public List<OutStorageAnalysis> GroupByOrderCounts { set; get; }

        /// <summary>
        /// 年出货量
        /// </summary>
        public List<OutStorageAnalysis> GroupByYearCounts { set; get; }

        public string RadzenChartTitleText
        {
            get
            {
                if (GroupByYearCounts.Count > 0)
                {
                    return $"{GroupByYearCounts.Min(m => m.Year)} - {GroupByYearCounts.Max(m => m.Year)}";
                }

                return "";
            }
        }

        public double AllYearCount
        {
            get
            {
                if (GroupByYearCounts.Count > 0)
                    return GroupByYearCounts.Sum(s => s.Count);
                return 0;
            }
        }

        public double OrderCount
        {
            get
            {
                if (GroupByYearCounts.Count > 0)
                    return GroupByOrderCounts.Sum(s => s.Count);
                return 0;
            }
        }
    }
}