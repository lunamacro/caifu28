using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Admin_AccountDetails : AdminPageBase
{
    public int pageSize = 15;
    public int PageIndex = 1;
    public long PageCount = 0;//总页数
    public long DataCount = 0;//总数据

    int outCount = 0;
    int inCount = 0;
    double outMoney = 0;
    double inMoney = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //获得当前页码
        PageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("index"), 1);

        if (!this.IsPostBack)
        {
            BindDataForYearMonth();
            BindDataForYearMonth1();
            BinDataForDay();
            BinDataForDay1();
        }
        BindData();
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.BusinessDetails);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForYearMonth()
    {
        ddlYear.Items.Clear();

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
            ddlYear.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear.SelectedIndex = ddlYear.Items.Count - 1;

        if (Month > 2)
        {
            ddlMonth.SelectedIndex = Month - 2;
        }
        else
        {
            ddlMonth.SelectedIndex = 0;
        }
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

        ddlYear1.SelectedIndex = ddlYear.Items.Count - 1;

        ddlMonth1.SelectedIndex = Month - 1;
    }

    private void BindData()
    {
        string start = ddlYear.SelectedValue + "-" + ddlMonth.SelectedValue + "-" + ddlDay.SelectedValue + " 00:00:00";
        DateTime dtStart = Shove._Convert.StrToDateTime(start, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        string end = ddlYear1.SelectedValue + "-" + ddlMonth1.SelectedValue + "-" + ddlDay1.SelectedValue + " 23:59:59";
        DateTime dtEnd = Shove._Convert.StrToDateTime(end, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));


        if (ddlYear.Items.Count < 1)
        {
            return;
        }

        //一下2个判断为时间判断
        //如果当前时间小于选择时间 则把 选择时间改成当前时间
        if (DateTime.Now.CompareTo(dtEnd) <= 0)
        {
            dtEnd = DateTime.Now;
        }

        if (dtEnd.CompareTo(dtStart) < 0)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "123", "jBox.error('开始时间不能小于结束时间!', '系统提示');", true);

            return;
        }
        string userName = keyword1.Value.Trim();
        long userID = _Site.OwnerUserID;
        if (userName != "" && userName != "输入用户名")
        {
            object o = Shove.Database.MSSQL.ExecuteScalar("select id from T_users where name=@name", new Shove.Database.MSSQL.Parameter("@name", SqlDbType.VarChar, 0, ParameterDirection.Input, userName));
            if (o == null)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "123", "jBox.info('查找的账号不存在,请重新输入!', '系统提示');", true);
                return;
            }
            userID = Shove._Convert.StrToLong(o.ToString(), 0);
        }

        long tradeID = -952473;
        if (!(keyword2.Value.Trim() == "" || keyword2.Value.Trim() == "输入流水号"))
        {
            tradeID = Shove._Convert.StrToLong(keyword2.Value.Trim(), -952473);
            if (tradeID == -952473)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "123", "jBox.error('请输入正确的交易明细编号(例如：234)!', '系统提示');", true);
                return;
            }
        }
        int ReturnValue = 0;
        string ReturnDescription = "";

        DataSet ds = null;

        if (string.IsNullOrEmpty(userID.ToString()) == false)
        {
            Users tu = new Users(_Site.ID)[_Site.ID, userID];

            keyword1.Value = tu.Name;
        }

        DAL.Procedures.P_GetUserAccountDetails(ref ds, 1, userID, dtStart, dtEnd, ref ReturnValue, ref ReturnDescription);

        if ((ds == null) || (ds.Tables.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Room_AccountDetail");

            return;
        }

        if (ReturnValue < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }
        DataView dv = ds.Tables[0].DefaultView;
        string tradeType = ddlTradeType.SelectedValue;
        string sqlStr = "OperatorType in (" + tradeType + ")";
        string schemeNumber = keyword3.Value.Trim();
        if (!(schemeNumber == "" || schemeNumber == "输入方案编号"))
        {
            sqlStr += " and Memo like '%" + schemeNumber + "%' ";
        }
        if (tradeID != -952473)
        {
            sqlStr += " and ID in (" + tradeID + ")";
        }
        dv.RowFilter = sqlStr;
        DataTable dt = dv.ToTable();
        if (dt == null || dt.Rows.Count < 1)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "123", "jBox.tip('无符合条件数据');", true);
            rptSchemes.DataSource = null;
            rptSchemes.DataBind();
            this.lblInCount.Text = this.lblOutCount.Text = 0.ToString();
            this.lblInMoney.Text = 0.ToString("0.00");
            this.lblOutMoney.Text = 0.ToString("0.00");
            this.lblInMoneySUM.Text = 0.ToString("0.00");
            this.lblOutMoneySUM.Text = 0.ToString("0.00");

            return;

        }

        //实例化分页对象
        PagedDataSource pg = new PagedDataSource();
        pg.AllowPaging = true;//设置启用分页
        pg.PageSize = 15;//设置每页显示数据
        pg.DataSource = ds.Tables[0].DefaultView;//分页数据源
        //设置分页索引
        pg.CurrentPageIndex = PageIndex - 1;
        //设置共总页数
        PageCount = pg.PageCount;
        DataCount = pg.DataSourceCount;
        this.rptSchemes.DataSource = pg;
        this.rptSchemes.DataBind();

        this.lblInCount.Text = inCount.ToString();
        this.lblOutCount.Text = outCount.ToString();
        this.lblInMoney.Text = inMoney.ToString("0.00");
        this.lblOutMoney.Text = outMoney.ToString("0.00");
        this.lblInMoneySUM.Text = Shove._Convert.StrToDouble(dt.Compute("SUM(moneyADD)", "").ToString(), 0).ToString("0.00");
        this.lblOutMoneySUM.Text = Shove._Convert.StrToDouble(dt.Compute("SUM(moneySUB)", "").ToString(), 0).ToString("0.00");
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        string gPagerId = ((Shove.Web.UI.ShoveGridPager)(Sender)).ID;
        switch (gPagerId)
        {
            case "gPager":
                hdCurDiv.Value = "divAccountDetail";
                BindData();
                break;
        }

    }

    protected void btnGO_Click(object sender, EventArgs e)
    {
        Shove._Web.Cache.ClearCache(Shove._Web.WebConfig.GetAppSettingsString("SystemPreFix") + _Site.ID.ToString() + "AccountDetail_" + _User.ID.ToString());

        BindData();
    }

    protected void g_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        DataGrid grid = (DataGrid)source;
        switch (grid.ID)
        {
            case "g":
                BindData();

                break;
        }
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        BinDataForDay();
        BinDataForDay1();
    }

    private void BinDataForDay()
    {
        ddlDay.Items.Clear();

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
        int i = int.Parse(ddlMonth.SelectedValue);
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
            ddlDay.Items.Add(new ListItem(j.ToString() + "日", j.ToString()));
        }

        if (Day > MaxDay)
        {
            Day = MaxDay;
        }

        ddlDay.SelectedIndex = Day - 1;
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
}
