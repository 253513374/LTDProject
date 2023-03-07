namespace Weitedianlan.Model.Entity.Analysis
{
    /// <summary>
    /// 年订单统计分析
    /// </summary>
    public class OrderNumberAnalysis
    {
        public OrderNumberAnalysis()
        {
        }

        public int Id { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        ///订单号
        public string OrderNumbels { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
    }
}