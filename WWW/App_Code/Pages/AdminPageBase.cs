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
public class AdminPageBase : System.Web.UI.Page
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
    public AdminPageBase()
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


        _User = Users.GetCurrentAdminUser(_Site.ID);
        if (isRequestLogin && (_User == null))
        {
            Response.Redirect("/UserLogin.aspx", true);
        }


        #endregion
        #region 判断权限

        if (_User.UserType != 99)
        {
            PF.GoError(ErrorNumber.NotEnoughCompetence, "对不起，您没有足够的权限访问此页面", "Admin_Welcome");

            return;
        }

        #endregion

        PageUrl = this.Request.Url.AbsoluteUri;

        string[] pageList = new string[] { "DEFAULT.ASPX", "FRAMELEFT.ASPX", "FRAMETOP.ASPX", "FRAMEBOTTOM.ASPX" };

        string thisUrl = PageUrl.Substring(PageUrl.LastIndexOf("/") + 1);

        if (pageList.Contains(thisUrl.ToUpper()) == false)
        {
            GetSchemeMonitor();
        }

        base.OnLoad(e);
    }


    /// <summary>
    /// 弹出提示
    /// </summary>
    /// <param name="page">当前页面</param>
    /// <param name="message">弹出的内容</param>
    /// <param name="iconType">图标类型</param>
    public void Alert(System.Web.UI.Page page, string message, EnumIconType iconType)
    {
        string icon = "none";
        switch (iconType)
        {
            case EnumIconType.Info:
                icon = "info";
                break;
            case EnumIconType.Question:
                icon = "question";
                break;
            case EnumIconType.Success:
                icon = "success";
                break;
            case EnumIconType.Warning:
                icon = "warning";
                break;
            case EnumIconType.Error:
                icon = "error";
                break;
            case EnumIconType.None:
            default:
                break;
        }
        string script = "<script type=\"text/javascript\">$.jBox.tip(\"" + message + "\",\"" + icon + "\");</script>";

        page.ClientScript.RegisterStartupScript(page.GetType(), "AlertMessage", script);
    }

    /// <summary>
    /// 注册脚本。输出方案监控功能
    /// </summary>
    protected void GetSchemeMonitor()
    {
        //string css = "<link href=\"/Style/SchemeMonitor.css\" rel=\"stylesheet\" type=\"text/css\" />";
        //string javascript1 = "<script src=\"/JScript/SchemeMonitor.js\" type=\"text/javascript\"></script>";
        //string javascript2 = "<script src=\"/JScript/jquery-1.8.3.min.js\" type=\"text/javascript\"></script>";
        //string SchemeMonitor = string.Format("{0}{1}{2}", css, javascript2, javascript1);
        //this.ClientScript.RegisterStartupScript(this.GetType(), "SchemeMonitor", SchemeMonitor);
    }

    public override void Dispose()
    {
        TimeSpan ts = DateTime.Now - StartTime;

        if (ts.Seconds >= 10)
        {
            new Log("Page").Write("耗时：" + ts.Minutes.ToString("00") + "分" + ts.Seconds.ToString("00") + "秒" + ts.Milliseconds.ToString("000") + "毫秒，" + PageUrl);
        }

        base.Dispose();
    }
}

/// <summary>
///弹出客户端提示的图标类型
/// </summary>
public enum EnumIconType
{
    None,
    Info,
    Question,
    Success,
    Warning,
    Error,
}