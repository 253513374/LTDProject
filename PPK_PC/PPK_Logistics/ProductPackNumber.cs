using System.Collections.ObjectModel;

namespace PPK_Logistics
{
    internal class ProductPackNumber : ObservableCollection<ProductPackNumber>
    {
        private string packCount = "";
        private string packEndCount = "";
        private string packpici = "";
        private string packProductname = "";
        private string packProductType = "";
        private string packRemark = "";
        private string packStorehouse = "";
        private string productnorms = "";
        private string productpacknumber = "";
        private string productPrice = "";
        private string producttoagent = "";
        private string time = "";

        public ProductPackNumber()
            : base()
        {
        }

        public ProductPackNumber(
         string productpacknumber,
         string packpici,
         string packCount,
         string packEndCount,
         string packProductname,
         string packStorehouse,
         string packRemark,
         string packProductType,
         string productPrice,
            string _time,
            string productnorm
        )// string packProductagent
        {
            this.productpacknumber = productpacknumber;
            this.packpici = packpici;
            this.packCount = packCount;
            this.packEndCount = packEndCount;
            this.packProductname = packProductname;
            this.packStorehouse = packStorehouse;
            this.packRemark = packRemark;
            this.packProductType = packProductType;
            this.productPrice = productPrice;
            this.time = _time;
            this.productnorms = productnorm;
            //this.producttoagent    = packProductagent;
        }

        public string DATETIME
        {
            get { return time; }
        }

        /// <summary>
        /// 包装数量
        /// </summary>
        public string PackCount
        {
            get { return this.packCount; }
        }

        /// <summary>
        /// 包装剩余数量
        /// </summary>
        public string PackEndCount
        {
            get { return this.packEndCount; }
        }

        /// <summary>
        /// 包装单编号
        /// </summary>
        public string PackNumber
        {
            get { return productpacknumber; }
        }

        /// <summary>
        /// 包装批次
        /// </summary>
        public string PackPici
        {
            get { return this.packpici; }
        }

        /// <summary>
        /// 包装产品名称
        /// </summary>
        public string PackProductname
        {
            get { return this.packProductname; }
        }

        /// <summary>
        /// 包装单类型
        /// </summary>
        public string PackProductType
        {
            get { return this.packProductType; }
        }

        /// <summary>
        /// 包装备注
        /// </summary>
        public string PackRemark
        {
            get { return packRemark; }
        }

        /// <summary>
        /// 包装仓库
        /// </summary>
        public string PackStorehouse
        {
            get { return packStorehouse; }
        }

        public string ProductNorms
        {
            get { return productnorms; }
        }

        /// <summary>
        /// 产品价格
        /// </summary>
        public string ProductPrice
        {
            get { return this.productPrice; }
        }

        public string ProductToAgent
        {
            get { return producttoagent; }
            // set { ;}
        }

        public ProductPackNumber GetItems(int k)
        {
            return this.Items[k];
        }
    }
}