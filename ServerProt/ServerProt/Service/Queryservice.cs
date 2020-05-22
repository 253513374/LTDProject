using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using ServerProt.ViewModel;


namespace ServerProt.Service
{
    public class Queryservice
    {
        public ServerUri ServerUri { set; get; } = new ServerUri();

        public string GetLogistinInfo(string _label)
        {
            //  string _ProductName = "", _PackDate = "", _PackBatch = "",  _fhData = "", _cHeat = "";
            StringBuilder _result = new StringBuilder();
            try
            {
                _result.Append("<Info xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

                //DataSet DS = new DataSet();

                #region MyRegion

                //string sqlWT = string.Format(@" SELECT
                //                             x.bzDate,
                //                             x.bzPici,
                //                             chDate,
                //                             chAddr,
                //                             A.AName,
                //                             A.ATel AS ProductName,
                //                                x.fhDate1,
                //                                x.fhAgent1,
                //                                x.fhType1,
                //                                x.fhPaper1,
                //                                x.Content
                //                                FROM
                //                                 tLabels_X x
                //                                JOIN tAgent A ON x.fhAgent1 = A.AID
                //                                WHERE
                //                                 XCode = '{0}'", _label);

                #endregion
                string sqltext = string.Format(@"select w.OrderTime as fhDate1,w.Dealers as fhAgent1,w.ExtensionName as fhType1,w.OrderNumbels as fhPaper1,t.AName from W_LabelStorage w join tAgent  t on w.Dealers =t.AID  where QRCode = '{0}';", _label);


                DataTable dt = new DataTable();//Middle.ExceDataTable(sqltext);
                if (dt != null)
                {
                    _result.Append(string.Format("<LabelNumber>{0}</LabelNumber>", _label));
                    _result.Append("<Cheat></Cheat>");
                    _result.Append("<ProductName >电缆</ ProductName>");
                    _result.Append(string.Format("<PackDate></PackDate>"));
                    _result.Append(string.Format("<PackBatch></PackBatch>"));
                    _result.Append(string.Format("<fhProductid>{0}</fhProductid>", dt.Rows[0]["fhAgent1"].ToString().Trim()));
                    _result.Append(string.Format("<fhDate1>{0}</fhDate1>", dt.Rows[0]["fhDate1"].ToString().Trim()));
                    _result.Append(string.Format("<fhNumbel>{0}</fhNumbel>", dt.Rows[0]["fhPaper1"].ToString().Trim()));
                    _result.Append(string.Format("<fhType1>{0}</fhType1>", dt.Rows[0]["fhType1"].ToString().Trim()));
                    _result.Append(string.Format("<Content></Content>"));
                    _result.Append(string.Format("<Cheat></Cheat>"));
                    _result.Append(string.Format("<First>{0}|{1}|{2}|{3}</First>", "", "", dt.Rows[0]["AName"].ToString().Trim(), dt.Rows[0]["fhDate1"].ToString().Trim()));
                    _result.Append("</Info>");

                }
                else
                {
                    _result.Append(string.Format("<LabelNumber>{0}</LabelNumber>", _label));
                    _result.Append("<Cheat></Cheat>");
                    _result.Append("<ProductName ></ ProductName>");
                    _result.Append("<PackDate></PackDate>");
                    _result.Append("<PackBatch></PackBatch>");
                    _result.Append("<fhProductid></fhProductid>");
                    _result.Append("<fhDate1></fhDate1>");
                    _result.Append("<fhNumbel></fhNumbel>");
                    _result.Append("<fhType1></fhType1>");
                    _result.Append("<Content></Content>");
                    _result.Append("<Cheat></Cheat>");
                    _result.Append(string.Format("<First>{0}|{1}|{2}|{3}</First>", "", "", "", ""));
                    _result.Append("</Info>");

                }
            }
            catch (Exception ex)
            {
                _result.Append(ex.Message);
            }
            return _result.ToString();
        }

    }
}
