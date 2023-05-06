using Microsoft.AspNetCore.Mvc;
using ScanCode.Model.ResponseModel;

using ScanCode.Mvc.Services;

namespace ScanCode.Web.Api.Controllers
{
    /// <summary>
    /// 抽奖
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class LotteryController : BaseController<LotteryController>
    {
        private readonly LotteryService _lotteryService;
        //private readonly ILogger<LotteryController> _logger;
        //  private HubConnection _hubConnection;

        public LotteryController(LotteryService lotteryService, ILogger<LotteryController> logger)
            : base(logger)
        {
            _lotteryService = lotteryService;
            // _logger = logger;
            // _hubConnection = connection;
        }

#if DEBUG

        /// <summary>
        /// 开始抽奖接口
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="qrcode">防伪标签序号</param>
        /// <param name="prizennumber">奖品编号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResponse<LotteryResult>> Get(string qrcode, string openid = "oz0TXwTew5RmbnTa2aeMPfHfsDnY",
            string prizennumber = "de5b6cdbdd224a6db1860bd6a2f113b1")
        {
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(qrcode) || string.IsNullOrEmpty(prizennumber))
            {
                return Failure<LotteryResult>("请输入正确参数");
                //return LotteryResult.Fail("参数错误,请检查参数");
            }

            if (qrcode.Trim().Length != 12)
            {
                return Failure<LotteryResult>("不是有效防伪标签序号");
            }

            var result = await _lotteryService.GetLotteryResultAsync(openid, qrcode, prizennumber);

            if (result.IsSuccess)
            {
                return Success(result);
            }

            return Failure<LotteryResult>(result.Message);
        }

#else

        ///<summary>
        /// 开始抽奖接口
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="qrcode">防伪标签序号</param>
        /// <param name="prizennumber">奖品编号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResponse<LotteryResult>> Get(string qrcode, string openid, string prizennumber)
        {
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(qrcode) || string.IsNullOrEmpty(prizennumber))
            {
                return Failure<LotteryResult>("请输入正确参数");
            }

            if (qrcode.Trim().Length != 12)
            {
                return Failure<LotteryResult>("不是有效防伪标签序号");
            }

            var result = await _lotteryService.GetLotteryResultAsync(openid, qrcode, prizennumber);

            if (result.IsSuccess)
            {
              return  Success(result);
            }
            return Failure<LotteryResult>(result.Message);
        }

#endif
    }
}