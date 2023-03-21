using Common.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       // Common.WeiXin.WeiXinUserInfo obWeiXinUserInfo = Common.WeiXin.DataCache.Get<Common.WeiXin.WeiXinUserInfo>(Session["openid"].ToString());
        if (!IsPostBack)
        {
            if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
            {
                //防伪标签
                Session["t"] = Request.Params["id"].Trim();

                //查询防伪
                Fwcx(Request.Params["id"].Trim());

                //产品溯源
                CPSY(Request.Params["id"].Trim());


                //判断获取微信的Openid
                //string Openid = obWeiXinUserInfo.Openid;
                string Openid = "oz0TXwTew5RmbnTa2aeMPfHfsDnY";
                Session["openid"] = Openid;



            }
            else
            {
                Response.Redirect("Error.html");
            }
        }
    }


    /// <summary>
    /// 防伪查询
    /// </summary>
    /// <param name="code"></param>
    private void Fwcx(string code)
    {
        //查询防伪
        string ReStr = Common.APIService.GetFunction("ScanByQRCode/AntiFake?qrcode=" + code);
        if (!string.IsNullOrEmpty(ReStr))
        {
            var obj = Newtonsoft.Json.Linq.JObject.Parse(ReStr);
            if (obj["isSuccess"].ToString().ToLower() == "true")
            {
                var objcode = Newtonsoft.Json.Linq.JObject.Parse(obj["antiFakeByData"].ToString());
                if (objcode["results"].ToString().ToUpper() == "OK" || objcode["results"].ToString().ToUpper() == "FLOWOFF")
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

                    if (objcode["results"].ToString().ToUpper() == "OK")
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
                    Literal1.Text = sb.ToString();
                }
                else
                {
                    //超时
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("<h3>尊敬的用户：</h3>");
                    sb.AppendLine("<p style=\" text-indent: 2.0em;\">");

                    sb.AppendLine("您查询的防伪码（" + code + "）不存在，谨防假冒！");
                    
                    sb.AppendLine("</p>");
                    Literal1.Text = sb.ToString();
                }

            }
            else
            {
                //访问不通
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("<h3>尊敬的用户：</h3>");
                sb.AppendLine("<p style=\" text-indent: 2.0em;\">");

                sb.AppendLine("您查询的防伪码（" + code + "），网络异常，请稍后！");

                sb.AppendLine("</p>");
                Literal1.Text = sb.ToString();
            }
        }
    }

    /// <summary>
    /// 产品溯源
    /// </summary>
    /// <param name="code"></param>
    private void CPSY(string code)
    {
        string ReStr = Common.APIService.GetFunction("ScanByQRCode/Traceability?qrcode=" + code);
        if (!string.IsNullOrEmpty(ReStr))
        {
            var obj = Newtonsoft.Json.Linq.JObject.Parse(ReStr);
            if (obj["status"].ToString().ToLower() == "true")
            {
                var html = "<p><span>标签序号：</span><span>" + obj["qrCode"].ToString() + "</span></p>";
                html += "<p><span>客户名称：</span><span>" + obj["agentName"].ToString() + "</span></p>";
                html += "<p><span>出库单号：</span><span>" + obj["orderNumbels"].ToString() + "</span></p>";
                html += "<p><span>出库时间：</span><span>" + obj["outTime"].ToString() + "</span></p>";
                Literal1.Text = html.ToString();

            }
        }
        
     
    }

}