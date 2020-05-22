using System;
using System.ComponentModel;
using ProtoBuf;

namespace PPK_Logistics
{
    [ProtoContract]
    public class UserReadonly : INotifyPropertyChanged// ObservableCollection<PackData_Simple>
    {
        private string userid = "";
        private string username = "";
        private string userpassword = "";
        private string userrole = "";
        private string usertoage = "";

        public UserReadonly()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 用户登录账号
        /// </summary>
        [ProtoMember(1)]
        public string UserId
        {
            get { return this.userid; }
            set
            {
                this.userid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserId"));
                //if (string.IsNullOrEmpty(userid))
                //{
                //    throw new ApplicationException("Please input Title.");
                //}
                //else
                //{
                //    this.userid = value;
                //    OnPropertyChanged(new PropertyChangedEventArgs("UserId"));
                //}
            }
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get { return this.username; }
            set
            {
                this.username = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserName"));
            }
        }

        /// <summary>
        /// 用户所属企业编号
        /// </summary>
        public string UserNunber
        {
            get { return this.userrole; }
            set
            {
                this.userrole = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserNunber"));
            }
        }

        /// <summary>
        /// 用户账号密码
        /// </summary>
        public string UserPassword
        {
            get { return userpassword; }
            set
            {
                userpassword = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserPassword"));
            }
        }

        /// <summary>
        /// 用户所属经销商名称
        /// </summary>
        public string UserToAge
        {
            get { return this.usertoage; }
            set
            {
                this.usertoage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserToAge"));
            }
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }
    }
}