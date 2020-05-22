using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using PPK_Logistics.Config;
using PPK_Logistics.DataSource;

namespace PPK_Logistics
{
    public class OracleDB
    {
        public static string Oracleloginstr = System.Configuration.ConfigurationManager.AppSettings["OracleConnString"].ToString();

        public OracleDB()
        {
        }

        /// <summary>
        /// 获取产品包装单
        /// </summary>
        /// <param name="companyid"></param>
        public DataTable GetPackNumber(string companyid)
        {
            DataSet dst = new DataSet();
            try
            {
                OracleConnection _conn = new OracleConnection(Oracleloginstr);
                if (_conn.State == ConnectionState.Closed)
                    _conn.Open();
                string Commandtext = string.Format("select t.*,p.productname,s.storagename,p.productnorms from WL_PACK_{0} t join Wl_Product_{0} p on t.product=p.id inner join wl_storage_{0} s on t.storehouse=s.id ", companyid);
                using (OracleCommand _cmd = new OracleCommand(Commandtext, _conn))
                {
                    _cmd.ExecuteReader();
                    using (OracleDataAdapter odater = new OracleDataAdapter(_cmd))
                    {
                        odater.Fill(dst);
                    }
                }
                _conn.Dispose();
                _conn.Close();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            return dst.Tables[0];
        }

        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <param name="productnumer"></param>
        /// <returns></returns>
        public DataSet GetProduct(string productnumer)
        {
            //DataTable table = new DataTable();
            DataSet dst = new DataSet();
            try
            {
                OracleConnection _conn = new OracleConnection(Oracleloginstr);
                if (_conn.State == ConnectionState.Closed)
                    _conn.Open();
                string Commandtext = string.Format("select t.productname,t.productnumber,t.productnorms,t.explanation from WL_PRODUCT_{0} t", productnumer);
                using (OracleCommand _cmd = new OracleCommand(Commandtext, _conn))
                {
                    _cmd.ExecuteReader();
                    using (OracleDataAdapter odater = new OracleDataAdapter(_cmd))
                    {
                        odater.Fill(dst);
                    }
                }
                _conn.Dispose();
                _conn.Close();
            }
            catch (System.Exception ex)
            {
            }
            return dst;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool GetUserInfo(string userid, string password)
        {
            bool _uinfo = false;
            try
            {
                OracleConnection _conn = new OracleConnection(Oracleloginstr);
                if (_conn.State == ConnectionState.Closed)
                    _conn.Open();

                string Commandtext = string.Format("select t.userid, u.username,a.agename,a.companyid from WL_USER t join WL_USERINFO u on u.userid=t.id  join WL_AGENT a on t.agent=a.id  where t.userid='{0}' AND t.PASSWORD='{1}'", userid, password);
                using (OracleCommand _cmd = new OracleCommand(Commandtext, _conn))
                {
                    using (OracleDataReader _rd = _cmd.ExecuteReader())
                    {
                        _rd.Read();
                        if (_rd.HasRows)
                        {
                            App.UserReadonly.UserId = _rd["userid"].ToString();
                            App.UserReadonly.UserName = _rd["username"].ToString();
                            App.UserReadonly.UserToAge = _rd["agename"].ToString();
                            App.UserReadonly.UserNunber = _rd["companyid"].ToString();
                            _uinfo = true;
                        }
                    }
                }
                _conn.Dispose();
                _conn.Close();
            }
            finally
            {
            }
            return _uinfo;
        }

        public bool OracleDBInsert(List<PackData> item, string pici, string companyid)
        {
            bool _return = false;
            OracleTransaction trans = null;
            OracleConnection _conn = new OracleConnection(Oracleloginstr);
            int k = 0;
            try
            {
                if (_conn.State == ConnectionState.Closed)
                    _conn.Open();
                trans = _conn.BeginTransaction();
                // string Commandtext = string.Format("select t.productname,t.productnumber,t.productnorms,t.explanation from WL_PRODUCT_{0} t");
                using (OracleCommand _cmd = new OracleCommand())
                {
                    _cmd.Connection = _conn;
                    _cmd.Transaction = trans;
                    for (k = 0; k < item.Count; k++)
                    {
                        switch (Tools.Norms)
                        {
                            case 2:
                                _cmd.CommandText = string.Format("insert  into WL_INPACK_{0}(id,LABELINFO2,LABELINFO3,DATENOW,PACK) values(WL_INPACK_{0}_SEQUENCE.nextval,'{1}','{2}',sysdate,'{3}')", companyid, item[k].RankOne, item[k].RankTwo, pici);
                                break;

                            case 3:

                                _cmd.CommandText = string.Format("insert  into WL_INPACK_{0}(id,LABELINFO1,LABELINFO2,LABELINFO3,DATENOW,PACK) values(WL_INPACK_{0}_SEQUENCE.nextval,'{1}','{2}','{3}',sysdate,'{4}')", companyid, item[k].RankOne, item[k].RankTwo, item[k].RankThree, pici);
                                break;
                        }
                        int odr = _cmd.ExecuteNonQuery();
                        if (odr > 0)
                        {
                            _return = true;
                            //成功
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Tools.Errorint = k;
                trans.Rollback();
                _return = false;
            }
            if (_return)
            {
                trans.Commit();
            }
            _conn.Close();
            return _return;
        }

        public void OracleDBUpdata()
        {
        }
    }
}