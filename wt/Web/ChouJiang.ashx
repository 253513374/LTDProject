<%@ WebHandler Language="C#" Class="ChouJiang" %>

using System;
using System.Web;
using System.Web.SessionState;

public class ChouJiang : IHttpHandler,IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");

        if (context.Request["ctype"] != null && context.Request["ctype"].ToString() != "")
        {
            /*
            #region 获取抽奖活动信息
            if (context.Request["ctype"].ToString() == "0")
            {
                //查询该码的红包金额
                
                //防伪码
                string code = context.Session["t"].ToString();


                string ReStr = Common.APIService.GetFunction("LotteryActivity?qrcode=" + code );
                if (!string.IsNullOrEmpty(ReStr))
                {
                    var obj = Newtonsoft.Json.Linq.JObject.Parse(ReStr);
                    if (obj["isSuccess"].ToString().ToLower() == "true")
                    {
                        if (obj["prizes"].ToString() != "[]")
                        {
                            var prizeinfo = Newtonsoft.Json.Linq.JObject.Parse(obj["prizes"].ToString());
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append("<div class=\"item-group\">");
                            sb.Append("<a href=\"#\" onclick=\"choujiang(" + prizeinfo["id"] + ")\">");
                            sb.Append("<div class=\"img-group\">");
                            sb.Append("<img class=\"goods-img\" src=\"" + prizeinfo["imageUrl"] + "\" alt=\"" + prizeinfo["description"] + "\">");
                            sb.Append("<p style=\" text-align:center;\">");
                            sb.Append(prizeinfo["name"]);
                            sb.Append("</p>");
                            sb.Append("</div>");
                            sb.Append("</a>");
                            sb.Append("</div>");
                            //第一次扫描，可以领取红包
                            context.Response.Write("{\"meg\":\"1\",\"zt\":\"" + sb.ToString() + "\"}");
                            return;
                        }
                        else
                        {
                            //没有抽奖的活动
                            context.Response.Write("{\"meg\":\"0\",\"zt\":\"" + obj["msg"].ToString() + "\"}");
                            return;
                        }
                    }
                    else
                    {
                        //没有抽奖的活动
                        context.Response.Write("{\"meg\":\"0\",\"zt\":\"" + obj["msg"].ToString() + "\"}");
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
            */

            #region 抽奖
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
                //奖品的ID
                string id = context.Request["id"].ToString();

                string ReStr = Common.APIService.GetFunction("Lottery?openid=" + context.Session["openid"] + "&qrcode=" + code + "&prizennumber=" + id);
                if (!string.IsNullOrEmpty(ReStr))
                {
                    var obj = Newtonsoft.Json.Linq.JObject.Parse(ReStr);
                    if (obj["isSuccess"].ToString().ToLower() == "true")
                    {
                        context.Response.Write("{\"meg\":\"1\",\"zt\":\"" + obj["message"].ToString() + "\"}");
                        return;
                    }
                    else
                    {
                        context.Response.Write("{\"meg\":\"0\",\"zt\":\"" + obj["message"].ToString() + "\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"meg\":\"0\",\"zt\":\"非法访问！\"}");
                    return;
                }
                
            }
            #endregion
            
            
            
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}