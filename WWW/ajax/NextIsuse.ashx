<%@ WebHandler Language="C#" Class="NextIsuse" %>

using System;
using System.Web;
using System.Text;
using donet.io.rong.methods;
using donet.io.rong.models;
using Newtonsoft.Json;
using Shove.Database;
using System.Data;
/// <summary>
/// 获取下一期的接口
/// @Author Bob
/// </summary>
public class NextIsuse : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {


        // 声明接受所有域的请求
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        context.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST");
        context.Response.Clear();
        context.Response.ContentType = "application/json";


        string lotteryId = context.Request.Form["LotteryId"];
       
        if (string.IsNullOrEmpty(lotteryId))
        {

            context.Response.Write(buildCallBack(false, "\"the LotteryId is null\""));
            context.ApplicationInstance.CompleteRequest();
            context.Response.End();
            return;
        }
        else
        {
            StringBuilder sb = new StringBuilder();

            context.Response.Clear();

            string strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sql_nextIsuse = new StringBuilder("SELECT LotteryID,ID,Name,StartTime,Dateadd(SS,-30,EndTime) as CloseTime,EndTime");
            sql_nextIsuse.Append(" FROM T_Isuses as s1 where LotteryID=" + lotteryId + " and Name =(");
            sql_nextIsuse.Append(" SELECT top 1 convert(int,Name)+1 as nextID FROM T_Isuses");
            sql_nextIsuse.Append(" where  LotteryID=" + lotteryId + " and  EndTime>='" + strNow + "' order by EndTime asc)");
            System.Data.DataTable dt_isuses = Shove.Database.MSSQL.Select(sql_nextIsuse.ToString());

            if (dt_isuses != null && dt_isuses.Rows.Count>0)
            {
                DataRow row = dt_isuses.Rows[0];
                NextIsuseEntity nextIsuse=new NextIsuseEntity();
                nextIsuse.lotteryId = lotteryId;
                nextIsuse.id = row["ID"].ToString();
                nextIsuse.name = row["Name"].ToString();
                nextIsuse.startTime = row["StartTime"].ToString();
                nextIsuse.closeTime = row["CloseTime"].ToString();
                nextIsuse.endTime = row["EndTime"].ToString();
                nextIsuse.systemTime = strNow;
                string json = JsonConvert.SerializeObject(nextIsuse);
                context.Response.Write(buildCallBack(true, json));
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();
                return;
         }
            else
            { context.Response.Write(buildCallBack(true, "\"none\""));
                context.ApplicationInstance.CompleteRequest();
                context.Response.End();
                return;
                
            }
        }
    }
        

       
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private string buildCallBack(bool isSuccess, string text)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{\"status\":\"" + (isSuccess ? "success" : "fail") + "\",");
        sb.Append("\"msg\":" + text + "}");
        return sb.ToString();
    }

    struct NextIsuseEntity {
        public string lotteryId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string startTime { get; set; }
        public string closeTime { get; set; }
        public string endTime { get; set; }
        public string systemTime { get; set; }
    }
}