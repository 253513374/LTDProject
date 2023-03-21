using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChouJiang : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["t"] != "" && Session["t"] != null)
            {
                string ReStr = Common.APIService.GetFunction("LotteryActivity?qrcode=" + Session["t"]);
                if (!string.IsNullOrEmpty(ReStr))
                {
                    var obj = Newtonsoft.Json.Linq.JObject.Parse(ReStr);
                    if (obj["isSuccess"].ToString().ToLower() == "true")
                    {
                        if (obj["prizes"].ToString() != "[]")
                        {
                            var prizeinfo = Newtonsoft.Json.Linq.JArray.Parse(obj["prizes"].ToString());
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            for (int i = 0; i < prizeinfo.Count; i++)
                            {
                                sb.Append("<div class=\"item-group\">");
                                sb.Append("<a href=\"javascript:void(0);\" onclick=\"choujiang(" + prizeinfo[i]["id"] + ")\">");
                                sb.Append("<div class=\"img-group\">");
                                sb.Append("<img class=\"goods-img\" src=\"" +Common.PubConstant.ImgPath+"/"+ prizeinfo[i]["imageUrl"] + "\" alt=\"" + prizeinfo[i]["description"] + "\">");
                                sb.Append("<p style=\" text-align:center;\">");
                                sb.Append(prizeinfo[i]["name"]);
                                sb.Append("</p>");
                                sb.Append("</div>");
                                sb.Append("</a>");
                                sb.Append("</div>");
                            }
                            choujiang.InnerHtml = sb.ToString();
                            
                        }
                        else
                        {
                            //没有抽奖的活动
                            Common.MessageBox.ShowAndRedirect(this.Page, "没有抽奖的活动", "index.aspx");
                            return;
                        }
                    }
                    else
                    {
                        //没有抽奖的活动
                        //context.Response.Write("{\"meg\":\"0\",\"zt\":\"" + obj["msg"].ToString() + "\"}");
                        Common.MessageBox.ShowAndRedirect(this.Page, "抽奖活动还没有开始", "index.aspx");
                        return;
                    }
                }
            }
            else
            {
                Common.MessageBox.ShowAndRedirect(this.Page, "非法访问", "Error.html");
            }

        }
    }

}