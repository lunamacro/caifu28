using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Web.Security;
using System.Text;
using System.Net;
using System.Security.Cryptography;

using Shove.Database;
using Shove.Alipay;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

public class PF
{
    public const int SystemStartYear = 2008;
    public const int SchemeMaxBettingMoney = 10000000;

    public static string DesKey = Shove._Web.WebConfig.GetAppSettingsString("DesKey");
    public static string CenterMD5Key = Shove._Web.WebConfig.GetAppSettingsString("CenterMD5Key");

    public static string GetCallCert()
    {
        string Result = "";

        Result = Shove._Web.WebConfig.GetAppSettingsString("DllCallCert");

        return Result;
    }

    /// <summary>
    /// 计算这个月有多少天 根据年和月
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <returns>返回这个月有多少天</returns>
    public static int CalculateMonthHaveDayByYearAndMonth(int year, int month)
    {
        if (2 == month)
        {
            if (year % 4 == 0 && year % 100 != 0 || year % 400 == 0)    //判断是否是闰年  
            {
                return 29;
            }
            else
            {
                return 28;
            }
        }
        else
        {
            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)  //一系列的判断条件  
                return 31;
            else
                return 30;
        }
    }

    #region 6636方法
    /// <summary>
    /// 对象转xml
    /// </summary>
    /// <param name="obj">对象，model or list</param>
    /// <param name="type">typeof(obj)</param>
    /// <returns>xml</returns>
    public static string ObjectToXML(object obj, Type type)
    {
        string xmlStr = "";
        try
        {
            XmlSerializer x = new XmlSerializer(type);
            MemoryStream ms = new MemoryStream();
            XmlWriter xw = new XmlTextWriter(ms, Encoding.UTF8);
            XmlSerializerNamespaces _namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[] {
new XmlQualifiedName(string.Empty, "")});
            x.Serialize(xw, obj, _namespaces);
            int count = (int)ms.Length;
            byte[] arr = new byte[count];
            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(arr, 0, count);
            Encoding utf = Encoding.UTF8;
            xmlStr = utf.GetString(arr).Trim();
        }
        catch
        {
            xmlStr = "";
        }
        return HttpUtility.HtmlDecode(xmlStr);
    }

    /// <summary>
    /// xml转为DataSet
    /// </summary>
    /// <param name="xmlStr"></param>
    /// <returns></returns>
    public static DataSet XmlToDataTable(string xmlStr)
    {
        if (!string.IsNullOrEmpty(xmlStr))
        {
            StringReader StrStream = null;
            XmlTextReader Xmlrdr = null;
            try
            {
                DataSet ds = new DataSet();
                //读取字符串中的信息
                StrStream = new StringReader(xmlStr);
                //获取StrStream中的数据
                Xmlrdr = new XmlTextReader(StrStream);
                //ds获取Xmlrdr中的数据               
                ds.ReadXml(Xmlrdr);
                return ds;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //释放资源
                if (Xmlrdr != null)
                {
                    Xmlrdr.Close();
                    StrStream.Close();
                    StrStream.Dispose();
                }
            }
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// 远程参数传递(保存session)
    /// </summary>
    /// <param name="_url">路径</param>
    /// <param name="_requestString">参数</param>
    /// <param name="userSession">当前用户Session</param>
    /// <param name="JSESSIONID">返回cookie后，赋值当前用户cookie作为每次传递的session</param>
    /// <returns>字符或XML</returns>
    public static string GetRemote(string _url, string _requestString, string userSession, out string JSESSIONID)
    {
        string result = "";
        try
        {
            CookieContainer container = new CookieContainer();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(_requestString);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";
            if (userSession != "")
                container.Add(new Uri(_url), new Cookie("JSESSIONID", userSession));

            request.ContentLength = data.Length;
            request.KeepAlive = true;
            request.CookieContainer = container;

            Stream newStream = request.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            response.Cookies = container.GetCookies(request.RequestUri);
            JSESSIONID = response.Cookies["JSESSIONID"].ToString().Split('=')[1].Trim();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            result = reader.ReadToEnd();
            response.Close();
        }
        catch
        {
            JSESSIONID = "";
            result = "false";
        }
        return result;
    }

    /// <summary>
    /// 远程参数传递
    /// </summary>
    /// <param name="domain">网址(例：http://)</param>
    /// <param name="postData">参数(例：action=GetShopInfoByIds&id=1)</param>
    /// <returns>字符串，如果多个回传，建议用JSON回传</returns>
    public static string GetRemote(string domain, string postData)
    {
        WebClient myclien = new WebClient();
        myclien.Encoding = System.Text.Encoding.UTF8;
        byte[] myData;
        myclien.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

        string strContent = string.Empty;
        try
        {
            myData = myclien.UploadData(domain, "POST", Encoding.Default.GetBytes(postData));
            strContent = System.Text.Encoding.UTF8.GetString(myData).ToString().Trim();
        }
        catch
        {
            strContent = "false";
        }
        return strContent;
    }

    /// <summary>  
    /// GZip压缩  
    /// </summary>  
    /// <param name="rawData"></param>  
    /// <returns></returns>  
    public static string CompressGZip(string rawString)
    {
        if (string.IsNullOrEmpty(rawString) || rawString.Length == 0)
        {
            return "";
        }
        else
        {
            byte[] rawData = System.Text.Encoding.UTF8.GetBytes(rawString.ToString());
            MemoryStream ms = new MemoryStream();
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            byte[] zippedData = ms.ToArray();
            return (string)(Convert.ToBase64String(zippedData));
        }
    }
    /// <summary>
    /// Gzip解压
    /// </summary>
    /// <param name="rawString"></param>
    /// <returns></returns>
    public static string DeCompressGZip(string rawString)
    {
        string decode = "";
        byte[] bytes = Convert.FromBase64String(rawString);  //将2进制编码转换为8位无符号整数数组.
        try
        {
            MemoryStream ms = new MemoryStream(bytes);
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress);
            MemoryStream outBuffer = new MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();

            decode = (string)(System.Text.Encoding.UTF8.GetString(outBuffer.ToArray()));
        }
        catch
        {
            decode = rawString;
        }
        return decode;
    }

    public static bool SetCookie(string strName, string strValue, int strDay)
    {
        try
        {
            HttpCookie cookie = new HttpCookie(strName);
            cookie.Expires = DateTime.Now.AddDays(strDay);
            if (HttpContext.Current.Request.Cookies[strName] != null)
            {
                cookie.Value = HttpUtility.UrlEncode(strValue);
                System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
            }
            else
            {
                cookie.Value = HttpUtility.UrlEncode(strValue);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static string GetCookieValue(string key)
    {
        return HttpContext.Current.Request.Cookies[key] == null ? String.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[key].Value);
    }

    public static void SetMoneyMsgCache(string key, double balance, double freeze, double takeCashQuota, double score)
    {
        Shove._Web.Cache.ClearCache("MoneyMsg_" + key);
        MoneyMessage mm = new MoneyMessage();
        mm.AbleBalance = balance;
        mm.FreezeBalance = freeze;
        mm.AbleScore = score;
        mm.TakeCashQuota = takeCashQuota;
        Shove._Web.Cache.SetCache("MoneyMsg_" + key, mm);
    }

    public static MoneyMessage GetMoneyMsgCache(string key, long siteId)
    {
        if ((MoneyMessage)Shove._Web.Cache.GetCache("MoneyMsg_" + key) == null)
        {
            Users tu = new Users(siteId)[siteId, key];
            message messageLogin = new message();
            messageLogin.body = "<user><userName>" + tu.Name + "</userName><userpwd>" + tu.Password + "</userpwd></user>";
            string schemeMessageXML = PF.ObjectToXML(messageLogin, typeof(message));

            //同步到YC-DLSA
            LoginSessionId loginSessionId = new LoginSessionId();
            string sessionId = "";
            string messageresult = PF.GetRemote(Shove._Web.WebConfig.GetAppSettingsString("ycurl"), "type=1010&message=" + HttpUtility.UrlEncode(PF.CompressGZip(schemeMessageXML)), loginSessionId.SessionId, out sessionId);
            if (messageresult == "false")
            {
                return new MoneyMessage();
            }
            else
            {
                string messageResultStr = messageresult != "false" ? PF.DeCompressGZip(messageresult) : "";
                DataSet ResultDs = PF.XmlToDataTable(messageResultStr);
                if (ResultDs != null && ResultDs.Tables["memberDetail"] != null && ResultDs.Tables["memberDetail"].Rows.Count > 0)
                {
                    //可用金额
                    double ableBalance = Convert.ToDouble(ResultDs.Tables["memberDetail"].Rows[0]["ableBalance"]);
                    //冻结
                    double freezeBalance = Convert.ToDouble(ResultDs.Tables["memberDetail"].Rows[0]["freezeBalance"]);
                    //可提款
                    double takeCashQuota = Convert.ToDouble(ResultDs.Tables["memberDetail"].Rows[0]["takeCashQuota"]);
                    //可用积分
                    double ableScore = Convert.ToDouble(ResultDs.Tables["memberDetail"].Rows[0]["ableScore"]);
                    PF.SetMoneyMsgCache(key, ableBalance, freezeBalance, takeCashQuota, ableScore);
                    MoneyMessage mm = new MoneyMessage();
                    mm.AbleBalance = ableBalance;
                    mm.AbleScore = ableScore;
                    mm.FreezeBalance = freezeBalance;
                    mm.TakeCashQuota = takeCashQuota;
                    return mm;
                }
                else
                {
                    return new MoneyMessage();
                }
            }
        }
        else
        {
            return (MoneyMessage)Shove._Web.Cache.GetCache("MoneyMsg_" + key);
        }
    }

    #endregion

    #region GoError()
    public static void GoError(int ErrorNumber, string Tip, string ClassName)
    {
        GoError("~/Error.aspx", ErrorNumber, Tip, ClassName);
    }

    public static void GoError(string ErrorPageUrl, int ErrorNumber, string Tip, string ClassName)
    {
        System.Web.HttpContext.Current.Response.Redirect(ErrorPageUrl + "?ErrorNumber=" + ErrorNumber.ToString() + "&Tip=" + System.Web.HttpUtility.UrlEncode(Tip) + "&ClassName=" + System.Web.HttpUtility.UrlEncode(Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), ClassName)), true);
    }

    #endregion

    #region GoLogin()

    public static void GoLogin()
    {
        GoLogin("UserLogin.aspx", "", true);
    }

    public static void GoLogin(bool isAtFramePageLogin)
    {
        GoLogin("UserLogin.aspx", "", isAtFramePageLogin);
    }

    public static void GoLogin(string RequestLoginPage)
    {
        GoLogin("UserLogin.aspx", RequestLoginPage, true);
    }

    public static void GoLogin(string RequestLoginPage, bool isAtFramePageLogin)
    {
        GoLogin("UserLogin.aspx", RequestLoginPage, isAtFramePageLogin);
    }

    public static void GoLogin(string LoginPageFileName, string RequestLoginPage)
    {
        GoLogin(LoginPageFileName, RequestLoginPage, true);
    }

    public static void GoLogin(string LoginPageFileName, string RequestLoginPage, bool isAtFramePageLogin)
    {
        if (RequestLoginPage.Contains("/Home/Alipay/"))
        {
            LoginPageFileName = Shove._Web.Utility.GetUrl() + "/Home/Alipay/Login.aspx";
        }
        else if ((RequestLoginPage.Contains("/Web/OnlinePay/")) || (RequestLoginPage.Contains("/Web/") && !RequestLoginPage.Contains("/Home/Web/")))
        {
            LoginPageFileName = Shove._Web.Utility.GetUrl() + "/Web/" + LoginPageFileName;
        }
        else
        {
            LoginPageFileName = Shove._Web.Utility.GetUrl() + "/" + LoginPageFileName;
        }

        if (isAtFramePageLogin)
        {
            HttpContext.Current.Response.Redirect(LoginPageFileName + "?RequestLoginPage=" + System.Web.HttpUtility.UrlEncode(RequestLoginPage), true);
        }
        else
        {

            HttpContext.Current.Response.Write("<script language=\"javascript\">window.top.location.href=\"" + LoginPageFileName + "?RequestLoginPage=" + System.Web.HttpUtility.UrlEncode(RequestLoginPage) + "\";</script>");
        }
    }

    #endregion

    #region 版本号相关

    public static string GetEdition()
    {
        return "5.0.001";
    }

    public static string GetDatabaseEdition()
    {
        return new SystemOptions()["SystemDatabaseVersion"].ToString("");
    }

    public static bool ValidEdition()
    {
        return (GetEdition() == GetDatabaseEdition());
    }

    #endregion

    // <summary>
    /// 标题加亮显示
    /// </summary>
    /// <param name="Str">要加亮的字符</param>
    /// <returns></returns>
    public static string StringAddsBrightly(string Str)
    {
        if (Str.IndexOf("[") >= 0)
        {
            Str = Str.Replace("[", "<font color='red'>[").Replace("]", "]</font>");

            return Str;
        }
        else
        {
            return Str;
        }
    }

    // 转换时间格式
    public static string ConvertDateTimeMMDDHHMM(string strDateTime)
    {
        DateTime dt;

        try
        {
            dt = DateTime.Parse(strDateTime);
        }
        catch
        {
            return "";
        }

        return dt.Month.ToString() + "-" + dt.Day.ToString() + " " + dt.Hour.ToString().PadLeft(2, '0') + ":" + dt.Minute.ToString().PadLeft(2, '0');

    }

    // 加密密码
    public static string EncryptPassword(string input)
    {
        bool IsMD5 = Shove._Web.WebConfig.GetAppSettingsBool("IsMD5", false);

        if (IsMD5)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5");
        }
        else
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(input + CenterMD5Key, "MD5");
        }
    }

    // 获取总站类实例
    public static Sites GetMasterSite()
    {
        return new Sites()[DAL.Functions.F_GetMasterSiteID()];
    }

    // 检验彩种是否有效
    public static bool ValidLotteryID(Sites Site, int LotteryID)
    {
        return (("," + Site.UseLotteryList + ",").IndexOf("," + LotteryID.ToString() + ",") >= 0);
    }

    // 校验彩票时间输入是否有效
    public static object ValidLotteryTime(string Time)
    {
        Time = Time.Trim();
        Regex regex = new Regex(@"[\d]{4}[-][\d]{1,2}[-][\d]{1,2}[\s][\d]{1,2}[:][\d]{1,2}[:][\d]{1,2}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        if (!regex.IsMatch(Time))
        {
            return null;
        }

        System.DateTime dt;

        try
        {
            dt = System.DateTime.Parse(Time);
        }
        catch
        {
            return null;
        }

        return dt;
    }

    // 拼合期号的附加 XML 字串(1球队)
    public static string BuildIsuseAdditionasXmlFor1Team(int Count, params string[] str)
    {
        string Result = "<Teams>";

        for (int i = 0; i < Count; i++)
        {
            Result += "<Team No=\"" + (i + 1).ToString() + "\" Team=\"" + str[i * 2] + "\" Time=\"" + str[i * 2 + 1] + "\"/>";
        }

        Result += "</Teams>";

        return Result;
    }

    // 拼合期号的附加 XML 字串(2球队)
    public static string BuildIsuseAdditionasXmlFor2Team(int Count, params string[] str)
    {
        string Result = "<Teams>";

        for (int i = 0; i < Count; i++)
        {
            Result += "<Team No=\"" + (i + 1).ToString() + "\" HostTeam=\"" + str[i * 3] + "\" QuestTeam=\"" + str[i * 3 + 1] + "\" Time=\"" + str[i * 3 + 2] + "\"/>";
        }

        Result += "</Teams>";

        return Result;
    }

    // 拼合期号的附加 XML 字串(2球队)
    public static string BuildIsuseAdditionasXmlFor2TeamNoLeague(int Count, params string[] str)
    {
        string Result = "<Teams>";

        for (int i = 0; i < Count; i++)
        {
            Result += "<Team No=\"" + (i + 1).ToString() + "\" HostTeam=\"" + str[i * 3] + "\" QuestTeam=\"" + str[i * 3 + 1] + "\" Time=\"" + str[i * 3 + 2] + "\"/>";
        }

        Result += "</Teams>";

        return Result;
    }

    // 拼合期号的附加 XML 字串(足彩单场)
    public static string BuildIsuseAdditionasXmlForZCDC(params string[] str)
    {
        string Result = "<Teams>";

        for (int i = 0; i < (str.Length / 5); i++)
        {
            Result += "<Team LeagueTypeID=\"" + str[i * 5] + "\" No=\"" + (i + 1).ToString() + "\" HostTeam=\"" + str[i * 5 + 1] + "\" QuestTeam=\"" + str[i * 5 + 2] + "\" LetBall=\"" + str[i * 5 + 3] + "\" Time=\"" + str[i * 5 + 4] + "\"/>";
        }

        Result += "</Teams>";

        return Result;
    }

    // 拼合期号的附加 XML 字串(高频玩法)
    public static string BuildIsuseAdditionasXmlForBJKL8(params string[] str)
    {
        string Result = "<ChaseDetail>";

        for (int i = 0; i < str.Length / 9; i++)
        {
            Result += "<Isuse IsuseID=\"" + str[i * 9] + "\" PlayTypeID = \"" + str[i * 9 + 1] + "\" LotteryNumber = \"" + str[i * 9 + 2] + "\" Multiple = \"" + str[i * 9 + 3] + "\" Money = \"" + str[i * 9 + 4] + "\" SecrecyLevel =\"" + str[i * 9 + 5] + "\" Share=\"" + str[i * 9 + 6] + "\" BuyedShare=\"" + str[i * 9 + 7] + "\" AssureShare=\"" + str[i * 9 + 8] + "\"/>";
        }

        return Result += "</ChaseDetail>";
    }

    // 拼合期号的附加 XML 字串(追号)
    public static string BuildIsuseAdditionasXmlForChase(params string[] str)
    {
        string Result = "<ChaseDetail>";

        for (int i = 0; i < str.Length / 6; i++)
        {
            Result += "<Isuse IsuseID=\"" + str[i * 6] + "\" PlayTypeID = \"" + str[i * 6 + 1] + "\" LotteryNumber = \"" + str[i * 6 + 2] + "\" Multiple = \"" + str[i * 6 + 3] + "\" Money = \"" + str[i * 6 + 4] + "\" SecrecyLevel =\"" + str[i * 6 + 5] + "\" Share=\"1\" BuyedShare=\"1\" AssureShare=\"0\"/>";
        }

        return Result += "</ChaseDetail>";
    }

    public static string GetPassWayStr(string val)
    {
        int len = val.Split(';').Length;
        string[] PassWauy = null;
        if (len < 3)
        {
            return "";
        }

        try
        {
            PassWauy = val.Split(';')[2].ToString().Replace("]", "").Replace("[", "").Split(',');
        }
        catch (System.Exception ex)
        {
            return "";
        }

        string Group = "单关,2x1,3x1,3x3,3x4,4x1,4x4,4x5,4x6,4x11,5x1,5x5,5x6,5x10,5x16,5x20,5x26,6x1,6x6,6x7,6x15,6x20,6x22,6x35,6x42,6x50,6x57,7x1,7x7,7x8,7x21,7x35,7x120,8x1,8x8,8x9,8x28,8x56,8x70,8x247";

        string letter = "A0,AA,AB,AC,AD,AE,AF,AG,AH,AI,AJ,AK,AL,AM,AN,AO,AP,AQ,AR,AS,AT,AU,AV,AW,AX,AY,AZ,BA,BB,BC,BD,BE,BF,BG,BH,BI,BJ,BK,BL,BM";

        string[] Groups = Group.Split(',');
        string[] letters = letter.Split(',');

        if (Groups.Length != letters.Length)
        {
            return "";
        }

        string StrPassWauy = "";
        for (int j = 0; j < PassWauy.Length; j++)
        {
            val = PassWauy[j].Substring(0, 2);
            for (int i = 0; i < letters.Length; i++)
            {
                if (val == letters[i].ToString())
                {
                    StrPassWauy += Groups[i].ToString() + ",";

                    break;
                }
            }
        }
        if (StrPassWauy != "")
        {
            StrPassWauy = StrPassWauy.Substring(0, StrPassWauy.Length - 1);
        }

        return StrPassWauy;
    }

    public static string GetPassWayBJDC(string val)
    {
        int len = val.Split(';').Length;
        string[] PassWauy = null;
        if (len < 3)
        {
            return "";
        }

        try
        {
            PassWauy = val.Split(';')[2].ToString().Replace("]", "").Replace("[", "").Split(',');
        }
        catch (System.Exception ex)
        {
            return "";
        }

        string Group = "单关,2x1,2x3,3x1,3x4,3x7,4x1,4x5,4x11,4x15,5x1,5x6,5x16,5x26,5x31,6x1,6x7,6x22,6x42,6x57,6x63,7x1,8x1,9x1,10x1,11x1,12x1,13x1,14x1,15x1";

        string letter = "A0,AA,AB,AC,AD,AE,AF,AG,AH,AI,AJ,AK,AL,AM,AN,AO,AP,AQ,AR,AS,AT,AU,AV,AW,AX,AY,AZ,BA,BB,BC";

        string[] Groups = Group.Split(',');
        string[] letters = letter.Split(',');

        if (Groups.Length != letters.Length)
        {
            return "";
        }

        string StrPassWauy = "";
        for (int j = 0; j < PassWauy.Length; j++)
        {
            val = PassWauy[j].Substring(0, 2);
            for (int i = 0; i < letters.Length; i++)
            {
                if (val == letters[i].ToString())
                {
                    StrPassWauy += Groups[i].ToString() + ",";

                    break;
                }
            }
        }
        if (StrPassWauy != "")
        {
            StrPassWauy = StrPassWauy.Substring(0, StrPassWauy.Length - 1);
        }

        return StrPassWauy;
    }

    public static string GetPassWay(string val)
    {
        int len = val.Split(';').Length;

        if (len < 3)
        {
            return "";
        }

        try
        {
            val = val.Split(';')[2].ToString().Replace("]", "").Replace("[", "").Substring(0, 2);
        }
        catch (System.Exception ex)
        {
            return "";
        }

        string Group = "单关,2x1,3x1,3x3,3x4,4x1,4x4,4x5,4x6,4x11,5x1,5x5,5x6,5x10,5x16,5x20,5x26,6x1,6x6,6x7,6x15,6x20,6x22,6x35,6x42,6x50,6x57,7x1,7x7,7x8,7x21,7x35,7x120,8x1,8x8,8x9,8x28,8x56,8x70,8x247";

        string letter = "A0,AA,AB,AC,AD,AE,AF,AG,AH,AI,AJ,AK,AL,AM,AN,AO,AP,AQ,AR,AS,AT,AU,AV,AW,AX,AY,AZ,BA,BB,BC,BD,BE,BF,BG,BH,BI,BJ,BK,BL,BM";

        string[] Groups = Group.Split(',');
        string[] letters = letter.Split(',');

        if (Groups.Length != letters.Length)
        {
            return "";
        }

        int wayIndex = -1;

        for (int i = 0; i < letters.Length; i++)
        {
            if (val == letters[i].ToString())
            {
                wayIndex = i;

                break;
            }
        }

        if (wayIndex < 0)
        {
            return "";
        }

        return Groups[wayIndex].ToString();
    }


    public static string GetPassWayTicketMode(string val)
    {
        string Group = "单关,2x1,3x1,3x3,3x4,4x1,4x4,4x5,4x6,4x11,5x1,5x5,5x6,5x10,5x16,5x20,5x26,6x1,6x6,6x7,6x15,6x20,6x22,6x35,6x42,6x50,6x57,7x1,7x7,7x8,7x21,7x35,7x120,8x1,8x8,8x9,8x28,8x56,8x70,8x247";

        string letter = "A0,AA,AB,AC,AD,AE,AF,AG,AH,AI,AJ,AK,AL,AM,AN,AO,AP,AQ,AR,AS,AT,AU,AV,AW,AX,AY,AZ,BA,BB,BC,BD,BE,BF,BG,BH,BI,BJ,BK,BL,BM";

        string[] Groups = Group.Split(',');
        string[] letters = letter.Split(',');

        if (Groups.Length != letters.Length)
        {
            return "";
        }

        int wayIndex = -1;

        for (int i = 0; i < letters.Length; i++)
        {
            if (val == letters[i].ToString())
            {
                wayIndex = i;

                break;
            }
        }

        if (wayIndex < 0)
        {
            return "";
        }

        return Groups[wayIndex].ToString();
    }

    public static string GetPassWayChineseMode(string val)
    {
        string Group = "单关,2串1,3串1,3串3,3串4,4串1,4串4,4串5,4串6,4串11,5串1,5串5,5串6,5串10,5串16,5串20,5串26,6串1,6串6,6串7,6串15,6串20,6串22,6串35,6串42,6串50,6串57,7串1,7串7,7串8,7串21,7串35,7串120,8串1,8串8,8串9,8串28,8串56,8串70,8串247";

        string letter = "A0,AA,AB,AC,AD,AE,AF,AG,AH,AI,AJ,AK,AL,AM,AN,AO,AP,AQ,AR,AS,AT,AU,AV,AW,AX,AY,AZ,BA,BB,BC,BD,BE,BF,BG,BH,BI,BJ,BK,BL,BM";

        string[] Groups = Group.Split(',');
        string[] letters = letter.Split(',');

        if (Groups.Length != letters.Length)
        {
            return "";
        }

        int wayIndex = -1;

        for (int i = 0; i < letters.Length; i++)
        {
            if (val == letters[i].ToString())
            {
                wayIndex = i;

                break;
            }
        }

        if (wayIndex < 0)
        {
            return "";
        }

        return Groups[wayIndex].ToString();
    }

    public static void GetStrScope(string str, string strStart, string strEnd, out int IStart, out int ILen)
    {
        IStart = str.IndexOf(strStart);
        if (IStart != -1)
            ILen = str.LastIndexOf(strEnd) - IStart;
        else
        {
            IStart = 0;
            ILen = 0;
        }
    }

    public static string GetScriptResTable(string val)
    {
        try
        {
            string way = GetPassWay(val);               //得到过关方式

            StringBuilder sb = new StringBuilder();

            val = val.Trim();

            int Istart, Ilen;

            GetStrScope(val, "[", "]", out Istart, out Ilen);

            val = val.Substring(Istart + 1, Ilen - 1);

            string type = val.Split(';')[0];

            if (type.Substring(0, 2) != "72" && type.Substring(0, 2) != "73")
            {
                return val;
            }

            int PlayTypeID = Shove._Convert.StrToInt(val.Split(';')[0], 7201);

            if (type.Substring(0, 2) == "72")
            {
                // Loong Add 2012-04-24 Description: 增加 比分、终赔
                sb.Append("<div class=\"tdbback\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"tablelay\">");
                sb.Append("<th scope='col' width='60'>赛事编号</th>");
                sb.Append("<th scope='col' width='80'>联赛</th><th scope='col' width='120'>主队 VS 客队</th>");
                sb.Append("<th scope='col' width='110'>预计停售时间</th><th scope='col' width='50'>设胆</th><th scope='col'>投注内容</th><th scope='col'>比分</th><th scope='col'>赛果</th><th scope='col'>参考赔率</th>");
            }
            else
            {
                // Loong Add 2012-04-24 Description: 增加 比分、终赔
                sb.Append("<div class=\"tdbback\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"tablelay\">");
                sb.Append("<th scope='col' width='60'>赛事编号</th>");
                sb.Append("<th scope='col' width='80'>联赛</th><th scope='col' width='120'>客队 VS 主队</th>");
                sb.Append("<th scope='col' width='110'>预计停售时间</th><th scope='col' width='50'>设胆</th><th scope='col'>投注内容</th><th scope='col'>比分</th><th scope='col'>赛果</th><th scope='col'>参考赔率</th>");
            }

            string Matchids = "";
            string[] ArrRes = null;
            string MatchListDan = "";
            string MatchidsDan = "";

            DataTable dtMatch = new DataTable();

            dtMatch.Columns.Add("MatchID", typeof(System.String));
            dtMatch.Columns.Add("PlayType", typeof(System.String));

            if (val.Split(';').Length == 4)
            {
                MatchListDan = val.Split(']')[0];
                val = val.Split('[')[1];

                foreach (string match in MatchListDan.Split('|'))
                {
                    DataRow dr = dtMatch.NewRow();

                    dr["MatchID"] = match.Split('(')[0];
                    dr["PlayType"] = match.Split('(')[1].Substring(0, match.Split('(')[1].LastIndexOf(')'));

                    MatchidsDan += match.Split('(')[0] + ",";
                    Matchids += match.Split('(')[0] + ",";

                    dtMatch.Rows.Add(dr);
                    dtMatch.AcceptChanges();
                }

                if (MatchidsDan.EndsWith(","))
                {
                    MatchidsDan = MatchidsDan.Substring(0, MatchidsDan.Length - 1);
                }

                if (string.IsNullOrEmpty(MatchidsDan))
                {
                    return val;
                }
            }

            foreach (string match in val.Split('|'))
            {
                DataRow dr = dtMatch.NewRow();

                dr["MatchID"] = match.Split('(')[0];
                dr["PlayType"] = match.Split('(')[1].Substring(0, match.Split('(')[1].LastIndexOf(')'));

                Matchids += match.Split('(')[0] + ",";

                dtMatch.Rows.Add(dr);
                dtMatch.AcceptChanges();
            }

            if (Matchids.EndsWith(","))
            {
                Matchids = Matchids.Substring(0, Matchids.Length - 1);
            }

            if (string.IsNullOrEmpty(Matchids))
            {
                return val;
            }

            DataTable table = null;

            if (type.Substring(0, 2) == "72")
            {
                // Loong Add 2012-04-24 Descript: Add Column Result as Goal
                string SQL = "Result as Goal,game,mainteam +'VS'+guestteam teamname,MatchNumber,DATEADD(minute, (select SystemEndAheadMinute from T_PlayTypes where id = " + PlayTypeID + ") * -1, StopSellingTime) date, id";

                switch (PlayTypeID)
                {
                    // Loong Add 2012-04-24 Column Scale
                    case 7201:
                        {
                            SQL += ", SPFResult as Result,SPFBonus as Scale ";
                        }
                        break;
                    case 7202:
                        {
                            SQL += ", ZQBFResult  as Result,ZQBFBonus as Scale ";
                        }
                        break;
                    case 7203:
                        {
                            SQL += ",ZJQSResult  as Result,ZJQSBonus as Scale ";
                        }
                        break;
                    case 7204:
                        {
                            SQL += ", BQCResult as Result,BQCBonus as Scale ";
                        }
                        break;
                }

                table = new DAL.Tables.T_Match().Open(SQL, "id in (" + Matchids + ")", "");
            }
            else
            {
                // Loong Add 2012-04-24 Descript: Add Column Result as Goal
                string SQL = "Result as Goal,game,guestteam +'VS'+mainteam teamname,MatchNumber,DATEADD(minute, (select SystemEndAheadMinute from T_PlayTypes where id = " + PlayTypeID + ") * -1, StopSellingTime) date, id";

                switch (PlayTypeID)
                {
                    case 7301:
                        {
                            SQL += ", SFResult as Result,SFBonus as Scale ";
                        }
                        break;
                    case 7302:
                        {
                            SQL += ", RFSFResult  as Result,RFSFBonus as Scale ";
                        }
                        break;
                    case 7303:
                        {
                            SQL += ",SFCResult  as Result,SFCBonus as Scale ";
                        }
                        break;
                    case 7304:
                        {
                            SQL += ", DXResult as Result,DXBonus as Scale ";
                        }
                        break;
                }

                table = new DAL.Tables.T_MatchBasket().Open(SQL, "id in (" + Matchids + ")", "");
            }

            if (table == null)
            {
                return val;
            }

            if (table.Rows.Count < 1)
            {
                return val;
            }

            string res = "";
            string Result = "";

            foreach (DataRow dr in table.Rows)
            {
                sb.Append("<tr class=\"trbg2\" bgcolor=\"#ffffff\"><td>" + dr["MatchNumber"].ToString() + "</td><td>" + dr["game"].ToString() + "</td><td>" + dr["teamname"].ToString() + "</td><td>" + Shove._Convert.StrToDateTime(dr["date"].ToString(), DateTime.Now.ToString()).ToString("yy-MM-dd HH:mm") + "</td>");
                sb.Append("<td>");

                if (MatchidsDan.IndexOf(dr["id"].ToString()) >= 0)
                {
                    sb.Append("是");
                }

                sb.Append("</td>");

                res = dtMatch.Select("MatchID='" + dr["id"].ToString() + "'")[0]["PlayType"].ToString();

                ArrRes = res.Split(',');
                sb.Append("<td>");

                foreach (string r in ArrRes)
                {
                    Result = Getesult(type, r);
                    if (Result.Equals(dr["Result"].ToString()))
                    {
                        Result = "<span class=\"red eng\">" + Result + "</span>";
                    }

                    sb.Append(Result + " ");
                }

                // Loong UPdate 2012-04-24 Description: 增加 比分、终赔
                sb.Append("</td><td align='center'>" + dr["Goal"].ToString());
                sb.Append("</td><td><span class=\"red eng\">" + dr["Result"].ToString() + "</span>");
                sb.Append("</td><td><b>" + dr["Scale"].ToString());
                sb.Append("</b></td></tr>");
            }

            sb.Append("</table></div>");
            sb.Append("<div style=\"text-align:center;width:660px;\">过关方式：" + way + "</div>");

            return sb.ToString();
        }
        catch (System.Exception ex)
        {
            new Log("TWZT").Write(ex.Message);

            return val;
        }
    }

    public static string GetScriptResTable1(string SchemeID, string type)
    {
        try
        {
            DataTable table = null;
            int[] PlayType;

            #region 查询该方案
            DataTable dt_Schemes = new DAL.Tables.T_Schemes().Open("PreBetType,IsPreBet", "ID=" + SchemeID, "");

            if (dt_Schemes == null)
            {
                //查询失败
                return "";
            }
            bool isPreBet = false;//是否是优化方案

            if (dt_Schemes != null && dt_Schemes.Rows.Count != 0)
            {
                if (dt_Schemes.Rows[0][1] == DBNull.Value || dt_Schemes.Rows[0][1].ToString().Length == 0)
                    isPreBet = false;
                else
                    isPreBet = Shove._Convert.StrToBool(dt_Schemes.Rows[0][1].ToString(), false);
            }
            #endregion

            #region 优化前的方案
            if (type == "7201" || type == "7202" || type == "7203" || type == "7204" || type == "7206" || type == "7207")
            {
                table = Shove.Database.MSSQL.Select("select a.ID,a.PlayType,c.Name,B.IsAbnormal,A.MatchNumber,A.LetBile,A.MainTeam,A.Game,A.GuestTeam,A.StopSellingTime,A.Score,A.PassType,B.SPFResult,B.SPFBonus,B.RQSPFResult,B.RQSPFBonus,b.BQCResult,B.BQCBonus,B.ZJQSResult,B.ZJQSBonus,B.ZQBFResult,B.ZQBFBonus,B.Result,b.MainLoseBall from T_SchemesContent as A inner join T_Match as B  on a.MatchID=b.ID,T_PlayTypes as c where A.PlayType = c.ID and a.SchemeID=" + SchemeID + " order by A.MatchNumber", new Shove.Database.MSSQL.Parameter[0]);
                PlayType = new int[5] { 1, 2, 3, 4, 5 };
            }
            else
            {
                table = Shove.Database.MSSQL.Select("select a.ID,SFBonus,RFSFBonus,DXBonus,SFCBonus,a.PlayType,c.Name,A.MatchNumber,A.LetBile,A.MainTeam,A.Game,A.GuestTeam,A.StopSellingTime,A.Score,A.PassType,B.IsAbnormal,B.SFResult,B.RFSFResult,b.DXResult,B.SFCResult,B.Result,b.Givewinlosescore as MainLoseBall,d.DefaultTotalScore,d.InvestGivenScore,a.MatchID AS MatchID from T_SchemesContent as A INNER JOIN dbo.T_SchemesMixcast d ON a.SchemeID = d.SchemeId inner join T_MatchBasket as B  on a.MatchID=b.ID,T_PlayTypes as c where A.PlayType = c.ID and a.SchemeID=" + SchemeID + " order by A.MatchNumber", new Shove.Database.MSSQL.Parameter[0]);
                PlayType = new int[4] { 11, 12, 13, 14 };
            }
            if (table == null)
            {
                return "";
            }

            if (table.Rows.Count < 1)
            {
                return "";
            }

            string way = table.Rows[0]["PassType"].ToString();
            string defaultTotalScore = "";
            string investGivenScore = "";
            if (type.Contains("73"))
            {
                defaultTotalScore = table.Rows[0]["DefaultTotalScore"].ToString();
                investGivenScore = table.Rows[0]["InvestGivenScore"].ToString();
            }
            if (string.IsNullOrEmpty(defaultTotalScore))
            {
                defaultTotalScore = "";
            }
            if (string.IsNullOrEmpty(investGivenScore))
            {
                investGivenScore = "";
            }
            StringBuilder sb = new StringBuilder();
            StringBuilder sbPrebet = new StringBuilder();//优化后的方案信息
            StringBuilder SBBouns = new StringBuilder();
            string emailStyle = " style=\"padding:3px;border-top:1px solid #DDD;border-left:1px solid #DDD;background-color:#FEFAEB;line-height:24px;font-weight:normal;font:12px \"Arial\",\"微软雅黑\"\"";
            string emailR = " style=\"padding:3px;border-top:1px solid #DDD;border-left:1px solid #DDD;border-right:1px solid #DDD;background-color:#FEFAEB;line-height:24px;font-weight:normal;font:12px \"Arial\",\"微软雅黑\"\"";

            string emailTDStyle = " style=\"padding:3px;border:1px solid #DDD;line-height:24px;font:12px \"Arial\",\"微软雅黑\"\"";

            if (type.Substring(0, 2) == "72")
            {
                sb.Append("<div class=\"tdbback\" style=\"text-align:center;\">");

                string isPre = "";

                #region 足球奖金优化列头
                if (isPreBet)
                {
                    isPre = "style=\"display:none\"";
                    //优化方案折叠
                    sb.Append("<p id=\"prebet-q\"><i>查看</i> 优化前的方案</p>");
                    //优化后方案
                    sbPrebet.Append("<div class=\"tdbback\" style=\"max-height:550px;overflow:auto;overflow-x:hidden;\">")
                            .Append("<p id=\"prebet-h\"></p>")
                            .Append("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"tablelay\">")
                            .AppendFormat("<th scope='col' width=\"8%\"{0}>序号</th>", emailStyle)
                            .AppendFormat("<th scope='col'width=\"10%\"{0}>优化方式</th>", emailStyle)
                            .AppendFormat("<th scope='col'width=\"10%\"{0}>过关方式</th><th scope='col'width=\"8%\"{0}>注数</th><th scope='col'width=\"27%\"{0}>投注内容</th>", emailStyle)
                            .AppendFormat("<th scope='col'width=\"27%\"{0}>赛果</th>", emailStyle)
                            .AppendFormat("<th scope='col' width=\"10%\"{0}>中奖金额</th>", emailStyle, emailR);
                }
                #endregion

                // Loong Add 2012-04-24 Description: 增加 比分、终赔
                sb.Append("<table " + isPre + " width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"tablelay\">");
                sb.AppendFormat("<th scope='col' width=\"8%\"{0}>赛事编号</th>", emailStyle);
                sb.AppendFormat("<th scope='col' width=\"10%\"{0}>联赛</th><th scope='col'width=\"10%\"{0}>玩法</th><th scope='col'width=\"17%\"{0}>主队 VS 客队</th>", emailStyle);
                sb.AppendFormat("<th scope='col' width=\"12%\"{0}>预计停售时间</th><th scope='col' width=\"5%\"{0}>设胆</th><th scope='col' width=\"5%\"{0}>比分</th><th scope='col' width=\"11%\"{0}>投注内容</th><th scope='col' width='9%'{0}>赛果</th><th scope='col' width=\"12%\"{1}>参考赔率</th>", emailStyle, emailR);

            }
            else
            {
                string str = "";

                /****  是否为优化方案 ****/
                if (isPreBet)
                {

                    str = "style=\"display:none\"";
                    sb.Append("<div class=\"tdbback\" style=\"text-align:center;\">");

                    //优化方案折叠
                    sb.Append("<p id=\"prebet-q\"><i>查看</i> 优化前的方案</p>");
                    //优化后方案
                    sbPrebet.Append("<div class=\"tdbback\">")
                            .Append("<p id=\"prebet-h\"></p>")
                            .Append("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"tablelay\">")
                            .AppendFormat("<th scope='col' width=\"8%\"{0}>序号</th>", emailStyle)
                            .AppendFormat("<th scope='col'width=\"10%\"{0}>优化方式</th>", emailStyle)
                            .AppendFormat("<th scope='col'width=\"10%\"{0}>过关方式</th><th scope='col'width=\"8%\"{0}>注数</th><th scope='col'width=\"27%\"{0}>投注内容</th>", emailStyle)
                            .AppendFormat("<th scope='col'width=\"27%\"{0}>赛果</th>", emailStyle)
                            .AppendFormat("<th scope='col' width=\"10%\"{0}>中奖金额</th>", emailStyle, emailR);
                }

                // Loong Add 2012-04-24 Description: 增加 比分、终赔
                sb.Append("<table " + str + " width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"tablelay\">");
                sb.AppendFormat("<th scope='col' width=\"8%\"{0}>赛事编号</th>", emailStyle);
                sb.AppendFormat("<th scope='col' width=\"10%\"{0}>联赛</th><th scope='col'width=\"10%\"{0}>玩法</th><th scope='col' width=\"17%\"{0}>客队 VS 主队</th>", emailStyle);
                sb.AppendFormat("<th scope='col' width=\"12%\"{0}>预计停售时间</th><th scope='col' width=\"5%\"{0}>设胆</th><th scope='col' width=\"5%\"{0}>比分</th><th scope='col' width=\"11%\"{0}>投注内容</th><th scope='col' width='9%'{0}>赛果</th><th scope='col' width=\"12%\"{1}>参考赔率</th>", emailStyle, emailR);
            }

            //循环对阵信息
            foreach (DataRow dr in table.Rows)
            {
                //是否异常比赛
                bool isAbnormal = Shove._Convert.StrToBool(dr["IsAbnormal"].ToString(), false);

                if (type.Substring(0, 2) == "72")
                {
                    sb.AppendFormat("<tr class=\"trbg2\" bgcolor=\"#ffffff\"><td{0}>" + dr["MatchNumber"].ToString() + "</td><td{0}>"
                        + dr["Game"].ToString() + "</td><td{0}>" + dr["Name"].ToString() + "</td><td{0}>" + dr["MainTeam"].ToString()
                        + "VS" + dr["GuestTeam"].ToString() + "</td><td{0}>"
                        + Shove._Convert.StrToDateTime(dr["stopsellingTime"].ToString(), DateTime.Now.ToString()).ToString("yy-MM-dd HH:mm") + "</td>", emailTDStyle);
                }
                else
                {

                    sb.AppendFormat("<tr class=\"trbg2\" bgcolor=\"#ffffff\"><td{0}>" + dr["MatchNumber"].ToString() + "</td><td{0}>"
                        + dr["Game"].ToString() + "</td><td{0}>" + dr["Name"].ToString() + "</td><td{0}>" + dr["GuestTeam"].ToString() + "VS" +
                        dr["MainTeam"].ToString() + "</td><td{0}>" +
                        Shove._Convert.StrToDateTime(dr["stopsellingTime"].ToString(), DateTime.Now.ToString()).ToString("yy-MM-dd HH:mm") + "</td>", emailTDStyle);
                }

                sb.AppendFormat("<td{0}>", emailTDStyle);

                if (dr["LetBile"].ToString() == "1")
                {
                    sb.Append("是");
                }

                string RF = "";
                if ("7201".IndexOf(dr["PlayType"].ToString()) >= 0)
                {
                    RF = "<i>(" + dr["MainLoseBall"].ToString() + ")</i>";
                }

                sb.Append("</td>");
                sb.AppendFormat("<td align='center'{0}>" + dr["Result"].ToString() + "</td>", emailTDStyle);
                sb.AppendFormat("<td colspan=\"3\"{0}>", emailTDStyle);
                sb.Append("<table  style=\"height:100%; width:100%;clear:both;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" >");

                DataTable DT = new DAL.Tables.T_ReferenceOdds().Open("", "schemesContentID=" + dr["ID"].ToString() + " ", "");

                string Types = "";
                string spValue = string.Empty;

                for (int i = 0; i < PlayType.Length; i++)
                {
                    string Result = "";
                    switch (PlayType[i].ToString())
                    {
                        case "1":
                            Types = "让球胜平负";
                            Result = dr["RQSPFResult"].ToString();
                            spValue = dr["RQSPFBonus"].ToString();
                            RF = "<i>(" + dr["MainLoseBall"].ToString() + ")</i>";
                            break;
                        case "2":
                            Types = "总进球数";
                            Result = dr["ZJQSResult"].ToString();
                            spValue = dr["ZJQSBonus"].ToString();
                            break;
                        case "3":
                            Types = "比分";
                            Result = dr["ZQBFResult"].ToString();
                            spValue = dr["ZQBFBonus"].ToString();
                            break;
                        case "4":
                            Types = "半全场";
                            Result = dr["BQCResult"].ToString();
                            spValue = dr["BQCBonus"].ToString();
                            break;
                        case "5":
                            Types = "胜平负";
                            Result = dr["SPFResult"].ToString();
                            spValue = dr["SPFBonus"].ToString();
                            break;
                        case "11":
                            Types = "胜负";
                            Result = dr["SFResult"].ToString();
                            spValue = dr["SFBonus"].ToString();
                            break;
                        case "12":
                            Types = "让分胜负";
                            Result = dr["RFSFResult"].ToString();
                            //RF = "<i>(" + dr["MainLoseBall"].ToString() + ")</i>";
                            spValue = dr["RFSFBonus"].ToString();
                            break;
                        case "13":
                            Types = "大小分";
                            Result = dr["DXResult"].ToString();
                            spValue = dr["DXBonus"].ToString();
                            break;
                        case "14":
                            Types = "胜分差";
                            Result = dr["SFCResult"].ToString();
                            spValue = dr["SFCBonus"].ToString();
                            break;
                        default:
                            Types = "未知玩法";
                            break;
                    }
                    DataRow[] DR = DT.Select("Type=" + PlayType[i] + "");
                    string Content = "";
                    string Bonus = "";
                    spValue = Shove._Convert.StrToDouble(spValue, 1).ToString("F2");
                    for (int j = 0; j < DR.Length; j++)
                    {
                        if (DR[j]["Content"].ToString() == "7")
                        {
                            DR[j]["Content"] = "7+";
                        }
                        if (DR[j]["Content"].ToString() == Result || isAbnormal == true)
                        {
                            Result = isAbnormal ? DR[j]["Content"].ToString() : Result;
                            Content += "<span class=\"red eng\">" + DR[j]["Content"].ToString() + RF + "</span> <br/>";
                            Bonus += "<span class=\"red eng\">" + (isAbnormal ? spValue : DR[j]["Bonus"].ToString()) + "</span> <br/>";
                        }

                        else
                        {
                            Content += DR[j]["Content"].ToString() + RF + " <br/>";
                            Bonus += DR[j]["Bonus"].ToString() + " <br/>";
                        }

                    }

                    if (DR.Length > 0)
                    {
                        if (defaultTotalScore != "" && Types == "大小分")
                        {
                            if (dr["Result"].ToString() != "")
                            {
                                double totalS = Convert.ToDouble(dr["Result"].ToString().Split(':')[0]) + Convert.ToDouble(dr["Result"].ToString().Split(':')[1]);
                                double customerTotalS = 0d;
                                string[] defaultArr = defaultTotalScore.Split(';');
                                for (int defInt = 0; defInt < defaultArr.Length; defInt++)
                                {
                                    if (defaultArr[defInt].Contains(dr["MatchID"].ToString() + "|"))
                                    {
                                        Content = Content + "(" + defaultArr[defInt].Split('|')[1] + ")";
                                        customerTotalS = Convert.ToDouble(defaultArr[defInt].Split('|')[1]);
                                    }
                                }
                                if (totalS < customerTotalS)
                                {
                                    Result = "小";
                                }
                                else
                                {
                                    Result = "大";
                                }
                            }
                            if (Result.Length <= 0)
                            {
                                string[] defaultArrI = defaultTotalScore.Split(';');
                                for (int defInt = 0; defInt < defaultArrI.Length; defInt++)
                                {
                                    if (defaultArrI[defInt].Contains(dr["MatchID"].ToString() + "|"))
                                    {
                                        Content = Content + "(" + defaultArrI[defInt].Split('|')[1] + ")";
                                    }
                                }
                            }
                        }
                        if (investGivenScore != "" && Types == "让分胜负")
                        {
                            if (dr["Result"].ToString() != "")
                            {
                                double[] totalS = { Convert.ToDouble(dr["Result"].ToString().Split(':')[0]), Convert.ToDouble(dr["Result"].ToString().Split(':')[1]) };
                                double customerTotalS = 0d;
                                string[] defaultArr = investGivenScore.Split(';');
                                for (int defInt = 0; defInt < defaultArr.Length; defInt++)
                                {
                                    if (defaultArr[defInt].Contains(dr["MatchID"].ToString() + "|"))
                                    {
                                        Content = Content + "(" + defaultArr[defInt].Split('|')[1] + ")";
                                        customerTotalS = Convert.ToDouble(defaultArr[defInt].Split('|')[1]);
                                    }
                                }
                                if (totalS[0] < totalS[1] + customerTotalS)
                                {
                                    Result = "让分主胜";
                                }
                                else
                                {
                                    Result = "让分主负";
                                }
                            }
                            if (Result.Length <= 0)
                            {
                                string[] defaultArrI = investGivenScore.Split(';');
                                for (int defInt = 0; defInt < defaultArrI.Length; defInt++)
                                {
                                    if (defaultArrI[defInt].Contains(dr["MatchID"].ToString() + "|"))
                                    {
                                        Content = Content + "(" + defaultArrI[defInt].Split('|')[1] + ")";
                                    }
                                }
                            }
                        }

                        if (type == "7206" || type == "7306")
                        {
                            sb.AppendFormat("<tr><td width=\"17%\"{0}>" + Types + "</td><td width=\"30%\">" + Content
                                + "</td><td width=\"23%\"{0}><span class=\"red eng\" >" + Result + "</span></td><td{0}>" + Bonus + "</td></tr>", emailTDStyle);
                        }
                        else
                        {
                            sb.AppendFormat("<tr><td width=\"32%\"{0}>" + Content + "</td><td width=\"28%\"{0}><span class=\"red eng\">" + Result
                            + "</span></td><td width=\"35%\"{0}>" + Bonus + "</td></tr>", emailTDStyle);
                        }
                    }

                    RF = "";
                }
                sb.Append("</table>");
                sb.Append("</td></tr>");
            }

            sb.Append("</table></div>");
            #endregion

            #region 竞彩奖金优化后的方案
            if (isPreBet)
            {
                //查询优化的投注内容
                DataTable dt_bet = new DAL.Tables.T_JCPreBet().Open("*", "SchemeID=" + SchemeID, "");
                if (dt_bet != null)
                {
                    #region 拼接matchID，并查询足球赛果表和篮球赛果表
                    List<string> listMatch = new List<string>();
                    foreach (DataRow item in dt_bet.Rows)
                    {
                        string PlayTeam = item["PlayTeam"].ToString();
                        string[] str = PlayTeam.Split('|');//235(1);1.5|236(2);3.55

                        for (int k = 0; k < str.Length; k++)
                        {
                            string[] strin = str[k].Split(';')[0].Split('(');
                            if (listMatch.Contains(strin[0]) == false)
                            {
                                listMatch.Add(strin[0]);
                            }
                        }
                    }
                    string matchIds = string.Empty;

                    foreach (var item in listMatch)
                    {
                        matchIds += item + ",";
                    }
                    if (matchIds.EndsWith(","))
                    {
                        matchIds = matchIds.Substring(0, matchIds.Length - 1);
                    }
                    //足球
                    DataTable dtMatchAll = null;

                    if (type.Substring(0, 2) == "72")
                    {
                        dtMatchAll = new DAL.Tables.T_Match().Open("*", "ID in(" + matchIds + ")", "");
                    }
                    else if (type.Substring(0, 2) == "73")//蓝球
                    {
                        dtMatchAll = new DAL.Tables.T_MatchBasket().Open("*", "ID in(" + matchIds + ")", "");
                    }

                    if (dtMatchAll == null) return "";

                    #endregion
                    int index = 0;//序号
                    string preBetType = string.Empty;//优化方式
                    string ggWay = string.Empty;//过关方式
                    StringBuilder strPlayTeam = new StringBuilder();//投注内容组合
                    StringBuilder strWinResult = new StringBuilder();//赛果组合
                    string investNum = string.Empty;//注数
                    string winMoney = string.Empty;//中奖金额
                    string winColor = string.Empty;

                    foreach (DataRow item in dt_bet.Rows)
                    {
                        #region 获取优化方式
                        if (dt_Schemes != null && dt_Schemes.Rows.Count != 0)
                        {
                            if (dt_Schemes.Rows[0][0] == DBNull.Value || dt_Schemes.Rows[0][0].ToString() == "1")
                            {
                                preBetType = "平均优化";
                            }
                            else if (dt_Schemes.Rows[0][0].ToString() == "2")
                            {
                                preBetType = "博热优化";
                            }
                            else if (dt_Schemes.Rows[0][0].ToString() == "3")
                            {
                                preBetType = "博冷优化";
                            }
                        }
                        #endregion
                        winMoney = string.Empty;
                        strPlayTeam.Clear();
                        strWinResult.Clear();
                        index++;
                        investNum = item["InvestNum"].ToString();//注数
                        ggWay = item["GGWay"].ToString();//过关方式
                        string PlayTeam = item["PlayTeam"].ToString();
                        string[] str = PlayTeam.Split('|');//235(1);1.5|236(2);3.55
                        bool isWin = true;//是否中奖
                        int winCount = 0;//判断中奖的依据

                        //循环单组组合
                        for (int k = 0; k < str.Length; k++)
                        {
                            winColor = string.Empty;
                            string mainTeam = string.Empty;//主队

                            string[] strin = str[k].Split(';')[0].Split('(');

                            string matchID = strin[0];//对阵信息ID

                            string reslt = strin[1].Replace(")", "");//投注内容

                            string castReslt = string.Empty;//转换后的投注

                            string openWinResult = string.Empty;//开奖结果

                            string dxUserDefault = string.Empty;//大小分用户预设总分

                            #region 足球
                            if (type.Substring(0, 2) == "72")
                            {
                                //查询足球对阵信息
                                DataRow[] mat = dtMatchAll.Select("ID = " + matchID + "");

                                if (mat.Length == 0) continue;

                                //是否异常比赛
                                bool isAbnormal = Shove._Convert.StrToBool(mat[0]["IsAbnormal"].ToString(), false);

                                //主
                                mainTeam = mat[0]["MainTeam"].ToString();

                                //取投注内容
                                if (type == "7201")
                                {
                                    castReslt = Get7201(reslt);//投注内容
                                    openWinResult = isAbnormal ? castReslt : mat[0]["RQSPFResult"].ToString();//开奖结果
                                }
                                else if (type == "7202")
                                {
                                    castReslt = Get7202(reslt);
                                    openWinResult = isAbnormal ? castReslt : mat[0]["ZQBFResult"].ToString();//开奖结果
                                }
                                else if (type == "7203")
                                {
                                    castReslt = Get7203(reslt);
                                    openWinResult = isAbnormal ? castReslt : mat[0]["ZJQSResult"].ToString();//开奖结果
                                }
                                else if (type == "7204")
                                {
                                    castReslt = Get7204(reslt);
                                    openWinResult = isAbnormal ? castReslt : mat[0]["BQCResult"].ToString();//开奖结果
                                }
                                else if (type == "7207")
                                {
                                    castReslt = Get7207(reslt);
                                    openWinResult = isAbnormal ? castReslt : mat[0]["SPFResult"].ToString();//开奖结果
                                }
                            }
                            #endregion

                            #region 篮球
                            else if (type.Substring(0, 2) == "73")//蓝球
                            {
                                //查询篮球对阵信息
                                DataRow[] mat = dtMatchAll.Select("ID = " + matchID + "");

                                if (mat.Length == 0) continue;

                                bool isAbnormal = Shove._Convert.StrToBool(mat[0]["IsAbnormal"].ToString(), false);

                                //主
                                mainTeam = mat[0]["MainTeam"].ToString();

                                //取投注内容
                                if (type == "7301")
                                {
                                    castReslt = Get7301(reslt);
                                    openWinResult = isAbnormal ? castReslt : mat[0]["SFResult"].ToString();//开奖结果
                                }
                                else if (type == "7302")
                                {
                                    if (investGivenScore != "" && mat[0]["Result"].ToString() != "")
                                    {
                                        double[] totalS = {Convert.ToDouble(mat[0]["Result"].ToString().Split(':')[0]), Convert.ToDouble(mat[0]["Result"].ToString().Split(':')[1])};
                                        double customerTotalS = 0d;
                                        string[] defaultArr = investGivenScore.Split(';');
                                        for (int defInt = 0; defInt < defaultArr.Length; defInt++)
                                        {
                                            if (defaultArr[defInt].Contains(mat[0]["ID"].ToString() + "|"))
                                            {
                                                dxUserDefault = defaultArr[defInt].Split('|')[1];
                                                customerTotalS = Convert.ToDouble(defaultArr[defInt].Split('|')[1]);
                                            }
                                        }
                                        if (totalS[0] <totalS[1] + customerTotalS)
                                        {
                                            castReslt = "让分主胜";
                                        }
                                        else
                                        {
                                            castReslt = "让分主负";
                                        }
                                    }
                                    string[] defaultArrI = investGivenScore.Split(';');
                                    for (int defInt = 0; defInt < defaultArrI.Length; defInt++)
                                    {
                                        if (defaultArrI[defInt].Contains(mat[0]["ID"].ToString() + "|"))
                                        {
                                            dxUserDefault = defaultArrI[defInt].Split('|')[1];
                                        }
                                    }
                                    castReslt = Get7302(reslt);
                                    openWinResult = isAbnormal ? castReslt : mat[0]["RFSFResult"].ToString();//开奖结果
                                }
                                else if (type == "7303")
                                {
                                    castReslt = Get7303(reslt);
                                    openWinResult = isAbnormal ? castReslt : mat[0]["SFCResult"].ToString();//开奖结果
                                }
                                else if (type == "7304")
                                {
                                    if (defaultTotalScore != "" && mat[0]["Result"].ToString() != "")
                                    {
                                        double totalS = Convert.ToDouble(mat[0]["Result"].ToString().Split(':')[0]) + Convert.ToDouble(mat[0]["Result"].ToString().Split(':')[1]);
                                        double customerTotalS = 0d;
                                        string[] defaultArr = defaultTotalScore.Split(';');
                                        for (int defInt = 0; defInt < defaultArr.Length; defInt++)
                                        {
                                            if (defaultArr[defInt].Contains(mat[0]["ID"].ToString() + "|"))
                                            {
                                                dxUserDefault = defaultArr[defInt].Split('|')[1];
                                                customerTotalS = Convert.ToDouble(defaultArr[defInt].Split('|')[1]);
                                            }
                                        }
                                        if (totalS < customerTotalS)
                                        {
                                            castReslt = "小";
                                        }
                                        else
                                        {
                                            castReslt = "大";
                                        }
                                    }
                                    string[] defaultArrI = defaultTotalScore.Split(';');
                                    for (int defInt = 0; defInt < defaultArrI.Length; defInt++)
                                    {
                                        if (defaultArrI[defInt].Contains(mat[0]["ID"].ToString() + "|"))
                                        {
                                            dxUserDefault = defaultArrI[defInt].Split('|')[1];
                                        }
                                    }
                                    castReslt = Get7304(reslt);
                                    openWinResult = isAbnormal ? castReslt : mat[0]["DXResult"].ToString();//开奖结果
                                }
                            }
                            #endregion

                            //判断是否中奖
                            if (castReslt == openWinResult)
                            {
                                winColor = " color=\"red\"";

                                winCount++;
                            }

                            strWinResult.AppendFormat("<font{0}>" + mainTeam + "(" + (string.IsNullOrEmpty(openWinResult) ? "-" : openWinResult) + ")</font>", winColor);

                            if (type == "7304" || type == "7302")
                            {

                                strPlayTeam.AppendFormat("<font{0}>" + mainTeam + "(" + (string.IsNullOrEmpty(castReslt) ? "-" : castReslt) + ")(" + dxUserDefault + ")</font>", winColor);
                            }
                            else
                            {
                                strPlayTeam.AppendFormat("<font{0}>" + mainTeam + "(" + (string.IsNullOrEmpty(castReslt) ? "-" : castReslt) + ")</font>", winColor);
                            }
                            if (k != str.Length - 1)
                            {
                                strPlayTeam.Append(",");
                                strWinResult.Append(",");
                            }
                        }
                        isWin = winCount == str.Length;

                        if (isWin)
                        {
                            winMoney = item["CastMoney"].ToString();//中奖金额
                        }

                        sbPrebet.AppendFormat("<tr>");
                        sbPrebet.AppendFormat("<td>{0}</td>", index);//序号
                        sbPrebet.AppendFormat("<td>{0}</td>", preBetType);//优化方式
                        sbPrebet.AppendFormat("<td>{0}</td>", ggWay);//过关方式
                        sbPrebet.AppendFormat("<td>{0}</td>", investNum);//注数
                        sbPrebet.AppendFormat("<td>{0}</td>", strPlayTeam);//单注组合
                        sbPrebet.AppendFormat("<td>{0}</td>", strWinResult);//赛果
                        sbPrebet.AppendFormat("<td>{0}</td>", winMoney);//中奖金额
                        sbPrebet.Append("</tr>");
                    }
                }
                sbPrebet.Append("</table></div>");

                sb.Append(sbPrebet.ToString());
            }
            #endregion

            sb.Append("<div style=\"text-align:center;width:660px;\">过关方式：" + way + "</div>");

            return sb.ToString();
        }
        catch (System.Exception ex)
        {
            new Log("TWZT").Write(ex.Message);

            return "";
        }
    }

    /// <summary>
    /// 获取竞彩优化数据To Json
    /// </summary>
    /// <param name="SchemeID"></param>
    /// <returns></returns>
    public static string GetScriptResTable1ToJson(string SchemeID, ref string preType)
    {
        StringBuilder returnSb = new StringBuilder();
        //读取方案表内容
        DataTable schemeDt = new DAL.Tables.T_Schemes().Open("ID,PreBetType,[DateTime],PlayTypeID,Multiple,WinMoney,WinMoneyNoWithTax", "ID=" + SchemeID, "");
        if (schemeDt == null || schemeDt.Rows.Count <= 0)
        {
            return "[]";
        }
        string preBetType = string.Empty;//优化类型
        StringBuilder clearanceModeSb = new StringBuilder();//过关方式
        DataTable jcPreBetDt = new DAL.Tables.T_JCPreBet().Open("ID,GGWay,PlayTeam,InvestNum,CastMoney,IsPlayMoney,IsPrize", "SchemeID=" + SchemeID, "");
        if (jcPreBetDt == null || jcPreBetDt.Rows.Count <= 0)
        {
            return "[]";
        }
        preBetType = schemeDt.Rows[0]["PreBetType"].ToString();
        switch (preBetType)
        {
            case "1":
                preBetType = "平均优化";
                break;
            case "2":
                preBetType = "博热优化";
                break;
            case "3":
                preBetType = "博冷优化";
                break;
            case "":
                return "[]";
        }
        preType = preBetType;
        clearanceModeSb.Append(",");
        for (int i = 0; i < jcPreBetDt.Rows.Count; i++)
        {
            if (!clearanceModeSb.ToString().Contains("," + jcPreBetDt.Rows[i]["GGWay"].ToString() + ","))
            {
                clearanceModeSb.Append(jcPreBetDt.Rows[i]["GGWay"].ToString() + ",");
            }
        }
        DataTable schemesMixcastDt = new DAL.Tables.T_SchemesMixcast().Open("LotteryNumber", "SchemeId=" + SchemeID, "");
        if (schemesMixcastDt == null || schemesMixcastDt.Rows.Count <= 0)
        {
            return "[]";
        }
        string lotteryNumber = schemesMixcastDt.Rows[0]["LotteryNumber"].ToString();
        string[] lotteryNumberArr = lotteryNumber.Split(';');
        lotteryNumber = lotteryNumberArr[1];
        string[] buyContentArr = lotteryNumber.Substring(1, lotteryNumber.Length - 2).Split('|');
        StringBuilder matchIdSb = new StringBuilder();
        for (int i = 0; i < buyContentArr.Length; i++)
        {
            matchIdSb.Append(buyContentArr[i].Remove(buyContentArr[i].IndexOf('(')) + ",");
        }
        matchIdSb = matchIdSb.Remove(matchIdSb.Length - 1, 1);
        string lotteryId = schemeDt.Rows[0]["PlayTypeID"].ToString().Substring(0, 2);
        string playType = schemeDt.Rows[0]["PlayTypeID"].ToString();
        DataTable matchDt = new DataTable();
        if (lotteryId == "72")
        {
            #region 足球
            matchDt = new DAL.Tables.T_Match().Open("ID,MainTeam,SPFResult,BQCResult,ZJQSResult,ZQBFResult,FirstHalfResult,Result,RQSPFResult,IsAbnormal", "ID IN(" + matchIdSb.ToString() + ")", "");
            returnSb.Append("[");
            for (int i = 0; i < jcPreBetDt.Rows.Count; i++)
            {
                returnSb.Append("{");
                returnSb.Append("\"ggWay\":\"" + jcPreBetDt.Rows[i]["GGWay"].ToString() + "\",");
                returnSb.Append("\"investNum\":\"" + jcPreBetDt.Rows[i]["InvestNum"].ToString() + "\",");
                //解析投注内容
                StringBuilder tempBuyContentSb = new StringBuilder();
                StringBuilder tempResult = new StringBuilder();
                string tempBuyContent = jcPreBetDt.Rows[i]["PlayTeam"].ToString();
                string[] tempBuyContentArr = tempBuyContent.Split('|');
                string matchId = string.Empty;
                string buyItem = string.Empty;
                string openResult = string.Empty;
                StringBuilder markRedSb = new StringBuilder();
                for (int j = 0; j < tempBuyContentArr.Length; j++)
                {
                    matchId = tempBuyContentArr[j].Remove(tempBuyContentArr[j].IndexOf('('));
                    buyItem = tempBuyContentArr[j].Substring(tempBuyContentArr[j].IndexOf('(') + 1, tempBuyContentArr[j].IndexOf(')') - tempBuyContentArr[j].IndexOf('(') - 1);
                    //取投注内容
                    if (playType == "7201")
                    {
                        openResult = matchDt.Select("ID=" + matchId)[0]["RQSPFResult"].ToString();//开奖结果

                    }
                    else if (playType == "7202")
                    {
                        openResult = matchDt.Select("ID=" + matchId)[0]["ZQBFResult"].ToString();//开奖结果
                    }
                    else if (playType == "7203")
                    {
                        openResult = matchDt.Select("ID=" + matchId)[0]["ZJQSResult"].ToString();//开奖结果
                    }
                    else if (playType == "7204")
                    {
                        openResult = matchDt.Select("ID=" + matchId)[0]["BQCResult"].ToString();//开奖结果
                    }
                    else if (playType == "7207")
                    {
                        openResult = matchDt.Select("ID=" + matchId)[0]["SPFResult"].ToString();//开奖结果
                    }
                    if (string.IsNullOrEmpty(openResult))
                    {
                        openResult = "-";
                    }
                    tempBuyContentSb.Append(matchDt.Select("ID=" + matchId)[0]["MainTeam"].ToString());
                    tempBuyContentSb.Append("(" + Getesult(playType, buyItem) + ")");
                    tempResult.Append(openResult);
                    tempBuyContentSb.Append(",");
                    tempResult.Append(",");
                    if (matchDt.Select("ID=" + matchId)[0]["IsAbnormal"].ToString() == "1")
                    {
                        markRedSb.Append("1,");
                    }
                    else
                    {
                        if (Getesult(playType, buyItem) == openResult)
                        {
                            markRedSb.Append("1,");
                        }
                        else
                        {
                            markRedSb.Append("0,");
                        }
                    }
                }
                tempBuyContentSb = tempBuyContentSb.Remove(tempBuyContentSb.Length - 1, 1);
                tempResult = tempResult.Remove(tempResult.Length - 1, 1);
                returnSb.Append("\"buyContent\":\"" + tempBuyContentSb.ToString() + "\",");
                returnSb.Append("\"result\":\"" + tempResult.ToString() + "\",");
                string winMoney = jcPreBetDt.Rows[i]["CastMoney"].ToString();
                if (tempBuyContentSb.ToString() != tempResult.ToString())
                {
                    winMoney = "0.00";
                }
                returnSb.Append("\"winMoney\":\"" + Convert.ToDecimal(winMoney).ToString("0.00") + "\",");

                returnSb.Append("\"markRed\":\"" + markRedSb.Remove(markRedSb.Length - 1, 1).ToString() + "\"");
                returnSb.Append("},");
            }
            returnSb = returnSb.Remove(returnSb.Length - 1, 1);
            returnSb.Append("]");
            #endregion
        }
        else
        {
            #region 篮球
            matchDt = new DAL.Tables.T_MatchBasket().Open("ID,MainTeam,SFResult,RFSFResult,SFCResult,DXResult,IsAbnormal", "ID IN(" + matchIdSb.ToString() + ")", "");
            returnSb.Append("[");
            for (int i = 0; i < jcPreBetDt.Rows.Count; i++)
            {
                returnSb.Append("{");
                returnSb.Append("\"ggWay\":\"" + jcPreBetDt.Rows[i]["GGWay"].ToString() + "\",");
                returnSb.Append("\"investNum\":\"" + jcPreBetDt.Rows[i]["InvestNum"].ToString() + "\",");
                //解析投注内容
                StringBuilder tempBuyContentSb = new StringBuilder();
                StringBuilder tempResult = new StringBuilder();
                string tempBuyContent = jcPreBetDt.Rows[i]["PlayTeam"].ToString();
                string[] tempBuyContentArr = tempBuyContent.Split('|');
                string matchId = string.Empty;
                string buyItem = string.Empty;
                string openResult = string.Empty;
                StringBuilder markRedSb = new StringBuilder();
                for (int j = 0; j < tempBuyContentArr.Length; j++)
                {
                    matchId = tempBuyContentArr[j].Remove(tempBuyContentArr[j].IndexOf('('));
                    buyItem = tempBuyContentArr[j].Substring(tempBuyContentArr[j].IndexOf('(') + 1, tempBuyContentArr[j].IndexOf(')') - tempBuyContentArr[j].IndexOf('(') - 1);
                    //取投注内容
                    if (playType == "7301")
                    {
                        openResult = matchDt.Select("ID=" + matchId)[0]["SFResult"].ToString();//开奖结果

                    }
                    else if (playType == "7302")
                    {
                        openResult = matchDt.Select("ID=" + matchId)[0]["RFSFResult"].ToString();//开奖结果
                    }
                    else if (playType == "7303")
                    {
                        openResult = matchDt.Select("ID=" + matchId)[0]["SFCResult"].ToString();//开奖结果
                    }
                    else if (playType == "7304")
                    {
                        openResult = matchDt.Select("ID=" + matchId)[0]["DXResult"].ToString();//开奖结果
                    }
                    if (string.IsNullOrEmpty(openResult))
                    {
                        openResult = "-";
                    }
                    tempBuyContentSb.Append(matchDt.Select("ID=" + matchId)[0]["MainTeam"].ToString());
                    tempBuyContentSb.Append("(" + Getesult(playType, buyItem) + ")");
                    tempResult.Append(openResult);
                    tempBuyContentSb.Append(",");
                    tempResult.Append(",");
                    if (matchDt.Select("ID=" + matchId)[0]["IsAbnormal"].ToString() == "1")
                    {
                        markRedSb.Append("1,");
                    }
                    else
                    {
                        if (Getesult(playType, buyItem) == openResult)
                        {
                            markRedSb.Append("1,");
                        }
                        else
                        {
                            markRedSb.Append("0,");
                        }
                    }
                }
                tempBuyContentSb = tempBuyContentSb.Remove(tempBuyContentSb.Length - 1, 1);
                tempResult = tempResult.Remove(tempResult.Length - 1, 1);
                returnSb.Append("\"buyContent\":\"" + tempBuyContentSb.ToString() + "\",");
                returnSb.Append("\"result\":\"" + tempResult.ToString() + "\",");
                string winMoney = jcPreBetDt.Rows[i]["CastMoney"].ToString();
                if (tempBuyContentSb.ToString() != tempResult.ToString())
                {
                    winMoney = "0.00";
                }
                returnSb.Append("\"winMoney\":\"" + Convert.ToDecimal(winMoney).ToString("0.00") + "\",");

                returnSb.Append("\"markRed\":\"" + markRedSb.Remove(markRedSb.Length - 1, 1).ToString() + "\"");
                returnSb.Append("},");
            }
            returnSb = returnSb.Remove(returnSb.Length - 1, 1);
            returnSb.Append("]");
            #endregion
        }
        return returnSb.ToString();
    }

    public static string Getesult(string PlayType, string num)
    {
        string res = string.Empty;

        switch (PlayType)
        {
            case "7201":
                res = Get7201(num);
                break;
            case "7202":
                res = Get7202(num);
                break;
            case "7203":
                res = Get7203(num);
                break;
            case "7204":
                res = Get7204(num);
                break;
            case "7207":
                res = Get7207(num);
                break;
            case "7301":
                res = Get7301(num);
                break;
            case "7302":
                res = Get7302(num);
                break;
            case "7303":
                res = Get7303(num);
                break;
            case "7304":
                res = Get7304(num);
                break;
        }

        return res;
    }

    public static string GetBJDCResult(string SchemeID, string type)
    {
        try
        {
            DataTable table = Shove.Database.MSSQL.Select("select a.ID,a.PlayType,c.Name,A.MatchNumber,A.LetBile,A.MainTeam,A.Game,A.GuestTeam,A.StopSellingTime,A.Score,A.PassType,b.FullScore,b.RQSPF_Result,b.RQSPF_Result_SP,b.SXDS_Result,b.SXDS_Result_SP,b.BQC_Result,b.BQC_Result_SP,b.BF_Result,b.BF_Result_SP,b.ZJQS_Result,b.ZJQS_Result_SP,b.GiveBall from T_SchemesContent as A inner join T_BJSingle as B  on a.MatchID=b.MatchID,T_PlayTypes as c where A.PlayType = c.ID and a.SchemeID=" + SchemeID + "", new Shove.Database.MSSQL.Parameter[0]);
            int[] PlayType = new int[5] { 1, 2, 3, 4, 5 };

            if (table == null)
            {
                return "";
            }

            if (table.Rows.Count < 1)
            {
                return "";
            }

            //查询优化方式
            DataTable dt_Schemes = new DAL.Tables.T_Schemes().Open("PreBetType,IsPreBet", "ID=" + SchemeID, "");

            bool isPreBet = false;//是否是优化方案

            if (dt_Schemes != null && dt_Schemes.Rows.Count != 0)
            {
                if (dt_Schemes.Rows[0][1] == DBNull.Value || dt_Schemes.Rows[0][1].ToString().Length == 0)
                    isPreBet = false;
                else
                    isPreBet = Shove._Convert.StrToBool(dt_Schemes.Rows[0][1].ToString(), false);
            }

            string way = table.Rows[0]["PassType"].ToString();
            StringBuilder sb = new StringBuilder();
            StringBuilder sbPrebet = new StringBuilder();//优化后的方案信息
            StringBuilder SBBouns = new StringBuilder();

            string emailStyle = " style=\"padding:3px;border-top:1px solid #DDD;border-left:1px solid #DDD;background-color:#FEFAEB;line-height:24px;font-weight:normal;font:12px \"Arial\",\"微软雅黑\"\"";

            string emailR = " style=\"padding:3px;border-top:1px solid #DDD;border-left:1px solid #DDD;border-right:1px solid #DDD;background-color:#FEFAEB;line-height:24px;font-weight:normal;font:12px \"Arial\",\"微软雅黑\"\"";

            string emailTDStyle = " style=\"padding:3px;border:1px solid #DDD;line-height:24px;font:12px \"Arial\",\"微软雅黑\"\"";


            if (type.Substring(0, 2) == "45")
            {
                sb.Append("<div class=\"tdbback\" style=\"text-align:center;\">");

                string isPre = "";

                sb.Append("<table " + isPre + " width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"tablelay\">");
                sb.AppendFormat("<th scope='col' width=\"10%\"{0}>赛事编号</th>", emailStyle);
                sb.AppendFormat("<th scope='col'width=\"8%\"{0}>联赛</th><th scope='col'width=\"10%\"{0}>玩法</th><th scope='col'width=\"15%\"{0}>主队 VS 客队</th>", emailStyle);
                sb.AppendFormat("<th scope='col' width=\"13%\"{0}>预计停售时间</th><th scope='col' width=\"5%\"{0}>设胆</th><th scope='col' width=\"5%\"{0}>比分</th><th scope='col' width=\"10%\"{0}>投注内容</th><th scope='col' width='12%'{0}>赛果(赔率)</th><th scope='col' width=\"10%\"{1}>参考赔率</th>", emailStyle, emailR);

            }

            foreach (DataRow dr in table.Rows)
            {
                if (type.Substring(0, 2) == "45")
                {
                    sb.AppendFormat("<tr class=\"trbg2\" bgcolor=\"#ffffff\"><td{0}>" + dr["MatchNumber"].ToString() + "</td><td{0}>"
                        + dr["Game"].ToString() + "</td><td{0}>" + dr["Name"].ToString() + "</td><td{0}>" + dr["MainTeam"].ToString() + "VS"
                        + dr["GuestTeam"].ToString() + "</td><td{0}>"
                        + Shove._Convert.StrToDateTime(dr["stopsellingTime"].ToString(), DateTime.Now.ToString()).ToString("yy-MM-dd HH:mm") + "</td>", emailTDStyle);
                }
                else
                {

                    sb.AppendFormat("<tr class=\"trbg2\" bgcolor=\"#ffffff\"><td{0}>" + dr["MatchNumber"].ToString() + "</td><td{0}>"
                        + dr["Game"].ToString() + "</td><td{0}>" + dr["Name"].ToString() + "</td><td{0}>"
                        + dr["GuestTeam"].ToString() + "VS" + dr["MainTeam"].ToString() + "</td><td{0}>"
                        + Shove._Convert.StrToDateTime(dr["stopsellingTime"].ToString(), DateTime.Now.ToString()).ToString("yy-MM-dd HH:mm") + "</td>", emailTDStyle);
                }

                sb.AppendFormat("<td{0}>", emailTDStyle);

                if (dr["LetBile"].ToString() == "1")
                {
                    sb.Append("是");
                }

                string RF = "";
                if ("4501".IndexOf(dr["PlayType"].ToString()) >= 0)
                {
                    RF = "<i>(" + dr["GiveBall"].ToString() + ")</i>";
                }

                sb.Append("</td>");
                sb.AppendFormat("<td align='center'{0}>" + dr["FullScore"].ToString() + "</td>", emailTDStyle);
                sb.AppendFormat("<td colspan=\"3\"{0}>", emailTDStyle);
                sb.Append("<table  style=\"height:100%; width:100%;clear:both;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" >");
                DataTable DT = new DAL.Tables.T_ReferenceOdds().Open("", "schemesContentID=" + dr["ID"].ToString() + " ", "");

                string Types = "";

                for (int i = 0; i < PlayType.Length; i++)
                {
                    string Result = "";
                    double ResultSP = 0;
                    switch (PlayType[i].ToString())
                    {
                        case "1":
                            Types = "让球胜平负";
                            Result = dr["RQSPF_Result"].ToString();
                            ResultSP = Shove._Convert.StrToDouble(dr["RQSPF_Result_SP"].ToString(), 0);
                            RF = "<i>(" + dr["GiveBall"].ToString() + ")</i>";
                            break;
                        case "2":
                            Types = "总进球数";
                            Result = dr["ZJQS_Result"].ToString();
                            ResultSP = Shove._Convert.StrToDouble(dr["ZJQS_Result_SP"].ToString(), 0);
                            break;
                        case "3":
                            Types = "上下单双";
                            Result = dr["SXDS_Result"].ToString();
                            ResultSP = Shove._Convert.StrToDouble(dr["SXDS_Result_SP"].ToString(), 0);
                            break;
                        case "4":
                            Types = "猜比分";
                            Result = dr["BF_Result"].ToString();
                            ResultSP = Shove._Convert.StrToDouble(dr["BF_Result_SP"].ToString(), 0);
                            break;
                        case "5":
                            Types = "半全场";
                            Result = dr["BQC_Result"].ToString().Replace("-", "");
                            ResultSP = Shove._Convert.StrToDouble(dr["BQC_Result_SP"].ToString(), 0);
                            break;
                        default:
                            Types = "未知玩法";
                            break;
                    }
                    DataRow[] DR = DT.Select("Type=" + PlayType[i] + "");
                    string Content = "";
                    string Bonus = "";
                    for (int j = 0; j < DR.Length; j++)
                    {
                        if (DR[j]["Content"].ToString() == Result)
                        {
                            Content += "<span class=\"red eng\">" + DR[j]["Content"].ToString() + RF + "</span> <br/>";
                            Bonus += "<span class=\"red eng\">" + DR[j]["Bonus"].ToString() + "</span> <br/>";
                        }

                        else
                        {
                            Content += DR[j]["Content"].ToString() + RF + " <br/>";
                            Bonus += DR[j]["Bonus"].ToString() + " <br/>";
                        }

                    }

                    if (DR.Length > 0)
                    {
                        string strSp = ResultSP > 0 ? "(" + ResultSP + ")" : string.Empty;

                        sb.AppendFormat("<tr><td width=\"31%\"{0}>" + Content + "</td><td width=\"38%\"{0}><span class=\"red eng\">" + Result + strSp
                            + "</span></td><td{0}>" + Bonus + "</td></tr>", emailTDStyle);
                    }

                    RF = "";
                }
                sb.Append("</table>");
                sb.Append("</td></tr>");
            }

            sb.Append("</table></div>");

            sb.Append("<div style=\"text-align:center;width:660px;\">过关方式：" + way + "</div>");

            return sb.ToString();
        }
        catch (System.Exception ex)
        {
            new Log("TWZT").Write(ex.Message);

            return "";
        }
    }

    public static string BindNumber(string schemeID, int playTypeid)
    {
        StringBuilder sb = new StringBuilder();
        DataTable DT = new DAL.Views.V_SchemeMixcast().Open("", "SchemeId=" + schemeID + "", "ID");

        string emailStyle = " style=\"padding:3px;border-top:1px solid #DDD;border-left:1px solid #DDD;background-color:#FEFAEB;line-height:24px;font-weight:normal;font:12px \"Arial\",\"微软雅黑\"\"";
        string emailR = " style=\"padding:3px;border-top:1px solid #DDD;border-left:1px solid #DDD;border-right:1px solid #DDD;background-color:#FEFAEB;line-height:24px;font-weight:normal;font:12px \"Arial\",\"微软雅黑\"\"";

        string emailTDStyle = " style=\"padding:3px;border:1px solid #DDD;line-height:24px;font:12px \"Arial\",\"微软雅黑\"\"";

        string str = string.Format("<tr><th align=\"center\"{0}>玩法</th><th align=\"center\"{0}>号码</th><th align=\"center\"{1}>注数</th></tr>", emailStyle, emailR);
        if (DT == null || DT.Rows.Count <= 0)
        {
            return "未上传";
        }

        else
        {
            sb.Append("<table>");
            sb.Append(str);
            foreach (DataRow DR in DT.Rows)
            {
                string playName = DR["PlayName"].ToString();
                playTypeid = Shove._Convert.StrToInt(DR["PlayTypeID"].ToString(), 0);

                if (playTypeid == 2803 || playTypeid == 6103)
                {
                    int starLevel = Regex.Matches(DR["LotteryNumber"].ToString(), @"-").Count;
                    if (starLevel == 1)
                    {
                        playName = "四星直选";
                    }
                    else if (starLevel == 2)
                    {
                        playName = "三星直选";
                    }
                    else if (starLevel == 3)
                    {
                        playName = "二星直选";
                    }
                    else if (starLevel == 4)
                    {
                        playName = "一星直选";
                    }
                    else
                    {
                        playName = "五星直选";
                    }
                }
                sb.AppendFormat("<tr><td{0}>" + playName + "</td><td{0}>" + DR["LotteryNumber"].ToString() + "</td><td{0}>" + DR["InvestNum"].ToString() + "</td></tr>", emailTDStyle);
            }
            sb.Append("</table>");
        }
        return sb.ToString();
    }

    #region 足彩
    static string[] RQSPF = { "Win", "Flat", "Lose" };
    static string[] ZJQS = { "In0", "In1", "In2", "In3", "In4", "In5", "In6", "In7" };
    static string[] BQC = { "SS", "SP", "SF", "PS", "PP", "PF", "FS", "FP", "FF" };
    static string[] ZQBF = { "S10", "S20", "S21", "S30", "S31", "S32", "S40", "S41", "S42", "S50", "S51", "S52", "Sother", "P00", "P11", "P22", "P33", "Pother", "F01", "F02", "F12", "F03", "F13", "F23", "F04", "F14", "F24", "F05", "F15", "F25", "Fother" };
    static string[] SPF = { "SPFWin", "SPFFlat", "SPFLose" };

    public static string GetPlayName(string playId)
    {
        string result = string.Empty;
        switch (playId)
        {
            case "7201":
                result = "让球胜平负";
                break;
            case "7202":
                result = "比分";
                break;
            case "7203":
                result = "总进球";
                break;
            case "7204":
                result = "半全场";
                break;
            case "7206":
                result = "混合过关";
                break;
            case "7207":
                result = "胜平负";
                break;
            case "7301":
                result = "胜负";
                break;
            case "7302":
                result = "让分胜负";
                break;
            case "7303":
                result = "胜分差";
                break;
            case "7304":
                result = "大小分";
                break;
            case "7306":
                result = "混合过关";
                break;
            default:
                break;
        }
        return result;
    }

    /// <summary>
    /// 让球胜平负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Get7201(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "胜";
                break;
            case "2": res = "平";
                break;
            case "3": res = "负";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }
    public static string GetId7201(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "胜": res = "1";
                break;
            case "平": res = "2";
                break;
            case "负": res = "3";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 比分
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Get7202(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "1:0";
                break;
            case "2": res = "2:0";
                break;
            case "3": res = "2:1";
                break;
            case "4": res = "3:0";
                break;
            case "5": res = "3:1";
                break;
            case "6": res = "3:2";
                break;
            case "7": res = "4:0";
                break;
            case "8": res = "4:1";
                break;
            case "9": res = "4:2";
                break;
            case "10": res = "5:0";
                break;
            case "11": res = "5:1";
                break;
            case "12": res = "5:2";
                break;
            case "13": res = "胜其他";
                break;
            case "14": res = "0:0";
                break;
            case "15": res = "1:1";
                break;
            case "16": res = "2:2";
                break;
            case "17": res = "3:3";
                break;
            case "18": res = "平其他";
                break;
            case "19": res = "0:1";
                break;
            case "20": res = "0:2";
                break;
            case "21": res = "1:2";
                break;
            case "22": res = "0:3";
                break;
            case "23": res = "1:3";
                break;
            case "24": res = "2:3";
                break;
            case "25": res = "0:4";
                break;
            case "26": res = "1:4";
                break;
            case "27": res = "2:4";
                break;
            case "28": res = "0:5";
                break;
            case "29": res = "1:5";
                break;
            case "30": res = "2:5";
                break;
            case "31": res = "负其他";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }
    public static string GetId7202(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1:0": res = "1";
                break;
            case "2:0": res = "2";
                break;
            case "2:1": res = "3";
                break;
            case "3:0": res = "4";
                break;
            case "3:1": res = "5";
                break;
            case "3:2": res = "6";
                break;
            case "4:0": res = "7";
                break;
            case "4:1": res = "8";
                break;
            case "4:2": res = "9";
                break;
            case "5:0": res = "10";
                break;
            case "5:1": res = "11";
                break;
            case "5:2": res = "12";
                break;
            case "胜其他": res = "13";
                break;
            case "0:0": res = "14";
                break;
            case "1:1": res = "15";
                break;
            case "2:2": res = "16";
                break;
            case "3:3": res = "17";
                break;
            case "平其他": res = "18";
                break;
            case "0:1": res = "19";
                break;
            case "0:2": res = "20";
                break;
            case "1:2": res = "21";
                break;
            case "0:3": res = "22";
                break;
            case "1:3": res = "23";
                break;
            case "2:3": res = "24";
                break;
            case "0:4": res = "25";
                break;
            case "1:4": res = "26";
                break;
            case "2:4": res = "27";
                break;
            case "0:5": res = "28";
                break;
            case "1:5": res = "29";
                break;
            case "2:5": res = "30";
                break;
            case "负其他": res = "31";
                break;

            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 总进球
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Get7203(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "0";
                break;
            case "2": res = "1";
                break;
            case "3": res = "2";
                break;
            case "4": res = "3";
                break;
            case "5": res = "4";
                break;
            case "6": res = "5";
                break;
            case "7": res = "6";
                break;
            case "8": res = "7+";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }
    public static string GetId7203(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "0": res = "1";
                break;
            case "1": res = "2";
                break;
            case "2": res = "3";
                break;
            case "3": res = "4";
                break;
            case "4": res = "5";
                break;
            case "5": res = "6";
                break;
            case "6": res = "7";
                break;
            case "7": res = "8";
                break;
            case "7+": res = "8";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 半全场胜平负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Get7204(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "胜胜";
                break;
            case "2": res = "胜平";
                break;
            case "3": res = "胜负";
                break;
            case "4": res = "平胜";
                break;
            case "5": res = "平平";
                break;
            case "6": res = "平负";
                break;
            case "7": res = "负胜";
                break;
            case "8": res = "负平";
                break;
            case "9": res = "负负";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }
    public static string GetId7204(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "胜胜": res = "1";
                break;
            case "胜平": res = "2";
                break;
            case "胜负": res = "3";
                break;
            case "平胜": res = "4";
                break;
            case "平平": res = "5";
                break;
            case "平负": res = "6";
                break;
            case "负胜": res = "7";
                break;
            case "负平": res = "8";
                break;
            case "负负": res = "9";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }


    /// <summary>
    /// 胜平负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Get7207(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "胜";
                break;
            case "2": res = "平";
                break;
            case "3": res = "负";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }
    public static string GetId7207(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "胜": res = "1";
                break;
            case "平": res = "2";
                break;
            case "负": res = "3";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }
    #endregion 足彩

    #region 北京单场
    static string[] BJDC_RQSPF = { "RQSPF_Win_SP", "RQSPF_Flat_SP", "RQSPF_Lose_SP" };
    static string[] BJDC_ZJQS = { "ZJQS_0_SP", "ZJQS_1_SP", "ZJQS_2_SP", "ZJQS_3_SP", "ZJQS_4_SP", "ZJQS_5_SP", "ZJQS_6_SP", "ZJQS_7_SP" };
    static string[] BJDC_BQC = { "BQC_WW_SP", "BQC_WF_SP", "BQC_WL_SP", "BQC_FW_SP", "BQC_FF_SP", "BQC_FL_SP", "BQC_LW_SP", "BQC_LF_SP", "BQC_LL_SP" };
    static string[] BJDC_BF = { "BF_Win10_SP", "BF_Win20_SP", "BF_Win21_SP", "BF_Win30_SP", "BF_Win31_SP", "BF_Win32_SP", "BF_Win40_SP", "BF_Win41_SP", "BF_Win42_SP", "BF_FlatPQT_SP",
                             "BF_Flat00_SP", "BF_Flat11_SP", "BF_Flat22_SP", "BF_Flat33_SP", "BF_FlatPQT_SP", 
                             "BF_Lose01_SP", "BF_Lose02_SP", "BF_Lose12_SP", "BF_Lose03_SP", "BF_Lose13_SP", "BF_Lose23_SP", "BF_Lose04_SP", "BF_Lose14_SP", "BF_Lose24_SP", "BF_LoseFQT_SP" };
    static string[] BJDC_DS = { "SXDS_SD_SP", "SXDS_SS_SP", "SXDS_XD_SP", "SXDS_XS_SP" };

    /// <summary>
    /// 让球胜平负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Get4501(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "胜";
                break;
            case "2": res = "平";
                break;
            case "3": res = "负";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }
    public static string GetId4501(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "胜": res = "1";
                break;
            case "平": res = "2";
                break;
            case "负": res = "3";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 总进球
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Get4502(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "0";
                break;
            case "2": res = "1";
                break;
            case "3": res = "2";
                break;
            case "4": res = "3";
                break;
            case "5": res = "4";
                break;
            case "6": res = "5";
                break;
            case "7": res = "6";
                break;
            case "8": res = "7";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }
    public static string GetId4502(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "0": res = "1";
                break;
            case "1": res = "2";
                break;
            case "2": res = "3";
                break;
            case "3": res = "4";
                break;
            case "4": res = "5";
                break;
            case "5": res = "6";
                break;
            case "6": res = "7";
                break;
            case "7": res = "8";
                break;
            case "7+": res = "8";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 上下单双
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Get4503(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "上单";
                break;
            case "2": res = "上双";
                break;
            case "3": res = "下单";
                break;
            case "4": res = "下双";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }
    public static string GetId4503(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "上单": res = "1";
                break;
            case "上双": res = "2";
                break;
            case "下单": res = "3";
                break;
            case "下双": res = "4";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 比分
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Get4504(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "1:0";
                break;
            case "2": res = "2:0";
                break;
            case "3": res = "2:1";
                break;
            case "4": res = "3:0";
                break;
            case "5": res = "3:1";
                break;
            case "6": res = "3:2";
                break;
            case "7": res = "4:0";
                break;
            case "8": res = "4:1";
                break;
            case "9": res = "4:2";
                break;
            case "10": res = "胜其他";
                break;
            case "11": res = "0:0";
                break;
            case "12": res = "1:1";
                break;
            case "13": res = "2:2";
                break;
            case "14": res = "3:3";
                break;
            case "15": res = "平其他";
                break;
            case "16": res = "0:1";
                break;
            case "17": res = "0:2";
                break;
            case "18": res = "1:2";
                break;
            case "19": res = "0:3";
                break;
            case "20": res = "1:3";
                break;
            case "21": res = "2:3";
                break;
            case "22": res = "0:4";
                break;
            case "23": res = "1:4";
                break;
            case "24": res = "2:4";
                break;
            case "25": res = "负其他";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }
    public static string GetId4504(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1:0": res = "1";
                break;
            case "2:0": res = "2";
                break;
            case "2:1": res = "3";
                break;
            case "3:0": res = "4";
                break;
            case "3:1": res = "5";
                break;
            case "3:2": res = "6";
                break;
            case "4:0": res = "7";
                break;
            case "4:1": res = "8";
                break;
            case "4:2": res = "9";
                break;
            case "胜其他": res = "10";
                break;
            case "0:0": res = "11";
                break;
            case "1:1": res = "12";
                break;
            case "2:2": res = "13";
                break;
            case "3:3": res = "14";
                break;
            case "平其他": res = "15";
                break;
            case "0:1": res = "16";
                break;
            case "0:2": res = "17";
                break;
            case "1:2": res = "18";
                break;
            case "0:3": res = "19";
                break;
            case "1:3": res = "20";
                break;
            case "2:3": res = "21";
                break;
            case "0:4": res = "22";
                break;
            case "1:4": res = "23";
                break;
            case "2:4": res = "24";
                break;
            case "负其他": res = "25";
                break;

            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 半全场
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Get4505(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "胜胜";
                break;
            case "2": res = "胜平";
                break;
            case "3": res = "胜负";
                break;
            case "4": res = "平胜";
                break;
            case "5": res = "平平";
                break;
            case "6": res = "平负";
                break;
            case "7": res = "负胜";
                break;
            case "8": res = "负平";
                break;
            case "9": res = "负负";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }
    public static string GetId4505(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "胜胜": res = "1";
                break;
            case "胜平": res = "2";
                break;
            case "胜负": res = "3";
                break;
            case "平胜": res = "4";
                break;
            case "平平": res = "5";
                break;
            case "平负": res = "6";
                break;
            case "负胜": res = "7";
                break;
            case "负平": res = "8";
                break;
            case "负负": res = "9";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }
    #endregion

    #region 篮彩

    static string[] SF = { "MainLose", "MainWin" };
    static string[] SFC = { "DifferGuest1_5", "DifferMain1_5", "DifferGuest6_10", "DifferMain6_10", "DifferGuest11_15", "DifferMain11_15", "DifferGuest16_20", "DifferMain16_20", "DifferGuest21_25", "DifferMain21_25", "DifferGuest26", "DifferMain26" };
    static string[] SFC_HH = { "DifferGuest1_5", "DifferGuest6_10", "DifferGuest11_15", "DifferGuest16_20", "DifferGuest21_25", "DifferGuest26", "DifferMain1_5", "DifferMain6_10", "DifferMain11_15", "DifferMain16_20", "DifferMain21_25", "DifferMain26" };
    static string[] DX = {  "big","small" };
    static string[] RFSF = { "letmainlose", "letmainwin" };

    /// <summary>
    /// 胜负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Get7301(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "主负";
                break;
            case "2": res = "主胜";
                break;

            default:
                res = "";
                break;
        }

        return res;
    }
    public static string GetId7301(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "主负": res = "1";
                break;
            case "主胜": res = "2";
                break;

            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 让分胜负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Get7302(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "让分主负";
                break;
            case "2": res = "让分主胜";
                break;

            default:
                res = "";
                break;
        }

        return res;
    }
    public static string GetId7302(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "让分主负": res = "1";
                break;
            case "让分主胜": res = "2";
                break;

            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 胜分差
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Get7303(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "客胜1-5";
                break;
            case "2": res = "主胜1-5";
                break;
            case "3": res = "客胜6-10";
                break;
            case "4": res = "主胜6-10";
                break;
            case "5": res = "客胜11-15";
                break;
            case "6": res = "主胜11-15";
                break;
            case "7": res = "客胜16-20";
                break;
            case "8": res = "主胜16-20";
                break;
            case "9": res = "客胜21-25";
                break;
            case "10": res = "主胜21-25";
                break;
            case "11": res = "客胜26+";
                break;
            case "12": res = "主胜26+";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }
    public static string GetH7303(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "客胜1-5";
                break;
            case "2": res = "客胜6-10";
                break;
            case "3": res = "客胜11-15";
                break;
            case "4": res = "客胜16-20";
                break;
            case "5": res = "客胜21-25";
                break;
            case "6": res = "客胜26+";
                break;
            case "7": res = "主胜1-5";
                break;
            case "8": res = "主胜6-10";
                break;
            case "9": res = "主胜11-15";
                break;
            case "10": res = "主胜16-20";
                break;
            case "11": res = "主胜21-25";
                break;
            case "12": res = "主胜26+";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }
    public static string GetId7303(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "客胜1-5分": res = "1";
                break;
            case "主胜1-5分": res = "2";
                break;
            case "客胜6-10分": res = "3";
                break;
            case "主胜6-10分": res = "4";
                break;
            case "客胜11-15分": res = "5";
                break;
            case "主胜11-15分": res = "6";
                break;
            case "客胜16-20分": res = "7";
                break;
            case "主胜16-20分": res = "8";
                break;
            case "客胜21-25分": res = "9";
                break;
            case "主胜21-25分": res = "10";
                break;
            case "客胜26+分": res = "11";
                break;
            case "主胜26+分": res = "12";
                break;
            case "客胜1-5": res = "1";
                break;
            case "主胜1-5": res = "2";
                break;
            case "客胜6-10": res = "3";
                break;
            case "主胜6-10": res = "4";
                break;
            case "客胜11-15": res = "5";
                break;
            case "主胜11-15": res = "6";
                break;
            case "客胜16-20": res = "7";
                break;
            case "主胜16-20": res = "8";
                break;
            case "客胜21-25": res = "9";
                break;
            case "主胜21-25": res = "10";
                break;
            case "客胜26+": res = "11";
                break;
            case "主胜26+": res = "12";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    public static string GetHId7303(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "客胜1-5分": res = "1";
                break;
            case "客胜6-10分": res = "2";
                break;
            case "客胜11-15分": res = "3";
                break;
            case "客胜16-20分": res = "4";
                break;
            case "客胜21-25分": res = "5";
                break;
            case "主胜1-5分": res = "7";
                break;
            case "主胜6-10分": res = "8";
                break;
            case "主胜11-15分": res = "9";
                break;
            case "主胜16-20分": res = "10";
                break;
            case "主胜21-25分": res = "11";
                break;
            case "客胜1-5": res = "1";
                break;
            case "客胜6-10": res = "2";
                break;
            case "客胜11-15": res = "3";
                break;
            case "客胜16-20": res = "4";
                break;
            case "客胜21-25": res = "5";
                break;
            case "客胜26+": res = "6";
                break;
            case "主胜1-5": res = "7";
                break;
            case "主胜6-10": res = "8";
                break;
            case "主胜11-15": res = "9";
                break;
            case "主胜16-20": res = "10";
                break;
            case "主胜21-25": res = "11";
                break;
            case "主胜26+": res = "12";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 大小分
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string Get7304(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "大";
                break;
            case "2": res = "小";
                break;

            default:
                res = "";
                break;
        }

        return res;
    }
    public static string GetId7304(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "大": res = "1";
                break;
            case "小": res = "2";
                break;

            default:
                res = "";
                break;
        }

        return res;
    }

    #endregion 篮彩

    // 中奖的记录，发送通知
    public static void SendWinNotification(DataSet ds)
    {
        if (ds == null)
        {
            return;
        }

        for (int i = 0; i < ds.Tables.Count; i++)
        {
            DataTable dt = ds.Tables[i];

            if (dt.Rows.Count < 1)
            {
                continue;
            }

            long SiteID = long.Parse(dt.Rows[0]["SiteID"].ToString());
            Sites ts = new Sites()[SiteID];

            if (ts == null)
            {
                continue;
            }

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DataRow dr = dt.Rows[j];

                Users tu = new Users(ts.ID)[ts.ID, long.Parse(dr["UserID"].ToString())];
                string schemeNo = "";
                if (tu == null)
                {
                    continue;
                }
                string userName = tu.Name;
                if (userName.Length < 1)
                {
                    userName = tu.Mobile;
                }
                DataTable schemeDt = new DAL.Tables.T_Schemes().Open("SchemeNumber", "ID=" + dr["SchemeID"].ToString(), "");
                if (schemeDt != null && schemeDt.Rows.Count > 0)
                {
                    schemeNo = schemeDt.Rows[0][0].ToString();
                }
                string content = MessageTemplate.SchemeWinning.Replace("[WebSiteName]", ts.Name).Replace("[UserName]", userName).Replace("[SchemeNo]", schemeNo);
                string resultMsg = "";
                bool resultBool = Message.Send(Convert.ToInt32(tu.ID), "", "", content, "", MessageType.SchemeWinning, SendTypeSingle.SelfAdaption, ref resultMsg);
            }
        }
    }

    public static void DataGridBindData(DataGrid g, DataTable dt)
    {
        g.DataSource = dt;

        try
        {
            g.DataBind();
        }
        catch (Exception e)
        {
            if (e.Message.Contains("无效的 CurrentPageIndex 值。它必须大于等于 0 且小于 PageCount。"))
            {
                g.CurrentPageIndex = 0;
            }
            else
            {
                throw new Exception(e.Message);
            }
        }
    }

    public static void DataGridBindData(DataGrid g, DataTable dt, Shove.Web.UI.ShoveGridPager gPager)
    {
        g.DataSource = dt;

        try
        {
            g.DataBind();
        }
        catch (Exception e)
        {
            if (e.Message.Contains("无效的 CurrentPageIndex 值。它必须大于等于 0 且小于 PageCount。"))
            {
                g.CurrentPageIndex = 0;
                gPager.PageIndex = 0;
            }
            else
            {
                throw new Exception(e.Message);
            }
        }

        gPager.Visible = (dt.Rows.Count > 0);
    }

    public static void DataGridBindData(DataGrid g, DataView dv, Shove.Web.UI.ShoveGridPager gPager)
    {
        g.DataSource = dv;

        try
        {
            g.DataBind();
        }
        catch (Exception e)
        {
            if (e.Message.Contains("无效的 CurrentPageIndex 值。它必须大于等于 0 且小于 PageCount。"))
            {
                g.CurrentPageIndex = 0;
                gPager.PageIndex = 0;
            }
            else
            {
                throw new Exception(e.Message);
            }
        }

        gPager.Visible = (dv.Count > 0);
    }

    public static string FilterNoRestrictionsLottery(string LotteryListRestrictions, string LotteryList)
    {
        if ((LotteryListRestrictions == "") || (LotteryList == ""))
        {
            return "";
        }

        LotteryListRestrictions = "," + LotteryListRestrictions + ",";

        string[] strs = LotteryList.Split(',');

        if ((strs == null) || (strs.Length < 1))
        {
            return "";
        }

        string Result = "";

        foreach (string str in strs)
        {
            if (LotteryListRestrictions.IndexOf("," + str + ",") >= 0)
            {
                if (Result != "")
                {
                    Result += ",";
                }

                Result += str;
            }
        }

        return Result;
    }

    // 获取 Url 返回的 Html 流
    public static string Post(string Url, string RequestString, int TimeoutSeconds)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

        if (TimeoutSeconds > 0)
        {
            request.Timeout = 1000 * TimeoutSeconds;
        }
        request.Method = "POST";
        request.AllowAutoRedirect = true;
        request.ContentType = "application/x-www-form-urlencoded";

        byte[] data = System.Text.Encoding.GetEncoding("gb2312").GetBytes(RequestString);

        Stream outstream = request.GetRequestStream();
        outstream.Write(data, 0, data.Length);
        outstream.Close();

        HttpWebResponse hwrp = (HttpWebResponse)request.GetResponse();
        StreamReader sr = new StreamReader(hwrp.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));

        return sr.ReadToEnd();
    }

    // 获取 Url 返回的 Html 流
    public static string GetHtml(string Url, string encodeing, int TimeoutSeconds)
    {
        HttpWebRequest request = null;
        HttpWebResponse response = null;
        StreamReader reader = null;
        try
        {
            request = (HttpWebRequest)WebRequest.Create(Url);
            request.UserAgent = "www.svnhost.cn";
            if (TimeoutSeconds > 0)
            {
                request.Timeout = 1000 * TimeoutSeconds;
            }
            request.AllowAutoRedirect = false;

            response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(encodeing));
                string html = reader.ReadToEnd();
                return html;
            }
            else
            {
                return "";
            }
        }
        catch (SystemException)
        {
            return "";
        }
    }

    //获取彩种的第二种类别ID
    public static int GetLotteryType2(int LotteryID)
    {
        int LotteryType2 = 0;

        object obj = Shove.Database.MSSQL.ExecuteScalar("Select [Type2] From T_Lotteries Where [ID] = @p1", new Shove.Database.MSSQL.Parameter("@p1", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID));

        if (obj == null)
        {
            return LotteryType2;
        }
        else
        {
            LotteryType2 = int.Parse(obj.ToString());
        }

        return LotteryType2;
    }

    #region 快乐扑克格式转换
    //系统格式转换成湖北快乐扑克期官方格式
    public static string ConvertIsuseName_HBKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-18", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002010 + Day * 49 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成山东快乐扑克期官方格式
    public static string ConvertIsuseName_SDKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002280 + Day * 53 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成河北快乐扑克期官方格式
    public static string ConvertIsuseName_HeBKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002280 + Day * 53 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成安徽快乐扑克期官方格式
    public static string ConvertIsuseName_AHKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002065 + Day * 48 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成黑龙江快乐扑克期官方格式
    public static string ConvertIsuseName_HLJKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002280 + Day * 53 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成辽宁快乐扑克期官方格式
    public static string ConvertIsuseName_LLKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002280 + Day * 53 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成陕西快乐扑克期官方格式
    public static string ConvertIsuseName_SXKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8001936 + Day * 49 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成浙江快乐扑克期官方格式
    public static string ConvertIsuseName_ZJKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002237 + Day * 52 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成四川快乐扑克期官方格式
    public static string ConvertIsuseName_SCKLPK(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2008-2-20", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (8002237 + Day * 52 + Time - 1).ToString();

        return Isuse;
    }

    //系统格式转换成山西快乐扑克期官方格式
    public static string ConvertIsuseName_ShXKLPK(string IsuseName)
    {
        return IsuseName;
    }
    #endregion

    //系统格式转换成十一运夺金方格式
    public static string ConvertIsuseName_SYYDJ(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }

        return IsuseName.Substring(2);
    }

    //系统格式转换成江西时时彩期官方格式
    public static string ConvertIsuseName_JxSSC(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }

        return IsuseName.Substring(4).Insert(4, "0");
    }

    //系统格式转换成快赢481的格式
    public static string ConvertIsuseName_HNKY481(string IsuseName)
    {
        int length = IsuseName.Length;

        if (length != 10)
        {
            return IsuseName;
        }
        string Datetime = IsuseName.Substring(0, 4) + "-" + IsuseName.Substring(4, 2) + "-" + IsuseName.Substring(6, 2);
        TimeSpan ts;

        try
        {
            ts = Shove._Convert.StrToDateTime(Datetime, "0000-00-00") - Shove._Convert.StrToDateTime("2010-02-22", "0000-00-00");
        }
        catch
        {
            return IsuseName;
        }

        double Day = 0;
        double Time = 0;

        Day = ts.Days;
        Time = Shove._Convert.StrToInt(IsuseName.Substring((IsuseName.Length - 2)), 0);

        string Isuse = (10003240 + Day * 72 + Time).ToString();  //09018200 是2009年12月8号的最后一期期号

        return Isuse;
    }

    //格式化中奖号码
    public static void FormatNumber(int LotteryID, string LastWinNumber, out string[] RedNumber, out string[] OrangeNumber, out string[] BlueNumber)
    {
        //若有两个空格，替换成一个空格
        LastWinNumber = LastWinNumber.Replace("  ", " ");

        if (LotteryID == 5) //双色球(10 11 15 16 25 29 + 02)
        {
            try
            {
                RedNumber = LastWinNumber.Split(new string[] { " + " }, StringSplitOptions.RemoveEmptyEntries)[0].Split(' ');
            }
            catch
            {
                RedNumber = new string[0];
            }

            try
            {
                BlueNumber = new string[] { LastWinNumber.Split(new string[] { " + " }, StringSplitOptions.RemoveEmptyEntries)[1] };
            }
            catch
            {
                BlueNumber = new string[0];
            }

            OrangeNumber = new string[0];

            return;
        }

        if (LotteryID == 6 || LotteryID == 29)//福彩3D(813) //时时彩(812)
        {
            try
            {
                string Number = LastWinNumber.Trim();
                BlueNumber = new string[Number.Length];

                for (int i = 0; i < BlueNumber.Length; i++)
                {
                    BlueNumber[i] = Number.Substring(i, 1);
                }
            }
            catch
            {
                BlueNumber = new string[0];
            }

            RedNumber = new string[0];
            OrangeNumber = new string[0];

            return;
        }

        if (LotteryID == 13) //七乐彩
        {
            try
            {
                OrangeNumber = LastWinNumber.Split(new string[] { " + " }, StringSplitOptions.RemoveEmptyEntries)[0].Split(' ');
            }
            catch
            {
                OrangeNumber = new string[0];
            }

            try
            {
                BlueNumber = new string[] { LastWinNumber.Split(new string[] { " + " }, StringSplitOptions.RemoveEmptyEntries)[1] };
            }
            catch
            {
                BlueNumber = new string[0];
            }

            RedNumber = new string[0];

            return;
        }

        if (LotteryID == 59) //15X5
        {
            try
            {
                RedNumber = LastWinNumber.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries)[0].Split(' ');
            }
            catch
            {
                RedNumber = new string[0];
            }

            try
            {
                BlueNumber = new string[] { LastWinNumber.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries)[1] };
            }
            catch
            {
                BlueNumber = new string[0];
            }

            OrangeNumber = new string[0];

            return;
        }

        if (LotteryID == 58)//东方6+1(451069+鼠)
        {
            try
            {
                string Number = LastWinNumber.Split('+')[0];
                OrangeNumber = new string[Number.Length];

                for (int i = 0; i < OrangeNumber.Length; i++)
                {
                    OrangeNumber[i] = Number.Substring(i, 1);
                }
            }
            catch
            {
                OrangeNumber = new string[0];
            }

            try
            {
                BlueNumber = new string[] { LastWinNumber.Split('+')[1] };
            }
            catch
            {
                BlueNumber = new string[0];
            }

            RedNumber = new string[0];

            return;
        }

        if (LotteryID == 60 || LotteryID == 61)//天天彩选4(3204)
        {
            try
            {
                string Number = LastWinNumber.Trim();
                RedNumber = new string[Number.Length];

                for (int i = 0; i < RedNumber.Length; i++)
                {
                    RedNumber[i] = Number.Substring(i, 1);
                }
            }
            catch
            {
                RedNumber = new string[0];
            }

            OrangeNumber = new string[0];
            BlueNumber = new string[0];

            return;
        }

        RedNumber = new string[0];
        OrangeNumber = new string[0];
        BlueNumber = new string[0];
    }

    /// <summary>
    /// 用户名合法校验
    /// 1,只能以汉字,大小写字母,数字,下划线开头开头 
    /// 2,不能有其他特殊字符 
    /// 3,长度在2--30位
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public static bool CheckUserName(string userName)
    {
        Regex regex = new Regex(@"^[\u4e00-\u9fa5a-zA-Z0-9_]{1}[\w]{1,29}$|^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Match m = regex.Match(userName);

        if (m.Success)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// UBB代码处理函数
    /// </summary>
    ///	<param name="_postpramsinfo">UBB转换参数表</param>
    /// <returns>输出字符串</returns>
    public static string UBBToHTML(string sDetail)
    {
        RegexOptions options = RegexOptions.IgnoreCase;
        StringBuilder sb = new StringBuilder();

        // [smilie]处标记
        sDetail = Regex.Replace(sDetail, @"\[smilie\](.+?)\[\/smilie\]", "<img src=\"$1\" />", options);

        sDetail = Regex.Replace(sDetail, @"\[b(?:\s*)\]", "<b>", options);
        sDetail = Regex.Replace(sDetail, @"\[i(?:\s*)\]", "<i>", options);
        sDetail = Regex.Replace(sDetail, @"\[u(?:\s*)\]", "<u>", options);
        sDetail = Regex.Replace(sDetail, @"\[/b(?:\s*)\]", "</b>", options);
        sDetail = Regex.Replace(sDetail, @"\[/i(?:\s*)\]", "</i>", options);
        sDetail = Regex.Replace(sDetail, @"\[/u(?:\s*)\]", "</u>", options);

        // Sub/Sup
        //
        sDetail = Regex.Replace(sDetail, @"\[sup(?:\s*)\]", "<sup>", options);
        sDetail = Regex.Replace(sDetail, @"\[sub(?:\s*)\]", "<sub>", options);
        sDetail = Regex.Replace(sDetail, @"\[/sup(?:\s*)\]", "</sup>", options);
        sDetail = Regex.Replace(sDetail, @"\[/sub(?:\s*)\]", "</sub>", options);

        // P
        //
        sDetail = Regex.Replace(sDetail, @"((\r\n)*\[p\])(.*?)((\r\n)*\[\/p\])", "<p>$3</p>", RegexOptions.IgnoreCase | RegexOptions.Singleline);




        // Anchors
        //
        sDetail = Regex.Replace(sDetail, @"\[url(?:\s*)\](www\.(.*?))\[/url(?:\s*)\]", "<a href=\"http://$1\" target=\"_blank\">$1</a>", options);
        sDetail = Regex.Replace(sDetail, @"\[url(?:\s*)\]\s*((https?://|ftp://|gopher://|news://|telnet://|rtsp://|mms://|callto://|bctp://|ed2k://|tencent)([^\[""']+?))\s*\[\/url(?:\s*)\]", "<a href=\"$1\" target=\"_blank\">$1</a>", options);
        sDetail = Regex.Replace(sDetail, @"\[url=www.([^\[""']+?)(?:\s*)\]([\s\S]+?)\[/url(?:\s*)\]", "<a href=\"http://www.$1\" target=\"_blank\">$2</a>", options);
        sDetail = Regex.Replace(sDetail, @"\[url=((https?://|ftp://|gopher://|news://|telnet://|rtsp://|mms://|callto://|bctp://|ed2k://|tencent://)([^\[""']+?))(?:\s*)\]([\s\S]+?)\[/url(?:\s*)\]", "<a href=\"$1\" target=\"_blank\">$4</a>", options);

        // Email
        //
        sDetail = Regex.Replace(sDetail, @"\[email(?:\s*)\](.*?)\[\/email\]", "<a href=\"mailto:$1\" target=\"_blank\">$1</a>", options);
        sDetail = Regex.Replace(sDetail, @"\[email=(.[^\[]*)(?:\s*)\](.*?)\[\/email(?:\s*)\]", "<a href=\"mailto:$1\" target=\"_blank\">$2</a>", options);

        #region

        // Font
        //
        sDetail = Regex.Replace(sDetail, @"\[color=([^\[\<]+?)\]", "<font color=\"$1\">", options);
        sDetail = Regex.Replace(sDetail, @"\[size=(\d+?)\]", "<font size=\"$1\">", options);
        sDetail = Regex.Replace(sDetail, @"\[size=(\d+(\.\d+)?(px|pt|in|cm|mm|pc|em|ex|%)+?)\]", "<font style=\"font-size: $1\">", options);
        sDetail = Regex.Replace(sDetail, @"\[font=([^\[\<]+?)\]", "<font face=\"$1\">", options);

        sDetail = Regex.Replace(sDetail, @"\[align=([^\[\<]+?)\]", "<p align=\"$1\">", options);
        sDetail = Regex.Replace(sDetail, @"\[float=(left|right)\]", "<br style=\"clear: both\"><span style=\"float: $1;\">", options);


        sDetail = Regex.Replace(sDetail, @"\[/color(?:\s*)\]", "</font>", options);
        sDetail = Regex.Replace(sDetail, @"\[/size(?:\s*)\]", "</font>", options);
        sDetail = Regex.Replace(sDetail, @"\[/font(?:\s*)\]", "</font>", options);
        sDetail = Regex.Replace(sDetail, @"\[/align(?:\s*)\]", "</p>", options);
        sDetail = Regex.Replace(sDetail, @"\[/float(?:\s*)\]", "</span>", options);

        // BlockQuote
        //
        sDetail = Regex.Replace(sDetail, @"\[indent(?:\s*)\]", "<blockquote>", options);
        sDetail = Regex.Replace(sDetail, @"\[/indent(?:\s*)\]", "</blockquote>", options);
        sDetail = Regex.Replace(sDetail, @"\[simpletag(?:\s*)\]", "<blockquote>", options);
        sDetail = Regex.Replace(sDetail, @"\[/simpletag(?:\s*)\]", "</blockquote>", options);

        // List
        //
        sDetail = Regex.Replace(sDetail, @"\[list\]", "<ul>", options);
        sDetail = Regex.Replace(sDetail, @"\[list=(1|A|a|I|i| )\]", "<ul type=\"$1\">", options);
        sDetail = Regex.Replace(sDetail, @"\[\*\]", "<li>", options);
        sDetail = Regex.Replace(sDetail, @"\[/list\]", "</ul>", options);


        #endregion

        // shadow
        //
        sDetail = Regex.Replace(sDetail, @"(\[SHADOW=)(\d*?),(#*\w*?),(\d*?)\]([\s]||[\s\S]+?)(\[\/SHADOW\])", "<table width='$2'  style='filter:SHADOW(COLOR=$3, STRENGTH=$4)'>$5</table>", options);

        // glow
        //
        sDetail = Regex.Replace(sDetail, @"(\[glow=)(\d*?),(#*\w*?),(\d*?)\]([\s]||[\s\S]+?)(\[\/glow\])", "<table width='$2'  style='filter:GLOW(COLOR=$3, STRENGTH=$4)'>$5</table>", options);

        // center
        //
        sDetail = Regex.Replace(sDetail, @"\[center\]([\s]||[\s\S]+?)\[\/center\]", "<center>$1</center>", options);


        #region 处理[quote][/quote]标记

        int intQuoteIndexOf = sDetail.ToLower().IndexOf("[quote]");
        int quotecount = 0;
        while (intQuoteIndexOf >= 0 && sDetail.ToLower().IndexOf("[/quote]") >= 0 && quotecount < 3)
        {
            quotecount++;
            sDetail = Regex.Replace(sDetail, @"\[quote\]([\s\S]+?)\[/quote\]", "<br /><br /><div class=\"msgheader\">引用:</div><div class=\"msgborder\">$1</div>", options);


            intQuoteIndexOf = sDetail.ToLower().IndexOf("[quote]", intQuoteIndexOf + 7);
        }

        #endregion


        //处理[area]标签
        sDetail = Regex.Replace(sDetail, @"\[area=([\s\S]+?)\]([\s\S]+?)\[/area\]", "<br /><br /><div class=\"msgheader\">$1</div><div class=\"msgborder\">$2</div>", options);

        sDetail = Regex.Replace(sDetail, @"\[area\]([\s\S]+?)\[/area\]", "<br /><br /><div class=\"msgheader\"></div><div class=\"msgborder\">$1</div>", options);



        #region 将网址字符串转换为链接

        sDetail = sDetail.Replace("&amp;", "&");

        // p2p link
        sDetail = Regex.Replace(sDetail, @"^((tencent|ed2k|thunder|vagaa):\/\/[\[\]\|A-Za-z0-9\.\/=\?%\-&_~`@':+!]+)", "<a target=\"_blank\" href=\"$1\">$1</a>", options);
        sDetail = Regex.Replace(sDetail, @"((tencent|ed2k|thunder|vagaa):\/\/[\[\]\|A-Za-z0-9\.\/=\?%\-&_~`@':+!]+)$", "<a target=\"_blank\" href=\"$1\">$1</a>", options);
        sDetail = Regex.Replace(sDetail, @"[^>=\]""]((tencent|ed2k|thunder|vagaa):\/\/[\[\]\|A-Za-z0-9\.\/=\?%\-&_~`@':+!]+)", "<a target=\"_blank\" href=\"$1\">$1</a>", options);
        #endregion


        #region 处理[img][/img]标记

        sDetail = Regex.Replace(sDetail, @"\[img\]\s*(http://[^\[\<\r\n]+?)\s*\[\/img\]", "<img src=\"$1\" border=\"0\" />", options);
        sDetail = Regex.Replace(sDetail, @"\[img=(\d{1,4})[x|\,](\d{1,4})\]\s*(http://[^\[\<\r\n]+?)\s*\[\/img\]", "<img src=\"$3\" width=\"$1\" height=\"$2\" border=\"0\" />", options);
        sDetail = Regex.Replace(sDetail, @"\[image\](http://[\s\S]+?)\[/image\]", "<img src=\"$1\" border=\"0\" />", options);
        sDetail = Regex.Replace(sDetail, @"\[img\]\s*(http://[^\[\<\r\n]+?)\s*\[\/img\]", "<img src=\"$1\" border=\"0\" onload=\"attachimg(this, 'load');\" onclick=\"zoom(this)\" />", options);
        sDetail = Regex.Replace(sDetail, @"\[img=(\d{1,4})[x|\,](\d{1,4})\]\s*(http://[^\[\<\r\n]+?)\s*\[\/img\]", "<img src=\"$3\" width=\"$1\" height=\"$2\" border=\"0\" onload=\"attachimg(this, 'load');\" onclick=\"zoom(this)\"  />", options);
        sDetail = Regex.Replace(sDetail, @"\[image\](http://[\s\S]+?)\[/image\]", "<img src=\"$1\" border=\"0\" />", options);

        #endregion

        #region 处理空格

        sDetail = sDetail.Replace("\t", "&nbsp; &nbsp; ");
        sDetail = sDetail.Replace("  ", "&nbsp; ");

        #endregion

        // [r/]
        //
        sDetail = Regex.Replace(sDetail, @"\[r/\]", "\r", options);

        // [n/]
        //
        sDetail = Regex.Replace(sDetail, @"\[n/\]", "\n", options);


        #region 处理换行

        //处理换行,在每个新行的前面添加两个全角空格
        sDetail = sDetail.Replace("\r\n", "<br />");
        sDetail = sDetail.Replace("\r", "");
        sDetail = sDetail.Replace("\n\n", "<br /><br />");
        sDetail = sDetail.Replace("\n", "<br />");
        sDetail = sDetail.Replace("{rn}", "\r\n");
        sDetail = sDetail.Replace("{nn}", "\n\n");
        sDetail = sDetail.Replace("{r}", "\r");
        sDetail = sDetail.Replace("{n}", "\n");

        #endregion

        return sDetail;
    }

    ///   <summary>
    ///   去除HTML标记
    ///   </summary>
    ///   <param   name="NoHTML">包括HTML的源码   </param>
    ///   <returns>已经去除后的文字</returns>
    public static string NoHTML(string Htmlstring)
    {
        //删除脚本
        Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "",
          RegexOptions.IgnoreCase);
        //删除HTML
        Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9",
          RegexOptions.IgnoreCase);
        Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "",
          RegexOptions.IgnoreCase);

        Htmlstring.Replace("<", "");
        Htmlstring.Replace(">", "");
        Htmlstring.Replace("\r\n", "");
        Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

        return Htmlstring;
    }

    /// <summary>
    /// 获取盈利指数 
    /// 盈利指数：每次盈利金额对应的分数称为盈利指数
    /// 盈利金额=中奖金额-方案金额 
    /// </summary>
    /// <param name="profitMoney">盈利金额</param>
    /// <returns>盈利指数</returns>
    public static int GetProfitPoints(double profitMoney)
    {
        int profitPoints = 0;
        if (profitMoney >= 1 && profitMoney <= 20)
        {
            profitMoney = 1;
        }
        else if (profitMoney >= 21 && profitMoney <= 50)
        {
            profitMoney = 2;
        }
        else if (profitMoney >= 51 && profitMoney <= 100)
        {
            profitMoney = 5;
        }
        else if (profitMoney >= 101 && profitMoney <= 200)
        {
            profitMoney = 10;
        }
        else if (profitMoney >= 201 && profitMoney <= 500)
        {
            profitMoney = 20;
        }
        else if (profitMoney >= 501 && profitMoney <= 1000)
        {
            profitMoney = 40;
        }
        else if (profitMoney >= 1001 && profitMoney <= 2000)
        {
            profitMoney = 80;
        }
        else if (profitMoney >= 2001 && profitMoney <= 5000)
        {
            profitMoney = 160;
        }
        else if (profitMoney >= 5001 && profitMoney <= 10000)
        {
            profitMoney = 320;
        }
        else if (profitMoney >= 10001 && profitMoney <= 20000)
        {
            profitMoney = 640;
        }
        else if (profitMoney >= 20001 && profitMoney <= 50000)
        {
            profitMoney = 1280;
        }
        else if (profitMoney >= 50001 && profitMoney <= 100000)
        {
            profitMoney = 2560;
        }
        else if (profitMoney >= 100001 && profitMoney <= 500000)
        {
            profitMoney = 5120;
        }
        else if (profitMoney >= 500001 && profitMoney <= 1000000)
        {
            profitMoney = 10240;
        }
        else if (profitMoney >= 1000001 && profitMoney <= 2000000)
        {
            profitMoney = 20480;
        }
        else if (profitMoney >= 2000001 && profitMoney <= 5000000)
        {
            profitMoney = 40960;
        }
        else if (profitMoney >= 5000001)
        {
            profitMoney = 81920;
        }
        return profitPoints;
    }

    /// <summary>
    /// 根据某列的值 查询某个表中该列的总值
    /// </summary>
    /// <param name="dt">要计算的表格</param>
    /// <param name="index">列所在的索引 从0开始</param>
    /// <param name="isPage">是否是统计本页数据</param>
    /// <param name="pageSize">每页记录数</param>
    /// <param name="pageIndex">统计的 dt 的当前页索引</param>
    /// <returns></returns>
    public static double GetSumByColumn(DataTable dt, int index, bool isPage, int pageSize, int pageIndex)
    {
        double sum = 0;

        int columnLength = dt.Columns.Count;
        if (index >= columnLength || index < 0)
        {
            return sum;
        }
        else
        {
            int rowLength = dt.Rows.Count;
            if (!isPage)    //统计总数据
            {
                for (int i = 0; i < rowLength; i++)
                {
                    sum += Shove._Convert.StrToDouble(dt.Rows[i][index].ToString(), 0);
                }
            }
            else          //统计当前页数据
            {
                rowLength = (pageIndex + 1) * pageSize;
                if (rowLength > dt.Rows.Count)
                {
                    rowLength = (pageIndex) * pageSize + (dt.Rows.Count % pageSize);
                }
                for (int i = pageIndex * pageSize; i < rowLength; i++)
                {
                    sum += Shove._Convert.StrToDouble(dt.Rows[i][index].ToString(), 0);
                }
            }
        }
        return sum;
    }


    /// <summary>
    /// 指定的提款方式,计算所需的提款手续费
    /// </summary>
    /// <param name="DistillType">提款方式: 1 支付宝提款, 2.银行卡提款</param>
    /// <param name="DistillMoney">提款金额</param>
    /// <param name="BankDetailedName">如果是 银行卡提款 要提供具体的银行名称,如:广东省深圳市中国工商银行宝安支行</param>
    /// <returns></returns>
    /*  工商银行  工行本地不收，异地最低1.8元，最高45元，200~5000按照0.9%收取  
        招商银行  招行本地不收，异地最低5元，最高50元，2500~25000按照0.2%收取  
        建设银行  建行本地不收，异地最低2元，最高25元，800~10000以下按照0.25%收取  
        其它银行  深圳本地2元/笔，异地最低10元/笔，最高50元/笔，1000~5000元按照1%收取  
        支付宝账户  实时到帐  5000元以内的1块钱，5000元以上每笔10元手续费。
    */

    public static double CalculateDistillFormalitiesFees(int DistillType, double DistillMoney, string BankDetailedName)
    {
        double formalitiesFees = 0;
        if (DistillType == 1)//支付宝提款
        {
            if (DistillMoney < 10000)
            {
                formalitiesFees = 1;
            }
            else
            {
                formalitiesFees = 10;
            }
        }
        else if (DistillType == 2)//银行卡提款
        {
            if (BankDetailedName.Replace("深圳发展", "").IndexOf("深圳") >= 0) //本地:银行卡
            {
                if (BankDetailedName.IndexOf("工商银行") >= 0 || BankDetailedName.IndexOf("招商银行") >= 0 || BankDetailedName.IndexOf("建设银行") >= 0
                    || BankDetailedName.IndexOf("工行") >= 0 || BankDetailedName.IndexOf("招行") >= 0 || BankDetailedName.IndexOf("建行") >= 0)
                {
                    formalitiesFees = 0;
                }
                else
                {
                    formalitiesFees = 2;
                }
            }
            else //异地:银行卡
            {
                if (BankDetailedName.IndexOf("工商银行") >= 0 || BankDetailedName.IndexOf("工行") >= 0)
                {
                    formalitiesFees = DistillMoney * 0.009;
                    if (formalitiesFees < 1.8)
                    {
                        formalitiesFees = 1.8;//最低1.8元
                    }
                    else if (formalitiesFees > 45)
                    {
                        formalitiesFees = 45;//最高45元
                    }
                }
                else if (BankDetailedName.IndexOf("招商银行") >= 0 || BankDetailedName.IndexOf("招行") >= 0)
                {
                    formalitiesFees = DistillMoney * 0.002;
                    if (formalitiesFees < 5)
                    {
                        formalitiesFees = 5;//最低5元
                    }
                    else if (formalitiesFees > 50)
                    {
                        formalitiesFees = 50;//最高50元
                    }
                }
                else if (BankDetailedName.IndexOf("建设银行") >= 0 || BankDetailedName.IndexOf("建行") >= 0)
                {
                    formalitiesFees = DistillMoney * 0.0025;// 按0.25%
                    if (formalitiesFees < 2)
                    {
                        formalitiesFees = 2;//最低5元
                    }
                    else if (formalitiesFees > 25)
                    {
                        formalitiesFees = 25;//最高25元
                    }
                }
                else //其他银行异地
                {
                    formalitiesFees = DistillMoney * 0.01; // 按1%
                    if (formalitiesFees < 10)
                    {
                        formalitiesFees = 10;//最低10元
                    }
                    else if (formalitiesFees > 50)
                    {
                        formalitiesFees = 50;//最高50元
                    }
                }
            }
        }


        return formalitiesFees;
    }

    #region 系统注册
    /// <summary>
    /// 系统注册
    /// </summary>
    /// <returns></returns>
    public static bool IsSystemRegister()
    {
        return true;
        if (Shove._Web.Utility.GetUrl().ToLower().IndexOf("shovesoft.net") >= 0 || Shove._Web.Utility.GetUrl().ToLower().IndexOf("localhost") >= 0)
        {
            return true;
        }

        if (!Shove._Convert.StrToBool(Shove._Web.WebConfig.GetAppSettingsString("Register"), false))
        {
            return false;
        }

        Shove._Security.License.Refresh(Shove._Web.WebConfig.GetAppSettingsString("com.shove.security.License"));

        return Shove._Security.License.getWebPagesAllow();

    }
    #endregion

    #region 记录竞彩、北京单场玩法的投注记录
    /// <summary>
    /// 记录竞彩、北京单场玩法的投注记录
    /// </summary>
    /// <param name="SchemesID">方案ID</param>
    /// <param name="LotteryNumber">方案号码</param>
    /// <param name="Type">玩法：1，过关，0，单关</param>
    /// <param name="EndTime">提前截止时间分钟（北京单场使用）</param>
    public static void RecordBetting(long SchemesID, string LotteryNumber, string Type, int EndTime = 0)
    {
        string PlayTypeID = LotteryNumber.Split(';')[0].ToString();
        string Number = LotteryNumber.Split(';')[1].ToString();
        string PassType = GetPassWayStr(LotteryNumber);
        string Strticket = "";
        string strticketDan = "";
        if (Number.IndexOf("][") > 0)
        {
            strticketDan = Number.Substring(0, Number.IndexOf("][")).Replace("]", "").Replace("[", "");
            Strticket = Number.Substring(Number.IndexOf("]["), Number.Length - Number.IndexOf("][")).Replace("]", "").Replace("[", "");
        }
        else
        {
            Strticket = Number.Replace("]", "").Replace("[", "");
        }

        if (PlayTypeID == "7201" || PlayTypeID == "7202" || PlayTypeID == "7203" || PlayTypeID == "7204" || PlayTypeID == "7206" || PlayTypeID == "7207")
        {
            if (strticketDan != "")
            {
                string[] ticketDan = strticketDan.Split('|');
                Match(ticketDan, SchemesID, PlayTypeID, "1", Type, PassType);

            }
            if (Strticket != "")
            {
                string[] ticket = Strticket.Split('|');
                Match(ticket, SchemesID, PlayTypeID, "0", Type, PassType);
            }

        }
        else if (PlayTypeID == "4501" || PlayTypeID == "4502" || PlayTypeID == "4503" || PlayTypeID == "4504" || PlayTypeID == "4505")
        {
            PassType = GetPassWayBJDC(LotteryNumber);
            if (strticketDan != "")
            {
                string[] ticketDan = strticketDan.Split('|');
                MatchBJDC(ticketDan, SchemesID, PlayTypeID, "1", Type, PassType, EndTime);

            }
            if (Strticket != "")
            {
                string[] ticket = Strticket.Split('|');
                MatchBJDC(ticket, SchemesID, PlayTypeID, "0", Type, PassType, EndTime);
            }

        }
        else
        {
            if (strticketDan != "")
            {
                string[] ticketDan = strticketDan.Split('|');
                MatchBasket(ticketDan, SchemesID, PlayTypeID, "1", Type, PassType);

            }
            if (Strticket != "")
            {
                string[] ticket = Strticket.Split('|');
                MatchBasket(ticket, SchemesID, PlayTypeID, "0", Type, PassType);
            }
        }
    }

    protected static void MatchBJDC(string[] ticket, long SchemeID, string PlayTypeID, string LetBile, string Type, string PassType, int EndTime)
    {
        DAL.Tables.T_SchemesContent T_SchemesContent = new DAL.Tables.T_SchemesContent();
        DAL.Tables.T_BJSingle T_PassRate = new DAL.Tables.T_BJSingle();

        DAL.Tables.T_ReferenceOdds T_ReferenceOdds = new DAL.Tables.T_ReferenceOdds();
        for (int i = 0; i < ticket.Length; i++)
        {
            string MatchID = ticket[i].Substring(0, ticket[i].IndexOf('('));
            string[] RecordResults = ticket[i].Substring(ticket[i].IndexOf('('), ticket[i].Length - ticket[i].IndexOf('(')).Replace(")", "").Replace("(", "").Split(',');
            DataTable DT = T_PassRate.Open("*", "MatchID=" + MatchID + "", ""); ;

            if (DT == null)
            {
                return;
            }
            if (DT.Rows.Count <= 0)
            {
                continue;
            }
            T_SchemesContent.SchemeID.Value = SchemeID;
            T_SchemesContent.PlayType.Value = PlayTypeID;
            T_SchemesContent.Game.Value = DT.Rows[0]["Game"].ToString();
            T_SchemesContent.MatchNumber.Value = DT.Rows[0]["MatchNumber"].ToString();
            T_SchemesContent.MainTeam.Value = DT.Rows[0]["MainTeam"].ToString();
            T_SchemesContent.GuestTeam.Value = DT.Rows[0]["GuestTeam"].ToString();
            T_SchemesContent.StopSellingTime.Value = Shove._Convert.StrToDateTime(DT.Rows[0]["StopSellTime"].ToString(), DateTime.Now.AddDays(-1).ToString()).AddMinutes(EndTime * -1);
            T_SchemesContent.PassType.Value = PassType;
            T_SchemesContent.LetBile.Value = LetBile;
            T_SchemesContent.MatchID.Value = MatchID;
            long SchemesContentID = T_SchemesContent.Insert();

            if (PlayTypeID == "4501")//让球胜平负
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    T_ReferenceOdds.Content.Value = Get4501(RecordResults[j]);
                    T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][BJDC_RQSPF[int.Parse(RecordResults[j]) - 1]].ToString(), 0).ToString("F2");
                    T_ReferenceOdds.Status.Value = 0;
                    T_ReferenceOdds.Type.Value = "1";
                    T_ReferenceOdds.Insert();
                }
            }
            if (PlayTypeID == "4502")//总进球数
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    T_ReferenceOdds.Content.Value = Get4502(RecordResults[j]);
                    T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][BJDC_ZJQS[int.Parse(RecordResults[j]) - 1]].ToString(), 0).ToString("F2");
                    T_ReferenceOdds.Status.Value = 0;
                    T_ReferenceOdds.Type.Value = "2";
                    T_ReferenceOdds.Insert();
                }
            }
            if (PlayTypeID == "4503")//上下单双
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    T_ReferenceOdds.Content.Value = Get4503(RecordResults[j]);
                    T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][BJDC_DS[int.Parse(RecordResults[j]) - 1]].ToString(), 0).ToString("F2");
                    T_ReferenceOdds.Status.Value = 0;
                    T_ReferenceOdds.Type.Value = "3";
                    T_ReferenceOdds.Insert();
                }
            }
            if (PlayTypeID == "4504")//猜比分
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    T_ReferenceOdds.Content.Value = Get4504(RecordResults[j]);
                    T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][BJDC_BF[int.Parse(RecordResults[j]) - 1]].ToString(), 0).ToString("F2");
                    T_ReferenceOdds.Status.Value = 0;
                    T_ReferenceOdds.Type.Value = "4";
                    T_ReferenceOdds.Insert();
                }
            }

            if (PlayTypeID == "4505")//半全场
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    T_ReferenceOdds.Content.Value = Get4505(RecordResults[j]);
                    T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][BJDC_BQC[int.Parse(RecordResults[j]) - 1]].ToString(), 0).ToString("F2");
                    T_ReferenceOdds.Status.Value = 0;
                    T_ReferenceOdds.Type.Value = "5";
                    T_ReferenceOdds.Insert();
                }
            }
        }
    }
    protected static void Match(string[] ticket, long SchemeID, string PlayTypeID, string LetBile, string Type, string PassType)
    {
        DAL.Tables.T_SchemesContent T_SchemesContent = new DAL.Tables.T_SchemesContent();
        DAL.Tables.T_PassRate T_PassRate = new DAL.Tables.T_PassRate();
        DAL.Tables.T_SingleRate T_SingleRate = new DAL.Tables.T_SingleRate();

        DAL.Tables.T_ReferenceOdds T_ReferenceOdds = new DAL.Tables.T_ReferenceOdds();
        for (int i = 0; i < ticket.Length; i++)
        {
            string MatchID = ticket[i].Substring(0, ticket[i].IndexOf('('));
            string[] RecordResults = ticket[i].Substring(ticket[i].IndexOf('('), ticket[i].Length - ticket[i].IndexOf('(')).Replace(")", "").Replace("(", "").Split(',');
            DataTable DT;
            if (Type == "1")
            {
                DT = T_PassRate.Open("*", "MatchID=" + MatchID + "", "");
            }
            else
            {
                DT = T_SingleRate.Open("*", "MatchID=" + MatchID + "", "");
            }

            if (DT == null)
            {
                return;
            }
            if (DT.Rows.Count <= 0)
            {
                continue;
            }
            T_SchemesContent.SchemeID.Value = SchemeID;
            T_SchemesContent.PlayType.Value = PlayTypeID;
            T_SchemesContent.Game.Value = DT.Rows[0]["Game"].ToString();
            T_SchemesContent.MatchNumber.Value = DT.Rows[0]["MatchNumber"].ToString();
            T_SchemesContent.MainTeam.Value = DT.Rows[0]["MainTeam"].ToString();
            T_SchemesContent.GuestTeam.Value = DT.Rows[0]["GuestTeam"].ToString();
            T_SchemesContent.StopSellingTime.Value = DT.Rows[0]["StopSellTime"].ToString();
            T_SchemesContent.PassType.Value = PassType;
            T_SchemesContent.LetBile.Value = LetBile;
            T_SchemesContent.MatchID.Value = MatchID;
            long SchemesContentID = T_SchemesContent.Insert();

            if (PlayTypeID == "7201")
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    T_ReferenceOdds.Content.Value = Get7201(RecordResults[j]);
                    T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][RQSPF[int.Parse(RecordResults[j]) - 1]].ToString(), 0).ToString("F2");
                    T_ReferenceOdds.Status.Value = 0;
                    T_ReferenceOdds.Type.Value = "1";
                    T_ReferenceOdds.Insert();
                }
            }
            if (PlayTypeID == "7202")
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    T_ReferenceOdds.Content.Value = Get7202(RecordResults[j]);
                    T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][ZQBF[int.Parse(RecordResults[j]) - 1]].ToString(), 0).ToString("F2");
                    T_ReferenceOdds.Status.Value = 0;
                    T_ReferenceOdds.Type.Value = "3";
                    T_ReferenceOdds.Insert();
                }
            }
            if (PlayTypeID == "7203")
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    T_ReferenceOdds.Content.Value = Get7203(RecordResults[j]);
                    T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][ZJQS[int.Parse(RecordResults[j]) - 1]].ToString(), 0).ToString("F2");
                    T_ReferenceOdds.Status.Value = 0;
                    T_ReferenceOdds.Type.Value = "2";
                    T_ReferenceOdds.Insert();
                }
            }
            if (PlayTypeID == "7204")
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    T_ReferenceOdds.Content.Value = Get7204(RecordResults[j]);
                    T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][BQC[int.Parse(RecordResults[j]) - 1]].ToString(), 0).ToString("F2");
                    T_ReferenceOdds.Status.Value = 0;
                    T_ReferenceOdds.Type.Value = "4";
                    T_ReferenceOdds.Insert();
                }
            }
            if (PlayTypeID == "7206")
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    string TypePlay = RecordResults[j].Substring(0, 1);
                    int Index = Shove._Convert.StrToInt(RecordResults[j].Substring(1, 2), 0);

                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    if (TypePlay == "1")
                    {
                        T_ReferenceOdds.Type.Value = "1";
                        T_ReferenceOdds.Content.Value = Get7201((Index + 1).ToString());
                        T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][RQSPF[Index]].ToString(), 0).ToString("F2");
                    }
                    if (TypePlay == "2")
                    {
                        T_ReferenceOdds.Type.Value = "2";
                        T_ReferenceOdds.Content.Value = Get7203((Index + 1).ToString());
                        T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][ZJQS[Index]].ToString(), 0).ToString("F2");
                    }
                    if (TypePlay == "3")
                    {
                        T_ReferenceOdds.Type.Value = "3";
                        T_ReferenceOdds.Content.Value = Get7202((Index + 1).ToString());
                        T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][ZQBF[Index]].ToString(), 0).ToString("F2");
                    }
                    if (TypePlay == "4")
                    {
                        T_ReferenceOdds.Type.Value = "4";
                        T_ReferenceOdds.Content.Value = Get7204((Index + 1).ToString());
                        T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][BQC[Index]].ToString(), 0).ToString("F2");
                    }
                    if (TypePlay == "5")
                    {
                        T_ReferenceOdds.Type.Value = "5";
                        T_ReferenceOdds.Content.Value = Get7207((Index + 1).ToString());
                        T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][SPF[Index]].ToString(), 0).ToString("F2");
                    }
                    T_ReferenceOdds.Status.Value = 0;

                    T_ReferenceOdds.Insert();
                }
            }

            if (PlayTypeID == "7207")
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    T_ReferenceOdds.Content.Value = Get7207(RecordResults[j]);
                    T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][SPF[int.Parse(RecordResults[j]) - 1]].ToString(), 0).ToString("F2");
                    T_ReferenceOdds.Status.Value = 0;
                    T_ReferenceOdds.Type.Value = "5";
                    T_ReferenceOdds.Insert();
                }
            }
        }
    }
    protected static void MatchBasket(string[] ticket, long SchemeID, string PlayTypeID, string LetBile, string Type, string PassType)
    {
        DAL.Tables.T_SchemesContent T_SchemesContent = new DAL.Tables.T_SchemesContent();
        DAL.Tables.T_PassRateBasket T_PassRate = new DAL.Tables.T_PassRateBasket();
        DAL.Tables.T_SingleRateBasket T_SingleRate = new DAL.Tables.T_SingleRateBasket();

        DAL.Tables.T_ReferenceOdds T_ReferenceOdds = new DAL.Tables.T_ReferenceOdds();
        for (int i = 0; i < ticket.Length; i++)
        {
            string MatchID = ticket[i].Substring(0, ticket[i].IndexOf('('));
            string[] RecordResults = ticket[i].Substring(ticket[i].IndexOf('('), ticket[i].Length - ticket[i].IndexOf('(')).Replace(")", "").Replace("(", "").Split(',');
            DataTable DT;
            if (Type == "1")
            {
                DT = T_PassRate.Open("*", "MatchID=" + MatchID + "", "");
            }
            else
            {
                DT = T_SingleRate.Open("*", "MatchID=" + MatchID + "", "");
            }

            if (DT == null)
            {
                return;
            }
            if (DT.Rows.Count <= 0)
            {
                continue;
            }
            T_SchemesContent.SchemeID.Value = SchemeID;
            T_SchemesContent.PlayType.Value = PlayTypeID;
            T_SchemesContent.Game.Value = DT.Rows[0]["Game"].ToString();
            T_SchemesContent.MatchNumber.Value = DT.Rows[0]["MatchNumber"].ToString();
            T_SchemesContent.MainTeam.Value = DT.Rows[0]["MainTeam"].ToString();
            T_SchemesContent.GuestTeam.Value = DT.Rows[0]["GuestTeam"].ToString();
            T_SchemesContent.StopSellingTime.Value = DT.Rows[0]["StopSellTime"].ToString();
            T_SchemesContent.PassType.Value = PassType;
            T_SchemesContent.LetBile.Value = LetBile;
            T_SchemesContent.MatchID.Value = MatchID;
            long SchemesContentID = T_SchemesContent.Insert();

            if (PlayTypeID == "7301")
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    T_ReferenceOdds.Content.Value = Get7301(RecordResults[j]);
                    T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][SF[int.Parse(RecordResults[j]) - 1]].ToString(), 0).ToString("F2");
                    T_ReferenceOdds.Status.Value = 0;
                    T_ReferenceOdds.Type.Value = "11";
                    T_ReferenceOdds.Insert();
                }
            }
            if (PlayTypeID == "7302")
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    T_ReferenceOdds.Content.Value = Get7302(RecordResults[j]);
                    T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][RFSF[int.Parse(RecordResults[j]) - 1]].ToString(), 0).ToString("F2");
                    T_ReferenceOdds.Status.Value = 0;
                    T_ReferenceOdds.Type.Value = "12";
                    T_ReferenceOdds.Insert();
                }
            }
            if (PlayTypeID == "7303")
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    T_ReferenceOdds.Content.Value = Get7303(RecordResults[j]);
                    T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][SFC[int.Parse(RecordResults[j]) - 1]].ToString(), 0).ToString("F2");
                    T_ReferenceOdds.Status.Value = 0;
                    T_ReferenceOdds.Type.Value = "14";
                    T_ReferenceOdds.Insert();
                }
            }
            if (PlayTypeID == "7304")
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    T_ReferenceOdds.Content.Value = Get7304(RecordResults[j]);
                    T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][DX[int.Parse(RecordResults[j]) - 1]].ToString(), 0).ToString("F2");
                    T_ReferenceOdds.Status.Value = 0;
                    T_ReferenceOdds.Type.Value = "13";
                    T_ReferenceOdds.Insert();
                }
            }
            if (PlayTypeID == "7306")
            {
                for (int j = 0; j < RecordResults.Length; j++)
                {
                    string TypePlay = RecordResults[j].Substring(0, 1);
                    int Index = Shove._Convert.StrToInt(RecordResults[j].Substring(1, 2), 0);

                    T_ReferenceOdds.SchemesContentID.Value = SchemesContentID;
                    if (TypePlay == "1")
                    {
                        T_ReferenceOdds.Type.Value = "11";
                        T_ReferenceOdds.Content.Value = Get7301((Index + 1).ToString());
                        T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][SF[Index]].ToString(), 0).ToString("F2");
                    }
                    if (TypePlay == "2")
                    {
                        T_ReferenceOdds.Type.Value = "12";
                        T_ReferenceOdds.Content.Value = Get7302((Index + 1).ToString());
                        T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][RFSF[Index]].ToString(), 0).ToString("F2");
                    }
                    if (TypePlay == "3")
                    {
                        T_ReferenceOdds.Type.Value = "13";
                        T_ReferenceOdds.Content.Value = Get7304((Index + 1).ToString());
                        T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][DX[Index]].ToString(), 0).ToString("F2");

                    }
                    if (TypePlay == "4")
                    {
                        T_ReferenceOdds.Type.Value = "14";
                        T_ReferenceOdds.Content.Value = GetH7303((Index + 1).ToString());
                        T_ReferenceOdds.Bonus.Value = Shove._Convert.StrToDouble(DT.Rows[0][SFC_HH[Index]].ToString(), 0).ToString("F2");
                    }

                    T_ReferenceOdds.Status.Value = 0;

                    T_ReferenceOdds.Insert();
                }
            }
        }
    }
    #endregion

    #region 还原充值前数据
    /// <summary>
    /// 充值成功后，绑定数据
    /// </summary>
    /// <param name="BuyID"></param>
    public static string BindDataForAliBuy(long BuyID)
    {
        string script = "";
        DataTable dt = new DAL.Tables.T_AlipayBuyTemp().Open("", "ID=" + BuyID.ToString(), "");

        if (dt == null || dt.Rows.Count == 0)
        {
            return "";
        }

        DataRow dr = dt.Rows[0];

        string HidIsuseID = dr["IsuseID"].ToString();
        string playType = dr["PlayTypeID"].ToString();
        bool IsChase = Shove._Convert.StrToBool(dr["IsChase"].ToString(), false);
        bool IsCoBuy = Shove._Convert.StrToBool(dr["IsCoBuy"].ToString(), false);
        string tb_Share = dr["Share"].ToString();
        string tb_BuyShare = dr["BuyShare"].ToString();
        string tb_AssureShare = dr["AssureShare"].ToString();
        string tb_OpenUserList = dr["OpenUsers"].ToString();
        string tb_Title = dr["Title"].ToString();
        string tb_Description = dr["Description"].ToString();
        string tbAutoStopAtWinMoney = dr["StopwhenwinMoney"].ToString();
        string tbSecrecyLevel = dr["SecrecyLevel"].ToString();
        string tb_LotteryNumber = dr["LotteryNumber"].ToString();
        string tb_hide_SumMoney = dr["SumMoney"].ToString();
        string tb_hide_AssureMoney = dr["AssureMoney"].ToString();
        string HidLotteryID = dr["LotteryID"].ToString();
        string tb_Multiple = dr["Multiple"].ToString();
        string AdditionasXml = dr["AdditionasXml"].ToString();
        int schemeBonusScale = Shove._Convert.StrToInt(dr["schemeBonusScale"].ToString(), 0);
        if (tb_Multiple == "")
        {
            tb_Multiple = "1";
        }

        double SumMoney = 0;
        int Share = 0;
        int BuyShare = 0;
        double AssureMoney = 0;
        int Multiple = 0;
        short SecrecyLevel = 0;
        int PlayTypeID = 0;
        int LotteryID = 0;
        long IsuseID = 0;
        double AutoStopAtWinMoney = 0;

        try
        {
            SumMoney = double.Parse(tb_hide_SumMoney);
            Share = int.Parse(tb_Share);
            BuyShare = int.Parse(tb_BuyShare);
            AssureMoney = double.Parse(tb_hide_AssureMoney);
            Multiple = int.Parse(tb_Multiple);
            SecrecyLevel = short.Parse(tbSecrecyLevel);
            PlayTypeID = int.Parse(playType);
            LotteryID = int.Parse(HidLotteryID);
            IsuseID = long.Parse(HidIsuseID);
            AutoStopAtWinMoney = double.Parse(tbAutoStopAtWinMoney);
        }
        catch { }

        if ((BuyShare == Share) && (AssureMoney == 0))
        {
            Share = 1;
            BuyShare = 1;
        }

        double BuyMoney = BuyShare * (SumMoney / Share) + AssureMoney;

        if (IsChase)
        {
            BuyMoney = double.Parse(tb_hide_SumMoney);
        }

        string LotteryNumber = tb_LotteryNumber;

        if (LotteryNumber[LotteryNumber.Length - 1] == '\n')
        {
            LotteryNumber = LotteryNumber.Substring(0, LotteryNumber.Length - 1);
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<script type='text/javascript' defer='defer'>");

        LotteryNumber = LotteryNumber.Replace("\r", "");
        int LotteryNum = 0;
        int tmpLotteryNum = 0;
        string Number = "";
        foreach (string lotteryNumber in LotteryNumber.Split('\n'))
        {
            tmpLotteryNum = new SLS.Lottery()[LotteryID].ToSingle(lotteryNumber, ref Number, PlayTypeID).Length;
            LotteryNum += tmpLotteryNum;
        }


        sb.Append("$('#buybs').val('").Append(Multiple).Append("');");
        string[] lns = LotteryNumber.Split(';');
        if (lns != null && lns.Length >= 3)
        {
            string ln1 = lns[1];
            string[] matchID = matcheResult(ln1, @"(?<address>(\d)+?)[\(]");

            string[] match = matcheResult(ln1, @"([\(])(?<address>.+?)[\)]");
            if (match == null || matchID == null || matchID.Length != match.Length || match.Length < 1)
            {
                return "";
            }
            string matchIDStr = "";
            for (int i = 0; i < match.Length; i++)
            {
                matchIDStr += matchID[i] + ",";
                foreach (string s in match[i].Split(','))
                {
                    sb.Append(getClickStr(matchID[i], Shove._Convert.StrToInt(s, -1), PlayTypeID));
                }

            }
            matchIDStr = matchIDStr.Trim(',');
            string sql = getSQLSTR(PlayTypeID, false);
            DataTable dtMatch = Shove.Database.MSSQL.Select(sql + " and MatchID in(" + matchIDStr + ") and DATEADD(minute, (select SystemEndAheadMinute from T_PlayTypes where id = " + PlayTypeID + ") * -1, StopSellTime) > GETDATE() order by CHARINDEX(','+ltrim(MatchID)+',','," + matchIDStr + ",')");
            if (dtMatch == null)
            {
                return "";
            }
            if (dtMatch.Rows.Count != matchID.Length)
            {
                return "-100";
            }
            #region 胆拖
            int D_Diff = 0;
            if (lns.Length == 4)
            {
                string[] num = matcheResult(ln1, @"([\[])(?<address>.+?)[\]]");

                if (num == null)
                {
                    return "";
                }
                int len = num.Length;
                if (len == 0)
                {
                    return "";
                }
                else if (len == 2)
                {
                    string[] DMatchID = matcheResult(num[0], @"(?<address>(\d)+?)[\(]");
                    if (DMatchID == null || DMatchID.Length < 1)
                    {
                        return "";
                    }
                    int DNumber1 = DMatchID.Length;
                    int DNumber2 = Shove._Convert.StrToInt(lns[3].Replace("[", "").Replace("]", ""), 0);
                    foreach (string s in DMatchID)
                    {
                        sb.Append("$('#sel" + s + " td:last input').prop('checked',false).click();");
                    }
                    D_Diff = DNumber1 - DNumber2;
                    sb.Append("$('#DIndex').val(" + D_Diff + ");");
                }

            }
            #endregion


            string ln2 = lns[2];
            if (ln2.IndexOf("A0") < 0)
            {

                if (ln2.Contains("AA") || ln2.Contains("AB") || ln2.Contains("AE") || ln2.Contains("AJ") || ln2.Contains("AQ") || ln2.Contains("BA") || ln2.Contains("BG"))
                {

                    sb.Append("$('#gTab li:eq(0)').click();");
                }
                else
                {
                    sb.Append("$('#gTab li:eq(1)').click();");
                }
                Dictionary<string, string> cuan = new Dictionary<string, string>();
                cuan.Add("AA", "r2c1");
                cuan.Add("AB", "r3c1");
                cuan.Add("AC", "r3c3");
                cuan.Add("AD", "r3c4");
                cuan.Add("AE", "r4c1");
                cuan.Add("AF", "r4c4");
                cuan.Add("AG", "r4c5");
                cuan.Add("AH", "r4c6");
                cuan.Add("AI", "r4c11");
                cuan.Add("AJ", "r5c1");
                cuan.Add("AK", "r5c5");
                cuan.Add("AL", "r5c6");
                cuan.Add("AM", "r5c10");
                cuan.Add("AN", "r5c16");
                cuan.Add("AO", "r5c20");
                cuan.Add("AP", "r5c26");
                cuan.Add("AQ", "r6c1");
                cuan.Add("AR", "r6c6");
                cuan.Add("AS", "r6c7");
                cuan.Add("AT", "r6c15");
                cuan.Add("AU", "r6c20");
                cuan.Add("AV", "r6c22");
                cuan.Add("AW", "r6c35");
                cuan.Add("AX", "r6c42");
                cuan.Add("AY", "r6c50");
                cuan.Add("AZ", "r6c57");
                cuan.Add("BA", "r7c1");
                cuan.Add("BB", "r7c7");
                cuan.Add("BC", "r7c8");
                cuan.Add("BD", "r7c21");
                cuan.Add("BE", "r7c35");
                cuan.Add("BF", "r7c120");
                cuan.Add("BG", "r8c1");
                cuan.Add("BH", "r8c8");
                cuan.Add("BI", "r8c9");
                cuan.Add("BJ", "r8c28");
                cuan.Add("BK", "r8c56");
                cuan.Add("BL", "r8c70");
                cuan.Add("BM", "r8c247");
                cuan.Add("A0", "单关");
                string[] cs = ln2.Replace("[", "").Replace("]", "").Split(',');
                if (cs == null || cs.Length < 1)
                {
                    return "";
                }
                string ggname = "";
                foreach (string s in cs)
                {
                    sb.Append("$('#" + cuan[s.Substring(0, 2)] + "').prop('checked',true);");
                    ggname += cuan[s.Substring(0, 2)] + ",";
                }
                if (D_Diff > 0)
                {
                    sb.Append("$('#ggname').val('" + ggname.Trim(',') + "');");
                }
                if (IsCoBuy)
                {
                    sb.Append("$('#Scheme_join').prop('checked',true);");
                    sb.Append("$('#tb_SchemeBonusScale').val(" + schemeBonusScale + ");");
                    sb.Append("$('#tb_Share').val(" + tb_Share + ");");
                    sb.Append("$('#tb_BuyShare').val(" + tb_BuyShare + ");");
                    sb.Append("$('#tb_AssureShare').val(" + tb_AssureShare + ");");
                    sb.Append("$('#tb_Title').val('" + tb_Title + "');");
                }
                else
                {
                    sb.Append("$('#Scheme_Buy').prop('checked',true);");
                }
                sb.Append("$('#SecrecyLevel").Append(SecrecyLevel.ToString()).Append("').prop('checked',false).click();");
                sb.Append("</script>");

                script = sb.ToString();

            }

        }
        return script;
    }
    /// <summary>
    /// 充值成功后，绑定数据
    /// </summary>
    /// <param name="BuyID"></param>
    public static string BindDataForAliBuyMix(long BuyID)
    {
        string script = "";
        DataTable dt = new DAL.Tables.T_AlipayBuyTemp().Open("", "ID=" + BuyID.ToString(), "");

        if (dt == null || dt.Rows.Count == 0)
        {
            return "";
        }

        DataRow dr = dt.Rows[0];

        string HidIsuseID = dr["IsuseID"].ToString();
        string playType = dr["PlayTypeID"].ToString();
        bool IsChase = Shove._Convert.StrToBool(dr["IsChase"].ToString(), false);
        bool IsCoBuy = Shove._Convert.StrToBool(dr["IsCoBuy"].ToString(), false);
        string tb_Share = dr["Share"].ToString();
        string tb_BuyShare = dr["BuyShare"].ToString();
        string tb_AssureShare = dr["AssureShare"].ToString();
        string tb_OpenUserList = dr["OpenUsers"].ToString();
        string tb_Title = dr["Title"].ToString();
        string tb_Description = dr["Description"].ToString();
        string tbAutoStopAtWinMoney = dr["StopwhenwinMoney"].ToString();
        string tbSecrecyLevel = dr["SecrecyLevel"].ToString();
        string tb_LotteryNumber = dr["LotteryNumber"].ToString();
        string tb_hide_SumMoney = dr["SumMoney"].ToString();
        string tb_hide_AssureMoney = dr["AssureMoney"].ToString();
        string HidLotteryID = dr["LotteryID"].ToString();
        string tb_Multiple = dr["Multiple"].ToString();
        string AdditionasXml = dr["AdditionasXml"].ToString();
        int schemeBonusScale = Shove._Convert.StrToInt(dr["schemeBonusScale"].ToString(), 0);
        if (tb_Multiple == "")
        {
            tb_Multiple = "1";
        }

        double SumMoney = 0;
        int Share = 0;
        int BuyShare = 0;
        double AssureMoney = 0;
        int Multiple = 0;
        short SecrecyLevel = 0;
        int PlayTypeID = 0;
        int LotteryID = 0;
        long IsuseID = 0;
        double AutoStopAtWinMoney = 0;

        try
        {
            SumMoney = double.Parse(tb_hide_SumMoney);
            Share = int.Parse(tb_Share);
            BuyShare = int.Parse(tb_BuyShare);
            AssureMoney = double.Parse(tb_hide_AssureMoney);
            Multiple = int.Parse(tb_Multiple);
            SecrecyLevel = short.Parse(tbSecrecyLevel);
            PlayTypeID = int.Parse(playType);
            LotteryID = int.Parse(HidLotteryID);
            IsuseID = long.Parse(HidIsuseID);
            AutoStopAtWinMoney = double.Parse(tbAutoStopAtWinMoney);
        }
        catch { }

        if ((BuyShare == Share) && (AssureMoney == 0))
        {
            Share = 1;
            BuyShare = 1;
        }

        double BuyMoney = BuyShare * (SumMoney / Share) + AssureMoney;

        if (IsChase)
        {
            BuyMoney = double.Parse(tb_hide_SumMoney);
        }

        string LotteryNumber = tb_LotteryNumber;

        if (LotteryNumber[LotteryNumber.Length - 1] == '\n')
        {
            LotteryNumber = LotteryNumber.Substring(0, LotteryNumber.Length - 1);
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<script type='text/javascript' defer='defer'>");

        LotteryNumber = LotteryNumber.Replace("\r", "");
        int LotteryNum = 0;
        int tmpLotteryNum = 0;
        string Number = "";
        foreach (string lotteryNumber in LotteryNumber.Split('\n'))
        {
            tmpLotteryNum = new SLS.Lottery()[LotteryID].ToSingle(lotteryNumber, ref Number, PlayTypeID).Length;
            LotteryNum += tmpLotteryNum;
        }

        sb.Append("$('#buybs').val('").Append(Multiple).Append("');");
        string[] lns = LotteryNumber.Split(';');
        if (lns != null && lns.Length >= 3)
        {
            string ln1 = lns[1];
            string[] matchID = matcheResult(ln1, @"(?<address>(\d)+?)[\(]");

            string[] match = matcheResult(ln1, @"([\(])(?<address>.+?)[\)]");
            if (match == null || matchID == null || matchID.Length != match.Length || match.Length < 1)
            {
                return "";
            }
            string matchIDStr = "";
            for (int i = 0; i < match.Length; i++)
            {
                matchIDStr += matchID[i] + ",";
                foreach (string s in match[i].Split(','))
                {
                    sb.Append(getClickStr(matchID[i], Shove._Convert.StrToInt(s, -1), PlayTypeID));
                }
                sb.Append(getBetStr(matchID[i], match[i], PlayTypeID));
            }

            matchIDStr = matchIDStr.Trim(',');
            string sql = getSQLSTR(PlayTypeID, false);
            DataTable dtMatch = Shove.Database.MSSQL.Select(sql + " and MatchID in(" + matchIDStr + ") and DATEADD(minute, (select max(SystemEndAheadMinute) as SystemEndAheadMinute from T_playtypes where LotteryID=" + LotteryID + " group by  LotteryID) * -1, StopSellTime) > GETDATE() order by CHARINDEX(','+ltrim(MatchID)+',','," + matchIDStr + ",')");
            if (dtMatch == null)
            {
                return "";
            }
            if (dtMatch.Rows.Count != matchID.Length)
            {
                return "-100";
            }
            string MatchNumber = "";
            string No1 = "";
            string No2 = "";
            foreach (DataRow item in dtMatch.Rows)
            {
                MatchNumber += item["MatchNumber"].ToString() + ",";
                No1 += item["MainTeam"].ToString() + ",";
                No2 += item["GuestTeam"].ToString() + ",";

            }
            MatchNumber = MatchNumber.Trim(',');
            No1 = No1.Trim(',');
            No2 = No2.Trim(',');
            sb.Append("intilcs('" + matchIDStr + "','" + MatchNumber + "','" + No1 + "','" + No2 + "');");
            string ln2 = lns[2];
            if (ln2.IndexOf("A0") < 0)
            {

                if (ln2.Contains("AA") || ln2.Contains("AB") || ln2.Contains("AE") || ln2.Contains("AJ") || ln2.Contains("AQ") || ln2.Contains("BA") || ln2.Contains("BG"))
                {

                    sb.Append("$('#gTab li:eq(0)').click();");
                }
                else
                {
                    sb.Append("$('#gTab li:eq(1)').click();");
                }
                Dictionary<string, string> cuan = new Dictionary<string, string>();
                cuan.Add("AA", "r2c1");
                cuan.Add("AB", "r3c1");
                cuan.Add("AC", "r3c3");
                cuan.Add("AD", "r3c4");
                cuan.Add("AE", "r4c1");
                cuan.Add("AF", "r4c4");
                cuan.Add("AG", "r4c5");
                cuan.Add("AH", "r4c6");
                cuan.Add("AI", "r4c11");
                cuan.Add("AJ", "r5c1");
                cuan.Add("AK", "r5c5");
                cuan.Add("AL", "r5c6");
                cuan.Add("AM", "r5c10");
                cuan.Add("AN", "r5c16");
                cuan.Add("AO", "r5c20");
                cuan.Add("AP", "r5c26");
                cuan.Add("AQ", "r6c1");
                cuan.Add("AR", "r6c6");
                cuan.Add("AS", "r6c7");
                cuan.Add("AT", "r6c15");
                cuan.Add("AU", "r6c20");
                cuan.Add("AV", "r6c22");
                cuan.Add("AW", "r6c35");
                cuan.Add("AX", "r6c42");
                cuan.Add("AY", "r6c50");
                cuan.Add("AZ", "r6c57");
                cuan.Add("BA", "r7c1");
                cuan.Add("BB", "r7c7");
                cuan.Add("BC", "r7c8");
                cuan.Add("BD", "r7c21");
                cuan.Add("BE", "r7c35");
                cuan.Add("BF", "r7c120");
                cuan.Add("BG", "r8c1");
                cuan.Add("BH", "r8c8");
                cuan.Add("BI", "r8c9");
                cuan.Add("BJ", "r8c28");
                cuan.Add("BK", "r8c56");
                cuan.Add("BL", "r8c70");
                cuan.Add("BM", "r8c247");
                cuan.Add("A0", "单关");
                string[] cs = ln2.Replace("[", "").Replace("]", "").Split(',');
                if (cs == null || cs.Length < 1)
                {
                    return "";
                }
                string ggname = "";
                foreach (string s in cs)
                {
                    sb.Append("$('#" + cuan[s.Substring(0, 2)] + "').click();");
                    ggname += cuan[s.Substring(0, 2)] + ",";
                }
                if (IsCoBuy)
                {
                    sb.Append("$('#Scheme_join').click();");
                    sb.Append("$('#trShowJion').show();");
                    sb.Append("$('#tb_SchemeBonusScale').val(" + schemeBonusScale + ");");
                    sb.Append("$('#tb_Share').val(" + tb_Share + ");");
                    sb.Append("$('#tb_BuyShare').val(" + tb_BuyShare + ");");
                    sb.Append("$('#tb_AssureShare').val(" + tb_AssureShare + ");");
                    sb.Append("$('#tb_Title').val('" + tb_Title + "');");
                }
                else
                {
                    sb.Append("$('#Scheme_Buy').click();");
                }
                sb.Append("$('#SecrecyLevel").Append(SecrecyLevel.ToString()).Append("').click();");
                sb.Append("getBetInfo();");
                sb.Append("intilBuy();");
                sb.Append("</script>");

                script = sb.ToString();

            }

        }
        return script;
    }
    public static string getMatchType(int index)
    {
        string str = "";
        switch (index)
        {
            case 1:
                str = "spf";
                break;
            case 2:
                str = "zjqs";
                break;
            case 3:
                str = "zqbf";
                break;
            case 4:
                str = "bqc";
                break;
            case 5:
                str = "sf";
                break;
        }
        return str;
    }
    public static string getBetStr(string matchID, string info, int playTypeID)
    {
        string str = "";
        string[] match = info.Split(',');
        if (match == null || match.Length < 1)
        {
            return "";
        }
        foreach (string s in match)
        {
            int ss = Shove._Convert.StrToInt(s.Substring(0, 1), -1);
            int index = Shove._Convert.StrToInt(s.Substring(1), -1);
            str += "addMatchCS(false," + index + ",'" + getMatchType(ss) + "','" + matchID + "');";
        }
        return str;
    }
    /// <summary>
    /// 充值成功后，绑定数据
    /// </summary>
    /// <param name="BuyID"></param>
    public static string BindDataForAliBuyDG(long BuyID)
    {
        string script = "";
        DataTable dt = new DAL.Tables.T_AlipayBuyTemp().Open("", "ID=" + BuyID.ToString(), "");

        if (dt == null || dt.Rows.Count == 0)
        {
            return "";
        }

        DataRow dr = dt.Rows[0];

        string HidIsuseID = dr["IsuseID"].ToString();
        string playType = dr["PlayTypeID"].ToString();
        bool IsChase = Shove._Convert.StrToBool(dr["IsChase"].ToString(), false);
        bool IsCoBuy = Shove._Convert.StrToBool(dr["IsCoBuy"].ToString(), false);
        string tb_Share = dr["Share"].ToString();
        string tb_BuyShare = dr["BuyShare"].ToString();
        string tb_AssureShare = dr["AssureShare"].ToString();
        string tb_OpenUserList = dr["OpenUsers"].ToString();
        string tb_Title = dr["Title"].ToString();
        string tb_Description = dr["Description"].ToString();
        string tbAutoStopAtWinMoney = dr["StopwhenwinMoney"].ToString();
        string tbSecrecyLevel = dr["SecrecyLevel"].ToString();
        string tb_LotteryNumber = dr["LotteryNumber"].ToString();
        string tb_hide_SumMoney = dr["SumMoney"].ToString();
        string tb_hide_AssureMoney = dr["AssureMoney"].ToString();
        string HidLotteryID = dr["LotteryID"].ToString();
        string tb_Multiple = dr["Multiple"].ToString();
        string AdditionasXml = dr["AdditionasXml"].ToString();
        int schemeBonusScale = Shove._Convert.StrToInt(dr["schemeBonusScale"].ToString(), 0);
        if (tb_Multiple == "")
        {
            tb_Multiple = "1";
        }

        double SumMoney = 0;
        int Share = 0;
        int BuyShare = 0;
        double AssureMoney = 0;
        int Multiple = 0;
        short SecrecyLevel = 0;
        int PlayTypeID = 0;
        int LotteryID = 0;
        long IsuseID = 0;
        double AutoStopAtWinMoney = 0;

        try
        {
            SumMoney = double.Parse(tb_hide_SumMoney);
            Share = int.Parse(tb_Share);
            BuyShare = int.Parse(tb_BuyShare);
            AssureMoney = double.Parse(tb_hide_AssureMoney);
            Multiple = int.Parse(tb_Multiple);
            SecrecyLevel = short.Parse(tbSecrecyLevel);
            PlayTypeID = int.Parse(playType);
            LotteryID = int.Parse(HidLotteryID);
            IsuseID = long.Parse(HidIsuseID);
            AutoStopAtWinMoney = double.Parse(tbAutoStopAtWinMoney);
        }
        catch { }

        if ((BuyShare == Share) && (AssureMoney == 0))
        {
            Share = 1;
            BuyShare = 1;
        }

        double BuyMoney = BuyShare * (SumMoney / Share) + AssureMoney;

        if (IsChase)
        {
            BuyMoney = double.Parse(tb_hide_SumMoney);
        }

        string LotteryNumber = tb_LotteryNumber;

        if (LotteryNumber[LotteryNumber.Length - 1] == '\n')
        {
            LotteryNumber = LotteryNumber.Substring(0, LotteryNumber.Length - 1);
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<script type='text/javascript' defer='defer'>");


        LotteryNumber = LotteryNumber.Replace("\r", "");
        int LotteryNum = 0;
        int tmpLotteryNum = 0;
        string Number = "";
        foreach (string lotteryNumber in LotteryNumber.Split('\n'))
        {
            tmpLotteryNum = new SLS.Lottery()[LotteryID].ToSingle(lotteryNumber, ref Number, PlayTypeID).Length;
            LotteryNum += tmpLotteryNum;
        }


        sb.Append("$('#buybs').val('").Append(Multiple).Append("');");
        string[] lns = LotteryNumber.Split(';');
        if (lns != null && lns.Length >= 3)
        {
            string ln1 = lns[1];
            string[] matchID = matcheResult(ln1, @"(?<address>(\d)+?)[\(]");

            string[] match = matcheResult(ln1, @"([\(])(?<address>.+?)[\)]");
            if (match == null || matchID == null || matchID.Length != match.Length || match.Length < 1)
            {
                return "";
            }
            string matchIDStr = "";
            for (int i = 0; i < match.Length; i++)
            {
                matchIDStr += matchID[i] + ",";
                foreach (string s in match[i].Split(','))
                {
                    sb.Append(getClickStr(matchID[i], Shove._Convert.StrToInt(s, -1), PlayTypeID));
                }

            }
            matchIDStr = matchIDStr.Trim(',');
            string sql = getSQLSTR(PlayTypeID, true);
            DataTable dtMatch = Shove.Database.MSSQL.Select(sql + " and MatchID in(" + matchIDStr + ") and DATEADD(minute, (select SystemEndAheadMinute from T_PlayTypes where id = " + PlayTypeID + ") * -1, StopSellTime) > GETDATE() order by CHARINDEX(','+ltrim(MatchID)+',','," + matchIDStr + ",')");
            if (dtMatch == null)
            {
                return "";
            }
            if (dtMatch.Rows.Count != matchID.Length)
            {
                return "-100";
            }



            string ln2 = lns[2];
            string[] cs = ln2.Replace("[", "").Replace("]", "").Split(',');
            if (cs == null || cs.Length < 1)
            {
                return "";
            }
            if (IsCoBuy)
            {
                sb.Append("$('#Scheme_join').prop('checked',true);");
                sb.Append("$('#tb_SchemeBonusScale').val(" + schemeBonusScale + ");");
                sb.Append("$('#tb_Share').val(" + tb_Share + ");");
                sb.Append("$('#tb_BuyShare').val(" + tb_BuyShare + ");");
                sb.Append("$('#tb_AssureShare').val(" + tb_AssureShare + ");");
                sb.Append("$('#tb_Title').val('" + tb_Title + "');");
            }
            else
            {
                sb.Append("$('#Scheme_Buy').prop('checked',true);");
            }
            sb.Append("$('#SecrecyLevel").Append(SecrecyLevel.ToString()).Append("').prop('checked',false).click();");
            sb.Append("</script>");

            script = sb.ToString();
        }
        return script;
    }
    public static string getMixinfo(int matchValue)
    {
        string str = "";
        if (matchValue > 0)
        {
            int r = matchValue / 100;
            int y = matchValue % 100;
            switch (r)
            {
                case 1:
                    str = y + "_spf";
                    break;
                case 2:
                    str = y + "_zjqs";
                    break;
                case 3:
                    str = y + "_zqbf";
                    break;
                case 4:
                    str = y + "_bqc";
                    break;
                case 5:
                    str = y + "_sf";
                    break;
            }
        }
        return str;
    }
    public static string getSQLSTR(int playTypeID, bool isDG)
    {
        string sql = "";
        switch (playTypeID)
        {
            case 7201:
                if (isDG)
                {
                    sql = "SELECT * FROM [T_singleRate]  where IsHhad = 1 ";
                }
                else
                {
                    sql = "SELECT * FROM [T_PassRate]  where IsHhad = 1 ";
                }
                break;
            case 7207:
                if (isDG)
                {
                    sql = "SELECT * FROM [T_singleRate]  where IsSPF = 1 ";
                }
                else
                {
                    sql = "SELECT * FROM [T_PassRate]  where IsSPF = 1 ";
                }
                break;
            case 7202:
                if (isDG)
                {
                    sql = "SELECT * FROM [T_singleRate]  where IsCrs = 1 ";
                }
                else
                {
                    sql = "SELECT * FROM [T_PassRate]  where IsCrs = 1 ";
                }
                break;
            case 7203:
                if (isDG)
                {
                    sql = "SELECT * FROM [T_singleRate]  where IsTtg = 1 ";
                }
                else
                {
                    sql = "SELECT * FROM [T_PassRate]  where IsTtg = 1 ";
                }
                break;
            case 7204:
                if (isDG)
                {
                    sql = "SELECT * FROM [T_singleRate]  where IsHafu = 1 ";
                }
                else
                {
                    sql = "SELECT * FROM [T_PassRate]  where IsHafu = 1 ";
                }
                break;
            case 7206:
                sql = "SELECT * FROM [T_PassRate]  where IsSPF = 1 and IsHhad = 1 and IsCrs = 1 and IsTtg = 1 and IsHafu = 1";

                break;
            case 7301:
                if (isDG)
                {
                    sql = "SELECT * FROM [T_SingleRateBasket]  where IsMnl = 1 ";
                }
                else
                {
                    sql = "SELECT * FROM [T_PassRateBasket]  where IsMnl = 1 ";
                }
                break;
            case 7302:
                if (isDG)
                {
                    sql = "SELECT * FROM [T_SingleRateBasket]  where IsHdc = 1 ";
                }
                else
                {
                    sql = "SELECT * FROM [T_PassRateBasket]  where IsHdc = 1 ";
                }
                break;
            case 7303:
                if (isDG)
                {
                    sql = "SELECT * FROM [T_SingleRateBasket]  where IsWnm = 1 ";
                }
                else
                {
                    sql = "SELECT * FROM [T_PassRateBasket]  where IsWnm = 1 ";
                }
                break;
            case 7304:
                if (isDG)
                {
                    sql = "SELECT * FROM [T_SingleRateBasket]  where IsHilo = 1 ";
                }
                else
                {
                    sql = "SELECT * FROM [T_PassRateBasket]  where IsHilo = 1 ";
                }
                break;
            case 7306:
                sql = "SELECT * FROM [T_PassRateBasket]  where IsMnl = 1 and IsHdc = 1 and IsWnm = 1 and IsHilo = 1";

                break;
        }
        return sql;
    }
    public static string[] matcheResult(string content, string matcheStr)
    {
        Regex reg = new Regex(matcheStr, RegexOptions.IgnoreCase);
        // 搜索匹配的字符串            
        MatchCollection matches = reg.Matches(content);
        int i = 0;
        string[] mat = new string[matches.Count];
        // 取得匹配项列表            
        foreach (Match match in matches)
            mat[i++] = match.Groups["address"].Value;

        return mat;
    }
    public static string getClickStr(string matchID, int s, int playTypeID)
    {
        string ss = "";
        if (s >= 0)
        {
            switch (playTypeID)
            {
                case 7201:
                    ss = "$('#vs" + matchID + " td:eq(" + (s + 9) + ")').click();";
                    break;
                case 7207:
                    ss = "$('#vs" + matchID + " td:eq(" + (s + 8) + ")').click();";
                    break;
                case 7202:
                    if (s < 13)
                    {
                        ss = "$('#pltr_" + matchID + " .sp_box .sp_table .sp_3 td:eq(" + s + ")').click();";
                    }
                    else if (s == 13)
                    {
                        ss = "$('#pltr_" + matchID + " .sp_box .sp_table .sp_3 td:eq(0)').click();";
                    }
                    else if (s > 13 && s < 18)
                    {
                        ss = "$('#pltr_" + matchID + " .sp_box .sp_table .sp_1 td:eq(" + (s - 13) + ")').click();";
                    }
                    else if (s == 18)
                    {
                        ss = "$('#pltr_" + matchID + " .sp_box .sp_table .sp_1 td:eq(0)').click();";
                    }
                    else if (s > 18 && s < 31)
                    {
                        ss = "$('#pltr_" + matchID + " .sp_box .sp_table .sp_0 td:eq(" + (s - 18) + ")').click();";
                    }
                    else if (s == 31)
                    {
                        ss = "$('#pltr_" + matchID + " .sp_box .sp_table .sp_0 td:eq(0)').click();";
                    }
                    break;
                case 7203:
                case 7204:
                    ss = "$('#vs" + matchID + " td:eq(" + (s + 5) + ")').click();";
                    break;
                case 7206:
                    ss = "$('.td" + getMixinfo(s) + matchID + "').click();";
                    break;
                case 7301:
                    ss = "$('#vs" + matchID + " td:eq(" + (s + 4) + ")').click();";
                    break;
                case 7302:
                case 7304:
                    ss = "$('#vs" + matchID + " td:eq(" + (s + 5) + ")').click();";
                    break;
                case 7303:
                    if (s % 2 == 0)
                    {
                        ss = "$('#pltr_" + matchID + " td:eq(" + (s / 2) + ")').click();";
                    }
                    else
                    {
                        ss = "$('#vs" + matchID + " td:eq(" + (s / 2 + 6) + ")').click();";
                    }

                    break;
                case 7306:
                    ss = "$('.td" + getMixinfo(s) + matchID + "').click();";
                    break;
            }

        }
        return ss;
    }
    #endregion

    /// <summary>
    /// 获取报错代码在文件中的位置
    /// </summary>
    /// <returns></returns>
    public static string GetLogLineNumber(StackFrame stackFrame, params object[] Params)
    {
        string typeName = "";

        if (Params.Length > 0)
        {
            typeName = ParamterToString(Params[0]);
        }

        string errMsg = "";
        if (Params.Length > 1)
        {
            errMsg = ParamterToString(Params[1]);
        }


        string returnValue = "";

#if !DEBUG
        return (typeName.Equals("") ? "" : "文件名:" + typeName + "，") + "方法名:" + stackFrame.GetMethod().Name + "，ErrorInfo:" + errMsg + "\r\n\t\t\t\t";
#endif

        returnValue = "文件名:" + stackFrame.GetFileName() + "，方法名:" + stackFrame.GetMethod().Name + "，第 " + stackFrame.GetFileLineNumber().ToString() + " 行，第 " + stackFrame.GetFileColumnNumber().ToString() + " 列。\r\n\t\t\t\tErrorInfo:" + errMsg;
        return returnValue;
    }
    public static string ParamterToString(object Param)
    {
        if (Param is DateTime)
        {
            return ((DateTime)Param).ToString("yyyyMMddHHmmss");
        }
        else if (Param is DataTable)
        {
            return Shove._Convert.DataTableToXML((DataTable)Param);
        }
        else if (Param is DataSet)
        {
            return ((DataSet)Param).GetXml();
        }
        else if (Param is string)
        {
            return (string)Param;
        }

        return Param.ToString();
    }

    /// <summary>
    /// 获得当前页面客户端的IP
    /// </summary>
    /// <returns>当前页面客户端的IP</returns>
    public static string GetIP()
    {
        string result = String.Empty;

        result = HttpContext.Current.Request.UserHostAddress;

        if (string.IsNullOrEmpty(result) || result == "::1")
        {
            return "127.0.0.1";
        }

        return result;
    }

    /// <summary>
    /// 根据ip获取登录地区
    /// </summary>
    /// <param name="IP"></param>
    /// <returns></returns>
    public static IPAddressInfo GetAddressInfo(string IP)
    {
        string strIPAddressInfo = PF.Post("http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js&ip=" + IP, "", 120);
        strIPAddressInfo = strIPAddressInfo.Replace("var remote_ip_info = ", "");
        strIPAddressInfo = strIPAddressInfo.Remove(strIPAddressInfo.Length - 1, 1);

        IPAddressInfo ipAddressInfo = null;
        try
        {
            ipAddressInfo = (IPAddressInfo)JsonConvert.DeserializeObject(strIPAddressInfo, typeof(IPAddressInfo));
        }
        catch
        {
            return null;
        }

        return ipAddressInfo;
    }

    /// <summary>
    /// 得到期号说明
    /// </summary>
    /// <param name="LotteryID"></param>
    /// <param name="Prize"></param>
    /// <returns></returns>
    public static string GetExplanation(int LotteryID, string Prize)
    {
        string Explanation = "";
        switch (LotteryID)
        {
            case 72:
                Explanation = "2串1倍投收益高";
                break;
            case 73:
                Explanation = "2串1易中奖";
                break;
            case 63:
                Explanation = "3位数字赢千元";
                break;
            case 64:
                Explanation = "2元赢取10万元";
                break;
            case 13:
                Explanation = "30选7赢百万";
                break;
            case 74:
                Explanation = "猜胜负赢500万";
                break;
            case 75:
                Explanation = "猜胜负赢500万";
                break;
            case 2:
                Explanation = "猜胜负赢500万";
                break;
            case 15:
                Explanation = "猜胜负赢500万";
                break;
            case 28:
                Explanation = "每天120期 10分钟一期";
                break;
            case 70:
            case 62:
            case 68:
            case 78:
                Explanation = "每天78期 10分钟一期";
                break;
            case 61:
            case 87:
                Explanation = "每天84期 10分钟一期";
                break;
            case 29:
                Explanation = "每天23期 30分钟一期";
                break;
            case 69:
                Explanation = "每日 20:30开奖";
                break;
            case 6:
                Explanation = "简单3位赢千元";
                break;
            case 5:
            case 3:
            case 39:
                Explanation = Prize;
                break;
        }
        return Explanation;
    }

    /// <summary>
    /// 获得战绩
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static string GetLevel(string level)
    {
        string strLevel = "";
        string imagePath = "http://" + HttpContext.Current.Request.Url.Authority + "/Images";
        if (level == "0")
        {
            strLevel = "";
        }
        else
        {
            int a = int.Parse(level) / 1000;//27553
            int b = int.Parse(level) % 1000 / 100;
            int c = int.Parse(level) % 100 / 10;
            int d = int.Parse(level) % 10;

            if (a > 0)
            {
                int n = a / 9 + 1;
                for (int i = 0; i < n; i++)
                {
                    if (i >= 4)
                    {
                        break;
                    }
                    int m = (a / 9 > 0) ? 9 : a;
                    strLevel += "<i style=\"display:inline-block; background:url(" + imagePath + "/Level/crown_" + m + ".gif); height:16px; width:16px;\"></i>";

                    a -= 9; //18
                }
            }
            if (b > 0)
            {
                strLevel += "<i style=\"display:inline-block; background:url(" + imagePath + "/Level/cup_" + b + ".gif); height:16px; width:16px;\"></i>";
            }
            if (c > 0)
            {
                strLevel += "<i style=\"display:inline-block; background:url(" + imagePath + "/Level/medal_" + c + ".gif); height:16px; width:16px;\"></i>";
            }
            if (d > 0)
            {
                strLevel += "<i style=\"display:inline-block; background:url(" + imagePath + "/Level/star_" + d + ".gif); height:16px; width:16px;\"></i>";
            }
        }
        return strLevel;
    }

    public static int CPSTGYRegisterSendVerfiyCode(string Content, string To)
    {
        string Betting_SMS_UserPassword = "";
        string Betting_SMS_RegCode = "";
        DataTable dt2 = new DAL.Tables.T_Options().Open("", "[key]='Betting_SMS_RegCode' or [key]='Betting_SMS_UserPassword'", "ID");
        if (dt2 != null && dt2.Rows.Count > 0)
        {
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                if (i == 0)
                {
                    Betting_SMS_UserPassword = dt2.Rows[i]["Value"].ToString();
                }
                if (i == 1)
                {
                    Betting_SMS_RegCode = dt2.Rows[i]["Value"].ToString();
                }
            }
        }
        DataSet ds = Shove.Gateways.SMS.SendSMS(Betting_SMS_RegCode, Betting_SMS_UserPassword, Content, To);
        if (null != ds && ds.Tables.Count > 1)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// 方案显示状态
    /// </summary>
    /// <param name="Share">份数</param>
    /// <param name="BuyedShare">认购份数</param>
    /// <param name="buyed">出票</param>
    /// <param name="quashStatus">状态</param>
    /// <param name="isopened">开奖</param>
    /// <param name="winmoney">中奖金额</param>
    /// <returns></returns>
    public static string SchemeState(int Share, int BuyedShare, bool buyed, int quashStatus, bool isopened, double winmoney)
    {
        string SchemeState = "";
        if (Share > BuyedShare && !buyed)
        {
            SchemeState = "招募中";
        }
        else if (Share <= BuyedShare && !buyed && !isopened)
        {
            SchemeState = "未出票";
        }
        else if (Share <= BuyedShare && buyed && !isopened)
        {
            SchemeState = "已出票";
        }
        else if (Share <= BuyedShare && !buyed && isopened)
        {
            SchemeState = "已流单";
        }
        else if (Share <= BuyedShare && buyed && isopened && winmoney == 0)
        {
            SchemeState = "未中奖";
        }
        else if (Share <= BuyedShare && buyed && isopened && winmoney > 0)
        {
            SchemeState = "已中奖";
        }
        if (quashStatus > 0)
        {
            SchemeState = "已撤单";
        }
        return SchemeState;
    }

    #region 订单锁处理
    /// <summary>
    /// 订单锁处理
    /// </summary>
    /// <param name="UserId">用户ID</param>
    /// <param name="UserName">用户名</param>
    /// <param name="LockState">True:打开锁;False:关闭锁</param>
    /// <param name="errLogMsg">错误信息记录</param>
    /// <returns>True:允许下单;False:禁止下单</returns>
    public static bool OrderLock(string UserId, string UserName, bool LockState, string errLogMsg)
    {
        Log log = new Log("LockInfo");
        try
        {
            if (!Users.OrderLock.ContainsKey(UserId))
            {
                Users.OrderLock.Add(UserId, false);
            }
            if (LockState)
            {
                //开启订单锁
                if (!Users.OrderLock[UserId])
                {
                    Users.OrderLock[UserId] = true;
                    log.Write("订单锁开启,用户：" + UserName);
                    return true;
                }
                else
                {
                    log.Write("已有订单在处理中.(锁定)用户:" + UserName);
                    return false;
                }

            }
            //关闭订单锁
            Users.OrderLock[UserId] = false;
            if (errLogMsg == "")
            {
                log.Write("正常关闭锁.用户:" + UserName);
            }
            else
            {
                log.Write("锁关闭;errorMsg:" + errLogMsg + ";用户：" + UserName);
            }
            return true;
        }
        catch (Exception ex)
        {
            log.Write("ex:" + ex);
            return false;
        }
    }
    #endregion

    #region 获取当前用户有效活动所送彩金
    /// <summary>
    /// 获取当前用户有效活动所送彩金
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static string GetCurrentEffective(decimal amount)
    {
        Users _users = Users.GetCurrentUser(1);
        int userId = Convert.ToInt32(_users.ID);
        return DAL.Functions.F_GetHandsel((long)userId, Convert.ToDouble(amount)).ToString("0.00");
    }
    /// <summary>
    /// 根据用户与金额获取有效活动所送彩金数
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static string GetCurrentEffective(decimal amount, long userId)
    {
        return DAL.Functions.F_GetHandsel(userId, Convert.ToDouble(amount)).ToString("0.00");
    }
    #endregion

    #region 获取用户的冻结彩金
    /// <summary>
    /// 获取用户的冻结彩金
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static double GetFreezonHandsel(int userId)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT HandselForzen FROM dbo.T_Users WHERE ID=@UserID");
        MSSQL.Parameter paras = new MSSQL.Parameter("@UserID", SqlDbType.Int, 4, ParameterDirection.Input, userId);
        DataTable dt = Shove.Database.MSSQL.Select(sb.ToString(), paras);
        if (dt.Rows.Count > 0)
        {
            return Convert.ToDouble(dt.Rows[0][0]);
        }
        return 0d;
    }
    #endregion

    #region 获取不可提款金额
    public static double GetNoCash(int userId)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT NoCash FROM dbo.T_Users WHERE ID=@UserID");
        MSSQL.Parameter paras = new MSSQL.Parameter("@UserID", SqlDbType.Int, 4, ParameterDirection.Input, userId);
        DataTable dt = Shove.Database.MSSQL.Select(sb.ToString(), paras);
        if (dt.Rows.Count > 0)
        {
            return Convert.ToDouble(dt.Rows[0][0]);
        }
        return 0d;

    }
    #endregion

    #region 获取用户的彩金
    /// <summary>
    /// 获取用户的彩金
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static double GetHandsel(int userId)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT HandselAmount FROM dbo.T_Users WHERE ID=@UserID");
        MSSQL.Parameter paras = new MSSQL.Parameter("@UserID", SqlDbType.Int, 4, ParameterDirection.Input, userId);
        DataTable dt = Shove.Database.MSSQL.Select(sb.ToString(), paras);
        if (dt.Rows.Count > 0)
        {
            return Convert.ToDouble(dt.Rows[0][0]);
        }
        return 0d;
    }
    #endregion

    #region 获取用户的可提款彩金
    /// <summary>
    /// 获取用户的可提款彩金
    /// </summary>
    /// <param name="UserID">用户ID</param>
    /// <returns></returns>
    public static double GetUserAllowTakeHandsel(long UserID)
    {
        Users _User = new Users(1)[1, UserID];
        bool isOk = true;//是否可提款
        Sites _Site = new Sites()[1];
        double allowMoney = 0d;//允许提款金额
        double maxHandsel = 0d;//最大提款金额
        //如果是彩金提款，判断用户是否已达到彩金的提取条件
        int handselDrawing = Convert.ToInt32(_Site.SiteOptions["HandselDrawing"].ToString(""));//是否允许提款 0:不允许; 1:允许
        double handselWithdrawalRatio = double.Parse(_Site.SiteOptions["HandselWithdrawalRatio"].ToString(""));//彩金提款比例
        double handselInCome = 0d;//用户所得彩金和
        double handselConsumption = 0d;//用户消费彩金总和

        DAL.Tables.T_UserDetails db = new DAL.Tables.T_UserDetails();
        DataTable userDetailsDt = db.Open("SUM(HandselAmount) AS HandselAmount", "OperatorType=1 AND HandselAmount>0 AND UserID=" + UserID, "");
        if (userDetailsDt.Rows.Count > 0)
        {
            if (userDetailsDt.Rows[0]["HandselAmount"].ToString() != "")
            {
                handselInCome = Convert.ToDouble(userDetailsDt.Rows[0]["HandselAmount"]);
            }
        }
        DataTable userDetailsConsumptionDt = db.Open("SUM(HandselAmount) AS HandselAmount", "OperatorType=101 AND HandselAmount>0 AND UserID=" + UserID + " AND SchemeID IN (SELECT ID FROM dbo.T_Schemes WHERE ID IN (SELECT SchemeID FROM dbo.T_UserDetails WHERE OperatorType=101 AND HandselAmount>0 AND UserID=" + UserID + ") AND QuashStatus=0)", "");
        if (userDetailsConsumptionDt.Rows.Count > 0)
        {
            if (userDetailsConsumptionDt.Rows[0]["HandselAmount"].ToString() == "")
            {
                handselConsumption = 0.00d;
            }
            else
            {
                handselConsumption = Convert.ToDouble(userDetailsConsumptionDt.Rows[0]["HandselAmount"]);
            }
        }
        //消费彩金额度未达到后台限制的比例，不允许提取
        if (handselConsumption < (handselInCome * handselWithdrawalRatio))
        {
            //提取失败
            allowMoney = 0d;
            isOk = false;
        }
        //计算允许提取的最大额度
        if (isOk)
        {
            maxHandsel = handselInCome * (1.00d - handselWithdrawalRatio);
            double tempMoney = _User.HandselAmount - maxHandsel;
            if (tempMoney <= 0)
            {
                allowMoney = _User.HandselAmount;
            }
            else
            {
                allowMoney = _User.HandselAmount - maxHandsel;
            }
        }
        return allowMoney;
    }
    #endregion

    #region 获取后台配置的二维码图片
    /// <summary>
    /// 获取后台配置的二维码图片
    /// </summary>
    /// <param name="typeName">默认返回微信的二维码(WeiXin,手机站点,APP)</param>
    public static string GetQRCodeImage(string typeName = "WeiXin")
    {
        Shove._IO.IniFile ini = new Shove._IO.IniFile(System.AppDomain.CurrentDomain.BaseDirectory + "Admin/QRCode/Config.ini");

        string img = ini.Read(typeName, "QRCodeImg");

        return string.Format("<img src=\"{0}\"/>", img);
    }
    #endregion

    #region 获取用户的不可取金额数
    /// <summary>
    /// 获取用户的不可取金额数
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static double GetUserNoCash(int userID)
    {
        DataTable dt = new DAL.Tables.T_Users().Open("NoCash", "ID=" + userID.ToString(), "");
        if (dt != null && dt.Rows.Count > 0)
        {
            return Convert.ToDouble(dt.Rows[0][0]);
        }
        return 0d;
    }
    #endregion

    #region 数组去重
    /// <summary>
    /// 数组去重
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static string[] ArrayRemoveRe(string[] values)
    {
        List<string> list = new List<string>();
        for (int i = 0; i < values.Length; i++)//遍历数组成员
        {
            if (list.IndexOf(values[i].ToLower()) == -1)//对每个成员做一次新数组查询如果没有相等的则加到新数组
                list.Add(values[i]);

        }

        return list.ToArray();
    }
    #endregion

    #region 获取是否扣税属性
    public static int GetTaxSwitch()
    {
        string sqlStr = "SELECT TaxSwitch FROM dbo.T_Sites";
        DataTable dt = new DataTable();
        dt = Shove.Database.MSSQL.Select(sqlStr);
        return Convert.ToInt32(dt.Rows[0][0]);
    }
    #endregion

    #region 用户名隐藏显示控制
    /// <summary>
    /// 用户名隐藏显示控制
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string UserControlHiedAndShow(string value)
    {
        string result = string.Empty;
        //读取配置
        DataTable dt = Shove.Database.MSSQL.Select("select Opt_UserControlHiedAndShow from T_Sites");
        if (dt == null || dt.Rows.Count == 0) return value;

        //取隐藏字节
        int length = Shove._Convert.StrToInt(dt.Rows[0][0].ToString(), 0);
        //判断为0，用户名将显示全
        if (length == 0) return value;

        //根据字节截取
        result = Shove._String.Cut(value, length).Replace(".", "*");
        //返回结果
        return result;
    }//end UserControlHiedAndShow

    #endregion

    #region 获得用户等级ICON
    /// <summary>
    /// 获得用户等级ICON
    /// </summary>
    /// <param name="userLevel">用户等级值</param>
    /// <param name="siteUrl">站点URL</param>
    /// <returns></returns>
    public static string GetUserLevel(int userLevel, string siteUrl)
    {
        StringBuilder sb = new StringBuilder();
        string path = siteUrl + "/images/level/";
        if (userLevel != 0)
        {
            int a = userLevel / 1000;
            int b = userLevel % 1000 / 100;
            int c = userLevel % 100 / 10;
            int d = userLevel % 10;

            if (a > 0)
            {
                int n = a / 9 + 1;
                for (int i = 0; i < n; i++)
                {
                    if (i >= 4)
                    {
                        break;
                    }
                    int m = (a / 9 > 0) ? 9 : a;
                    sb.Append(path).AppendFormat("crown_{0}.gif,", m);
                    a -= 9; //18
                }
            }
            if (b > 0)
                sb.Append(path).AppendFormat("cup_{0}.gif,", b);
            if (c > 0)
                sb.Append(path).AppendFormat("medal_{0}.gif,", c);
            if (d > 0)
                sb.Append(path).AppendFormat("star_{0}.gif,", d);
        }
        if (sb.Length > 0)
        {
            sb.Remove(sb.Length - 1, 1);
        }
        return sb.ToString();
    }//end GetUserLevel 
    #endregion

    /// <summary>
    /// 验证是否为弱密码
    /// </summary>
    /// <param name="pwd"></param>
    /// <returns>True:是弱密码；False:不是密码</returns>
    public static bool IsLowerPassword(string pwd)
    {
        //如果密码长度小于6位
        if (pwd.Length < 6)
        {
            return true;
        }
        string[] tempArr = new string[pwd.Length];
        for (int i = 0; i < pwd.Length; i++)
        {
            tempArr[i] = pwd[i].ToString();
        }
        //数组去重后是否超过了3位数
        if (ArrayRemoveRe(tempArr).Length < 3)
        {
            return true;
        }
        //检验密码是否为连续的数字
        for (int i = 0; i < pwd.Length - 1; i++)
        {
            int temp = 0;
            if (int.TryParse(pwd[i].ToString(), out temp) && int.TryParse(pwd[i + 1].ToString(), out temp))
            {

                int result = Convert.ToInt32(pwd[i]) - Convert.ToInt32(pwd[i + 1]);
                if (result == -1 || result == 1)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 验证图片是否安全
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool IsSafe(string path)
    {
        StreamReader sr = new StreamReader(path, Encoding.Default);
        string strContent = sr.ReadToEnd();
        strContent = strContent.ToLower();
        sr.Close();
        string str = "request|.getfolder|.createfolder|.deletefolder|.createdirectory|.deletedirectory|.saveas";
        str += "|wscript.shell|script.encode|server.|.createobject|execute|activexobject|language=|unsafe";
        foreach (string s in str.Split('|'))
        {
            if (strContent.IndexOf(s) != -1)
            {
                try
                {
                    File.Delete(path);
                    Users _tempUser = Users.GetCurrentUser(1);
                    new Log("UploadFileEx").Write("上传失败：" + s + "，操作人：" + _tempUser.ID.ToString());
                    return false;
                }
                catch (Exception)
                {
                    File.Delete(path);
                    Users _tempUser = Users.GetCurrentUser(1);
                    new Log("UploadFileEx").Write("上传失败：" + s + "，操作人：" + _tempUser.ID.ToString());
                    return false;
                }
            }
        }
        return true;
    }

    #region 验证QQ号码
    public static bool ValidQQ(string qq)
    {
        Regex reg = new Regex(@"[1-9][0-9]{4,}");
        Match match = reg.Match(qq);
        return match.Success;
    }
    #endregion

    public static int[] GetLotteryBonusArry(int LotteryID)
    {
        DataTable dt = new DAL.Tables.T_WinTypes().Open("DefaultMoney", "LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(LotteryID.ToString()), "[Order]");

        int dtCount = dt.Rows.Count;
        int[] BonusNumbers = new int[dtCount];
        if (dt == null)
        {
            return BonusNumbers;
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string DefaultMoney = dt.Rows[i]["DefaultMoney"].ToString();
            BonusNumbers[i] = Shove._Convert.StrToInt(DefaultMoney.Substring(0, DefaultMoney.Length - 5), 0);
        }

        return BonusNumbers;
    }
}
