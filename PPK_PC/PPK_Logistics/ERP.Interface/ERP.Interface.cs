using PPK_Logistics.Config;
using PPK_Logistics.DataSync;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PPK_Logistics.ERP.Interface
{
    public class ERP
    {
        public virtual DataTable GetOutboundOrder()
        { return null; }

        /// <summary>
        /// 获取当前数据源表结构
        /// </summary>
        /// <returns></returns>
        public static DataTable TabClone()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("KCSWZMX_JZRQ", typeof(String)));
            dt.Columns.Add(new DataColumn("KCSWZ_KHID"));
            dt.Columns.Add(new DataColumn("KCSWZMX_SWKCBH"));
            dt.Columns.Add(new DataColumn("KCSWZ_SWLX"));
            dt.Columns.Add(new DataColumn("KH_MC"));
            dt.Columns.Add(new DataColumn("Comment"));
            dt.Columns.Add(new DataColumn("KCSWZMX_FZCKSL"));
            dt.Columns.Add(new DataColumn("WL_FJLDW"));
            dt.Columns.Add(new DataColumn("yfsl", typeof(String), "0"));

            return dt;

            //rowNew["KCSWZMX_JZRQ"] = row["KCSWZMX_JZRQ"];
            //rowNew["KCSWZ_KHID"] = row["KCSWZ_KHID"];
            //rowNew["KCSWZMX_SWKCBH"] = row["KCSWZMX_SWKCBH"];
            //rowNew["KCSWZ_SWLX"] = row["KCSWZ_SWLX"];
            //rowNew["KH_MC"] = row["KH_MC"];
            //rowNew["KCSWZMX_FZCKSL"] = row["KCSWZMX_FZCKSL"];
            //rowNew["WL_FJLDW"] = row["WL_FJLDW"];
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
                        _DataRow["Comment"] = item1["Comment"];
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
                    _DataRow["Comment"] = item1["Comment"];
                    Merge.Rows.Add(_DataRow);
                }
            }
            return Merge;
        }
    }
}
