using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

using Shove.Database;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Threading;
using System.Linq;

[Serializable]
public class Users
{
    #region 成员变量

    private long _id;
    private long _siteid;
    public string _password;
    public string _passwordadv;
    protected static string _biossSN = Shove._System.SystemInformation.GetBIOSSerialNumber();
    public long ID
    {
        set
        {
            _id = value;

            string ReturnDescription = "";

            if (this.GetUserInformationByID(ref ReturnDescription) < 0)
            {
                _id = -1;
            }
        }
        get
        {
            return _id;
        }
    }

    public long SiteID
    {
        set
        {
            _siteid = value;

            Site.ID = value;

            string ReturnDescription = "";

            Site.GetSiteInformationByID(ref ReturnDescription);

        }
        get
        {
            return _siteid;
        }
    }

    public string Name;
    public string NickName;

    public string Password
    {
        set
        {
            _password = PF.EncryptPassword(value);
        }
        get
        {
            return _password;
        }
    }
    public string PasswordAdv
    {
        set
        {
            _passwordadv = PF.EncryptPassword(value);
        }
        get
        {
            return _passwordadv;
        }
    }

    public string Mobile;
    public bool isMobileValided;

    public bool isCanLogin;

    public DateTime RegisterTime;
    public DateTime LastLoginTime;
    public string LastLoginIP;
    public int LoginCount;

    public short UserType;

    public string BankAddress;
    public string BankName;
    public string BankCardNumber;
    public string UserCradName;


    public double Balance;
    public double Freeze;
    public double HandselAmount;
    public double HandselForzen;
    public double NoCash;

    public int Level;


    public Sites Site = new Sites();


    public string HeadUrl = "";
    public string GexingQianming;
    public string accessToken;
    public int FromClient;

    //代理相关字段
    public int ReferId;
    public string wxopenid;
    public int isAgent;
    public int agentGroup;
    public bool isWXBind;

    /// <summary>
    /// 锁标识列表.true:锁开启,不可下单;false:锁关闭,可下单
    /// </summary>
    public static Dictionary<string, bool> OrderLock = new Dictionary<string, bool>();
    #endregion

    #region 索引器

    public Users this[long siteid, long id]
    {
        get
        {
            Users user = new Users(siteid);

            user.ID = id;

            return user;
        }
    }

    public Users this[long siteid, string name]
    {
        get
        {
            Users user = new Users(siteid);

            user.Name = name;

            string ReturnDescription = "";

            if (user.GetUserInformationByName(ref ReturnDescription) < 0)
            {
                return null;
            }

            return user;
        }
    }

    #endregion

    public Users()
    {
        throw new Exception("不能用无参数的 Users 类的构造来申明实例。此无参数构造函数是为了能使其序列化。");
    }

    public Users(long siteid)
    {

        SiteID = siteid;

        _id = -1;

        Name = "";
        NickName = "";
        Password = "";
        PasswordAdv = "";

        Mobile = "";
        isMobileValided = false;
        isCanLogin = true;
        RegisterTime = DateTime.Now;
        LastLoginTime = DateTime.Now;
        FromClient = 1;//默认来源PC
        try
        {
            LastLoginIP = System.Web.HttpContext.Current.Request.UserHostAddress;
        }
        catch
        {
            LastLoginIP = "127.0.0.1";
        }

        LoginCount = 0;
        UserType = 1;

        BankAddress = "";
        BankName = "";
        BankCardNumber = "";
        UserCradName = "";

        Balance = 0;
        Freeze = 0;
        HandselAmount = 0;
        HandselForzen = 0;
        NoCash = 0;

        Level = 0;

        HeadUrl = "";
        GexingQianming = "";
        accessToken = PF.EncryptPassword(RegisterTime.ToShortTimeString()); ;

        ReferId=60;
        wxopenid="";
        isAgent=0;
        agentGroup=60;
        isWXBind = false;
    }

    public int Add(ref string ReturnDescription)
    {
        RegisterTime = DateTime.Now;
        ReturnDescription = "";

        int ReturnValue = DAL.Procedures.P_UserAdd(SiteID, Name, NickName, Password, PasswordAdv, Mobile, isMobileValided,
             UserType, accessToken, FromClient,ReferId,wxopenid,isAgent,agentGroup,isWXBind,HeadUrl, ref _id, ref ReturnDescription);

        if (ReturnValue < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (_id < 0)
        {
            return (int)_id;
        }

        ID = _id;
        return (int)_id;
    }

    #region 修改用户资料

    public int EditByID(ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_UserEditByID(SiteID, ID, Name, NickName, Password, PasswordAdv, Mobile,UserType,
            isMobileValided, isCanLogin,BankAddress, BankName, BankCardNumber,UserCradName,Balance,Level,HeadUrl,GexingQianming,accessToken,
            HandselAmount, ReferId, isAgent, agentGroup, isWXBind,
            ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        Sites site = new Sites(1)[1];
        Users user = new Users(1)[1, ID];
        string userName = user.Name;
        if (userName.Length < 1)
        {
            userName = user.Mobile;
        }
 

        return 0;
    }

    public int EditByName(ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_UserEditByName(SiteID, ID, Name, NickName, Password, PasswordAdv, Mobile,
            isMobileValided, isCanLogin, BankAddress, BankName, BankCardNumber, UserCradName, Balance, Level, HeadUrl, GexingQianming, accessToken,
            HandselAmount, ReferId, isAgent, agentGroup, isWXBind,
            ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        Sites site = new Sites(1)[1];
        Users user = new Users(1)[1, ID];
        string userName = user.Name;
        if (userName.Length < 1)
        {
            userName = user.Mobile;
        }


        return 0;
    }
    #endregion

    #region 用户登录

    // 正常用户登录
    public int Login(ref string ReturnDescription)
    {
        int ReturnValue = -1;
        ReturnDescription = "";


        /*
         * 这一步操作非常耗时
        if (LastLoginIP == "" || LastLoginIP == null)
        {
            LastLoginIP = System.Web.HttpContext.Current.Request.UserHostAddress;
        }

        IPselect(LastLoginIP); 
         */
        
        int Result = DAL.Procedures.P_Login(SiteID, Name, Password, LastLoginIP, ref _id, ref _passwordadv,
             ref Mobile, ref isMobileValided, ref isCanLogin, ref RegisterTime, ref LastLoginTime,
            ref LastLoginIP, ref LoginCount, ref UserType, ref BankAddress, ref BankName, ref BankCardNumber, ref Balance, ref Freeze,
            ref HandselAmount, ref Level, 
            ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            return Result;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        ID = _id;


        if (UserType==99)
        {
            SaveUserIDToAdminCookie();
            SaveUserIDToCookie();
        }
        else
        {
            SaveUserIDToCookie();
        }
        return 0;
    }

    public int LoginAdmin(ref string ReturnDescription)
    {
        
        return 0;
    }

    //查询IP
    public string IPselect(string ip)
    {
        string address = "未知地区";

        DataTable dt = new DAL.Tables.T_IPAddress().Open("City", " Country = '" + ip + "'", "");
        if (dt.Rows.Count > 0)
        {
            address = dt.Rows[0]["City"].ToString();
        }
        else
        {
            try
            {
                WebRequest request = WebRequest.Create("http://www.ip138.com/ips138.asp?ip=" + ip + "&action=2");
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));
                string str = reader.ReadToEnd();

                MatchCollection ml = Regex.Matches(str, "<td align=\"center\"><ul class=\"ul1\"><li>.*?</li>");
                foreach (Match m in ml)
                {
                    address = m.Value.Replace("<td align=\"center\"><ul class=\"ul1\"><li>本站主数据：", "").Replace("</li>", "");
                }

                if (address == "保留地址 ")
                {
                    address = address.Replace("保留地址 ", "未知地区");
                }

                string sql = string.Format("insert into T_IPAddress(Country,City) values('{0}','{1}')", ip, address);
                int i = Shove.Database.MSSQL.ExecuteNonQuery(sql);
                if (i < 0)
                {
                    new Log("IP_Log").Write("T_IPAddress表写入失败");
                }

                reader.Close();
                reader.Dispose();
                response.Close();
            }
            catch (Exception e)
            {
                new Log("IP_Log").Write("IP地址获取异常！：" + e.Message);
            }
        }

        return address;
    }

    // 直接设置了登录状态
    public int LoginDirect(ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        ReturnDescription = "";

        if (!isCanLogin)
        {
            ReturnDescription = "用户被限制登录";

            return -1;
        }

        SaveUserIDToCookie();

        return 0;
    }

    // 注销
    public int Logout(ref string ReturnDescription)
    {
        ReturnDescription = "";

        RemoveUserIDFromCookie();
        RemoveUserIDFromAdminCookie();

        return 0;
    }

    private void SaveUserIDToCookie()
    {
        string Key = "SLS.Center.LoginUserID";

        HttpCookie cookieLoginUserID = new HttpCookie(Key, Shove._Security.Encrypt.Encrypt3DES(PF.GetCallCert(), Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), ID.ToString() + ";" + _biossSN), PF.DesKey));
        cookieLoginUserID.Path = "/";


        try
        {
            HttpContext.Current.Response.Cookies.Add(cookieLoginUserID);
        }
        catch { }

    }

    private void SaveUserIDToAdminCookie()
    {
        string Key = "SLS.Center.LoginAdminUserID";

        HttpCookie cookieLoginUserID = new HttpCookie(Key, Shove._Security.Encrypt.Encrypt3DES(PF.GetCallCert(), Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), ID.ToString() + ";" + _biossSN), PF.DesKey));
        cookieLoginUserID.Path = "/";

        try
        {
            HttpContext.Current.Response.Cookies.Add(cookieLoginUserID);
        }
        catch { }

    }

    private void RemoveUserIDFromCookie()
    {
        string Key = "SLS.Center.LoginUserID";

        HttpCookie cookieLoginUserID = new HttpCookie(Key);
        cookieLoginUserID.Value = "";
        cookieLoginUserID.Expires = DateTime.Now.AddYears(-20);
        cookieLoginUserID.Path = "/";

        try
        {
            HttpContext.Current.Response.Cookies.Add(cookieLoginUserID);
        }
        catch { }
    }

    private void RemoveUserIDFromAdminCookie()
    {
        string Key = "SLS.Center.LoginAdminUserID";

        HttpCookie cookieLoginUserID = new HttpCookie(Key);
        cookieLoginUserID.Value = "";
        cookieLoginUserID.Expires = DateTime.Now.AddYears(-20);
        cookieLoginUserID.Path = "/";

        try
        {
            HttpContext.Current.Response.Cookies.Add(cookieLoginUserID);
        }
        catch { }
    }

    public static Users GetCurrentUser(long siteid)
    {
        string Key = "SLS.Center.LoginUserID";

        // 从 Cookie 中取出 UserID
        HttpCookie cookieUser = HttpContext.Current.Request.Cookies[Key];

        if ((cookieUser == null) || (String.IsNullOrEmpty(cookieUser.Value)))
        {
            return null;
        }

        string CookieUserID = cookieUser.Value;

        try
        {
            CookieUserID = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), Shove._Security.Encrypt.Decrypt3DES(PF.GetCallCert(), CookieUserID, PF.DesKey));
            string tempSn = CookieUserID.Split(';')[1];
            if (tempSn == _biossSN)
            {
                CookieUserID = CookieUserID.Split(';')[0];
            }
            else
            {
                CookieUserID = "";
            }
        }
        catch
        {
            CookieUserID = "";
        }

        long UserID = -1;
        Users t_user = null;

        if (!String.IsNullOrEmpty(CookieUserID))
        {
            UserID = Shove._Convert.StrToLong(CookieUserID, -1);

            if (UserID > 0)
            {
                t_user = new Users(siteid)[siteid, UserID];

                if (t_user != null)
                {
                    if (OrderLock == null || !OrderLock.ContainsKey(UserID.ToString()))
                    {
                        OrderLock.Add(UserID.ToString(), false);
                    }
                    return t_user;
                }
            }
        }

        return null;
    }

    public static Users GetCurrentAdminUser(long siteid)
    {
        string Key = "SLS.Center.LoginAdminUserID";

        // 从 Cookie 中取出 UserID
        HttpCookie cookieUser = HttpContext.Current.Request.Cookies[Key];

        if ((cookieUser == null) || (String.IsNullOrEmpty(cookieUser.Value)))
        {
            return null;
        }

        string CookieUserID = cookieUser.Value;

        try
        {
            CookieUserID = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), Shove._Security.Encrypt.Decrypt3DES(PF.GetCallCert(), CookieUserID, PF.DesKey));
            string tempSn = CookieUserID.Split(';')[1];
            if (tempSn == _biossSN)
            {
                CookieUserID = CookieUserID.Split(';')[0];
            }
            else
            {
                CookieUserID = "";
            }
        }
        catch
        {
            CookieUserID = "";
        }

        long UserID = -1;
        Users t_user = null;

        if (!String.IsNullOrEmpty(CookieUserID))
        {
            UserID = Shove._Convert.StrToLong(CookieUserID, -1);

            if (UserID > 0)
            {
                t_user = new Users(siteid)[siteid, UserID];

                if (t_user != null)
                {
                    return t_user;
                }
            }
        }

        return null;
    }

    #endregion

    #region 获取用户信息
    public int GetUserInformationByID(ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        Users tu = new Users(SiteID);
        Clone(tu);

        int Result = DAL.Procedures.P_GetUserInformationByID(ID, SiteID, ref Name, ref NickName, ref _password, ref _passwordadv, ref Mobile, ref isMobileValided, ref isCanLogin,
            ref RegisterTime, ref LastLoginTime, ref LastLoginIP, ref LoginCount, ref UserType, ref BankAddress, ref BankName, ref BankCardNumber, ref UserCradName, ref Balance,
            ref Freeze, ref Level, ref HeadUrl, ref GexingQianming, ref accessToken, ref HandselAmount, ref HandselForzen,
            ref NoCash, ref FromClient, ref ReferId, ref wxopenid, ref isAgent, ref agentGroup, ref isWXBind, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            tu.Clone(this);

            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            tu.Clone(this);

            return ReturnValue;
        }

        return 0;
    }

    public int GetUserInformationByID(ref string ReturnDescription, long uid)
    {
        if (uid < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        Users tu = new Users(SiteID);
        Clone(tu);

        int Result = DAL.Procedures.P_GetUserInformationByID(ID, SiteID, ref Name, ref NickName, ref _password, ref _passwordadv, ref Mobile, ref isMobileValided, ref isCanLogin,
            ref RegisterTime, ref LastLoginTime, ref LastLoginIP, ref LoginCount, ref UserType, ref BankAddress, ref BankName, ref BankCardNumber, ref UserCradName, ref Balance,
            ref Freeze, ref Level, ref HeadUrl, ref GexingQianming, ref accessToken, ref HandselAmount, ref HandselForzen,
            ref NoCash, ref FromClient, ref ReferId, ref wxopenid, ref isAgent, ref agentGroup, ref isWXBind, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            tu.Clone(this);

            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            tu.Clone(this);

            return ReturnValue;
        }

        return 0;
    }

    public int GetUserInformationByName(ref string ReturnDescription)
    {
        int ReturnValue = -1;
        ReturnDescription = "";

        Users tu = new Users(SiteID);
        Clone(tu);

        int Result = DAL.Procedures.P_GetUserInformationByName(Name, SiteID, ref _id, ref NickName, ref _password, ref _passwordadv, ref Mobile, ref isMobileValided, ref isCanLogin,
            ref RegisterTime, ref LastLoginTime, ref LastLoginIP, ref LoginCount, ref UserType, ref BankAddress, ref BankName, ref BankCardNumber, ref UserCradName, ref Balance,
            ref Freeze, ref Level, ref HeadUrl, ref GexingQianming, ref accessToken, ref HandselAmount, ref HandselForzen,
            ref NoCash, ref FromClient, ref ReferId, ref wxopenid, ref isAgent, ref agentGroup, ref isWXBind, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            tu.Clone(this);

            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            tu.Clone(this);

            return ReturnValue;
        }

        return 0;
    }

    #endregion

    #region 充值

    // 在线支付后，增加电子货币
    public int AddUserBalance(double Money, double FormalitiesFees, string PayNumber, string PayBank, string Memo, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_UserAddMoney(SiteID, ID, Money, FormalitiesFees, PayNumber, PayBank, Memo, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        return 0;
    }
    // 在线支付后，增加电子货币
    public int AddUserBalance(double Money, double FormalitiesFees, string PayNumber, string PayBank, string Memo, string trade_on, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_UserAddMoneys(SiteID, ID, Money, FormalitiesFees, PayNumber, PayBank, Memo, trade_on, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }
        return 0;
    }

    // 手动增加电子货币(后台直接为用户充值)
    public int AddUserBalanceManual(double Money, long uid, string Memo, long OperatorID, int IsGiveHandsel, ref string ReturnDescription)
    {
        if (uid < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_UserAddMoneyManual(SiteID, uid, Money, Memo, OperatorID, IsGiveHandsel, ref ReturnValue, ref ReturnDescription);
        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2, uid);

        return 0;
    }
    // 银行卡增加电子货币
    public int AddUserBalanceManualByPayType(string PayType, double Money, long uid, string Memo, long OperatorID, int IsGiveHandsel, ref string ReturnDescription)
    {
        if (uid < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_UserAddMoneyManualByPayType(SiteID, uid, PayType, Money, Memo, OperatorID, IsGiveHandsel, ref ReturnValue, ref ReturnDescription);
        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2, uid);

        return 0;
    }
    // 手动增加彩金(后台直接为用户充值)
    public int AddUserHandselManual(double Money, long uid, string Memo, long OperatorID, ref string ReturnDescription)
    {
        if (uid < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_UserAddHandselManual(SiteID, uid, Money, Memo, OperatorID, ref ReturnValue, ref ReturnDescription);
        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2, uid);

        return 0;
    }

    #endregion

    #region 购彩、撤单、追号、追号套餐

    public long InitiateScheme(long IsuseID, int PlayTypeID, string Title, string Description, string LotteryNumber, string UpdateloadFileContent, int Multiple,
        double Money, double AssureMoney, int Share, int BuyShare, string OpenUsers, short SecrecyLevel, double SchemeBonusScale, int comefromClient, ref string ReturnDescription)
    {
        if (!PF.IsSystemRegister())
        {

            ReturnDescription = "请联系管理员输入软件序列号！";

            return -101;
        }

        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        ReturnDescription = "";

        if ((SecrecyLevel < 0) || (SecrecyLevel > 3))
        {
            SecrecyLevel = 0;
        }

        long SchemeID = -1;

        int Result = DAL.Procedures.P_InitiateScheme(SiteID, ID, IsuseID, PlayTypeID, Title, Description, LotteryNumber, UpdateloadFileContent,
            Multiple, Money, AssureMoney, Share, BuyShare, OpenUsers.Replace('，', ','), SecrecyLevel, SchemeBonusScale, comefromClient, ref SchemeID, ref ReturnDescription);

        if (Result < 0)
        {
            return -1;
        }

        if (SchemeID < 0)
        {
            return SchemeID;
        }
        return SchemeID;
    }

    public long InitiateSchemeYT(long IsuseID, int PlayTypeID, string Title, string Description, string LotteryNumber, string UpdateloadFileContent, int Multiple,
        double Money, double AssureMoney, int Share, int BuyShare, string OpenUsers, short SecrecyLevel, double SchemeBonusScale, int comefromClient, double LimitMoney, ref string ReturnDescription)
    {
        if (!PF.IsSystemRegister())
        {

            ReturnDescription = "请联系管理员输入软件序列号！";

            return -101;
        }

        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        ReturnDescription = "";

        if ((SecrecyLevel < 0) || (SecrecyLevel > 3))
        {
            SecrecyLevel = 0;
        }

        long SchemeID = -1;

        int Result = DAL.Procedures.P_InitiateSchemeYT(SiteID, ID, IsuseID, PlayTypeID, Title, Description, LotteryNumber, UpdateloadFileContent,
            Multiple, Money, AssureMoney, Share, BuyShare, OpenUsers.Replace('，', ','), SecrecyLevel, SchemeBonusScale, comefromClient, LimitMoney, ref SchemeID, ref ReturnDescription);

        if (Result < 0)
        {
            return -1;
        }

        if (SchemeID < 0)
        {
            return SchemeID;
        }

        return SchemeID;
    }


    public int JoinScheme(long SchemeID, int Share, int FromClient, ref string ReturnDescription, int buyShare, string buyDateTime)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_JoinScheme(Site.ID, ID, SchemeID, Share, false, FromClient, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            //关闭订单锁
            PF.OrderLock(this.ID.ToString(), this.Name, false, "数据库读写错误");
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            //关闭订单锁
            PF.OrderLock(this.ID.ToString(), this.Name, false, "数据库读写错误");
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);
        //关闭订单锁
        PF.OrderLock(this.ID.ToString(), this.Name, false, "参与合买成功，谢谢！");
        return 0;
    }

    public int QuashScheme(long SchemeID, bool isSystemQuashed, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_QuashScheme(Site.ID, SchemeID, isSystemQuashed, false, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }
        return 0;
    }

    public int Quash(long BuyDetailID, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_Quash(SiteID, BuyDetailID, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }
        return 0;
    }

    public int InitiateChaseTask(string Title, string Description, int LotteryID, double StopWhenWinMoney, string DetailXML, string LotteryNumber, double SchemeBonusScalec, int comefromClient, ref long SchemeID, ref string ReturnDescription)
    {
        if (!PF.IsSystemRegister())
        {

            ReturnDescription = "请联系管理员输入软件序列号！";

            return -101;
        }

        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        ReturnDescription = "";

        long ChaseTaskID = -1;

        int Result = DAL.Procedures.P_InitiateChaseTask(Site.ID, ID, Title, Description, LotteryID, StopWhenWinMoney, DetailXML, LotteryNumber, SchemeBonusScalec, comefromClient, ref ChaseTaskID, ref SchemeID, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }
        
        if (ChaseTaskID < 0)
        {
            return (int)ChaseTaskID;
        }

        return (int)ChaseTaskID;
    }

    public int ExecChaseTaskDetail(long ChaseTaskDetailID, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";
        long schemeId = 0;

        int Result = DAL.Procedures.P_ExecChaseTaskDetail(Site.ID, ChaseTaskDetailID, ref ReturnValue, ref ReturnDescription, ref schemeId);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }
        return 0;
    }

    public int QuashChaseTask(long ChaseTaskID, bool isSystemQuashed, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_QuashChaseTask(Site.ID, ChaseTaskID, isSystemQuashed, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        return 0;
    }

    public int QuashChaseTaskDetail(long ChaseTaskDetailID, bool isSystemQuashed, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_QuashChaseTaskDetail(Site.ID, ChaseTaskDetailID, isSystemQuashed, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        return 0;
    }

    public int InitiateCustomChase(int LotteryID, int PlayTypeID, int Price, short Type, DateTime EndTime, int IsuseCount, int Multiple, int Nums, short BetType, string LotteryNumber, short StopType, double stopMoney, double Money, string Title, string ChaseXML, ref string ReturnDescription)
    {
        if (!PF.IsSystemRegister())
        {

            ReturnDescription = "请联系管理员输入软件序列号！";

            return -101;
        }

        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        ReturnDescription = "";

        int CustomChaseID = -1;
        DataSet ds = new DataSet();
        int Result = DAL.Procedures.P_ChasesAdd(ref ds, ID, LotteryID, PlayTypeID, Price, Type, DateTime.Now, EndTime, IsuseCount, Multiple, Nums, BetType, LotteryNumber, StopType, stopMoney, Money, Title, ChaseXML, ref CustomChaseID, ref ReturnDescription);

        if (Result < 0)
        {
            return -1;
        }

        if (CustomChaseID < 0)
        {
            return (int)CustomChaseID;
        }
        return (int)CustomChaseID;
    }

    #endregion


    #region 中奖

    public int Win(long SchemeID, double Money, ref string ReturnDescription)  // 中奖后，发送通知。注意：不写中奖数据，由后台统一计算、写入。
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        ReturnDescription = "";
        return 0;
    }

    #endregion

    #region 提款
    public int Distill(int DistillType, double Money, double FormalitiesFees, string BankUserName, string _BankName, string _BankCardNumber, string _AlipayID, string _AlipayName, string Memo, bool IsCps, ref string ReturnDescription, int MoneyType)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_Distill(Site.ID, ID, DistillType, Money, FormalitiesFees, BankUserName, _BankName, _BankCardNumber, _AlipayID, _AlipayName, Memo, IsCps, ref ReturnValue, ref ReturnDescription, MoneyType);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }
        return 0;
    }

    public int DistillQuash(long DistillID, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_DistillQuash(Site.ID, ID, DistillID, ref ReturnValue, ref  ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        return 0;
    }

    public int DistillAccept(long DistillID, string PayName, string PayBank, string PayCardNumber, string _AlipayID, string _AlipayName, string Memo, long HandleOperatorID, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_DistillAccept(Site.ID, ID, DistillID, PayName, PayBank, PayCardNumber, _AlipayID, _AlipayName, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }
        return 0;
    }

    public int DistillNoAccept(long DistillID, string Memo, long HandleOperatorID, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_DistillNoAccept(Site.ID, ID, DistillID, Memo, HandleOperatorID, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        Sites site = new Sites(1)[1];
        Users user = new Users(1)[1, ID];
        string userName = user.Name;
        if (userName.Length < 1)
        {
            userName = user.Mobile;
        }

        return 0;
    }

    #endregion

    #region 积分兑换

    public int ScoringExchange(double Scoring, ref string ReturnDescription)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_ScoringExchange(Site.ID, ID, Scoring, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            ReturnDescription = "数据库读写错误";

            return -1;
        }

        if (ReturnValue < 0)
        {
            return ReturnValue;
        }

        string ReturnDescription_2 = "";
        GetUserInformationByID(ref ReturnDescription_2);

        return 0;
    }

    #endregion

    #region 是否在招股对象范围之内、以及能够查看方案

    public bool isInOpenUsers(long SchemeID)
    {
        return isInOpenUsers(DAL.Functions.F_GetSchemeOpenUsers(SchemeID));
    }

    public bool isInOpenUsers(string OpenUsers) // OpenUsers:[1][2]...
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        if (OpenUsers == "")
        {
            return true;
        }

        return (OpenUsers.IndexOf("[" + ID.ToString() + "]") >= 0);
    }


    #endregion

    /// <summary>
    /// 上网IP所在地区
    /// </summary>
    /// <returns></returns>
    public string GetConnectNetAddress(string ip)
    {
        string address = "";

        IPAddressInfo ipAddressInfo = PF.GetAddressInfo(PF.GetIP());
        if (ipAddressInfo != null)
        {
            address = ipAddressInfo.province + (ipAddressInfo.city != "" ? "-" + ipAddressInfo.city : "") + (ipAddressInfo.district != "" ? "-" + ipAddressInfo.district : "");
        }

        return address;
    }

    public void Clone(Users user)
    {
        user._id = _id;
        user._siteid = _siteid;
        user._password = _password;
        user._passwordadv = _passwordadv;

        user.Name = Name;
        user.NickName = NickName;

        user.Mobile = Mobile;
        user.isMobileValided = isMobileValided;

        user.isCanLogin = isCanLogin;

        user.RegisterTime = RegisterTime;
        user.LastLoginTime = LastLoginTime;
        user.LastLoginIP = LastLoginIP;
        user.LoginCount = LoginCount;

        user.UserType = UserType;

        user.NickName = NickName;
        user.BankName = BankName;
        user.BankCardNumber = BankCardNumber;
        user.UserCradName = UserCradName;

        user.Balance = Balance;
        user.Freeze = Freeze;
        user.Level = Level;

        user.HeadUrl = HeadUrl;
        user.GexingQianming = GexingQianming;
        user.accessToken = accessToken;
        

        user.HandselAmount = HandselAmount;
        user.HandselForzen = HandselForzen;
        user.NoCash = NoCash;

        user.FromClient = FromClient;
        user.ReferId = ReferId;
        user.wxopenid = wxopenid;

        user.isAgent = isAgent;
        user.agentGroup = agentGroup;
        user.isWXBind = isWXBind;

        user.Site = Site;
    }

    #region 得到推广URL
    public string GetPromotionURL(int type)
    {
        if (ID < 0)
        {
            throw new Exception("Users 尚未初始化到具体的数据实例上，请先使用 GetUserInformation 等获取数据信息");
        }

        string fileName = "PromoteUserReg.aspx";

        if (type == 1)
        {
            fileName = "PromoteCpsReg.aspx";
        }

        string url = Shove._Web.Utility.GetUrl() + "/Home/Room/" + fileName + "?id=" + ID.ToString().PadLeft(10, '0') + GetSign().ToString();

        return url;
    }

    public int GetSign()
    {
        /*1.把用户ID的位数和系数相乘的结果相加
        从高位到低位系数分别为8、2、5、7、1
         2.得到的结果除以8，取余数
         */
        int number = 0;

        int[] coefficient = new int[] { 8, 2, 5, 7, 1 };

        for (int i = 0; i < ID.ToString().Length; i++)
        {
            number += Shove._Convert.StrToInt(ID.ToString().Substring(i, 1), 0) * coefficient[i % 5];
        }

        number = number % 10;

        return number;
    }
    #endregion

    #region 根据用户ID获得用户对象
    /// <summary>
    /// 根据用户ID获得用户对象
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static DataTable GetUserById(long userID)
    {
        return Shove.Database.MSSQL.Select("SELECT * FROM dbo.T_Users WHERE SiteID = 1 and id = " + userID); ;
    }//end GetUserById 
    #endregion

    #region 用户名隐藏显示控制(2015年6月12日 10:18:05)
    /// <summary>
    /// 用户名隐藏显示控制
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string UserControlHiedAndShow(string value)
    {
        string result = string.Empty;
        //读取配置
        DataTable dt = MSSQL.Select("select Opt_UserControlHiedAndShow from T_Sites");
        if (dt == null || dt.Rows.Count == 0) return value;

        //取隐藏字节
        int length = Shove._Convert.StrToInt(dt.Rows[0][0].ToString(), 0);
        //判断为0，用户名将显示全
        if (length == 0) return value;

        //根据字节截取
        result = Shove._String.Cut(value, length).Replace(".", "*");
        //返回结果
        return result;
    }

    #endregion

    public string GetSchemeNumber(string schemeId)
    {
        string sqlStr = "SELECT SchemeNumber FROM dbo.T_Schemes WHERE ID=" + schemeId;
        DataTable dt = Shove.Database.MSSQL.Select(sqlStr);
        if (dt != null && dt.Rows.Count > 0)
        {
            return dt.Rows[0]["SchemeNumber"].ToString();
        }
        return "";
    }
}

