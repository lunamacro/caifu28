using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_Promote_PromoteAddUser : CPSPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
            BindNews();
        }
    }

    private void BindData()
    {
        try
        {
            this.l_siteName.Text = _Site.Name;
            string s_PromoteId = Shove._Web.Utility.GetRequest("PromoteId");
            if ("" != s_PromoteId)
            {
                long l_PromoteId = Shove._Convert.StrToLong(s_PromoteId, -1);
                if (-1 != l_PromoteId)
                {
                    DataTable dt = new DAL.Tables.T_Users().Open("Name", "ID=" + l_PromoteId, "");
                    if (null != dt && dt.Rows.Count > 0) 
                    {
                        this.l_userName.Text = dt.Rows[0]["Name"].ToString();
                        this.l_siteName2.Text = this.l_siteName.Text;
                        this.l_siteName3.Text = this.l_siteName.Text;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            this.l_siteName.Text = "未知";
            this.l_userName.Text = "未知";
            this.btnOk.Enabled = false;
            this.btnCancel.Enabled = false;
        }
    }

    private void BindNews()
    {
        DataTable dt = new DAL.Tables.T_News().Open("top 10 ID,DateTime,Title", "TypeID = 103003", "DateTime desc");
        if (null != dt && dt.Rows.Count > 0)
        {
            this.rpt_newsList.DataSource = dt;
            this.rpt_newsList.DataBind();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("http://" + Request.Url.Authority + "/Default.aspx", true);
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Session.Add("PromoteId", Shove._Web.Utility.GetRequest("PromoteId"));
        Response.Redirect("http://" + Request.Url.Authority + "/Default.aspx", true);
    }
}