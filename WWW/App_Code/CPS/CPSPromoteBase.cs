using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// CPS推广员父页面
/// </summary>
public class CPSPromoteBase : System.Web.UI.Page
{
    public Sites _Site;
    public Users _User;

    protected override void OnInit(EventArgs e)
    {
        #region 获取站点

        _Site = new Sites()[1];

        if (null == _Site)
        {
            PF.GoError(ErrorNumber.Unknow, "域名无效，限制访问", this.GetType().BaseType.FullName);

            return;
        }
        #endregion

        _User = Users.GetCurrentUser(_Site.ID);
        if (null == _User || -1 == _User.ID)
        {
            Response.Redirect("/CPS/Default.aspx", true);
        }


        if(_User.isAgent!=1)
        {
            Shove._Web.JavaScript.Alert(this, "你不是CPS推广员,不能访问此页面。", "/CPS/NotCPS.aspx");
        }
        base.OnInit(e);
    }
}