using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Wtdl.Model.ResponseModel;
using Wtdl.Mvc.Controllers.APIController;
using Wtdl.Mvc.Services;

namespace Wtdl.Controller.Controllers.APIController
{
    [Route("[controller]")]
    [ApiController]
    public class TraceabilityController : BaseController<TraceabilityController>
    {
        private readonly SearchByCodeService _searchByCodeService;

        private ILogger<TraceabilityController> _logger;

        public TraceabilityController(SearchByCodeService codeService, ILogger<TraceabilityController> logger)
            : base(logger)
        {
            _searchByCodeService = codeService;
            _logger = logger;
        }

        /// <summary>
        /// 返回查询标签溯源信息
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <returns>
        /// </returns>
        [ResponseCache(Duration = 300, VaryByQueryKeys = new string[] { "qrcode" })]
        [HttpGet]
        public async Task<TraceabilityResult> Get(string qrcode)
        {
            // your function code here
            if (string.IsNullOrEmpty(qrcode))
            {
                return new TraceabilityResult() { Status = false, Msg = "查询标签序号不能为空" };
            }

            qrcode = qrcode.Trim();
            var result = await _searchByCodeService.GetWLabelStorageAsync(qrcode);

            return result;
        }
    }
}