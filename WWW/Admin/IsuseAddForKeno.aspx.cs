using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Shove.Database;

public partial class Admin_IsuseAddForKeno : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            int LotteryID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("LotteryID"), -1);

            //if (!PF.ValidLotteryID(_Site, LotteryID))
            //{
            //    PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_IsuseAddForKeno");
            //    return;
            //}

            tbLotteryID.Value = LotteryID.ToString();

            string IntervalType = DAL.Functions.F_GetLotteryIntervalType(LotteryID);

            if (!IntervalType.StartsWith("分"))
            {
                this.Response.Redirect("IsuseAdd.aspx?LotteryID=" + LotteryID.ToString(), true);

                return;
            }

            object oLastDate = MSSQL.ExecuteScalar("SELECT ISNULL(MAX(case LotteryID when 28 then DATEADD([day], - 1, EndTime) when 66 then DATEADD([day], - 1, EndTime) else EndTime end), DATEADD([day], - 1, GETDATE())) AS LastDate from T_Isuses where LotteryID = " + LotteryID.ToString());

            if (oLastDate == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAddForKeno");

                return;
            }

            DateTime dtLastDate = DateTime.Now;
            dtLastDate = DateTime.Parse(oLastDate.ToString()).AddDays(1);
            
            tbDate.Text = dtLastDate.Year.ToString() + "-" + dtLastDate.Month.ToString() + "-" + dtLastDate.Day.ToString();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.IssueManage);

        base.OnInit(e);
    }

    #endregion

    protected void btnAdd_Click(object sender, System.EventArgs e)
    {
        DateTime StartDate;

        try
        {
            StartDate = DateTime.Parse(tbDate.Text);
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "开始日期输入错误。");

            return;
        }

        int Days = Shove._Convert.StrToInt(tbDays.Text, 0);

        if (Days < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入要连续增加的天数。");

            return;
        }

        if (Days > 10)
        {
            Shove._Web.JavaScript.Alert(this.Page, "高频彩种一次最多只能增加10天。");

            return;
        }

        int LotteryID = int.Parse(tbLotteryID.Value);
        string Explanation = PF.GetExplanation(LotteryID, "");
        if (SLS.Lottery.JND28.ID == LotteryID)
        {
           
            double Interval = 3.5;
            string FirstEndTime = "20:01:00";
            int Degree =395;

            for (int i = 0; i < Days; i++)
            {
                string yesterdayDate = StartDate.AddDays(-1).Date.ToString("yyyy-MM-dd");
                string todayDate = StartDate.Date.ToString("yyyy-MM-dd");
               // string sDate = StartDate.Date.ToString("yyyy-MM-dd");
                DataTable dt = new DAL.Tables.T_Isuses().Open("[ID]", "StartTime between '" + yesterdayDate + " 19:58:00' and '" + todayDate + " 19:00:00' and LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(tbLotteryID.Value), "");

                if (dt == null)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAddForKeno");

                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    StartDate = StartDate.AddDays(1);

                    continue;
                }

                DateTime IsuseStartTime = DateTime.Parse(StartDate.AddDays(-1).Date.ToShortDateString() + " 19:58:30");
                DateTime IsuseEndTime = DateTime.Parse(StartDate.AddDays(-1).Date.ToShortDateString() + " " + FirstEndTime);


                
                    DateTime timing = new DateTime(2017, 6, 13);
                    // 计算期号。
                    string Isuse = (2151465 + (new TimeSpan(Convert.ToDateTime(tbDate.Text).Ticks - timing.Ticks).Days + i) * 395).ToString();
               
                long NewIsuseID = -1;
                string ReturnDescription = "";

                int Result = DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, IsuseStartTime, IsuseEndTime, IsuseEndTime, Explanation, "", ref NewIsuseID, ref ReturnDescription);

                if (Result < 0)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAddForKeno");

                    return;
                }

                if (NewIsuseID < 0)
                {
                    PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_IsuseAddForKeno");

                    return;
                }

                for (int j = 2; j <= Degree; j++)
                {

                    IsuseStartTime = IsuseEndTime;
                    IsuseEndTime = IsuseEndTime.AddMinutes(Interval);
                    // 第 2151880 期时间。
                        DateTime timing1 = new DateTime(2017,6, 13);
                        // 计算期号。
                        Isuse = (2151465 + (new TimeSpan(Convert.ToDateTime(tbDate.Text).Ticks - timing1.Ticks).Days + i) * 395 + (j - 1)).ToString();
                   

                    int Results = -1;
                    Results = DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, IsuseStartTime, IsuseEndTime, IsuseEndTime, Explanation, "", ref NewIsuseID, ref ReturnDescription);
                    if (Results < 0)
                    {
                        break;
                    }
                }

                StartDate = StartDate.AddDays(1);
            }
           
        }
        else if (SLS.Lottery.BJXY28.ID==LotteryID)
        {

            double Interval = 5;
            string FirstEndTime = "09:05:00";
            int Degree = 179;

            for (int i = 0; i < Days; i++)
            {
               
                string todayDate = StartDate.Date.ToString("yyyy-MM-dd");
                // string sDate = StartDate.Date.ToString("yyyy-MM-dd");
                DataTable dt = new DAL.Tables.T_Isuses().Open("[ID]", "StartTime between '" + todayDate + " 09:00:00' and '" + todayDate + " 23:51:00' and LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(tbLotteryID.Value), "");

                if (dt == null)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAddForKeno");

                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    StartDate = StartDate.AddDays(1);

                    continue;
                }

                DateTime IsuseStartTime = DateTime.Parse(StartDate.Date.ToShortDateString() + " 09:00:00");
                DateTime IsuseEndTime = DateTime.Parse(StartDate.Date.ToShortDateString() + " " + FirstEndTime);



                DateTime timing = new DateTime(2017, 6, 19);
                // 计算期号。
                string Isuse = (829568 + (new TimeSpan(Convert.ToDateTime(tbDate.Text).Ticks - timing.Ticks).Days + i) * Degree).ToString();

                long NewIsuseID = -1;
                string ReturnDescription = "";

                int Result = DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, IsuseStartTime, IsuseEndTime, IsuseEndTime, Explanation, "", ref NewIsuseID, ref ReturnDescription);

                if (Result < 0)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAddForKeno");

                    return;
                }

                if (NewIsuseID < 0)
                {
                    PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_IsuseAddForKeno");

                    return;
                }

                for (int j = 2; j <= Degree; j++)
                {

                    IsuseStartTime = IsuseEndTime;
                    IsuseEndTime = IsuseEndTime.AddMinutes(Interval);
                    // 第 2151880 期时间。
                    DateTime timing1 = new DateTime(2017, 6, 19);
                    // 计算期号。
                    Isuse = (829568 + (new TimeSpan(Convert.ToDateTime(tbDate.Text).Ticks - timing1.Ticks).Days + i) * Degree + (j - 1)).ToString();


                    int Results = -1;
                    Results = DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, IsuseStartTime, IsuseEndTime, IsuseEndTime, Explanation, "", ref NewIsuseID, ref ReturnDescription);
                    if (Results < 0)
                    {
                        break;
                    }
                }

                StartDate = StartDate.AddDays(1);
            }

        }

        //腾讯分分彩 
        else if (100 == LotteryID)
        {

            string IntervalType = DAL.Functions.F_GetLotteryIntervalType(LotteryID);
            int Interval = 1;
            string FirstEndTime = IntervalType.Substring(IntervalType.IndexOf(";") + 1, 8);
            int Degree = 1440;

            if (FirstEndTime.EndsWith(";"))
            {
                FirstEndTime = FirstEndTime.Substring(0, FirstEndTime.Length - 1);
            }

           

            for (int i = 0; i < Days; i++)
            {
                string sDate = StartDate.Date.ToString("yyyy-MM-dd");
                DataTable dt = new DAL.Tables.T_Isuses().Open("[ID]", "StartTime between '" + sDate + " 0:0:0' and '" + sDate + " 23:59:59' and LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(tbLotteryID.Value), "");

                if (dt == null)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAddForKeno");

                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    StartDate = StartDate.AddDays(1);

                    continue;
                }

                DateTime IsuseStartTime = DateTime.Parse(StartDate.Date.ToShortDateString() + " 0:0:0");
                DateTime IsuseEndTime = IsuseStartTime.AddMinutes(Interval);
                string Isuse = ConvertIsuseName(LotteryID, StartDate.Year.ToString() + StartDate.Month.ToString().PadLeft(2, '0') + StartDate.Day.ToString().PadLeft(2, '0') + StartDate.Hour.ToString().PadLeft(2, '0') + IsuseEndTime.Minute.ToString().PadLeft(2, '0'));

                
               

                
                long NewIsuseID = -1;
                string ReturnDescription = "";

                int Result = DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, IsuseStartTime, IsuseEndTime, IsuseEndTime, Explanation, "", ref NewIsuseID, ref ReturnDescription);

                if (Result < 0)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAddForKeno");

                    return;
                }

                if (NewIsuseID < 0)
                {
                    PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_IsuseAddForKeno");

                    return;
                }

                for (int j = 2; j <= Degree; j++)
                {
                    IsuseStartTime = IsuseEndTime;
                    IsuseEndTime = IsuseEndTime.AddMinutes(Interval);
                    Isuse = Isuse = IsuseEndTime.Year.ToString() + IsuseEndTime.Month.ToString().PadLeft(2, '0') + IsuseEndTime.Day.ToString().PadLeft(2, '0') + IsuseEndTime.Hour.ToString().PadLeft(2, '0') + IsuseEndTime.Minute.ToString().PadLeft(2, '0');




                    int Results = -1;
                    Results = DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, IsuseStartTime, IsuseEndTime, IsuseEndTime, Explanation, "", ref NewIsuseID, ref ReturnDescription);
                    if (Results < 0)
                    {
                        break;
                    }
                }

                StartDate = StartDate.AddDays(1);
            }


            
        }
        else
        {
            

            string IntervalType = DAL.Functions.F_GetLotteryIntervalType(LotteryID);
            int Interval = int.Parse(IntervalType.Substring(1, IntervalType.IndexOf(";") - 1));
            string FirstEndTime = IntervalType.Substring(IntervalType.IndexOf(";") + 1, 8);
            int Degree = int.Parse(IntervalType.Substring(IntervalType.LastIndexOf(";") + 1));

            if (FirstEndTime.EndsWith(";"))
            {
                FirstEndTime = FirstEndTime.Substring(0, FirstEndTime.Length - 1);
            }

            for (int i = 0; i < Days; i++)
            {
                string sDate = StartDate.Date.ToString("yyyy-MM-dd");
                DataTable dt = new DAL.Tables.T_Isuses().Open("[ID]", "StartTime between '" + sDate + " 0:0:0' and '" + sDate + " 23:59:59' and LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(tbLotteryID.Value), "");

                if (dt == null)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAddForKeno");

                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    StartDate = StartDate.AddDays(1);

                    continue;
                }

                DateTime IsuseStartTime = DateTime.Parse(StartDate.Date.ToShortDateString() + " 0:0:0");
                DateTime IsuseEndTime = DateTime.Parse(StartDate.Date.ToShortDateString() + " " + FirstEndTime);
                string Isuse = ConvertIsuseName(LotteryID, StartDate.Year.ToString() + StartDate.Month.ToString().PadLeft(2, '0') + StartDate.Day.ToString().PadLeft(2, '0') + "01");


                long NewIsuseID = -1;
                string ReturnDescription = "";

                int Result = DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, IsuseStartTime, IsuseEndTime, IsuseEndTime, Explanation, "", ref NewIsuseID, ref ReturnDescription);

                if (Result < 0)
                {
                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_IsuseAddForKeno");

                    return;
                }

                if (NewIsuseID < 0)
                {
                    PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_IsuseAddForKeno");

                    return;
                }

                for (int j = 2; j <= Degree; j++)
                {
                    IsuseStartTime = IsuseEndTime;
                    IsuseEndTime = IsuseEndTime.AddMinutes(Interval);
                    
                    Isuse = ConvertIsuseName(LotteryID, StartDate.Year.ToString() + StartDate.Month.ToString().PadLeft(2, '0') + StartDate.Day.ToString().PadLeft(2, '0') + j.ToString().PadLeft(2, '0'));
                    

                    int Results = -1;
                    Results = DAL.Procedures.P_IsuseAdd(LotteryID, Isuse, IsuseStartTime, IsuseEndTime, IsuseEndTime, Explanation, "", ref NewIsuseID, ref ReturnDescription);
                    if (Results < 0)
                    {
                        break;
                    }
                }

                StartDate = StartDate.AddDays(1);
            }
        }

        this.Response.Redirect("Isuse.aspx?LotteryID=" + tbLotteryID.Value, true);
    }

    protected void btnBack_Click(object sender, System.EventArgs e)
    {
        this.Response.Redirect("Isuse.aspx?LotteryID=" + tbLotteryID.Value, true);
    }

    private string ConvertIsuseName(int LotteryID, string IsuseName)
    {
        return IsuseName;
    }

}
