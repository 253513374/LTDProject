<%@ WebHandler Language="C#" Class="Index" %>

using System;
using System.Web;
using System.Web.SessionState;

public class Index : IHttpHandler,IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");

        if (context.Request["ctype"] != null && context.Request["ctype"].ToString() != "")
        {
            /*
            if(context.Request["ctype"].ToString()=="0")
            {
                string code = context.Request["code"].ToString();
                //查询防伪
                string ReStr =Common. APIService.GetFunction("ScanByQRCode/AntiFake?qrcode="+code);
                if (!string.IsNullOrEmpty(ReStr))
                {
                    var obj = Newtonsoft.Json.Linq.JObject.Parse(ReStr);
                    if (obj["isSuccess"].ToString().ToLower() == "true")
                    {
                        var objcode=Newtonsoft.Json.Linq.JObject.Parse(obj["antiFakeByData"].ToString());
                        if (objcode["results"].ToString().ToUpper()  == "OK" || objcode["results"].ToString().ToUpper()  == "FLOWOFF")
                        {
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.AppendLine("<h3>尊敬的用户：</h3>");
                            sb.AppendLine("<p style=\" text-indent: 2.0em;\">");
                            if (objcode["fibreColor"].ToString().Trim().IndexOf("纤维") > 0)
                            {
                                sb.AppendLine("您所查询的防伪标签照片如下，请观察" + objcode["fibreColor"].ToString().Trim() + "的形状、位置、方向、是否与标签上的是否相符，若相符且能挑出，则是正品的防伪标识。否则谨防假冒!");
                            }
                            else if (objcode["fibreColor"].ToString().Trim().IndexOf("亮片") > 0)
                            {
                                sb.AppendLine("您所查询的防伪标签照片如下，请观察" + objcode["fibreColor"].ToString().Trim() + "的形状、位置、方向是否与标签上的是否相符，若相符且能挑出，则是正品的防伪标识。否则谨防假冒!");
                            }
                            else if (objcode["fibreColor"].ToString().Trim().IndexOf("直条") > 0)
                            {
                                sb.AppendLine("您所查询的防伪标签照片如下，请观察" + objcode["fibreColor"].ToString().Trim() + "的形状、位置、方向是否与标签上的是否相符，若相符且能挑出，则是正品的防伪标识。否则谨防假冒!");
                            }
                            else if (objcode["fibreColor"].ToString().Trim().IndexOf("应码") > 0)
                            {
                                sb.AppendLine("您所查询的防伪标签照片如下，请观察" + objcode["fibreColor"].ToString().Trim() + "竖线内或框内的数码是否相符，若相符且能撕出，则是正品的防伪标识。否则谨防假冒!");
                            }
                            else if (objcode["fibreColor"].ToString().Trim().ToLower().IndexOf("paillette") > 0)
                            {
                                sb.AppendLine("您所查询的防伪标签照片如下，请观察" + objcode["fibreColor"].ToString().Trim() + "的形状、位置、方向是否与标签上的是否相符，若相符且能挑出，则是正品的防伪标识。否则谨防假冒!");
                            }
                            else
                            {
                                sb.AppendLine("您所查询的防伪标签照片如下，请核对" + objcode["fibreColor"].ToString().Trim() + "的形状、位置、方向、是否与标签上的是否相符，若相符且能挑出，则是正品的防伪标识。否则谨防假冒!");
                            }
                            sb.AppendLine("</p>");


                            sb.AppendLine(" <img class=\"bq\" src=\"" + objcode["imgUrl"].ToString().Trim() + "\" width=\"60%\" style=\"margin:10px auto;\"/>");
                            sb.AppendLine("<br>");
                            sb.AppendLine("<div class=\"fw_info\">");
                            sb.AppendLine("<p><span>标签序号：</span><span>" + objcode["labelNum"].ToString() + "</span></p>");
                            sb.AppendLine("<p><span>纤维特征：</span><span>" + objcode["fibreColor"].ToString() + "</span></p>");
                            sb.AppendLine("<p><span>商品名称：</span><span>" + objcode["productName"].ToString() + "</span></p>");

                            sb.AppendLine("<p><span>用户名称：</span><span>" + objcode["corpName"].ToString() + "</span></p>");


                            sb.AppendLine("<p><span>首次查询时间：</span><span>" + objcode["firstQueryTime"].ToString() + "</span></p>");

                            if (objcode["results"].ToString().ToUpper()  == "OK")
                            {
                                //正常查询次数
                                sb.AppendLine("<p><span>查询次数：</span><span>第" + objcode["queryCount"].ToString() + " 次查询</span> </p><br>");

                            }
                            if (objcode["results"].ToString().ToUpper() == "FLOWOFF")
                            {
                                //超次查询次数
                                sb.AppendLine("<p><span>查询次数：</span><span>第" + objcode["queryCount"].ToString() + " 次查询</span> <span style=\"color:#F00;\">超次</span></p><br>");

                            }
                            sb.AppendLine(" <p><span style=\"color: #f40;\">重要提示：</span><span>" + objcode["fibreColor"].ToString().Trim() + "可用针或刀尖挑出。非印刷墨迹，请注意区别。</span></p>");

                            sb.AppendLine("</div>");


                            sb.AppendLine("<div class=\"fw_info\" >");
                            sb.AppendLine("<p>感谢您使用纹理防伪系统查询真伪，纹理防伪一个也假不了。</p>");
                            sb.AppendLine("<p>如有疑问，请拨打电话<a href=\"400-6800-315\"><span style=\"color: #f40;\">400-6800-315</span></a>详细咨询</p>");
                            sb.AppendLine("</div>");
                            context.Response.Write("{\"meg\":\"1\",\"zt\":\"" +System.Web.HttpUtility.UrlEncodeUnicode( sb.ToString()) + "\"}");
                        }
                        else
                        {
                            //超时
                            context.Response.Write("{\"meg\":\"0\",\"zt\":\"接口返回异常\"}");
                        }

                    }
                    else
                    {
                        //访问不通
                        context.Response.Write("{\"meg\":\"0\",\"zt\":\"访问不通\"}");
                    }
                }
                else
                {
                    //访问不通
                    context.Response.Write("{\"meg\":\"0\",\"zt\":\"非法访问\"}");

                }
            }

            //查询溯源
            if (context.Request["ctype"].ToString() == "1")
            {
                string code = context.Request["code"].ToString();
                //查询溯源接口
                string ReStr = Common.APIService.GetFunction("ScanByQRCode/Traceability?qrcode=" + code);
                context.Response.Write(ReStr);
            }

             */
             
            //查询是否第一次领取红包
            if (context.Request["ctype"].ToString() == "2")
            {
                //判断微信唯一编码
                if (context.Session["openid"] == null)
                {
                    context.Response.Write("{\"meg\":\"0\",\"zt\":\"非法访问！\"}");
                    return;
                }
                //调取是否是第一次领取
                string code = context.Request["code"].ToString();

                string ReStr = Common.APIService.GetFunction("RedPacket/RedPackStatus?openid=" + context.Session["openid"] + "&qrcode=" + code, "");
                if (!string.IsNullOrEmpty(ReStr))
                {
                    var obj = Newtonsoft.Json.Linq.JObject.Parse(ReStr);
                    if (obj["isSuccess"].ToString().ToLower() == "true")
                    {
                        if (obj["stuteCode"].ToString().ToUpper() == "QRCODE")
                        {
                            //第一次扫描微信红包
                            context.Response.Write("{\"meg\":\"1\",\"zt\":\"" + code + "\"}");
                            return;
                            
                        }
                        else if (obj["stuteCode"].ToString().ToUpper() == "CAPTCHA")
                        {
                            //第二次扫描微信红包
                            context.Response.Write("{\"meg\":\"2\",\"zt\":\"" + code + "\"}");
                            return;
                        }
                        else
                        {
                            //标签还没有扫码出库，无法参与活动
                            context.Response.Write("{\"meg\":\"0\",\"zt\":\"活动还没有开始\"}");
                            return;
                        }
                    }
                    else
                    {
                        //标签还没有扫码出库，无法参与活动
                        context.Response.Write("{\"meg\":\"0\",\"zt\":\"活动还没有开始\"}");
                        return;
                    }
                }
                else
                {
                    context.Response.Write("{\"meg\":\"0\",\"zt\":\"服务器不能访问！\"}");
                    return;
                }
            }
            
            //判断是否有抽奖的活动
            if (context.Request["ctype"].ToString() == "3")
            {
                string code = context.Request["code"].ToString();
                //抽奖活动接口
                string ReStr = Common.APIService.GetFunction("LotteryActivity?qrcode=" + code);
                if (!string.IsNullOrEmpty(ReStr))
                {
                    var obj = Newtonsoft.Json.Linq.JObject.Parse(ReStr);
                    if (obj["isSuccess"].ToString().ToLower() == "true")
                    {
                        //第一次扫描，可以领取红包
                        context.Response.Write("{\"meg\":\"1\",\"zt\":\"" + obj["msg"].ToString() + "\"}");
                        return;
                    }
                    else
                    {
                        //标签还没有扫码出库，无法参与活动
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
            

        }


    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}