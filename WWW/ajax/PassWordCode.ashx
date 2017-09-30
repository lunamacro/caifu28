<%@ WebHandler Language="C#" Class="PassWordCode" %>

using System;
using System.Web;

public class PassWordCode : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        //不让浏览器缓存
        context.Response.Buffer = true;
        context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
        context.Response.AddHeader("pragma", "no-cache");
        context.Response.AddHeader("cache-control", "");
        context.Response.CacheControl = "no-cache";
        context.Response.ContentType = "text/plain";

        string RegCode = "";

        if (!string.IsNullOrEmpty(context.Request["RegCode"]))
        {
            RegCode = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest(context, "RegCode"));
        }

        if (!string.IsNullOrEmpty(RegCode))
        {
            try
            {
                string Key = "ASP.NET_SessionId_UL";
                HttpCookie cookieLoginUserID = HttpContext.Current.Request.Cookies[Key];
                if (String.IsNullOrEmpty(cookieLoginUserID.Value))
                {
                    context.Response.Write("{\"message\": \"-1\"}");//验证码已过期，请重新输入。
                    return;
                }
                if (cookieLoginUserID.Value != Shove._Security.Encrypt.MD5(PF.GetCallCert() + RegCode.ToLower()).ToLower())
                {
                    context.Response.Write("{\"message\": \"-2\"}");//验证码输入错误，请重新输入。
                    cookieLoginUserID.Expires = DateTime.Now.AddYears(-1);
                    return;
                }
            }
            catch (Exception e)
            {
            }
        }

        context.Response.Write("{\"message\":\"0\"}");
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}