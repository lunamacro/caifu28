<%@ WebHandler Language="C#" Class="Join" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using System.Text;

using Shove.Database;

public class Join : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        //不让浏览器缓存
        context.Response.Buffer = true;
        context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
        context.Response.AddHeader("pragma", "no-cache");
        context.Response.AddHeader("cache-control", "");
        context.Response.CacheControl = "no-cache";
        context.Response.ContentType = "text/plain";
        StringBuilder sb = new StringBuilder();
        Users _User = Users.GetCurrentUser(1);

        if (_User == null)
        {
            context.Response.Write("{\"message\": \"您的登录信息丢失，请重新登录。\"}");

            return;
        }

        //订单锁处理,打开订单锁
        bool orderLockState = PF.OrderLock(_User.ID.ToString(), _User.Name, true, "");
        if (!orderLockState)
        {
            sb.Append("{\"error\":\"-101\",");
            sb.Append("\"buyID\":\"-1\",");
            sb.Append("\"msg\":\"订单在处理中，请不要重复提交订单。\"}");

            context.Response.Clear();
            context.Response.Write(sb.ToString());
            context.Response.End();
            return;
        }

        int Share = 1;
        int MaxShare = 1;


        if (Shove._Convert.StrToLong(context.Request["BuyShare"], 0) <= 0)
        {
            context.Response.Write("{\"message\": \"购买份数不能小于或者等于0。\"}");

            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "购买份数不能小于或者等于0");

            return;
        }



        if (string.IsNullOrEmpty(context.Request["BuyShare"]))
        {
            context.Response.Write("{\"message\": \"请输入购买份数。\"}");

            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "请输入购买份数");

            return;
        }

        Share = Shove._Convert.StrToInt(context.Request["BuyShare"].ToString(), 1);


        double ShareMoney = 0;

        ShareMoney = Shove._Convert.StrToInt(context.Request["ShareMoney"].ToString(), 1);

        long SchemeID = 0;

        if (string.IsNullOrEmpty(context.Request["SchemeID"]))
        {
            context.Response.Write("{\"message\": \"参数错误，请重新加入方案。\"}");

            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "参数错误，请重新加入方案。");

            return;
        }

        SchemeID = Shove._Convert.StrToLong(context.Request["SchemeID"].ToString(), 1);

        DataTable dt = new DAL.Views.V_SchemeSchedulesWithQuashed().Open("", "ID=" + SchemeID.ToString(), "");

        if (dt == null)
        {
            context.Response.Write("{\"message\": \"参数错误，请重新加入方案。\"}");

            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "参数错误，请重新加入方案。");

            return;
        }

        if (dt.Rows.Count < 1)
        {
            context.Response.Write("{\"message\": \"参数错误，请重新加入方案。\"}");

            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "参数错误，请重新加入方案。");

            return;
        }

        string dateTime = dt.Rows[0]["EndTime"].ToString();
        int LotteryID = Shove._Convert.StrToInt(dt.Rows[0]["LotteryID"].ToString(), 1);
        MaxShare = Shove._Convert.StrToInt(dt.Rows[0]["Share"].ToString(), 1);
        if (LotteryID == 72 || LotteryID == 45)
        {
            DateTime EndTime = Shove._Convert.StrToDateTime(dateTime, DateTime.Now.AddDays(-1).ToString());

            if (EndTime.CompareTo(DateTime.Now) < 1)
            {
                context.Response.Write("{\"message\": \"方案已截止，请参与其它方案。\"}");

                //关闭订单锁
                PF.OrderLock(_User.ID.ToString(), _User.Name, false, "方案已截止，请参与其它方案。");

                return;
            }
        }
        else
        {
            DateTime EndTime = Shove._Convert.StrToDateTime(dt.Rows[0]["SystemEndTime"].ToString(), DateTime.Now.AddDays(-1).ToString());

            if (EndTime.CompareTo(DateTime.Now) < 1)
            {
                context.Response.Write("{\"message\": \"方案已截止，请参与其它方案。\"}");

                //关闭订单锁
                PF.OrderLock(_User.ID.ToString(), _User.Name, false, "方案已截止，请参与其它方案。");

                return;
            }
        }

        //既不是发起人，也不在招股对象之内
        if (!_User.isCanViewSchemeContent(SchemeID))
        {
            context.Response.Write("{\"message\": \"对不起，您不在此方案的招股对象之内。\"}");

            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "对不起，您不在此方案的招股对象之内。");

            return;
        }

        if (ShareMoney * Share > (_User.Balance + _User.HandselAmount))
        {
            context.Response.Write("{\"message\": \"您的账户余额不足，请先充值，谢谢。\"}");

            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "您的账户余额不足，请先充值，谢谢。");

            return;
        }

        string ReturnDescription = "";

        if (_User.JoinScheme(SchemeID, Share, 1, ref ReturnDescription, Share, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) < 0 || ReturnDescription != "")
        {
            if (ReturnDescription.IndexOf("方案剩余份数已不足") > -1)
            {
                context.Response.Write("{\"message\": \"方案剩余份数已不足 " + Share.ToString() + " 份 \"}");

                //关闭订单锁
                PF.OrderLock(_User.ID.ToString(), _User.Name, false, "方案剩余份数已不足 " + Share.ToString() + " 份");

                return;
            }
            else
            {
                context.Response.Write(ReturnDescription);

                //关闭订单锁
                PF.OrderLock(_User.ID.ToString(), _User.Name, false, "ReturnDescription");

                return;
            }
        }

        string Key = "join_ProjectList_lotteryID" + LotteryID.ToString();
        Shove._Web.Cache.ClearCache(Key);

        Key = "join_SchemeList_lotteryID" + LotteryID.ToString();
        Shove._Web.Cache.ClearCache(Key);

        //关闭订单锁
        PF.OrderLock(_User.ID.ToString(), _User.Name, false, "参与合买成功，谢谢！");

        context.Response.Write("{\"message\": \"参与合买成功，谢谢！\"}");
        context.Response.End();
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
                table = new DAL.Tables.T_Match().Open("StopSellTime", "MatchID in (" + Matchids + ")", " StopSellTime ");
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