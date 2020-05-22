using System.ComponentModel;
using ProtoBuf;

namespace PPK_Logistics.DataSource
{
    /// <summary>
    /// 包装箱的数据类
    /// </summary>
    [ProtoContract]
    public class Checkin_ltjy : INotifyPropertyChanged
    {
        private string _barcode;

        private string _productid;

        private string _status = "待";

        private string _userid;

        public Checkin_ltjy()
        { }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 标签序号
        /// </summary>
        [ProtoMember(1)]
        public string barcode
        {
            set
            {
                _barcode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("barcode"));
            }
            get { return _barcode; }
        }

        /// <summary>
        /// 产品ID
        /// </summary>
        [ProtoMember(2)]
        public string productid
        {
            set
            {
                _productid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("productid"));
            }
            get { return _productid; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        [ProtoMember(3)]
        public string status
        {
            set
            {
                _status = value;
                OnPropertyChanged(new PropertyChangedEventArgs("status"));
            }
            get { return _status; }
        }

        /// <summary>
        /// 发布者
        /// </summary>
        [ProtoMember(4)]
        public string userid
        {
            set
            {
                _userid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("userid"));
            }
            get { return _userid; }
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