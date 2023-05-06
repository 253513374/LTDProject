namespace ScanCode.WinUI.model.ReQuest
{
    public class RequestOrder
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string DDNO { set; get; }

        /// <summary>
        /// 订单日期
        /// </summary>
        public string DDRQ { set; get; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string KH { set; get; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public string SL { set; get; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public string OrderCount { set; get; } = "0";

        /// <summary>
        /// 单位
        /// </summary>
        public string DW { set; get; }
    }
}