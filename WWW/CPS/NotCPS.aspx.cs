using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_NotCPS : CPSPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            BindNews();
        }
    }
    #region 绑定新闻列表
    /// <summary>
    /// 绑定新闻列表
    /// </summary>
    private void BindNews()
    {
        this.rpt_newsList.DataSource = new CPSBLL.CPSNews().GetCPSNews("xwgg", 1, 10);
        this.rpt_newsList.DataBind();
    }
    #endregion
}