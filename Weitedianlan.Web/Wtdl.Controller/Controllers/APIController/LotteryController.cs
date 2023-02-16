using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public LotteryController(LotteryService lotteryService)
        {
            _lotteryService = lotteryService;
        }

        /// <summary>
        /// 用于抽奖接口
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="code">防伪标签序号</param>
        /// <param name="prizennumber">奖品编号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<LotteryResult> Get(string openid, string code, string prizennumber)
        {
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(code) || string.IsNullOrEmpty(prizennumber))
            {
                return new LotteryResult
                {
                    IsSuccess = false,
                    Message = "参数错误。"
                };
            }
            return await _lotteryService.GetLotteryResultAsync(openid, code, prizennumber);
        }
    }
}