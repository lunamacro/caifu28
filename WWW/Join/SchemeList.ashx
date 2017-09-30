<%@ WebHandler Language="C#" Class="SchemeList" %>

using System;
using System.Web;
using System.Data;

using System.Text;

using Shove.Database;

/// <summary>
/// 首页调用，方案列表部分
/// </summary>
public class SchemeList : IHttpHandler
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

        string lotteryID = "72";

        if (!string.IsNullOrEmpty(context.Request["lotteryID"]))
        {
            lotteryID = Shove._Web.Utility.GetRequest(context, "lotteryID");
        }

        int TopNum = 10;

        if (!string.IsNullOrEmpty(context.Request["TopNum"]))
        {
            TopNum = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest(context, "TopNum"), 10);
        }

        string Key = "join_SchemeList_lotteryID" + lotteryID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("select top " + TopNum.ToString() + " a.ID,b.Name as InitiateName,AtTopStatus,b.Level,Money, c.Name as PlayTypeName, a.Multiple, Share, BuyedShare, Schedule, c.LotteryID,AssureMoney, ")
                   .AppendLine("	InitiateUserID, QuashStatus, PlayTypeID, Buyed, SecrecyLevel, EndTime, d.IsOpened, LotteryNumber,case Schedule when 100 then 1 else 0 end as IsFull ")
                   .AppendLine("from")
                   .AppendLine("	(")
                   .AppendLine("		select top 1 ID, EndTime,IsOpened from T_Isuses where getdate() between StartTime and EndTime and LotteryID =" + lotteryID)
                   .AppendLine("	) as d")
                   .AppendLine("inner join T_Schemes a on a.IsuseID = d.ID  and a.isOpened = 0 and a.Share > 1 and a.buyed = 0 and a.QuashStatus = 0 and a.Schedule < 100")
                   .AppendLine("inner join T_Users b on a.InitiateUserID = b.ID")
                   .AppendLine("inner join T_PlayTypes c on a.PlayTypeID = c.ID")
                   .AppendLine("order by a.AtTopStatus desc,a.id desc,a.QuashStatus asc, a.Schedule desc");

            dt = MSSQL.Select(sql.ToString());

            if (dt == null)
            {
                return;
            }

            if (dt.Rows.Count > 0)
            {
                Shove._Web.Cache.SetCache(Key, dt, 0);
            }
        }

        StringBuilder sbContent = new StringBuilder();

        float Money = 0;
        int Share = 0;
        double Schedule = 0;
        double BuyedShare = 0;
        string Surplus = "";

        for (int i = 0; i < TopNum; i++)
        {


            if (i >= dt.Rows.Count)
            {
                sbContent.Append("<tr><td><span></span></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");

                continue;
            }
            int LotteryID = Shove._Convert.StrToInt(dt.Rows[i]["LotteryID"].ToString(), 1);

            if (LotteryID == 45 || LotteryID == 72 || LotteryID == 73)
            {
                DateTime EndTime = Shove._Convert.StrToDateTime(GetScriptResTable(dt.Rows[i]["LotteryNumber"].ToString()), DateTime.Now.AddDays(-1).ToString());

                if (EndTime.CompareTo(DateTime.Now) < 1)
                {
                    continue;
                }
            }
            Money = Shove._Convert.StrToFloat(dt.Rows[i]["Money"].ToString(), 0);
            Share = Shove._Convert.StrToInt(dt.Rows[i]["Share"].ToString(), 0);
            Schedule = Shove._Convert.StrToDouble(dt.Rows[i]["Schedule"].ToString(), 0);
            BuyedShare = Shove._Convert.StrToInt(dt.Rows[i]["BuyedShare"].ToString(), 0);

            Surplus = (Share - BuyedShare).ToString();

            if (Surplus.IndexOf(".") >= 0)
            {
                Surplus = (Shove._Convert.StrToInt(Surplus.Substring(0, Surplus.IndexOf(".")), 0) + 1).ToString();
            }

            sbContent.Append("<tr><td><span>" + (i + 1).ToString() + "</span></td>");
            sbContent.Append("<td>" + Users.UserControlHiedAndShow(dt.Rows[i]["InitiateName"].ToString()) + "</td>");
            if (_sites.Opt_IsShowUserUrl == 0)
            {
                sbContent.Append("<td><div style=background-image:url(/Join/Images/gold.gif); width:" + (9 * Shove._Convert.StrToInt(dt.Rows[i]["Level"].ToString(), 0)).ToString() + "px;background-repeat:repeat-x;></div></td>");
            }
            else
            {
                sbContent.Append("<td><div style=background-image:url(/Join/Images/gold.gif); width:" + (9 * Shove._Convert.StrToInt(dt.Rows[i]["Level"].ToString(), 0)).ToString() + "px;background-repeat:repeat-x;></div><a href=\"../Home/Web/Score.aspx?id=" + dt.Rows[i]["InitiateUserID"].ToString() + "&LotteryID=" + dt.Rows[i]["LotteryID"].ToString() + "\" target=\"_blank\">查看</a></td>");
            }
            sbContent.Append("<td><span>" + dt.Rows[i]["Multiple"].ToString() + "</span> 倍</td>");
            sbContent.Append("<td><span>" + Money.ToString() + "</span> 元</td>");
            sbContent.Append("<td><span id=\"spanShareMoney_" + dt.Rows[i]["ID"].ToString() + "\">" + (Money / Share).ToString() + "</span> 元</td>");
            if (dt.Rows[i]["Schedule"].ToString() == "100")
            {
                sbContent.Append("<td><span>满员</span></td>");
            }
            else if (dt.Rows[i]["QuashStatus"].ToString() != "0")
            {
                sbContent.Append("<td><span>已撤单</span></td>");
            }
            else
            {
                if (Shove._Convert.StrToDouble(dt.Rows[i]["AssureMoney"].ToString(), 0) > 0)
                {
                    sbContent.Append("<td><span>" + dt.Rows[i]["Schedule"].ToString() + "%+<span class=red>" + (Shove._Convert.StrToDouble(dt.Rows[i]["AssureMoney"].ToString(), 0) / Money * 100).ToString("0.00") + "%(保)</span></span></td>");
                }
                else
                {
                    sbContent.Append("<td><span>" + dt.Rows[i]["Schedule"].ToString() + "%</span></td>");
                }
            }
            sbContent.Append("<td><span id=\"spanSurplus_" + dt.Rows[i]["ID"].ToString() + "\">" + Surplus + "</span> 份");

            if (dt.Rows[i]["QuashStatus"].ToString() != "0" || dt.Rows[i]["Schedule"].ToString() == "100")
            {
                sbContent.Append("<td>--</td>");
                sbContent.Append("<td><a href=\"/Home/Room/Scheme.aspx?id=" + dt.Rows[i]["ID"].ToString() + "\" target=_blan title=点击查看方案详细信息>详情</a></td></tr>");
            }
            else
            {
                sbContent.Append("<td>");
                sbContent.Append("<img align=\"absmiddle\" class=\"jian\" onclick=\"return MReduction(" + dt.Rows[i]["ID"].ToString() + ");\" src=\"/Home/Room/Images/tmimg.gif\" /> ");
                sbContent.Append("<input type=\"text\" value=\"1\" size=\"4\" style=\"width: 25px;\" id=\"tbShare_" + dt.Rows[i]["ID"].ToString() + "\" class=\"Share\" /> ");
                sbContent.Append("<img align=\"absmiddle\" class=\"jia\" onclick=\"return MAddition(" + dt.Rows[i]["ID"].ToString() + ");\" src=\"/Home/Room/Images/tmimg.gif\" /></td>");
                sbContent.Append("</td>");
                sbContent.Append("<td><a href=\"javascript:void(0);\" class=\"join\"><img src=\"/Join/images/btn_cy.gif\" alt=\"参与\" align=\"middle\" mid=\"" + dt.Rows[i]["ID"].ToString() + "\" /></a>&nbsp;<a href=\"/Home/Room/Scheme.aspx?id=" + dt.Rows[i]["ID"].ToString() + "\" target=_blan title=点击查看方案详细信息>详情</a></td></tr>");
            }
        }

        DataTable dtNew = new DataTable();
        dtNew.Columns.Add("Content", typeof(System.String));

        DataRow drNew = dtNew.NewRow();
        drNew["Content"] = sbContent.ToString();
        dtNew.Rows.Add(drNew);

        dtNew.AcceptChanges();

        DataSet ds = new DataSet();

        ds.Tables.Add(dtNew);

        string jsonData = JsonHelper.GetJsonByDataset(ds);

        context.Response.Write(jsonData);
    }
    public string GetScriptResTable(string val)
    {
        try
        {
            val = val.Trim();

            int Istart, Ilen;

            GetStrScope(val, "[", "]", out Istart, out Ilen);

            string matchlist = val.Substring(Istart + 1, Ilen - 1);

            string type = val.Split(';')[0];

            if (type.Substring(0, 2) != "45" && type.Substring(0, 2) != "72" && type.Substring(0, 2) != "73")
            {
                return val;
            }

            string Matchids = "";
            string MatchListDan = "";

            if (val.Split(';').Length == 4)
            {
                MatchListDan = matchlist.Split(']')[0];

                foreach (string match in MatchListDan.Split('|'))
                {
                    Matchids += match.Split('(')[0] + ",";
                }
            }

            foreach (string match in matchlist.Split('|'))
            {
                Matchids += match.Split('(')[0] + ",";
            }

            if (Matchids.EndsWith(","))
            {
                Matchids = Matchids.Substring(0, Matchids.Length - 1);
            }

            DataTable table = null;

            if (type.Substring(0, 2) == "72")
            {
                table = new DAL.Tables.T_Match().Open("StopSellingTime", "id in (" + Matchids + ")", " StopSellingTime ");
            }
            if (type.Substring(0, 2) == "45")
            {
                table = new DAL.Tables.T_BJSingle().Open("StopSellTime", "MatchID in (" + Matchids + ")", " StopSellTime ");
            }
            if (type.Substring(0, 2) == "73")
            {
                table = new DAL.Tables.T_MatchBasket().Open("StopSellingTime", "id in (" + Matchids + ")", " StopSellingTime");
            }

            if (table == null || table.Rows.Count < 1)
            {
                return "";
            }

            DataTable dtPlayType = new DAL.Tables.T_PlayTypes().Open("SystemEndAheadMinute", "ID=" + type.Substring(0, 4), "");

            if (dtPlayType == null)
            {
                return "";
            }

            if (dtPlayType.Rows.Count < 1)
            {
                return "";
            }

            return Shove._Convert.StrToDateTime(table.Rows[0][0].ToString(), DateTime.Now.AddDays(-1).ToString()).AddMinutes(Shove._Convert.StrToInt(dtPlayType.Rows[0]["SystemEndAheadMinute"].ToString(), 0) * -1).ToString();
        }
        catch
        {
            return "";
        }
    }
    public void GetStrScope(string str, string strStart, string strEnd, out int IStart, out int ILen)
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
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}