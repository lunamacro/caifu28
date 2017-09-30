<%@ WebHandler Language="C#" Class="AppGateway" %>

using System;
using System.Web;
using System.Data;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Web.Security;
using System.Xml;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Shove.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NetDimension.Weibo;
using System.Configuration;


public class AppGateway : IHttpHandler
{
    Log log = new Log("AppGateway");

    StackTrace stackTrace;
    StackFrame stackFrame;
    string typeName;

    public void ProcessRequest(HttpContext context)
    {
        // 声明接受所有域的请求
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        context.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST");
        context.Response.Clear();
        context.Response.ContentType = "application/json";
        
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }


}

