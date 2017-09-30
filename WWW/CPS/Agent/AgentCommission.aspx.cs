using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class CPS_Agent_AgentCommission : CPSAgentBase
{
    private string userName = "";
    private int userType = -1;
    private long lottery = -1;
    private string startDate = "";
    private string endDate = "";

    public int pageIndex = 1;
    public int pageSize = 20;
    public long dataCount = 0;
    public long pageCount = 0;

    public double buyLotteryMoney = 0;
    public double buyLotteryBonus = 0;

    public string getDataCountSQL = "select COUNT(1) from ({0}) as NewTable";
    public string pagingSQL = "select top {0} * from ({1})as newTable where rowNumber > (({2} - 1) * {3}) and rowNumber <= ({4} * {5})";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindLottery();
            BindUrlParams();
            if (startDate != "" && endDate != "" && DateTime.Parse(startDate) > DateTime.Parse(endDate))
            {
                Shove._Web.JavaScript.Alert(this, "截至时间不能小于开始时间");
                return;
            }
            BindDataForAgent();
        }
    }

    /// <summary>
    /// 绑定url上面的参数值
    /// </summary>
    private void BindUrlParams()
    {
        userName = Shove._Web.Utility.GetRequest("name");
        pageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("pageIndex"), 1);
        userType = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("userType"), -1);
        lottery = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("lottery"), -1);
        if ("" != userName)
        {
            this.txtName.Text = userName;
        }
        if (-1 != userType)
        {
            ddlTypeList.SelectedValue = userType + "";
        }
        if (-1 != lottery)
        {
            ddlLottery.SelectedValue = lottery + "";
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
    }


    private void BindDataForAgent()
    {
        if (null != _User && _User.ID != -1)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select u.Name as username,a.PrintOutDatetime,l.Name as LotteryName,a.SchemeID,s.SchemeNumber,a.OperatorType,");
            sb.AppendLine("(case a.Money when 0 then 0 else a.[Money]/a.BonusScale end)as DetailMoney,a.Money,a.BonusScale,ROW_NUMBER() over(order by a.PrintOutDatetime desc)as rowNumber");
            sb.AppendLine("from T_CpsBonusDetails as a ");
            sb.AppendLine("left join T_Users as u on a.FromUserID=u.ID ");
            sb.AppendLine("left join T_Lotteries as l on a.LotteryID=l.ID ");
            sb.AppendLine("left join T_Schemes as s on a.SchemeID=s.ID ");
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" where 1 = 1 and a.OwnerUserID = " + _User.ID);
            if ("" != userName)
            {
                sbWhere.Append(" and u.Name like '%" + userName + "%'");
            }
            if (-1 != userType)
            {
                sbWhere.Append(" and a.OperatorType = " + userType);
            }
            if (-1 != lottery)
            {
                sbWhere.Append(" and a.LotteryID = " + lottery);
            }
            if ("" != startDate)
            {
                sbWhere.Append(" and a.PrintOutDatetime >= '" + startDate + "'");
            }
            if ("" != endDate)
            {
                sbWhere.Append(" and a.PrintOutDatetime < '" + endDate +"'");
            }
            sb.AppendLine(sbWhere.ToString());

            dataCount = Shove._Convert.StrToLong(Shove.Database.MSSQL.ExecuteScalar(string.Format(getDataCountSQL, sb.ToString())) + "", 0);
            pageCount = CPSBLL.CalculateSumPageCount(pageSize, dataCount);
            pagingSQL = string.Format(pagingSQL, pageSize, sb.ToString(), pageIndex, pageSize, pageIndex, pageSize);
            DataTable dt = Shove.Database.MSSQL.Select(pagingSQL);
            this.rpt_list.DataSource = dt;
            this.rpt_list.DataBind();
      }
    }


    /// <summary>
    /// 绑定彩种
    /// </summary>
    private void BindLottery()
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
            buyLotteryMoney += Shove._Convert.StrToDouble(drv["DetailMoney"].ToString(), 0.00);
            buyLotteryBonus += Shove._Convert.StrToDouble(drv["Money"].ToString(), 0.00);
        }
    }
}