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
using System.Text;

public partial class Admin_Handsel : AdminPageBase
{
    private DataSet ds = null;
    public int PageIndex;//当前页
    public int PageCount;//总页数
    public int DataCount;//总数据
    protected void Page_Load(object sender, EventArgs e)
    {
        BindData();
    }
    protected void rptSchemes_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Del")
        {
            DAL.Tables.T_StationSMS t = new DAL.Tables.T_StationSMS();
            t.Delete("ID= " + e.CommandArgument);
            return;
        }

        if (e.CommandName == "CHK")
        {
            string id = e.CommandArgument.ToString();
        }
    }
    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.HandselManage);

        base.OnInit(e);
    }

    #endregion
    protected void ShoveConfirmButton_Click(object sender, EventArgs e)
    {
        BindData();
    }
    private void BindData()
    {
        StringBuilder condition = new StringBuilder();
        condition.Append("1=1 AND ");

        DateTime startTime, endTime;
        string giveObj = selGiveObj.Value;
        string giveType = selGiveType.Value;

        #region 验证
        if (txtStartTime.Value.Length > 0 && txtEndTime.Value.Length > 0)
        {
            startTime = Convert.ToDateTime(txtStartTime.Value);
            endTime = Convert.ToDateTime(txtEndTime.Value);
            if (endTime < startTime)
            {
                Shove._Web.JavaScript.Alert(this, "开始时间不可以大于结束时间");
                return;
            }
        }
        #endregion

        #region 条件
        if (txtStartTime.Value.Length > 0)
        {
            condition.Append("StartTime>='" + txtStartTime.Value + " 00:00:00' AND ");
        }
        if (txtEndTime.Value.Length > 0)
        {
            condition.Append("EndTime<='" + txtEndTime.Value + " 23:59:59' AND ");
        }
        if (selGiveObj.Value == "0")
        {
            condition.Append("GiveObject=0 AND ");
        }
        if (selGiveObj.Value == "1")
        {
            condition.Append("GiveObject=1 AND ");
        }
        if (selGiveType.Value == "0")
        {
            condition.Append("GiveType=0 AND ");
        }
        if (selGiveType.Value == "1")
        {
            condition.Append("GiveType=1 AND ");
        }
        condition = condition.Remove(condition.Length - 4, 4);
        #endregion

        int intResult = -1;
        int TotalRowCount = 0;
        int PageCountt = 0;

        PageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("pageindex"), 1);
        DataSet ds = null;

        intResult = DAL.Procedures.P_Pager(ref ds, PageIndex, 10, 0, "*",
             "T_HandselRule", "StartTime,EndTime desc", "" + condition.ToString() + "", "ID", ref TotalRowCount, ref PageCountt);

        DataTable dt = null;
        if (ds.Tables.Count > 0)
        {
            dt = ds.Tables[0];
        }

        DataTable dt_Stat = Shove.Database.MSSQL.Select("select count(*) as coun from T_HandselRule where " + condition.ToString());
        if (dt_Stat == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_StationSMSList");

            return;
        }
        DataCount = Shove._Convert.StrToInt(dt_Stat.Rows[0]["coun"].ToString(), 0);
        int pagsizeq = DataCount % 10;

        if (pagsizeq == 0)
        {

            PageCount = DataCount / 10;

        }
        else
        {
            PageCount = (DataCount / 10) + 1;
        }


        this.rptSchemes.DataSource = dt;
        this.rptSchemes.DataBind();
    }
    public string JudgeObjectType(string giveObject)
    {
        if (giveObject == "0")
        {
            return "新用户";
        }
        else if (giveObject == "1")
        {
            return "老用户";
        }
        else
        {
            return "所有用户";
        }
    }
}