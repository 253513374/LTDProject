using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Senparc.Weixin.TenPay.V2;
using System.Diagnostics;
using Wtdl.Mvc.Models;
using Wtdl.Mvc.Services;

namespace Wtdl.Web.Api.Controllers
{
    /// <summary>
    /// 抽奖
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class LotteryController : ControllerBase
    {
        private readonly LotteryService _lotteryService;
        private readonly ILogger<LotteryController> _logger;
        //  private HubConnection _hubConnection;

        public LotteryController(LotteryService lotteryService, ILogger<LotteryController> logger)
        {
            _lotteryService = lotteryService;
            _logger = logger;
            // _hubConnection = connection;
        }

        /// <summary>
        /// 开始抽奖接口
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="qrcode">防伪标签序号</param>
        /// <param name="prizennumber">奖品编号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<LotteryResult> Get(string qrcode, string openid = "oz0TXwTew5RmbnTa2aeMPfHfsDnY",
            string prizennumber = "de5b6cdbdd224a6db1860bd6a2f113b1")
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(qrcode) || string.IsNullOrEmpty(prizennumber))
            {
                return new LotteryResult
                {
                    IsSuccess = false,
                    Message = "参数错误。"
                };
            }

            if (qrcode.Trim().Length != 12)
            {
                return new LotteryResult()
                {
                    IsSuccess = false,
                    Message = "参数错误,请检查参数。"
                };
            }
            var result = await _lotteryService.GetLotteryResultAsync(openid, qrcode, prizennumber);

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            _logger.LogInformation("抽奖时间: {0}.{1:000} 秒",
                ts.Seconds, ts.Milliseconds);

            return result;
        }
    }
}