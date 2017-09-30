using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// CPS代理商父页面
/// </summary>
public class CPSAgentBase : System.Web.UI.Page
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

        //检查是否登录用户
        _User = Users.GetCurrentUser(_Site.ID);
        if (null == _User || -1 == _User.ID)
        {
            Response.Redirect("/CPS/Default.aspx", true);
        }

        Users _userCPS = new Users(1)[1,_User.ID];
        if(_userCPS.isAgent!=2)
        {
            Shove._Web.JavaScript.Alert(this, "你不是CPS代理商,不能访问此页面。", "/CPS/NotCPS.aspx");
        }
        base.OnInit(e);
    }
}