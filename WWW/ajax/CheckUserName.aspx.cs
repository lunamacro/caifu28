using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ajax_CheckUserName : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string UserName = "";

        if (!string.IsNullOrEmpty(Request["UserName"]))
        {
            UserName = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("UserName"));
        }
        string operate = "";
        if (!string.IsNullOrEmpty(Request["operate"]))
        {
            operate = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("operate"));
        }
        if ("ValidateUserName" == operate)
        {
            //System.Threading.Thread.Sleep(10000);
            long result = new DAL.Tables.T_Users().GetCount("Name='" + UserName + "' or Mobile = '" + UserName + "'");
            if (result > 0)
            {
                Response.Write("{\"error\": \"0\"}");
            }
            else
            {
                Response.Write("{\"error\": \"-1\"}");
            }
            Response.End();
        }

        if (!string.IsNullOrEmpty(UserName))
        {
            string sql = "SELECT * FROM T_Users WHERE Name='" + UserName + "' OR Mobile='" + UserName + "'";
            int result = Convert.ToInt32(Shove.Database.MSSQL.ExecuteScalar(sql));
            if (result <= 0)
            {
                Response.Write("{\"message\": \"用户不存在\"}");
                Response.End();
            }
        }

        Users _User = Users.GetCurrentUser(1);

        string action = "";

        if (!string.IsNullOrEmpty(Request["action"]))
        {
            action = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("action"));
        }

        if (action.Equals("loginout"))
        {

            string ReturnDescption = "";
            int Result = 0;

            if (_User != null)
            {
                Result = _User.Logout(ref ReturnDescption);
            }

            if (_User == null)
            {
                Response.Write("{\"message\": \"-1\"}");
                Response.End();
            }

            if (Result < 0 || ReturnDescption != "")
            {
                Response.Write("{\"message\": \"退出失败，请重新退出。\"}");
                Response.End();
            }

            Response.Write("{\"message\": \"-1\",\"name\": \"\",\"Balance\": \"0\",\"ismanager\": \"False\" }");
            Response.End();
        }

        if (_User == null)
        {
            Response.Write("{\"message\": \"-1\",\"name\": \"\",\"Balance\": \"0\" ,\"ismanager\": \"False\" }");
            Response.End();
        }

        DAL.Tables.T_Options TOptions = new DAL.Tables.T_Options();
        DataTable dt = TOptions.Open("[Value]", "[ID]=32005", "");
        if (dt.Rows[0]["Value"].ToString().ToLower() == "false")
        {
            Response.Write("{\"message\": \"" + _User.ID.ToString() + "\",\"name\": \"" + _User.Name + "\",\"Balance\": \"" + _User.Balance.ToString() + "\" ,\"ismanager\": \"false\" }");
            Response.End();
        }
        Response.Write("{\"message\": \"" + _User.ID.ToString() + "\",\"name\": \"" + _User.Name + "\",\"Balance\": \"" + _User.Balance.ToString() + "\" ,\"ismanager\": \"" + (_User.UserType==99).ToString() + "\" }");
        Response.End();
    }
}