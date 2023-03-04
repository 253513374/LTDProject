using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Senparc.Weixin.TenPay.V2;
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

        //  private HubConnection _hubConnection;

        public LotteryController(LotteryService lotteryService)
        {
            _lotteryService = lotteryService;
            // _hubConnection = connection;
        }

        /// <summary>
        /// 用于抽奖接口
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="qrcode">防伪标签序号</param>
        /// <param name="prizennumber">奖品编号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<LotteryResult> Get(string openid, string qrcode, string prizennumber)
        {
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
            return await _lotteryService.GetLotteryResultAsync(openid, qrcode, prizennumber);
        }
    }
}