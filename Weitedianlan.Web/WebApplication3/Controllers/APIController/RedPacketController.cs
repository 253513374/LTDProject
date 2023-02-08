using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wtdl.Mvc.Controllers.APIController
{
    /// <summary>
    /// 红包领取接口
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class RedPacketController : ControllerBase
    {
        private RedPacketController()
        {
        }

        /// <summary>
        /// 第一次领取红包，正在开发
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="qrcode">标签序号</param>
        /// <returns></returns>
        [HttpGet]
        public string Get(string openid, string qrcode)
        {
            return "Hello World!";
        }

        /// <summary>
        ///  输入验证码第二次领取红包，正在开发
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="qrcode">标签序号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        [HttpPost]
        public string Post(string openid,string qrcode, string code)
        {
            return "Hello World!";
        }
    }
}