using System;
using System.Collections.Generic;
using System.Text;

namespace   Wtdl.Share
{
    public class WeixConfigResponse
    {
        public string appId { get; set; }
        public long timestamp { get; set; }
        public string nonceStr { get; set; }
        public string signature { get; set; }
    }
}