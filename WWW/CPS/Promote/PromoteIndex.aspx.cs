using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;

public partial class CPS_Promote_PromoteIndex : CPSPromoteBase
{
    public int pageSize = 50;
    public int PageIndex = 1;
    public long PageCount = 0;//总页数
    public long DataCount = 0;//总数据
    public string SourceFrom = "";
    public string subTitle = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        //获得当前页码
        PageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("index"), 1);
        SourceFrom = Request.QueryString["SourceFrom"];



        if (SourceFrom==null)
        {
            subTitle = "我的代理线"; 
        }
        else
        {
            subTitle = "我推荐的会员";
        }

        if (!this.IsPostBack)
        {
            BindDataForYearMonth0();
            BindDataForYearMonth1();
            BinDataForDay0();
            BinDataForDay1();
            //BindDataForYearMonth();
        }

        BindData();
    }

    private void BindDataForYearMonth0()
    {
        ddlYear0.Items.Clear();

        DateTime dt = DateTime.Now;
        int Year = dt.Year;
        int Month = dt.Month;

        if (Year < PF.SystemStartYear)
        {
            btnGO.Enabled = false;

            return;
        }

        for (int i = PF.SystemStartYear; i <= Year; i++)
        {
            ddlYear0.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear0.SelectedIndex = ddlYear0.Items.Count - 1;

        ddlMonth0.SelectedIndex = Month - 1;
    }

    private void BindDataForYearMonth1()
    {
        ddlYear1.Items.Clear();

        DateTime dt = DateTime.Now;
        int Year = dt.Year;
        int Month = dt.Month;

        if (Year < PF.SystemStartYear)
        {
            btnGO.Enabled = false;

            return;
        }

        for (int i = PF.SystemStartYear; i <= Year; i++)
        {
            ddlYear1.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear1.SelectedIndex = ddlYear1.Items.Count - 1;

        ddlMonth1.SelectedIndex = Month - 1;
    }

    private void BinDataForDay0()
    {
        ddlDay0.Items.Clear();

        int[] Month = new Int32[7];
        Month[0] = 1;
        Month[1] = 3;
        Month[2] = 5;
        Month[3] = 7;
        Month[4] = 8;
        Month[5] = 10;
        Month[6] = 12;

        DateTime dtTime = DateTime.Now;
        int Year = dtTime.Year;
        int Day = dtTime.Day;
        int i = int.Parse(ddlMonth0.SelectedValue);
        int MaxDay = 0;

        if (Month.Contains(i))
        {
            MaxDay = 31;
        }
        else if (i == 2)
        {
            if (((Year % 4) == 0) && ((Year % 100) != 0) && ((Year % 400) == 0))
            {
                MaxDay = 29;
            }
            else
            {
                MaxDay = 28;
            }
        }
        else
        {
            MaxDay = 30;
        }

        for (int j = 1; j <= MaxDay; j++)
        {
            ddlDay0.Items.Add(new ListItem(j.ToString() + "日", j.ToString()));
        }

        if (Day > MaxDay)
        {
            Day = MaxDay;
        }

        ddlDay0.SelectedIndex = Day - 1;
    }

    private void BinDataForDay1()
    {
        ddlDay1.Items.Clear();

        int[] Month = new Int32[7];
        Month[0] = 1;
        Month[1] = 3;
        Month[2] = 5;
        Month[3] = 7;
        Month[4] = 8;
        Month[5] = 10;
        Month[6] = 12;

        DateTime dtTime = DateTime.Now;
        int Year = dtTime.Year;
        int Day = dtTime.Day;
        int i = int.Parse(ddlMonth1.SelectedValue);
        int MaxDay = 0;

        if (Month.Contains(i))
        {
            MaxDay = 31;
        }
        else if (i == 2)
        {
            if (((Year % 4) == 0) && ((Year % 100) != 0) && ((Year % 400) == 0))
            {
                MaxDay = 29;
            }
            else
            {
                MaxDay = 28;
            }
        }
        else
        {
            MaxDay = 30;
        }

        for (int j = 1; j <= MaxDay; j++)
        {
            ddlDay1.Items.Add(new ListItem(j.ToString() + "日", j.ToString()));
        }

        if (Day > MaxDay)
        {
            Day = MaxDay;
        }

        ddlDay1.SelectedIndex = Day - 1;
    }

    protected void ddlMonth0_SelectedIndexChanged(object sender, EventArgs e)
    {
        BinDataForDay0();
    }

    protected void ddlMonth1_SelectedIndexChanged(object sender, EventArgs e)
    {
        BinDataForDay1();
    }

    private void BindDataForYearMonth()
    {
        ddlYear.Items.Clear();

        DateTime dt = DateTime.Now;
        int Year = dt.Year;
        int Month = dt.Month;

        if (Year < PF.SystemStartYear)
        {
            return;
        }

        for (int i = PF.SystemStartYear; i <= Year; i++)
        {
            ddlYear.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear.SelectedIndex = ddlYear.Items.Count - 1;

        ddlMonth.SelectedIndex = Month - 1;
    }

    private void BindData()
    {
        /*
        if (ddlYear.Items.Count == 0)
        {
            return;
        }

        int ReturnValue = -1;
        string ReturnDescription = "";

        DataSet ds = null;
        int Results = -1;
        Results = DAL.Procedures.P_GetAccount(ref ds, _Site.ID, int.Parse(ddlYear.SelectedValue), int.Parse(ddlMonth.SelectedValue), ref ReturnValue, ref ReturnDescription);

        if (Results < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_FinanceAddMoney");

            return;
        }

        if (ReturnValue < 0)
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_FinanceAddMoney");

            return;
        }

        #region 合计

        DataTable dtAll = ds.Tables[0];

        //当月用户消费总金额
        double allBuy = dtAll.Select().Sum(item => Shove._Convert.StrToDouble(item["Buy"].ToString(), 0));
        lb_Buy.Text = string.Format("{0:C}", allBuy);

        //当月用户充值总金额
        double allSurrogateIn = dtAll.Select().Sum(item => Shove._Convert.StrToDouble(item["SurrogateIn"].ToString(), 0));
        lb_SurrogateIn.Text = string.Format("{0:C}", allSurrogateIn);
        lb_SurrogateIn.ForeColor = allSurrogateIn > 0 ? Color.Red : Color.Green;

        //当月总的中奖总金额
        double allWinMoney = dtAll.Select().Sum(item => Shove._Convert.StrToDouble(item["WinMoney"].ToString(), 0));
        lb_WinMoney.Text = string.Format("{0:C}", allWinMoney);

        //当月积分兑换总金额
        double allExpertsIn = dtAll.Select().Sum(item => Shove._Convert.StrToDouble(item["ExpertsIn"].ToString(), 0));
        lb_ExpertsIn.Text = string.Format("{0:C}", allExpertsIn);

        //当月推广佣金总金额
        double allSurrogateOut = dtAll.Select().Sum(item => Shove._Convert.StrToDouble(item["SurrogateOut"].ToString(), 0));
        //当月联盟推广佣金总金额
        double allScoringExchangeOut = dtAll.Select().Sum(item => Shove._Convert.StrToDouble(item["ScoringExchangeOut"].ToString(), 0));
        lb_CPS.Text = string.Format("{0:C}", allSurrogateOut + allScoringExchangeOut);

        //当月总盈利
        double allEarning = dtAll.Select().Sum(item => Shove._Convert.StrToDouble(item["Earning"].ToString(), 0));
        lb_Earning.Text = string.Format("{0:C}", allEarning);
        lb_Earning.ForeColor = allEarning > 0 ? Color.Red : Color.Green;

        #endregion
        
        ds.Tables[0]
        */
        DateTime startTime = new DateTime(Convert.ToInt32(ddlYear0.SelectedValue), Convert.ToInt32(ddlMonth0.SelectedValue), Convert.ToInt32(ddlDay0.SelectedValue));
        DateTime endTime = new DateTime(Convert.ToInt32(ddlYear1.SelectedValue), Convert.ToInt32(ddlMonth1.SelectedValue), Convert.ToInt32(ddlDay1.SelectedValue)).AddDays(1).AddSeconds(-1);
        //String userKeyword = txtUserKeyword.Text.Trim();
        //String agentKeyword = Request.QueryString["ParentName"];// txtAgentKeyword.Text.Trim();
        //String groupid = Request.QueryString["groupid"];
        String userKeyword = txtUserKeyword.Text.Trim();
        String groupid = _User.ID.ToString();
        if (SourceFrom == null)
        {
            userKeyword = _User.Name;
            groupid = _User.agentGroup.ToString();
        }
        




        string commandText = @"select 
	MAX(tb3.ID) AS GroupUserId,
	MAX(tb3.Name) AS Name,
    MAX(tb3.NickName) AS NickName,

    COUNT(temp2.UserID) as cnt,
    isnull(MAX(temp2.ReferId) ,0) AS ReferId,
    isnull(SUM(temp2.pay),0) AS pay,
    isnull(SUM(temp2.Balance),0) AS Balance,
    isnull(SUM(temp2.Handse),0) AS Handse,
    isnull(SUM(temp2.dis),0) AS dis,
    isnull(SUM(temp2.win),0) AS win
    from
        [dbo].[T_Users] tb3   left join
(

        SELECT 
			   MAX(temp.UserID) as UserID,
               COUNT(temp.UserID) as cnt,
               MAX(temp.ReferId) AS ReferId,
               SUM(temp.pay) AS pay,
                SUM(temp.Balance) AS Balance,
                SUM(temp.Handse) AS Handse,
               SUM(temp.dis) AS dis,
               SUM(temp.win) AS win
        FROM
        (
            SELECT u.SiteID,
                   u.ID AS UserID,
                   u.ReferId as  ReferId,
                   0.00 AS pay,
                   u.Balance AS Balance,
                   0.00 as  Handse,
                   0.00 AS dis,
                   0.00 AS win
            FROM dbo.T_Users u
            UNION ALL
            SELECT p.SiteID,
                   p.UserID,
                   0 as ReferId,
                   p.Money AS pay,
                   0.00 AS Balance,
                   p.HandselMoney as Handse,
                   0.00 AS dis,
                   0.00 AS win
            FROM dbo.T_UserPayDetails p
            WHERE p.Result = 1
                  AND p.DateTime BETWEEN @startTime AND @endTime
            UNION ALL
            SELECT d.SiteID,
                   d.UserID,
                   0 AS ReferId,
                   0.00 AS pay,
                   0.00 AS Balance,
                   0.00 as Handse,
                   d.Money AS dis,
                   0.00 AS win
            FROM dbo.T_UserDistills d
            WHERE d.Result = 1
                  AND d.DateTime BETWEEN @startTime AND @endTime
            UNION ALL
            SELECT w.SiteID,
                   w.InitiateUserID AS UserID,
                   0 AS ReferId,
                   0.00 AS pay,
                   0.00 AS Balance,
                   0.00 as Handse,
                   0.00 AS dis,
                   w.Money AS win
            FROM dbo.T_Schemes w
            WHERE  w.[QuashStatus]=0 and w.[Buyed]=1 and w.DateTime BETWEEN @startTime AND @endTime
        ) AS temp
        group by temp.UserID
)AS temp2
        
		on {0} where {1} {2}
		group by tb3.ID
		";
        String condition1 = "";
        if (SourceFrom == null)
        {
            condition1 = String.Format("temp2.ReferId=tb3.ID");
        }
        else
        {
            condition1 = String.Format("temp2.UserID=tb3.ID");
        }

        String condition2 = String.Format("tb3.[ReferId]={0}", groupid);
        String condition3 = String.IsNullOrEmpty(userKeyword) ? "" : String.Format(" and tb3.Name like '%{0}%' ", userKeyword);


        //String condition2 = String.IsNullOrEmpty(Request.QueryString["SourceFrom"]) ? "" : (String.IsNullOrEmpty(agentKeyword) ? "and (tb3.name IS NULL)" : String.Format("and (tb3.name = '{0}')", agentKeyword));// temp2.Name like '%{0}%' or 
        commandText = String.Format(commandText, condition1, condition2, condition3);
        Shove.Database.MSSQL.Parameter[] paramArray = new Shove.Database.MSSQL.Parameter[3];
        paramArray[0] = new Shove.Database.MSSQL.Parameter("siteID", SqlDbType.BigInt, 0, ParameterDirection.Input, _Site.ID);
        paramArray[1] = new Shove.Database.MSSQL.Parameter("startTime", SqlDbType.DateTime, 0, ParameterDirection.Input, startTime);
        paramArray[2] = new Shove.Database.MSSQL.Parameter("endTime", SqlDbType.DateTime, 0, ParameterDirection.Input, endTime);
        //实例化分页对象
        PagedDataSource pg = new PagedDataSource();
        pg.AllowPaging = true;//设置启用分页
        pg.PageSize = pageSize;//设置每页显示数据
        pg.DataSource = Shove.Database.MSSQL.Select(commandText, paramArray).DefaultView;//分页数据源
        //设置分页索引
        pg.CurrentPageIndex = PageIndex - 1;
        //设置共总页数
        PageCount = pg.PageCount;
        DataCount = pg.DataSourceCount;
        this.rptSchemes.DataSource = pg;
        this.rptSchemes.DataBind();
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BindData();
    }

    protected void btnGO_Click(object sender, EventArgs e)
    {
        // 查询
        BindData();
    }
}