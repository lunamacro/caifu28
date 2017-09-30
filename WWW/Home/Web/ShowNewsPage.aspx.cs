using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Home_Web_ShowNewsPage : SitePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Fill();
        }
    }
    private void Fill()
    {
        string id = Request.QueryString["ID"];
        string infoType = Request.QueryString["infoType"];
        string dataTableName = "T_News";
        if (infoType == "1")
        {
            dataTableName = "T_SiteAffiches";
        }
        string strSql = "select top 1 Content from {0} where ID={1}";
        strSql = string.Format(strSql, dataTableName, id);
        DataTable dt = Shove.Database.MSSQL.Select(strSql);
        if (dt != null && dt.Rows.Count > 0)
        {
            Response.Write(dt.Rows[0]["Content"].ToString());
        }
        else
        {
            Response.Write("无法显示内容！");
        }
    }
}