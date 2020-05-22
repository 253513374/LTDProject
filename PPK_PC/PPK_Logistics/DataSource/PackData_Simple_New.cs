using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PPK_Logistics.DataSource
{
    /// <summary>
    /// 包装信息类
    /// </summary>
    [Serializable]
    public class PackData_Simple_New : INotifyPropertyChanged
    {
        private string _crib = "";
        private List<PackData_Simple> _listPD_Simple = new List<PackData_Simple>();

        private int _packDataCount;

        //private string _status = "待上传";
        private string status = "待上传";

        public PackData_Simple_New()
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
        public PackData_Simple_New(string _crib, List<PackData_Simple> _listPD_Simple)
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
        public List<PackData_Simple> LISTPD_SIMPLE
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
        public int PACKDATACOUNT
        /// </summary>
        {
            get { return _listPD_Simple.Count; }
            set { _packDataCount = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        #endregion Model
    }
}