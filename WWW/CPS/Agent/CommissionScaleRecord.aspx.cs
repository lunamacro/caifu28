using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_Agent_CommissionScaleRecord : CPSAgentBase
{
    public long lotteryID = -1;
    public string startTime = "";
    public string endTime = "";
    public string userType = "";

    public int pageIndex = 1;
    public int pageSize = 20;
    public long pageCount = 0;
    public long dataCount = 0;

    public string order = "InureTime";
    public string orderWay = "DESC";

    public string selectDataSQL = "select ID,CpsUsersBonusScaleID,OwnerUserID,LotteryID,PrimaryBonusScale,BonusScale,[DateTime],InureTime,OperatorID,Memo, (select Name from T_Lotteries where ID = LotteryID)as LotteryName,ROW_NUMBER() over(order by {0} {1}) as rowNumber from T_CpsUsersBonusScaleLog where {2}";
    public string getDataCountSQL = "select COUNT(1) from ({0}) as NewTable";
    public string pagingSQL = "select top {0} * from ({1})as newTable where rowNumber > (({2} - 1) * {3}) and rowNumber <= ({4} * {5})";

    public long CpsID = -1;
    public string userName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CpsID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("CpsID"), -1);
            userName = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("UserName"));
            userType = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("userType"));
            pageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("pageIndex")), 1);
            if (-1 != CpsID && "" != userName)
            {
                this.hideCpsID.Value = CpsID.ToString();
                this.hideUserName.Value = userName;

                BindLottery();
                BindUrlParams();
                if (startTime != "" && endTime != "" && DateTime.Parse(startTime) > DateTime.Parse(endTime))
                {
                    Shove._Web.JavaScript.Alert(this, "截至时间不能小于开始时间");
                    return;
                }
                BindData();
            }
            else
            {
                Shove._Web.JavaScript.Alert(this, "参数错误");
            }
        }
    }
    private void BindUrlParams()
    {
        startTime = Shove._Web.Utility.GetRequest("startTime");
        if ("" != startTime)
        {
            this.txtStartDate.Text = startTime;
            startTime += " 00:00:00";
        }
        endTime = Shove._Web.Utility.GetRequest("endTime");
        if ("" != endTime)
        {
            this.txtEndDate.Text = endTime;
            endTime += " 23:59:59";
        }
        lotteryID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("lotteryID"), -1);
        if (-1 != lotteryID)
        {
            ddlLottery.SelectedValue = lotteryID.ToString();
        }
    }

    private void BindData()
    {
        if (_User != null && _User.ID != -1)
        {
            string where = " OwnerUserID = " + CpsID + " and 1 = 1";
            if ("" != userType) {
                where += " and memo = " + userType;
            }
            if (-1 != lotteryID)
            {
                where += " and LotteryID = " + lotteryID;
            }
            if ("" != startTime)
            {
                where += " and InureTime >= '" + startTime + "'";
            }
            if ("" != endTime)
            {
                where += " and InureTime < '" + endTime + "'";
            }
            selectDataSQL = string.Format(selectDataSQL, order, orderWay, where);
            dataCount = Shove._Convert.StrToLong(Shove.Database.MSSQL.ExecuteScalar(string.Format(getDataCountSQL, selectDataSQL)) + "", 0);
            pageCount = CPSBLL.CalculateSumPageCount(pageSize, dataCount);
            pagingSQL = string.Format(pagingSQL, pageSize, selectDataSQL, pageIndex, pageSize, pageIndex, pageSize);
            DataTable dt = Shove.Database.MSSQL.Select(pagingSQL);
            this.rpt_dataList.DataSource = dt;
            this.rpt_dataList.DataBind();
        }
    }
    private void BindLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("ID,Name", "ID in(" + _Site.UseLotteryListQuickBuy + ")", "");
        if (null != dt && dt.Rows.Count > 0)
        {
            this.ddlLottery.DataValueField = "ID";
            this.ddlLottery.DataTextField = "Name";
            this.ddlLottery.DataSource = dt;
            this.ddlLottery.DataBind();
            ddlLottery.Items.Insert(0, new ListItem("请选择彩种", "-1"));
        }
    }
}