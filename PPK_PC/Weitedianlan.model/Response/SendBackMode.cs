using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weitedianlan.model.Response
{
    public class SendBackMode
    {
        public string QrCode { set; get; }
        public string ReCount { set; get; }
        public int ResulCode { set; get; }
        public string ResultStatus { set; get; }
        public string Errorinfo { get; set; }
    }
}
 