using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///CPSPage 的摘要说明
/// </summary>
public class CPSPage : System.Web.UI.Page
{
    public Sites _Site;
    public Users _User = null;
    protected override void OnPreInit(EventArgs e)
    {
        #region 获取站点

        _Site = new Sites()[1];

        if (null == _Site)
        {
            PF.GoError(ErrorNumber.Unknow, "域名无效，限制访问", this.GetType().BaseType.FullName);

            return;
        }
        #endregion

        

        //获得登陆用户
        _User = Users.GetCurrentUser(_Site.ID);
        
        base.OnPreInit(e);
    }
}