<%@ WebHandler Language="C#" Class="NowIsuse" %>

using System;
using System.Web;
using System.Text;
using donet.io.rong.methods;
using donet.io.rong.models;
using Newtonsoft.Json;
using Shove.Database;
using System.Data;
/// <summary>
/// 获取当前期的接口
/// @Author Bob
/// </summary>
public class NowIsuse : IHttpHandler
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
            sql_nextIsuse.Append(" FROM T_Isuses ");
            sql_nextIsuse.Append(" where  LotteryID="+lotteryId+" and  StartTime<='" + strNow + "' and  EndTime>='" + strNow + "'");
            System.Data.DataTable dt_isuses = Shove.Database.MSSQL.Select(sql_nextIsuse.ToString());

            if (dt_isuses != null && dt_isuses.Rows.Count>0)
            {
                DataRow row = dt_isuses.Rows[0];
                NowIsuseEntity nowIsuse = new NowIsuseEntity();
                nowIsuse.lotteryId = lotteryId;
                nowIsuse.id = row["ID"].ToString();
                nowIsuse.name = row["Name"].ToString();
                nowIsuse.startTime = row["StartTime"].ToString();
                nowIsuse.closeTime = row["CloseTime"].ToString();
                nowIsuse.endTime = row["EndTime"].ToString();
                nowIsuse.systemTime = strNow;
                string json = JsonConvert.SerializeObject(nowIsuse);
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

    struct NowIsuseEntity
    {
        public string lotteryId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string startTime { get; set; }
        public string closeTime { get; set; }
        public string endTime { get; set; }
        public string systemTime { get; set; }
    }
}