namespace Wtdl.Model.Entity.Analysis
{
    public class OutStorageAnalysis
    {
        public int Id { set; get; }

        /// <summary>
        /// 年份
        /// </summary>
        public int Year { set; get; }

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { set; get; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumbels { set; get; }

        /// <summary>
        /// 订单号数量
        /// </summary>
        public double Count { set; get; }
    }
}