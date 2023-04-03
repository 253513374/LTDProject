using Microsoft.AspNetCore.Mvc;
using Wtdl.Controller.Services;
using Wtdl.Model.ResponseModel;

namespace Wtdl.Controller.Controllers.APIController
{
    [Route("[controller]")]
    [ApiController]
    public class RedPacketRecordController : ControllerBase
    {
        private readonly UserItemsService _userItemsService;

        public RedPacketRecordController(UserItemsService service)
        {
            _userItemsService = service;
        }

        /// <summary>
        /// 返回用户领取红包记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [HttpPost("UserRedPackets")]
        public async Task<UserRedPacketRecordResult> GetRedPacketRecord(string openid)
        {
            //var result
            return await _userItemsService.GetRedPacketRecord(openid);
        }
    }
}