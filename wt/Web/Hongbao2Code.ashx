<%@ WebHandler Language="C#" Class="Hongbao2Code" %>

using System;
using System.Web;
using System.Web.SessionState;

public class Hongbao2Code : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");

        if (context.Request["ctype"] != null && context.Request["ctype"].ToString() != "")
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
                        //有红包

                        context.Response.Write("{\"meg\":\"1\",\"zt\":\"成功！\"}");
                        return;
                    }
                    else
                    {
                        //发放红包失败
                        context.Response.Write("{\"meg\":\"0\",\"zt\":\"" + obj["message"].ToString() + "\"}");
                        return;
                    }
                }
            }
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