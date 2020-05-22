using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PPK_Logistics.DataSource
{
    public class OutboundOrder : INotifyPropertyChanged
    {
        private string KCSWZ_JZRQ = "";
        private string KCSWZ_KHID = "";
        private string KCSWZ_SWKCBH = "";
        private string KCSWZMX_FZCKSL = "0";
        private string KH_MC = "";
        private string KCSWZMX_SFSL = "0";
        private string KCSWZ_BCCK = "";
        private string KCSWZ_BRCK = "";
        private string KCSWZ_RCKDH = "";
        private string KCSWZ_SWLX = "";

        private string KCSWZ_BRCKID = "";

        private string KCSWZ_CPLX = "";
        private string remarks="";

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        public OutboundOrder(string KCSWZ_SWKCBH, string KCSWZ_KHID, string KCSWZ_JZRQ, string KH_MC, string KCSWZMX_FZCKSL,string KCSWZMX_SFSL,string KCSWZ_SWLX,string KCSWZ_FJLDW, string Remark)
        {
            this.KCSWZMX_FZCKSL = KCSWZMX_FZCKSL;
            this.KCSWZ_KHID = KCSWZ_KHID;
            this.KCSWZ_JZRQ = KCSWZ_JZRQ;
            this.KH_MC = KH_MC;
            this.KCSWZ_SWKCBH = KCSWZ_SWKCBH;
            this.KCSWZMX_SFSL = KCSWZMX_SFSL;
            this.KCSWZ_SWLX = KCSWZ_SWLX;
            this.KCSWZ_FJLDW = KCSWZ_FJLDW;
            this.Remarks = Remark;
        }

        public OutboundOrder(string KCSWZ_SWKCBH,string KCSWZ_RCKDH, string KCSWZ_BCCK, string KCSWZ_BRCK, string KCSWZ_JZRQ, string KCSWZMX_FZCKSL, string KCSWZMX_SFSL,string KCSWZ_SWLX,string KCSWZ_BRCKID,string KCSWZ_FJLDW,string Remark)
        {
            this.KCSWZMX_FZCKSL = KCSWZMX_FZCKSL;
            this.KCSWZ_RCKDH = KCSWZ_RCKDH;
            this.KCSWZ_JZRQ = KCSWZ_JZRQ;
            this.KCSWZ_BCCK = KCSWZ_BCCK;
            this.KCSWZ_BRCK = KCSWZ_BRCK;
            this.KCSWZ_SWKCBH = KCSWZ_SWKCBH;
            this.KCSWZMX_SFSL = KCSWZMX_SFSL;
            this.KCSWZ_SWLX = KCSWZ_SWLX;
            this.KCSWZ_BRCKID = KCSWZ_BRCKID;
            this.KCSWZ_FJLDW = KCSWZ_FJLDW;
            this.Remarks = Remark;
        }

        public OutboundOrder()
        {
        }

        /// <summary>
        /// 销售出库时间记录
        /// </summary>
        public string KCSWZ_JZRQ1
        {
            get
            {
                return KCSWZ_JZRQ;
            }

            set
            {
                KCSWZ_JZRQ = value;
                OnPropertyChanged(new PropertyChangedEventArgs("KCSWZ_JZRQ1"));
            }
        }

        /// <summary>
        /// 客户编号
        /// </summary>
        public string KCSWZ_KHID1
        {
            get
            {
                return KCSWZ_KHID;
            }

            set
            {
                KCSWZ_KHID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("KCSWZ_KHID1"));
            }
        }

        /// <summary>
        /// 销售出库编号
        /// </summary>
        public string KCSWZ_SWKCBH1
        {
            get
            {
                return KCSWZ_SWKCBH;
            }

            set
            {
                KCSWZ_SWKCBH = value;
                OnPropertyChanged(new PropertyChangedEventArgs("KCSWZ_SWKCBH1"));
            }
        }

        /// <summary>
        /// 出库数量
        /// </summary>
        public string KCSWZMX_FZCKSL1
        {
            get
            {
                return KCSWZMX_FZCKSL;
            }

            set
            {
                KCSWZMX_FZCKSL = value;
                OnPropertyChanged(new PropertyChangedEventArgs("KCSWZMX_FZCKSL1"));
            }
        }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string KH_MC1
        {
            get
            {
                return KH_MC;
            }

            set
            {
                KH_MC = value;
                OnPropertyChanged(new PropertyChangedEventArgs("KH_MC1"));
            }
        }

        /// <summary>
        /// 实际扫码数量
        /// </summary>
        public string KCSWZMX_SFSL1
        {
            get
            {
                return KCSWZMX_SFSL;
            }

            set
            {
                KCSWZMX_SFSL = value;
                OnPropertyChanged(new PropertyChangedEventArgs("KCSWZMX_SFSL1"));
            }
        }

        public string KCSWZ_BCCK1
        {
            get
            {
                return KCSWZ_BCCK;
            }

            set
            {
                KCSWZ_BCCK = value;
                OnPropertyChanged(new PropertyChangedEventArgs("KCSWZ_BCCK1"));
            }
        }

        public string KCSWZ_BRCK1
        {
            get
            {
                return KCSWZ_BRCK;
            }

            set
            {
                KCSWZ_BRCK = value;
                OnPropertyChanged(new PropertyChangedEventArgs("KCSWZ_BRCK1"));
            }
        }

        public string KCSWZ_SWLX1
        {
            get
            {
                return KCSWZ_SWLX;
            }

            set
            {
                KCSWZ_SWLX = value;
                OnPropertyChanged(new PropertyChangedEventArgs("KCSWZ_SWLX1"));
            }
        }

        public string KCSWZ_BRCKID1
        {
            get
            {
                return KCSWZ_BRCKID;
            }

            set
            {
                KCSWZ_BRCKID = value;
            }
        }

        /// <summary>
        /// 产品单位
        /// </summary>
        public string KCSWZ_FJLDW
        {
            get
            {
                return KCSWZ_CPLX;
            }

            set
            {
                KCSWZ_CPLX = value;
                OnPropertyChanged(new PropertyChangedEventArgs("KCSWZ_FJLDW"));
            }
        }

        /// <summary>
        /// 备注，摘要
        /// </summary>
        public string Remarks
        {
            get
            {
                return remarks;
            }

            set
            {
                remarks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remarks")); 
            }
        }

      
       
    }
}