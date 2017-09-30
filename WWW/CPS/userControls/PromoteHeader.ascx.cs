using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_userControls_PromoteHeader : UserControlBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.img_logo.ImageUrl = GetImg();
        string url = this.Request.Url.ToString();
        if (url.IndexOf("PromoteIndex") > -1)
        {
            this.Default.Attributes.Add("class", "curr");
        }
        if (url.IndexOf("PromoteNumber") > -1)
        {
            this.Number.Attributes.Add("class", "curr");
        }
        if (url.IndexOf("PromoteCommission") > -1 || url.IndexOf("UserBuyLotteryDetails") > -1)
        {
            this.Commission.Attributes.Add("class", "curr");
        }
        if (url.IndexOf("PromoteDate") > -1)
        {
            this.Date.Attributes.Add("class", "curr");
        }
    }

    /// <summary>
    /// 退出按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLoginOut_Click(object sender, EventArgs e)
    {
        Users _user = new Users(1);
        string ReturnDescription = "";
        int result = 0;
        try
        {
            result = _user.Logout(ref ReturnDescription);
        }
        catch
        {

        }
        if (0 == result)
        {
            Response.Redirect("../Default.aspx", true);
        }
    }

}