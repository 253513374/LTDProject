using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weitedianlan.model.Response
{
   public  class ResponseModel
    {
        public int code { get; set; }
        public string result { get; set; }
        public dynamic data { get; set; }
    }
}
