using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Shove.Database;
using System.Text.RegularExpressions;


public partial class JND28_Default : System.Web.UI.Page
{
    public int LotteryID;
    public string NewsInfo = "";
    public Sites _Site;
    protected void Page_Load(object sender, EventArgs e)
    {
        _Site = new Sites()[1];

        if (_Site == null)
        {
            PF.GoError(ErrorNumber.Unknow, "域名无效，限制访问", "SitePageBase");

            return;
        }

        LotteryID = 98;

        bool result = false;
        string UseLotteryList = Shove._Web.Cache.GetCacheAsString("Site_UseLotteryList" + _Site.ID, "");
        string[] Lottery = null;

        if (UseLotteryList == "")
        {
            UseLotteryList = DAL.Functions.F_GetUsedLotteryList(_Site.ID);

            if (UseLotteryList != "")
            {
                Shove._Web.Cache.SetCache("Site_UseLotteryList" + _Site.ID, UseLotteryList, 5);
            }
        }

        Lottery = UseLotteryList.Split(',');
        for (int i = 0; i < Lottery.Length; i++)
        {
            if (LotteryID.ToString().Equals(Lottery[i]))
            {
                result = true;
                break;
            }
        }

        if (!result)
        {
            this.divSubmit.Visible = false;
            this.divunSubmit.Visible = true;
        }
        else
        {
            this.divSubmit.Visible = true;
            this.divunSubmit.Visible = false;
        }

        if (!IsPostBack)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(JND28_Default), this.Page);
            BindWinRakking();
            GetNewsInfo(98);

            BinOpt_InitiateSchemeBonusScale();
            //发起合买时发起人最少认购数
            double number = Shove._Convert.StrToDouble(_Site.SiteOptions["Opt_InitiateSchemeMinBuyAndAssureScale"].ToString(""), 0);
            this.labInitiateSchemeMinBuyAndAssureScale.Value = number.ToString();
        }

    }

    private void BinOpt_InitiateSchemeBonusScale()
    {
        double out3 = Shove._Convert.StrToDouble(_Site.SiteOptions["Opt_InitiateSchemeBonusScale"].ToString(""), 0) * 100;
        int out1 = Shove._Convert.StrToInt(out3.ToString(), 0);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i <= 10; i++)
        {
            if (i == 0)
            {
                if (out1 == 0)
                {
                    sb.Append("<li class=\"on\" checked=\"checked\" value=\"" + i + "\">无</li>");
                }
                else
                {
                    sb.Append("<li value=\"" + i + "\">无</li>");
                }
            }
            else
            {
                if (out1 == i)
                {
                    sb.Append("<li class=\"on\" checked=\"checked\" value=\"" + i + "\">" + i + "%</li>");
                }
                else
                {
                    sb.Append("<li  value=\"" + i + "\">" + i + "%</li>");
                }
            }
        }
        ul_cway_list.InnerHtml = sb.ToString();
    }

    /// <summary>
    /// 获取资讯信息
    /// </summary>
    /// <param name="LotteryID"></param>
    /// <returns></returns>
    public void GetNewsInfo(int LotteryID)
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_LotteryNews" + LotteryID.ToString());

        StringBuilder sb = new StringBuilder();

        if (dt == null || dt.Rows.Count == 0)
        {
            dt = new DAL.Tables.T_News().Open("top 5 ID,Title,Content", "isShow = 1 and TypeID = 111074", "isCommend,DateTime desc");

            if (dt != null && dt.Rows.Count > 0)
            {
                Shove._Web.Cache.SetCache("DataCache_LotteryNews" + LotteryID.ToString(), dt, 120);
            }
        }

        foreach (DataRow dr in dt.Rows)
        {
            Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = regex.Match(dr["Content"].ToString());

            if (m.Success)
            {
                sb.Append("<p><a  href=\"" + dr["Content"].ToString() + "\" id=\"" + dr["ID"].ToString() + "\" target=\"_blank\">");
                sb.Append(dr["Title"].ToString());
            }
            else
            {
                sb.Append("<p><a  href=\"/News/View.aspx?ID=" + dr["ID"].ToString() + "\" id=\"" + dr["ID"].ToString() + "\" target=\"_blank\">");
                sb.Append(dr["Title"].ToString());
            }

            sb.AppendLine("</a></p>");
        }
        if (string.IsNullOrEmpty(sb.ToString()))
        {
            NewsInfo = "<p>暂无资讯</p>";
        }
        else
        {
            NewsInfo = sb.ToString();
        }
    }

    private DataTable GetData()
    {
        string CacheKey = "BJXY28_OpenWinInfo";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            dt = Shove.Database.MSSQL.Select("select Top 100 [Name], EndTime, WinLotteryNumber  from T_Isuses where LotteryID = 98 and EndTime < GETDATE() and ISNULL(WinLotteryNumber,'') <> '' and IsOpened = 1 order by EndTime Desc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return null;
            }

            Shove._Web.Cache.SetCache(CacheKey, dt, 30);
        }

        return dt;
    }

    public string WinRanking { get; set; }

    private void BindWinRakking()
    {

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("SQLDefault_WinRanking99");

        if (dt == null)
        {
            string sql = @"SELECT TOP 5 A.*,B.LotteryID,C.Name AS LotteryName,D.Name AS UserName FROM T_Schemes AS A
                    INNER JOIN T_PlayTypes AS B ON A.PlayTypeID=B.ID INNER JOIN T_Lotteries AS C ON B.LotteryID=C.ID
                    INNER JOIN T_Users AS D ON A.InitiateUserID=D.ID WHERE A.isOpened=1 AND A.WinMoney > 0 AND B.LotteryID=98 ORDER BY A.UpdateDatetime DESC";

            dt = Shove.Database.MSSQL.Select(sql);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(130)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count > 0)
            {
                // 1200M
                Shove._Web.Cache.SetCache("SQLDefault_WinRanking", dt, 1200);
            }
        }

        WinRanking = "";

        if (dt.Rows.Count < 1)
        {
            WinRanking = "<p>暂无中奖记录</p>";
            return;
        }

        foreach (DataRow dr in dt.Rows)
        {
            string Name = dr["UserName"].ToString();
            Name = Users.UserControlHiedAndShow(Name);

            WinRanking += "<p><span>" + Name + "</span>喜中 <b class=\"col-red\">" + Convert.ToDouble(dr["WinMoneyNoWithTax"].ToString()).ToString("F2") + "</b> 元</p>";
        }
        if ("" == WinRanking)
        {
            WinRanking = "<p style=\"height:30px; line-height:30px;\">暂无中奖记录</p>";
        }
    }
    /// <summary>
    /// 获取用户彩金金额
    /// </summary>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public string GetHandselAmount()
    {

        return Users.GetCurrentUser(1).HandselAmount.ToString("0.00");
    }
}