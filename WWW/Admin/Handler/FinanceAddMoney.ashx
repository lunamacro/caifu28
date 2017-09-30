<%@ WebHandler Language="C#" Class="FinanceAddMoney" %>

using System;
using System.Web;

public class FinanceAddMoney : IHttpHandler
{
    HttpContext hc;
    public void ProcessRequest(HttpContext context)
    {
        hc = context;
        string act = hc.Request.Form["act"];
        switch (act)
        {
            case "ResetStatus":
                hc.Response.Write(ResetStatus());
                break;
        }
    }
    private string ResetStatus()
    {
        string id = hc.Request.Form["ID"];
        string result = hc.Request.Form["Result"];
        if (!string.IsNullOrEmpty(id))
        {
            
            System.Data.DataTable userPayDetail = new DAL.Tables.T_UserPayDetails().Open(" UserID, PayType, Money, Result ", string.Format(" ID = {0}", id), "");

            string sqlStr = "UPDATE dbo.V_UserPayDetails SET Result={0} WHERE ID={1}";
            sqlStr = string.Format(sqlStr, result, id);
            int i = Shove.Database.MSSQL.ExecuteNonQuery(sqlStr);

            if (0 < userPayDetail.Rows.Count)
            {
                object resultOld = userPayDetail.Rows[0]["Result"];
                object payType = userPayDetail.Rows[0]["PayType"];
                if (Convert.ToInt32(result) != Convert.ToInt32(resultOld) && (payType.Equals("0") || payType.Equals("1") || payType.Equals("2")))
                {
                    double money = Convert.ToDouble(userPayDetail.Rows[0]["Money"]);

                    if (result.Equals("1"))
                    {
                        Users tu = new Users(1);
                        string Messages = "用户充值";
                        string ReturnDescription = "";
                        long uid = Shove._Convert.StrToLong(userPayDetail.Rows[0]["UserID"].ToString(), -1);
                        int isHand = 0;


                        string sqlStrup = "UPDATE  T_Users SET Balance = Balance +{0} WHERE   [ID] = {1} ";
                        sqlStrup = string.Format(sqlStrup, money, uid);
                        int r1 = Shove.Database.MSSQL.ExecuteNonQuery(sqlStrup);

                        string sqlStrinsert = "INSERT  INTO T_UserDetails ( SiteID , UserID , OperatorType ,[Money] ,  [Memo] ,  OperatorID , HandselAmount)  VALUES  ( 1 , " + uid + " ,1 ," + money + " ,'在线充值' , 60 , 0  )  ";
                        int r2 = Shove.Database.MSSQL.ExecuteNonQuery(sqlStrinsert);
                        
                        //if (tu.AddUserBalanceManual(money, uid, Messages, 69, isHand, ref ReturnDescription) < 0)
                        //if (tu.AddUserBalance(money, uid, Messages, 69, isHand, ref ReturnDescription) < 0)
                        //{
                           // PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_UserAddMoney");
                            //return "{\"status\":\"-1\"}";
                        //}
                    }
                }
            }

            return "{\"status\":\"" + i.ToString() + "\"}";
        }
        return "{\"status\":\"-1\"}";
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}