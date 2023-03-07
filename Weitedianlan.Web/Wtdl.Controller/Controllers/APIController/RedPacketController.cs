using Microsoft.AspNetCore.Mvc;
using Wtdl.Controller.Models.ResponseModel;
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
        /// 第一次二维码领取红包
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="qrcode">标签序号</param>
        /// <returns></returns>
        [HttpPost("QRCode")]
        public async Task<RedPacketResult> QRCode(string openid, string qrcode)
        {
            return await _scanByRedPacketService.GrantQRCodeRedPackets(openid, qrcode);
            //  return Ok(result);
        }

        /// <summary>
        /// 第二次输入验证码领取红包
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="qrcode">标签序号</param>
        /// <param name="captcha">验证码</param>
        /// <returns></returns>
        [HttpPost("Captcha")]
        public async Task<RedPacketResult> Captcha(string openid, string qrcode, string captcha)
        {
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(qrcode) || string.IsNullOrEmpty(captcha))
            {
                return new RedPacketResult()
                {
                    Code = 400,
                    Message = "参数错误,请检查参数。"
                };
            }

            if (qrcode.Length != 12)
            {
                return new RedPacketResult()
                {
                    Code = 400,
                    Message = "参数错误,请检查参数。"
                };
            }
            return await _scanByRedPacketService.GrantCaptchaRedPackets(openid, qrcode, captcha);
            // return Ok(result);
        }

        /// <summary>
        /// 返回用户的现金红包领取状态,
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="qrcode">标签序号</param>
        /// <returns></returns>
        [HttpGet("RedPackStatus")]
        public async Task<RedStatusResult> Get(string openid, string qrcode)
        {
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(qrcode))
            {
                return new RedStatusResult
                {
                    IsSuccess = false,
                    Message = "参数错误,请检查参数。",
                    StuteCode = "NOT"
                };
            }
            return await _scanByRedPacketService.AnyFirstRedPacket(qrcode);
            // return Ok(new RedPacketResult());
        }
    }
}