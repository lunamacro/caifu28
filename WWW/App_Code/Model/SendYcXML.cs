using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;

public class message
{
    head Head = new head();
    string Body = "";

    public head head { get { return Head; } set { Head = value; } }
    public string body { get { return Body; } set { Body = value.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>",""); } }   
}

public class head
{
    public head()
    {
        string agentId1 = Shove._Web.WebConfig.GetAppSettingsString("ycAgentId");//"yc6636";
        string key = Shove._Web.WebConfig.GetAppSettingsString("ycKey");// "201305a7ffc826736";

        messageId = agentId1 + DateTime.Now.ToString("yyyyMMdd") + new Random().Next(999999);
        agentId = agentId1;
        digest = FormsAuthentication.HashPasswordForStoringInConfigFile(messageId + agentId1 + key, "MD5");
    }

    string MessageId = "";
    string AgentId = "";
    string Digest = "";

    public string messageId { get { return MessageId; } set { MessageId = value; } }
    public string agentId { get { return AgentId; } set { AgentId = value; } }
    public string digest { get { return Digest; } set { Digest = value; } }
}

public class LoginSessionId
{
    string sessionId = "";
    public string SessionId
    {
        get
        {
            MD5DES md5 = new MD5DES();
            sessionId = md5.MD5Decrypt(PF.GetCookieValue("SLS.Center.LoginSessionID"), "7??`?U?");
            return sessionId;
        }
        set
        {
            sessionId = value;
            MD5DES md5 = new MD5DES();
            PF.SetCookie("SLS.Center.LoginSessionID", md5.MD5Encrypt(sessionId, "7??`?U?"), 365);
        }
    }
}

public class MoneyMessage
{
    double ableBalance = 0;
    double freezeBalance = 0;
    double takeCashQuota = 0;
    double ableScore = 0;

    public double AbleBalance { get { return ableBalance; } set { ableBalance = value; } }
    public double FreezeBalance { get { return freezeBalance; } set { freezeBalance = value; } }
    public double TakeCashQuota { get { return takeCashQuota; } set { takeCashQuota = value; } }
    public double AbleScore { get { return ableScore; } set { ableScore = value; } }
}