using Hprose.Client;
using PPK_Logistics.DataSource;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.IO;
using PPK_Logistics.ERP.Interface;

namespace PPK_Logistics.DataSync
{
    public class Data_Sync
    {
        private HproseHttpClient hpclient;

        public Data_Sync(string url)
        {
            hpclient = new HproseHttpClient(url);
            hpclient.KeepAlive = true;
            hpclient.Timeout = 300000;
        }

        public string HproseDataDelete(string code)
        {
            string returnStaus = "";
            try
            {
                returnStaus = "";//new PDAServer().MarketReturn2(new PackDataDelete(code, App.UserReadonly.UserId));// hpclient.Invoke<string>("DeletePackOne", new Object[] { code, App.UserReadonly.UserNunber });
            }
            catch (Exception ee)
            {
                returnStaus = ee.Message.ToString();
            }
            return returnStaus;
        }

        /// <summary>
        /// 下载数据到客户端
        /// </summary>
        /// <param name="MethodsName"></param>
        /// <returns></returns>
        public List<OutboundOrder> HproseDownDataDBCK()
        {
            List<OutboundOrder> OutboundOrderList = new List<OutboundOrder>();
            try
            {
                DataTable DataTable = new PDAServer().GetOutOrderDBCK();
                if (DataTable != null && DataTable.Rows.Count > 0)
                {
                    //public OutboundOrder(string KCSWZ_SWKCBH, string KCSWZ_RCKDH, string KCSWZ_BCCK, string KCSWZ_BRCK, string KCSWZ_JZRQ, string KCSWZMX_FZCKSL, string KCSWZMX_SFSL)
                    foreach (DataRow row in DataTable.Rows)
                    {
                        //OutboundOrderList.Add(new OutboundOrder(row["KCSWZ_SWKCBH"].ToString(), row["KCSWZ_KHID"].ToString(), row["KCSWZ_JZRQ"].ToString(), row["KH_MC"].ToString(), row["KCSWZMX_FZCKSL"].ToString(), row["yfsl"].ToString()));

                        OutboundOrderList.Add(new OutboundOrder(row["KCSWZ_SWKCBH"].ToString(),row["KCSWZ_RCKDH"].ToString(),row["BCCK"].ToString(),row["BRCK"].ToString(),row["KCSWZ_JZRQ"].ToString(),row["KCSWZMX_FZCKSL"].ToString(),row["yfsl"].ToString(),row["KCSWZ_SWLX"].ToString(),row["KCSWZ_DFCK"].ToString(),row["WL_FJLDW"].ToString().Trim(), row["Comment"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                OutboundOrder _product = new OutboundOrder();
                _product.KH_MC1 = ex.Message;
                OutboundOrderList.Add(_product);
            }
            return OutboundOrderList;
        }

    
        /// <summary>
        /// 加载单号数据
        /// </summary>
        /// <returns></returns>
        public List<OutboundOrder> HproseDownDataOrder()
        {
            List<OutboundOrder> OutboundOrderList = new List<OutboundOrder>();
            try
            {
                //DataTable DataTable = new PDAServer().GetOutboundOrder();
                DataTable DataTable=new GJPERPinterface().GetOutboundOrder();
                if (DataTable!=null&&DataTable.Rows.Count>0)
                {
                    foreach (DataRow row in DataTable.Rows)
                    {
                        OutboundOrderList.Add(new OutboundOrder(row["KCSWZMX_SWKCBH"].ToString(), row["KCSWZ_KHID"].ToString(), row["KCSWZMX_JZRQ"].ToString(), row["KH_MC"].ToString(), row["KCSWZMX_FZCKSL"].ToString(), row["yfsl"].ToString(), row["KCSWZ_SWLX"].ToString(), row["WL_FJLDW"].ToString().Trim(), row["Comment"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                OutboundOrder _product = new OutboundOrder();
                _product.KH_MC1 = ex.Message;
                OutboundOrderList.Add(_product);
            }
            return OutboundOrderList;
        }

        /// <summary>
        /// 同步数据到服务器端（上传）
        /// </summary>
        /// <param name="MethodsName"></param>
        /// <returns></returns>
        public string HproseSendData(PackData_Simple _PackData)
        {
            PackData_Simple _PackData_Simple = null;
            try
            {
                _PackData_Simple = hpclient.Invoke<PackData_Simple>("PackGoodsAddOne", new Object[] { _PackData });

                if (_PackData_Simple != null)
                {
                    if (_PackData_Simple.Status == "成功")
                    {
                        return _PackData_Simple.LABELLIST;
                    }
                    else
                    {
                        // Tools.PackData_Simpl_Error.Add(_PackData_Simple);
                        return _PackData_Simple.LABELLIST + "ERROR：" + _PackData_Simple.Status;
                    }
                }
                else
                {
                    return "Error_Null";
                }
            }
            catch (System.Exception ex)
            {
                //Tools.PackData_Simpl_Error.Add(_PackData_Simple);
                return _PackData.LABELLIST + "ERROR:" + ex.Message;
            }
            // return "Error";
        }

        public Checkin_ltjyStack HproseSendDataStow(Checkin_ltjyStack _PackData)
        {
            Checkin_ltjyStack _Checkin_ltjyStack = null;
            try
            {
                _Checkin_ltjyStack = hpclient.Invoke<Checkin_ltjyStack>("CheckInD", new Object[] { _PackData, App.UserReadonly.UserNunber, App.UserReadonly.UserToAge });
                return _Checkin_ltjyStack;
            }
            catch (System.Exception ex)
            {
                //Tools.Checkin_ltjyStack_Error.Add(_Checkin_ltjyStack);
                _PackData.Status = "网络超时";
                return _PackData;
            }
        }

        public void HproseTestData()
        {
            try
            {
                string _infp = hpclient.Invoke<string>("labelshow", new Object[] { "110030241112", App.UserReadonly.UserNunber });
            }
            catch (System.Exception ex)
            {
            }
        }

        /// <summary>
        /// 登录账户信息验证
        /// </summary>
        /// <param name="u"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public string UserInfoVerify(string u, string p)
        {
            string ret = "";
            try
            {
                string userinfo = new PDAServer().login(u, p);// hpclient.Invoke<string>("login", new Object[] { u, p, "V2.0_0917" });

                if (userinfo.Contains("|") == true)
                {
                    // Program.UserRight = userinfo;

                    App.UserReadonly.UserId = u;
                    return "1";
                }
                //string userinfo = hpclient.Invoke<string>("login", new Object[] { u, p });
                //string[] uinfo = userinfo.Split('|');
                //if (uinfo[0] == "1")
                //{
                //    string[] info = uinfo[1].Split(',');
                //    App.UserReadonly.UserId = info[0].ToString();
                //    App.UserReadonly.UserName = "";//_rd["username"].ToString();
                //    App.UserReadonly.UserToAge = info[2].ToString();
                //    App.UserReadonly.UserNunber = info[1].ToString();
                //    ret = uinfo[0].ToString();
                //}
                //else
                //{
                //    ret = uinfo[1].ToString();
                //}
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }
            return ret;
        }


    }
}