using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public partial class UserReg : SitePageBase
{
    public long _Cpsid = -1;
    bool Opt_Cps_Status_ON =false;
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(UserReg), this.Page);
        
        if (!IsPostBack)
        {
            GetCPSExtension();
            GetImg();
        }
        Opt_Cps_Status_ON = _Site.Opt_Cps_Status_ON;
        string attr = _Site.Opt_Cps_Status_ON ? "block" : "none";
        liCpsCode.Style.Add("display", attr);
        limCpsCode.Style.Add("display", attr);
    }
    /// <summary>
    /// 获得CPS推广
    /// </summary>
    private void GetCPSExtension()
    {
        string str = Shove._Web.Utility.GetRequest("flag");
        if ("" != str)
        {
            string CpsCode = Shove._Security.Encrypt.Decrypt3DES(PF.GetCallCert(), str, PF.DesKey);

            if (CpsCode.Length == 0) return;

            regCpsCode.Value = CpsCode;
            regMCpsCode.Value = CpsCode;

            long cpsID = CPSBLL.GetCpsIDByCode(CpsCode);
            if (cpsID > 0)
            {
                Session["UserReg_Change_CpsID"] = cpsID;
            }
        }
    }
    /// <summary>
    /// 图片
    /// </summary>
    private void GetImg()
    {
        DataTable dt = new DAL.Tables.T_PageResources().Open("*", "PageName='表头logo'", "");
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row["ResourceName"].Equals("左边"))
                {
                    LogoImage.InnerHtml = "<a href=\"/Default.aspx\"> <img src=\"" + row["ResourceUrl"] + "\" style=\"border: 0px;width:250px;height:77px;\" alt=\"\" /></a>";
                }
            }
        }
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public int CheckMobile(string mobile)
    {
        Regex regex = new Regex(@"^[1][358]\d{9}$");
        Match m = regex.Match(mobile.Trim());
        if (!m.Success)
        {
            return -1;
        }
        int Number = 0;
        Random rd = new Random();
        Number = rd.Next(100000, 1000000);
        Sites site = new Sites(1)[1];
        string Content = MessageTemplate.BindPhone.Replace("[WebSiteName]", site.Name).Replace("[UserName]", mobile).Replace("[Code]", Number.ToString());
        string resultMsg = "";
        bool result = Message.Send(-1, mobile, "", Content, Number.ToString(), MessageType.UserRegistration, SendTypeSingle.Sms, ref resultMsg);
        if (result)
        {
            return 1;
        }
        return -2;
    }

    /// <summary>
    /// 手机注册
    /// </summary>
    /// <param name="regMobile"></param>
    /// <param name="regPwd1"></param>
    /// <param name="regPwd2"></param>
    /// <param name="regCode"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public string MobileRegister(string regMobile, string regPwd1, string regPwd2, string regCode, string qq)
    {
        DataTable dt = new DAL.Tables.T_SMS().Open(" top 1 *", " [DateTime] > DATEADD(MINUTE,-10,GETDATE()) and [To] = '" + regMobile + "' and VerifyCode = '" + regCode + "'", " [DateTime] desc");

        if (dt == null || dt.Rows.Count <= 0)
        {
            return "-3"; //验证码输入错误
        }
        if (PF.IsLowerPassword(regPwd1))
        {
            return "-4";//密码强度过弱
        }
        if (!PF.ValidQQ(qq))
        {
            return "-5";
        }

        long cps_id = Shove._Convert.StrToLong(Session["UserReg_Change_CpsID"] + "", -1);
        if (Opt_Cps_Status_ON && cps_id <= 0)
        {
            return "-1";
        }

        string mobile = regMobile;
        string password = regPwd1;
        string password2 = regPwd2;

        Users user = new Users(1);

        user.Name = "";
        user.Mobile = mobile;
        user.isMobileValided = true;
        user.Password = password;


        string ReturnDescription = "";
        int Result = user.Add(ref ReturnDescription);

        if (Result < 0)
        {
            new Log("Users").Write("会员注册不成功：" + ReturnDescription);
            return "-1";
        }

        if (cps_id > 0)//写入会员转移表
        {
            DAL.Tables.T_CpsUserChange tcpsUserChange = new DAL.Tables.T_CpsUserChange();
            if (tcpsUserChange.GetCount("ChangeType = 0 and Type = -1 and UserID=" + Result + " ") < 1)//普通会员
            {
                tcpsUserChange.UserID.Value = Result;
                tcpsUserChange.DateTime.Value = DateTime.Now;
                tcpsUserChange.NowUserID.Value = CPSBLL.getUserIDByCpsID(cps_id + "");
                tcpsUserChange.Type.Value = -1;
                tcpsUserChange.ChangeType.Value = 0;
                DAL.Tables.T_CPSRel cpsRel = new DAL.Tables.T_CPSRel();
                cpsRel.UserID.Value = Result;
                cpsRel.ParentID.Value = CPSBLL.getUserIDByCpsID(cps_id + "");
                cpsRel.MemberType.Value = -1;
                cpsRel.IsPassed.Value = 1;
                cpsRel.Insert();
                tcpsUserChange.Insert();
            }
        }

        Result = user.Login(ref ReturnDescription);

        if (Result < 0)
        {
            new Log("Users").Write("注册成功后登录失败：" + ReturnDescription);
            return "-2";
        }

        return "/Home/Room/UserRegSuccess.aspx";
    }

    /// <summary>
    /// 一般注册
    /// </summary>
    /// <param name="regName"></param>
    /// <param name="regPwd1"></param>
    /// <param name="regPwd2"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public string Register(string regName, string regPwd1, string regPwd2, string qq)
    {
        string name = regName;
        string password = regPwd1;
        string password2 = regPwd2;

        if (!IsKeyWords(name))
        {
            new Log("Users").Write("用户名中包含敏感字符，请换一个用户名");
            return "-3";
        }
        if (PF.IsLowerPassword(regPwd1))
        {
            //密码强度过弱
            return "-4";
        }
        if (qq == null || qq == "")
        {
            return "-5";
        }
        if (!PF.ValidQQ(qq))
        {
            return "-5";
        }

        long cps_id = Shove._Convert.StrToLong(Session["UserReg_Change_CpsID"] + "", -1);
        if (Opt_Cps_Status_ON && cps_id <= 0)
        {
            return "-1";
        }


        Users user = new Users(1);

        user.Name = name;
        user.Password = password;

        string ReturnDescription = "";
        int Result = user.Add(ref ReturnDescription);

        if (Result < 0)
        {
            new Log("Users").Write("会员注册不成功：" + ReturnDescription);
            return "-1";
        }

        if (cps_id > 0)//写入会员转移表
        {
            DAL.Tables.T_CpsUserChange tcpsUserChange = new DAL.Tables.T_CpsUserChange();
            if (tcpsUserChange.GetCount("ChangeType = 0 and Type = -1 and UserID = " + Result + " ") < 1)//普通会员
            {
                tcpsUserChange.UserID.Value = Result;
                tcpsUserChange.DateTime.Value = DateTime.Now;
                tcpsUserChange.NowUserID.Value = CPSBLL.getUserIDByCpsID(cps_id + "");
                tcpsUserChange.Type.Value = -1;
                tcpsUserChange.ChangeType.Value = 0;
                DAL.Tables.T_CPSRel cpsRel = new DAL.Tables.T_CPSRel();
                cpsRel.UserID.Value = Result;
                cpsRel.ParentID.Value = CPSBLL.getUserIDByCpsID(cps_id + "");
                cpsRel.MemberType.Value = -1;
                cpsRel.IsPassed.Value = 1;
                cpsRel.Insert();
                tcpsUserChange.Insert();
            }
        }

        Result = user.Login(ref ReturnDescription);

        if (Result < 0)
        {
            new Log("Users").Write("注册成功后登录失败：" + ReturnDescription);
            return "-2";
        }

        return "/Home/Room/UserRegSuccess.aspx";
    }

    private bool IsKeyWords(string UserName)
    {
        DataTable dtKeyWords = new DAL.Tables.T_Sensitivekeywords().Open("", "", "");

        string KeyWords = "";

        if (dtKeyWords == null || dtKeyWords.Rows.Count < 1)
        {
            return true;
        }

        KeyWords = dtKeyWords.Rows[0]["KeyWords"].ToString().Replace("<p>", "").Replace("</p>", "");

        foreach (string str in KeyWords.Split(','))
        {
            if (string.IsNullOrEmpty(str))
                continue;
            if (UserName.IndexOf(str) >= 0)
            {
                return false;
            }
        }

        return true;
    }

}
