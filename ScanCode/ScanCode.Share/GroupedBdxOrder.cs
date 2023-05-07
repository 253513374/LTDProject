namespace ScanCode.Share
{
    public class GroupedBdxOrder
    {
        public string DDNO { get; set; }
        public string KH { get; set; }
        public string DDRQ { get; set; }
        public string THRQ { get; set; }

        /// <summary>
        /// 单据发货数量
        /// </summary>
        public int TotalSL { get; set; }

        public string DW { get; set; }

        /// <summary>
        /// 实际扫码发货数量
        /// </summary>
        public int TotalCount { get; set; }
    }
}