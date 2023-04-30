using Microsoft.AspNetCore.Mvc;
using Wtdl.Model.Entity;
using Wtdl.Model.ResponseModel;
using Wtdl.Repository;

namespace Wtdl.Controller.Controllers.APIController
{
    [Route("[controller]")]
    [ApiController]
    public class UserAwardInfoController : BaseController<UserAwardInfoController>
    {
        private readonly UserAwardInfoRepository _userAwardInfoRepository;
        private readonly ILogger<UserAwardInfoController> _logger;

        public UserAwardInfoController(UserAwardInfoRepository userAwardInfoRepository, ILogger<UserAwardInfoController> logger)
            : base(logger)
        {
            _logger = logger;
            _userAwardInfoRepository = userAwardInfoRepository;
        }

        /// <summary>
        /// 返回用户的领取奖品记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ApiResponse<List<UserAwardInfo>>> GetById(string id)
        {
            var userAwardInfo = await _userAwardInfoRepository.GetByOpenIdAsync(id);

            if (userAwardInfo == null)
            {
                return FailureList<UserAwardInfo>("没有领取奖品记录");
            }

            return SuccessList(userAwardInfo);
        }

        /// <summary>
        /// 添加用户领取奖品记录
        /// </summary>
        /// <param name="userAwardInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse<UserAwardInfo>> Add([FromBody] UserAwardInfo userAwardInfo)
        {
            //var v = await _userAwardInfoRepository.FindSingleAsync(f => f.WeChatOpenId.Contains(userAwardInfo.WeChatOpenId) && f.QrCode.Contains(userAwardInfo.QrCode));
            //if (v is not null)
            //{
            //    userAwardInfo.Id = v.Id;
            //    var upresult = await _userAwardInfoRepository.UpdateAsync(userAwardInfo);
            //    if (upresult > 0)
            //    {
            //        return Success(userAwardInfo, "更新领奖地址记录成功");
            //    }

            //    return Failure<UserAwardInfo>("更新地址添加失败", null, new List<string>() { "数据添加失败" });
            //}

            var result = await _userAwardInfoRepository.AddAsync(userAwardInfo);
            if (result > 0)
            {
                return Success(userAwardInfo, "添加领奖地址记录成功");
            }
            return Failure<UserAwardInfo>("领奖地址添加失败", null, new List<string>() { "数据添加失败" });
        }

        /// <summary>
        /// 更新用户领取奖品的信息（联系方式与联系地址）
        /// </summary>
        /// <param name="userAwardInfo"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ApiResponse<UserAwardInfo>> Update([FromBody] UserAwardInfo userAwardInfo)
        {
            var result = await _userAwardInfoRepository.UpdateAsync(userAwardInfo);

            if (result > 0)
            {
                return Success(userAwardInfo, "更新UserAwardInfo记录");
            }
            return Failure<UserAwardInfo>("用户奖品信息更新失败", null, new List<string>() { "数据添加失败" });
            //  return Failure<UserAwardInfo>(404, "UserAwardInfo updated", new List<string>() { "数据添加失败" });
        }
    }
}