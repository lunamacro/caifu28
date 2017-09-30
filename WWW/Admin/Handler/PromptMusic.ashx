<%@ WebHandler Language="C#" Class="PromptMusic" %>

using System;
using System.Web;
using System.Media;
using System.Text;
using System.Data;

public class PromptMusic : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        
        StringBuilder sbCount = new StringBuilder();

        
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("SELECT COUNT(ID) AS [count] FROM dbo.T_UserDistills WHERE Result=0");
        int count = 0;
        object o = Shove.Database.MSSQL.ExecuteScalar(sb.ToString());
        if (o != null)
        {
            count = Shove._Convert.StrToInt(o.ToString(), 0); 
        }
        sbCount.Append(count + ",");
        
        count = 0;
        object o2 = Shove.Database.MSSQL.ExecuteScalar("SELECT COUNT(ID) AS [count] FROM dbo.T_UserPayDetails WHERE Result=0");
        if (o2 != null)
        {
            count = Shove._Convert.StrToInt(o2.ToString(), 0);
        }
        sbCount.Append(count);

        context.Response.Clear();
        context.Response.Write(sbCount.ToString());
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}