using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PPK_Logistics.DataSync
{
    internal class SqlCommand_L
    {
        #region 数据库连接状态

        /// <summary>
        /// 测试数据库连接状态
        /// </summary>
        /// <param name="_connStr">连接值</param>
        /// <param name="_connType">数据库类型</param>
        /// <returns></returns>
        public static bool ConnectionStation(string _connStr, string _connType)
        {
            try
            {
                switch (_connType)
                {
                    case "SQL":
                        using (SqlConnection _conn = new SqlConnection(_connStr))
                        {
                            if (_conn.State == ConnectionState.Closed)
                            {
                                _conn.Open();
                            }
                        }
                        break;
                        /*case "Oracle":
                            using (OracleConnection _conn = new OracleConnection(_connStr))
                            {
                                if (_conn.State == ConnectionState.Closed)
                                {
                                    _conn.Open();
                                }
                            }
                            break;*/
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion 数据库连接状态

        #region 装载数据运行

        /// <summary>
        /// 装载数据运行
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="cmdText"></param>
        /// <param name="paras"></param>
        private static void PrepareCommand(IDbConnection conn, IDbCommand cmd, string cmdText, params SqlParameter[] paras)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 3600;

            if (paras != null)
            {
                foreach (IDataParameter para in paras)
                    cmd.Parameters.Add(para);
            }
        }

        #endregion 装载数据运行

        #region 返回数据行数

        /// <summary>
        /// 返回数据行数
        /// </summary>
        /// <param name="_connStr"></param>
        /// <param name="_cmdText"></param>
        /// <param name="_conType"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static int ExecuteNoneQuery(string _connStr, string _cmdText, string _conType, params SqlParameter[] paras)
        {
            try
            {
                int i = 0;
                switch (_conType)
                {
                    case "SQL":
                        using (SqlConnection _conn = new SqlConnection(_connStr))
                        {
                            SqlCommand _cmd = new SqlCommand();
                            PrepareCommand(_conn, _cmd, _cmdText, paras);
                            i = _cmd.ExecuteNonQuery();
                        }
                        break;
                        /*case "Oracle":
                            using (OracleConnection _conn = new OracleConnection(_connStr))
                            {
                                OracleCommand _cmd = new OracleCommand();
                                PrepareCommand(_conn, _cmd, _cmdText, paras);
                                i = _cmd.ExecuteNonQuery();
                            }
                            break;*/
                }
                return i;
            }
            catch
            {
                return -1;
            }
        }

        #endregion 返回数据行数

        #region 返回数据行数

        /// <summary>
        /// 返回数据行数
        /// </summary>
        /// <param name="_connStr"></param>
        /// <param name="_cmdText"></param>
        /// <param name="_conType"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static string[] ExecuteNoneQueryRef(string _connStr, string _cmdText, string _conType, params SqlParameter[] paras)
        {
            string[] Err = new string[2];
            try
            {
                int i = 0;
                switch (_conType)
                {
                    case "SQL":
                        using (SqlConnection _conn = new SqlConnection(_connStr))
                        {
                            SqlCommand _cmd = new SqlCommand();
                            PrepareCommand(_conn, _cmd, _cmdText, paras);
                            i = _cmd.ExecuteNonQuery();
                        }
                        break;
                        /*case "Oracle":
                            using (OracleConnection _conn = new OracleConnection(_connStr))
                            {
                                OracleCommand _cmd = new OracleCommand();
                                PrepareCommand(_conn, _cmd, _cmdText, paras);
                                i = _cmd.ExecuteNonQuery();
                            }
                            break;*/
                }
                Err[0] = i.ToString();
                Err[1] = "";

                return Err;
            }
            catch (Exception ex)
            {
                Err[0] = "0".ToString();
                Err[1] = ex.ToString();
                return Err;
            }
        }

        #endregion 返回数据行数

        #region 返回数据读取器

        /// <summary>
        /// 返回数据读取器
        /// </summary>
        /// <param name="_connStr"></param>
        /// <param name="_cmdText"></param>
        /// <param name="_conType"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string _connStr, string _cmdText, string _conType, params SqlParameter[] paras)
        {
            SqlDataReader _rd = null;
            switch (_conType)
            {
                case "SQL":
                    using (SqlConnection _conn = new SqlConnection(_connStr))
                    {
                        using (SqlCommand _cmd = new SqlCommand())
                        {
                            PrepareCommand(_conn, _cmd, _cmdText, paras);
                            _rd = _cmd.ExecuteReader();
                        }
                    }
                    break;
                    /*case "Oracle":
                        using (OracleConnection _conn = new OracleConnection(_connStr))
                        {
                            OracleCommand _cmd = new OracleCommand();
                            PrepareCommand(_conn, _cmd, _cmdText, paras);
                            _rd = _cmd.ExecuteReader();
                        }
                        break;*/
            }
            return _rd;
        }

        #endregion 返回数据读取器

        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="_connStr"></param>
        /// <param name="_cmdText"></param>
        /// <param name="_conType"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string _connStr, string _cmdText, string _conType, params SqlParameter[] paras)
        {
            try
            {
                DataSet _ds = new DataSet();

                using (SqlConnection _conn = new SqlConnection(_connStr))
                {
                    SqlCommand _cmd = new SqlCommand();

                    PrepareCommand(_conn, _cmd, _cmdText, paras);
                    SqlDataAdapter _ada = new SqlDataAdapter();
                    _ada.SelectCommand = _cmd;
                    _ada.Fill(_ds, "Table");
                    _ada.Dispose();
                    _conn.Close();
                    _cmd.Dispose();
                }
                return _ds;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 返回首行首列
        /// </summary>
        /// <param name="_connStr"></param>
        /// <param name="_cmdText"></param>
        /// <param name="_conType"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string _connStr, string _cmdText, string _conType, params SqlParameter[] paras)
        {
            try
            {
                object _rd = null;
                switch (_conType)
                {
                    case "SQL":
                        using (SqlConnection _conn = new SqlConnection(_connStr))
                        {
                            using (SqlCommand _cmd = new SqlCommand())
                            {
                                PrepareCommand(_conn, _cmd, _cmdText, paras);
                                _rd = _cmd.ExecuteScalar();
                            }
                        }
                        break;
                        /*case "Oracle":
                            using (OracleConnection _conn = new OracleConnection(_connStr))
                            {
                                OracleCommand _cmd = new OracleCommand();
                                PrepareCommand(_conn, _cmd, _cmdText, paras);
                                _rd = _cmd.ExecuteScalar();
                            }
                            break;*/
                }
                return _rd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}