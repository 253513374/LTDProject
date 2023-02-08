using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wtdl.Mvc.Models;
using Wtdl.Mvc.Services;

namespace Wtdl.Web.Api.Controllers
{
    /// <summary>
    /// 抽奖接口，，
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class LotteryController : ControllerBase
    {
        private readonly LotteryService _lotteryService;

        public LotteryController(LotteryService lotteryService)
        {
            _lotteryService = lotteryService;
        }

        /// <summary>
        /// 用于抽奖接口，正在开发
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="code">防伪标签序号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<LotteryViewModel> Get(string openid, string code)
        {
            return new LotteryViewModel();
        }
    }
}