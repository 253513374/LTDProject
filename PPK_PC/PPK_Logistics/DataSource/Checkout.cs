using ProtoBuf;
using System;
using System.Collections.Generic;

namespace PPK_Logistics.DataSource
{
    [ProtoBuf.ProtoContract]
    public class Checkout
    {
        private string agentid = "";
        private string Productid = "";
        private string barcode = "";
        private string ckd = "";
        private string timejzrq = "";
        private string status = "待";
        private string agentname = "";
        private string userid = "1";
        private string outtype = "";
        private string outstock = "";
        private string outstockid = "";
        private string Remark;

        /// <summary>
        /// 经销商ID
        /// </summary>
        [ProtoMember(1)]
        public string AGEENID
        {
            get { return agentid; }
            set { agentid = value; }
        }

        /// <summary>
        /// 条码序号
        /// </summary>
        [ProtoMember(2)]
        public string BRACODE
        {
            get { return barcode; }
            set { barcode = value; }
        }

        /// <summary>
        /// 出库单号
        /// </summary>
        [ProtoMember(3)]
        public string CKD
        {
            get { return ckd; }
            set { ckd = value; }
        }

        /// <summary>
        /// 出库类型
        /// </summary>
        [ProtoMember(4)]
        public string OutType
        {
            get { return outtype; }
            set { outtype = value; }
        }

        /// <summary>
        /// //产品ID
        /// </summary>
        [ProtoMember(5)]
        public string PRODUCTID
        {
            get { return Productid; }
            set { Productid = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        [ProtoMember(6)]
        public string STATUS
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        /// 发往的仓库
        /// </summary>
        [ProtoMember(7)]
        public string AgentName
        {
            get { return agentname; }
            set { agentname = value; }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        [ProtoMember(8)]
        public string USERID
        {
            get { return userid; }
            set { userid = value; }
        }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string Outstock
        {
            get
            {
                return outstock;
            }

            set
            {
                outstock = value;
            }
        }

        /// <summary>
        /// 下单日期
        /// </summary>
        public string Timejzrq
        {
            get
            {
                return timejzrq;
            }

            set
            {
                timejzrq = value;
            }
        }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public string Outstockid
        {
            get
            {
                return outstockid;
            }

            set
            {
                outstockid = value;
            }
        }

        /// <summary>
        /// 备注，摘要
        /// </summary>
        public string Remarks
        {
            get { return Remark; }
            set { Remark = value; }
        }
    }

    public class CheckoutInfo
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public static string AGEENID_KHID = "";

        /// <summary>
        /// 条码序号
        /// </summary>
        public static string BRACODE_BARCODE= "";

        /// <summary>
        /// 出库单号
        /// </summary>
        public static string CKD_CKBH = "";

        /// <summary>
        /// 发货类型true为箱发货，false为垛发货
        /// </summary>
        //public static bool OutType = "";

        /// <summary>
        /// 下单日期
        /// </summary>
        public static string PRODUCTID_JZRQ = "";

        /// <summary>
        /// 状态
        /// </summary>
        public static string STATUS = "";

        /// <summary>
        /// 客户名称
        /// </summary>
        public static string StockOut_KHMC = "";

        /// <summary>
        /// 用户ID
        /// </summary>
        public static string USERID = "";

        /// <summary>
        /// 单号下单数量
        /// </summary>
        public static string YFSL = "0";

        /// <summary>
        /// 实际出库数量
        /// </summary>
        public static string SFSL = "0";

        public static string PDSL = "0";

        /// <summary>
        /// 拨出仓库
        /// </summary>
        public static string KCSWZ_BCCK1 = "";

        /// <summary>
        /// 拨入仓库名称
        /// </summary>
        public static string KCSWZ_BRCK1 = "";

        /// <summary>
        /// 拨入仓库ID
        /// </summary>
        public static string KCSWZ_BRCKID = "";

        /// <summary>
        /// 出库类型
        /// </summary>
        public static string KCSWZ_SWLX1 = "";

        /// <summary>
        /// 产品单位
        /// </summary>
        public static string KCSWZ_FJLDW = "";

        /// <summary>
        /// 备注
        /// </summary>
        public static string Remarks = "";
    }
}