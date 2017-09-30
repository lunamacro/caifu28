using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;

using Shove.Database;
using System.Text.RegularExpressions;

public partial class Admin_BetUpdate : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();
            BindDataRuels();
        }
    }

    private void BindData()
    {
        int num = Shove._Convert.StrToInt(ddl_homelist.SelectedItem.Value, 0);
        int lotteryid = Shove._Convert.StrToInt(ddl_lottery.SelectedItem.Value, 0);

        string sql = @"SELECT ID, Name, DefaultMoney From T_WinTypes WHERE LOTTERYID=" + lotteryid.ToString() + " and hall=" + num;

        DataTable dt = Shove.Database.MSSQL.Select(sql);

        if (null != dt && dt.Rows.Count > 0)
        {
            rpt_BetList.DataSource = dt;
            rpt_BetList.DataBind();
        }


        string sqlLimit = @"SELECT * From T_BetLimit WHERE lotteryId=" + lotteryid.ToString() + " order by paixu ";

        DataTable dtLimit = Shove.Database.MSSQL.Select(sqlLimit);

        if (null != dtLimit && dtLimit.Rows.Count > 0)
        {
            rpt_limit.DataSource = dtLimit;
            rpt_limit.DataBind();
        }


    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        Button button = sender as Button;

        string id = ((HiddenField)button.Parent.FindControl("WinTypeID")).Value.Trim();
        string DefaultMoney = ((TextBox)button.Parent.FindControl("DefaultMoney")).Text.Trim();
        if (id.Length <= 0 || Shove._Convert.StrToDouble(DefaultMoney, 0) <= 0)
        {
            Shove._Web.JavaScript.Alert(this, "保存失败。");
            return;
        }
        string updateSQL = "UPDATE T_WinTypes SET DefaultMoney=" + DefaultMoney + ",DefaultMoneyNoWithTax = " + DefaultMoney + " WHERE ID = " + id;

        int result = Shove.Database.MSSQL.ExecuteNonQuery(updateSQL);
        if (result < 0)
        {
            Shove._Web.JavaScript.Alert(this, "保存失败。");
            return;
        }
        Shove._Web.JavaScript.Alert(this, "保存成功。");


    }
    protected void ddl_homelist_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }

    protected void ddl_homelist_SelectedLotteryChange(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindDataRuels()
    {

        int num = Shove._Convert.StrToInt(ddl_rueltList.SelectedItem.Value, 0);

        string sql = @"select * from T_BackWaterRule where type=" + num;

        DataTable dt = Shove.Database.MSSQL.Select(sql);

        if (null != dt && dt.Rows.Count > 0)
        {
            rpt_backteul.DataSource = dt;
            rpt_backteul.DataBind();
        }
    }
    protected void btn_saveteul_Click(object sender, EventArgs e)
    {
        Button button = sender as Button;
        string id = ((HiddenField)button.Parent.FindControl("teulid")).Value.Trim();
        string MinMoney = ((TextBox)button.Parent.FindControl("MinMoney")).Text.Trim();
        string MaxMoney = ((TextBox)button.Parent.FindControl("MaxMoney")).Text.Trim();
        string Proportion = ((TextBox)button.Parent.FindControl("Proportion")).Text.Trim();

        if (id.Length <= 0 || Shove._Convert.StrToDouble(MinMoney, 0) <= 0)
        {
            Shove._Web.JavaScript.Alert(this, "保存失败。");
            return;
        }
        if (Shove._Convert.StrToDouble(MaxMoney, 0) <= 0 || Shove._Convert.StrToDouble(Proportion, 0) <= 0)
        {
            Shove._Web.JavaScript.Alert(this, "保存失败。");
            return;
        }


        string updateSQL = "UPDATE T_BackWaterRule SET MinMoney=" + MinMoney + ",MaxMoney = " + MaxMoney + ",Proportion=" + Proportion + "  WHERE ID = " + id;

        int result = Shove.Database.MSSQL.ExecuteNonQuery(updateSQL);
        if (result < 0)
        {
            Shove._Web.JavaScript.Alert(this, "保存失败。");
            return;
        }
        Shove._Web.JavaScript.Alert(this, "保存成功。");

    }
    protected void ddl_rueltList_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataRuels();
    }


    protected void btn_savelimit_Click(object sender, EventArgs e)
    {
        Button button = sender as Button;
        string id = ((HiddenField)button.Parent.FindControl("limitid")).Value.Trim();
        string limit_min = ((TextBox)button.Parent.FindControl("MinLimit")).Text.Trim();
        string limit_max = ((TextBox)button.Parent.FindControl("MaxLimit")).Text.Trim();
        string limit_max_all = ((TextBox)button.Parent.FindControl("MaxLimitAll")).Text.Trim();

        if (id.Length <= 0 || Shove._Convert.StrToInt(limit_min, 0) <= 0)
        {
            Shove._Web.JavaScript.Alert(this, "保存失败。");
            return;
        }
        if (Shove._Convert.StrToInt(limit_max, 0) <= 0 || Shove._Convert.StrToInt(limit_max_all, 0) <= 0)
        {
            Shove._Web.JavaScript.Alert(this, "保存失败。");
            return;
        }


        string updateSQL = "UPDATE T_BetLimit SET limit_min=" + limit_min + ",limit_max = " + limit_max + ",limit_max_all=" + limit_max_all + "  WHERE ID = " + id;

        int result = Shove.Database.MSSQL.ExecuteNonQuery(updateSQL);
        if (result < 0)
        {
            Shove._Web.JavaScript.Alert(this, "保存失败。");
            return;
        }
        Shove._Web.JavaScript.Alert(this, "保存成功。");

    }

}
