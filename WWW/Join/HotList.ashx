<%@ WebHandler Language="C#" Class="HotList" %>

using System;
using System.Web;
using System.Data;

using System.Text;

using Shove.Database;

/// <summary>
/// 首页调用，热门方案列表部分
/// </summary>
public class HotList : IHttpHandler
{
    Sites _sites = new Sites()[1];
    public void ProcessRequest(HttpContext context)
    {
        //不让浏览器缓存
        context.Response.Buffer = true;
        context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
        context.Response.AddHeader("pragma", "no-cache");
        context.Response.AddHeader("cache-control", "");
        context.Response.CacheControl = "no-cache";
        context.Response.ContentType = "text/plain";
        string UserName = context.Request["UserName"].ToString();
        StringBuilder sql = new StringBuilder();

        sql.AppendLine("select top 11 * from (select a.ID,b.Name as InitiateName,AtTopStatus,b.Level,Money, c.Name as PlayTypeName, a.Multiple, Share, BuyedShare, Schedule, c.LotteryID,AssureMoney,")
                .AppendLine("  InitiateUserID, QuashStatus, PlayTypeID, Buyed, SecrecyLevel, EndTime, d.IsOpened,e.Name AS LotteryName, LotteryNumber,case Schedule when 100 then 1 else 0 end as IsFull ")
                .AppendLine("from")
                .AppendLine("	(")
                .AppendLine("		select ID, EndTime,IsOpened from T_Isuses where getdate() between StartTime and EndTime AND IsOpened=0")
                .AppendLine("	) as d")
                .AppendLine("inner join T_Schemes a on a.IsuseID = d.ID  and a.isOpened = 0 and a.Share > 1 and a.buyed = 0 and a.QuashStatus = 0 and a.Schedule < 100")
                .AppendLine("inner join T_Users b on a.InitiateUserID = b.ID inner join T_PlayTypes c on a.PlayTypeID = c.ID")
                .AppendLine("inner join T_Lotteries e on c.LotteryID=e.ID) HeMai")
                .AppendLine("where HeMai.ID not in(select SchemeID From T_SchemesContent sc where sc.StopSellingTime<=GETDATE() and sc.SchemeID=HeMai.ID)")
                .AppendLine("order by HeMai.Schedule desc, HeMai.AtTopStatus desc,HeMai.InitiateName desc,HeMai.id desc,HeMai.QuashStatus asc");

        DataTable dt = MSSQL.Select(sql.ToString());

        //DataTable dt_List = Transform(dt);
        DataTable dt_List = dt;

        if (dt_List == null)
        {
            return;
        }

        StringBuilder sbContent = new StringBuilder();
        StringBuilder sbContent1 = new StringBuilder();
        float Money = 0;
        int Share = 0;
        double Schedule = 0;
        double BuyedShare = 0;
        string Surplus = "";
        string name = "";
        foreach (DataRow dr in dt_List.Rows)
        {
            Money = Shove._Convert.StrToFloat(dr["Money"].ToString(), 0);
            Share = Shove._Convert.StrToInt(dr["Share"].ToString(), 0);
            Schedule = Shove._Convert.StrToDouble(dr["Schedule"].ToString(), 0);
            BuyedShare = Shove._Convert.StrToInt(dr["BuyedShare"].ToString(), 0);

            Surplus = (Share - BuyedShare).ToString();

            if (Surplus.IndexOf(".") >= 0)
            {
                Surplus = (Shove._Convert.StrToInt(Surplus.Substring(0, Surplus.IndexOf(".")), 0) + 1).ToString();
            }

            if (dr["AtTopStatus"].ToString().Equals("2"))
            {
                if (!string.IsNullOrEmpty(UserName) && UserName.Equals(dr["InitiateName"].ToString()))
                {
                    name = dr["InitiateName"].ToString();
                }
                else
                {
                    name = Users.UserControlHiedAndShow(dr["InitiateName"].ToString());
                }
                if (Shove._Convert.StrToInt(dr["SecrecyLevel"].ToString(), 0) > 0)//方案是否保密
                {
                    if (_sites.Opt_IsShowUserUrl == 0)
                    {
                        sbContent1.Append("<tr><td><img src=\"/Images/lock-close-ico.png\" title=\"" + GetSecrecyLevel(dr["SecrecyLevel"].ToString()) + "\" /></td><td class=\"name\">" + name + "</td>");
                    }
                    else
                    {
                        sbContent1.Append("<tr><td><img src=\"/Images/lock-close-ico.png\" title=\"" + GetSecrecyLevel(dr["SecrecyLevel"].ToString()) + "\" /></td><td class=\"name\"><a target=\"_blank\" href=\"Home/Web/Score.aspx?id=" + dr["InitiateUserID"].ToString() + "&LotteryID=" + dr["LotteryID"].ToString() + "\">" + name + "</a></td>");
                    }
                }
                else
                {
                    if (_sites.Opt_IsShowUserUrl == 0)
                    {
                        sbContent1.Append("<tr><td><img src=\"/Images/lock-open-ico.png\" title=\"" + GetSecrecyLevel(dr["SecrecyLevel"].ToString()) + "\" /></td><td class=\"name\">" + name + "</td>");
                    }
                    else
                    {
                        sbContent1.Append("<tr><td><img src=\"/Images/lock-open-ico.png\" title=\"" + GetSecrecyLevel(dr["SecrecyLevel"].ToString()) + "\" /></td><td class=\"name\"><a target=\"_blank\" href=\"Home/Web/Score.aspx?id=" + dr["InitiateUserID"].ToString() + "&LotteryID=" + dr["LotteryID"].ToString() + "\">" + name + "</a></td>");
                    }
                }
                //战绩显示
                sbContent1.Append("<td>" + PF.GetLevel(dr["Level"].ToString()) + "</td>");

                sbContent1.Append("<td>" + dr["LotteryName"].ToString() + "</td>");

                if (Shove._Convert.StrToDouble(dr["AssureMoney"].ToString(), 0) > 0)
                {
                    sbContent1.Append("<td>" + dr["Schedule"].ToString() + "%+" + (Shove._Convert.StrToDouble(dr["AssureMoney"].ToString(), 0) / Money * 100).ToString("0.00") + "%<strong>保</strong></td>");
                }
                else
                {
                    sbContent1.Append("<td>" + dr["Schedule"].ToString() + "%</td>");
                }
                sbContent1.Append("<td><em class=\"col-red\">" + Money.ToString() + "</em>元 / <em class=\"col-red\">" + (Money / Share).ToString() + "</em>元</td>");
                sbContent1.Append("<td class=\"buy-number\"><span id=\"spSurplus_" + dr["ID"].ToString() + "\" style=\"display:none;\">" + Surplus + "</span>");
                sbContent1.Append("<span class=\"minus\"></span>");
                sbContent1.Append("<span id=\"spanShareMoney_" + dr["ID"].ToString() + "\" style=\"display:none;\">" + (Money / Share).ToString() + "</span>");
                if (Surplus.Length >= 4)
                {
                    sbContent1.Append("<input class=\"group_input\" style='width:80px;' type=\"text\" id=\"txtShare_" + dr["ID"].ToString() + "\" value=\"剩" + Surplus + "份\" style='ime-mode: disabled; color:#999999;' onfocus=\"setFocus(this)\" onblur=\"setBlur(this)\" />");
                }
                else
                {
                    sbContent1.Append("<input class=\"group_input\" style='width:80px;' type=\"text\" id=\"txtShare_" + dr["ID"].ToString() + "\" value=\"剩" + Surplus + "份\" style='ime-mode: disabled; color:#999999;' onfocus=\"setFocus(this)\" onblur=\"setBlur(this)\" />");
                }
                sbContent1.Append("<span class=\"add\"></span></td>");
                sbContent1.Append("<td class=\"t_r\"><span><a rel=\"" + dr["LotteryID"].ToString() + "\" class=\"btn buy\" id=\"joinBuy\" mid=\"" + dr["ID"].ToString() + "\">购买</a></span><a href=Home/Room/Scheme.aspx?id=" + dr["ID"].ToString() + " class=\"btn\" target=\"_blank\" rel=\"nofollow\">详情</a></td></tr>");
            }
            else
            {
                if (!string.IsNullOrEmpty(UserName) && UserName.Equals(dr["InitiateName"].ToString()))
                {
                    name = dr["InitiateName"].ToString();
                }
                else
                {
                    name = Users.UserControlHiedAndShow(dr["InitiateName"].ToString());
                }
                if (Shove._Convert.StrToInt(dr["SecrecyLevel"].ToString(), 0) > 0)//方案是否保密
                {
                    if (_sites.Opt_IsShowUserUrl == 0)
                    {
                        sbContent.Append("<tr><td><img src=\"/Images/lock-close-ico.png\" title=\"" + GetSecrecyLevel(dr["SecrecyLevel"].ToString()) + "\" /></td><td class=\"name\">" + name + "</td>");
                    }
                    else
                    {
                        sbContent.Append("<tr><td><img src=\"/Images/lock-close-ico.png\" title=\"" + GetSecrecyLevel(dr["SecrecyLevel"].ToString()) + "\" /></td><td class=\"name\"><a target=\"_blank\" href=\"Home/Web/Score.aspx?id=" + dr["InitiateUserID"].ToString() + "&LotteryID=" + dr["LotteryID"].ToString() + "\">" + name + "</a></td>");
                    }
                }
                else
                {
                    if (_sites.Opt_IsShowUserUrl == 0)
                    {
                        sbContent.Append("<tr><td><img src=\"/Images/lock-open-ico.png\" title=\"" + GetSecrecyLevel(dr["SecrecyLevel"].ToString()) + "\" /></td><td class=\"name\">" + name + "</td>");
                    }
                    else
                    {
                        sbContent.Append("<tr><td><img src=\"/Images/lock-open-ico.png\" title=\"" + GetSecrecyLevel(dr["SecrecyLevel"].ToString()) + "\" /></td><td class=\"name\"><a target=\"_blank\" href=\"Home/Web/Score.aspx?id=" + dr["InitiateUserID"].ToString() + "&LotteryID=" + dr["LotteryID"].ToString() + "\">" + name + "</a></td>");
                    }
                }
                //战绩显示
                sbContent.Append("<td>" + PF.GetLevel(dr["Level"].ToString()) + "</td>");

                sbContent.Append("<td>" + dr["LotteryName"].ToString() + "</td>");

                if (Shove._Convert.StrToDouble(dr["AssureMoney"].ToString(), 0) > 0)
                {
                    sbContent.Append("<td>" + dr["Schedule"].ToString() + "%+" + (Shove._Convert.StrToDouble(dr["AssureMoney"].ToString(), 0) / Money * 100).ToString("0.00") + "%<strong>保</strong></td>");
                }
                else
                {
                    sbContent.Append("<td>" + dr["Schedule"].ToString() + "%</td>");
                }
                sbContent.Append("<td><em class=\"col-red\">" + Money.ToString() + "</em>元 / <em class=\"col-red\">" + (Money / Share).ToString() + "</em>元</td>");
                sbContent.Append("<td class=\"buy-number\"><span id=\"spSurplus_" + dr["ID"].ToString() + "\" style=\"display:none;\">" + Surplus + "</span>");
                sbContent.Append("<span class=\"minus\"></span>");
                sbContent.Append("<span id=\"spanShareMoney_" + dr["ID"].ToString() + "\" style=\"display:none;\">" + (Money / Share).ToString() + "</span>");
                if (Surplus.Length >= 4)
                {
                    sbContent.Append("<input class=\"group_input\" style='width:80px;' type=\"text\" id=\"txtShare_" + dr["ID"].ToString() + "\" value=\"剩" + Surplus + "份\" style='ime-mode: disabled; color:#999999;' onfocus=\"setFocus(this)\" onblur=\"setBlur(this)\" />");
                }
                else
                {
                    sbContent.Append("<input class=\"group_input\" style='width:80px;' type=\"text\" id=\"txtShare_" + dr["ID"].ToString() + "\" value=\"剩" + Surplus + "份\" style='ime-mode: disabled; color:#999999;' onfocus=\"setFocus(this)\" onblur=\"setBlur(this)\" />");
                }
                sbContent.Append("<span class=\"add\"></span></td>");
                sbContent.Append("<td class=\"t_r\"><span><a rel=\"" + dr["LotteryID"].ToString() + "\" class=\"btn buy\" id=\"joinBuy\" mid=\"" + dr["ID"].ToString() + "\">购买</a></span><a href=Home/Room/Scheme.aspx?id=" + dr["ID"].ToString() + " class=\"btn\" target=\"_blank\" rel=\"nofollow\">详情</a></td></tr>");
            }
        }

        context.Response.Write("{\"message\":\"" + (sbContent1.ToString() + sbContent.ToString()).Replace("\"", "\\\"") + "\"}");
    }

    private string GetSecrecyLevel(string secrecyLevel)
    {
        string level = "";
        switch (secrecyLevel)
        {
            case "0":
                level = "方案公开";
                break;
            case "1":
            case "2":
            case "3":
                level = "方案保密";
                break;
        }
        return level;
    }

    /// <summary>
    /// 转换表格中的竞彩足球，竞彩篮球格式
    /// </summary>
    /// <param name="tb">转换的表</param>
    /// <returns></returns>
    private DataTable Transform(DataTable dt)
    {

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            int lotteryID = Shove._Convert.StrToInt(dt.Rows[i]["lotteryID"].ToString(), -1);

            //竞彩足球或竞彩篮球
            DataRow[] rows = null;
            if (lotteryID == 45 || lotteryID == 72 || lotteryID == 73)
            {
                DataTable DT = new DAL.Tables.T_SchemesMixcast().Open(" top 1 LotteryNumber ", "SchemeId=" + dt.Rows[i]["ID"].ToString() + "", "");

                string LotteryNumber = DT.Rows[0]["LotteryNumber"].ToString();

                DateTime EndTime = Shove._Convert.StrToDateTime(new AppGateway_ashx().GetScriptResTable(LotteryNumber), DateTime.Now.AddDays(-1).ToString());

                if (EndTime.CompareTo(DateTime.Now) < 1)
                {
                    dt.Rows.Remove(dt.Rows[i]);
                    //i--;
                    continue;
                }
                // if (PlayTypeID >= 7201 && PlayTypeID <= 7204)
                //  {
                LotteryNumber = new AppGateway_ashx().Football(LotteryNumber);
                //}
                //else
                //{
                //    LotteryNumber = Basketball(LotteryNumber);
                //}

                rows = dt.Select("ID = '" + dt.Rows[i]["ID"].ToString() + "'", "ID");
                rows[0]["LotteryNumber"] = LotteryNumber;
                dt.AcceptChanges();

            }
        }
        return dt;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}