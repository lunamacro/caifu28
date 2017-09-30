using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Shove.Database;
using System.Text;

public partial class CPS_Agent_AgentCommissionSet : CPSAgentBase
{

    private string userName = "";

    public int pageSize = 20;
    public int pageIndex = 1;
    public long dataCount = 0;
    public long pageCount = 0;

    public string getDataCountSQL = "select COUNT(1) from ({0}) as NewTable";
    public string pagingSQL = "select top {0} * from ({1})as newTable where rowNumber > (({2} - 1) * {3}) and rowNumber <= ({4} * {5})";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindUrlParams();
            bind();
        }
    }
    /// <summary>
    /// 绑定url上面的参数值
    /// </summary>
    private void BindUrlParams()
    {
        pageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("PageIndex"), 1);
        userName = Shove._Web.Utility.GetRequest("userName");
        if ("" != userName)
        {
            this.txtName.Value = userName.Trim();
        }
    }

    private void bind()
    {
        if (null != _User)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select u.ID as UserID,c.ID as CpsID,u.Name as UserName,u.RegisterTime,c.[Type]as UserType,ROW_NUMBER() over(order by u.RegisterTime)as rowNumber  ");
            sb.AppendLine("from T_Cps as c");
            sb.AppendLine("join T_Users as u on u.ID = c.OwnerUserID");
            sb.AppendLine("where ParentID = " + CPSBLL.getCpsIDByUserID(_User.ID) + " and c.[Type] = 2 and c.[ON] = 1 and c.HandleResult = 1");
            if ("" != userName)
            {
                sb.AppendLine(" and u.Name like '%" + userName + "%'");
            }

            dataCount = Shove._Convert.StrToLong(Shove.Database.MSSQL.ExecuteScalar(string.Format(getDataCountSQL, sb.ToString())) + "", 0);
            pageCount = CPSBLL.CalculateSumPageCount(pageSize, dataCount);
            pagingSQL = string.Format(pagingSQL, pageSize, sb.ToString(), pageIndex, pageSize, pageIndex, pageSize);
            DataTable dt = Shove.Database.MSSQL.Select(pagingSQL);
            this.rpt_dataList.DataSource = dt;
            this.rpt_dataList.DataBind();
        }
    }
}