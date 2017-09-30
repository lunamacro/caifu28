using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_userControls_PromoteAccount : UserControlBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (_User == null && -1 != _User.ID)
        {
            Response.Redirect("/CPS/Default.aspx", true);
            return;
        }
        l_name.Text = string.IsNullOrEmpty(_User.Name) ? _User.Mobile : _User.Name;
        DateTime date = DateTime.Now;
        if (date.Hour < 9)
        {
            l_sayHello.Text = "早上好 !";
        }
        else if (date.Hour >= 9 && date.Hour < 12)
        {
            l_sayHello.Text = "上午好 !";
        }
        else if (date.Hour >= 12 && date.Hour <= 14)
        {
            l_sayHello.Text = "中午好 !";
        }
        else if (date.Hour > 14 && date.Hour < 19)
        {
            l_sayHello.Text = "下午好 !";
        }
        else
        {
            l_sayHello.Text = "晚上好 !";
        } 
        if (!IsPostBack)
        {
            BindAccoutInfo();
        }
    }
    /// <summary>
    /// 绑定账户信息
    /// </summary>
    private void BindAccoutInfo()
    {
        //每日
        int TodayNewUsersCount = 0;
        int TodayBuyUsersCount = 0;
        double TodayBuySumMoney = 0;
        double TodayBonus = 0;

        //本月
        int MonthNewUsersCount = 0;
        int MonthBuyUsersCount = 0;
        double MonthBuySumMoney = 0;
        double MonthBonus = 0;

        //累计
        int NewUsersCount = 0;
        int BuyUsersCount = 0;
        double BuySumMoney = 0;
        double Bonus = 0;

        int ReturnValue = -1;
        string ReturnDescription = "";

        DAL.Procedures.P_CpsGetPromoteUserAccount(_User.ID, ref TodayNewUsersCount, ref TodayBuyUsersCount, ref TodayBuySumMoney, ref TodayBonus, ref MonthNewUsersCount, ref MonthBuyUsersCount, ref MonthBuySumMoney, ref MonthBonus, ref NewUsersCount, ref BuyUsersCount, ref BuySumMoney, ref Bonus, ref ReturnValue, ref ReturnDescription);
        if (ReturnValue < 0)
        {
            ReturnDescription = "数据库读写错误";
            return;
        }

        this.l_userCountForDay.Text = TodayNewUsersCount.ToString();
        this.l_userBuyCountForDay.Text = TodayBuyUsersCount.ToString();
        this.l_userBuyMoneyForDay.Text = TodayBuySumMoney.ToString();
        this.l_commissionForDay.Text = TodayBonus.ToString();

        this.l_userCountForMonth.Text = MonthNewUsersCount.ToString();
        this.l_userBuyCountForMonth.Text = MonthBuyUsersCount.ToString();
        this.l_userBuyMoneyForMonth.Text = MonthBuySumMoney.ToString();
        this.l_commissionForMonth.Text = MonthBonus.ToString();

        this.l_sumUserCount.Text = NewUsersCount.ToString();
        this.l_sumUserBuyCount.Text = BuyUsersCount.ToString();
        this.l_sumUserBuyMoney.Text = BuySumMoney.ToString();
        this.l_sumCommission.Text = Bonus.ToString();
    }
}