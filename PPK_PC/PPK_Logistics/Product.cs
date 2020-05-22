using System;

namespace PPK_Logistics
{
    /// <summary>
    /// 企业产品信息类
    /// </summary>
    [Serializable]
    public class Product
    {
        public Product()
        { }

        /// <summary>
        /// 说明
        /// </summary>
        public string EXPLANATION
        {
            set;
            get;
        }

        /// <summary>
        /// ID
        /// </summary>
        public decimal ID
        {
            set;
            get;
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string PRODUCTNAME
        {
            set;
            get;
        }

        /// <summary>
        /// 对应关系
        /// </summary>
        public string PRODUCTNORMS
        {
            set;
            get;
        }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string PRODUCTNUMBER
        {
            set;
            get;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string REMARK
        {
            set;
            get;
        }

        /// <summary>
        /// 规格
        /// </summary>
        public string STANDARD
        {
            set;
            get;
        }
    }
}