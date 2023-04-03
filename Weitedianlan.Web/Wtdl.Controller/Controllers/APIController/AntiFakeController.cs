using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Wtdl.Model.ResponseModel;

using Wtdl.Mvc.Services;

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

        public AntiFakeController(SearchByCodeService codeService, ILogger<AntiFakeController> logger)
        : base(logger)
        {
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
        public async Task<AntiFakeResult> GetSearchByCodeAsync(string qrcode)
        {
            if (string.IsNullOrEmpty(qrcode))
            {
                return new AntiFakeResult
                {
                    IsSuccess = false,
                    Message = "参数错误。"
                };
            }

            return await _searchByCodeService.QueryTag(qrcode);
        }
    }
}