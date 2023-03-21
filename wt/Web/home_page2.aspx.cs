using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class home_page2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if ( Session["openid"] != null&&Session["openid"] != "" )
            {
                string ReStr = Common.APIService.GetFunction("UserItems/GetUserItems?openid=" + Session["openid"]);
                if (!string.IsNullOrEmpty(ReStr))
                {
                    var obj = Newtonsoft.Json.Linq.JObject.Parse(ReStr);
                    if (obj["success"].ToString().ToLower() == "true")
                    {
                        //所有的记录
                        System.Text.StringBuilder sball = new System.Text.StringBuilder();
                        //红包类型
                        System.Text.StringBuilder sbred = new System.Text.StringBuilder();
                        //抽奖
                        System.Text.StringBuilder sbCJ = new System.Text.StringBuilder();
                        //红包记录
                        if (obj["redPacketInfos"].ToString() != "[]")
                        {
                            var red = Newtonsoft.Json.Linq.JArray.Parse(obj["redPacketInfos"].ToString());
                         
                            for (int i = 0; i < red.Count; i++)
                            {
                                //红包记录
                                sbred.Append("<li>");
                                sbred.Append("<div class=\"teb-left1\"><h3>" + red[i]["qrCode"] + "</h3></div>");
                                sbred.Append("<div class=\"teb-left2\"><h3>" + red[i]["issueTime"] + "</h3></div>");
                                sbred.Append("<div class=\"teb-right3\"><h3>" +float.Parse( red[i]["totalAmount"].ToString ()).ToString("N") + "元</h3></div>");
                                sbred.Append("</li>");

                                //记录全部的
                                sball.Append("<li>");
                                sball.Append("<div class=\"teb-left1\"><h3>" + red[i]["qrCode"] + "</h3></div>");
                                sball.Append("<div class=\"teb-left2\"><h3>" + red[i]["issueTime"] + "</h3></div>");
                                sball.Append("<div class=\"teb-right3\"><h3>" + float.Parse(red[i]["totalAmount"].ToString()).ToString("N") + "元</h3></div>");
                                sball.Append("</li>");
                            }
                           
                        }

                        //抽奖记录
                        if (obj["lotteryInfos"].ToString() != "[]")
                        {
                            var choujiang = Newtonsoft.Json.Linq.JArray.Parse(obj["lotteryInfos"].ToString());
                            for (int i = 0; i < choujiang.Count; i++)
                            {
                                //抽奖记录
                                sbCJ.Append("<li>");
                                sbCJ.Append("<div class=\"teb-left1\"><h3>" + choujiang[i]["qrCode"] + "</h3></div>");
                                sbCJ.Append("<div class=\"teb-left2\"><h3>" + choujiang[i]["time"] + "</h3></div>");
                                sbCJ.Append("<div class=\"teb-right3\"><h3>" + float.Parse(choujiang[i]["prizeName"].ToString()).ToString("N") + "元</h3></div>");
                                sbCJ.Append("</li>");

                                //记录全部的
                                sball.Append("<li>");
                                sball.Append("<div class=\"teb-left1\"><h3>" + choujiang[i]["qrCode"] + "</h3></div>");
                                sball.Append("<div class=\"teb-left2\"><h3>" + choujiang[i]["time"] + "</h3></div>");
                                sball.Append("<div class=\"teb-right3\"><h3>" + float.Parse(choujiang[i]["prizeName"].ToString()).ToString("N") + "元</h3></div>");
                                sball.Append("</li>");

                            }
                        }

                        //显示数据
                        //全部的
                        this.all.InnerHtml = "<ul>" + sball.ToString() + "</ul>";
                        //红包
                        this.hongbao.InnerHtml = "<ul>" + sbred.ToString() + "</ul>";
                        //抽奖的
                        this.choujiang.InnerHtml = "<ul>" + sbCJ.ToString() + "</ul>";
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