using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Hongbao2Code : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //判断是否是微信扫描
            if (Session["openid"] != "" && Session["openid"] != null)
            {

            }
            else
            {
                //跳转到首页
                Response.Redirect("Index.aspx");
            }
        }
    }
}