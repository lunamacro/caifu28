using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Shove.Database;

/// <summary>
///北京单场帮助类
/// </summary>
public class BJDCManage
{
    public BJDCManage()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 获取北京单场对阵信息（没缓存，考虑到赔率浮动）
    /// </summary>
    /// <param name="LotteryId"></param>
    /// <param name="PlayTypeID"></param>
    /// <param name="HTMLIsuses"></param>
    /// <param name="Isuses"></param>
    /// <returns></returns>
    public static DataTable GetBJSingle(string LotteryId, string PlayTypeID, ref string HTMLIsuses, ref string Isuses)
    {
        //获取北京单场最新的一条数据
        DataTable dtBJSingle = MSSQL.Select("select top 1 Issue from T_BJSingle order by Issue desc");

        if (dtBJSingle == null || dtBJSingle.Rows.Count == 0)
        {
            return null;
        }

        string newIsuses = dtBJSingle.Rows[0]["Issue"].ToString();

        //获取期号
        DataTable dtIsuses = MSSQL.Select(string.Format("select Name from T_Isuses where Name='{0}' and LotteryID={1}", newIsuses, LotteryId));

        if (dtIsuses == null || dtIsuses.Rows.Count == 0)
        {
            //没有期号
            return null;
        }

        HTMLIsuses = " <em style=\"font-size:21px;\">第<em style=\"color:red\">" + newIsuses + "</em>期</em>";
        Isuses = newIsuses;

        DataTable dt = Shove.Database.MSSQL.Select("Select * from T_BJSingle  where IsStopBetting=0 and Issue=" + newIsuses + " and DATEADD(minute, (select SystemEndAheadMinute from T_PlayTypes where id = " + PlayTypeID + ") * -1, StopSellTime) > GETDATE() order by [Day],MatchDate");

        return dt;
    }

    /// <summary>
    /// 返回周几的对应中文名
    /// </summary>
    /// <param name="weekday"></param>
    /// <returns></returns>
    public static string GetWeekDayName(DayOfWeek weekday)
    {
        string weekDayName = "";
        switch (weekday)
        {
            case DayOfWeek.Friday:
                weekDayName = "星期五";
                break;
            case DayOfWeek.Monday:
                weekDayName = "星期一";
                break;
            case DayOfWeek.Saturday:
                weekDayName = "星期六";
                break;
            case DayOfWeek.Sunday:
                weekDayName = "星期日";
                break;
            case DayOfWeek.Thursday:
                weekDayName = "星期四";
                break;
            case DayOfWeek.Tuesday:
                weekDayName = "星期二";
                break;
            case DayOfWeek.Wednesday:
                weekDayName = "星期三";
                break;
        }

        return weekDayName;
    }
}