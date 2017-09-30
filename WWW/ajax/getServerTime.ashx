<%@ WebHandler Language="C#" Class="getServerTime" %>

using System;
using System.Web;

public class getServerTime : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
	context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        context.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST");
        context.Response.Clear();
       
        context.Response.ContentType = "text/plain";
        context.Response.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}