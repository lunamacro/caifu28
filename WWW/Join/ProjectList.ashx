<%@ WebHandler Language="C#" Class="ProjectList" %>

using System;
using System.Web;
using System.Data;

using System.Text;

using Shove.Database;

public class ProjectList : IHttpHandler
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

        int pageindex;
        int.TryParse(context.Request["p"], out pageindex);

        string order = "";
        if (!string.IsNullOrEmpty(context.Request["orderby"]))
        {
            string[] strarr = context.Request["orderby"].ToString().Split('_');
            if (strarr[1] == "0")
                order = strarr[0] + " asc";
            else
                order = strarr[0] + " desc";

        }

        if (pageindex == 0)
            pageindex = 1;

        int PageNum = 20;
        if (!string.IsNullOrEmpty(context.Request["EachPageNum"]))
        {
            PageNum = Shove._Convert.StrToInt(context.Request["EachPageNum"].ToString(), 20);
        }

        string Condition = "";
        string PlayTypeID = "";
        string Name = "";
        string State = "";
        string lotteryID = "";
        string SchemeBonusScale = "";
        Condition += " Buyed=0";
        //方案状态
        if (!string.IsNullOrEmpty(context.Request["State"]))
        {
            State = Shove._Web.Utility.GetRequest(context, "State");

            switch (State)
            {
                case "-1":
                    Condition += "";
                    break;
                case "1":
                    Condition += " and Schedule < 100 and QuashStatus = 0";
                    break;
                case "2":
                    Condition += " and QuashStatus <> 0";
                    break;
                case "100":
                    Condition += " and Schedule >= 100";
                    break;
                default:
                    break;
            }
        }
        else
        {
            Condition += " and Schedule < 100 and QuashStatus = 0";
        }

        //佣金比例
        if (!string.IsNullOrEmpty(context.Request["SchemeBonusScale"]))
        {
            SchemeBonusScale = Shove._Web.Utility.GetRequest(context, "SchemeBonusScale");
            if (SchemeBonusScale != "-1")
            {
                Condition += " and SchemeBonusScale<=" + SchemeBonusScale;
            }
        }

        if (!string.IsNullOrEmpty(context.Request["PlayTypeID"]))
        {
            if (!string.IsNullOrEmpty(Condition))
            {
                Condition += " and ";
            }

            PlayTypeID = Shove._Web.Utility.GetRequest(context, "PlayTypeID");
            Condition += "PlayTypeID=" + PlayTypeID;
        }

        if (!string.IsNullOrEmpty(context.Request["Name"]))
        {
            if (!string.IsNullOrEmpty(Condition))
            {
                Condition += " and ";
            }

            Name = Shove._Web.Utility.GetRequest(context, "Name");
            Condition += "InitiateName like '%" + Name + "%'";
        }

        if (!string.IsNullOrEmpty(context.Request["lotteryID"]))
        {
            lotteryID = Shove._Web.Utility.GetRequest(context, "lotteryID");
        }

        /*string Key = "join_ProjectList_lotteryID" + lotteryID.ToString();

         DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

         if (dt == null)
         {
             StringBuilder sql = new StringBuilder();

             sql.AppendLine("select a.ID,a.SchemeBonusScale,b.Name as InitiateName,AtTopStatus,b.Level,Money, c.Name as PlayTypeName,c.LotteryId as LotteryID, a.Multiple, Share, BuyedShare, Schedule, AssureMoney, ")
                    .AppendLine("	InitiateUserID, QuashStatus, PlayTypeID, Buyed, SecrecyLevel, EndTime, d.IsOpened, LotteryNumber,case Schedule when 100 then 1 else 0 end as IsFull ")
                    .AppendLine("from")
                    .AppendLine("	(")
                    .AppendLine("		select top 1 ID, EndTime,IsOpened from T_Isuses where getdate() between StartTime and EndTime and LotteryID =" + lotteryID)
                    .AppendLine("	) as d")
                    .AppendLine("inner join T_Schemes a on a.IsuseID = d.ID  and a.isOpened = 0 and a.Share > 1")
                    .AppendLine("inner join T_Users b on a.InitiateUserID = b.ID")
                    .AppendLine("inner join T_PlayTypes c on a.PlayTypeID = c.ID")
                    .AppendLine("order by c.Sort desc, a.QuashStatus asc,IsFull asc, a.AtTopStatus desc, a.Schedule desc");

             dt = MSSQL.Select(sql.ToString());

             if (dt == null)
             {
                 return;
             }

             if (dt.Rows.Count > 0)
             {
                 Shove._Web.Cache.SetCache(Key, dt, 0);
             }
         }*/
        StringBuilder sql = new StringBuilder();
        if (lotteryID != "-1")
        {
            sql.AppendLine("select a.AtTopStatus,a.ID,a.SchemeBonusScale,b.Name as InitiateName,AtTopStatus,b.Level,Money, c.Name as PlayTypeName,c.LotteryId as LotteryID, a.Multiple, Share, BuyedShare, Schedule, AssureMoney, ")
                   .AppendLine("	InitiateUserID, QuashStatus, PlayTypeID, Buyed, SecrecyLevel, EndTime, d.IsOpened,e.Name as LotteryName, LotteryNumber,case Schedule when 100 then 1 else 0 end as IsFull ")
                   .AppendLine("from")
                   .AppendLine("	(")
                   .AppendLine("		select top 1 ID, EndTime,IsOpened from T_Isuses where getdate() between StartTime and EndTime and LotteryID =" + lotteryID)
                   .AppendLine("	) as d")
                   .AppendLine("inner join T_Schemes a on a.IsuseID = d.ID  and a.isOpened = 0 and a.Share > 1 and SecrecyLevel = 0")
                   .AppendLine("inner join T_Users b on a.InitiateUserID = b.ID")
                   .AppendLine("inner join T_PlayTypes c on a.PlayTypeID = c.ID")
                   .AppendLine("inner join T_Lotteries e on c.LotteryID=e.ID")
                   .AppendLine("order by a.AtTopStatus desc, a.QuashStatus asc,a.Schedule desc,a.Money desc,a.ID desc");
        }
        else
        {
            sql.AppendLine("select a.AtTopStatus, a.ID,a.SchemeBonusScale,b.Name as InitiateName,AtTopStatus,b.Level,Money, c.Name as PlayTypeName,c.LotteryId as LotteryID, a.Multiple, Share, BuyedShare, Schedule, AssureMoney, ")
                   .AppendLine("	InitiateUserID, QuashStatus, PlayTypeID, Buyed, SecrecyLevel, EndTime, d.IsOpened,e.Name as LotteryName, LotteryNumber,case Schedule when 100 then 1 else 0 end as IsFull ")
                   .AppendLine("from ")
                   .AppendLine("	(")
                   .AppendLine("		select  ID, EndTime,IsOpened from T_Isuses where getdate() between StartTime and EndTime ")
                   .AppendLine("	) as d")
                   .AppendLine("inner join T_Schemes a on a.IsuseID = d.ID  and a.isOpened = 0 and a.Share > 1 and SecrecyLevel = 0")
                   .AppendLine("inner join T_Users b on a.InitiateUserID = b.ID")
                   .AppendLine("inner join T_PlayTypes c on a.PlayTypeID = c.ID")
                   .AppendLine("inner join T_Lotteries e on c.LotteryID=e.ID")
                   .AppendLine("order by a.AtTopStatus desc,a.QuashStatus asc,a.Schedule desc,a.Money desc,a.ID desc");
        }

        DataTable dt = MSSQL.Select(sql.ToString());

        if (dt == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(order))
        {
            order = "AtTopStatus desc,QuashStatus asc,Schedule desc,Money desc,ID desc";
        }



        DataRow[] dr = dt.Select(Condition, order);
        //剔除竞彩足球和竞彩篮球未开始的数据        
        int inValidJC = 0;//无效数据
        foreach (DataRow dRow in dr)
        {
            int LotteryID = Shove._Convert.StrToInt(dRow["LotteryID"].ToString(), 1);

            if (LotteryID == 45 || LotteryID == 72 || LotteryID == 73)
            {
                DateTime EndTime = Shove._Convert.StrToDateTime(GetScriptResTable(dRow["LotteryNumber"].ToString()), DateTime.Now.AddDays(-1).ToString());


                if (EndTime.CompareTo(DateTime.Now) < 1)
                {
                    inValidJC++;
                    continue;
                }
            }
        }

        int perPageRowCount = PageNum;
        int rowCount = dr.Length - inValidJC;
        int pageCount = rowCount % perPageRowCount == 0 ? rowCount / perPageRowCount : rowCount / perPageRowCount + 1;

        if (pageindex > pageCount)
        {
            pageindex = pageCount;
        }

        if (pageindex < 1)
        {
            pageindex = 1;
        }

        int Count = 0;

        if (perPageRowCount * pageindex > rowCount)
        {
            Count = rowCount;
        }
        else
        {
            Count = perPageRowCount * pageindex;
        }

        StringBuilder sbContent = new StringBuilder();

        float Money = 0;
        int Share = 0;
        double Schedule = 0;
        double BuyedShare = 0;
        string Surplus = "";
        int number = (pageindex - 1) * PageNum; ;

        for (int i = (pageindex - 1) * perPageRowCount; i < Count; i++)
        {
            /*int LotteryID = Shove._Convert.StrToInt(dt.Rows[i]["LotteryID"].ToString(), 1);

            if (LotteryID == 72 || LotteryID == 73)
            {
                DateTime EndTime = Shove._Convert.StrToDateTime(GetScriptResTable(dt.Rows[i]["LotteryNumber"].ToString()), DateTime.Now.AddDays(-1).ToString());

                if (EndTime.CompareTo(DateTime.Now) < 1)
                {
                    continue;
                }
            }*/

            Money = Shove._Convert.StrToFloat(dr[i]["Money"].ToString(), 0);
            Share = Shove._Convert.StrToInt(dr[i]["Share"].ToString(), 0);
            Schedule = Shove._Convert.StrToDouble(dr[i]["Schedule"].ToString(), 0);
            BuyedShare = Shove._Convert.StrToInt(dr[i]["BuyedShare"].ToString(), 0);

            Surplus = (Share - BuyedShare).ToString();

            //Surplus = (Share * (1 - Schedule * 0.01)).ToString();

            if (Surplus.IndexOf(".") >= 0)
            {
                Surplus = (Shove._Convert.StrToInt(Surplus.Substring(0, Surplus.IndexOf(".")), 0) + 1).ToString();
            }

            sbContent.Append("<tr><td width=\"60\">" + (i + 1).ToString() + "</td>");
            sbContent.Append("<td width=\"70\">" + Users.UserControlHiedAndShow(dr[i]["InitiateName"].ToString()) + "</td>");
            if (lotteryID == "-1")
            {
                //合买大厅首页
                sbContent.Append("<td width=\"90\"><div style=background-image:url(Images/gold.gif); width:" + (9 * Shove._Convert.StrToInt(dr[i]["Level"].ToString(), 0)).ToString() + "px;background-repeat:repeat-x;></div>");
                if (_sites.Opt_IsShowUserUrl == 0)
                {
                    sbContent.Append("&nbsp;&nbsp;</td>");
                }
                else
                {
                    sbContent.Append("<a href=../Home/Web/Score.aspx?id=" + dr[i]["InitiateUserID"].ToString() + "&LotteryID=" + dr[i]["LotteryID"].ToString() + ">查看</a></td>");
                }
                sbContent.Append("<td width=\"78\">" + dr[i]["LotteryName"].ToString() + "</td>");//彩种名称
            }
            else
            {
                //合买大厅详情
                sbContent.Append("<td width=\"40\"><div style=background-image:url(Images/gold.gif); width:" + (9 * Shove._Convert.StrToInt(dr[i]["Level"].ToString(), 0)).ToString() + "px;background-repeat:repeat-x;></div>");
                if (_sites.Opt_IsShowUserUrl == 0)
                {
                    sbContent.Append("&nbsp;&nbsp;</td>");
                }
                else
                {
                    sbContent.Append("<a href=../Home/Web/Score.aspx?id=" + dr[i]["InitiateUserID"].ToString() + "&LotteryID=" + dr[i]["LotteryID"].ToString() + ">查看</a></td>");
                }
                sbContent.Append("<td width=\"40\"><span>" + dr[i]["Multiple"].ToString() + "</span> 倍</td>");
                sbContent.Append("<td width=\"70\"><span>" + Money.ToString() + "</span> 元</td>");
                sbContent.Append("<td width=\"70\"><span id=\"spanShareMoney_" + dr[i]["ID"].ToString() + "\">" + (Money / Share).ToString() + "</span> 元</td>");
            }
            if (Shove._Convert.StrToDouble(dr[i]["AssureMoney"].ToString(), 0) > 0)
            {
                sbContent.Append("<td width=\"120\">" + dr[i]["Schedule"].ToString() + "%+<span class=red>" + (Shove._Convert.StrToDouble(dr[i]["AssureMoney"].ToString(), 0) / Money * 100).ToString("0.00") + "%(保)</td>");
            }
            else
            {
                sbContent.Append("<td width=\"120\">" + dr[i]["Schedule"].ToString() + "%</td>");
            }
            if (lotteryID == "-1")
            {
                //合买大厅首页
                sbContent.Append("<td width=\"120\">" + Money.ToString() + "元|<span id=\"spanShareMoney_" + dr[i]["ID"].ToString() + "\">" + (Money / Share).ToString() + "元</span></td>");
                if (dr[i]["Schedule"].ToString() == "100")
                {
                    sbContent.Append("<td width=\"120\">已满员</td>");
                }
                else if (dr[i]["QuashStatus"].ToString() != "0")
                {
                    sbContent.Append("<td width=\"120\">已撤单</td>");
                }
                else
                {
                    sbContent.Append("<td width=\"100\"><input type=\"text\" id=\"txtSurplus_" + dr[i]["ID"].ToString() + "\" style=\"width:90px;\" value=\"剩余" + Surplus + "份\" onblur=\"javascript:setBlur(this);\" onfocus=\"javascript:setFocus(this);\" />");
                    sbContent.Append("<span id=\"spSurplus_" + dr[i]["ID"].ToString() + "\" style=\"display:none;\">剩余" + Surplus + "份</span>");
                    sbContent.Append("<input type=\"text\" id=\"tbShare_" + dr[i]["ID"].ToString() + "\" style=\"display:none;\" class=\"Share\" value=\"" + Surplus + "\">");
                    sbContent.Append("<span id=\"spanSurplus_" + dr[i]["ID"].ToString() + "\" style=\"display:none;\">" + Surplus + "</span></td>");
                }
            }
            else
            {
                //合买大厅详情
                if (dr[i]["Schedule"].ToString() == "100")
                {
                    sbContent.Append("<td width=\"100\">已满员</td>");
                    sbContent.Append("<td width=\"80\">已满员</td>");
                }
                else if (dr[i]["QuashStatus"].ToString() != "0")
                {
                    sbContent.Append("<td width=\"100\">已撤单</td>");
                    sbContent.Append("<td width=\"80\"><span id=\"spSurplus_" + dr[i]["ID"].ToString() + "\" >" + Surplus + "份</span></td>");
                }
                else
                {
                    sbContent.Append("<td width=\"100\"><span id=\"spSurplus_" + dr[i]["ID"].ToString() + "\" >" + Surplus + "份</span></td>");
                    sbContent.Append("<td width=\"80\"><input type=\"text\" id=\"txtSurplus_" + dr[i]["ID"].ToString() + "\" style=\"width:90px;\" value=\"" + Surplus + "份\" onblur=\"javascript:setBlur(this);\" onfocus=\"javascript:setFocus(this);\" />");
                    sbContent.Append("<span id=\"spanSurplus_" + dr[i]["ID"].ToString() + "\" style=\"display:none;\">" + Surplus + "</span></td>");
                }
            }
            if (dr[i]["QuashStatus"].ToString() != "0" || dr[i]["Schedule"].ToString() == "100")
            {

                sbContent.Append("<td width=\"50\" style=\"text-align:right;\">--</td>");
                sbContent.Append("<td width=\"50\" style=\"text-align:center;\"><a href=../Home/Room/Scheme.aspx?id=" + dr[i]["ID"].ToString() + " target=_blan title=点击查看方案详细信息>详情</a></td></tr>");
            }
            else
            {
                //sbContent.Append("<td><input type=\"text\" value=\"1\" size=\"4\" id=\"tbShare_" + dr[i]["ID"].ToString() + "\"  class=\"Share\" /></td>");
                /* sbContent.Append("<td>");
                 sbContent.Append("<img align=\"absmiddle\" class=\"jian\" onclick=\"return MReduction(" + dr[i]["ID"].ToString() + ");\" src=\"/Home/Room/Images/tmimg.gif\" /> ");
                 sbContent.Append("<input type=\"text\" value=\"1\" size=\"4\" style=\"width: 25px;\" id=\"tbShare_" + dr[i]["ID"].ToString() + "\" class=\"Share\" /> ");
                 sbContent.Append("<img align=\"absmiddle\" class=\"jia\" onclick=\"return MAddition(" + dr[i]["ID"].ToString() + ");\" src=\"/Home/Room/Images/tmimg.gif\" /></td>");
                 sbContent.Append("</td>");*/

                sbContent.Append("<td width=\"50\" style=\"text-align:right;\"><a href=\"javascript:void(0);\" class=\"join\"><img src=\"images/btn_cy.gif\" alt=\"参与\" align=\"middle\" mid=\"" + dr[i]["ID"].ToString() + "\" /></a></td>");
                sbContent.Append("<td width=\"55\" style=\"text-align:center;\"><a href=../Home/Room/Scheme.aspx?id=" + dr[i]["ID"].ToString() + " target=_blank title=点击查看方案详细信息>详情</a></td></tr>");
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

        DataTable dtPage = new DataTable();

        dtPage.Columns.Add("page", typeof(System.String));

        StringBuilder sb = new StringBuilder();

        sb.Append("<span class=\"jilu\">共" + pageCount.ToString() + "页，" + dr.Length + "条记录</span><span id=\"first\"><a href=\"#\" onclick = \"InitData(0);\">首页</a></span>");

        if (pageindex == 1)
        {
            sb.Append("<span class=\"disabled\">« 上一页</span>");
        }
        else
        {
            sb.Append("<span><a href=\"#\" onclick = \"InitData(" + (pageindex - 2).ToString() + ");\">« 上一页</a></span>");
        }

        for (int i = 0; i < pageCount; i++)
        {
            if (i == pageindex - 1)
            {
                sb.Append("<span class=\"current\" onclick = \"InitData(" + i.ToString() + ");\">" + (i + 1).ToString() + "</span>");

                continue;
            }

            if ((i < pageindex + 4 || i < 9) && (i > pageindex - 6 || i > pageCount - 10))
            {
                sb.Append("<a href=\"#\" onclick = \"InitData(" + i.ToString() + ");\">" + (i + 1).ToString() + "</a>");
            }
        }

        if (pageindex == pageCount)
        {
            sb.Append("<span class=\"disabled\">下一页 »</span>");
        }
        else
        {
            sb.Append("<span><a href=\"#\" onclick = \"InitData(" + (pageindex).ToString() + ");\">下一页 »</a></span>");
        }

        sb.Append("<span id=\"last\" value=\"" + pageCount.ToString() + "\"><a href=\"#\" onclick = \"InitData(" + (pageCount - 1).ToString() + ");\">尾页</a></span>");

        DataRow drPage = dtPage.NewRow();

        drPage["page"] = sb.ToString();

        dtPage.Rows.Add(drPage);
        dtPage.AcceptChanges();
        ds.Tables.Add(dtPage);

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