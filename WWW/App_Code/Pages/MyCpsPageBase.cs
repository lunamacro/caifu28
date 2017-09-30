using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;

using Shove.Web.UI;
using System.Collections;

/// <summary>
/// 后台管理员基类，管理员后台所有的页面都需要继承这个页面
/// </summary>
public class MyCpsPageBase : System.Web.UI.Page
{
    public Sites _Site;
    public Users _User;

    public bool isRequestLogin = true;                  // 是否需要登录
    public string RequestLoginPage = "";                // 需要登录后，转跳到 Login.aspx 页面，登录后，会按此页面自动定位回来
    public bool isAtFramePageLogin = true;              // 是否框架内的页面转跳登录。

    public string RequestCompetences = "";              // 需要的权限列表，用 [Administrator][Competence][...]... 表示

    public DateTime StartTime;
    public string PageUrl;
    public DateTime SystemDateTime = DateTime.Now;
    public Hashtable PropertyBag = new Hashtable();
    public MyCpsPageBase()
    {
        StartTime = DateTime.Now;
    }

    protected override void OnLoad(EventArgs e)
    {
        #region 获取站点

        //_Site = new Sites()[Shove._Web.Utility.GetUrlWithoutHttp()];
        _Site = new Sites()[1];

        if (_Site == null)
        {
            PF.GoError(ErrorNumber.Unknow, "域名无效，限制访问", this.GetType().FullName);

            return;
        }

        #endregion

        #region 获取用户


        _User = Users.GetCurrentUser(_Site.ID);
        if (null == _User || -1 == _User.ID)
        {
            Response.Redirect("/CPS/Default.aspx", true);
        }


        #endregion


        PageUrl = this.Request.Url.AbsoluteUri;

        string[] pageList = new string[] { "DEFAULT.ASPX", "FRAMELEFT.ASPX", "FRAMETOP.ASPX", "FRAMEBOTTOM.ASPX" };

        string thisUrl = PageUrl.Substring(PageUrl.LastIndexOf("/") + 1);


        base.OnLoad(e);
    }


}
