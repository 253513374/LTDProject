using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BackEndServer.ViewModel;
using System.Threading;
using System.Collections.ObjectModel;

namespace BackEndServer.Service
{
    public class Queryservice
    {
        public ServerUriModel ServerUri { set; get; } = new ServerUriModel();

       

        public string ServerMsg{ set; get; }
     

        private int QueryCount { set; get; } = 0;
        public string GetUrl()
        {
            return ServerUri.GetUri();
        }


        public bool IsUrl()
        {
            return ServerUri.IsUrl();
        }

      

    }

    public class ScanQuery
    {
        public string QrCode { set; get; }

        public string Name { set; get; }
    }
}
