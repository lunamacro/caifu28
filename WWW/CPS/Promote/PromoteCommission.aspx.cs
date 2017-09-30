using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class CPS_Promote_PromoteCommission : CPSPromoteBase
{
    private string userName = "";
    private int lottery = -1;
    private string startDate = "";
    private string endDate = "";

    public int pageIndex = 1;
    public int pageSize = 20;
    public long dataCount = 0;
    public long pageCount = 0;

    public double buyLotteryMoney = 0;
    public double buyLotteryBonus = 0;
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
        lottery = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("lottery"), -1);
        if ("" != userName)
        {
            this.txtName.Text = userName;
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

    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindDataForAgent()
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
        string order = "cbd.[DateTime]";
        string orderWay = "desc";
        DAL.Procedures.P_CpsGetBonusDetails(ref ds, IsPaging, pageIndex, pageSize, ref dataCount, buyUserID, userName, ownerUserID, ownerUserName, ownerUserType, startDate, endDate, isuseID, isuseName, lottery, lotteryName, schemeID, schemeNumber, startBonus, endBonus, startBonusScale, endBonusScale, startBuyMoney, endBuyMoney, fromClient, order, orderWay);
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
            buyLotteryMoney += Shove._Convert.StrToDouble(drv["BuyMoney"].ToString(), 0.00);
            buyLotteryBonus += Shove._Convert.StrToDouble(drv["Bonus"].ToString(), 0.00);
        }
    }
}