using Microsoft.AspNetCore.Mvc;
using ScanCode.Model.ResponseModel;

using ScanCode.Mvc.Services;

namespace ScanCode.Mvc.Controllers.APIController
{
    /// <summary>
    /// 红包发放接口
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class RedPacketController : BaseController<RedPacketController>
    {
        private ScanByRedPacketService _scanByRedPacketService;

        private readonly ILogger _logger;

        public RedPacketController(ScanByRedPacketService service, ILogger<RedPacketController> logger) : base(logger)
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
        public async Task<ApiResponse<RedPacketResult>> QRCode(string openid, string qrcode)
        {
            _logger.LogInformation($"收到发放现金红包请求openid{openid}   qrcode:{qrcode}");
            //校验参数
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(qrcode))
            {
                return Failure<RedPacketResult>("请输入正确参数");
            }

            if (qrcode.Length != 12)
            {
                Failure<RedPacketResult>("请输入正确的二维码序号");
            }

            var result = await _scanByRedPacketService.GrantQRCodeRedPackets(openid, qrcode);

            if (result.IsSuccess)
            {
                return Success(result);
            }
            return Failure<RedPacketResult>(result.Message);
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
        public async Task<ApiResponse<RedPacketResult>> Captcha(string openid, string qrcode, string captcha)
        {
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(qrcode) || string.IsNullOrEmpty(captcha))
            {
                return Failure<RedPacketResult>("请输入正确参数");
            }

            if (qrcode.Length != 12)
            {
                Failure<RedPacketResult>("请输入正确的二维码序号");
            }
            var result = await _scanByRedPacketService.GrantCaptchaRedPackets(openid, qrcode, captcha);
            if (result.IsSuccess)
            {
                return Success(result);
            }
            return Failure<RedPacketResult>(result.Message);
        }

        /// <summary>
        /// 返回用户的现金红包领取状态,
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="qrcode">标签序号</param>
        /// <returns></returns>
        [HttpGet("RedPackStatus")]
        public async Task<ApiResponse<RedStatusResult>> Get(string openid, string qrcode, string ordernumbels)
        {
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(qrcode))
            {
                return Failure<RedStatusResult>("请输入正确参数");
            }
            var result = await _scanByRedPacketService.GetRedStatusResultAsync(openid, qrcode, ordernumbels);
            if (result.IsSuccess)
            {
                return Success(result);
            }
            return Failure<RedStatusResult>(result.Message, result);
        }
    }
}