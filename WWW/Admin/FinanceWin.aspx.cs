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

public partial class Admin_FinanceWin : AdminPageBase
{
    public string WinMoney = "0";
    public int PageIndex;//当前页
    public int PageCount;//总页数
    public int DataCount;//总数据
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            tbID.Text = Shove._Web.Utility.GetRequest("id");

            if (tbID.Text == "")
            {
                tbID.Text = "-1";
            }

            BindDataForLottery();

            BindDataForIsuse();


        }
        BindData();
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.WinDetails);

        base.OnInit(e);
    }

    #endregion
    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_WinList");

            return;
        }
        ddlLottery.Items.Clear();
        ddlLottery.Items.Add(new ListItem("全部", "0"));
        foreach (DataRow dr in dt.Rows)
        {
            ddlLottery.Items.Add(new ListItem(dr["Name"].ToString(), dr["ID"].ToString()));
        }

    }

    private void BindDataForIsuse()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        DataTable dt = null;

        string Condtion = "LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue);

        if (Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) != "72" && Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) != "73")
        {
            Condtion += " and EndTime < GetDate() and isOpened = 1";
        }


        dt = new DAL.Tables.T_Isuses().Open("[ID], [Name]", Condtion, "EndTime desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_WinList");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlIsuse, dt, "Name", "ID");

        //if (ddlIsuse.Items.Count > 0)
        //{
        //    BindData();
        //}
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindDataForIsuse();
    }

    private void BindData()
    {
        long UserID = Shove._Convert.StrToLong(tbID.Text, -1);
        string Name = Shove._Web.Utility.FilteSqlInfusion(tbName.Text.Trim().ToString());
        string StrWhere = " SiteID = " + _Site.ID.ToString() + " and OperatorType = dbo.F_GetDetailsOperatorType('中奖')";
        if (Name != "")
        {
            StrWhere = "Name = '" + Name + "' and SiteID = " + _Site.ID.ToString() + " and OperatorType = dbo.F_GetDetailsOperatorType('中奖')";
        }

        if (tbID.Text != "-1")
        {
            StrWhere += " and UserID = " + UserID.ToString();
        }
        if (tbTimeStart.Text.Trim() != "")
        {
            StrWhere += " and  DateTime >= '" + tbTimeStart.Text.Trim() + "'";
        }
        if (tbTimeEnd.Text.Trim() != "")
        {
            StrWhere += " and  DateTime <= '" + tbTimeEnd.Text.Trim() + "'";
        }
        if (tbMoneyStart.Text.Trim() != "")
        {
            StrWhere += " and  Money >= " + tbMoneyStart.Text.Trim();
        }
        if (tbMoneyEnd.Text.Trim() != "")
        {
            StrWhere += " and  Money <= " + tbMoneyEnd.Text.Trim();
        }
        if (ddlFromClient.SelectedValue != "0")
        {
            StrWhere += " and  FromClient = " + ddlFromClient.SelectedValue;
        }
        if (ddlLottery.SelectedValue != "0")
        {
            StrWhere += " and LotteryID=" + ddlLottery.SelectedValue;
        }
        if (ddlIsuse.SelectedValue != "")
        {
            StrWhere += " and IsuseID=" + ddlIsuse.SelectedValue + " ";
        }

        DataTable dt;

        dt = new DAL.Views.V_UserDetails().Open("ID", StrWhere, "[DateTime] desc");


        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_FinanceWin");

            return;
        }


        int intResult = -1;
        int TotalRowCount = 0;
        int PageCountt = 0;
        PageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("pageindex"), 1);
        DataSet ds = null;
        intResult = DAL.Procedures.P_Pager(ref ds, PageIndex, 10, 0, "*",
             "V_UserDetails", "DateTime desc", "" + StrWhere + "", "[DateTime] desc", ref TotalRowCount, ref PageCountt);

        DataTable dp = null;
        if (ds.Tables.Count > 0)
        {

            dp = ds.Tables[0];
        }

        DataCount = Shove._Convert.StrToInt((dt.Rows.Count).ToString(), 0);

        int pagsizeq = DataCount % 10;

        if (pagsizeq == 0)
        {

            PageCount = DataCount / 10;

        }
        else
        {
            PageCount = (DataCount / 10) + 1;
        }

        this.rptSchemes.DataSource = dp;
        this.rptSchemes.DataBind();

        DataTable dtWin = new DAL.Views.V_UserDetails().Open("", StrWhere, "[DateTime] desc");

        WinMoney = Shove._Convert.StrToDouble(dtWin.Compute("SUM(Money)", "").ToString(), 0).ToString("0.00");
    }

    //protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    //{
    //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
    //    {
    //        double money;

    //        money = Shove._Convert.StrToDouble(e.Item.Cells[2].Text, 0);
    //        e.Item.Cells[2].Text = (money == 0) ? "" : money.ToString("0.00");

    //        e.Item.Cells[0].Text = "<a href='UserDetail.aspx?SiteID=" + e.Item.Cells[5].Text + "&ID=" + e.Item.Cells[4].Text + "'>" + e.Item.Cells[0].Text + "</a>";
    //    }
    //}

    protected void btnRead_Click(object sender, System.EventArgs e)
    {

        if (tbName.Text.Trim() != "")
        {
            Users tu = new Users(_Site.ID)[_Site.ID, tbName.Text.Trim()];

            if (tu == null)
            {
                Shove._Web.JavaScript.Alert(this.Page, "用户名不存在。");

                return;
            }

            tbID.Text = tu.ID.ToString();
        }
        else
        {
            tbID.Text = "-1";
        }

        BindData();
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }
}
