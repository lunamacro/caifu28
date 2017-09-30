using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
///UserControlBasePage 的摘要说明
/// </summary>
public class UserControlBasePage : System.Web.UI.UserControl
{
    /// <summary>
    /// 站点
    /// </summary>
    public Sites _Site = null;
    public Users _User = null;

    protected override void OnInit(EventArgs e)
    {
        #region 获取站点

        _Site = new Sites()[1];

        if (_Site == null)
        {
            PF.GoError(ErrorNumber.Unknow, "域名无效，限制访问", this.GetType().BaseType.FullName);

            return;
        }
        #endregion

        //检查是否登录用户
        _User = Users.GetCurrentUser(1);

        base.OnInit(e);
    }

    public string GetImg()
    {
        DataTable dt = new DAL.Tables.T_PageResources().Open("ResourceName,ResourceUrl", "PageName='首页'", "");
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row["ResourceName"].Equals("左边"))
                {
                    return row["ResourceUrl"].ToString();
                }
            }
        }
        return "";
    }
}