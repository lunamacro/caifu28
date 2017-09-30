using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Text;

public partial class UserLogin : SitePageBase
{
    public string LoginIframeUrl = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        AjaxPro.Utility.RegisterTypeForAjax(typeof(UserLogin), this.Page);

        LoginIframeUrl = ResolveUrl("~/Home/Room/UserLoginDialog.aspx");

        if (!IsPostBack)
        {
            if (_User != null)
            {
                Response.Redirect("Admin/");
            }

            if (Shove._Web.Cache.GetCache("IsLoginFirst") != null)
            {
                Shove._Web.Cache.ClearCache("IsLoginFirst");
            }
            GetImg();
        }
    }

    private void GetImg()
    {
        DataTable dt = new DAL.Tables.T_PageResources().Open("*", "PageName='表头logo'", "");
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row["ResourceName"].Equals("左边"))
                {
                    LogoImage.InnerHtml = "<a href=\"#\"> <img src=\"" + row["ResourceUrl"] + "\" style=\"border: 0px;width:250px;height:77px;\" alt=\"\" /></a>";
                }
            }
        }
    }

    protected void btnLogin_Click(object sender, System.EventArgs e)
    {
        string ReturnDescription = "";
        int Result = -1;

        Users user = new Users(1);
        user.Name = Shove._Web.Utility.FilteSqlInfusion(tbUserName.Value);
        user.Password = Shove._Web.Utility.FilteSqlInfusion(tbPassWord.Value);


        Result = user.Login(ref ReturnDescription);
        

        if (Result < 0)
        {
            hLogin.Value = "1";
            error_tips.InnerText = ReturnDescription;
            return;
        }

        //Response.Redirect("~/Admin");
        new Login().GoToRequestLoginPage("~/Admin");

       
    }

    /// <summary>
    /// 校验用户是否可用
    /// </summary>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
    public int CheckUserName(string name)
    {
        if (!PF.CheckUserName(name))
        {
            return -1;
        }

        DataTable dt = new DAL.Tables.T_Users().Open("ID", "Name = '" + Shove._Web.Utility.FilteSqlInfusion(name) + "'", "");

        if (dt != null && dt.Rows.Count > 0)
        {
            return -2;
        }

        if (Shove._String.GetLength(name) < 5 || Shove._String.GetLength(name) > 16)
        {
            return -3;
        }

        return 0;
    }


}
