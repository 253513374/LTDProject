using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wtdl.Share
{
    public class GroupedBdxOrder
    {
        public string DDNO { get; set; }
        public string KH { get; set; }
        public string DDRQ { get; set; }
        public string THRQ { get; set; }
        public int TotalSL { get; set; }

        public string DW { get; set; }
        public int TotalCount { get; set; }
    }
}