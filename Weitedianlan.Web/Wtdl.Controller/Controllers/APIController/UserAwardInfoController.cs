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

        public UserAwardInfoController(UserAwardInfoRepository userAwardInfoRepository, ILogger<UserAwardInfoController> logger)
            : base(logger)
        {
            _userAwardInfoRepository = userAwardInfoRepository;
        }

        //通过OpenID获取UserAwardInfo记录
        [HttpGet("{id}")]
        public async Task<ApiResponse<List<UserAwardInfo>>> GetById(string id)
        {
            var userAwardInfo = await _userAwardInfoRepository.GetByOpenIdAsync(id);

            if (userAwardInfo == null)
            {
                return Failure<List<UserAwardInfo>>(StatusCodes.Status404NotFound, "UserAwardInfo not found");
            }

            return Success<List<UserAwardInfo>>("UserAwardInfo found", userAwardInfo);
        }

        // 添加UserAwardInfo记录
        [HttpPost]
        public async Task<ApiResponse<UserAwardInfo>> Add([FromBody] UserAwardInfo userAwardInfo)
        {
            var result = await _userAwardInfoRepository.AddAsync(userAwardInfo);
            if (result > 0)
            {
                return Success<UserAwardInfo>("添加UserAwardInfo记录", userAwardInfo);
            }
            return Failure<UserAwardInfo>(404, "", new List<string>() { "数据添加失败" });
        }

        // 更新UserAwardInfo记录
        [HttpPut("{id}")]
        public async Task<ApiResponse<UserAwardInfo>> Update([FromBody] UserAwardInfo userAwardInfo)
        {
            var result = await _userAwardInfoRepository.UpdateAsync(userAwardInfo);

            if (result > 0)
            {
                return Success<UserAwardInfo>("更新UserAwardInfo记录", userAwardInfo);
            }
            return Failure<UserAwardInfo>(404, "UserAwardInfo updated", new List<string>() { "数据添加失败" });
            //  return Failure<UserAwardInfo>(404, "UserAwardInfo updated", new List<string>() { "数据添加失败" });
        }
    }
}