using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Wtdl.Mvc.Models;
using Wtdl.Mvc.Services;
using Wtdl.Repository;

namespace Wtdl.Mvc.Controllers.APIController
{
    /// <summary>
    /// 标签溯源信息接口
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class TraceabilityController : ControllerBase
    {
        private SearchByCodeService _searchByCodeService;

        public TraceabilityController(SearchByCodeService service)
        {
            _searchByCodeService = service;
        }

        /// <summary>
        /// 查询标签溯源信息
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <returns>
        ///
        /// </returns>
        [HttpGet]
        public async Task<OutStorageResult> Get(string qrcode)
        {
            if (string.IsNullOrEmpty(qrcode))
            {
                return new OutStorageResult() { Status = false, Msg = "查询标签序号不能为空" };
            }

            qrcode = qrcode.Trim();
            return await _searchByCodeService.GetWLabelStorageAsync(qrcode);
        }
    }
}