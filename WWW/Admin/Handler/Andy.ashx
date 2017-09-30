<%@ WebHandler Language="C#" Class="Andy" %>

using System;
using System.Web;
using System.Text;

public class Andy : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        context.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST");
        context.Response.Clear();
        context.Response.ContentType = "application/json";
        
        
        System.Text.StringBuilder result = new System.Text.StringBuilder();
        string action = context.Request.Form["action"];

        //获取代理列表
        if (action.Equals("getAgent"))
        {
            string groupid = context.Request.Form["groupid"];
            
            int index = 0;
            string sql = @"select ID,NickName from T_Users where isAgent=1 and agentGroup=" + groupid;
            System.Data.DataTable dtGroup = Shove.Database.MSSQL.Select(sql);
            if (dtGroup.Rows.Count <= 0)
            {
                result.Append("{\"code\":\"119\"}");
                context.Response.Write(result.ToString());
                context.Response.End();
            }
            
            
            result.Append("{\"code\":\"200\",\"data\":[");
            
            foreach (System.Data.DataRow row in dtGroup.Rows)
            {
                if (index == 0)
                {
                    result.Append("{\"text\":\"" + row["NickName"] + "\",\"value\":\"" + row["ID"] + "\"}");
                }
                else
                {
                    result.Append(",{\"text\":\"" + row["NickName"] + "\",\"value\":\"" + row["ID"] + "\"}");
                }
                index++;
            }
            result.Append("]}");
            context.Response.Write(result.ToString());
            context.Response.End();
        }
        
        
         //获取代理详情
        if (action.Equals("getAgentList"))
        {
            string userId = context.Request.Form["userId"];
            string isAgent = context.Request.Form["isAgent"];

            if (!isAgent.Equals("2") && !isAgent.Equals("1"))
            {
                result.Append("{\"code\":\"119\",");
                result.Append("\"msg\":\"您没有权限查看代理列表\"}");
                context.Response.Write(result.ToString());
                context.Response.End();
            }
        
            string sqlAgent = @"select 
	MAX(tb3.ID) AS GroupUserId,
	MAX(tb3.Name) AS Name,
    MAX(tb3.NickName) AS NickName,
    MAX(tb3.Balance) AS Balance,

    COUNT(temp2.UserID) as cnt,
    isnull(MAX(temp2.ReferId) ,0) AS ReferId,
    isnull(SUM(temp2.pay),0) AS pay,
    isnull(SUM(temp2.handsel),0) AS handsel,
    isnull(SUM(temp2.dis),0) AS dis,
    isnull(SUM(temp2.win),0) AS win
    from
        [dbo].[T_Users] tb3   left join
(

        SELECT 
			   MAX(temp.UserID) as UserID,
               COUNT(temp.UserID) as cnt,
               MAX(temp.ReferId) AS ReferId,
               SUM(temp.pay) AS pay,
               SUM(temp.handsel) AS handsel,
               SUM(temp.dis) AS dis,
               SUM(temp.win) AS win
        FROM
        (
            SELECT u.SiteID,
                   u.ID AS UserID,
                   u.ReferId as  ReferId,
                   0.00 AS pay,
                   0.00 AS handsel,
                   0.00 AS dis,
                   0.00 AS win
            FROM dbo.T_Users u
            UNION ALL
            SELECT p.SiteID,
                   p.UserID,
                   0 as ReferId,
                   p.Money AS pay,
                   0.00 AS handsel,
                   0.00 AS dis,
                   0.00 AS win
            FROM dbo.T_UserPayDetails p
            WHERE p.Result = 1
                  AND p.Money>0
            UNION ALL
            SELECT p.SiteID,
                   p.UserID,
                   0 as ReferId,
                   0.00 AS pay,
                   p.HandselMoney AS handsel,
                   0.00 AS dis,
                   0.00 AS win
            FROM dbo.T_UserPayDetails p
            WHERE p.Result = 1
                  AND p.HandselMoney>0
            UNION ALL
            SELECT d.SiteID,
                   d.UserID,
                   0 AS ReferId,
                   0.00 AS pay,
                   0.00 AS handsel,
                   d.Money AS dis,
                   0.00 AS win
            FROM dbo.T_UserDistills d
            WHERE d.Result = 1 
            UNION ALL
            SELECT w.SiteID,
                   w.InitiateUserID AS UserID,
                   0 AS ReferId,
                   0.00 AS pay,
                   0.00 AS handsel,
                   0.00 AS dis,
                   w.Money AS win
            FROM dbo.T_Schemes w
            WHERE  w.[QuashStatus]=0 and w.[Buyed]=1
        ) AS temp
        group by temp.UserID
)AS temp2
        
		on {0} where {1}
		group by tb3.ID
		";
            String condition1 = "";
            if (isAgent.Equals("2"))
            {
                condition1 = String.Format("temp2.ReferId=tb3.ID");
            }
            else
            {
                condition1 = String.Format("temp2.UserID=tb3.ID");
            }
            String condition2 = String.Format("tb3.[ReferId]={0}", userId);


            sqlAgent = String.Format(sqlAgent, condition1, condition2);
            System.Data.DataTable dtAgent = Shove.Database.MSSQL.Select(sqlAgent);


            if (dtAgent.Rows.Count <= 0)
            {
                result.Append("{\"code\":\"119\",");
                result.Append("\"msg\":\"您暂时还没有推荐任何会员加入\"}");
                context.Response.Write(result.ToString());
                context.Response.End();
            }


            result.Append("{\"code\":\"200\",\"msg\":[");
            int index = 0;
            foreach (System.Data.DataRow row in dtAgent.Rows)
            {
                if (index == 0)
                {
                    result.Append("{\"uid\":\"" + row["GroupUserId"] + "\",\"username\":\"" + row["Name"] + "\",\"wxname\":\"" + row["NickName"] + "\",\"Balance\":\"" + row["Balance"] + "\",\"pay\":\"" + row["pay"] + "\",\"handsel\":\"" + row["handsel"] + "\",\"dis\":\"" + row["dis"] + "\",\"win\":\"" + row["win"] + "\"}");
                }
                else
                {
                    result.Append(",{\"uid\":\"" + row["GroupUserId"] + "\",\"username\":\"" + row["Name"] + "\",\"wxname\":\"" + row["NickName"] + "\",\"Balance\":\"" + row["Balance"] + "\",\"pay\":\"" + row["pay"] + "\",\"handsel\":\"" + row["handsel"] + "\",\"dis\":\"" + row["dis"] + "\",\"win\":\"" + row["win"] + "\"}");
                }
                index++;
            }
            result.Append("]}" );
            context.Response.Write(result.ToString());
            context.Response.End();
        }
        
        
        //上分请求
        if (action.Equals("sendCharge"))
        {
            string userId = context.Request.Form["userId"];
            string paymethod = context.Request.Form["paymethod"];
            string money = context.Request.Form["money"];
            StringBuilder sb = new StringBuilder();

            int out_money = 0;
            if (!int.TryParse(money, out out_money))
            {
                sb.Append("{\"error\":\"-100\",");
                sb.Append("\"msg\":\"上分金额有错误。\"}");
                context.Response.Clear();
                context.Response.Write(sb.ToString());
                context.Response.End();
            }

            if (out_money < 20)
            {
                sb.Append("{\"error\":\"-101\",");
                sb.Append("\"msg\":\"上分金额必须大于20。\"}");
                context.Response.Clear();
                context.Response.Write(sb.ToString());
                context.Response.End();
            }

           

            DAL.Tables.T_UserPayDetails payTable = new DAL.Tables.T_UserPayDetails();
            payTable.SiteID.Value = 1;
            payTable.UserID.Value = int.Parse(userId);
            payTable.DateTime.Value = DateTime.Now;
            payTable.PayType.Value = paymethod;
            payTable.Money.Value = out_money;
            payTable.FormalitiesFees.Value =0;
            payTable.Result.Value =0;
            payTable.AlipayNo.Value ="";
            payTable.HandselMoney.Value = 0;

            

            if (payTable.Insert()<=0)
            {
                sb.Append("{\"error\":\"-99\",");
                sb.Append("\"msg\":\"上分请求提交失败。\"}");
                context.Response.Clear();
                context.Response.Write(sb.ToString());
                context.Response.End();
            }
            else
            {
                sb.Append("{\"error\":\"0\",");
                sb.Append("\"msg\":\"上分请求提交成功。\"}");
                context.Response.Clear();
                context.Response.Write(sb.ToString());
                context.Response.End();
            }
            
        }
        
        
        //获取投注历史
        if (action.Equals("getBetHistory"))
        {
            string lotteryId = context.Request.Form["lotteryId"];
            string hallIndex = context.Request.Form["hallIndex"];
            
            int index = 0;
            string sql = @"select top 20 81 as [type], c.ID,a.LotteryNumber,a.Money,b.DateTime,c.NickName,c.HeadUrl from T_SchemesFrom sf left join 
            T_SchemesMixcast a on sf.[SchemesID]=a.[SchemeId] left join T_Schemes b on a.SchemeId=b.ID left join T_Users c on  b.InitiateUserID=c.ID 
            where (b.[PlayTypeID]=" + lotteryId + "01 or b.[PlayTypeID]=" + lotteryId + "02 or b.[PlayTypeID]=" + lotteryId + "03) and sf.HomeIndex=" + hallIndex + " order by b.DateTime desc";
            System.Data.DataTable dtGroup = Shove.Database.MSSQL.Select(sql);
            if (dtGroup.Rows.Count <= 0)
            {
                result.Append("{\"error\":\"119\"}");
                context.Response.Write(result.ToString());
                context.Response.End();
            }
            //假人记录
            string sqlfake = @"select top 20 * from T_FakeBet 
            where lotteryID=" + lotteryId + " and ([homeIndex]=" + hallIndex + " or [NickName]='Admin') order by [DateTime] desc";
            System.Data.DataTable dtFake = Shove.Database.MSSQL.Select(sqlfake);
            foreach (System.Data.DataRow row in dtFake.Rows)
            {
                System.Data.DataRow newRow;
                newRow = dtGroup.NewRow();
                newRow["type"] = row["type"];
                newRow["DateTime"] = row["DateTime"];
                newRow["ID"] = "0";
                newRow["NickName"] = row["NickName"];
                newRow["HeadUrl"] = row["Avatar"];
                newRow["LotteryNumber"] = row["LotteryNumber"];
                newRow["Money"] = row["Money"];
                dtGroup.Rows.Add(newRow);

            }
            

            result.Append("{\"error\":\"0\",\"msg\":[");
            
            //foreach (System.Data.DataRow row in dtGroup.Rows)
            
            foreach (System.Data.DataRow row in dtGroup.Select("1=1", "DateTime asc"))
            {
                if(index<dtGroup.Rows.Count-20){
                    index++;
                    continue;
                }

                if (index == dtGroup.Rows.Count - 20)
                {
                    result.Append("{\"type\":\"" + row["type"] + "\",\"sentTime\":\"" + row["DateTime"] + "\",\"senderUserId\":\"" + row["ID"] + "\" ,\"nickname\":\"" + row["NickName"] + "\" ,\"avatar\":\"" + row["HeadUrl"] + "\" ,\"betType\":\"" + row["LotteryNumber"] + "\" ,\"money\":\"" + row["Money"] + "\"    }");
                }
                else
                {
                    result.Append(",{\"type\":\"" + row["type"] + "\",\"sentTime\":\"" + row["DateTime"] + "\",\"senderUserId\":\"" + row["ID"] + "\" ,\"nickname\":\"" + row["NickName"] + "\" ,\"avatar\":\"" + row["HeadUrl"] + "\" ,\"betType\":\"" + row["LotteryNumber"] + "\" ,\"money\":\"" + row["Money"] + "\"    }");
                }
                index++;
            }
            result.Append("]}");
            context.Response.Write(result.ToString());
        }
        
        
         //插入开盘封盘的记录
        if (action.Equals("pushMessage"))
        {
            string messtype = context.Request.Form["messtype"];
            string lotteryId = context.Request.Form["lotteryId"];
            string periods_current = context.Request.Form["periods_current"];
            
            string sql = @"select ID from T_FakeBet where [type]="+messtype+" and [Avatar]='"+periods_current+"'  and [lotteryID]=" + lotteryId;
            System.Data.DataTable dtGroup = Shove.Database.MSSQL.Select(sql);
            if (dtGroup.Rows.Count <= 0)
            {
                string isql = "insert into T_FakeBet ([NickName],[Money],[LotteryNumber],[lotteryID],[Avatar],[type]) values ('Admin',0,''," + lotteryId + ",'" + periods_current + "'," + messtype + ")";
                Shove.Database.MSSQL.ExecuteNonQuery(isql);
                
            }
            
            context.Response.Write("{\"error\":\"0\"}");
            context.Response.End();
        }


        //获取提现记录
        if (action.Equals("getDislist"))
        {
            string userId = context.Request.Form["userId"];

            int index = 0;
            string sql = @"select top 20 * from T_UserDistills where UserID=" + userId + " order by DateTime desc ";
            System.Data.DataTable dtGroup = Shove.Database.MSSQL.Select(sql);
            if (dtGroup.Rows.Count <= 0)
            {
                result.Append("{\"error\":\"119\"}");
                context.Response.Write(result.ToString());
                context.Response.End();
            }


            result.Append("{\"error\":\"0\",\"msg\":[");

            //foreach (System.Data.DataRow row in dtGroup.Rows)
            foreach (System.Data.DataRow row in dtGroup.Rows)
            {
                DateTime disDate = Convert.ToDateTime(row["DateTime"].ToString());
                string distime = disDate.ToShortDateString();;
                string Result = getChargeStat(row["Result"].ToString());
                
                if (index == 0)
                {
                    result.Append("{\"DateTime\":\"" + distime + "\",\"Money\":\"" + row["Money"] + "\" ,\"Result\":\"" + Result + "\" ,\"Memo\":\"" + row["Memo"]+ "\"    }");
                }
                else
                {
                    result.Append(",{\"DateTime\":\"" + distime + "\",\"Money\":\"" + row["Money"] + "\" ,\"Result\":\"" + Result + "\" ,\"Memo\":\"" + row["Memo"] + "\"    }");
                }
                index++;
            }
            result.Append("]}");
            context.Response.Write(result.ToString());
        }
        
        
        
        //删除会员
        if (action.Equals("deleteUser"))
        {
            string userId = context.Request.Form["userId"];
            try
            {
                //删除会员
                string sql = @"delete from T_Users where ID=" + userId;
                Shove.Database.MSSQL.ExecuteNonQuery(sql);
                //删除详情
                sql = @"delete from [T_UserToken] where [userId]=" + userId;
                Shove.Database.MSSQL.ExecuteNonQuery(sql);
                //删除投注
                sql = @"delete from [T_Schemes] where [InitiateUserID]=" + userId;
                Shove.Database.MSSQL.ExecuteNonQuery(sql);
                //删除充值
                sql = @"delete from [T_UserPayDetails] where [UserID]=" + userId;
                Shove.Database.MSSQL.ExecuteNonQuery(sql);
                
                sql = @"delete from [T_UserDetails] where [UserID]=" + userId;
                Shove.Database.MSSQL.ExecuteNonQuery(sql);
                //删除提现
                sql = @"delete from [T_UserDistills] where [InitiateUserID]=" + userId;
                Shove.Database.MSSQL.ExecuteNonQuery(sql);



                context.Response.Write("{\"error\":\"0\"}");
            }
            catch (Exception)
            {
                context.Response.Clear();
                context.Response.Write("{\"error\":\"-90\"}");

            }
        }
        
        
       
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }


    public string getChargeStat(string stat)
    {
        if (stat.Equals("0"))
        {
            return "<font color='#ff0000'>申请中</font>";
        }
        else if (stat.Equals("1"))
        {
            return "<font color='#11cc22'>成功</font>";
        }
        else
        {
            return "<font color='#1122ff'>拒绝</font>";
        }
    }


}