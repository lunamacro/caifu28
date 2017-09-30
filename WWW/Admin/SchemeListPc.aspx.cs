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

public partial class Admin_SchemeListPc : AdminPageBase
{
    public int pageSize = 20;
    public int PageIndex = 1;
    public int PageCount = 1;//总页数
    public int DataCount = 1;//总数据

   
    public string initiateName = "";
    public string startTime = "";
    public string endTime = "";

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
       

        if ("" != this.txt_userName.Text.Trim())
        {
            initiateName = this.txt_userName.Text.Trim();
        }

        if ("" != this.txt_startDate.Text.Trim())
        {
            DateTime startDate = DateTime.Parse(this.txt_startDate.Text.Trim());
            startTime = startDate.ToString("yyyy-MM-dd") + " 00:00:00";
        }
        else
        {
            DateTime startDate = DateTime.Now.AddDays(-1);
            this.txt_startDate.Text = startDate.ToString("yyyy-MM-dd");
            startTime = startDate.ToString("yyyy-MM-dd") + " 00:00:00";
        }

        if ("" != this.txt_endDate.Text.Trim())
        {
            DateTime endDate = DateTime.Parse(this.txt_endDate.Text.Trim());
            endTime = endDate.ToString("yyyy-MM-dd") + " 23:59:00";
        }
        else
        {
            DateTime endDate = DateTime.Now.AddDays(-1);
            this.txt_endDate.Text = endDate.ToString("yyyy-MM-dd");
            endTime = endDate.ToString("yyyy-MM-dd") + " 23:59:59";
        }
        // DataCount = new DAL.Views.V_SchemeSchedules().GetCount(Condition); //总条数
        // PageCount = ((DataCount - 1) / pageSize) + 1;//总页数
        int intResult = -1;
       // int returnValue = -1;
      //  string returnDescription = "";
        DataSet ds = null;
       // intResult = DAL.Procedures.P_SchemesListPc(ref ds, initiateName, startTime, endTime, ref returnValue, ref returnDescription);
        string querySql = @"select ROW_NUMBER() over(order by uid desc) as RowNumber , uid,Name as uname,
SUM(pay0) as pay0,SUM(win0) as win0,sum(lose0) as lose0,
SUM(pay1) as pay1,SUM(win1) as win1,sum(lose1) as lose1,
SUM(pay2) as pay2,SUM(win2) as win2,sum(lose2) as lose2
from 
(select s.InitiateUserID as uid,
sum(s.Money) as pay0,sum(s.WinMoney) as win0, sum(s.WinMoney-s.Money) as lose0,
0 as pay1,0 as win1,0 as lose1,
0 as pay2,0 as win2,0 as lose2
from T_Schemes as s WITH(NOLOCK)
right join T_SchemesFrom as f on s.ID = f.SchemesID
where HomeIndex =0 and s.QuashStatus=0 and s.isOpened=1
 and s.DateTime>='startTime' and s.DateTime<='endTime'
group by s.InitiateUserID

union all
select s.InitiateUserID as uid,
0 as pay0,0 as win0,0 as lose0,
sum(s.Money) as pay1,sum(s.WinMoney) as win1, sum(s.WinMoney-s.Money) as lose1,
0 as pay2,0 as win2,0 as lose2
from T_Schemes as s WITH(NOLOCK)
right join T_SchemesFrom as f on s.ID = f.SchemesID
where HomeIndex =1 and s.QuashStatus=0 and s.isOpened=1
 and s.DateTime>='startTime' and s.DateTime<='endTime'
group by s.InitiateUserID

union all
select s.InitiateUserID as uid,
0 as pay0,0 as win0,0 as lose0,
0 as pay1,0 as win1,0 as lose1,
sum(s.Money) as pay2,sum(s.WinMoney) as win2, sum(s.WinMoney-s.Money) as lose2
from T_Schemes as s WITH(NOLOCK)
right join T_SchemesFrom as f on s.ID = f.SchemesID
where HomeIndex =2 and s.QuashStatus=0 and s.isOpened=1
 and s.DateTime>='startTime' and s.DateTime<='endTime'
group by s.InitiateUserID
) as K 
left join T_Users as u on u.ID=K.uid
where 1=1 ";

        if(string.IsNullOrEmpty(initiateName)&&initiateName.Length>1){
            querySql+=" and u.Name like '%"+initiateName+"%'";
        }
 querySql+="group by K.uid,u.Name";
 querySql = querySql.Replace("startTime", startTime);
 querySql = querySql.Replace("endTime", endTime);
 string getDataCountSQL = "select COUNT(1)as DataCount from ({0}) as NewTable";
 string getDataSQL = "select top {0} * from ({1}) as NewTable where RowNumber > ({2} - 1) * {3} and RowNumber <= ({4} * {5})";
 DataCount = Shove._Convert.StrToInt(Shove.Database.MSSQL.ExecuteScalar(string.Format(getDataCountSQL, querySql)) + "", 0);
 PageCount = ((DataCount - 1) / pageSize) + 1;
 DataTable dt = Shove.Database.MSSQL.Select(string.Format(getDataSQL, pageSize, querySql, PageIndex, pageSize, PageIndex, pageSize));
      

        //if (ds.Tables.Count > 0)
        //{

        //   // dt = DataSetHelper.SplitDataTable(ds.Tables[0], 1, 20);
        //  //  DataCount = Shove._Convert.StrToInt(dt.Rows.Count.ToString(), 0);
        //    PageCount = ((DataCount - 1) / pageSize) + 1;//总页数
        //}
        //else
        //{
        //    DataCount = 0;
        //}


        //int pagsizeq = DataCount % 20;

        //if (pagsizeq == 0)
        //{

        //    PageCount = DataCount / 20;

        //}
        //else
        //{
        //    PageCount = (DataCount / 20) + 1;
        //}

        this.rpt_list.DataSource = dt;
        this.rpt_list.DataBind();

    }
    #endregion

    public string TranHomeFrom(string status)
    {
        switch (status)
        {
            case "0":
                return "玩法一";
            case "1":
                return "玩法二";
            case "2":
                return "玩法三";
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

        Regex numberReg = new Regex("^\\d+$");
        Regex dateReg = new Regex(@"\d{4}([\s/-][\d]{2}){2}"); //2014-12-10 00:00:00:000
       
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

        initiateName = Shove._Web.Utility.GetRequest("userName").Trim();
        if (null != initiateName)
        {
            this.txt_userName.Text = initiateName;
        }
        PageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("PageIndex"), 1);
        string today = DateTime.Now.ToShortDateString();
        long Rebate_count = new DAL.Tables.T_BackWaterHistory().GetCount("created between '" + today + " 00:00:00' and '" + today + " 23:59:59'");
        if (Rebate_count < 1)
        {
            //未回水
            btn_rebate.Visible = true;
            lab_rebate.Visible = false;
        }else{
            //已经回水了
            btn_rebate.Visible = false;
            lab_rebate.Visible = true;
        }
    }
    #endregion


    #region 页面初始化

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.SchemeQuery);

        base.OnInit(e);
    }

    #endregion

    protected void btn_search_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void btn_Rebate_Click(object sender, EventArgs e)
    {
        //反水
        int intResult = -1;
        int returnValue = -1;
        string returnDescription = "";
        intResult = DAL.Procedures.P_GetRebateMoney(ref returnValue,ref returnDescription);
        if (intResult >= 0)
        {
            this.Alert(this.Page, "完成回水，今天内不能再次点击！", EnumIconType.Success);
        }
        else
        {
            this.Alert(this.Page, "回水失败！", EnumIconType.Error);
        }
        GetURLParamsAndInit();
    }

    public string GetSchemeState(int Share, int BuyedShare, bool Buyed, int QuashStatus, bool IsOpened, double WinMoney)
    {
        string schemeState = PF.SchemeState(Share, BuyedShare, Buyed, QuashStatus, IsOpened, WinMoney);

        return schemeState;
    }

}
