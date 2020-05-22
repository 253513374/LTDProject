using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PPK_Logistics
{
    public class DataInfo
    {
        /// <summary>
        /// 数据列
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 执行语句
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionStrings { get; set; }

        /// <summary>
        /// 连接类型
        /// </summary>
        public string ConnectType { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        public string DataBase { get; set; }

        /// <summary>
        /// 脚本参数
        /// </summary>
        public SqlParameter[] Parameters { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 连接服务器
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// 数据表
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 登录名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 其他条件
        /// </summary>
        public string WhereOrder { get; set; }
    }
}