using System;
using System.Collections.Generic;

namespace PPK_Logistics.DataSource
{
    [Serializable]
    public class Agent
    {
        public Agent()
        {
        }

        #region Model

        private string _agename;
        private string _ccusabbname;
        private string _ccuscode;
        private string _city;
        private string _companyid;
        private string _district;
        private decimal _grope;
        private decimal _id;
        private string _province;
        private string _remark;
        private decimal _subid;

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string AGENAME
        {
            set { _agename = value; }
            get { return _agename; }
        }

        /// <summary>
        /// 经销商全称
        /// </summary>
        public string CCUSABBNAME
        {
            set { _ccusabbname = value; }
            get { return _ccusabbname; }
        }

        /// <summary>
        /// 经销商编码
        /// </summary>
        public string CCUSCODE
        {
            set { _ccuscode = value; }
            get { return _ccuscode; }
        }

        /// <summary>
        /// 市
        /// </summary>
        public string CITY
        {
            set { _city = value; }
            get { return _city; }
        }

        /// <summary>
        /// 企业唯一编码
        /// </summary>
        public string COMPANYID
        {
            set { _companyid = value; }
            get { return _companyid; }
        }

        /// <summary>
        /// 区
        /// </summary>
        public string DISTRICT
        {
            set { _district = value; }
            get { return _district; }
        }

        /// <summary>
        /// 等级
        /// </summary>
        public decimal GROPE
        {
            set { _grope = value; }
            get { return _grope; }
        }

        /// <summary>
        /// ID
        /// </summary>
        public decimal ID
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// 省
        /// </summary>
        public string PROVINCE
        {
            set { _province = value; }
            get { return _province; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string REMARK
        {
            set { _remark = value; }
            get { return _remark; }
        }

        /// <summary>
        /// 上级ID
        /// </summary>
        public decimal SUBID
        {
            set { _subid = value; }
            get { return _subid; }
        }

        #endregion Model
    }

    [Serializable]
    public class Agents
    {
        public List<Agent> list;
    }
}