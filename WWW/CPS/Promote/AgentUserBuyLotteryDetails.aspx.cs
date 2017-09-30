using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class CPS_Agent_AgentUserBuyLotteryDetails : CPSPromoteBase
{
    public long userID = -1;
    public string userName = "";
    public int operatorType = 1;
    public long lottery = -1;
    public string startTime = "";
    public string endTime = "";

    public string order = "";
    public string orderWay = "";

    public int pageSize = 20;
    public int pageIndex = 1;
    public long dataCount = 0;
    public long pageCount = 0;

    public string getDataCountSQL = "select COUNT(1) from ({0}) as NewTable";
    public string pagingSQL = "select top {0} * from ({1})as newTable where rowNumber > (({2} - 1) * {3}) and rowNumber <= ({4} * {5})";

    public double sumBuyMoney = 0;
    public double sumMoney = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            userID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("userID"), -1);
            userName = Shove._Web.Utility.GetRequest("userName");
            operatorType = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("operatorType"), -1);
            if (-1 == userID || "" == userName || -1 == operatorType)
            {
                Shove._Web.JavaScript.Alert(this, "参数错误");
                return;
            }
            BindLottery();
            BindUrlParams();
            if (startTime != "" && endTime != "" && DateTime.Parse(startTime) > DateTime.Parse(endTime))
            {
                Shove._Web.JavaScript.Alert(this, "截至时间不能小于开始时间");
                return;
            }
            BindData();
        }
    }

    /// <summary>
    /// 绑定url上面的参数值
    /// </summary>
    public void BindUrlParams()
    {
        this.hideUserID.Value = userID.ToString();
        this.hideUserName.Value = userName;
        lottery = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("lottery"), -1);
        pageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("pageIndex"), 1);
        this.hideOperatorType.Value = operatorType + "";
        order = Shove._Web.Utility.GetRequest("order");
        orderWay = Shove._Web.Utility.GetRequest("orderWay");
        if (-1 != lottery)
        {
            this.ddlLottery.SelectedValue = lottery.ToString();
        }
        startTime = Shove._Web.Utility.GetRequest("startTime");
        endTime = Shove._Web.Utility.GetRequest("endTime");
        if ("" != startTime)
        {
            this.txtStartTime.Text = startTime;
            startTime += " 00:00:00";
        }
        if ("" != endTime)
        {
            this.txtEndTime.Text = endTime;
            endTime += " 23:59:59";
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void BindData()
    {
        if (null != _User)
        {
            string where = " where s.InitiateUserID = " + userID + " and 1 = 1 ";
            if (-1 != lottery)
            {
                where += " and i.LotteryID = " + lottery;
            }
            if ("" != startTime)
            {
                where += " and s.DateTime >= '" + startTime + "' ";
            }
            if ("" != endTime)
            {
                where += " and s.DateTime < '" + endTime + "'";
            }
            if ("" == order)
            {
                order = "s.DateTime";
            }
            if ("" == orderWay)
            {
                orderWay = "desc";
            }
            if ("" != order)
            {
                switch (order)
                {
                    case "PrintOutDateTime":
                        order = "s.DateTime";
                        this.orderForPrintOutDateTime.Attributes.Add("orderWay", orderWay == "desc" ? "asc" : "desc");
                        this.orderForPrintOutDateTime.InnerText = ("出票时间 " + (orderWay == "desc" ? "↓" : "↑"));
                        break;
                    case "buyMoney":
                        order = "s.[Money]";
                        this.orderForBuyMoney.Attributes.Add("orderWay", orderWay == "desc" ? "asc" : "desc");
                        this.orderForBuyMoney.InnerText = ("购买金额 " + (orderWay == "desc" ? "↓" : "↑"));
                        break;
                }

            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select s.DateTime as [DateTime],l.Name as LotteryName,s.ID as SchemeID,s.SchemeNumber,s.[Money] as buyMoney,m.[LotteryNumber] ");
            sb.AppendLine(",ROW_NUMBER() over(order by (" + order + ") " + orderWay + ")as rowNumber");
            sb.AppendLine("from T_Schemes as s ");
            sb.AppendLine("left join T_SchemesMixcast as m on m.SchemeId=s.ID ");
            sb.AppendLine("left join  T_Isuses as i on s.IsuseID=i.ID ");
            sb.AppendLine("left join T_Lotteries as l on i.LotteryID=l.ID ");
            sb.AppendLine(where);
            dataCount = Shove._Convert.StrToLong(Shove.Database.MSSQL.ExecuteScalar(string.Format(getDataCountSQL, sb.ToString())) + "", 0);
            pageCount = CPSBLL.CalculateSumPageCount(pageSize, dataCount);
            pagingSQL = string.Format(pagingSQL, pageSize, sb.ToString(), pageIndex, pageSize, pageIndex, pageSize);


            //Response.Write(pagingSQL);    
            //Response.End();
            DataTable dt = Shove.Database.MSSQL.Select(pagingSQL);
            this.rpt_list.DataSource = dt;
            this.rpt_list.DataBind();
        }
    }

    /// <summary>
    /// 绑定彩种
    /// </summary>
    public void BindLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("ID,Name", "ID in(" + _Site.UseLotteryListQuickBuy + ")", "Sort asc");
        if (null != dt && dt.Rows.Count > 0)
        {
            this.ddlLottery.DataValueField = "ID";
            this.ddlLottery.DataTextField = "Name";
            this.ddlLottery.DataSource = dt;
            this.ddlLottery.DataBind();
            ddlLottery.Items.Insert(0, new ListItem("请选择彩种", "-1"));
        }
    }
    protected void rpt_list_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            sumBuyMoney += Shove._Convert.StrToDouble(drv["buyMoney"].ToString(), 0.00);
        }
    }

}