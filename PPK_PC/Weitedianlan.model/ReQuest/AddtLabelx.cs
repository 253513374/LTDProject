using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weitedianlan.model.ReQuest
{
    public class AddtLabelx
    {
        public int ID { get; set; }
        public string QRCode { get; set; }
        public System.DateTime OrderTime { get; set; }
        public System.DateTime OutTime { get; set; }
        public string Dealers { get; set; }
        public string DealersName { set; get; }
        public string Adminaccount { get; set; }
        public string OutType { get; set; }
        public string OrderNumbels { get; set; }
        public string ExtensionName { get; set; }
        public string ExtensionOrder { get; set; }
    }
}
