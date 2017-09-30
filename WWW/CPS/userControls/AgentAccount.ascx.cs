using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Shove.Database;
using System.Text;

public partial class CPS_userControls_AgentAccount : UserControlBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {


        if (_User == null && -1 != _User.ID)
        {
            Response.Redirect("/CPS/Default.aspx", true);
            return;
        }
        l_name.Text = _User.Name;
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
            BindCpsInfo();
        }
    }

    private void BindCpsInfo()
    {
        int TodayNewUsersCount = 0;					//今日新增会员数
        double TodayBuySumMoney = 0;					//今日会员购彩金额
        double TodayBonus = 0;						    //今日会员佣金

        int TodayNewPromoteCount = 0;				//今日新增推广员数
        double TodayPromoteBuySumMoney = 0;			    //今日推广员购彩金额
        double TodayPromoteBonus = 0;				    //今日推广员佣金

        int TodayPromoteNewUserCount = 0;			//今日推广员发展会员数
        double TodayPromoteUserBuySumMoney = 0;		    //今日推广员发展会员购彩金额
        double TodayPromoteUserBonus = 0;			    //今日推广员发展会员佣金

        double ToDaySumBonus = 0;					    //今日总佣金

        int MonthNewUsersCount = 0;					//本月新增会员数
        double MonthBuySumMoney = 0;					//本月会员购彩金额
        double MonthBonus = 0;						    //本月会员佣金

        int MonthNewPromoteCount = 0;				//本月新增推广员数
        double MonthPromoteBuySumMoney = 0;			    //本月推广员购彩金额
        double MonthPromoteBonus = 0;				    //本月推广员佣金

        int MonthPromoteNewUserCount = 0;			//本月推广员发展会员数
        double MonthPromoteUserBuySumMoney = 0;		    //本月推广员发展会员购彩金额
        double MonthPromoteUserBonus = 0;			    //本月推广员发展会员佣金

        double MonthSumBonus = 0;					    //本月总佣金

        int SumNewUsersCount = 0;					//总会员数
        double SumBuySumMoney = 0;					    //总会员购彩金额
        double SumBonus = 0;							//总会员佣金

        int SumNewPromoteCount = 0;					//总新增推广员数
        double SumPromoteBuySumMoney = 0;			    //总推广员购彩金额
        double SumPromoteBonus = 0;					    //总推广员佣金

        int SumPromoteNewUserCount = 0;				//总推广员发展会员数
        double SumPromoteUserBuySumMoney = 0;		    //总推广员发展会员购彩金额
        double SumPromoteUserBonus = 0;				    //总推广员发展会员佣金

        double Bonus = 0;							    //总佣金

        int ReturnValue = 0;
        string ReturnDescription = "";

        DAL.Procedures.P_CpsGetAgentUserAccount(_User.ID, ref TodayNewUsersCount, ref TodayBuySumMoney, ref TodayBonus,
            ref TodayNewPromoteCount, ref TodayPromoteBuySumMoney, ref TodayPromoteBonus,
            ref TodayPromoteNewUserCount, ref TodayPromoteUserBuySumMoney, ref TodayPromoteUserBonus, ref ToDaySumBonus,

            ref MonthNewUsersCount, ref MonthBuySumMoney, ref MonthBonus,
            ref MonthNewPromoteCount, ref MonthPromoteBuySumMoney, ref MonthPromoteBonus,
            ref MonthPromoteNewUserCount, ref MonthPromoteUserBuySumMoney, ref MonthPromoteUserBonus, ref MonthSumBonus,

            ref SumNewUsersCount, ref SumBuySumMoney, ref SumBonus,
            ref SumNewPromoteCount, ref SumPromoteBuySumMoney, ref SumPromoteBonus,
            ref SumPromoteNewUserCount, ref SumPromoteUserBuySumMoney, ref SumPromoteUserBonus, ref Bonus,

            ref ReturnValue, ref ReturnDescription);

        //每一天
        l_AddUserCountForDay.Text = TodayNewUsersCount + "";
        l_UserBuyLotteryMoneyForDay.Text = TodayBuySumMoney.ToString("0.00");
        l_UserBuyLotteryCommissionForDay.Text = TodayBonus.ToString("0.00");

        l_AddPromoteCountForDay.Text = TodayNewPromoteCount + "";
        l_PromoteBuyLotteryMoneyForDay.Text = TodayPromoteBuySumMoney.ToString("0.00");
        l_PromoteBuyLotteryCommissionForDay.Text = TodayPromoteBonus.ToString("0.00");

        l_PromoteAddUserCountForDay.Text = TodayPromoteNewUserCount + "";
        l_PromoteAddUserBuyLotteryMoneyForDay.Text = TodayPromoteUserBuySumMoney.ToString("0.00");
        l_PromoteAddUserBuyLotteryCommissionForDay.Text = TodayPromoteUserBonus.ToString("0.00");

        l_SumCommissionForDay.Text = ToDaySumBonus.ToString("0.00");

        //每个月
        l_AddUserCountForMonth.Text = MonthNewUsersCount + "";
        l_UserBuyLotteryMoneyForMonth.Text = MonthBuySumMoney.ToString("0.00");
        l_UserBuyLotteryCommissionForMonth.Text = MonthBonus.ToString("0.00");

        l_AddPromoteCountForMonth.Text = MonthNewPromoteCount + "";
        l_PromoteBuyLotteryMoneyForMonth.Text = MonthPromoteBuySumMoney.ToString("0.00");
        l_PromoteBuyLotteryCommissionForMonth.Text = MonthPromoteBonus.ToString("0.00");

        l_PromoteAddUserCountForMonth.Text = MonthPromoteNewUserCount + "";
        l_PromoteAddUserBuyLotteryMoneyForMonth.Text = MonthPromoteUserBuySumMoney.ToString("0.00");
        l_PromoteAddUserBuyLotteryCommissionForMonth.Text = MonthPromoteUserBonus.ToString("0.00");

        l_SumCommissionForMonth.Text = MonthSumBonus.ToString("0.00");

        //总计
        l_AddUserCountForTotal.Text = SumNewUsersCount + "";
        l_UserBuyLotteryMoneyForTotal.Text = SumBuySumMoney.ToString("0.00");
        l_UserBuyLotteryCommissionForTotal.Text = SumBonus.ToString("0.00");

        l_AddPromoteCountForTotal.Text = SumNewPromoteCount + "";
        l_PromoteBuyLotteryMoneyForTotal.Text = SumPromoteBuySumMoney.ToString("0.00");
        l_PromoteBuyLotteryCommissionForTotal.Text = SumPromoteBonus.ToString("0.00");

        l_PromoteAddUserCountForTotal.Text = SumPromoteNewUserCount + "";
        l_PromoteAddUserBuyLotteryMoneyForTotal.Text = SumPromoteUserBuySumMoney.ToString("0.00");
        l_PromoteAddUserBuyLotteryCommissionForTotal.Text = SumPromoteUserBonus.ToString("0.00");

        l_SumCommissionTotal.Text = Bonus.ToString("0.00");
    }
}