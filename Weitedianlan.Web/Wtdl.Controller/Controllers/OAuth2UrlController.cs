using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace Wtdl.Controller.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OAuth2UrlController : ControllerBase
    {
        /// <summary>
        /// // 微信公众号的 AppId
        /// </summary>
        private string appId = Config.SenparcWeixinSetting.WeixinAppId;

        /// <summary>
        /// // 微信公众号的 AppSecret
        /// </summary>
        private string appSecret = Config.SenparcWeixinSetting.WeixinAppSecret;

        [HttpGet]
        public async Task<IActionResult> Get(string qrcode)
        {
            var UrlBase =
                OAuthApi.GetAuthorizeUrl(appId,
                    "http://www.chn315.top/",
                    qrcode, OAuthScope.snsapi_base);//snsapi_base方式回调地址

            return Ok(UrlBase);
        }
    }
}