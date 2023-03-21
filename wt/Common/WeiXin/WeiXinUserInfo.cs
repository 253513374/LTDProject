using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.WeiXin
{
    /// <summary>
    /// 微信用户信息
    /// </summary>
    public class WeiXinUserInfo
    {
        /// <summary>
        /// 用户的唯一标识
        /// </summary>
        public string Openid { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 国家，如中国为CN
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Headimgurl { get; set; }

    }
}
