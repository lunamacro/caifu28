using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

public class Message
{
    public Message()
    {
    }
    /// <summary>
    /// 发送消息(此方法会根据系统后台设置的消息选项进行消息的发送)
    /// </summary>
    /// <param name="toUserId">接收人</param>
    /// <param name="content">信息内容</param>
    /// <param name="type">信息类型</param>
    /// <returns>True:发送成功;False:发送失败</returns>
    public static bool Send(int toUserId, string toUserPhone, string toUserEmail, string content, string verificationCode, MessageType type, SendTypeSingle sendTypeSingle, ref string resultMsg)
    {
        
        return true;
    }
    /// <summary>
    /// 发送短信
    /// </summary>
    /// <returns></returns>
    private static bool SendSms(string toUserPhone, string content, string verificationCode)
    {
        DAL.Tables.T_SMS sms = new DAL.Tables.T_SMS();
        sms.Content.Value = content;
        sms.DateTime.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        sms.From.Value = "";
        sms.IsSent.Value = 0;
        sms.SiteID.Value = 1;
        sms.SMSID.Value = -1;
        sms.To.Value = toUserPhone;
        sms.VerifyCode.Value = verificationCode;
        long i = sms.Insert();
        if (i > -1)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <returns></returns>
    private static bool SendEmail(int toUserId, string toUserEmail, string content, string title, string code)
    {
        DAL.Tables.T_Email email = new DAL.Tables.T_Email();
        email.Content.Value = content;
        email.DateTime.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        email.IsSent.Value = 0;
        email.SiteID.Value = 1;
        email.To.Value = toUserEmail;
        email.UserID.Value = toUserId;
        email.Title.Value = title;
        email.VerifyCode.Value = code;
        long i = email.Insert();
        if (i > -1)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 发送站内信
    /// </summary>
    /// <returns></returns>
    private static bool SendStation(int toUserId, string content)
    {
        DAL.Tables.T_StationSMS stationsms = new DAL.Tables.T_StationSMS();
        stationsms.AimID.Value = toUserId;
        stationsms.Content.Value = content;
        stationsms.DateTime.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        stationsms.isRead.Value = 0;
        stationsms.isShow.Value = 1;
        stationsms.SiteID.Value = 1;
        stationsms.SourceID.Value = 1;
        stationsms.title.Value = "";
        stationsms.Type.Value = 2;
        long i = stationsms.Insert();
        if (i > -1)
        {
            return true;
        }
        return false;
    }
}

#region 单独发送类型
public enum SendTypeSingle
{
    /// <summary>
    /// 根据系统后台设置自适应
    /// </summary>
    SelfAdaption,
    /// <summary>
    /// 单独发送短信
    /// </summary>
    Sms,
    /// <summary>
    /// 单独发送邮件
    /// </summary>
    Email,
    /// <summary>
    /// 单独发送
    /// </summary>
    StationMail
}
#endregion

#region 消息类型
public enum MessageType
{
    /// <summary>
    /// 绑定手机号码
    /// </summary>
    BindPhone,
    /// <summary>
    /// 绑定邮箱
    /// </summary>
    BindEmail,
    /// <summary>
    /// 找回密码
    /// </summary>
    RetrievePassword,
    /// <summary>
    /// 用户注册
    /// </summary>
    UserRegistration,
    /// <summary>
    /// 提款被受理
    /// </summary>
    WithdrawalAcceptance,
    /// <summary>
    /// 提款被拒绝
    /// </summary>
    RefuseWithdrawal,
    /// <summary>
    /// 方案中奖
    /// </summary>
    SchemeWinning,
    /// <summary>
    /// 用户资料修改
    /// </summary>
    UserDataModification
}
#endregion

#region 消息模板
public class MessageTemplate
{
    /// <summary>
    /// 绑定手机号
    /// </summary>
    public static string BindPhone
    {
        get
        {
            return "尊敬的[UserName]，您的本次验证码为：[Code]，请勿泄露，有效时间为10分钟。";
        }
    }
    /// <summary>
    /// 绑定邮箱
    /// </summary>
    public static string BindEmail
    {
        get
        {
            return "尊敬的[UserName]，您的本次验证码为：[Code]，请勿泄露，有效时间为10分钟。";
        }
    }
    /// <summary>
    /// 找回密码
    /// </summary>
    public static string RetrievePassword
    {
        get
        {
            return "尊敬的[UserName]，您的本次验证码为：[Code]，请勿泄露，有效时间为10分钟。";
        }
    }
    /// <summary>
    /// 用户注册
    /// </summary>
    public static string UserRegistration
    {
        get
        {
            return "尊敬的[UserName]，您的注册验证码为：[Code]，请勿泄露，有效时间为10分钟。";
        }
    }
    /// <summary>
    /// 同意提款
    /// </summary>
    public static string WithdrawalAcceptance
    {
        get
        {
            return "尊敬的[UserName]，您申请的提款已被受理，详情请登陆网站个人中心查看。";
        }
    }
    /// <summary>
    /// 拒绝提款
    /// </summary>
    public static string RefuseWithdrawal
    {
        get
        {
            return "尊敬的[UserName]，您申请的提款已被拒绝，详情请登陆网站个人中心查看。";
        }
    }
    /// <summary>
    /// 中奖
    /// </summary>
    public static string SchemeWinning
    {
        get
        {
            return "尊敬的[UserName]，您购买的方案已中奖，方案号[SchemeNo]，详情请登陆网站个人中心查看。";
        }
    }
    /// <summary>
    /// 用户资料修改
    /// </summary>
    public static string UserDataModification
    {
        get
        {
            return "尊敬的[UserName]，您的个人资料信息修改成功，请注意保存。";
        }
    }
}
#endregion