<%@ WebHandler Language="C#" Class="WxOauth2" %>

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
using UlinePayUtil;
using System.Resources;
using System.Reflection;

public class WxOauth2 : IHttpHandler
{
    Log log = new Log("wx");
    private Dictionary<string, string> cfg = new Dictionary<string, string>(1);
    long timeStamp = 0;

    string domainName = "cf28.560651.com";


    public void ProcessRequest(HttpContext context)
    {
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        context.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST");
        context.Response.Clear();
        context.Response.ContentType = "application/json";

        string code = Shove._Web.Utility.GetRequest("code");
        string FromClient = Shove._Web.Utility.GetRequest("FromClient");
        string RefId = Shove._Web.Utility.GetRequest("RefId");
        
        if (string.IsNullOrEmpty(code))
        {
            context.Response.Write(buildCallBack(false, "code is null"));
            return;
        }
        //log.Write("code:" + code);

        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        timeStamp = (long)(DateTime.Now - startTime).TotalSeconds; // 相差秒数

        Regex regex = new Regex(@".{32}");
        bool checkCode = regex.IsMatch(code);
        if (!checkCode)
        {
            context.Response.Write(buildCallBack(false, "code非法格式"));
            return;
        }

        XmlDocument xml = new XmlDocument();
        xml.Load(context.Request.PhysicalApplicationPath + "/Properties/wx.xml");
        XmlNode xmlNode = xml.SelectSingleNode("root");

        string APPID = xmlNode.FirstChild.InnerText;
        string SECRET = xmlNode.LastChild.InnerText;




        if (string.IsNullOrEmpty(APPID) || string.IsNullOrEmpty(SECRET))
        {
            log.Write("资源文件读取失败");
            context.Response.Write(buildCallBack(false, "资源文件读取失败"));
            return;
        }
        StringBuilder sbUrl = new StringBuilder("https://api.weixin.qq.com/sns/oauth2/access_token?");
        sbUrl.Append("appid=" + APPID);
        sbUrl.Append("&secret=" + SECRET);
        sbUrl.Append("&code=" + code);
        sbUrl.Append("&grant_type=authorization_code");
        string url = sbUrl.ToString();
        string resultGetAccessToken = HttpService.Get(url);

        if (resultGetAccessToken.IndexOf("errcode") > 0)
        {
            log.Write("用code获取accessToken失败" + resultGetAccessToken);
            context.Response.Write(buildCallBack(false, resultGetAccessToken));
            return;
        }
        else
        {
            //WxGetAccessToken
            WxGetAccessToken wxGetAccessToken = (WxGetAccessToken)JsonConvert.DeserializeObject(resultGetAccessToken, typeof(WxGetAccessToken));

            string openid = wxGetAccessToken.openid;
            //遍历用户是否已经注册过

            //未注册过
            string ACCESS_TOKEN = wxGetAccessToken.access_token;
            url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + ACCESS_TOKEN + "&openid=" + openid;
            string resultJson = HttpService.Get(url);

            if (resultJson.IndexOf("errcode") > 0)
            {
                //获取用户资料出错
                log.Write("获取用户资料出错" + resultJson);
                context.Response.Write(buildCallBack(false, resultJson));
                return;
            }
            else
            {
                //获取用户资料成功
                WxUserInfo wxUserInfo = (WxUserInfo)JsonConvert.DeserializeObject(resultJson, typeof(WxUserInfo));

               // DataTable dt_users = new DAL.Tables.T_Users().Open("", "Memo='" + wxUserInfo.unionid + "'", "");
                string querySql = "select * from T_Users where wxopenid='" + wxUserInfo.unionid.Trim() + "'";
                log.Write(querySql);
                DataTable dt_users =Shove.Database.MSSQL.Select(querySql);

                string accessToken = PF.EncryptPassword(timeStamp.ToString());
                
                //未注册过，注册新用户
                if (dt_users == null || dt_users.Rows.Count==0)
                {
                    if (string.IsNullOrEmpty(RefId) || int.Parse(RefId)<=0)
                    {
                        context.Response.Write(buildCallBack(false, "推荐人信息错误"));
                        return;
                    }

                    Users refferUser = new Users(1)[1, int.Parse(RefId)];
                    if (refferUser.ID < 0)
                    {
                        context.Response.Write(buildCallBack(false, "推荐人ID不存在"));
                        return;
                    }
                    if (refferUser.isAgent == 0)
                    {
                        context.Response.Write(buildCallBack(false, "此推荐人无推荐资格"));
                        return;
                    }
                    
                    
                    
                    Users user = new Users(1);
                    user.Name = "wx"+timeStamp; //过滤特殊字符
                    user.NickName = wxUserInfo.nickname;
                    user.accessToken = accessToken;
                    user._password = accessToken;
                    user._passwordadv = "";
                    user.FromClient = int.Parse(FromClient);
                    user.HeadUrl = wxUserInfo.headimgurl.Trim();
                    user.isAgent = 0;
                    user.agentGroup = refferUser.agentGroup;
                    user.ReferId = (int)refferUser.ID;
                    user.wxopenid = wxUserInfo.unionid.Trim();
                    string ReturnDescription = "";
                    int resultUid = user.Add(ref ReturnDescription);

                    if (resultUid < 0)
                    {
                        context.Response.Write(buildCallBack(false, "会员注册不成功"));
                        return;
                    }
                    else
                    {
                        DataTable dt = new DAL.Tables.T_Users().Open("", "ID=" + resultUid, "");
                        if (dt == null || dt.Rows.Count <= 0)
                        {
                            context.Response.Write(buildCallBack(false, "用户表读取异常"));
                            return;
                        }
                        
                        DataRow row = dt.Rows[0];
                        StringBuilder sb = new StringBuilder();
                        long[] msgCount = GetTheNewMessageCount(row["ID"].ToString());
                        string freeze = (Convert.ToDouble(row["Freeze"]) + Convert.ToDouble(row["HandselForzen"])).ToString("0.00");
                        sb.Append("{\"error\":\"0\",");
                        sb.Append("\"msg\":\"\",");
                        sb.Append("\"timeStamp\":\"" + timeStamp + "\",");
                        sb.Append("\"accessToken\":\"" + accessToken + "\",");
                        sb.Append("\"uid\":\"" + dt.Rows[0]["ID"].ToString() + "\",");
                        sb.Append("\"Password\":\"" + dt.Rows[0]["Password"].ToString().ToUpper() + "\",");
                        sb.Append("\"PasswordAdv\":\"" + dt.Rows[0]["PasswordAdv"].ToString().ToUpper() + "\",");
                        sb.Append("\"Name\":\"" + dt.Rows[0]["Name"].ToString() + "\",");
                        sb.Append("\"NickName\":\"" + dt.Rows[0]["NickName"].ToString() + "\",");
                        sb.Append("\"Mobile\":\"" + dt.Rows[0]["Mobile"].ToString() + "\",");
                        sb.Append("\"Level\":\"" + dt.Rows[0]["Level"].ToString() + "\",");
                        sb.Append("\"isMobileValided\":\"" + dt.Rows[0]["isMobileValided"].ToString() + "\",");
                        sb.Append("\"HeadUrl\":\"" + dt.Rows[0]["HeadUrl"].ToString() + "\",");
                        sb.Append("\"GexingQianming\":\"" + dt.Rows[0]["GexingQianming"].ToString() + "\",");
                        sb.Append("\"BankAddress\":\"" + dt.Rows[0]["BankAddress"].ToString() + "\",");
                        sb.Append("\"BankName\":\"" + dt.Rows[0]["BankName"].ToString() + "\",");
                        sb.Append("\"BankCardNumber\":\"" + dt.Rows[0]["BankCardNumber"].ToString() + "\",");
                        sb.Append("\"UserCradName\":\"" + dt.Rows[0]["UserCradName"].ToString() + "\",");
                        sb.Append("\"Balance\":\"" + Shove._Convert.StrToDouble(dt.Rows[0]["Balance"].ToString(), 0).ToString("0.00") + "\",");
                        sb.Append("\"HandselAmount\":\"" + Shove._Convert.StrToDouble(dt.Rows[0]["HandselAmount"].ToString(), 0).ToString("0.00") + "\",");
                        sb.Append("\"isAgent\":\"" + dt.Rows[0]["isAgent"].ToString() + "\",");
                        sb.Append("\"ReferId\":\"" + dt.Rows[0]["ReferId"].ToString() + "\",");
                        sb.Append("\"agentGroup\":\"" + dt.Rows[0]["agentGroup"].ToString() + "\",");
                        sb.Append("\"isWXBind\":\"" + dt.Rows[0]["isWXBind"].ToString() + "\",");
                        sb.Append("\"RefferUrl\":\"http://" + domainName + "/AppGateWay/?pid=" + dt.Rows[0]["ID"].ToString() + "\",");
                        sb.Append("\"msgCount\":\"0\"}");
                        context.Response.Write(sb.ToString());
                        return;
                    }
                }
                //注册过，进行登录
                else
                {
                   
                    Users user = new Users(1)[1, int.Parse(dt_users.Rows[0]["ID"].ToString())];
                    user.NickName = wxUserInfo.nickname;
                    user.HeadUrl = wxUserInfo.headimgurl.Trim();
                    user.accessToken = accessToken;
                    string editDes = "";
                    if(user.EditByID(ref editDes)<0){
                        context.Response.Write(buildCallBack(false, "微信登录失败"));
                        return;
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.Append("{\"error\":\"0\",");
                    sb.Append("\"msg\":\"\",");
                    sb.Append("\"timeStamp\":\"" + timeStamp + "\",");
                    sb.Append("\"accessToken\":\"" + accessToken + "\",");
                    sb.Append("\"uid\":\"" + user.ID.ToString() + "\",");
                    sb.Append("\"Password\":\"" + user.Password.ToUpper() + "\",");
                    sb.Append("\"PasswordAdv\":\"" + user.PasswordAdv.ToUpper() + "\",");
                    sb.Append("\"Name\":\"" + user.Name.ToString() + "\",");
                    sb.Append("\"NickName\":\"" + user.NickName.ToString() + "\",");
                    sb.Append("\"Mobile\":\"" + user.Mobile.ToString() + "\",");
                    sb.Append("\"Level\":\"" + user.Level.ToString() + "\",");
                    sb.Append("\"isMobileValided\":\"" + user.isMobileValided.ToString() + "\",");
                    sb.Append("\"HeadUrl\":\"" + user.HeadUrl.ToString() + "\",");
                    sb.Append("\"GexingQianming\":\"" + user.GexingQianming.ToString() + "\",");
                    sb.Append("\"BankAddress\":\"" + user.BankAddress.ToString() + "\",");
                    sb.Append("\"BankName\":\"" + user.BankName.ToString() + "\",");
                    sb.Append("\"BankCardNumber\":\"" + user.BankCardNumber.ToString() + "\",");
                    sb.Append("\"UserCradName\":\"" + user.UserCradName.ToString() + "\",");
                    sb.Append("\"Balance\":\"" + Shove._Convert.StrToDouble(user.Balance.ToString(), 0).ToString("0.00") + "\",");
                    sb.Append("\"HandselAmount\":\"" + Shove._Convert.StrToDouble(user.HandselAmount.ToString(), 0).ToString("0.00") + "\",");
                    sb.Append("\"isAgent\":\"" + user.isAgent.ToString() + "\",");
                    sb.Append("\"ReferId\":\"" + user.ReferId.ToString() + "\",");
                    sb.Append("\"agentGroup\":\"" + user.agentGroup.ToString() + "\",");
                    sb.Append("\"isWXBind\":\"" + user.isWXBind.ToString() + "\",");
                    sb.Append("\"RefferUrl\":\"http://" + domainName + "/AppGateWay/?pid=" + user.ID.ToString() + "\",");
                    sb.Append("\"msgCount\":\"0\"}");
                    context.Response.Write(sb.ToString());
                    return;
                }
            }

        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private string buildCallBack(bool isSuccess, string text)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{\"status\":\"" + (isSuccess ? "success" : "fail") + "\",");
        sb.Append("\"msg\":" + text + "}");
        return sb.ToString();
    }


    private SortedDictionary<string, object> FromXml(string xml)
    {
        if (string.IsNullOrEmpty(xml))
        {
            log.Write("将空的xml串转换为WxPayData不合法!");
            throw new Exception("将空的xml串转换为WxPayData不合法!");
        }

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
        XmlNodeList nodes = xmlNode.ChildNodes;
        SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();
        foreach (XmlNode xn in nodes)
        {
            XmlElement xe = (XmlElement)xn;
            m_values[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
        }

        return m_values;
    }

    /// <summary>
    /// 返回未读站内信的总条数
    /// </summary>
    /// <param name="userID">用户ID</param>
    /// <returns></returns>
    public long[] GetTheNewMessageCount(string userID)
    {
        long[] result = new long[2];
        result[0] = new DAL.Tables.T_StationSMS().GetCount("isShow = 1 and AimID = " + userID); //全部站内性条数
        result[1] = new DAL.Tables.T_StationSMS().GetCount("isShow = 1 and isRead = 0 and AimID = " + userID);//未读条数

        return result;
    }
}