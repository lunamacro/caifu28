<%@ WebHandler Language="C#" Class="WinDetails" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;

public class WinDetails : IHttpHandler
{

    HttpContext hc;
    public void ProcessRequest(HttpContext context)
    {
        hc = context;
        string act = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["act"]);
        switch (act)
        {
            case "GetLotterys":
                hc.Response.Write(GetLotterys());
                break;
            case "GetIssuse":
                hc.Response.Write(GetIssuse());
                break;
            case "GetWinLevel":
                hc.Response.Write(GetWinLevel());
                break;
            case "GetWinDes":
                hc.Response.Write(GetWinDes());
                break;
            case "SaveData":
                hc.Response.Write(SaveData());
                break;
        }
    }
    private string GetLotterys()
    {
        string sql_lottery = @"DECLARE @sql VARCHAR(500) DECLARE @lotteries VARCHAR(200) SET @lotteries = ( SELECT   UseLotteryListRestrictions FROM T_Sites) SET @sql = 'select id,name from t_lotteries where id in (5,39,6,63,64,3,13,74,75) and id in ('+ @lotteries + ') order by [order] asc'  EXECUTE(@sql)";
        DataTable dt_lottery = Shove.Database.MSSQL.Select(sql_lottery);
        return JsonHelper.ObjectToJson(dt_lottery);
    }
    private string GetIssuse()
    {
        string lotteryId = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["lotteryId"]);
        string sql_issue = "select id,name from t_isuses where isopened = 1 and  lotteryid =" + lotteryId + " order by name desc";
        DataTable issueDt = Shove.Database.MSSQL.Select(sql_issue);
        return JsonHelper.ObjectToJson(issueDt);
    }
    private string GetWinLevel()
    {
        string lotteryId = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["lotteryId"]);
        string issuseId = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["issuseId"]);
        string sql_wintype = string.Empty;
        if (lotteryId == "39")
        {
            sql_wintype = "select Name,DefaultMoney,LotteryID from T_WinTypes where  Name <> '七等奖' and  Name <> '八等奖' and lotteryid = " + lotteryId + "order by [Order] asc";
        }
        else
        {
            sql_wintype = "select Name,DefaultMoney,LotteryID from T_WinTypes where  Name <> '三星直选奖' and lotteryid = " + lotteryId + "order by [Order] asc";
        }
        DataTable dt_wintype = Shove.Database.MSSQL.Select(sql_wintype);
        return JsonHelper.ObjectToJson(dt_wintype);
    }
    private string GetWinDes()
    {
        string lotteryId = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["lotteryId"]);
        string issuseId = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["issuseId"]);
        string sqlStr = "SELECT TotalSales,WinType,WinCount,WinMoney,OpenNumber,Progressive,OpenTime FROM dbo.T_LotteryAnnouncement where LotteryID={0} and IssueID={1}";
        sqlStr = string.Format(sqlStr, lotteryId, issuseId);
        DataTable dt = Shove.Database.MSSQL.Select(sqlStr);
        return JsonHelper.ObjectToJson(dt);
    }
    private string SaveData()
    {
        try
        {
            string lotteryId = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["lotteryId"]);
            string issuseId = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["issuseId"]);
            string winDetail = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["winDetail"]);
            string totalSales = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["totalSales"]);
            string progressive = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["progressive"]);
            string openTime = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["OpenTime"]);
            string existSql = "SELECT COUNT(ID) FROM dbo.T_LotteryAnnouncement WHERE IssueID=" + issuseId + " AND LotteryID=" + lotteryId;
            DataTable exisDt = Shove.Database.MSSQL.Select(existSql);
            string sqlStr = string.Empty;
            string issuseSql = "SELECT Name,WinLotteryNumber,OpeniSTime FROM dbo.T_Isuses WHERE ID=" + issuseId;
            DataTable issuseDt = Shove.Database.MSSQL.Select(issuseSql);
            string winLotteryNumber = string.Empty;
            //string openiSTime = string.Empty;
            string issuseName = string.Empty;
            if (issuseDt.Rows.Count > 0)
            {
                issuseName = issuseDt.Rows[0]["Name"].ToString();
                winLotteryNumber = issuseDt.Rows[0]["WinLotteryNumber"].ToString();
                //openiSTime = issuseDt.Rows[0]["OpeniSTime"].ToString();
            }
            if (exisDt.Rows[0][0].ToString() != "0")
            {
                //数据库中已存在此条记录
                sqlStr = "UPDATE dbo.T_LotteryAnnouncement SET WinType='" + winDetail + "',TotalSales=" + totalSales + ",Progressive=" + progressive + ",IssueName='" + issuseName + "',OpenNumber='" + winLotteryNumber + "',OpenTime='" + openTime + "' WHERE IssueID=" + issuseId + " AND LotteryID=" + lotteryId;
            }
            else
            {
                //数据库中不存在此条记录
                sqlStr = @"
        INSERT INTO dbo.T_LotteryAnnouncement
        ( LotteryID ,
          IssueID ,
          IssueName ,
          TotalSales ,
          WinType ,
          WinCount ,
          WinMoney ,
          DateTime ,
          OpenTime ,
          OpenNumber ,
          Progressive ,
          OtherField2 ,
          OtherField3 ,
          OtherField4
        )
        VALUES  ( {0} ,
          {1} , 
          '{2}' , 
          {3} ,
          '{4}' , 
          0 , 
          '' ,
          GetDate() , 
          '{6}' , 
          '{7}' , 
          '{5}' , 
          '' , 
          '' , 
          ''  
        )
            ";
                sqlStr = string.Format(sqlStr, lotteryId, issuseId, issuseName, totalSales, winDetail, progressive, openTime, winLotteryNumber);
            }
            int i = Shove.Database.MSSQL.ExecuteNonQuery(sqlStr);
            if (i > -1)
            {
                return "{\"IsOk\":true}";
            }
            return "{\"IsOk\":false}";
        }
        catch (Exception)
        {

            return "{\"IsOk\":false}";
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