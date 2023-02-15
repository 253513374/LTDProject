using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weitedianlan.Model.Entity;
using Wtdl.Mvc.Models;
using Wtdl.Mvc.Models.ResponseModel;
using Wtdl.Mvc.Services;

namespace Wtdl.Mvc.Controllers.APIController
{
    /// <summary>
    /// 扫码查询：溯源、防伪信息
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class SanByQRCodeController : ControllerBase
    {
        private readonly SearchByCodeService _searchByCodeService;

        public SanByQRCodeController(SearchByCodeService codeService)
        {
            _searchByCodeService = codeService;
        }

        /// <summary>
        /// 返回查询防伪数据
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <returns></returns>
        [HttpGet("AntiFake")]
        public async Task<AntiFakeResult> GetSearchByCodeAsync(string qrcode)
        {
            try
            {
                var s = await _searchByCodeService.GetSearchByCodeAsync(qrcode);

                return new AntiFakeResult()
                {
                    IsSuccess = true,
                    Message = "",
                    AntiFakeByData = s,
                };
            }
            catch (Exception e)
            {
                return new AntiFakeResult()
                {
                    IsSuccess = false,
                    Message = e.Message,
                    AntiFakeByData = null,
                };
            }
        }

        /// <summary>
        /// 返回查询标签溯源信息
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <returns>
        /// </returns>
        [HttpGet("Traceability")]
        public async Task<TraceabilityResult> Get(string qrcode)
        {
            if (string.IsNullOrEmpty(qrcode))
            {
                return new TraceabilityResult() { Status = false, Msg = "查询标签序号不能为空" };
            }

            qrcode = qrcode.Trim();
            return await _searchByCodeService.GetWLabelStorageAsync(qrcode);
        }
    }
}