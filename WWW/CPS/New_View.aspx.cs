using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_New_View : CPSPage
{
    public long newID = -1;
    public string newType = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        newID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("ID"), -1);
        newType = Shove._Web.Utility.GetRequest("NewType");
        if (-1 == newID || "" == newType) 
        {
            Shove._Web.JavaScript.Alert(this, "获取新闻信息失败");
        }
        if (!IsPostBack)
        {
            BindData();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindData()
    {
        string ID = Shove._Web.Utility.GetRequest("ID");
        DataTable dt = new DAL.Tables.T_News().Open("ID,[DateTime],Title,Content", "TypeID = " + (newType == "tgzn" ? "103002" : "103003") + " and ID=" + newID, "");
        if (null != dt && dt.Rows.Count > 0)
        {
            this.Title.Text = dt.Rows[0]["Title"].ToString();
            this.DateTime.Text = dt.Rows[0]["DateTime"].ToString();
            this.Content.Text = dt.Rows[0]["Content"].ToString();
        }
    }
}