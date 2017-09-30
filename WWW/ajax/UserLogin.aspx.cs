using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ajax_UserLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string UserName = "";

        if (!string.IsNullOrEmpty(Request["UserName"]))
        {
            UserName = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("UserName"));
        }

        string Password = "";
        int count = 2;

        if (!string.IsNullOrEmpty(Request["Password"]))
        {
            Password = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("Password"));
        }

        if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
        {
            string ReturnDescription = "";

            Users user = new Users(1);
            user.Name = UserName;
            user.Password = Password;

            string sql = @"SELECT a.* FROM T_SystemLog AS a INNER JOIN T_Users AS b ON a.UserID=b.ID
                        WHERE (b.Name='" + UserName + "' OR b.Mobile='" + UserName + "') AND a.[DateTime] > DATEADD(MINUTE,-10,GETDATE()) AND IsFrozenState > 0";
            DataTable dt_sys = Shove.Database.MSSQL.Select(sql);
            if (dt_sys != null && dt_sys.Rows.Count > 0)
            {
                count = 2 - dt_sys.Rows.Count;
            }

            int Result = 0;

            Result = user.Login(ref ReturnDescription);

            if (Result < 0)
            {
                if (count <= 0)
                {
                    Response.Write("{\"message\": \"" + ReturnDescription + "\"}");
                }
                else
                {
                    if (Result == -2 || Result == -5 || Result == -6)
                    {
                        Response.Write("{\"message\": \"" + ReturnDescription + "\"}");
                    }
                    else
                    {
                        Response.Write("{\"message\": \"" + ReturnDescription + "，您还有" + count + "次输入密码的机会\"}");
                    }
                }
                Response.End();
            }

            string sql_sys = "UPDATE T_SystemLog SET IsFrozenState=0 WHERE UserID=" + user.ID;
            int result = Shove.Database.MSSQL.ExecuteNonQuery(sql_sys, new Shove.Database.MSSQL.Parameter[0]);
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
            Response.Write("{\"message\": \"" + _User.ID.ToString() + "\",\"name\": \"" + _User.Name + "\",\"Balance\": \"" + _User.Balance.ToString() + "\" ,\"ismanager\": \"false\",\"HandselAmount\":\"" + _User.HandselAmount.ToString("0.00") + "\" }");
            Response.End();
        }
        if (string.IsNullOrEmpty(_User.Name))
        {
            Response.Write("{\"message\": \"" + _User.ID.ToString() + "\",\"name\": \"" + _User.Mobile + "\",\"Balance\": \"" + _User.Balance.ToString() + "\" ,\"ismanager\": \"" + (_User.Competences.CompetencesList != "").ToString() + "\",\"HandselAmount\":\"" + _User.HandselAmount.ToString("0.00") + "\",\"isCpsManager\":\"" + (_User.Competences.CompetencesList == "[CpsManage]") + "\" }");
            Response.End();
        }
        Response.Write("{\"message\": \"" + _User.ID.ToString() + "\",\"name\": \"" + _User.Name + "\",\"Balance\": \"" + _User.Balance.ToString() + "\" ,\"ismanager\": \"" + (_User.Competences.CompetencesList != "").ToString() + "\",\"HandselAmount\":\"" + _User.HandselAmount.ToString("0.00") + "\",\"isCpsManager\":\"" + (_User.Competences.CompetencesList == "[CpsManage]") + "\" }");
        Response.End();
    }
}
