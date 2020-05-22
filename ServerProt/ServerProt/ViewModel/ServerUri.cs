using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ServerProt.ViewModel
{
    public class ServerUri
    {
        public string ServerIP { set; get; } = "192.168.1.96";

        public string ServerProt { set; get; } = "2015";

        public string GetUri()
        {
            //Text="http://192.168.1.152:2018/"
            return "http://" + ServerIP + ":" + ServerProt +"/";
        }


    }
}
