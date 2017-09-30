using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AppGateWay_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string reffer = Request.QueryString["pid"];
        int refid = int.Parse(reffer);
        if (refid <= 0)
        {
            Response.Write("参数错误！");
            Response.End();
        }

        Users refUser = new Users(1)[1, refid];
        if (refUser.ID <= 0 || refUser.isAgent==0)
        {
            Response.Write("非法的推荐人！");
            Response.End();
        }

        DataTable AppSetting = Shove.Database.MSSQL.Select("Select top 1 * from T_AppSetting");
        string downUrl = AppSetting.Rows[0]["IOSUrl"].ToString();

        string ipAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
        if (ipAddress != null)
        {
            DataTable ipRecord = Shove.Database.MSSQL.Select("Select top 1 * from T_RefferIP where IPAddress='" + ipAddress + "' and [DateTime]> DATEADD(d,-1, GETDATE()) order by ID desc");
            if (ipRecord == null || ipRecord.Rows.Count == 0)
            {
                Shove.Database.MSSQL.Select("insert T_RefferIP ([DateTime],[IPAddress],[refid]) values(GETDATE(),'" + ipAddress + "'," + refid.ToString() + ")");
            }
            else
            {
                Shove.Database.MSSQL.Select("update T_RefferIP set [DateTime]=GETDATE() ,refid=" + refid.ToString() + " where ID=" + ipRecord.Rows[0]["ID"].ToString());
            }
        }

        Response.Redirect(downUrl);

    }
}