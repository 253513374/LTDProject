using Microsoft.AspNetCore.Mvc;
using Wtdl.Mvc.Models;
using Wtdl.Mvc.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Wtdl.Web.Api.Controllers
{
    /// <summary>
    /// 用于获取抽奖活动信息接口。
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class LotteryActivityController : ControllerBase
    {
        // GET: api/<LotteryActivityController>

        private readonly LotteryService _lotteryService;

        private readonly ILogger<LotteryActivityController> _logger;

        public LotteryActivityController(LotteryService lotteryService,
            ILogger<LotteryActivityController> logger)
        {
            _lotteryService = lotteryService;
            _logger = logger;
        }

        /// <summary>
        /// 获取抽奖活动信息以及参与活动的产品信息。
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <returns>返回ActivityViewModel JSON对象</returns>
        [HttpGet]
        public async Task<ActivityViewModel> GetLotteryActivity(string qrcode = "")
        {
            _logger.LogInformation("收到访问信息:LotteryActivityController.GetLotteryActivity");

            return await _lotteryService.GetLotteryActivityAsync();
        }
    }
}