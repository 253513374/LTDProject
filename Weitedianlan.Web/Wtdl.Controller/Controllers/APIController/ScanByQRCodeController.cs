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
    public class ScanByQRCodeController : ControllerBase
    {
        private readonly SearchByCodeService _searchByCodeService;

        private ILogger<ScanByQRCodeController> _logger;

        public ScanByQRCodeController(SearchByCodeService codeService, ILogger<ScanByQRCodeController> logger)
        {
            _searchByCodeService = codeService;
            _logger = logger;
        }

        /// <summary>
        /// 返回查询防伪数据
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <returns></returns>
        [HttpGet("AntiFake")]
        [ResponseCache(Duration = 300, VaryByQueryKeys = new string[] { "qrcode" })]
        public async Task<AntiFakeResult> GetSearchByCodeAsync(string qrcode)
        {
            _logger.LogInformation("查询防伪数据");
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

        /// <summary>
        /// 返回查询标签溯源信息
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <returns>
        /// </returns>
        [ResponseCache(Duration = 300, VaryByQueryKeys = new string[] { "qrcode" })]
        [HttpGet("Traceability")]
        public async Task<TraceabilityResult> Get(string qrcode)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // your function code here

            if (string.IsNullOrEmpty(qrcode))
            {
                return new TraceabilityResult() { Status = false, Msg = "查询标签序号不能为空" };
            }

            qrcode = qrcode.Trim();
            var result = await _searchByCodeService.GetWLabelStorageAsync(qrcode);

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            _logger.LogInformation("溯源信息查询时间: {0}.{1:000} 秒",
                ts.Seconds, ts.Milliseconds);

            return result;
        }
    }
}