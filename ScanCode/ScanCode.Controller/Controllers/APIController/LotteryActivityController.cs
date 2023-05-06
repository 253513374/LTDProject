using Microsoft.AspNetCore.Mvc;
using ScanCode.Model.ResponseModel;

using ScanCode.Mvc.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScanCode.Web.Api.Controllers
{
    /// <summary>
    /// 获取抽奖活动信息.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class LotteryActivityController : BaseController<LotteryActivityController>
    {
        private readonly LotteryService _lotteryService;

        private readonly ILogger<LotteryActivityController> _logger;

        public LotteryActivityController(LotteryService lotteryService,
            ILogger<LotteryActivityController> logger) : base(logger)
        {
            _lotteryService = lotteryService;
            _logger = logger;
        }

        /// <summary>
        /// 获取抽奖活动(活动奖品)信息。
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <returns>返回ActivityViewModel JSON对象</returns>
        [HttpGet]
        public async Task<ApiResponse<ActivityResult>> GetLotteryActivity(string qrcode = "")
        {
            ActivityResult result = await _lotteryService.GetLotteryActivityAsync();

            if (result.IsSuccess)
            {
                return Success(result);
            }

            return Failure<ActivityResult>($"{result.Msg}");
        }
    }
}