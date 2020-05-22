using PPK_Logistics.Config;
using PPK_Logistics.DataSync;
using System;
using System.Data;
using System.IO;

namespace PPK_Logistics.ERP.Interface
{
    public class GJPERPinterface :ERP
    {
        public GJPERPinterface() { }
        public override DataTable GetOutboundOrder()
        {
            DataInfo _dataInfo = new DataInfo();
            _dataInfo.ConnectType = "SQL";
            _dataInfo.ConnectionStrings = Tools.GetAppSettings("ConnectionStringGJPERP");
            _dataInfo.CommandText = SQLCommandtext.GJPCommands;

            DataTable _DataTable = SqlCommand_BLL.ReturnExecuteDataSet(_dataInfo).Tables[0];

            DataTable TEMP = ERP.TabClone();
            foreach (DataRow row in _DataTable.Rows)
            {
                DataRow rowNew = TEMP.NewRow();
                rowNew["KCSWZMX_JZRQ"] = row["BillDate"];
                rowNew["KCSWZ_KHID"] = row["UserCode"];
                rowNew["KCSWZMX_SWKCBH"] = row["BillCode"];
                rowNew["KCSWZ_SWLX"] = row["BillType"];
                rowNew["KH_MC"] = row["KHMC"];
                rowNew["Comment"] = row["Comment"];
                rowNew["KCSWZMX_FZCKSL"] = row["Qty"];
                rowNew["WL_FJLDW"] = row["danwei"];
                TEMP.Rows.Add(rowNew);
            }

            DataTable MergeTEMP = MergeDataTable(TEMP);
            return MergeTEMP;
        }
    }
}