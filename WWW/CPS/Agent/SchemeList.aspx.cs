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

public partial class Admin_SchemeList : AdminPageBase
{
    public int pageSize = 15;
    public int PageIndex = 1;
    public long PageCount = 1;//总页数
    public long DataCount = 1;//总数据

    public string lotteryID = "";
    public string issueID = "";
    public string schemeState = "";
    public string winState = "";
    public string startTime = "";
    public string endTime = "";
    public string buyWay = "";
    public string userName = "";
    public string zuheValue = "";

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
        string Condition = "";
        string ConditionZuhe = "";
        if (ddl_lotteryList.Items.Count < 1)
        {
            return;
        }
        string lotteryList = ddl_lotteryList.SelectedValue;
        if (lotteryList.Equals("-1"))
        {
            Condition = "1 = 1";
            ConditionZuhe = " and (ptID=9801 or ptID=9901) and BetNum not in ('大','小','单','双') ";
        }
        else
        {
            Condition = string.Format("1 = 1 and LotteryID = {0}", ddl_lotteryList.SelectedValue);// and IsuseID = {1}, ddl_issueList.SelectedValue
            ConditionZuhe = string.Format(" and  ptID = {0}01  and BetNum not in ('大','小','单','双') ", ddl_lotteryList.SelectedValue);
        }
        switch (ddl_schemeState.SelectedValue)
        {
            case "2":   //招募中
                Condition += " and Schedule < 100 and Buyed = 0 and QuashStatus = 0 and IsOpened = 0";
                break;
            case "3":   //未出票
                Condition += " and Schedule = 100 and Buyed = 0 and QuashStatus = 0 and IsOpened = 0";
                break;
            case "4":   //已出票
                Condition += " and Buyed = 1 and QuashStatus = 0 and IsOpened = 0";
                break;
            case "5":   //已撤单
                Condition += " and QuashStatus > 0 and IsOpened = 0 and Buyed = 0";
                break;
            case "6":   //已流单
                Condition += " and Share <= BuyedShare and IsOpened = 1 and Buyed = 0";
                break;
            case "7":   //未中奖
                Condition += " and Buyed = 1 and IsOpened = 1 and WinMoney <= 0";
                break;
            case "8":   //已中奖
                Condition += " and Buyed = 1 and IsOpened = 1 and WinMoney > 0";
                break;
        }
        //switch (ddl_winState.SelectedValue)
        //{
        //    case "2":   //未开奖
        //        Condition += " and Buyed = 1 and IsOpened = 0";
        //        break;
        //    case "3":   //未中奖
        //        Condition += " and Buyed = 1 and WinMoneyNoWithTax < 0";
        //        break;
        //    case "4":   //已中奖
        //        Condition += " and Buyed = 1 and WinMoneyNoWithTax > 0";
        //        break;
        //    case "5":   //已退款
        //        break;
        //}
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
        if (!string.IsNullOrEmpty(txt_SchemeNumber.Text))
        {
            Condition += " and SchemeNumber = '" + this.txt_SchemeNumber.Text.Trim() + "'";
        }
        if ("" != this.txt_startDate.Text.Trim())
        {
            DateTime startDate = DateTime.Parse(this.txt_startDate.Text.Trim());
            Condition += " and [DateTime] >= '" + startDate.ToString("yyyy-MM-dd") + " 00:00:000'";
        }

        if ("" != this.txt_endDate.Text.Trim())
        {
            DateTime endDate = DateTime.Parse(this.txt_endDate.Text.Trim());
            Condition += " and [DateTime] <= '" + endDate.ToString("yyyy-MM-dd") + " 23:59:000'";
        }

        DataCount = new DAL.Views.V_SchemeSchedules().GetCount(Condition);
        PageCount = ((DataCount - 1) / pageSize) + 1;

        string SQL = "";
        if (this.zuhe.Checked)
        {
            SQL = "select TOP " + pageSize + " * from (";
            SQL += "select ID,LotteryName,[DateTime],EndTime,SchemeNumber,InitiateName,[Money],BuyedShare,WinMoney,Share,Schedule,[State],IsOpened,QuashStatus,Buyed,([Money] / Share) as EachShareMoney,ROW_NUMBER() over(order by [DateTime] desc,[Money] desc) as RowNumber,FromClient,0 as HomeIndex,BetNum from V_Schemes_Zuhe";
            SQL += " where " + Condition + ConditionZuhe; 
            SQL += " )as NewTable";
            SQL += " where RowNumber > (" + PageIndex + " - 1) * " + pageSize + " and RowNumber <= " + PageIndex + " * " + pageSize + "";
            //Response.Write(SQL);
            //Response.End();
        }
        else
        {
            SQL = "select TOP " + pageSize + " * from (";
            SQL += "select ID,LotteryName,[DateTime],EndTime,SchemeNumber,InitiateName,[Money],BuyedShare,WinMoney,Share,Schedule,[State],IsOpened,QuashStatus,Buyed,([Money] / Share) as EachShareMoney,ROW_NUMBER() over(order by [DateTime] desc,[Money] desc) as RowNumber,FromClient,0 as HomeIndex,BetNum from V_Schemes_Zuhe";
            SQL += " where " + Condition;
            SQL += " )as NewTable";
            SQL += " where RowNumber > (" + PageIndex + " - 1) * " + pageSize + " and RowNumber <= " + PageIndex + " * " + pageSize + "";
        }



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

    public string TranHomeFrom(string status)
    {
        switch (status)
        {
            case "0":
                return "1";
            case "1":
                return "2";
            case "2":
                return "3";
            default:
                return "";
        }
    }

    #region 获得URL上面的参数并且初始化
    /// <summary>
    /// 获得URL上面的参数并且初始化
    /// </summary>
    public void GetURLParamsAndInit()
    {
        BindLotteryList();
        Regex numberReg = new Regex("^\\d+$");
        Regex dateReg = new Regex(@"\d{4}([\s/-][\d]{2}){2}"); //2014-12-10 00:00:00:000
        lotteryID = Shove._Web.Utility.GetRequest("lotteryID").Trim();
        if ("" != lotteryID && numberReg.IsMatch(lotteryID))
        {
            ddl_lotteryList.SelectedValue = lotteryID;
        }
        /*
        BindDataForIsuse();
        issueID = Shove._Web.Utility.GetRequest("issueID").Trim();
        if ("" != issueID && numberReg.IsMatch(issueID))
        {
            ddl_issueList.SelectedValue = issueID;
        }
        */
        schemeState = Shove._Web.Utility.GetRequest("schemeState").Trim();
        if ("" != schemeState && numberReg.IsMatch(schemeState))
        {
            ddl_schemeState.SelectedValue = schemeState;
        }
        winState = Shove._Web.Utility.GetRequest("winState").Trim();
        //if ("" != winState && numberReg.IsMatch(winState))
        //{
        //    ddl_winState.SelectedValue = winState;
        //}
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
        userName = Shove._Web.Utility.GetRequest("userName").Trim();
        if (null != userName)
        {
            this.txt_userName.Text = userName;
        }
        zuheValue = Shove._Web.Utility.GetRequest("zuheValue").Trim();
        if (null != zuheValue)
        {
            this.zuhe.Checked = zuheValue.Equals("yes");
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
        ddl_lotteryList.Items.Add(new ListItem("全部彩种", "-1"));
        DataTable dt = new DAL.Tables.T_Lotteries().Open("ID,Name", "ID IN(" + _Site.UseLotteryListRestrictions + ")", "[order] asc");
        if (null != dt && dt.Rows.Count > 0)
        {
            this.ddl_lotteryList.DataTextField = "Name";
            this.ddl_lotteryList.DataValueField = "ID";
            this.ddl_lotteryList.DataSource = dt;
            this.ddl_lotteryList.DataBind();
        }
        ddl_lotteryList.Items.Insert(0, new ListItem("全部", "-1"));
    }
    #endregion

    #region 根据彩种获取这个彩种的期号
    /// <summary>
    /// 根据彩种获取这个彩种的期号
    /// </summary>
    /*
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
    */
    #endregion

    #region 页面初始化

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.SchemeQuery);

        base.OnInit(e);
    }

    #endregion

    protected void ddl_lotteryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        // BindDataForIsuse();
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        BindData();
    }

    public string GetSchemeState(int Share, int BuyedShare, bool Buyed, int QuashStatus, bool IsOpened, double WinMoney)
    {
        string schemeState = PF.SchemeState(Share, BuyedShare, Buyed, QuashStatus, IsOpened, WinMoney);

        return schemeState;
    }

}
