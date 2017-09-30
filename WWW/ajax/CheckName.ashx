<%@ WebHandler Language="C#" Class="CheckName" %>

using System;
using System.Web;
using System.Data;

using System.Text;

using Shove.Database;

public class CheckName : IHttpHandler
{
    public void ProcessRequest (HttpContext context) {
        //不让浏览器缓存
        context.Response.Buffer = true;
        context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
        context.Response.AddHeader("pragma", "no-cache");
        context.Response.AddHeader("cache-control", "");
        context.Response.CacheControl = "no-cache";
        context.Response.ContentType = "json/plain";

        string UserName = "";
        string mobile = "";

        if (string.IsNullOrEmpty(context.Request["UserName"]))
        {
            context.Response.Write("{\"message\": \"-1\"}");
            context.Response.End();
        }

        UserName = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "UserName"));
        mobile = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "Mobile"));

        if (!new CheckName_ashx().IsKeyWords(UserName) || !(new CheckName_ashx().IsKeyWords(mobile)))
        {
            context.Response.Write("{\"message\": \"-4\"}"); //包含敏感字符
            context.Response.End();

            return;
        }

        if (!string.IsNullOrEmpty(mobile))
        {
            DataTable dt_Mobile = new DAL.Tables.T_Users().Open("ID", "Name = '" + Shove._Web.Utility.FilteSqlInfusion(mobile) + "' OR Mobile = '" + Shove._Web.Utility.FilteSqlInfusion(mobile) + "'", "");

            if ((dt_Mobile != null && dt_Mobile.Rows.Count > 0))
            {
                context.Response.Write("{\"message\": \"-5\"}");
                context.Response.End();

                return;
            }
        }

        DataTable dt = new DAL.Tables.T_Users().Open("ID", "Name = '" + Shove._Web.Utility.FilteSqlInfusion(UserName) + "'", "");

        if ((dt != null && dt.Rows.Count > 0))
        {
            string UserName1 = UserName + DateTime.Now.Year.ToString();
            string UserName2 = UserName + DateTime.Now.Month.ToString();
            string UserName3 = UserName + DateTime.Now.Day.ToString();

            while (true)
            {
                dt = new DAL.Tables.T_Users().Open("ID", "Name ='" + UserName1 + "' or Name='" + UserName2 + "' or Name='" + UserName3 + "'", "");

                if (dt.Rows.Count < 1 && new CheckName_ashx().IsKeyWords(UserName1))
                {
                    break;
                }

                if (!new CheckName_ashx().IsKeyWords(UserName1))
                {
                    UserName = new CheckName_ashx().RandomNumber() + new CheckName_ashx().RandomNumber() + new CheckName_ashx().RandomNumber() + new CheckName_ashx().RandomNumber();
                }

                UserName1 = UserName + new CheckName_ashx().RandomNumber();
                UserName2 = UserName + new CheckName_ashx().RandomNumber() + DateTime.Now.Year.ToString();
                UserName3 = UserName + new CheckName_ashx().RandomNumber() + DateTime.Now.Month.ToString();
            }

            context.Response.Write("{\"message\": \"-2\", \"UserName1\": \"" + UserName1 + "\", \"UserName2\": \"" + UserName2 + "\", \"UserName3\": \"" + UserName3 + "\"}");
            context.Response.End();
            return;
        }

        if (Shove._String.GetLength(UserName) < 5 || Shove._String.GetLength(UserName) > 16)
        {
            context.Response.Write("{\"message\": \"-3\"}");
            context.Response.End();
            return;
        }
        
        context.Response.Write("{\"message\": \"0\"}");
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}