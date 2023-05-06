using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

namespace ScanCode.Controller.Controllers.OAuth
{
    [Route("[controller]")]
    [ApiController]
    public class OAuth2Controller : ControllerBase
    {
        public readonly string appId = Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        private readonly string appSecret = Config.SenparcWeixinSetting.WeixinAppSecret;//与微信公众账号后台的AppId设置保持一致，区分大小写。

        /// <summary>
        /// 微信网页授权回调接口
        /// </summary>
        /// <param name="code">微信授权code</param>
        /// <param name="state">微信授权state</param>
        /// <returns></returns>
        [HttpGet("WXLogin")]
        public async Task<IActionResult> Get(string code)
        {
            OAuthAccessTokenResult result = null;
            try
            {
                result = OAuthApi.GetAccessToken(appId, appSecret, code);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }
            //Console.WriteLine($"code:{code},state:{state}");
            return Ok(result.openid);
        }
    }
}