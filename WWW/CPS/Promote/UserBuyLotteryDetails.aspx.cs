using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class CPS_Promote_UserBuyLotteryDetails : CPSPromoteBase
{
    public long userID = -1;
    public string userName = "";
    public long lottery = -1;
    public string startTime = "";
    public string endTime = "";

    public string order = "cbd.[DateTime]";
    public string orderWay = "desc";

    public int pageSize = 20;
    public int pageIndex = 1;
    public long dataCount = 0;
    public long pageCount = 0;
    public double sumBuyMoney = 0;
    public double sumMoney = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            userID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("userID"), -1);
            userName = Shove._Web.Utility.GetRequest("userName");
            if (-1 == userID || "" == userName)
            {
                Shove._Web.JavaScript.Alert(this, "参数错误", "PromoteNumber.aspx");
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
    private void BindUrlParams()
    {
        this.hideUserID.Value = userID.ToString();
        this.hideUserName.Value = userName;
        lottery = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("lottery"), -1);
        pageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("pageIndex"), 1);
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
        if ("" == order)
        {
            order = "PrintOutDateTime";
        }
        if ("" == orderWay)
        {
            orderWay = "desc";
        }
        switch (order)
        {
            case "PrintOutDateTime":
                order = "cbd.[DateTime]";
                this.orderForPrintOutDateTime.Attributes.Add("orderWay", orderWay == "desc" ? "asc" : "desc");
                this.orderForPrintOutDateTime.InnerText = ("出票时间 " + (orderWay == "desc" ? "↓" : "↑"));
                break;
            case "BuyMoney":
                order = "cbd.[Money]/cbd.BonusScale";
                this.orderForBuyMoney.Attributes.Add("orderWay", orderWay == "desc" ? "asc" : "desc");
                this.orderForBuyMoney.InnerText = ("购买金额 " + (orderWay == "desc" ? "↓" : "↑"));
                break;
            case "Money":
                order = "cbd.Money";
                this.orderForMyCommission.Attributes.Add("orderWay", orderWay == "desc" ? "asc" : "desc");
                this.orderForMyCommission.InnerText = ("我的佣金 " + (orderWay == "desc" ? "↓" : "↑"));
                break;
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindData()
    {
        DataSet ds = new DataSet();
        bool IsPaging = true;
        long buyUserID = -1;
        long ownerUserID = _User.ID;
        string ownerUserName = "";
        int ownerUserType = -1;
        int isuseID = -1;
        string isuseName = "";
        string lotteryName = "";
        long schemeID = -1;
        string schemeNumber = "";
        double startBonus = -1;
        double endBonus = -1;
        double startBonusScale = -1;
        double endBonusScale = -1;
        double startBuyMoney = -1;
        double endBuyMoney = -1;
        int fromClient = -1;
        DAL.Procedures.P_CpsGetBonusDetails(ref ds, IsPaging, pageIndex, pageSize, ref dataCount, buyUserID, userName, ownerUserID, ownerUserName, ownerUserType, startTime, endTime, isuseID, isuseName, lottery, lotteryName, schemeID, schemeNumber, startBonus, endBonus, startBonusScale, endBonusScale, startBuyMoney, endBuyMoney, fromClient, order, orderWay);
        if (ds.Tables.Count > 0)
        {
            pageCount = CPSBLL.CalculateSumPageCount(pageSize, dataCount);
            this.rpt_list.DataSource = ds.Tables[0];
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
            sumBuyMoney += Shove._Convert.StrToDouble(drv["BuyMoney"].ToString(), 0.00);
            sumMoney += Shove._Convert.StrToDouble(drv["Bonus"].ToString(), 0.00);
        }
    }

}