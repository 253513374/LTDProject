using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PubConstant
    {
        /// <summary>
        /// 获取连接字符串网页接口
        /// </summary>
        public static string VirtualPath
        {
            get
            {
                string _connectionString = System.Configuration.ConfigurationManager.AppSettings["VirtualPath"];

                return _connectionString;
            }
        }

        /// <summary>
        /// 奖品图片地址
        /// </summary>
        public static string ImgPath
        {
            get
            {
                string _connectionString = System.Configuration.ConfigurationManager.AppSettings["ImgPath"];

                return _connectionString;
            }
        }


    }
}
