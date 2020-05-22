using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PPK_Logistics.DataSource
{
    [ProtoBuf.ProtoContract]
    public class CheckoutReturn
    {
        private string agentid = "";
        private string barcode = "";

        private string ckdnum = "";
        private string status = "待";

        private string userid = "";

        /// <summary>
        ///
        /// </summary>
        [ProtoBuf.ProtoMember(1)]
        public string Agentid
        {
            get { return agentid; }
            set { agentid = value; }
        }

        [ProtoBuf.ProtoMember(2)]
        public string Barcode
        {
            get { return barcode; }
            set { barcode = value; }
        }

        /// <summary>
        /// 退库单号
        /// </summary>
        [ProtoBuf.ProtoMember(3)]
        public string Ckdnum
        {
            get { return ckdnum; }
            set { ckdnum = value; }
        }

        [ProtoBuf.ProtoMember(4)]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        [ProtoBuf.ProtoMember(5)]
        public string Userid
        {
            get { return userid; }
            set { userid = value; }
        }
    }
}