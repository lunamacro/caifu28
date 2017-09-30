using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class CPS_Agent_AgentNumber : CPSAgentBase
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
    string refid = "";

    public string getDataCountSQL = "select COUNT(1) from ({0}) as NewTable";
    public string pagingSQL = "select top {0} * from ({1})as newTable where rowNumber > (({2} - 1) * {3}) and rowNumber <= ({4} * {5})";

    protected void Page_Load(object sender, EventArgs e)
    {

        refid = Request["UserID"];
        if (!IsPostBack)
        {
            BindUrlParams();
            if (startDate != "" && endDate != "" && DateTime.Parse(startDate) > DateTime.Parse(endDate))
            {
                Shove._Web.JavaScript.Alert(this, "截至时间不能小于开始时间");
                return;
            }
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
        this.reUserID.Value = refid;

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
        if (_User != null)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("select u.ID as UserID,max(u.HandselAmount) as Handse,u.Name as UserName,u.RealityName as RealityName,u.[RegisterTime]  as [DateTime],ROW_NUMBER() over(order by u.[RegisterTime] desc)as rowNumber");
            sb.AppendLine("from T_Users as u");
           // sb.AppendLine("left join T_Users as cuc on u.agentGroup = cuc.ID");
           // sb.AppendLine("left join T_Users as r on u.ReferId = r.ID");
                if ("" != startDate)
            {
                sb.AppendFormat(" and u.[RegisterTime] >= '{0}'", startDate);
            }
            if ("" != endDate)
            {
                sb.AppendFormat(" and u.[RegisterTime] < '{0}'", endDate);
            }
            sb.AppendLine("where u.isAgent=0 and u.ReferId = " + refid);

            if ("" != userName)
            {
                sb.Append(" and u.Name like '%" + userName + "%'");
            }

            sb.AppendLine("group by u.ID,u.Name,u.RealityName,u.RegisterTime");


            dataCount = Shove._Convert.StrToLong(Shove.Database.MSSQL.ExecuteScalar(string.Format(getDataCountSQL, sb.ToString())).ToString(), 0);
            pageCount = CPSBLL.CalculateSumPageCount(pageSize, dataCount);
            pagingSQL = string.Format(pagingSQL, pageSize, sb.ToString(), pageIndex, pageSize, pageIndex, pageSize);

            string finalSQL = @"
	select temp.UserID, max(temp.UserName) as UserName,max(temp.Handse) as Handse, max(temp.RealityName) as RealityName, max(temp.[DateTime]) as [DateTime], max(temp.rowNumber) as rowNumber, sum(temp.pay) as pay, sum(temp.dis) as dis 
	from
	(
		select u.*, 0.00 as pay, 0.00 as dis 
		from ({0}) u 
	union all
		select u.*, p.Money as pay, 0.00 as dis 
		from ({0}) u 
        inner join V_UserPayDetails p on p.UserID = u.UserID
        where p.Result = 1 and p.DateTime between '{1}' and '{2}'
	union all
		select u.*, 0.00 as pay, d.Money as dis 
		from ({0}) u 
        inner join V_UserDistills d on d.UserID = u.UserID
        where d.Result = 1 and d.DateTime between '{1}' and '{2}'
	)
	as temp
	group by temp.UserID
";
            if (string.IsNullOrEmpty(startDate)) startDate = DateTime.Now.AddYears(-10).ToString();
            if (string.IsNullOrEmpty(endDate)) endDate = DateTime.Now.ToString();
            finalSQL = string.Format(finalSQL, pagingSQL, startDate, endDate);

            //Response.Write(finalSQL);    
            //Response.End();

            DataTable dt = Shove.Database.MSSQL.Select(finalSQL);
            this.rpt_list.DataSource = dt;
            this.rpt_list.DataBind();
        }
    }
    protected void rpt_list_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            //sumBonus += Shove._Convert.StrToDouble(drv["SumBonus"].ToString(), 0.00);
            //sumMoney += Shove._Convert.StrToDouble(drv["SumMoney"].ToString(), 0.00);
        }
    }

}