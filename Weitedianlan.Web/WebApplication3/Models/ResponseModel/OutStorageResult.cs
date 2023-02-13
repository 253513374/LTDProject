namespace Wtdl.Mvc.Models
{
    /// <summary>
    /// 溯源信息
    /// </summary>
    public class OutStorageResult
    {
        /// <summary>
        /// 查询状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 查询信息
        /// </summary>
        public string Msg { get; init; }

        /// <summary>
        /// 标签序号
        /// </summary>
        public string QRCode { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string AgentName { get; init; }

        /// <summary>
        /// 出库单号
        /// </summary>
        public string OrderNumbels { get; init; }

        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime OutTime { get; init; }
    }
}