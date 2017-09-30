using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class CPS_Promote_PromoteNumber : CPSPromoteBase
{
    private string date = "all";
    private string searchIsShow = "false";
    private string userName = "";
    private int userType = 1;
    private string startDate = "";
    private string endDate = "";

    public int pageSize = 20;
    public int pageIndex = 1;
    public long dataCount = 0;
    public long pageCount = 0;

    public double sumBonus = 0;
    public double sumMoney = 0;

    public string getDataCountSQL = "select COUNT(1) from ({0}) as NewTable";
    public string pagingSQL = "select top {0} * from ({1})as newTable where rowNumber > (({2} - 1) * {3}) and rowNumber <= ({4} * {5})";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindUrlParams();
            this.BindData();
        }
    }

    /// <summary>
    /// 绑定url上面的参数值
    /// </summary>
    private void BindUrlParams()
    {
        pageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("pageIndex"), 1);
        searchIsShow = Shove._Web.Utility.GetRequest("searchIsShow");
        bool b_isShow = Shove._Convert.StrToBool(searchIsShow, false);
        if (b_isShow)
        {
            this.divSearch.Style.Add("display", "block");
        }
        userType = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("userType"), 1);
        switch (userType)
        {
            case 1:
                this.span1.Attributes.Add("class", "curr");
                this.timeType.InnerText = "时间：";
                break;
            case 2:
                this.span2.Attributes.Add("class", "curr");
                this.timeType.InnerText = "转入时间：";
                break;
            case 3:
                this.span3.Attributes.Add("class", "curr");
                this.timeType.InnerText = "转出时间：";
                break;
        }
        this.hideUserType.Value = userType + "";
        userName = Shove._Web.Utility.GetRequest("userName");
        if ("" != userName)
        {
            this.txtName.Text = userName.Trim();
        }
        startDate = Shove._Web.Utility.GetRequest("startDate");
        if ("" != startDate)
        {
            this.txtStartDate.Text = startDate;
            startDate += " 00:00:00";
        }
        endDate = Shove._Web.Utility.GetRequest("endDate");
        if ("" != endDate)
        {
            this.txtEndDate.Text = endDate;
            endDate += " 23:59:59";
        }
        date = Shove._Web.Utility.GetRequest("date");
        DateTime now = DateTime.Now;
        int year = now.Year;
        int month = now.Month;
        switch (date)
        {
            case "all":
                this.dateForAll.Attributes.Add("class", "curr");
                break;
            case "day":
                this.dateForDay.Attributes.Add("class", "curr");
                if (startDate == "")
                {
                    startDate = now.ToString("yyyy-MM-dd") + " 00:00:00";
                }
                if (endDate == "")
                {
                    endDate = now.ToString("yyyy-MM-dd") + " 23:59:59";
                }
                break;
            case "month":
                this.dateForMonth.Attributes.Add("class", "curr");
                if (startDate == "")
                {
                    startDate = year + "-" + month.ToString().PadLeft(2, '0') + "-01 00:00:00";
                }
                if (endDate == "")
                {
                    endDate = year + "-" + month.ToString().PadLeft(2, '0') + "-" + PF.CalculateMonthHaveDayByYearAndMonth(year, month) + " 23:59:59";
                }
                break;
            case "year":
                this.dateForYear.Attributes.Add("class", "curr");
                if (startDate == "")
                {
                    startDate = year + "-01-01 00:00:00";
                }
                if (endDate == "")
                {
                    endDate = year + "-12-" + PF.CalculateMonthHaveDayByYearAndMonth(year, 12) + " 23:59:59";
                }
                break;
            default:
                this.dateForAll.Attributes.Add("class", "curr");
                break;
        }
        this.hideDate.Value = date;
    }

    /// <summary>
    /// 绑定用户信息
    /// </summary>
    private void BindData()
    {
        StringBuilder sb = new StringBuilder();
        switch (userType)
        {
            case 1: //自发展的
                sb.AppendLine("select u.ID as UserID,u.Name as UserName,cuc.[DateTime],ROW_NUMBER() over(order by cuc.[DateTime] desc)as rowNumber,ISNULL(SUM(cbd.[Money]/cbd.BonusScale), 0)as SumMoney,ISNULL(SUM(cbd.[Money]), 0)as SumBonus");
                sb.AppendLine("from T_CpsUserChange as cuc");
                sb.AppendLine("left join T_Cps as c on c.OwnerUserID = cuc.NowUserID");
                sb.AppendLine("left join T_Users as u on u.ID = cuc.UserID");
                sb.AppendLine("left join T_CpsBonusDetails as cbd on cbd.OwnerUserID = c.OwnerUserID and cbd.FromUserID = cuc.UserID");
                if ("" != startDate)
                {
                    sb.AppendFormat(" and cbd.[PrintOutDatetime] >= '{0}'", startDate);
                }
                if ("" != endDate)
                {
                    sb.AppendFormat(" and cbd.[PrintOutDatetime] < '{0}'", endDate);
                }
                sb.AppendLine("where cuc.[Type] = -1 and cuc.[ChangeType] = 0 and cuc.NowUserID = " + _User.ID);
               
                if ("" != userName)
                {
                    sb.Append(" and u.Name like '%" + userName + "%'");
                }
                
                sb.AppendLine("group by u.ID,u.Name,cuc.[DateTime]");
                break;
            case 2: //转入

                sb.AppendLine("select u.ID as UserID,u.Name as UserName,ISNULL(SUM(b.Money),0)as SumBonus,ISNULL(SUM(b.Money/b.BonusScale),0)as SumMoney");
                sb.AppendLine(",(select top 1 [DateTime] from T_CpsUserChange where NowUserID = " + _User.ID + " and  UserID = u.ID order by [DateTime] desc)as [DateTime]");
                sb.AppendLine(",ROW_NUMBER() over(order by (select top 1 [DateTime] from T_CpsUserChange where NowUserID = " + _User.ID + " and  UserID = u.ID order by [DateTime] desc) desc)as rowNumber");
                sb.AppendLine("from T_Users as u");
                sb.AppendLine("LEFT JOIN T_CpsBonusDetails as b on b.FromUserID = u.ID and b.OwnerUserID = " + _User.ID);
                sb.AppendLine("where u.ID in(");
                sb.AppendLine("	select distinct UserID");
                sb.AppendLine("	from T_CpsUserChange as cuc");
                sb.AppendLine("	where cuc.NowUserID = " + _User.ID + " and cuc.ChangeType = 1 and cuc.[Type] = -1 ");
                if ("" != startDate)
                {
                    sb.Append(" and cuc.[DateTime] >= '" + startDate + "' ");
                }
                if ("" != endDate)
                {
                    sb.Append(" and cuc.[DateTime] < '" + endDate + "'");
                }
                sb.AppendLine(")");
                if ("" != userName)
                {
                    sb.AppendLine(" and u.Name like '%" + userName + "%'");
                }
                sb.AppendLine("group by u.ID,u.Name");
                break;
            case 3: //转出
                sb.AppendLine("select u.ID as UserID,u.Name as UserName,ISNULL(SUM(b.Money),0)as SumBonus,ISNULL(SUM(b.Money/b.BonusScale),0)as SumMoney");
                sb.AppendLine(",(select top 1 [DateTime] from T_CpsUserChange where PristineUserID = " + _User.ID + " and  UserID = u.ID order by [DateTime] desc)as [DateTime]");
                sb.AppendLine(",ROW_NUMBER() over(order by (select top 1 [DateTime] from T_CpsUserChange where PristineUserID = " + _User.ID + " and  UserID = u.ID order by [DateTime] desc) desc)as rowNumber");
                sb.AppendLine("from T_Users as u");
                sb.AppendLine("LEFT JOIN T_CpsBonusDetails as b on b.FromUserID = u.ID and b.OwnerUserID = " + _User.ID);
                sb.AppendLine("where u.ID in(");
                sb.AppendLine("	select distinct UserID");
                sb.AppendLine("	from T_CpsUserChange as cuc");
                sb.AppendLine("	where cuc.PristineUserID = " + _User.ID + " and cuc.ChangeType = 1 and cuc.[Type] = -1 ");
                if ("" != startDate)
                {
                    sb.Append(" and cuc.[DateTime] >= '" + startDate + "' ");
                }
                if ("" != endDate)
                {
                    sb.Append(" and cuc.[DateTime] < '" + endDate + "'");
                }
                sb.AppendLine(")");
                if ("" != userName)
                {
                    sb.AppendLine(" and u.Name like '%" + userName + "%'");
                }
                sb.AppendLine("group by u.ID,u.Name");
                break;
        }

        dataCount = Shove._Convert.StrToLong(Shove.Database.MSSQL.ExecuteScalar(string.Format(getDataCountSQL, sb.ToString())).ToString(), 0);
        pageCount = CPSBLL.CalculateSumPageCount(pageSize, dataCount);
        pagingSQL = string.Format(pagingSQL, pageSize, sb.ToString(), pageIndex, pageSize, pageIndex, pageSize);
        DataTable dt = Shove.Database.MSSQL.Select(pagingSQL);
        this.rpt_list.DataSource = dt;
        this.rpt_list.DataBind();
    }
    protected void rpt_list_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            sumBonus += Shove._Convert.StrToDouble(drv["SumBonus"].ToString(), 0.00);
            sumMoney += Shove._Convert.StrToDouble(drv["SumMoney"].ToString(), 0.00);
        }
    }

}