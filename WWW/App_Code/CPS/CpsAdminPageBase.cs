using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Shove.Web.UI;

/// <summary>
/// PageBase 的摘要说明
/// </summary>
public class CpsAdminPageBase : System.Web.UI.Page
{
    public Sites _Site;
    public Users _User;

    protected override void OnInit(EventArgs e)
    { 
        #region 获取站点

        _Site = new Sites()[1];

        if (null == _Site)
        {
            PF.GoError(ErrorNumber.Unknow, "域名无效，限制访问", this.GetType().FullName);

            return;
        }

        #endregion

        #region 获取用户

        _User = Users.GetCurrentUser(1);

        if (null == _User || -1 == _User.ID)
        {

            Response.Redirect("/CPS/Default.aspx", true);
            return;
        }

        #endregion


        HtmlMeta hm = new HtmlMeta();
        hm.HttpEquiv = "X-UA-Compatible";
        hm.Content = "IE=EmulateIE7";
        base.OnInit(e);
    }

}
