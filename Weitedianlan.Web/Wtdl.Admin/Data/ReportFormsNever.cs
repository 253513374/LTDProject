using Weitedianlan.Model.Entity.Analysis;

namespace Wtdl.Admin.Data
{
    /// <summary>
    /// 缓存不长变动的数据。
    /// </summary>
    public class ReportFormsNever
    {
        public ReportFormsNever()
        {
            GroupByYearOrders = new List<OutStorageAnalysis>();
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
        public List<OutStorageAnalysis> GroupByYearOrders { set; get; }

        /// <summary>
        /// 年出货量
        /// </summary>
        public List<OutStorageAnalysis> GroupByYearCounts { set; get; }

        public string RadzenChartTitleText
        {
            get
            {
                return $"{GroupByYearCounts.Min(m => m.Year)} - {GroupByYearCounts.Max(m => m.Year)}";
            }
        }

        public double AllYearCount
        {
            get
            {
                return GroupByYearCounts.Sum(s => s.Count);
            }
        }

        public double OrderCount
        {
            get
            {
                return GroupByYearOrders.Sum(s => s.Count);
            }
        }
    }
}