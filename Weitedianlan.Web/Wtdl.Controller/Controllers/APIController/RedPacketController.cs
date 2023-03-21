using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Wtdl.Controller.Models.ResponseModel;
using Wtdl.Model.ResponseModel;
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

        private readonly ILogger _logger;

        public RedPacketController(ScanByRedPacketService service, ILogger<RedPacketController> logger)
        {
            _scanByRedPacketService = service;
            _logger = logger;
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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            _logger.LogInformation($"收到发放现金红包请求openid{openid}   qrcode:{qrcode}");
            //校验参数
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(qrcode))
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

            var result = await _scanByRedPacketService.GrantQRCodeRedPackets(openid, qrcode);

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            _logger.LogInformation("发放现金红包时间: {0}.{1:000} 秒",
                ts.Seconds, ts.Milliseconds);

            return result;
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
            _logger.LogInformation($"用户的现金红包领取状态,openid{openid}   qrcode:{qrcode}");
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(qrcode))
            {
                return new RedStatusResult
                {
                    IsSuccess = false,
                    Message = "参数错误,请检查参数。",
                    StuteCode = "NOT"
                };
            }
            return await _scanByRedPacketService.GetRedStatusResultAsync(openid, qrcode);
            // return Ok(new RedPacketResult());
        }
    }
}