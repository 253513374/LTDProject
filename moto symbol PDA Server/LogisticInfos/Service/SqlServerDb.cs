using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BackEndServer.Service
{
    public class SqlServerDb
    {
        //数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.
        private static readonly string con = ConfigurationManager.AppSettings["ConnectionString"];

        public SqlServerDb()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        public static DataSet ExceDataSet(string sql)
        {
            DataSet DS = new DataSet();
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                {
                    try
                    {
                        da.Fill(DS);
                    }
                    catch (Exception ex)
                    {


                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                return DS;
            }
        }

        public static DataTable ExceDataTable(string sql)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                {
                    try
                    {
                        da.Fill(dt);
                    }
                    catch
                    {
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                return dt;
            }
        }

        /// <summary>
        /// 执行SQL语句（Add,UpDate,Del）
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExceNoQuery(string sql)
        {
            bool flag = false;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    try
                    {
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    catch
                    {
                        flag = false;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                return flag;
            }
        }

        public static object ExceScalar(string sql)
        {
            object flag = null;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    try
                    {
                        flag = cmd.ExecuteScalar();
                    }
                    catch (Exception exs)
                    {
                        flag = null;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return flag;
        }
    }
}
