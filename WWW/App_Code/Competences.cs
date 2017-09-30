using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Competences 的摘要说明
/// </summary>
public class Competences
{
    #region 最新权限变量

    #region 超级管理权限
    /// <summary>
    /// 1.0超级管理权限，拥有对网站的全部控制权。
    /// </summary>
    public const string Administrator = "Administrator";
    #endregion

    #region 内容管理
    /// <summary>
    /// 图片新闻
    /// </summary>
    public const string ImageNews = "ImageNews";
    /// <summary>
    /// 新闻资讯
    /// </summary>
    public const string News = "News";
    /// <summary>
    /// 站点公告
    /// </summary>
    public const string SiteNotice = "SiteNotice";
    /// <summary>
    /// 购彩大厅
    /// </summary>
    public const string BuyLotteryHall = "BuyLotteryHall";
    /// <summary>
    /// 名人管理
    /// </summary>
    public const string CelebManage = "CelebManage";
    /// <summary>
    /// 页脚管理
    /// </summary>
    public const string FooterManage = "FooterManage";
    /// <summary>
    /// 联系我们
    /// </summary>
    public const string Contact = "Contact";
    /// <summary>
    /// 站点图片管理
    /// </summary>
    public const string SiteImageManage = "SiteImageManage";

    /// <summary>
    /// APP图片轮播
    /// </summary>
    public const string AppImage = "AppImage";
    /// <summary>
    /// 协议
    /// </summary>
    public const string Protocol = "Protocol";
    /// <summary>
    /// 敏感关键词管理
    /// </summary>
    public const string SensitivityCharManage = "SensitivityCharManage";
    /// <summary>
    /// 客户端用户反馈
    /// </summary>
    public const string ClientUserFeedback = "ClientUserFeedback";
    #endregion

    #region 用户管理
    /// <summary>
    /// 用户一览表
    /// </summary>
    public const string UserList = "UserList";
    /// <summary>
    /// 用户账户明细
    /// </summary>
    public const string UserAccountDetails = "UserAccountDetails";

    /// <summary>
    /// 用户积分明细
    /// </summary>
    public const string UserIntegralDetail = "UserIntegralDetail";
    /// <summary>
    /// 为用户充值
    /// </summary>
    public const string UserRecharge = "UserRecharge";

    /// <summary>
    /// 用户彩金充值
    /// </summary>
    public const string UserAddHandsel = "UserAddHandsel";
    /// <summary>
    /// 用户登录日志
    /// </summary>
    public const string UserLoginLog = "UserLoginLog";
    /// <summary>
    /// 用户统计访问
    /// </summary>
    public const string UserLoginCount = "UserLoginCount";
    #endregion

    #region 彩票业务中心
    /// <summary>
    /// 玩法时间设置
    /// </summary>
    public const string PlayTimeSet = "PlayTimeSet";
    /// <summary>
    /// 期号管理
    /// </summary>
    public const string IssueManage = "IssueManage";
    /// <summary>
    /// 方案置顶
    /// </summary>
    public const string SchemeTop = "SchemeTop";
    /// <summary>
    /// 方案撤单
    /// </summary>
    public const string SchemeQuash = "SchemeQuash";
    /// <summary>
    /// 出票
    /// </summary>
    public const string PrintOut = "PrintOut";
    /// <summary>
    /// 录入开奖号码
    /// </summary>
    public const string WriteOpenNumber = "WriteOpenNumber";
    /// <summary>
    /// 开奖派奖
    /// </summary>
    public const string OpenAndDistribute = "OpenAndDistribute";
    /// <summary>
    /// 中奖积分比例设置
    /// </summary>
    public const string WinIntegralScaleSet = "WinIntegralScaleSet";
    /// <summary>
    /// 录入开奖公告
    /// </summary>
    public const string WriteOpenNotice = "WriteOpenNotice";
    /// <summary>
    /// 方案查询
    /// </summary>
    public const string SchemeQuery = "SchemeQuery";
    /// <summary>
    /// 中奖查询
    /// </summary>
    public const string WinQuery = "WinQuery";
    /// <summary>
    /// 追号和套餐查询
    /// </summary>
    public const string ChaseAndPackageQuery = "ChaseAndPackageQuery";
    /// <summary>
    /// 加奖设置
    /// </summary>
    public const string AwardSet = "AwardSet";
    /// <summary>
    /// 竞彩过关管理
    /// </summary>
    public const string JCPassManage = "JCPassManage";
    #endregion

    #region 消息管理
    /// <summary>
    /// 邮件参数管理
    /// </summary>
    public const string EmailParamManage = "EmailParamManage";
    /// <summary>
    /// 短信参数管理
    /// </summary>
    public const string SMSParamManage = "SMSParamManage";
    /// <summary>
    /// 消息模板
    /// </summary>
    public const string MessageTemplate = "MessageTemplate";
    /// <summary>
    /// 发出消息选项
    /// </summary>
    public const string SendMessageOption = "SendMessageOption";
    /// <summary>
    /// 发送Email
    /// </summary>
    public const string SendEmail = "SendEmail";
    /// <summary>
    /// 已发送Email
    /// </summary>
    public const string AlreadySendEmail = "AlreadySendEmail";
    /// <summary>
    /// 发送站内信
    /// </summary>
    public const string SendSiteMessage = "SendSiteMessage";
    /// <summary>
    /// 发送手机短信
    /// </summary>
    public const string SendSMS = "SendSMS";
    /// <summary>
    /// 已发送手机短信
    /// </summary>
    public const string AlreadySendSMS = "AlreadySendSMS";
    /// <summary>
    /// 站内信内容监控
    /// </summary>
    public const string SiteMessageListener = "SiteMessageListener";
    /// <summary>
    /// 系统监控日志
    /// </summary>
    public const string SystemListenerLog = "SystemListenerLog";
    #endregion

    #region 财务中心
    /// <summary>
    /// 用户充值明细
    /// </summary>
    public const string RechargeDetails = "RechargeDetails";

    /// <summary>
    /// 用户交易明细
    /// </summary>
    public const string BusinessDetails = "BusinessDetails";

    /// <summary>
    /// 用户中奖明细
    /// </summary>
    public const string WinDetails = "WinDetails";

    /// <summary>
    /// 用户提款明细
    /// </summary>
    public const string DrawingDetails = "DrawingDetails";

    /// <summary>
    /// 待付款用户
    /// </summary>
    public const string StayPaymentUser = "StayPaymentUser";

    /// <summary>
    /// 收支汇总
    /// </summary>
    public const string BalanceOfPayments = "BalanceOfPayments";
    /// <summary>
    /// 充值送彩金设置
    /// </summary>
    public const string HandselManage = "HandselManage";
    /// <summary>
    /// 处理用户提款申请
    /// </summary>
    public const string DrawMoneyApply = "DrawMoneyApply";
    #endregion

    #region 客户端消息推送
    /// <summary>
    /// 推送模板
    /// </summary>
    public const string PushTemplate = "PushTemplate";

    /// <summary>
    /// 推送选项
    /// </summary>
    public const string PushOption = "PushOption";

    /// <summary>
    /// 发送推送消息
    /// </summary>
    public const string SendPushMessage = "SendPushMessage";
    #endregion

    #region CPS权限
    /// <summary>
    /// CPS权限
    /// </summary>
    public const string CpsManage = "CpsManage";
    #endregion

    #region 系统管理
    /// <summary>
    /// 站点资料
    /// </summary>
    public const string SiteInfo = "SiteInfo";
    /// <summary>
    /// 系统参数设置
    /// </summary>
    public const string SystemParamSet = "SystemParamSet";
    /// <summary>
    /// 权限管理
    /// </summary>
    public const string CompetenceManage = "CompetenceManage";
    /// <summary>
    /// 支付参数设置
    /// </summary>
    public const string PayParamSet = "PayParamSet";
    /// <summary>
    /// APP自动更新
    /// </summary>
    public const string APPAutoUpdate = "APPAutoUpdate";
    /// <summary>
    /// 过滤条件设置
    /// </summary>
    public const string FilterWhereSet = "FilterWhereSet";
    #endregion

    #endregion

    public Users User;

    public string CompetencesList
    {
        get
        {
            if ((User == null) || (User.ID < 1))
            {
                throw new Exception("没有初始化 Competences 类的 User 变量");
            }

            return DAL.Functions.F_GetUserCompetencesList(User.ID);
        }
    }

    public Competences()
    {
        User = null;
    }

    public Competences(Users user)
    {
        User = user;
    }

    public bool this[string CompetenceName]
    {
        get
        {
            if ((User == null) || (User.ID < 1))
            {
                throw new Exception("没有初始化 Competences 类的 User 变量");
            }

            return (DAL.Functions.F_GetUserCompetencesList(User.ID).IndexOf("[" + CompetenceName + "]") >= 0);
        }
    }

    // 设置用户权限
    public int SetUserCompetences(string CompetencesList, string GroupsList, ref string ReturnDescription)
    {
        if ((User == null) || (User.ID < 1))
        {
            throw new Exception("没有初始化 Competences 类的 User 变量");
        }

        int ReturnValue = -1;
        ReturnDescription = "";

        int Result = DAL.Procedures.P_SetUserCompetences(User.Site.ID, User.ID, CompetencesList, GroupsList, ref ReturnValue, ref ReturnDescription);

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

    // 是否拥有权限列表需要的权限
    public bool IsOwnedCompetences(string RequestCompetencesList)
    {
        if ((User == null) || (User.ID < 1))
        {
            throw new Exception("没有初始化 Competences 类的 User 变量");
        }

        string UserCompetencesList = CompetencesList;

        if (UserCompetencesList.IndexOf("[" + Competences.Administrator + "]") >= 0)    // 拥有超级权限
        {
            return true;
        }
        if (!string.IsNullOrEmpty(RequestCompetencesList) && RequestCompetencesList.IndexOf("[") == -1)
        {
            RequestCompetencesList = "[" + RequestCompetencesList.Trim() + "]";
        }
        else
        {
            RequestCompetencesList = RequestCompetencesList.Trim();
        }
        if (RequestCompetencesList == "")
        {
            return true;
        }

        RequestCompetencesList = RequestCompetencesList.Replace("][", ",");
        RequestCompetencesList = RequestCompetencesList.Substring(1, RequestCompetencesList.Length - 2);

        string[] strs = RequestCompetencesList.Split(',');

        bool HaveThisCompetences = false;

        foreach (string str in strs)
        {
            if ((UserCompetencesList.IndexOf("[" + str + "]") >= 0) && (!HaveThisCompetences))
            {
                HaveThisCompetences = true;
            }
        }

        return HaveThisCompetences;
    }

    // 构建需要的权限列表字串
    public static string BuildCompetencesList(params string[] CompetenceList)
    {
        string Result = "";

        foreach (string str in CompetenceList)
        {
            Result += "[" + str + "]";
        }

        return Result;
    }
}
