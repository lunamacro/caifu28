<%@ WebHandler Language="C#" Class="Handsel" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;

public class Handsel : IHttpHandler
{

    HttpContext hc;
    public void ProcessRequest(HttpContext context)
    {
        hc = context;
        string act = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["act"]);
        switch (act)
        {
            case "GetQuota":
                hc.Response.Write(GetQuota());
                break;
            case "DeleteRules":
                hc.Response.Write(DeleteRules());
                break;
            case "AddActivity":
                hc.Response.Write(AddActivity());
                break;
            case "GetHandselRule":
                hc.Response.Write(GetHandselRule());
                break;
            case "GetHandselSection":
                hc.Response.Write(GetHandselSection());
                break;
            case "GetCurrentEffective":
                hc.Response.Write(GetCurrentEffective());
                break;
            case "GetHandselMoney":
                hc.Response.Write(GetHandselMoney());
                break;
            case "CheckActive":
                hc.Response.Write(CheckActive());
                break;
        }
    }
    /// <summary>
    /// 获取配置信息
    /// </summary>
    /// <returns></returns>
    private string GetQuota()
    {
        string handselRuleID = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["handselRuleID"]);
        DAL.Tables.T_HandselSection dal = new DAL.Tables.T_HandselSection();
        DataTable dt = dal.Open("ID,HandselRuleID,ConditionLowest,ConditionHighest,Numerical", "HandselRuleID=" + handselRuleID, "Numerical asc");
        return JsonHelper.DataTableToJSON(dt, "handselsection");
    }
    /// <summary>
    /// 删除配置信息
    /// </summary>
    /// <returns></returns>
    private string DeleteRules()
    {
        string HandselRuleID = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["HandselRuleID"]);
        ReturnMsgBase model = new ReturnMsgBase();
        try
        {
            DAL.Tables.T_HandselSection sectionDal = new DAL.Tables.T_HandselSection();
            DAL.Tables.T_HandselRule ruleDal = new DAL.Tables.T_HandselRule();
            var sectionNum = sectionDal.Delete("HandselRuleID=" + HandselRuleID);
            var ruleNum = ruleDal.Delete("ID=" + HandselRuleID);
            model.IsOk = true;
            model.Msg = "删除成功";
            model.Status = 1;
            return JsonHelper.ObjectToJson(model);
        }
        catch (Exception ex)
        {
            model.IsOk = false;
            model.Msg = "删除失败";
            model.Status = 0;
            return JsonHelper.ObjectToJson(model);
            throw ex;
        }
    }
    /// <summary>
    /// 添加活动
    /// </summary>
    /// <returns></returns>
    private string AddActivity()
    {
        string startTime = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["startTime"]) + " 00:00:00";
        string endTime = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["endTime"]) + " 23:59:59";
        string objType = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["objType"]);
        string giveType = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["giveType"]);
        string section = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["sectionList"]);
        string changeType = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["changeType"]);
        string handselRuleID = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["handselRuleID"]);
        ReturnMsgBase model = new ReturnMsgBase();
        DAL.Tables.T_HandselRule ruleDal = new DAL.Tables.T_HandselRule();
        DAL.Tables.T_HandselSection sectionDal = new DAL.Tables.T_HandselSection();

        StringBuilder where = new StringBuilder();
        where.Append("(((StartTime<='" + startTime + "'  AND  EndTime>='" + startTime + "') OR (StartTime<= '" + endTime + "' AND EndTime>='" + endTime + "'))OR StartTime >= '" + startTime + "' AND EndTime <= '" + endTime + "') ");
        //1.验证时间段是否有重叠
        if (handselRuleID != null)
        {
            where.Append("AND ID<>" + handselRuleID);
        }
        DataTable ruleDt = ruleDal.Open("ID,StartTime,EndTime,GiveObject,GiveType", where.ToString(), "");
        if (ruleDt != null)
        {
            if (ruleDt.Rows.Count > 0)
            {
                string tempObj = "";
                for (int i = 0; i < ruleDt.Rows.Count; i++)
                {
                    tempObj += ruleDt.Rows[i]["GiveObject"].ToString();
                    //时间有重叠,判断是否对同一对象
                    if (ruleDt.Rows[i]["GiveObject"].ToString() == "2" || ruleDt.Rows[i]["GiveObject"].ToString() == objType || objType == "2")
                    {
                        //条件不成立，返回失败
                        model.IsOk = false;
                        if (changeType == "update")
                        {
                            model.Msg = "修改失败，在此活动时间内已有类似的活动！";
                        }
                        else
                        {
                            model.Msg = "添加失败，在此活动时间内已有类似的活动！";
                        }
                        model.Status = 0;
                        return JsonHelper.ObjectToJson(model);
                    }
                }
                if (tempObj == "01" || tempObj == "10")
                {
                    //条件不成立，返回失败
                    model.IsOk = false;
                    if (changeType == "update")
                    {
                        model.Msg = "修改失败，在此活动时间内已有类似的活动！";
                    }
                    else
                    {
                        model.Msg = "添加失败，在此活动时间内已有类似的活动！";
                    }
                    model.Status = 0;
                    return JsonHelper.ObjectToJson(model);
                }
            }
        }
        //2.验证彩金区间是否有重叠
        List<HandselSection> list = JsonHelper.Json2Obj<List<HandselSection>>(section);
        if (list.Count > 1)
        {
            for (int i = 0; i < list.Count() - 1; i++)
            {
                for (int j = i + 1; j < list.Count(); j++)
                {
                    if (list[i].ConditionLowest >= list[j].ConditionLowest && list[i].ConditionLowest <= list[j].ConditionHighest)
                    {
                        //验证失败 
                        model.IsOk = false;
                        if (changeType == "update")
                        {
                            model.Msg = "修改失败，最低限额与最高限额区间有误(区间不可重叠)，请检查！";
                        }
                        else
                        {
                            model.Msg = "添加失败，最低限额与最高限额区间有误(区间不可重叠)，请检查！";
                        }
                        model.Status = 0;
                        return JsonHelper.ObjectToJson(model);
                    }
                    if (list[i].ConditionHighest <= list[j].ConditionHighest && list[i].ConditionHighest >= list[j].ConditionLowest)
                    {
                        //验证失败
                        model.IsOk = false;
                        if (changeType == "update")
                        {
                            model.Msg = "修改失败，最低限额与最高限额区间有误(区间不可重叠)，请检查！";
                        }
                        else
                        {
                            model.Msg = "添加失败，最低限额与最高限额区间有误(区间不可重叠)，请检查！";
                        }
                        model.Status = 0;
                        return JsonHelper.ObjectToJson(model);
                    }
                }
            }
        }
        //条件成立，添加活动规则
        try
        {
            //更新操作，先删除旧数据，再添加新数据
            if (changeType == "update")
            {
                ruleDal.Delete("ID=" + handselRuleID);
                sectionDal.Delete("HandselRuleID=" + handselRuleID);
            }
            //添加规则
            ruleDal.CreateTime.Value = DateTime.Now;
            ruleDal.EndTime.Value = endTime;
            ruleDal.GiveObject.Value = objType;
            ruleDal.GiveType.Value = giveType;
            ruleDal.StartTime.Value = startTime;
            long tempId = ruleDal.Insert();
            //添加区间
            for (int i = 0; i < list.Count(); i++)
            {
                sectionDal.ConditionHighest.Value = list[i].ConditionHighest;
                sectionDal.ConditionLowest.Value = list[i].ConditionLowest;
                sectionDal.HandselRuleID.Value = tempId;
                sectionDal.Numerical.Value = list[i].Numerical;
                sectionDal.Insert();
            }
            model.IsOk = true;
            if (changeType == "update")
            {
                model.Msg = "修改成功";
            }
            else
            {
                model.Msg = "添加成功";
            }
            model.Status = 1;
            return JsonHelper.ObjectToJson(model);
        }
        catch (Exception ex)
        {
            model.IsOk = false;
            if (changeType == "update")
            {
                model.Msg = "修改失败，系统异常";
            }
            else
            {
                model.Msg = "添加失败,系统异常";
            }
            model.Status = 0;
            return JsonHelper.ObjectToJson(model);
            throw ex;
        }
    }
    /// <summary>
    /// 获取规则信息
    /// </summary>
    /// <returns></returns>
    private string GetHandselRule()
    {
        string handselRuleId = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["handselRuleId"]);
        DAL.Tables.T_HandselRule handselRuleDal = new DAL.Tables.T_HandselRule();
        DataTable handselRuleDt = handselRuleDal.Open("StartTime,EndTime,GiveObject,GiveType", "ID=" + handselRuleId, "");
        return JsonHelper.ObjectToJson(handselRuleDt);
    }
    /// <summary>
    /// 获取区间信息
    /// </summary>
    /// <returns></returns>
    private string GetHandselSection()
    {
        string handselRuleId = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["handselRuleId"]);
        DAL.Tables.T_HandselSection handselSectionDal = new DAL.Tables.T_HandselSection();
        DataTable handselSectionDt = handselSectionDal.Open("ID,HandselRuleID,ConditionLowest,ConditionHighest,Numerical", "HandselRuleID=" + handselRuleId, "");
        return JsonHelper.ObjectToJson(handselSectionDt);
    }
    /// <summary>
    /// 获取当前用户有效活动信息
    /// </summary>
    /// <returns></returns>
    private string GetCurrentEffective()
    {
        Users _users = Users.GetCurrentUser(1);
        decimal amount = Convert.ToDecimal(Shove._Web.Utility.FilteSqlInfusion(hc.Request.QueryString["amount"]));
        int userId = Convert.ToInt32(_users.ID);
        string sqlStr = "SELECT ID,StartTime,EndTime,GiveObject,GiveType,CreateTime FROM dbo.T_HandselRule WHERE GETDATE() BETWEEN StartTime AND EndTime AND GiveObject=0 AND (SELECT RegisterTime FROM dbo.T_Users WHERE ID=@UID) BETWEEN StartTime AND EndTime UNION SELECT ID,StartTime,EndTime,GiveObject,GiveType,CreateTime FROM dbo.T_HandselRule WHERE GETDATE() BETWEEN StartTime AND EndTime AND GiveObject=1 AND (SELECT RegisterTime FROM dbo.T_Users WHERE ID=@UID) < StartTime UNION SELECT ID,StartTime,EndTime,GiveObject,GiveType,CreateTime FROM dbo.T_HandselRule WHERE GETDATE() BETWEEN StartTime AND EndTime AND GiveObject=2";
        Shove.Database.MSSQL.Parameter[] param ={
    new Shove.Database.MSSQL.Parameter("@UID",SqlDbType.Int,4,ParameterDirection.Input,userId)
    };
        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
        DataTable dt = Shove.Database.MSSQL.Select(conn, sqlStr, param);
        string ruleId = "";
        if (dt != null)
        {
            ruleId = dt.Rows[0]["ID"].ToString();
            DAL.Tables.T_HandselSection sectionDal = new DAL.Tables.T_HandselSection();
            DataTable sectionDt = sectionDal.Open("Numerical,ConditionLowest,ConditionHighest", "HandselRuleID=" + ruleId, "");
            for (int i = 0; i < sectionDt.Rows.Count; i++)
            {
                if (Convert.ToDecimal(sectionDt.Rows[i]["ConditionLowest"]) <= amount && amount <= Convert.ToDecimal(sectionDt.Rows[i]["ConditionHighest"]))
                {
                    return JsonHelper.ObjectToJson(sectionDt.Rows[i]["Numerical"]);
                }
            }
        }

        return JsonHelper.ObjectToJson(0);
    }
    /// <summary>
    /// 获取赠送彩金数
    /// </summary>
    /// <returns></returns>
    private string GetHandselMoney()
    {
        string handselMoney = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["handselMoney"]);
        string userName = Shove._Web.Utility.FilteSqlInfusion(hc.Request.Form["userName"]);
        DataTable dt = new DAL.Tables.T_Users().Open("ID", "Name='" + userName + "'", "");
        long userId = 0;
        if (dt.Rows.Count > 0)
        {
            userId = (long)dt.Rows[0][0];
            Regex r = new Regex(@"([1-9]\d*\.?\d*)|(0\.\d*[1-9])");
            if (!string.IsNullOrEmpty(handselMoney))
            {
                Match m = r.Match(handselMoney);
                if (m.Success)
                {
                    return PF.GetCurrentEffective(Convert.ToInt32(handselMoney), userId);
                }
            }
        }
        return "0.00";
    }
    /// <summary>
    /// 检查是否有有效的活动
    /// </summary>
    /// <returns></returns>
    private string CheckActive()
    {
        Users _user = Users.GetCurrentUser(1);
        Shove.Database.MSSQL.Parameter[] parms ={
                                               new Shove.Database.MSSQL.Parameter("@UserID",SqlDbType.Int,4,ParameterDirection.Input,_user.ID)
                                               };
        DataTable userDt = Shove.Database.MSSQL.Select("SELECT COUNT(*) FROM   dbo.T_UserDetails WHERE  UserID = @UserID AND OperatorType = @UserID", parms);
        int userType = 0;//新用户
        if (userDt.Rows.Count > 0)
        {
            userType = 1;//老用户
        }
        Shove.Database.MSSQL.Parameter[] parmsCheck = { 
                                                      new Shove.Database.MSSQL.Parameter("@GiveObject",SqlDbType.Int,4,ParameterDirection.Input,userType)
                                                      };
        DataTable dt = Shove.Database.MSSQL.Select(@"SELECT ID
                                                   FROM dbo.T_HandselRule
                                                   WHERE GETDATE() BETWEEN StartTime AND EndTime
                                                   AND ( GiveObject = @GiveObject
                                                   OR GiveObject = 2
                                                )", parmsCheck);
        if (dt.Rows.Count > 0)
        {
            return "T";
        }
        return "N";
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
public class HandselSection
{
    /// <summary>
    /// 最低限额
    /// </summary>
    public double ConditionLowest { get; set; }
    /// <summary>
    /// 最高限额
    /// </summary>
    public double ConditionHighest { get; set; }
    /// <summary>
    /// 数值
    /// </summary>
    public float Numerical { get; set; }
}