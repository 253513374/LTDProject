using PPK_Logistics.Config;
using PPK_Logistics.DataSource;
using PPK_Logistics.DataSync;
using PPK_Logistics.Verify;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using System.Linq;
using System.Text;

namespace PPK_Logistics
{
    public class PDAServer
    {
        public string login(string u, string p)
        {
            try
            {
                DataInfo _dataInfo = new DataInfo();
                _dataInfo.ConnectType = "SQL";
                _dataInfo.Parameters = null;
                _dataInfo.ConnectionStrings = Tools.GetAppSettings("ConnectionString");
                _dataInfo.CommandText = string.Format("select a.AType from tUser u join tAgent a on u.AgentID = a.AID  where UserID='{0}' and PWD='{1}'", u,new  EnCodeClass().Encrypt(p));
                //_dataInfo.CommandText = string.Format("select UserID from tUser where UserID='{0}' and PWD='{1}'", u, BLL.EnCodeClass.Encrypt(p));

                string i = "-1";
                string quanxian = "";

                DataSet set = SqlCommand_BLL.ReturnExecuteDataSet(_dataInfo);
                if (set != null)
                {
                    if (set.Tables[0].Rows.Count > 0)
                    {
                        _dataInfo.CommandText = string.Format("select * from tUserRight where UserID='{0}' and Flag='1'", u);
                        DataSet DS = SqlCommand_BLL.ReturnExecuteDataSet(_dataInfo);
                        for (int n = 0; n < DS.Tables[0].Rows.Count; n++)
                        {
                            quanxian += DS.Tables[0].Rows[n]["MID"].ToString().Trim() + ",";
                        }
                        i = "1|" + quanxian + "|" + set.Tables[0].Rows[0][0].ToString();
                    }
                }
                else
                {
                    i = "0";
                }
                return i.ToString();
            }
            catch (Exception ex)
            {
                //rtbHistory.AppendText("---------------Login报错：---------------");
                //rtbHistory.AppendText(string.Format("报错时间：{0}", System.DateTime.Now.ToString()));
                //rtbHistory.AppendText(string.Format("报错内容：{0}", ex.ToString()));
                return ex.Message.ToString();
            }
        }

        #region MyRegion

       
        /// <summary>
        /// 获取发货单
        /// </summary>
        /// <param name="SFTK"></param>
        /// <returns></returns>
        public MemoryStream GetERP_Kcswz(string SFTK)
        {
            DataInfo _dataInfo = new DataInfo();
            _dataInfo.ConnectType = "SQL";
            _dataInfo.ConnectionStrings = Tools.GetAppSettings("ConnectionStringERP");
            _dataInfo.CommandText = string.Format(@"
                                    SELECT T4.*,SUM(T5.KCSWZMX_FZCKSL) AS KCSWZMX_FZCKSL  FROM
                                    (
                                    SELECT Top 100  T.KCSWZ_JZRQ, T.KCSWZ_KHID, T.KCSWZ_SWKCBH, T2.KH_MC
                                    FROM JSERP8.KCSWZ T
                                    joIn JSERP8.KH T2 ON T.KCSWZ_KHID = T2.KH_KHID
                                    where T.KCSWZ_SFTK='{0}'
                                    order by  T.KCSWZ_JZRQ desc
                                    ) T4
                                    JOIN JSERP8.KCSWZMX T5 ON T4.KCSWZ_SWKCBH=T5.KCSWZMX_SWKCBH
                                    WHERE T5.KCSWZMX_FZCKSL>0
                                    GROUP BY T4.KCSWZ_JZRQ, T4.KCSWZ_KHID, T4.KCSWZ_SWKCBH, T4.KH_MC
                                    order by  T4.KCSWZ_JZRQ desc", SFTK);

            DataTable _DataTable = SqlCommand_BLL.ReturnExecuteDataSet(_dataInfo).Tables[0];

            DataTable TEMP = _DataTable.Clone();
            foreach (DataColumn col in TEMP.Columns)
            {
                if (col.ColumnName == "KCSWZ_JZRQ") col.DataType = typeof(String);
            }
            foreach (DataRow row in _DataTable.Rows)
            {
                DataRow rowNew = TEMP.NewRow();
                rowNew["KCSWZ_JZRQ"] = row["KCSWZ_JZRQ"];
                rowNew["KCSWZ_KHID"] = row["KCSWZ_KHID"];
                rowNew["KCSWZ_SWKCBH"] = row["KCSWZ_SWKCBH"];
                rowNew["KH_MC"] = row["KH_MC"];
                rowNew["KCSWZMX_FZCKSL"] = row["KCSWZMX_FZCKSL"];
                TEMP.Rows.Add(rowNew);
            }

            DataTable MergeTEMP = MergeDataTable(TEMP);
            MemoryStream _stream = new MemoryStream();
            if (MergeTEMP != null && MergeTEMP.Rows.Count > 0) MergeTEMP.WriteXml(_stream);
            return _stream;
        }



        /// <summary>
        /// 获取单个发货单 发货总数量  ，返回整个发货单信息
        /// </summary>
        /// <param name="ckbh"></param>
        /// <returns></returns>
        public MemoryStream GetERP_KcswzSearch(string ckbh)
        {
            DataInfo _dataInfo = new DataInfo();
            _dataInfo.ConnectType = "SQL";
            _dataInfo.ConnectionStrings = Tools.GetAppSettings("ConnectionStringERP");
            _dataInfo.CommandText = string.Format(@"
            SELECT T3.KCSWZ_JZRQ, T3.KCSWZ_KHID, T3.KCSWZ_SWKCBH, T3.KH_MC ,SUM( T4.KCSWZMX_FZCKSL) as KCSWZMX_FZCKSL
            FROM(
            SELECT T.KCSWZ_JZRQ, T.KCSWZ_KHID, T.KCSWZ_SWKCBH, T2.KH_MC
            FROM JSERP8.KCSWZ T joIn JSERP8.KH T2 ON T.KCSWZ_KHID = T2.KH_KHID
            where T.KCSWZ_SWKCBH like '%{0}%') T3
            join JSERP8.KCSWZMX T4 On T3.KCSWZ_SWKCBH = T4.KCSWZMX_SWKCBH
            Group by T3.KCSWZ_JZRQ, T3.KCSWZ_KHID, T3.KCSWZ_SWKCBH, T3.KH_MC  ", ckbh);

            DataTable _DataTable = SqlCommand_BLL.ReturnExecuteDataSet(_dataInfo).Tables[0];
            DataTable TEMP = _DataTable.Clone();
            foreach (DataColumn col in TEMP.Columns)
            {
                if (col.ColumnName == "KCSWZ_JZRQ") col.DataType = typeof(String);
            }
            foreach (DataRow row in _DataTable.Rows)
            {
                DataRow rowNew = TEMP.NewRow();
                rowNew["KCSWZ_JZRQ"] = row["KCSWZ_JZRQ"];
                rowNew["KCSWZ_KHID"] = row["KCSWZ_KHID"];
                rowNew["KCSWZ_SWKCBH"] = row["KCSWZ_SWKCBH"];
                rowNew["KH_MC"] = row["KH_MC"];
                rowNew["KCSWZMX_FZCKSL"] = row["KCSWZMX_FZCKSL"];
                TEMP.Rows.Add(rowNew);
            }

            DataTable MergeTEMP = MergeDataTable(TEMP);// MergeDataTable(DataTable First)
            MemoryStream _stream = new MemoryStream();
            if (MergeTEMP != null && MergeTEMP.Rows.Count > 0)
            {
                MergeTEMP.WriteXml(_stream);
                return _stream;
            }
            else
            {
                return null;
            }
        }

        public byte[] GetERP_kh()
        {
            return null;
        }

     

        public DataTable GetOutboundOrder()
        {
            DataInfo _dataInfo = new DataInfo();
            _dataInfo.ConnectType = "SQL";
            _dataInfo.ConnectionStrings = Tools.GetAppSettings("ConnectionStringERP");
            _dataInfo.CommandText = @"SELECT
	                                        T5.KCSWZMX_JZRQ,
	                                        T5.KCSWZ_KHID,
	                                        T5.KH_MC,
	                                        T5.KCSWZ_SWLX,
	                                        T5.KCSWZMX_SWKCBH,
	                                        T5.WL_FJLDW,
	                                        SUM (T5.KCSWZMX_FZCKSL) AS KCSWZMX_FZCKSL
                                        FROM
	                                        (
		                                        SELECT
			                                        T.KCSWZMX_JZRQ,
			                                        T2.KCSWZ_KHID,
			                                        T2.KCSWZ_SWLX,
			                                        T3.KH_MC,
			                                        T.KCSWZMX_SWKCBH,
			                                        T.KCSWZMX_CKSL,
			                                        T.KCSWZMX_FZCKSL,
			                                        T4.WL_FJLDW
		                                        FROM
			                                        JSERP8.KCSWZMX T
		                                        JOIN JSERP8.KCSWZ T2 ON T2.KCSWZ_SWKCBH = T.KCSWZMX_SWKCBH    
		                                        JOIN JSERP8.KH T3 ON T2.KCSWZ_KHID = T3.KH_KHID
		                                        JOIN JSERP8.WL T4 ON T.KCSWZMX_WLID = T4.WL_WLID
                                           WHERE 
				                                        DateDiff(dd,T.KCSWZMX_WHSJ,getdate())<=5 AND T2.KCSWZ_SFTK = 'N' AND T2.KCSWZ_SWLX = 'XSCK'
	                                        ) T5
                                        GROUP BY
	                                        T5.KCSWZMX_JZRQ,
	                                        T5.KCSWZ_KHID,
	                                        T5.KH_MC,
	                                        T5.KCSWZ_SWLX,
	                                        T5.WL_FJLDW,
	                                        T5.KCSWZMX_SWKCBH";

            DataTable _DataTable =SqlCommand_BLL.ReturnExecuteDataSet(_dataInfo).Tables[0];

            DataTable TEMP = _DataTable.Clone();
            foreach (DataColumn col in TEMP.Columns)
            {
                if (col.ColumnName == "KCSWZ_JZRQ") col.DataType = typeof(String);
            }
            foreach (DataRow row in _DataTable.Rows)
            {
                DataRow rowNew = TEMP.NewRow();
                rowNew["KCSWZMX_JZRQ"] = row["KCSWZMX_JZRQ"];
                rowNew["KCSWZ_KHID"] = row["KCSWZ_KHID"];
                rowNew["KCSWZMX_SWKCBH"] = row["KCSWZMX_SWKCBH"];
                rowNew["KCSWZ_SWLX"] = row["KCSWZ_SWLX"];
                rowNew["KH_MC"] = row["KH_MC"];
                rowNew["KCSWZMX_FZCKSL"] = row["KCSWZMX_FZCKSL"];
                rowNew["WL_FJLDW"] = row["WL_FJLDW"];
                TEMP.Rows.Add(rowNew);
            }

            DataTable MergeTEMP = MergeDataTable(TEMP);
            //MemoryStream _stream = new MemoryStream();
            // if (TEMP != null && TEMP.Rows.Count > 0) TEMP.WriteXml(_stream);

            return MergeTEMP;
        }

        public DataTable GetOutOrderDBCK()
        {
            DataInfo _dataInfo = new DataInfo();
            _dataInfo.ConnectType = "SQL";
            _dataInfo.ConnectionStrings = Tools.GetAppSettings("ConnectionStringERP");
            _dataInfo.CommandText = @"
                                    SELECT
	                                    T4.*, SUM (T5.KCSWZMX_FZCKSL) AS KCSWZMX_FZCKSL
                                    FROM
	                                    (
		                                    SELECT
			                                    TOP 1000 T.KCSWZ_JZRQ,
			                                    T.KCSWZ_CKID,
			                                    T.KCSWZ_SWLX,
			                                    T.KCSWZ_SWKCBH,
			                                    T.KCSWZ_RCKDH,
			                                    T2.CK_MC AS BCCK,
			                                    T3.CK_MC AS BRCK,
			                                    T.KCSWZ_DFCK
		                                    FROM
			                                    JSERP8.KCSWZ T
		                                    JOIN JSERP8.CK T2 ON T.KCSWZ_CKID = T2.CK_CKID
		                                    JOIN JSERP8.CK T3 ON T.KCSWZ_DFCK = T3.CK_CKID
		                                    WHERE
			                                    T.KCSWZ_SWLX = 'DBCK'
		                                    AND T.KCSWZ_SFTK = 'N'
		                                    ORDER BY
			                                    T.KCSWZ_JZRQ DESC
	                                    ) T4
                                    JOIN JSERP8.KCSWZMX T5 ON T4.KCSWZ_SWKCBH = T5.KCSWZMX_SWKCBH
                                    WHERE
	                                    DateDiff(
		                                    dd,
		                                    T5.KCSWZMX_WHSJ,
		                                    getdate()
	                                    ) <= 30
                                    GROUP BY
	                                    T4.KCSWZ_JZRQ,
	                                    T4.KCSWZ_CKID,
	                                    T4.BCCK,
	                                    T4.BRCK,
	                                    T4.KCSWZ_SWLX,
	                                    T4.KCSWZ_RCKDH,
	                                    T4.KCSWZ_SWKCBH,
	                                    T4.KCSWZ_DFCK
                                    ORDER BY
	                                    T4.KCSWZ_JZRQ DESC";

            DataTable _DataTable = SqlCommand_BLL.ReturnExecuteDataSet(_dataInfo).Tables[0];

            DataTable TEMP = _DataTable.Clone();

            TEMP.Columns.Add("WL_FJLDW");
            foreach (DataColumn col in TEMP.Columns)
            {
                if (col.ColumnName == "KCSWZ_JZRQ") col.DataType = typeof(String);
            }
            foreach (DataRow row in _DataTable.Rows)
            {
                DataRow rowNew = TEMP.NewRow();
                rowNew["KCSWZ_JZRQ"] = row["KCSWZ_JZRQ"];
                rowNew["KCSWZ_RCKDH"] = row["KCSWZ_RCKDH"];
                rowNew["KCSWZ_SWKCBH"] = row["KCSWZ_SWKCBH"];
                rowNew["KCSWZ_SWLX"] = row["KCSWZ_SWLX"];
                rowNew["KCSWZ_DFCK"] = row["KCSWZ_DFCK"];
                rowNew["BCCK"] = row["BCCK"];
                rowNew["BRCK"] = row["BRCK"];
                rowNew["KCSWZMX_FZCKSL"] = row["KCSWZMX_FZCKSL"];
                TEMP.Rows.Add(rowNew);
            }
            DataTable MergeTEMP = MergeDataTableDbck(TEMP);
            //MemoryStream _stream = new MemoryStream();
            // if (TEMP != null && TEMP.Rows.Count > 0) TEMP.WriteXml(_stream);
            return MergeTEMP;
        }

        /// <summary>
        /// 发货数据入库
        /// </summary>
        /// <param name="checkout"></param>
        /// <returns></returns>
        public List<Checkout> MarketOut(byte[] checkout)
        {
            List<Checkout> _ErrorOut = new List<Checkout>(1000);
            if (checkout.Length > 0)
            {
                MemoryStream _MemoryStream = new MemoryStream(checkout);
                List<Checkout> _CheckOut = ProtoBuf.Serializer.Deserialize<List<Checkout>>(_MemoryStream);
                if (_CheckOut != null && _CheckOut.Count > 0)
                {
                    GetERP_Wl(_CheckOut[0].AGEENID.ToString(), _CheckOut[0].AgentName.ToString());
                    DataValidity(_CheckOut, _ErrorOut);//校验有效数据，剔除无效数据
                    using (SqlConnection _SqlConnection = new SqlConnection(Tools.GetAppSettings("ConnectionString")))
                    {
                        if (_SqlConnection.State == ConnectionState.Closed) _SqlConnection.Open();
                        var i = 0;
                        using (SqlCommand updateCmd = new SqlCommand())
                        {
                            try
                            {
                                updateCmd.Connection = _SqlConnection;
                                for (i = 0; i < _CheckOut.Count; i++)
                                {
                                    updateCmd.CommandText = string.Format(@"insert into tLabels_X( XCode,PCode, fhAgent1,fhOriginalDate1, fhDate1 , fhPt1,fhType1, fhPaper1) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", _CheckOut[i].BRACODE.ToString(), _CheckOut[i].BRACODE.ToString(), _CheckOut[i].AGEENID.ToString(), _CheckOut[i].Timejzrq.ToString(), DateTime.Now.ToString(), _CheckOut[i].USERID.ToString(), "扎", _CheckOut[i].CKD.ToString());
                                    updateCmd.ExecuteNonQuery();
                                }
                            }
                            catch (System.Exception ex)
                            {
                                _CheckOut[i].STATUS = ex.Message;
                                _ErrorOut.AddRange(_CheckOut);
                            }
                        }
                    }
                }
                return _ErrorOut;
            }
            else { return null; }
        }
        #endregion


        /// <summary>
        /// 获取单个发货单的发货总数量
        /// </summary>
        /// <param name="ckbh"></param>
        /// <returns></returns>
        public int GetERP_KcswzCount(string ckbh)
        {
            try
            {
                DataInfo _dataInfo = new DataInfo();
                _dataInfo.ConnectType = "SQL";
                _dataInfo.ConnectionStrings = Tools.GetAppSettings("ConnectionStringERP");
                _dataInfo.CommandText = string.Format(@"SELECT
	                                                        KCSWZMX_SWKCBH,
	                                                        COUNT (t.KCSWZMX_FZCKSL) AS KCSWZMX_FZCKSL
                                                        FROM
	                                                        JSERP8.KCSWZMX t
                                                        WHERE
	                                                        t.KCSWZMX_SWKCBH = '{0}'
                                                        GROUP BY
	                                                        KCSWZMX_SWKCBH", ckbh);
                DataTable _DataTable = SqlCommand_BLL.ReturnExecuteDataSet(_dataInfo).Tables[0];

                if (_DataTable.Rows.Count > 0)
                {
                    return int.Parse(_DataTable.Rows[0][1].ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }

        public void GetERP_Wl(string khid, string khmc)
        {
            try
            {
                DataInfo _dataInfo = new DataInfo();
                _dataInfo.ConnectType = "SQL";
                _dataInfo.ConnectionStrings = Tools.GetAppSettings("ConnectionString");

                _dataInfo.CommandText = string.Format("select AID from tAgent where AID='{0}'", khid);

                object _obj = SqlCommand_BLL.ReturnExecuteScalar(_dataInfo);
                if (_obj == null)
                {
                    _dataInfo.CommandText = string.Format("insert into tAgent(AID ,AName,ABelong,AType) values('{0}','{1}','公司',0)", khid, khmc);
                    SqlCommand_BLL.ReturnExecuteNonQuery(_dataInfo);
                }
            }
            catch (System.Exception ex)
            {
            }
        }
        /// <summary>
        /// 发货数据扫码入库
        /// </summary>
        /// <param name="_CheckOut"></param>
        /// <returns></returns>
        public Checkout MarketOut2(Checkout _CheckOut)
        {
            if (_CheckOut != null)
            {
                if (!DataValidity2(_CheckOut)) return _CheckOut;//校验有效数据，剔除无效数据
                using (SqlConnection _SqlConnection = new SqlConnection(Tools.GetAppSettings("ConnectionString")))
                {
                    if (_SqlConnection.State == ConnectionState.Closed) _SqlConnection.Open();
                  
                    using (SqlCommand updateCmd = new SqlCommand())
                    {
                        try
                        {
                            updateCmd.Connection = _SqlConnection;

                            if(_CheckOut.OutType== OutType.DBCK.ToString())
                            {
                                updateCmd.CommandText = string.Format(@"insert into tLabels_X( XCode,PCode,Content, fhAgent1,fhOriginalDate1, fhDate1 , fhPt1,fhType1, fhPaper1,Content1) values('{0}','{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", _CheckOut.BRACODE.ToString(), _CheckOut.Remarks, _CheckOut.Outstockid.ToString(), _CheckOut.Timejzrq.ToString(), DateTime.Now.ToString(), _CheckOut.USERID.ToString(), "扎", _CheckOut.CKD.ToString(),_CheckOut.OutType.ToString());

                                GetERP_Wl(_CheckOut.Outstockid.ToString(), _CheckOut.Outstock.ToString());//把客户信息写入数据库
                            }
                            else
                            {
                                updateCmd.CommandText = string.Format(@"insert into tLabels_X( XCode,PCode,Content, fhAgent1,fhOriginalDate1, fhDate1 , fhPt1,fhType1, fhPaper1,Content1) values('{0}','{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", _CheckOut.BRACODE.ToString(), _CheckOut.Remarks, _CheckOut.AGEENID.ToString(), _CheckOut.Timejzrq.ToString(), DateTime.Now.ToString(), _CheckOut.USERID.ToString(), "扎", _CheckOut.CKD.ToString(), _CheckOut.OutType.ToString());
                                GetERP_Wl(_CheckOut.AGEENID.ToString(), _CheckOut.AgentName.ToString());//把客户信息写入数据库
                            }

                            updateCmd.ExecuteNonQuery();

                             _CheckOut.STATUS = "出库成功" ;
                            return _CheckOut;
                        }
                        catch (System.Exception ex)
                        {
                            if(ex.Message.Contains("ORA-00001"))
                            {

                            }
                            _CheckOut.STATUS = ex.Message;

                            return _CheckOut;
                            //_ErrorOut.AddRange(_CheckOut);
                        }
                    }
                }
            }
            else
            {
                return _CheckOut;
            }
        }

        public List<CheckoutReturn> MarketReturn(byte[] CheckoutReturn)
        {
            MemoryStream _MemoryStream = new MemoryStream(CheckoutReturn);

            List<CheckoutReturn> _CheckoutReturn = ProtoBuf.Serializer.Deserialize<List<CheckoutReturn>>(_MemoryStream);
            List<CheckoutReturn> _CheckoutError = new List<CheckoutReturn>(500);
            if (_CheckoutReturn != null && _CheckoutReturn.Count > 0)
            {
                DataInfo _dataInfo = new DataInfo();
                _dataInfo.ConnectType = "SQL";
                _dataInfo.ConnectionStrings = Tools.ConnectionString("ConnStr");

                for (int k = 0; k < _CheckoutReturn.Count; k++)
                {
                    _dataInfo.CommandText = string.Format("insert into tLabels_Re( XCode,PCode, fhAgent1,fhOriginalDate1, fhDate1 ,fhRept1, fhPt1,fhType1, fhPaper1) select T1.XCode,T1.PCode, T1.fhAgent1,T1.fhOriginalDate1, T1.fhDate1 ,'{0}', T1.fhPt1,T1.fhType1, T1.fhPaper1 from  tLabels_X T1 where T1.XCode='{1}'", _CheckoutReturn[k].Userid, _CheckoutReturn[k].Barcode);
                    int _obj = SqlCommand_BLL.ReturnExecuteNonQuery(_dataInfo);
                    if (_obj > 0)
                    {
                        _dataInfo.CommandText = string.Format("delete  from tLabels_X  where XCode='{0}'", _CheckoutReturn[k].Barcode);
                        SqlCommand_BLL.ReturnExecuteNonQuery(_dataInfo);
                    }
                    else
                    {
                        _CheckoutReturn[k].Status = "未发货";
                        _CheckoutError.Add(_CheckoutReturn[k]);
                    }
                }
            }
            return _CheckoutError;
        }

        /// <summary>
        /// 退货
        /// </summary>
        /// <param name="CheckoutReturn"></param>
        /// <returns></returns>
        public  PackDataDelete MarketReturn2(PackDataDelete CheckoutReturn)
        {
            try
            {
                if (CheckoutReturn != null)
                {
                    DataInfo _dataInfo = new DataInfo();
                    _dataInfo.ConnectType = "SQL";
                    _dataInfo.ConnectionStrings = Tools.GetAppSettings("ConnectionString");

                    _dataInfo.CommandText = string.Format("insert into tLabels_Re( XCode,PCode, fhAgent1,fhOriginalDate1, fhDate1 ,fhRept1, fhPt1,fhType1, fhPaper1) select T1.XCode,T1.PCode, T1.fhAgent1,T1.fhOriginalDate1, T1.fhDate1 ,'{0}', T1.fhPt1,T1.fhType1, T1.fhPaper1 from  tLabels_X T1 where T1.XCode='{1}'", CheckoutReturn.UserID, CheckoutReturn.NUMBER);
                    int _obj = SqlCommand_BLL.ReturnExecuteNonQuery(_dataInfo);
                    if (_obj > 0)
                    {
                        _dataInfo.CommandText = string.Format("delete  from tLabels_X  where XCode='{0}'", CheckoutReturn.NUMBER);
                        SqlCommand_BLL.ReturnExecuteNonQuery(_dataInfo);
                        CheckoutReturn.STATUS = "退货成功";
                    }
                    else
                    {
                        CheckoutReturn.STATUS = "未发货";
                    }
                }
            }
            catch (Exception ex)
            {
                CheckoutReturn.STATUS = ex.Message;
            }

            return CheckoutReturn;
        }

        /// <summary>
        /// 合表查询实时发货数据
        /// </summary>
        /// <param name="First"></param>
        /// <returns></returns>
        public DataTable MergeDataTable(DataTable First)
        {
            DataTable Merge = new DataTable();
            Merge = First.Clone();
            Merge.Columns.Add("yfsl");

            DataInfo _dataInfo = new DataInfo();
            _dataInfo.ConnectType = "SQL";
            _dataInfo.ConnectionStrings = Tools.GetAppSettings("ConnectionString");
            _dataInfo.CommandText = string.Format(@"SELECT
	                                                    fhPaper1,
	                                                    COUNT (fhPaper1) AS yfsl,
	                                                    fhOriginalDate1
                                                    FROM
	                                                    tLabels_X T
                                                    WHERE
	                                                    DateDiff(dd, T.fhDate1, getdate()) <= 10
                                                    GROUP BY
                                                    fhPaper1,
                                                    fhOriginalDate1");

            DataTable MergeTable = SqlCommand_BLL.ReturnExecuteDataSet(_dataInfo).Tables[0];
            foreach (DataRow item1 in First.Rows)
            {
                bool Re = true;
                foreach (DataRow item2 in MergeTable.Rows)
                {
                    if (item2["fhPaper1"].ToString().Trim() == item1["KCSWZMX_SWKCBH"].ToString().Trim())
                    {
                        DataRow _DataRow = Merge.NewRow();
                        _DataRow["yfsl"] = item2["yfsl"];
                        _DataRow["KCSWZMX_JZRQ"] = item1["KCSWZMX_JZRQ"];
                        _DataRow["KCSWZ_KHID"] = item1["KCSWZ_KHID"];
                        _DataRow["KCSWZMX_SWKCBH"] = item1["KCSWZMX_SWKCBH"];
                        _DataRow["KCSWZ_SWLX"] = item1["KCSWZ_SWLX"];
                        _DataRow["KH_MC"] = item1["KH_MC"];
                        _DataRow["KCSWZMX_FZCKSL"] = item1["KCSWZMX_FZCKSL"];
                        _DataRow["WL_FJLDW"] = item1["WL_FJLDW"];
                        Merge.Rows.Add(_DataRow);
                        Re = false;
                        break;
                    }
                }
                if (Re)
                {
                    DataRow _DataRow = Merge.NewRow();
                    _DataRow["yfsl"] = "0";
                    _DataRow["KCSWZMX_JZRQ"] = item1["KCSWZMX_JZRQ"];
                    _DataRow["KCSWZ_KHID"] = item1["KCSWZ_KHID"];
                    _DataRow["KCSWZMX_SWKCBH"] = item1["KCSWZMX_SWKCBH"];
                    _DataRow["KCSWZ_SWLX"] = item1["KCSWZ_SWLX"];
                    _DataRow["KH_MC"] = item1["KH_MC"];
                    _DataRow["KCSWZMX_FZCKSL"] = item1["KCSWZMX_FZCKSL"];
                    _DataRow["WL_FJLDW"] = item1["WL_FJLDW"];
                    Merge.Rows.Add(_DataRow);
                }
            }
            return Merge;
        }

        public DataTable MergeDataTableDbck(DataTable First)
        {
            DataTable Merge = new DataTable();
            Merge = First.Clone();
            Merge.Columns.Add("yfsl");

            DataInfo _dataInfo = new DataInfo();
            _dataInfo.ConnectType = "SQL";
            _dataInfo.ConnectionStrings = Tools.GetAppSettings("ConnectionString");
            _dataInfo.CommandText = string.Format(@"  select TOP 100 fhPaper1, COUNT(fhPaper1) AS yfsl,fhOriginalDate1  from tLabels_X T
                                      GROUP BY fhPaper1, fhOriginalDate1
                                      ORDER BY fhOriginalDate1 DESC");

            DataTable MergeTable = SqlCommand_BLL.ReturnExecuteDataSet(_dataInfo).Tables[0];
            foreach (DataRow item1 in First.Rows)
            {
                bool Re = true;
                foreach (DataRow item2 in MergeTable.Rows)
                {
                    if (item2["fhPaper1"].ToString().Trim() == item1["KCSWZ_SWKCBH"].ToString().Trim())
                    {
                        DataRow rowNew = Merge.NewRow();
                        rowNew["yfsl"] = item2["yfsl"];
                        rowNew["KCSWZ_JZRQ"] = item1["KCSWZ_JZRQ"];
                        rowNew["KCSWZ_RCKDH"] = item1["KCSWZ_RCKDH"];
                        rowNew["KCSWZ_SWKCBH"] = item1["KCSWZ_SWKCBH"];
                        rowNew["KCSWZ_SWLX"] = item1["KCSWZ_SWLX"];
                        rowNew["KCSWZ_DFCK"] = item1["KCSWZ_DFCK"];
                        rowNew["BCCK"] = item1["BCCK"];
                        rowNew["BRCK"] = item1["BRCK"];
                        rowNew["KCSWZMX_FZCKSL"] = item1["KCSWZMX_FZCKSL"];
                        rowNew["WL_FJLDW"] = item1["WL_FJLDW"];
                        Merge.Rows.Add(rowNew);
                        Re = false;
                        break;
                    }
                }
                if (Re)
                {
                    DataRow _DataRow = Merge.NewRow();
                    _DataRow["yfsl"] = "0";
                    _DataRow["KCSWZ_JZRQ"] = item1["KCSWZ_JZRQ"];
                    _DataRow["KCSWZ_RCKDH"] = item1["KCSWZ_RCKDH"];
                    _DataRow["KCSWZ_SWKCBH"] = item1["KCSWZ_SWKCBH"];
                    _DataRow["KCSWZ_SWLX"] = item1["KCSWZ_SWLX"];
                    _DataRow["KCSWZ_DFCK"] = item1["KCSWZ_DFCK"];
                    _DataRow["BCCK"] = item1["BCCK"];
                    _DataRow["BRCK"] = item1["BRCK"];
                    _DataRow["KCSWZMX_FZCKSL"] = item1["KCSWZMX_FZCKSL"];
                    _DataRow["WL_FJLDW"] = item1["WL_FJLDW"];
                    Merge.Rows.Add(_DataRow);
                }
            }
            return Merge;
        }

        public DataTable MarketOutSCount(string ckbh)
        {
            try
            {
                DataInfo _dataInfo = new DataInfo();
                _dataInfo.ConnectType = "SQL";
                _dataInfo.ConnectionStrings = Tools.GetAppSettings("ConnectionString");
                _dataInfo.CommandText = string.Format(@"  select TOP 100 fhPaper1, COUNT(fhPaper1) AS yfsl,fhOriginalDate1  from tLabels_X T  where fhPaper1='{0}'
                                      GROUP BY fhPaper1, fhOriginalDate1
                                      ORDER BY fhOriginalDate1 DESC", ckbh);

                return SqlCommand_BLL.ReturnExecuteDataSet(_dataInfo).Tables[0];
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 发货数据校验
        /// </summary>
        /// <param name="_CheckOut"></param>
        /// <param name="ErrorOut"></param>
        private void DataValidity(List<Checkout> _CheckOut, List<Checkout> ErrorOut)
        {
            if (_CheckOut == null) return;
            DataInfo _dataInfo = new DataInfo();
            _dataInfo.ConnectType = "SQL";
            _dataInfo.ConnectionStrings = Tools.GetAppSettings("ConnectionString");

            List<Checkout> TempCheckout = new List<Checkout>(1500);
            try
            {
                for (int k = 0; k < _CheckOut.Count; k++)
                {
                    _dataInfo.CommandText = string.Format("SELECT  top 1 XCode FROM tLabels_X where XCode = '{0}'", _CheckOut[k].BRACODE);

                    object _obj = SqlCommand_BLL.ReturnExecuteScalar(_dataInfo);

                    if (_obj != null)
                    {
                        _CheckOut[k].STATUS = "重复发货";
                        ErrorOut.Add(_CheckOut[k]);
                    }
                    else
                    {
                        TempCheckout.Add(_CheckOut[k]);
                    }
                }
                _CheckOut.RemoveRange(0, _CheckOut.Count);
                _CheckOut.AddRange(TempCheckout);
            }
            catch (System.Exception ex)
            {
            }
        }

        private bool DataValidity2(Checkout _CheckOut)
        {
            DataInfo _dataInfo = new DataInfo();
            _dataInfo.ConnectType = "SQL";
            _dataInfo.ConnectionStrings = Tools.GetAppSettings("ConnectionString");

            //List<Checkout> TempCheckout = new List<Checkout>(1500);
            try
            {
                _dataInfo.CommandText = string.Format("SELECT  top 1 XCode FROM tLabels_X where XCode = '{0}'", _CheckOut.BRACODE);

                object _obj = SqlCommand_BLL.ReturnExecuteScalar(_dataInfo);

                if (_obj != null)
                {
                    _CheckOut.STATUS = "重复出库";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                _CheckOut.STATUS = ex.Message;
                return false;
            }
        }
    }
}