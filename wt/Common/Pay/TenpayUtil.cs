using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Pay
{
    public class TenpayUtil
    {

        public static string getNoncestr()
        {
            Random random = new Random();

            return MD5Utils.getMD5(random.Next(1000).ToString()).ToLower().Replace("s", "S");

        }

        /// <summary>
        /// 唯一标识码
        /// </summary>
        /// <returns></returns>
        public static string GuidNO()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
