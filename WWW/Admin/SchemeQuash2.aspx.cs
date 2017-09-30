using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Shove.Database;
using System.Threading;
using System.Text.RegularExpressions;

public partial class Admin_SchemeQuash2 : AdminPageBase
{
    public int pageSize = 5;
    public int PageIndex = 1;
    public long PageCount = 1;//总页数
    public long DataCount = 1;//总数据
    double ScheduleOfAllCanNotQuashScheme = 80; //管理员可以撤单的进度

    protected void Page_Load(object sender, EventArgs e)
    {
        ScheduleOfAllCanNotQuashScheme = Shove._Convert.StrToDouble(_Site.SiteOptions["ScheduleOfAllCanNotQuashScheme"].ToString(""), 0.8) * 100;
        PageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("PageIndex"), 1);

        if (!this.IsPostBack)
        {
            string searchType = Shove._Web.Utility.GetRequest("searchType");
            if ("userName" == searchType && !chk_userName.Checked)
            {
                chk_userName.Checked = true;
            }
            else
            {
                chk_schemeNumber.Checked = true;
            }
            BindDataForLottery();
            ddlLottery.SelectedValue = Shove._Web.Utility.GetRequest("LotteryID");
            BindData();
        }
    }

    private void BindData()
    {
        DAL.Tables.T_Isuses T_Isuses = new DAL.Tables.T_Isuses();
        //查出某个彩种的当前期
        DataTable dt = T_Isuses.Open("top 1 ID ", " GETDATE()  betWeen StartTime and EndTime AND LotteryID=" + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) + "", " EndTime ");
        if (null != dt && dt.Rows.Count > 0)
        {
            string Condition = "SiteID = " + _Site.ID.ToString() + " and IsuseID = " + dt.Rows[0]["ID"].ToString() + " and QuashStatus = 0 and Buyed = 0 and Schedule < 100 ";
            string schemeNumberAndUserName = this.txt_schemeNumberAndUserName.Text.Trim();
            if ("" != schemeNumberAndUserName)
            {
                if (chk_schemeNumber.Checked)
                {
                    Condition += " and SchemeNumber='" + this.txt_schemeNumberAndUserName.Text.Trim() + "'";
                }
                else if (chk_userName.Checked)
                {
                    Condition += " and InitiateName='" + this.txt_schemeNumberAndUserName.Text.Trim() + "'";
                }
            }
            DataCount = new DAL.Views.V_SchemeSchedules().GetCount(Condition);
            PageCount = ((DataCount - 1) / pageSize) + 1;
            string Cmd = "select top " + pageSize + " * from (";
            Cmd += "select ID,LotteryName,InitiateName,SchemeNumber,Multiple,[Money],Schedule,ROW_NUMBER() over(order by [money] desc) as RowNumber";
            Cmd += " from V_SchemeSchedules ";
            Cmd += " where " + Condition + " and Schedule < " + ScheduleOfAllCanNotQuashScheme; ;
            Cmd += " )as NewTable where Schedule < 100 and RowNumber > (" + PageIndex + " - 1 ) * " + pageSize + " and RowNumber <= " + PageIndex + " * " + pageSize + "";
            dt = MSSQL.Select(Cmd);
        }
        this.rpt_list.DataSource = dt;
        this.rpt_list.DataBind();
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        PageIndex = 1;
        BindData();
    }

    protected void btnQuash_click(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        if (null != btn)
        {
            string id = ((HiddenField)btn.Parent.FindControl("hide_id")).Value;
            string schedule = ((HiddenField)btn.Parent.FindControl("hide_Schedule")).Value;
            if ("" != id && (new Regex("^\\d+$").IsMatch(id)))
            {
                Double lockSpeed = 0;//管理员不可撤单方案进度
                string RedirectUrl = "SchemeQuash2.aspx?LotteryID=" + ddlLottery.SelectedValue + "&PageIndex=" + PageIndex + "&searchType=" + (chk_schemeNumber.Checked ? "schemeNumber" : "userName");
                DAL.Tables.T_Schemes scheme = new DAL.Tables.T_Schemes();
                DAL.Tables.T_BuyDetails buyDetails = new DAL.Tables.T_BuyDetails();
                DAL.Tables.T_Users users = new DAL.Tables.T_Users();
                if (_Site.SiteOptions["ScheduleOfAllCanNotQuashScheme"].Value != null)
                {
                    lockSpeed = Convert.ToDouble(_Site.SiteOptions["ScheduleOfAllCanNotQuashScheme"].Value);
                }
                scheme.QuashStatus.Value = 2;
                buyDetails.QuashStatus.Value = 2;
                if (Convert.ToDouble(schedule) >= lockSpeed * 100.00)
                {
                    string msg = "方案撤单失败。原因：进度已经达到系统预定的刻度（{0}%）";
                    msg = string.Format(msg, lockSpeed * 100);
                    Shove._Web.JavaScript.Alert(this, msg, RedirectUrl);
                }
                else
                {
                    int returnValue = 0;
                    string returnDesc = "";
                    DAL.Procedures.P_QuashScheme(_Site.ID, Convert.ToInt32(id), true, true, ref returnValue, ref returnDesc);
                    //撤单状态修改
                    if (scheme.Update("ID=" + id) >= 0)
                    {
                        Shove._Web.JavaScript.Alert(this, "方案撤单成功。", RedirectUrl);
                    }
                    else
                    {
                        Shove._Web.JavaScript.Alert(this, "方案撤单失败。", RedirectUrl);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 绑定彩种列表
    /// </summary>
    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ")", "[Order]");

        if (dt != null && dt.Rows.Count > 0)
        {
            this.ddlLottery.DataValueField = "ID";
            this.ddlLottery.DataTextField = "Name";
            this.ddlLottery.DataSource = dt;
            this.ddlLottery.DataBind();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.SchemeQuash);

        base.OnInit(e);
    }

    #endregion

    /// <summary>
    /// 搜索按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_search_Click(object sender, EventArgs e)
    {
        PageIndex = 1;
        this.BindData();
    }
}