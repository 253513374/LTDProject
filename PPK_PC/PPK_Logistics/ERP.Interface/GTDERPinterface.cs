using PPK_Logistics.Config;
using PPK_Logistics.DataSync;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PPK_Logistics.ERP.Interface
{
    class GTDERPinterface :ERP
    {
        public DataTable GetOutboundOrder()
        {
            DataInfo _dataInfo = new DataInfo();
            _dataInfo.ConnectType = "SQL";
            _dataInfo.ConnectionStrings = Tools.GetAppSettings("ConnectionStringERP");
            _dataInfo.CommandText = SQLCommandtext.GTDCommands;

            DataTable _DataTable = SqlCommand_BLL.ReturnExecuteDataSet(_dataInfo).Tables[0];

            DataTable TEMP = _DataTable.Clone();
            //foreach (DataColumn col in TEMP.Columns)
            //{
            //    if (col.ColumnName == "KCSWZ_JZRQ") col.DataType = typeof(String);
            //}
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
    }
}
