using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wtdl.Web.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AntiCounterfeitingController : ControllerBase
    {
        public AntiCounterfeitingController()
        {
        }

        /// <summary>
        /// 查询防伪码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetByCode")]
        public async Task<string> Get(string code)
        {
            return code;
        }
    }
}