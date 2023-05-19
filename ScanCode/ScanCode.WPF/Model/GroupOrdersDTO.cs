using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanCode.WPF.Model
{
    public partial class GroupOrdersDto : ObservableObject
    {
        /// <summary>
        /// 出库订单号
        /// </summary>
        public string? Ddno { set; get; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string? Kh { set; get; }

        /// <summary>
        /// 订单日期
        /// </summary>
        public string? Ddrq { set; get; }

        /// <summary>
        /// 提货日期
        /// </summary>
        public string? Thrq { set; get; }

        /// <summary>
        /// 单据发货数量
        /// </summary>
        public int? Totalsl { set; get; }

        public string? Dw { set; get; }

        /// <summary>
        /// 实际扫码发货数量
        /// </summary>
        public int? Totalcount { set; get; }
    }
}