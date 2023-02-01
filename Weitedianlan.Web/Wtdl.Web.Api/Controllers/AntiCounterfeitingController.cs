using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wtdl.Web.Api.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<string> Get(string code)
        {
            return "value";
        }
    }
}