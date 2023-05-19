using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace ScanCode.Controller.Controllers
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
                    "https://www.chn315.top/",
                    qrcode, OAuthScope.snsapi_base);//snsapi_base方式回调地址

            return Ok(UrlBase);
        }

        //[HttpGet("OAuthUrl")]
        //public ActionResult Login(string state)
        //{
        //    // var state = "yourState"; // 防止 CSRF 的随机字符串
        //    var url = OAuthApi.GetAuthorizeUrl(
        //        appId,
        //        "https://www.rewt.cn/Callback",
        //        state,
        //        OAuthScope.snsapi_base);

        //    //直接重定向到微信授权URL
        //    return Redirect(url);
        //}
    }
}