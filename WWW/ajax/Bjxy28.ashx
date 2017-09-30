<%@ WebHandler Language="C#" Class="CancelAttention" %>

using System;
using System.Web;
using System.Text;
using Newtonsoft.Json;

public class CancelAttention : IHttpHandler {

    StringBuilder sb = new StringBuilder();
    
    public void ProcessRequest (HttpContext context) {
        // 声明接受所有域的请求
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        context.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST");
        context.Response.Clear();
        context.Response.ContentType = "application/json";
        //彩票ID，有98加拿大，99北京
        string lotteryId = Shove._Web.Utility.GetRequest("lotteryId");
        //操作，有1查询近期开奖记录，2查询指定房间的赔率表，3走势图
        string opt = Shove._Web.Utility.GetRequest("opt");
        //房间号，有0回水厅，1保本厅，2高赔率厅
        string hall = Shove._Web.Utility.GetRequest("hall");

        if (string.IsNullOrEmpty(opt)) {
            context.Response.Clear();

            sb.Append("{\"error\":\"-108\",");
            sb.Append("\"msg\":\"缺少opt\"}");
            context.Response.Write(sb);
            return;
        }
        if (string.IsNullOrEmpty(lotteryId))
        {
            context.Response.Clear();

            sb.Append("{\"error\":\"-108\",");
            sb.Append("\"msg\":\"lotteryId\"}");
            context.Response.Write(sb);
            return;
        }
        else if (!(lotteryId.Equals("98")||lotteryId.Equals("99")))
        {
            context.Response.Clear();

            sb.Append("{\"error\":\"-108\",");
            sb.Append("\"msg\":\"lotteryId只接受98/99\"}");
            context.Response.Write(sb);
            return;
        }
        switch (opt) { 
            case "1":
                //读取近十期记录
                 System.Data.DataTable dt = Shove.Database.MSSQL.Select(@"SELECT TOP 50 [Name],[WinLotteryNumber]
  FROM [SLS53_WangXianSheng].[dbo].[T_Isuses] where LotteryId=" + lotteryId + " and LEN(RTRIM(WinLotteryNumber))>2 order by ID desc;");

        if (dt == null)
        {
           
            context.Response.Clear();
           
            sb.Append("{\"error\":\"-108\",");
            sb.Append("\"msg\":\"获取开奖号码错误\"}");
            context.Response.Write(sb);
            return;

        }

        if (dt.Rows.Count < 1)
        {
           
            context.Response.Clear();
           
            sb.Append("{\"error\":\"-108\",");
            sb.Append("\"msg\":\"暂无开奖号码\"}");
            context.Response.Write(sb);
            return;
        }
        else
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder list = new StringBuilder("[");
            context.Response.Clear();
            
            for (int i = 0; i < dt.Rows.Count; i++) {
                string winLotteryNumber = dt.Rows[i]["WinLotteryNumber"].ToString();
                int a = Convert.ToInt32(winLotteryNumber.Substring(0, 1));
                int b = Convert.ToInt32(winLotteryNumber.Substring(1, 1));
                int c = Convert.ToInt32(winLotteryNumber.Substring(2,1));
                int sum = a + b + c;
                string type1 = "";
                string type2 = "";
                string type3 = "";
                if (sum > 13) type1 = "大"; else type1 = "小";
                if (sum%2==0) type2 = "双"; else type2 = "单";
                if (sum>0&&sum % 3 == 0) type3 = "红";
                else if (sum > 0 && (sum+1) % 3 == 0) type3 = "蓝";
                else type3 = "绿";
                list.Append("{\"name\":\"" + dt.Rows[i]["Name"] + "\",\"winLotteryNumber\":\"" + a + "+" + b + "+" + c + "=" + sum);
                list.Append("\",\"type1\":\"" + type1 + "\",\"type2\":\"" + type2 + "\",\"type3\":\"" + type3 + "\"}");
                if (i != (dt.Rows.Count - 1))
                    list.Append(",");
            }
            list.Append("]");
            sb.Append("{\"error\":\"0\",");
            sb.Append("\"msg\":"+list+"}");
            context.Response.Write(sb);
            return;
        }
         break;
            case "2":
                //读取各房间的赔率
         if (string.IsNullOrEmpty(hall))
         {
             context.Response.Clear();

             sb.Append("{\"error\":\"-108\",");
             sb.Append("\"msg\":\"缺少大厅ID，0回水，1保本，2高赔率\"}");
             context.Response.Write(sb);
             return;
         }
         else {
             StringBuilder sb = new StringBuilder();
             StringBuilder list = new StringBuilder("[");
             context.Response.Clear();
             
             System.Data.DataTable wintype_dt = Shove.Database.MSSQL.Select(@"SELECT * FROM [SLS53_WangXianSheng].[dbo].[T_WinTypes] where LotteryId=" + lotteryId + " and hall ="+hall+" order by ID asc");

             if (wintype_dt == null)
             {

                 context.Response.Clear();

                 sb.Append("{\"error\":\"-108\",");
                 sb.Append("\"msg\":\"获取玩法表错误\"}");
                 context.Response.Write(sb);
                 return;

             }

             if (wintype_dt.Rows.Count < 1)
             {

                 context.Response.Clear();

                 sb.Append("{\"error\":\"-108\",");
                 sb.Append("\"msg\":\"暂无玩法表数据\"}");
                 context.Response.Write(sb);
                 return;
             }
             else
             {
                 for (int i = 0; i < wintype_dt.Rows.Count; i++)
                 {
                     list.Append("{\"id\":\"" + wintype_dt.Rows[i]["ID"] + "\"");
                     list.Append(",\"lotteryId\":\"" + wintype_dt.Rows[i]["LotteryID"] + "\"");
                     list.Append(",\"name\":\"" + wintype_dt.Rows[i]["Name"] + "\"");
                     list.Append(",\"order\":\"" + wintype_dt.Rows[i]["Order"] + "\"");
                     list.Append(",\"defaultMoney\":\"" + Convert.ToDouble(wintype_dt.Rows[i]["DefaultMoney"]) + "\"");
                     list.Append(",\"hall\":\"" + wintype_dt.Rows[i]["Hall"] + "\"}");
                     if (i != (wintype_dt.Rows.Count - 1))
                         list.Append(",");
                 }
             }
             
             
             list.Append("]");
             sb.Append("{\"error\":\"0\",");
             sb.Append("\"msg\":" + list + "}");
             context.Response.Write(sb);
             return;
         }
         break;

            //走势图返回
            case "3":
         System.Data.DataTable dtChart = Shove.Database.MSSQL.Select(@"SELECT TOP 50 [Name],[WinLotteryNumber],[OpeniSTime] FROM [SLS53_WangXianSheng].[dbo].[T_Isuses] where LotteryId=" + lotteryId + " and LEN(RTRIM(WinLotteryNumber))>2 order by ID desc;");

         if (dtChart == null)
         {

             context.Response.Clear();

             sb.Append("{\"error\":\"-108\",");
             sb.Append("\"msg\":\"获取开奖号码错误\"}");
             context.Response.Write(sb);
             return;

         }

         if (dtChart.Rows.Count < 1)
         {

             context.Response.Clear();

             sb.Append("{\"error\":\"-108\",");
             sb.Append("\"msg\":\"暂无开奖号码\"}");
             context.Response.Write(sb);
             return;
         }
         else
         {
             StringBuilder sb = new StringBuilder();
             StringBuilder list = new StringBuilder("[");
             context.Response.Clear();

             for (int i = 0; i < dtChart.Rows.Count; i++)
             {
                 string winLotteryNumber = dtChart.Rows[i]["WinLotteryNumber"].ToString();
                 int a = Convert.ToInt32(winLotteryNumber.Substring(0, 1));
                 int b = Convert.ToInt32(winLotteryNumber.Substring(1, 1));
                 int c = Convert.ToInt32(winLotteryNumber.Substring(2, 1));
                 int sum = a + b + c;
                 string type1 = "";
                 string type2 = "";
                 string type3 = "";
                 if (sum > 13) type1 = "大"; else type1 = "小";
                 if (sum % 2 == 0) type2 = "双"; else type2 = "单";
                 if (sum > 0 && sum % 3 == 0) type3 = "红";
                 else if (sum > 0 && (sum + 1) % 3 == 0) type3 = "蓝";
                 else type3 = "绿";
                 list.Append("{\"name\":\"" + dtChart.Rows[i]["Name"] + "\",\"winLotteryNumber1\":\"" + a + "\",\"winLotteryNumber2\":\"" + b + "\",\"winLotteryNumber3\":\"" + c);
                 list.Append("\",\"sum\":\"" + sum);
                 list.Append("\",\"type1\":\"" + type1 + "\",\"type2\":\"" + type2 + "\",\"type3\":\"" + type3 + "\",\"OpeniSTime\":\"" + dtChart.Rows[i]["OpeniSTime"].ToString() + "\"}");
                 if (i != (dtChart.Rows.Count - 1))
                     list.Append(",");
             }
             list.Append("]");
             sb.Append("{\"error\":\"0\",");
             sb.Append("\"msg\":" + list + "}");
             context.Response.Write(sb);
             return;
         }
         break;
        }

       
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}