namespace ScanCode.WPF.HubServer.Entity
{
    public partial class T_BDX_ORDER
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
        /// 物料明细序号
        /// </summary>
        public string XH { set; get; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string GGXH { set; get; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public string SL { set; get; }

        /// <summary>
        /// 单位
        /// </summary>
        public string DW { set; get; }

        /// <summary>
        /// 单价
        /// </summary>
        public string DJ { set; get; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string YS { set; get; }

        //public partial class T_BDX_ORDER1
        //{
        //    public string DDNO { get; set; }
        //    public Nullable<System.DateTime> DDRQ { get; set; }
        //    public string KH { get; set; }
        //    public string XH { get; set; }
        //    public string GGXH { get; set; }
        //    public string SL { get; set; }
        //    public string DW { get; set; }
        //    public string DJ { get; set; }
        //    public string YS { get; set; }
        //}
    }
}