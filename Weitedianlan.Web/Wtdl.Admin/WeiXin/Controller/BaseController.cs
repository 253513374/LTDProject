//using Microsoft.AspNetCore.Mvc;
//using Senparc.Weixin;
//using Senparc.Weixin.Entities;

//namespace Wtdl.Admin.WeiXin.Controller
//{
//    public class BaseController : ControllerBase
//    {
//        protected string AppId
//        {
//            get
//            {
//                return Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
//            }
//        }

//        protected static ISenparcWeixinSettingForMP MpSetting
//        {
//            get
//            {
//                return Config.SenparcWeixinSetting.MpSetting;
//            }
//        }
//    }
//}