<%@ WebHandler Language="C#" Class="HeMai" %>
using System;
using System.Web;
using System.Data;

using System.Text;

using Shove.Database;
public class HeMai : IHttpHandler
{
    int FY = 0;
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

        int lottid = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("lottid").ToString(), 0);//彩种ID

        int pag = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("pag").ToString(), 0);//当前页数

        int userid = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("userid").ToString(), 0);//用户名

        StringBuilder sql = new StringBuilder();

        int countdes = 0;//总条数

        int TotalRowCount = 0;
        int PageCount = 0;
        int intResult = -1;
        int pagsize = 1;
        int crrsize = 15;//本页显示条数

        DataTable dq = null;

        StringBuilder su = new StringBuilder();
        //全部名人
        if (lottid == 0)
        {
            DataTable dn = null;
            dn = MSSQL.Select("select UserID,UserName,LotteryID,(select count(ID) from v_Hemai where InitiateUserID=a.UserID) as isdeng from  (select *,ROW_NUMBER() over(partition by UserID order by UserID) as m from T_Personages) a where m=1 and a.IsShow=1 order by lotteryid ,[order] ");
            if (dn.Rows.Count == 0 || dn == null)
            {
                su.Append("暂无名人");
            }
            else
            {
                string Name = "";
                foreach (DataRow dy in dn.Rows)//是否有发起合买
                {
                    Name = dy["UserName"].ToString();

                    Name = Users.UserControlHiedAndShow(Name);

                    if (Shove._Convert.StrToInt(dy["isdeng"].ToString(), 0) > 0)
                    {
                        su.Append(" <a title=\"有合买方案\"  onclick=\"hongcliks(" + dy["UserID"]
                            + ")\" style=\"cursor:pointer\"   target=\"_blank\" ><span class=\"\"></span>" + Name + "</a>");
                    }
                    else
                    {
                        if (_sites.Opt_IsShowUserUrl == 0)
                        {
                            su.Append("<a title=\"暂无合买方案\" style=\"cursor:pointer\">" + Name + "</a>");
                        }
                        else
                        {
                            su.Append("<a title=\"暂无合买方案\" style=\"cursor:pointer\" href=\"/Home/Web/Score.aspx?LotteryID=" +
                                72 + "&usid=56&ID=" + dy["UserID"].ToString() + "\" target=\"_blank\" >" + Name + "</a>");
                        }
                    }
                }
            }

        }
        //彩种名人
        if (lottid != 0)//选择彩种显示名人
        {
            DataTable ddn = null;
            ddn = MSSQL.Select("select UserID,UserName,LotteryID,(select count(ID) from v_Hemai where InitiateUserID=UserID and LotteryID=" + lottid + " )as isdeng from T_Personages WHERE LotteryID=" + lottid + " and IsShow=1 order by lotteryid ,[order]");

            if (ddn.Rows.Count == 0 || ddn == null)
            {
                su.Append("暂无名人");
            }
            else
            {
                string Name = "";
                foreach (DataRow dy in ddn.Rows)
                {
                    Name = dy["UserName"].ToString();

                    Name = Users.UserControlHiedAndShow(Name);

                    if (Shove._Convert.StrToInt(dy["isdeng"].ToString(), 0) > 0)
                    {
                        su.Append(" <a  title=\"有合买方案\" style=\"cursor:pointer\" onclick=\"hongclik(" + dy["UserID"]
                            + ")\" href=\"#\"><span class=\"\"></span>" + Name + "</a>");
                    }
                    else
                    {
                        if (_sites.Opt_IsShowUserUrl == 0)
                        {
                            su.Append("<a title=\"暂无合买方案\" style=\"cursor:pointer\" >" + Name + "</a>");
                        }
                        else
                        {
                            su.Append("<a title=\"暂无合买方案\" style=\"cursor:pointer\" href=\"/Home/Web/Score.aspx?LotteryID=" + lottid + "&usid=5&ID=" + dy["UserID"].ToString() + "\" target=\"_blank\" >" + Name + "</a>");
                        }
                    }
                }
            }
        }

        //判断是否是点击名人显示合买信息

        DataSet ds = null;
        string HandselManyuan = new DAL.Tables.T_Sites().Open("HandselManyuan", "", "").Rows[0]["HandselManyuan"].ToString();
        string where = "";

        if (HandselManyuan == "0")
        {
            where += " and buyedshare<share";
        }
        if (lottid != 0)
        {
            where += " and LotteryID=" + lottid;
        }
        if (userid != 0)//是否有点击名人
        {
            where += " and InitiateUserID=" + userid;
        }
        dq = MSSQL.Select(" select COUNT(*) as count from v_Hemai where 1=1 " + where);

        intResult = DAL.Procedures.P_Pager(ref ds, pag, crrsize, 0, "*",
                    "v_Hemai", "schedule desc", "1=1" + where, "ID", ref TotalRowCount, ref PageCount);

        //DataTable dt = Transform(ds.Tables[0]);

        DataTable dt = ds.Tables[0];

        if (dt != null)
        {
            countdes = Shove._Convert.StrToInt(dq.Rows[0]["count"].ToString(), 0);//总条数

            int pagsizeq = countdes % crrsize;

            if (pagsizeq == 0)
            {
                pagsize = countdes / crrsize;
            }
            else
            {
                pagsize = countdes / crrsize + 1;
            }

        }

        StringBuilder sbContent = new StringBuilder();

        if (dt.Rows.Count == 0)
        {
            sbContent.Append("暂无合买信息");
        }
        else
        {

            float Money = 0;
            int Share = 0;
            double Schedule = 0;
            double BuyedShare = 0;
            string Surplus = "";
            int i = 0;
            string Name = "";
            foreach (DataRow dr in dt.Rows)//循环添加合买信息
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

                if (i % 2 == 0)
                {
                    sbContent.Append("<tr class=\"scheme-item\"><td class=\"t_c\" style='text-align:center'><span>" + (i + 1).ToString() + "</span></td>");
                }
                else
                {
                    sbContent.Append("<tr bgcolor=\"f7f7f7\" class=\"scheme-item\"><td class=\"t_c\" style='text-align:center'><span>" + (i + 1).ToString() + "</span></td>");
                }

                Name = dr["InitiateName"].ToString();

                Name = Users.UserControlHiedAndShow(Name);
                if (_sites.Opt_IsShowUserUrl == 0)
                {
                    sbContent.Append("<td style='text-align:center'>" + Name + "</td>");
                }
                else
                {
                    sbContent.Append("<td style='text-align:center'><a href=\"/Home/Web/Score.aspx?LotteryID=" + dr["LotteryID"].ToString() + "&ID=" + dr["InitiateUserID"].ToString() + "\" target=\"_blank\" >" + Name + "</a></td>");
                }
                sbContent.Append("<td class=\"t_k\" style='text-align:center'>" + PF.GetLevel(dr["Level"].ToString()) + "</td>");

                sbContent.Append("<td class=\"t_k\" style='text-align:center'>" + dr["LotteryName"].ToString() + "</td>");

                if (Shove._Convert.StrToDouble(dr["AssureMoney"].ToString(), 0) > 0)
                {
                    sbContent.Append("<td style='text-align:center'><p style='margin-top:-5px;'>" + Shove._Convert.StrToDouble(dr["Schedule"].ToString(), 0).ToString("0.00") + "%+" + (Shove._Convert.StrToDouble(dr["AssureMoney"].ToString(), 0) / Money * 100).ToString("0.00") + "%<strong id=\"bao\">保</strong></p></td>");
                }
                else
                {
                    sbContent.Append("<td style='text-align:center'><p style='margin-top:-5px;'>" + Convert.ToDouble(dr["Schedule"]).ToString("0.00") + "%</p></td>");
                }
                sbContent.Append("<td class=\"totalmoney\"><span style='color:red'>" + Money.ToString() + "</span>元 /<td class=\"totalmoney01\" id=\"spanShareMoney_" + dr["ID"].ToString() + "\"><span style='color:red'>" + (Money / Share).ToString() + "</span>元</td></td>");
                sbContent.Append("<td style='text-align:center'><span id=\"spSurplus_" + dr["ID"].ToString() + "\" style=\"display:none;\">" + Surplus + "</span>");

                if (Shove._Convert.StrToDouble(dr["schedule"].ToString(), 100) >= 100)
                {
                    sbContent.Append("<a class=\"jian\"></a>");

                    sbContent.Append("<input class=\"group_input\"  disabled='disabled' style='width:80px;text-align:center' type=\"text\"  onBlur=\"blue(" + dr["ID"].ToString() + ")\" id=\"txtShare_" + dr["ID"].ToString() + "\" value=\"剩" + Surplus + "份\" style='ime-mode: disabled; color:#999999;' onfocus=\"setFocus(this)\" onblur=\"setBlur(this)\" />");

                    sbContent.Append("<a class=\"jia\"  style=\"background-image:url(../Images/jia-1.png);\"></a></td>");

                    sbContent.Append("<td class=\"t_r\" style='text-align:center'><span><a rel=\"" + dr["LotteryID"].ToString() + "\" class=\"buy_btn\" >已满员</a></span><a  href=../Home/Room/Scheme.aspx?id=" + dr["ID"].ToString() + " class=\"btn_mini\" target=\"_blank\" rel=\"nofollow\">详情</a></td></tr>");
                }
                else
                {
                    sbContent.Append("<a class=\"jian\" style=\"background-image:url(../Images/jian-1.png) \"  onclick=\"return MReduction1(" + dr["ID"].ToString() + ");\"></a>");

                    sbContent.Append("<input class=\"group_input\" style='width:80px;text-align:center' type=\"text\"  onBlur=\"blue(" + dr["ID"].ToString() + ")\" id=\"txtShare_" + dr["ID"].ToString() + "\" value=\"剩" + Surplus + "份\" style='ime-mode: disabled; color:#999999;' onfocus=\"setFocus(this)\" onblur=\"setBlur(this)\" />");

                    sbContent.Append("<a class=\"jia\" onclick=\"return MAddition1(" + dr["ID"].ToString() + ");\"></a></td>");

                    sbContent.Append("<td class=\"t_r\" style='text-align:center'><span><a rel=\"" + dr["LotteryID"].ToString() + "\" class=\"buy_btn\" id=\"joinBuy\" mid=\"" + dr["ID"].ToString() + "\">购买</a></span><a  href=../Home/Room/Scheme.aspx?id=" + dr["ID"].ToString() + " class=\"btn_mini\" target=\"_blank\" rel=\"nofollow\">详情</a></td></tr>");
                }

                i++;
            }
        }

        StringBuilder st = new StringBuilder();

        if (lottid != 0)//点击彩种查看期号和结束时间
        {
            if (lottid == 72)
            {
                st.Append("72");
            }
            else if (lottid == 45)
            {
                st.Append("45");
            }
            else if (lottid == 73)
            {
                st.Append("73");
            }
            else
            {//结束时间，和开奖频率

                DataTable db = MSSQL.Select("select LotteryID,Name,StartTime,EndTime from T_Isuses where LotteryID=" + lottid + " and StartTime<GETDATE() and GETDATE()<EndTime");

                DataTable dp = MSSQL.Select(" select Code,TypeID,IntervalType from T_Lotteries  where ID=" + lottid + " ");

                if (dp.Rows.Count <= 0)
                {
                    return;

                }

                string Frequency;

                switch (lottid)
                {
                    case 5:
                        Frequency = "每周二、四、日晚上21:30开奖";
                        break;
                    case 39:
                        Frequency = "每周一、三、六 20:30开奖";
                        break;
                    case 13:
                        Frequency = "每周一、三、五 21:30开奖";
                        break;
                    case 3:
                        Frequency = "每周二、五、日 20:30开奖";
                        break;
                    case 6:
                    case 63:
                    case 64:
                        Frequency = "每日 20:30开奖";
                        break;
                    case 74:
                    case 75:
                    case 2:
                    case 15:
                        Frequency = "不定期开奖";
                        break;
                    default:

                        Frequency = "每十分钟一期";
                        break;
                }

                if (db.Rows.Count <= 0)
                {
                    string path = "Default.aspx";
                    st.Append(" <p class=\"border-gray-top\">当前暂无奖期<span style=\"color:red\">开奖频率：" + Frequency + "</span>  <input type=\"text\" id=\"te\" value=\"\" style=\" display:none\"  /></p> ");
                    st.Append(" <p class=\"border-gray-top\" id=\"timer\" style=\"color:Red\"></p><a id=\"buy-button\" href=\"../" + dp.Rows[0]["Code"] + "/" + path + "\"  target=\"_blank\" ></a> ");
                    st.Append(" <img src=\"../Images/PassTatistics/" + lottid + ".png\">");

                }
                else
                {
                    DataTable dbal = MSSQL.Select("select top 1  LotteryID, SystemEndAheadMinute from T_PlayTypes where LotteryID=" + lottid + " "); //查看后台提前截止时间

                    string dat = DateTime.Parse(db.Rows[0]["EndTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                    DateTime ti = DateTime.Parse(dat);

                    int dtime = Shove._Convert.StrToInt(dbal.Rows[0]["SystemEndAheadMinute"].ToString(), 0);

                    string enddime = DateTime.Parse((ti.AddMinutes(-dtime)).ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                    st.Append("  <p class=\"border-gray-top\" >当前销售：" + db.Rows[0]["Name"] + "期<span style=\"color:red\">开奖频率：" + Frequency + "</span></p>");
                    st.Append(" <p class=\"border-gray-top\" id=\"timer\" style=\"color:Red\" ></p><a id=\"buy-button\" href=\"../" + dp.Rows[0]["Code"] + "/\"target=\"_blank\"></a> <input type=\"hidden\"  id=\"tee\" value=\"" + enddime + "\" /> <input type=\"hidden\" id=\"pag\" value=\"" + pagsize + "\" /> ");
                    st.Append(" <img src=\"../Images/PassTatistics/" + lottid + ".png\">");
                }

            }
        }
        else
        {
            st.Append(" <p class=\"border-gray-top\">当前销售：只显示本期合买信息<span>开奖频率：每个彩种规定的频率开奖</span><input type=\"text\" id=\"te\" value=\"" + pagsize + "\" style=\" display:none\"/></p> ");
            st.Append(" <p></p> ");
            st.Append(" <img src=\"../Images/PassTatistics/全部彩种.png\">");
        }

        int pagecount = countdes - FY;
        int pageindex = pag;
        int pageys = pagsize;

        context.Response.Write("{\"message\":\"" + sbContent.ToString().Replace("\"", "\\\"") + "\",\"masf\":\"" + st.ToString().Replace("\"", "\\\"") + "\",\"page\":\"" + pageindex + "\",\"pagsize\":\"" + pagsize + "\",\"pagecount\":\"" + pagecount + "\",\"top\":\"" + su.ToString().Replace("\"", "\\\"") + "\"}");
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
                    i--;
                    FY++;
                    continue;
                }

                LotteryNumber = new AppGateway_ashx().Football(LotteryNumber);

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