using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.WeiXin
{
    public class BasePage : System.Web.UI.Page
    {
        //公众号信息部分
        public string appid = ConfigurationManager.AppSettings["wxAppid"];
        public string appsecret = ConfigurationManager.AppSettings["wxAppSecret"];
        public string redirect_uri = HttpUtility.UrlEncode(HttpContext.Current.Request.Url.ToString());
        //应用授权作用域:弹出授权页面，可通过openid拿到昵称、性别、所在地
        public string scope = "snsapi_userinfo";
        //重定向后会带上state参数，开发者可以填写a-zA-Z0-9的参数值，最多128字节
        public string state = "STATE";
        public string accesstoken = "";
        public string openid = "";
        protected override void OnInit(EventArgs e)
        {
            if (Session["openid"] != null)
            {
                return;
            }
            //不写日志记录
            //LogHelper.WriteFile("请求链接：" + HttpContext.Current.Request.Url.ToString());
            //微信认证部分：第一步 获得code
            string code = Request["code"];
            if (string.IsNullOrEmpty(code))
            {
                GetOAuthCode();
            }
            //微信认证部分：第二步：通过code换取网页授权access_token
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appid, appsecret, code);
            string result = HttpClientHelper.GetResponse(url);

            //不写日志记录
            //LogHelper.WriteFile("通过Code获取access_token和openid：" + result);


            JObject outputObj = JObject.Parse(result);

            //微信认证部分：第三步 拉取用户信息(需scope为 snsapi_userinfo)
            accesstoken = outputObj["access_token"].ToString();
            openid = outputObj["openid"].ToString();
            url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", accesstoken, openid);
            string result1 = HttpClientHelper.GetResponse(url);

            //不写日志记录
            //LogHelper.WriteFile("通过Code获取access_token和openid获取用户信息：" + result1);


            WeiXinUserInfo objWeiXinUserInfo = result1.JsonToEntity<WeiXinUserInfo>();
            //将获得的openid填入到session中
            Session["openid"] = objWeiXinUserInfo.Openid;
            //将用户信息加入缓存
            DataCache.Insert(objWeiXinUserInfo.Openid, objWeiXinUserInfo);

        }

        /// <summary>
        /// 第一步：用户同意授权，获取code
        /// </summary>
        public void GetOAuthCode()
        {
            //认证第一步：重定向跳转至认证网址
            string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&&response_type=code&scope={2}&state={3}#wechat_redirect", appid, redirect_uri, scope, state);
            Response.Redirect(url);

        }
    }
}
