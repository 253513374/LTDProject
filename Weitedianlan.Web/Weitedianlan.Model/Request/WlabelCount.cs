using System;
using System.Collections.Generic;
using System.Text;

namespace Weitedianlan.Model.Request
{
    public class WlabelCount
    {

        /// <summary>
        /// 出库时间
        /// </summary>
        public string OutTime { get; set; }

      
        public string Aname { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string Count { set; get; }

        public string ID { set; get; }

        /// <summary>
        /// 提货单号
        /// </summary>
        public string OrderNumbels { set; get; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string OrderTime { set; get; }

        public string OutType { set; get; }

        public string QRCode { set; get; }

        public string Adminaccount { set; get; }

        public string Dealers { set; get; }

        public string ExtensionName { set; get; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string AName { set; get; }
    }
}
