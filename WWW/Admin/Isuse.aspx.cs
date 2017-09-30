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

using Shove.Database;
using System.Text.RegularExpressions;

public partial class Admin_Isuse : AdminPageBase
{
    public int pageSize = 15;
    public int PageIndex = 1;
    public int PageCount = 1;//总页数
    public int DataCount = 1;//总数据
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            string lotteryID = Shove._Web.Utility.GetRequest("LotteryID");
            this.hide_lotteryID.Value = lotteryID;
            PageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("PageIndex"), 1);
            BindLotteryList();
            //彩种ID不为空，并且彩种ID只能是数字
            if (!string.IsNullOrEmpty(lotteryID) && new Regex("^\\d+$").IsMatch(lotteryID))
            {
                ddlLottery.SelectedValue = lotteryID;
                GetIssueListByLotteryID(lotteryID);
            }
        }
    }

    public void BindLotteryList()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], case  when ID = 74 then '足球胜负（任选九）' else Name end as [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ") and id <> 75", "[Order]");
        if (null != dt && dt.Rows.Count > 0)
        {
            this.ddlLottery.DataValueField = "ID";
            this.ddlLottery.DataTextField = "Name";
            this.ddlLottery.DataSource = dt;
            this.ddlLottery.DataBind();
            this.ddlLottery.Items.Insert(0, new ListItem("请选择彩种", "-1"));
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.IssueManage);

        base.OnInit(e);
    }

    #endregion

    protected void ddlLottery_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.hide_lotteryID.Value = ddlLottery.SelectedValue;
        GetIssueListByLotteryID(ddlLottery.SelectedValue);
    }

    public void GetIssueListByLotteryID(string lotteryID)
    {
        string sql = "select top " + pageSize + " * from ( select ID,Name,StartTime,EndTime,LotteryID,ROW_NUMBER() over(order by Name asc)as RowNumber from T_Isuses where LotteryID = " + lotteryID + " and EndTime > GETDATE()) as NewTable where RowNumber > (" + PageIndex + " - 1) * " + pageSize + " and RowNumber < = " + PageIndex + " * " + pageSize;
        DataTable dt = Shove.Database.MSSQL.Select(sql);
        DataTable endIsusesDt = Shove.Database.MSSQL.Select(@"SELECT TOP 1 ID ,Name ,StartTime ,EndTime ,LotteryID ,ROW_NUMBER() OVER ( ORDER BY Name ASC ) AS RowNumber FROM    T_Isuses WHERE   LotteryID = " + lotteryID + " AND EndTime > GETDATE() ORDER BY StartTime DESC");
        if (null != dt && dt.Rows.Count > 0)
        {
            DataRow dr = endIsusesDt.Rows[endIsusesDt.Rows.Count - 1];
            this.lt_lastIssue.Text = dr["Name"].ToString();
            this.lt_lastIssueStartTime.Text = dr["StartTime"].ToString();
            this.lt_lastIssueEndTime.Text = dr["EndTime"].ToString();

            this.DataCount = int.Parse(new DAL.Tables.T_Isuses().GetCount("LotteryID = " + lotteryID + " and EndTime > GETDATE()").ToString());
            this.PageCount = ((DataCount - 1) / pageSize) + 1;
        }
        else
        {
            this.lt_lastIssue.Text = "-";
            this.lt_lastIssueStartTime.Text = "-";
            this.lt_lastIssueEndTime.Text = "-";
        }
        this.rpt_issueList.DataSource = dt;
        this.rpt_issueList.DataBind();
    }

    protected void btn_add_Click(object sender, EventArgs e)
    {

        string selectValue = ddlLottery.SelectedValue.ToString();
        Response.Redirect("IsuseAddForKeno.aspx?LotteryID=" + selectValue, true);
    }

    public string GetEditLinkByLotteryID(string lotteryID, string issueID)
    {
      
        return string.Format("<a href='IsuseEdit.aspx?LotteryID={0}&IsuseID={1}' >修改</a>", lotteryID, issueID);
    }
}
