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
using System.Text;

/// <summary>
/// PageBase 的摘要说明
/// </summary>
public class RoomPageBase : System.Web.UI.Page
{
    public Sites _Site;
    public Users _User;

    public bool isRequestLogin = true;                  // 是否需要登录
    public string RequestLoginPage = "";                // 需要登录后，转跳到 Login.aspx 页面，登录后，会按此页面自动定位回来
    public bool isAtFramePageLogin = true;              // 是否框架内的页面转跳登录。

    public bool isAllowPageCache = false;                // 是否允许缓存该页面
    public int PageCacheSeconds = 0;

    public DateTime StartTime;
    public string PageUrl;

    public DateTime SystemDateTime = DateTime.Now;

    //弹出广告页面列表
    public static string FloatNotifyPageList = Shove._Web.WebConfig.GetAppSettingsString("FloatNotifyPageList");
    //弹出广告显示时间（单位秒）
    public static int FloatNotifyTimeOut = Shove._Web.WebConfig.GetAppSettingsInt("FloatNotifyTimeOut", 0);

    public RoomPageBase()
    {
        StartTime = DateTime.Now;
    }

    protected override void OnLoad(EventArgs e)
    {
        if (!this.IsPostBack)
        {
            new FirstUrl().Save();
        }

        PageUrl = this.Request.Url.AbsoluteUri;

        #region 获取站点

        //_Site = new Sites()[Shove._Web.Utility.GetUrlWithoutHttp()];
        _Site = new Sites()[1];

        if (_Site == null)
        {
            PF.GoError(ErrorNumber.Unknow, "域名无效，限制访问", "SitePageBase");

            return;
        }

        #endregion

        #region 获取用户

        _User = Users.GetCurrentUser(_Site.ID);

        if (isRequestLogin && (_User == null))
        {
            PF.GoLogin(RequestLoginPage, isAtFramePageLogin);

            return;
        }

        #endregion

        #region 加载头部和底部
        try
        {
            PlaceHolder phHead = Page.FindControl("phHead") as PlaceHolder;

            if (phHead != null)
            {
                UserControl Head = Page.LoadControl("~/Home/Room/UserControls/WebHead.ascx") as UserControl;
                phHead.Controls.Add(Head);
            }


            PlaceHolder phFoot = Page.FindControl("phFoot") as PlaceHolder;

            if (phFoot != null)
            {
                UserControl Foot = Page.LoadControl("~/Home/Room/UserControls/WebFoot.ascx") as UserControl;
                phFoot.Controls.Add(Foot);
            }
        }
        catch { }

        #endregion

        #region 缓存

        if (isAllowPageCache)
        {
            if (PageCacheSeconds > 0)
            {
                this.Response.Cache.SetExpires(DateTime.Now.AddSeconds(PageCacheSeconds));
                this.Response.Cache.SetCacheability(HttpCacheability.Server);
                this.Response.Cache.VaryByParams["*"] = true;
                this.Response.Cache.SetValidUntilExpires(true);
                this.Response.Cache.SetVaryByCustom("SitePage");
            }
        }

        #endregion

        base.OnLoad(e);
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
