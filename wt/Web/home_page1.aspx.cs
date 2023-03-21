using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class home_page1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["openid"] != "" && Session["openid"] != null)
            {
                string ReStr = Common.APIService.GetFunction("UserItems/GetUserItems?openid=" + Session["openid"]);
                if (!string.IsNullOrEmpty(ReStr))
                {
                    var obj = Newtonsoft.Json.Linq.JObject.Parse(ReStr);
                    if (obj["success"].ToString().ToLower() == "true")
                    {
                        //红包领取的记录
                        string hongbao = "领取次数：" + obj["redpacktCount"].ToString() + "次";
                        hongbao+="<br />";
                        hongbao += "总金额：" +float.Parse( obj["redpacketTotalAmount"].ToString ()).ToString("N") + "元";
                        hbjl.InnerHtml = hongbao;

                        //抽奖记录
                        string choujiang = "抽奖次数：" + obj["lotteryCount"].ToString() + "次";
                        choujiang += "<br />";
                        choujiang += "中奖次数：" + float.Parse(obj["lotteryWinCount"].ToString()).ToString("N") + "元";
                        cjjl.InnerHtml = choujiang;
                        
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