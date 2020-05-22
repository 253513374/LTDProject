using System;
using System.ComponentModel;

namespace PPK_Logistics.DataSource
{
    /// <summary>
    /// 包装信息类
    /// </summary>
    [Serializable]
    public class PackData_Simple : INotifyPropertyChanged
    {
        public PackData_Simple()
            : base()
        {
        }

        /// <summary>
        /// 根据包装信息初始化包装数据类
        ///
        /// _labelList" ==扫描的标签序号
        /// "_productid"==产品ID
        /// "_packNum"  ==包装单号
        /// "_packpici" ==包装批次
        /// "_companyid"==企业唯一编码
        /// "_agentid"  ==经销商ID
        /// "_remark    ==备注
        /// _productname==产品名称
        /// _datetime   ==包装时间
        /// </summary>
        /// <param name="_labelList" > 扫描的标签序号</param>
        /// <param name="_productid"></param>
        /// <param name="_packNum"></param>
        /// <param name="_packpici"></param>
        /// <param name="_companyid"></param>
        /// <param name="_agentid"></param>
        /// <param name="_remark"></param>
        public PackData_Simple(string _labelList, int _productid, string _productname, string _packNum, string _packpici, string _companyid, int _agentid, string _remark)
        {
            this._agentid = _agentid;
            this._companyid = _companyid;
            this._packNum = _packNum;
            this._packpici = _packpici;
            this._remark = _remark;
            this._productid = _productid;
            this._labelList = _labelList;
            this._productname = _productname;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        #region Model

        private int _agentid;
        private string _companyid = "";
        private string _datetime = "";
        private string _labelList = "";
        private string _packNum = "";
        private string _packpici = "";
        private int _productid;
        private string _productname = "";
        private string _remark = "";
        private string _status = "待上传";

        /// <summary>
        /// 经销商ID
        /// </summary>
        public int AGENTID
        {
            set
            {
                _agentid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AGENTID"));
            }
            get { return _agentid; }
        }

        /// <summary>
        /// 企业唯一编码
        /// </summary>
        public string COMPANYID
        {
            set
            {
                _companyid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("COMPANYID"));
            }
            get { return _companyid; }
        }

        /// <summary>
        /// 包装时间
        /// </summary>
        public string DATETIME
        {
            set
            {
                _datetime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DATETIME"));
            }
            get { return _datetime; }
        }

        /// <summary>
        /// 标签序号
        /// </summary>
        public string LABELLIST
        {
            set
            {
                _labelList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LABELLIST"));
            }
            get { return _labelList; }
        }

        /// <summary>
        /// 包装单号
        /// </summary>
        public string PACKNUM
        {
            set
            {
                _packNum = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PACKNUM"));
            }
            get { return _packNum; }
        }

        /// <summary>
        /// 批次
        /// </summary>
        public string PACKPICI
        {
            set
            {
                _packpici = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PACKPICI"));
            }
            get { return _packpici; }
        }

        /// <summary>
        /// 产品ID
        /// </summary>
        public int PRODUCTID
        {
            set
            {
                _productid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PRODUCTID"));
            }
            get { return _productid; }
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string PRODUCTNAME
        {
            set
            {
                _productname = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PRODUCTNAME"));
            }
            get { return _productname; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string REMARK
        {
            set
            {
                _remark = value;
                OnPropertyChanged(new PropertyChangedEventArgs("REMARK"));
            }
            get { return _remark; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Status"));
            }
        }

        #endregion Model
    }
}