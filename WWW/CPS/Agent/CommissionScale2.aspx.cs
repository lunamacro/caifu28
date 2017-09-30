using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
public partial class CPS_Agent_CommissionScale2 : CPSAgentBase
{
    public int pageSize = 20;
    public int pageIndex = 1;
    public long dataCount = 0;
    public long pageCount = 0;
    public string UserName = string.Empty;
    public long CpsID = -1;
    public int UserType = -1;
    private string startDate = "";
    private string endDate = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserName = Shove._Web.Utility.GetRequest("UserName");
            CpsID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("CpsID"), -1);
            UserType = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("UserType"), -1);
            if (-1 == CpsID || -1 == UserType)
            {
                Shove._Web.JavaScript.Alert(this, "参数错误");
                return;
            }

            BindUrlParams();
            if (startDate != "" && endDate != "" && DateTime.Parse(startDate) > DateTime.Parse(endDate))
            {
                Shove._Web.JavaScript.Alert(this, "截至时间不能小于开始时间");
                return;
            }
            this.BindData();
        }
    }

    /// <summary>
    /// 绑定url上面的参数值
    /// </summary>
    private void BindUrlParams()
    {
        pageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("PageIndex"), 1);

        this.hideCpsID.Value = CpsID + "";
        this.hideUserName.Value = UserName;
        this.hideUserType.Value = UserType + "";

        startDate = Shove._Web.Utility.GetRequest("StartTime");
        if ("" != startDate)
        {
            this.txtStartDate.Text = startDate;
            startDate += " 00:00:00";
        }
        endDate = Shove._Web.Utility.GetRequest("EndTime");
        if ("" != endDate)
        {
            this.txtEndDate.Text = endDate;
            endDate += " 23:59:59";
        }
    }

    /// <summary>
    /// 绑定数据。
    /// </summary>
    private void BindData()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(@"select owneruserid as CpsID,datetime HandlelDateTime,ROW_NUMBER() OVER(order by datetime desc) as RowNumber,a.memo [type] from T_CpsUsersBonusScaleLog a  where a.id in(
select max(id) from T_CpsUsersBonusScaleLog where OwnerUserID = " + CpsID + " group by memo)");
        if ("" != startDate)
        {
            sb.AppendFormat(" and a.[DateTime] >= '{0}'", startDate);
        }
        if ("" != endDate)
        {
            sb.AppendFormat("  and a.[DateTime] <= '{0}'", endDate);
        }
        string getCountSQL = string.Format("select COUNT(*) from({0}) as NewTable", sb.ToString());
        string getDataSQL = string.Format("select top {0} * from({1}) as NewTable where RowNumber > ({2} - 1) * {3} and RowNumber <= {4} * {5}", pageSize, sb.ToString(), pageIndex, pageSize, pageIndex, pageSize);
        object obj = Shove.Database.MSSQL.ExecuteScalar(getCountSQL);
        dataCount = (null == obj ? 0 : Shove._Convert.StrToLong(obj.ToString(), 0));
        pageCount = ((dataCount - 1) / pageSize) + 1;
        //Response.Write(getDataSQL);
        DataTable dt = Shove.Database.MSSQL.Select(getDataSQL);
        this.rpt_dataList.DataSource = dt;
        this.rpt_dataList.DataBind();
    }
}