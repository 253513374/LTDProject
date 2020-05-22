using System.ComponentModel;

namespace PPK_Logistics.DataSource
{
    public class PackDataDelete : INotifyPropertyChanged
    {
        private string number = "";

        private string status = "等待";

        private string userid = "";

        public PackDataDelete()
        {
        }

        public PackDataDelete(string code)
        {
            this.number = code;
        }

        public PackDataDelete(string code,string _user)
        {
            this.number = code;
            this.userid = _user;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string NUMBER
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NUMBER"));
            }
        }

        public string STATUS
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged(new PropertyChangedEventArgs("STATUS"));
            }
        }

        public string UserID
        {
            get { return userid; }
            set
            {
                userid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserID"));
            }
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }

            //this.PropertyChanged(this, e);
        }
    }
}