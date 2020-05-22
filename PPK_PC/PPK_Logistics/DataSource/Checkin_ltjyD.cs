using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace PPK_Logistics.DataSource
{
    /// <summary>
    /// 包装信息类
    /// </summary>
    [ProtoContract]
    public class Checkin_ltjyStack : INotifyPropertyChanged
    {
        private string _crib = "";
        private List<Checkin_ltjy> _listPD_Simple = new List<Checkin_ltjy>();

        private int _packDataCount;

        //private string _status = "待上传";
        private string status = "待上传";

        public Checkin_ltjyStack()
            : base()
        {
        }

        /// <summary>
        /// 根据包装信息初始化包装数据类
        ///
        /// _crib" ==垛号
        /// "_listPD_Simple"==箱与产品的List
        /// </summary>
        /// <param name="_crib" > 扫描的标签序号</param>
        /// <param name="_listPD_Simple"></param>
        public Checkin_ltjyStack(string _crib, List<Checkin_ltjy> _listPD_Simple)
        {
            this._crib = _crib;
            this._listPD_Simple = _listPD_Simple;
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

        /// <summary>
        /// 垛号
        /// </summary>
        [ProtoMember(1)]
        public string CRIB
        {
            set
            {
                _crib = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CRIB"));
            }
            get { return _crib; }
        }

        /// <summary>
        /// 箱列表
        /// </summary>
        [ProtoMember(2)]
        public List<Checkin_ltjy> LISTPD_SIMPLE
        {
            set
            {
                _listPD_Simple = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LISTPD_SIMPLE"));
            }
            get { return _listPD_Simple; }
        }

        /// <summary>
        /// 箱数
        /// </summary>>
        [ProtoMember(3)]
        public int PACKDATACOUNT
        {
            get
            {
                return _listPD_Simple.Count;
            }
            set
            {
                _packDataCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PACKDATACOUNT"));
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        [ProtoMember(4)]
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Status"));
            }
        }

        #endregion Model
    }
}