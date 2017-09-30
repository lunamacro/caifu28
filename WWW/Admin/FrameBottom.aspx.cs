using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Admin_FrameBottom : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (_User.UserType != 99)
        {
            PF.GoError(ErrorNumber.NotEnoughCompetence, "对不起，您没有足够的权限访问此页面", "Admin_Welcome");

            return;
        }
        //DiffIsOpen();
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isAtFramePageLogin = false;

        base.OnInit(e);
    }

    #endregion

    //protected void lbLogout_Click(object sender, EventArgs e)
    //{
    //    if (_User != null)
    //    {
    //        string ReturnDescription = "";

    //        _User.Logout(ref ReturnDescription);
    //    }

    //    Response.Write("<script language=\"javascript\">window.top.location.href=\"../Default.aspx\"</script>");
    //}

    private void DiffIsOpen()
    {
        //DAL.Tables.T_Options TOptions = new DAL.Tables.T_Options();
        //DataTable dt = TOptions.Open("[Value]", "[ID]=32005", "");
        //if (dt.Rows[0]["Value"].ToString().ToLower() == "false")//不开源显示EIMS ID管理链接
        //{
        //    ReturnHomepage.Visible = false;
        //    lbLogout.Visible = false;
        //}
        //else//开源则不显示
        //{
        //    ReturnHomepage.HRef = _Site.Urls;
        //    ReturnHomepage.Visible = true;
        //    lbLogout.Visible = true;
        //}
    }
}
