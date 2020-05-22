using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PPK_Logistics.DataSync
{
    internal class SqlCommand_BLL
    {
        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="_dataInfo"></param>
        /// <returns></returns>
        public static DataSet ReturnExecuteDataSet(DataInfo _dataInfo)
        {
            return SqlCommand_L.ExecuteDataSet(_dataInfo.ConnectionStrings, _dataInfo.CommandText, _dataInfo.ConnectType, _dataInfo.Parameters);
        }

        /// <summary>
        /// 返回运行结果
        /// </summary>
        /// <param name="_dataInfo"></param>
        /// <returns></returns>
        public static int ReturnExecuteNonQuery(DataInfo _dataInfo)
        {
            return SqlCommand_L.ExecuteNoneQuery(_dataInfo.ConnectionStrings, _dataInfo.CommandText, _dataInfo.ConnectType, _dataInfo.Parameters);
        }

        /// <summary>
        /// 返回运行结果
        /// </summary>
        /// <param name="_dataInfo"></param>
        /// <returns></returns>
        public static string[] ReturnExecuteNonQueryRef(DataInfo _dataInfo)
        {
            return SqlCommand_L.ExecuteNoneQueryRef(_dataInfo.ConnectionStrings, _dataInfo.CommandText, _dataInfo.ConnectType, _dataInfo.Parameters);
        }

        /// <summary>
        /// 返回数据读取器
        /// </summary>
        /// <param name="_dataInfo"></param>
        /// <returns></returns>
        public static SqlDataReader ReturnExecuteReader(DataInfo _dataInfo)
        {
            return SqlCommand_L.ExecuteReader(_dataInfo.ConnectionStrings, _dataInfo.CommandText, _dataInfo.ConnectType, _dataInfo.Parameters);
        }

        /// <summary>
        /// 返回读取列
        /// </summary>
        /// <param name="_dataInfo"></param>
        /// <returns></returns>
        public static object ReturnExecuteScalar(DataInfo _dataInfo)
        {
            return SqlCommand_L.ExecuteScalar(_dataInfo.ConnectionStrings, _dataInfo.CommandText, _dataInfo.ConnectType, _dataInfo.Parameters);
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="_dataInfo"></param>
        /// <returns></returns>
        public static bool TestConnectionState(DataInfo _dataInfo)
        {
            return SqlCommand_L.ConnectionStation(_dataInfo.ConnectionStrings, _dataInfo.ConnectType);
        }
    }
}