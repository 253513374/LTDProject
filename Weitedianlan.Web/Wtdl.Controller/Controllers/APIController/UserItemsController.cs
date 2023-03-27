using Microsoft.AspNetCore.Mvc;
using Wtdl.Controller.Services;
using Wtdl.Model.ResponseModel;

namespace Wtdl.Controller.Controllers.APIController
{
    [Route("[controller]")]
    [ApiController]
    public class UserItemsController : ControllerBase
    {
        private readonly UserItemsService _userItemsService;

        public UserItemsController(UserItemsService service)
        {
            _userItemsService = service;
        }

        /// <summary>
        /// 返回用户抽奖记录与领取红包记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [HttpPost("GetUserItems")]
        public async Task<UserItemsRecordResult> GetLotteryInfo(string openid)
        {
            //var result
            return await _userItemsService.GetUserLotteryinfo(openid);
        }
    }
}