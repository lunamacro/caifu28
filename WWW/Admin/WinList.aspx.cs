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
using System.Text.RegularExpressions;

public partial class Admin_WinList : AdminPageBase
{
    public int pageSize = 15;
    public int PageIndex = 1;
    public long PageCount = 1;//总页数
    public long DataCount = 1;//总数据

    public string lotteryID = "";
    public string issueID = "";

    public string schemeNumber = "";
    public string startMoney = "";
    public string endMoney = "";

    public string startTime = "";
    public string endTime = "";
    public string buyWay = "";
    public string userName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            GetURLParamsAndInit();
            BindData();
        }
    }

    #region 绑定数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    public void BindData()
    {
        if (ddl_lotteryList.Items.Count < 1 || ddl_issueList.Items.Count < 1)
        {
            return;
        }
        string Condition = string.Format("1 = 1 and WinMoney > 0 and LotteryID = {0} and IsuseID = {1}", ddl_lotteryList.SelectedValue, ddl_issueList.SelectedValue);
        switch (ddl_buyWay.SelectedValue)
        {
            case "1":   //网站
                Condition += " and FromClient = 1";
                break;
            case "2":   //安卓
                Condition += " and FromClient = 2";
                break;
            case "3":   //苹果
                Condition += " and FromClient = 3";
                break;
            case "4":   //手机站点
                Condition += " and FromClient = 4";
                break;
            case "5":   //其它
                Condition += " and FromClient = 5";
                break;
        }
        if ("" != this.txt_userName.Text.Trim())
        {
            Condition += " and InitiateName = '" + this.txt_userName.Text.Trim() + "'";
        }
        if ("" != this.txt_startDate.Text.Trim())
        {
            DateTime startDate = DateTime.Parse(this.txt_startDate.Text.Trim());
            Condition += " and [DateTime] >= '" + startDate.ToString("yyyy-MM-dd") + " 00:00:000'";
        }
        if ("" != this.txt_schemeNumber.Text.Trim()) 
        {
            Condition += " and SchemeNumber = '" + this.txt_schemeNumber.Text.Trim() + "'";
        }
        if ("" != this.txt_startMoney.Text.Trim()) 
        {
            Condition += " and WinMoneyNoWithTax >= " + txt_startMoney.Text.Trim();
        }
        if ("" != this.txt_endMoney.Text.Trim())
        {
            Condition += " and WinMoneyNoWithTax <= " + this.txt_endMoney.Text.Trim();
        }
        if ("" != this.txt_endDate.Text.Trim())
        {
            DateTime endDate = DateTime.Parse(this.txt_endDate.Text.Trim());
            Condition += " and [DateTime] <= '" + endDate.ToString("yyyy-MM-dd") + " 23:59:000'";
        }
        DataCount = new DAL.Views.V_SchemeSchedules().GetCount(Condition);
        PageCount = ((DataCount - 1) / pageSize) + 1;
        string SQL = "select TOP " + pageSize + " * from (";
        SQL += "select FromClient,ID,LotteryName,InitiateName,SchemeNumber,[DateTime],[Money],WinMoneyNoWithTax,WinMoney,WinDescription,Multiple,ROW_NUMBER() over(order by [Money] desc) as RowNumber from V_SchemeSchedules";
        SQL += " where " + Condition;
        SQL += " )as NewTable";
        SQL += " where RowNumber > (" + PageIndex + " - 1) * " + pageSize + " and RowNumber <= " + PageIndex + " * " + pageSize + " order by [DateTime] desc";
        DataTable dt = Shove.Database.MSSQL.Select(SQL);
        this.rpt_list.DataSource = dt;
        this.rpt_list.DataBind();
    }
    #endregion
    public string TranComeFrom(string status)
    {
        switch (status)
        {
            case "1":
                return "网页端";
            case "2":
                return "安卓端";
            case "3":
                return "苹果端";
            case "4":
                return "手机站点";
            case "5":
                return "其它";
            default:
                return "未知";
        }
    }
    protected void ddl_lotteryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataForIsuse();
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.WinQuery);

        base.OnInit(e);
    }

    #endregion

    #region 获得URL上面的参数并且初始化
    /// <summary>
    /// 获得URL上面的参数并且初始化
    /// </summary>
    public void GetURLParamsAndInit()
    {
        BindLotteryList();
        Regex numberReg = new Regex("^\\d+$");
        Regex dateReg = new Regex(@"\d{4}([\s/-][\d]{1,2}){2}"); //2014-12-10 00:00:00:000
        lotteryID = Shove._Web.Utility.GetRequest("lotteryID").Trim();
        if ("" != lotteryID && numberReg.IsMatch(lotteryID))
        {
            ddl_lotteryList.SelectedValue = lotteryID;
        }
        BindDataForIsuse();
        issueID = Shove._Web.Utility.GetRequest("issueID").Trim();
        if ("" != issueID && numberReg.IsMatch(issueID))
        {
            ddl_issueList.SelectedValue = issueID;
        }
        startTime = Shove._Web.Utility.GetRequest("startTime").Trim();
        if ("" != startTime && dateReg.IsMatch(startTime))
        {
            this.txt_startDate.Text = startTime;
        }
        endTime = Shove._Web.Utility.GetRequest("endTime").Trim();
        if ("" != endTime && dateReg.IsMatch(endTime))
        {
            this.txt_endDate.Text = endTime;
        }
        buyWay = Shove._Web.Utility.GetRequest("buyWay").Trim();
        if ("" != buyWay && numberReg.IsMatch(buyWay))
        {
            this.ddl_buyWay.SelectedValue = buyWay;
        }
        schemeNumber = Shove._Web.Utility.GetRequest("schemeNumber").Trim();
        if ("" != schemeNumber)
        {
            this.txt_schemeNumber.Text = schemeNumber;
        }
        startMoney = Shove._Web.Utility.GetRequest("startMoney").Trim();
        if ("" != startMoney)
        {
            this.txt_startMoney.Text = startMoney;
        }
        endMoney = Shove._Web.Utility.GetRequest("endMoney").Trim();
        if ("" != endMoney)
        {
            this.txt_endMoney.Text = endMoney;
        }
        userName = Shove._Web.Utility.GetRequest("userName").Trim();
        if (null != userName)
        {
            this.txt_userName.Text = userName;
        }
        PageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("PageIndex"), 1);
    }
    #endregion

    #region 绑定网站使用的彩种列表
    /// <summary>
    /// 绑定彩种列表
    /// </summary>
    public void BindLotteryList()
    {
        ddl_lotteryList.Items.Clear();
        DataTable dt = new DAL.Tables.T_Lotteries().Open("ID,Name", "ID IN(" + _Site.UseLotteryListRestrictions + ")", "[order] asc");
        if (null != dt && dt.Rows.Count > 0)
        {
            this.ddl_lotteryList.DataTextField = "Name";
            this.ddl_lotteryList.DataValueField = "ID";
            this.ddl_lotteryList.DataSource = dt;
            this.ddl_lotteryList.DataBind();
        }
    }
    #endregion

    #region 根据彩种获取这个彩种的期号
    /// <summary>
    /// 根据彩种获取这个彩种的期号
    /// </summary>
    private void BindDataForIsuse()
    {
        if (ddl_lotteryList.Items.Count < 1)
        {
            return;
        }
        DataTable dt = new DAL.Tables.T_Isuses().Open("ID,Name", "StartTime < GetDate() and LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(ddl_lotteryList.SelectedValue), "EndTime desc");

        ddl_issueList.Items.Clear();
        if (dt != null && dt.Rows.Count > 0)
        {
            this.ddl_issueList.DataValueField = "ID";
            this.ddl_issueList.DataTextField = "Name";
            this.ddl_issueList.DataSource = dt;
            this.ddl_issueList.DataBind();
        }
    }
    #endregion

    protected void btn_search_Click(object sender, EventArgs e)
    {
        BindData();
    }
}
