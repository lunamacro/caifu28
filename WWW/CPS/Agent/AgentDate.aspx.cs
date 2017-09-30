using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Shove.Database;
using System.Text;

public partial class CPS_Agent_AgentDate : CPSAgentBase
{

    public double sumBuyMoney = 0;
    public double sumPayBonus = 0;
    public double sumAgentMemberBuyLottery = 0;
    public double sumAgentPromoteBuyLottery = 0;
    public double sumAgentPromoteMemberBuyLottery = 0;
    public double sumPromoteMemberBuyLottery = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindYearToDropDownList();
            BindData();
        }
    }

    private void BindData()
    {
        DataTable dt = new DAL.Tables.T_CpsUsersBonusScale().Open("ID,OwnerUserID,LotteryID,BonusScale,(select Name from T_Lotteries where ID = LotteryID)as LotteryName", "OwnerUserID=" + CPSBLL.getCpsIDByUserID(_User.ID), "");
        if (null != dt && dt.Rows.Count > 0)
        {
            string lotteryID = "";
            foreach (DataRow dr in dt.Rows)
            {
                lotteryID = dr["LotteryID"] + "";
                string BonusScale = dr["BonusScale"] + "";
                switch (lotteryID)
                {
                    case "2"://四场进球彩
                        li_scjqc.Visible = true;
                        ltSCJQC.Text = BonusScale;
                        break;
                    case "3"://七星彩
                        li_qxc.Visible = true;
                        ltQXC.Text = BonusScale;
                        break;
                    case "5"://双色球
                        li_ssq.Visible = true;
                        ltSSQ.Text = BonusScale;
                        break;
                    case "6"://3D
                        li_fc3d.Visible = true;
                        ltFC3D.Text = BonusScale;
                        break;
                    case "13"://七乐彩
                        li_qlc.Visible = true;
                        ltQLC.Text = BonusScale;
                        break;
                    case "15"://六场半全场
                        li_lcbqc.Visible = true;
                        ltLCBQC.Text = BonusScale;
                        break;
                    case "28"://重庆时时彩
                        li_cqssc.Visible = true;
                        ltCQSSC.Text = BonusScale;
                        break;
                    case "39"://超级大乐透
                        li_dlt.Visible = true;
                        ltDLT.Text = BonusScale;
                        break;
                    case "61"://江西时时彩
                        li_jxssc.Visible = true;
                        ltJXSSC.Text = BonusScale;
                        break;
                    case "62"://十一运夺金
                        li_syydj.Visible = true;
                        ltSYYDJ.Text = BonusScale;
                        break;
                    case "63"://排列3
                        li_pl3.Visible = true;
                        ltPL3.Text = BonusScale;
                        break;
                    case "64"://排列5
                        li_pl5.Visible = true;
                        ltPL5.Text = BonusScale;
                        break;
                    case "70"://江西11选5
                        li_jx11x5.Visible = true;
                        ltJX11X5.Text = BonusScale;
                        break;
                    case "72"://竞彩足球
                        li_jczq.Visible = true;
                        ltJCZQ.Text = BonusScale;
                        break;
                    case "73"://竞彩篮球
                        li_jclq.Visible = true;
                        ltJCLQ.Text = BonusScale;
                        break;
                    case "74"://胜负彩
                        li_sfc.Visible = true;
                        ltSFC.Text = BonusScale;
                        break;
                    case "75"://任选九
                        li_rx9.Visible = true;
                        ltRXJ.Text = BonusScale;
                        break;
                    case "78"://广东11选5
                        li_gd11x5.Visible = true;
                        ltGD11X5.Text = BonusScale;
                        break;
                    case "83"://江苏快3
                        li_jsk3.Visible = true;
                        ltJSK3.Text = BonusScale;
                        break;
                }
            }
        }
        DataTable table = new DAL.Tables.T_CpsAdminAccountByMonth().Open("[Year],[Month],BuyMoney,PayBonus,AgentMemberBuyLottery,AgentPromoteBuyLottery,AgentPromoteMemberBuyLottery,PromoteMemberBuyLottery,IsPayOff", "OwnerUserID = " + _User.ID + " and [Year] = " + ddlYear.SelectedValue, "[Year],[Month]");
        this.rpt_list.DataSource = table;
        this.rpt_list.DataBind();
    }

    private void BindYearToDropDownList()
    {
        DateTime now = DateTime.Now;
        int year = now.Year;
        while (year > 2004)
        {
            ddlYear.Items.Add(new ListItem(year.ToString(), year.ToString()));
            year--;
        }
    }

    public string GetLinkByType(string type)
    {
        string link = "";
        string temp = "http://" + Request.Url.Authority;
        switch (type)
        {
            case "pc":
                link = temp + "/" + new DAL.Tables.T_Cps().Open("SerialNumber", "OwnerUserID=" + _User.ID, "").Rows[0]["SerialNumber"] + ".aspx";
                break;
            case "apk":
                break;
            case "ipa":
                break;
        }
        return link;
    }

    /// <summary>
    /// 获得查看明细超链接
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    public string GetDetailsLink(object year, object month)
    {
        string paramStr = "AgentCommission.aspx?startDate={0}&endDate={1}&SearchIsShow=true&date=month";
        DateTime now = DateTime.Now;
        int tempYear = Shove._Convert.StrToInt(year.ToString(), now.Year);
        int tempMonth = Shove._Convert.StrToInt(month.ToString(), now.Month);
        string startDate = tempYear + "-" + tempMonth + "-01";
        string endDate = tempYear + "-" + tempMonth + "-" + PF.CalculateMonthHaveDayByYearAndMonth(tempYear, tempMonth);
        paramStr = string.Format(paramStr, startDate, endDate);
        string url = string.Format("<a href='{0}' target='_blank'>查看明细</a>", paramStr);
        return url;
    }

    /// <summary>
    /// 年份选择发生改变
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.BindData();
    }

    protected void rpt_list_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            sumBuyMoney += Shove._Convert.StrToDouble(drv["BuyMoney"].ToString(), 0.00);
            sumPayBonus += Shove._Convert.StrToDouble(drv["PayBonus"].ToString(), 0.00);

            sumAgentMemberBuyLottery += Shove._Convert.StrToDouble(drv["AgentMemberBuyLottery"].ToString(), 0.00);
            sumAgentPromoteBuyLottery += Shove._Convert.StrToDouble(drv["AgentPromoteBuyLottery"].ToString(), 0.00);

            sumAgentPromoteMemberBuyLottery += Shove._Convert.StrToDouble(drv["AgentPromoteMemberBuyLottery"].ToString(), 0.00);
            sumPromoteMemberBuyLottery += Shove._Convert.StrToDouble(drv["PromoteMemberBuyLottery"].ToString(), 0.00);
        }
    }
}