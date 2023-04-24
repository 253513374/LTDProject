using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Wtdl.Controller.Controllers.APIController;
using Wtdl.Model.ResponseModel;

using Wtdl.Mvc.Services;
using Wtdl.RedisCache;

namespace Wtdl.Mvc.Controllers.APIController
{
    /// <summary>
    /// 扫码查询：溯源、防伪信息
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AntiFakeController : BaseController<AntiFakeController>
    {
        private readonly SearchByCodeService _searchByCodeService;

        private ILogger<AntiFakeController> _logger;
        private readonly IRedisCache _redisCache;

        public AntiFakeController(SearchByCodeService codeService,
            IRedisCache redisCache,
            ILogger<AntiFakeController> logger)
        : base(logger)
        {
            _redisCache = redisCache;
            _searchByCodeService = codeService;
            _logger = logger;
        }

        /// <summary>
        /// 返回查询防伪数据
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 300, VaryByQueryKeys = new string[] { "qrcode" })]
        public async Task<ApiResponse<AntiFakeResult>> GetSearchByCodeAsync(string qrcode)
        {
            if (string.IsNullOrEmpty(qrcode))
            {
                return Failure<AntiFakeResult>("无效的防伪码");
            }

            var result = await _searchByCodeService.QueryTag(qrcode);

            return result.IsSuccess
                    ? Success(result, "查询成功")
                    : Failure<AntiFakeResult>(result.Message);
        }
    }
}