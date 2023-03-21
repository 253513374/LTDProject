<%@ WebHandler Language="C#" Class="Hongbao2" %>

using System;
using System.Web;
using System.Web.SessionState;

public class Hongbao2 : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        if (context.Request["ctype"] != null && context.Request["ctype"].ToString() != "")
        {

            #region 查询红包
            if (context.Request["ctype"].ToString() == "0")
            {
                //查询该码的红包金额
                //判断微信唯一编码
                if (context.Session["openid"] == null)
                {
                    context.Response.Write("{\"meg\":\"0\",\"zt\":\"非法访问！\"}");
                    return;
                }
                //防伪码
                string code = context.Session["t"].ToString();
                //验证码
                string yzm = context.Request["yzm"].ToString();

                string ReStr = Common.APIService.HttpPost("RedPacket/QRCode?openid=" + context.Session["openid"] + "&qrcode=" + code + "&captcha=" + yzm, "");
                if (!string.IsNullOrEmpty(ReStr))
                {
                    var obj = Newtonsoft.Json.Linq.JObject.Parse(ReStr);
                    if (obj["isSuccess"].ToString().ToLower() == "true")
                    {
                        //第一次扫描，可以领取红包
                        context.Response.Write("{\"meg\":\"1\",\"zt\":\"" + obj["code"].ToString() + "\"}");
                        return;
                    }
                    else
                    {
                        //标签还没有扫码出库，无法参与活动
                        context.Response.Write("{\"meg\":\"0\",\"zt\":\"" + obj["message"].ToString() + "\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"meg\":\"0\",\"zt\":\"服务器不能访问！\"}");
                    return;
                }


            }
            #endregion



            #region 发送红包
            if (context.Request["ctype"].ToString() == "1")
            {

                //判断微信唯一编码
                if (context.Session["openid"] == null)
                {
                    context.Response.Write("{\"meg\":\"0\",\"zt\":\"非法访问！\"}");
                    return;
                }
                //防伪码
                string code = context.Session["t"].ToString();
                string yzm = context.Request["yzm"].ToString();

                string ReStr = Common.APIService.HttpPost("RedPacket/QRCode?openid=" + context.Session["openid"] + "&qrcode=" + code + "&captcha=" + yzm, "");
                
                if (!string.IsNullOrEmpty(ReStr))
                {
                    var obj = Newtonsoft.Json.Linq.JObject.Parse(ReStr);
                    if (obj["isSuccess"].ToString().ToLower() == "true")
                    {
                        Common.Pay.TenPay TenPay = new Common.Pay.TenPay();
                        if (TenPay.GetJsApiParameters(context.Session["Openid"].ToString(), Convert.ToInt32(obj["code"].ToString())))
                        {
                            //发放红包成功

                            context.Response.Write("{\"meg\":\"1\",\"zt\":\"成功！\"}");
                            return;
                        }
                        else
                        {
                            //发放红包失败
                            context.Response.Write("{\"meg\":\"0\",\"zt\":\"系统繁忙！\"}");
                            return;
                        }
                    }
                }


            }
            #endregion


        }
        else
        {
            context.Response.Write("{\"meg\":\"0\",\"zt\":\"非法访问！\"}");
            return;
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}