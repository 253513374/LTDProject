using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wtdl.Mvc.Models;
using Wtdl.Mvc.Services;

namespace Wtdl.Mvc.Controllers.APIController
{
    /// <summary>
    /// 红包领取接口
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class RedPacketController : ControllerBase
    {
        private ScanByRedPacketService _scanByRedPacketService;

        public RedPacketController(ScanByRedPacketService service)
        {
            _scanByRedPacketService = service;
        }

        /// <summary>
        /// 第二次输入验证码领取红包，正在开发
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="qrcode">标签序号</param>
        /// <param name="captcha">验证码</param>
        /// <returns></returns>
        [HttpPost("Captcha")]
        public async Task<IActionResult> Captcha(string openid, string qrcode, string captcha)
        {
            var result = await _scanByRedPacketService.GrantCaptchaRedPackets(openid, qrcode, captcha);
            return Ok(result);
        }

        /// <summary>
        /// 第一次二维码领取红包，正在开发
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="qrcode">标签序号</param>
        /// <returns></returns>
        [HttpPost("QRCode")]
        public async Task<IActionResult> QRCode(string openid, string qrcode)
        {
            var result = await _scanByRedPacketService.GrantQRCodeRedPackets(openid, qrcode);
            return Ok(result);
        }

        /// <summary>
        /// 返回标签序号的现金红包的领取状态，是否可以领取现金红包，是第一次领取现金红包
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="qrcode">标签序号</param>
        /// <returns></returns>
        [HttpGet("Status")]
        public async Task<IActionResult> Get(string openid, string qrcode)
        {
            await _scanByRedPacketService.AnyFirstRedPacket(qrcode);
            return Ok(new RedPacketResult());
        }
    }
}