using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weitedianlan.Model.Entity;
using Wtdl.Mvc.Services;

namespace Wtdl.Mvc.Controllers.APIController
{
    /// <summary>
    /// 防伪查询接口
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
        /// 查询防伪数据
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<SearchByCode> GetSearchByCodeAsync(string qrcode)
        {
            //var searchByCodeService = new SearchByCodeService();
            return await _searchByCodeService.GetSearchByCodeAsync(qrcode);
        }
    }
}