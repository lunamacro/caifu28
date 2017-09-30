using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_Contact : CPSPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }
    private void BindData()
    {
        DataTable dt = new DAL.Tables.T_ContactUs().Open("", "ContactUsType = 3", "");
        if (dt != null && dt.Rows.Count > 0)
        {
            lbContent.InnerHtml = "";
            lbContent.InnerHtml = dt.Rows[0]["ContentDetails"].ToString();
        }
    }
}