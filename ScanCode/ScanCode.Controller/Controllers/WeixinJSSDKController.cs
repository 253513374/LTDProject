using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.MP.Helpers;

namespace ScanCode.Controller.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WeixinJSSDKController : ControllerBase
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
        public async Task<IActionResult> Get()
        {
            var jssdkUiPackage = await JSSDKHelper.GetJsSdkUiPackageAsync(appId, appSecret, Request.AbsoluteUri());
            //   return View(jssdkUiPackage);

            //var nonceStr = JSSDKHelper.GetNoncestr();
            //var timestamp = JSSDKHelper.GetTimestamp();

            //var url = Request.Query["url"].ToString(); // 获取当前页面的 URL

            //var signature = JSSDKHelper.GetSignature(appId, appSecret, nonceStr, timestamp, url); // 获取签名

            //var weixConfig = new
            //{
            //    appId,
            //    timestamp,
            //    nonceStr,
            //    signature
            //};

            return Ok(jssdkUiPackage);
        }
    }
}