using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BackEndServer.ViewModel
{
 
   public class ServerUriModel : INotifyPropertyChanged
    {
        public ServerUriModel() { }


        private string ip = "192.168.1.96";
        private string prot = "2015";

        public string ServerIP
        {
            set {
                ip = value;
                OnPropertyChanged(ServerIP);
            }
            get {
                return ip;
            }
        } 

        public string ServerProt {
            set
            {
                prot = value;
                OnPropertyChanged(ServerProt);
            }
            get
            {
                return prot;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string strPropertyInfo)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(strPropertyInfo));
            }
        }
      

        public string GetUri()
        {
            //Text="http://192.168.1.152:2018/"
            return "http://" + ServerIP + ":" + ServerProt + "/";
        }

        public bool IsUrl()
        {
            if(ServerIP!=""&& ServerProt!="")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
