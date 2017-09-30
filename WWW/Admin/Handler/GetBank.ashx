<%@ WebHandler Language="C#" Class="GetBank" %>

using System;
using System.Web;

public class GetBank : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        //银行卡号
        string BankNo = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("BankNo"));

        string BankName = Bank.GetBankName(BankNo);

        if (string.IsNullOrEmpty(BankName) == false)
        {
            context.Response.Write("{\"IsOk\":true,\"BankName\":\"" + BankName + "\"}");
        }
        else
        {
            BankName = "未能识别银行卡号，请手动选择开户银行。";

            context.Response.Write("{\"IsOk\":false,\"BankName\":\"" + BankName + "\"}");
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}