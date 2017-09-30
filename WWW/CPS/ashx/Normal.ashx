<%@ WebHandler Language="C#" Class="Normal" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

public class Normal : IHttpHandler
{
    HttpContext hc;
    public void ProcessRequest(HttpContext context)
    {
        hc = context;
        string act = hc.Request.Form["act"];
        switch (act)
        {
            case "ApplyPromoter":
                hc.Response.Write(ApplyPromoter());
                break;
            case "ApplyAgent":
                hc.Response.Write(ApplyAgent());
                break;
            case "IgnoreCPS":
                hc.Response.Write(IgnoreCPS());
                break;
            case "AgentManagementEnabled":
                hc.Response.Write(AgentManagementEnabled());
                break;
            case "isLoginOut":
                hc.Response.Write(isLoginOut());
                break;
            case "PassAudit":
                hc.Response.Write(PassAudit());
                break;
            case "DeleteAuditLog":
                hc.Response.Write(DeleteAuditLog());
                break;
            case "SetCommissionRate":
                hc.Response.Write(SetCommissionRate());
                break;
            case "PayOff":
                hc.Response.Write(PayOff());
                break;
            case "PayOffAll":
                hc.Response.Write(PayOffAll());
                break;
            case "IgnoreApplication":
                hc.Response.Write(IgnoreApplication());
                break;
            case "SetCommissionRateSingle":
                hc.Response.Write(SetCommissionRateSingle());
                break;
            case "SetCommissionRateAll":
                hc.Response.Write(SetCommissionRateAll());
                break;
            case "GetHideUserList":
                hc.Response.Write(GetHideUserList());
                break;
            case "SaveCommissionRateSingleUser":
                hc.Response.Write(SaveCommissionRateSingleUser());
                break;
            case "SaveCommissionRateAllUser":
                hc.Response.Write(SaveCommissionRateAllUser());
                break;
        }
    }
    /// <summary>
    /// 申请成为推广员
    /// </summary>
    /// <returns></returns>
    private string ApplyPromoter()
    {
        string pageUrl = "";
        string result = "";
        long userId = Convert.ToInt64(hc.Request.Form["ID"]);
        string url = hc.Request.Form["url"];
        Sites _Site = new Sites()[1];
        if (CPSBLL.CheckIsBindMobile(userId))//手机号是否验证
        {
            /*
             *  验证是否已经申请过了
             */
            long count = new DAL.Tables.T_Cps().GetCount("OwnerUserID=" + userId);
            if (count > 0)
            {
                DataTable dt1 = new DAL.Tables.T_Cps().Open("ID,OwnerUserID,HandleResult,Type,HandlelDateTime", "OwnerUserID=" + userId, "");
                if (null == dt1 || dt1.Rows.Count == 0)
                {
                    result = "已经申请过推广员,获取申请信息失败";
                }
                else
                {
                    DataRow dr = dt1.Rows[0];
                    /*
                     *  0:  审核中
                     *  1:  审核完成
                     *  -2 被拒绝
                     */
                    string handleResult = dr["HandleResult"].ToString();
                    if ("0" == handleResult)
                    {
                        pageUrl = "Audit.aspx?Type=Audit";
                        result = "申请信息审核中";
                    }
                    if ("1" == handleResult)
                    {
                        //判断用户属于那个类型的，1是代理商 2是推广员
                        switch (dr["Type"].ToString())
                        {
                            case "2":
                                pageUrl = "Promote/PromoteIndex.aspx";
                                result = "你已经是CPS推广员，不需要重复操作";
                                break;
                            case "1":
                                pageUrl = "Agent/AgentIndex.aspx";
                                result = "你已经是CPS代理商了，不需要重复操作";
                                break;
                        }
                    }
                    else
                    {
                        result = "申请成功,申请信息审核中";
                        pageUrl = "Audit.aspx?Type=Audit";
                        DAL.Tables.T_Cps c = new DAL.Tables.T_Cps();
                        c.HandleResult.Value = 0;
                        c.Type.Value = 2;
                        c.ResultMemo.Value = "";
                        c.HandlelDateTime.Value = DateTime.Now;
                        c.Update("ID=" + dr["ID"]);
                    }
                }
                return "{\"result\":\"" + result + "\",\"url\":\"" + pageUrl + "\"}";
            }
            else
            {
                DAL.Tables.T_Cps cps = new DAL.Tables.T_Cps();
                cps.SiteID.Value = 1;
                cps.OwnerUserID.Value = userId;
                cps.DateTime.Value = DateTime.Now.ToString();
                cps.ON.Value = true;
                cps.Type.Value = 2;
                cps.ParentID.Value = -1;
                cps.OperatorID.Value = 1;   //操作员
                cps.HandlelDateTime.Value = DateTime.Now;

                string sql = "select PromoterAuditings from T_Sites";
                DataTable dt = Shove.Database.MSSQL.Select(sql);
                if (null != dt && dt.Rows.Count > 0 && !(Shove._Convert.StrToBool(dt.Rows[0]["PromoterAuditings"].ToString(), true)))//不需要审核
                {
                    cps.HandleResult.Value = 1;
                    cps.SerialNumber.Value = CPSBLL.CreateSerialNumber();
                    pageUrl = "Promote/PromoteIndex.aspx";
                }
                else
                {
                    cps.HandleResult.Value = 0;
                    pageUrl = "Audit.aspx?Type=Audit";
                }
                long cpsID = cps.Insert();
                if (cpsID < 0)
                {
                    return "{\"result\":\"申请失败\",\"url\":\"\"}";
                }
                if (!Shove._Convert.StrToBool(dt.Rows[0]["PromoterAuditings"].ToString(), true))
                {
                    DAL.Tables.T_CpsUserChange tcpsUserChange = new DAL.Tables.T_CpsUserChange();
                    if (tcpsUserChange.GetCount("ChangeType < 1 and Type = 2 and UserID = " + userId) < 1)//推广员
                    {
                        tcpsUserChange.UserID.Value = userId;
                        tcpsUserChange.DateTime.Value = DateTime.Now;
                        tcpsUserChange.Type.Value = 2;
                        tcpsUserChange.OperatorID.Value = 1;
                        tcpsUserChange.ChangeType.Value = -1;

                        long v = tcpsUserChange.Insert();
                        if (v < 0)
                        {
                            return "{\"result\":\"申请失败\",\"url\":\"\"}";
                        }
                    }
                    //读取站点的默认佣金比例
                    DAL.Tables.T_CpsSiteBonusScale tCpsSiteBonusScale = new DAL.Tables.T_CpsSiteBonusScale();
                    dt = tCpsSiteBonusScale.Open("LotteryID,PromoterBonusScale", "LotteryID in (" + _Site.UseLotteryListQuickBuy + ")", "id asc");
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        DataTable dt2 = Shove.Database.MSSQL.Select("select top 1 memo from T_CpsUsersBonusScaleLog order by id desc");

                        long Scaleid = 1;
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            Scaleid = Shove._Convert.StrToLong(dt2.Rows[0][0].ToString(), 1) + 1;
                        }
                        //循环为这个代理商生成佣金比例数据
                        foreach (DataRow dr in dt.Rows)
                        {
                            string returnDesc = "";
                            int v = CPSBLL.SetCommission(cpsID, Convert.ToInt64(dr["LotteryID"]), Convert.ToDouble(dr["PromoterBonusScale"]), Scaleid, ref returnDesc);
                            if (v < 0)
                            {
                                return "{\"result\":\"申请失败\",\"url\":\"\"}";
                            }
                        }
                    }
                }
                return "{\"result\":\"申请成功，申请信息审核中\",\"url\":\"" + pageUrl + "\"}";
            }
        }
        else
        {
            return "{\"result\":\"申请推广员必须先绑定手机号码才可以\",\"url\":\"" + "/CPS/MobileValided.aspx?returnUrl=" + url + "\"}";
        }
    }
    private string ApplyAgent()
    {
        string pageUrl = "";
        string result = "";
        long userId = Convert.ToInt64(hc.Request.Form["ID"]);
        string url = hc.Request.Form["url"];
        Sites _Site = new Sites()[1];
        if (CPSBLL.CheckIsBindMobile(userId))//手机号是否验证
        {
            /*
             *  验证是否已经申请过了
             */
            long count = new DAL.Tables.T_Cps().GetCount("OwnerUserID=" + userId);
            if (count > 0)
            {
                DataTable dt1 = new DAL.Tables.T_Cps().Open("ID,OwnerUserID,HandleResult,Type,HandlelDateTime", "OwnerUserID=" + userId, "");
                if (null == dt1 || dt1.Rows.Count == 0)
                {
                    result = "已经申请过了，获取申请信息失败";
                }
                else
                {
                    DataRow dr = dt1.Rows[0];
                    /*
                     *  0:  审核中
                     *  1:  审核完成
                     */
                    string handleResult = dr["HandleResult"].ToString();
                    if ("0" == handleResult)
                    {
                        pageUrl = "Audit.aspx?Type=Audit";
                        result = "申请信息审核中";
                    }
                    if ("1" == handleResult)
                    {
                        //判断用户属于那个类型的，1是代理商 2是推广员
                        switch (dr["Type"].ToString())
                        {
                            case "2":
                                pageUrl = "Promote/PromoteIndex.aspx";
                                result = "你已经是CPS推广员，不需要重复操作";
                                break;
                            case "1":
                                pageUrl = "Agent/AgentIndex.aspx";
                                result = "你已经是CPS代理商了，不需要重复操作";
                                break;
                        }
                    }
                    else
                    {
                        result = "申请成功,申请信息审核中";
                        pageUrl = "Audit.aspx?Type=Audit";
                        DAL.Tables.T_Cps c = new DAL.Tables.T_Cps();
                        c.Type.Value = 1;
                        c.HandleResult.Value = 0;
                        c.ResultMemo.Value = "";
                        c.HandlelDateTime.Value = DateTime.Now;
                        c.Update("ID=" + dr["ID"]);
                    }
                }
                return "{\"result\":\"" + result + "\",\"url\":\"" + pageUrl + "\"}";
            }
            else
            {
                DAL.Tables.T_Cps cps = new DAL.Tables.T_Cps();
                cps.SiteID.Value = 1;
                cps.OwnerUserID.Value = userId;
                cps.DateTime.Value = DateTime.Now.ToString();
                cps.ON.Value = true;
                cps.Type.Value = 1;
                cps.ParentID.Value = -1;
                cps.OperatorID.Value = 1;   //操作员
                cps.HandlelDateTime.Value = DateTime.Now;

                string sql = "select AgentAuditings from T_Sites";
                DataTable dt = Shove.Database.MSSQL.Select(sql);
                if (null != dt && dt.Rows.Count > 0 && !(Shove._Convert.StrToBool(dt.Rows[0]["AgentAuditings"].ToString(), true)))//不需要审核
                {
                    cps.HandleResult.Value = 1;
                    cps.SerialNumber.Value = CPSBLL.CreateSerialNumber();
                    pageUrl = "Agent/AgentIndex.aspx";
                }
                else
                {
                    cps.HandleResult.Value = 0;
                    pageUrl = "Audit.aspx?Type=Audit";
                }
                long cpsID = cps.Insert();
                if (cpsID < 0)
                {
                    return "{\"result\":\"申请失败\",\"url\":\"\"}";
                }
                if (!Shove._Convert.StrToBool(dt.Rows[0]["AgentAuditings"].ToString(), true))
                {
                    DAL.Tables.T_CpsUserChange tcpsUserChange = new DAL.Tables.T_CpsUserChange();
                    if (tcpsUserChange.GetCount("ChangeType < 1 and Type = 1 and UserID = " + userId + " ") < 1)
                    {
                        tcpsUserChange.UserID.Value = userId;
                        tcpsUserChange.DateTime.Value = DateTime.Now;
                        tcpsUserChange.Type.Value = 1;
                        tcpsUserChange.OperatorID.Value = 1;
                        tcpsUserChange.ChangeType.Value = -1;

                        long v = tcpsUserChange.Insert();
                        if (v < 0)
                        {
                            return "{\"result\":\"申请失败\",\"url\":\"\"}";
                        }
                    }

                    DAL.Tables.T_CpsSiteBonusScale tCpsSiteBonusScale = new DAL.Tables.T_CpsSiteBonusScale();
                    dt = tCpsSiteBonusScale.Open("LotteryID,AgentBonusScale", "LotteryID in (" + _Site.UseLotteryListQuickBuy + ")", "id asc");
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        DataTable dt2 = Shove.Database.MSSQL.Select("select top 1 memo from T_CpsUsersBonusScaleLog order by id desc");

                        long Scaleid = 1;
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            Scaleid = Shove._Convert.StrToLong(dt2.Rows[0][0].ToString(), 1) + 1;
                        }
                        //循环为这个代理商生成佣金比例数据
                        foreach (DataRow dr in dt.Rows)
                        {
                            string returnDesc = "";
                            int v = CPSBLL.SetCommission(cpsID, Convert.ToInt64(dr["LotteryID"]), Convert.ToDouble(dr["AgentBonusScale"]), Scaleid, ref returnDesc);
                            if (v < 0)
                            {
                                return "{\"result\":\"申请失败\",\"url\":\"\"}";
                            }
                        }
                    }
                }
                return "{\"result\":\"申请成功，申请信息审核中\",\"url\":\"" + pageUrl + "\"}";
            }
        }
        else
        {
            return "{\"result\":\"申请代理商必须先绑定手机号码才可以，申请信息审核中\",\"url\":\"" + "/CPS/MobileValided.aspx?returnUrl=" + url + "\"}";
        }
    }
    /// <summary>
    /// 忽略商家申请
    /// </summary>
    /// <returns></returns>
    private string IgnoreCPS()
    {
        long cpsID = Shove._Convert.StrToLong(hc.Request.Form["cpsId"], -1);
        string url = hc.Request.Form["url"];
        if (-1 == cpsID)
        {
            return "{\"result\":\"修改失败\",\"url\":\"" + url + "\"}";
        }
        DataTable dt = new DAL.Tables.T_Cps().Open("ID,OwnerUserID,HandleResult,[ON],HandlelDateTime", "ID=" + cpsID, "");
        if (null == dt || dt.Rows.Count == 0)
        {
            return "{\"result\":\"数据库繁忙，获取用户信息失败\",\"url\":\"\"}";
        }
        DataRow dr = dt.Rows[0];
        if (Convert.ToInt32(dr["HandleResult"] + "") != 0)
        {
            return "{\"result\":\"该商家已经被处理\",\"url\":\"\"}";
        }
        Users _User = Users.GetCurrentUser(1);
        Users user = new Users(1)[1, Shove._Convert.StrToLong(dt.Rows[0]["OwnerUserID"].ToString(), -1)];

        //写审核记录
        DAL.Tables.T_AuditingRecords record = new DAL.Tables.T_AuditingRecords();
        record.CpsID.Value = cpsID;
        record.OwnerUserID.Value = user.ID;
        record.OwnerUserName.Value = user.Name;
        record.OwnerUserMobile.Value = user.Mobile;
        record.ApplyDateTime.Value = dt.Rows[0]["HandlelDateTime"];
        record.HandlerDateTime.Value = DateTime.Now;
        record.HandlerResult.Value = -1;
        record.OperateID.Value = _User.ID;
        long recordID = record.Insert();

        //更新T_Cps表
        DAL.Tables.T_Cps cps = new DAL.Tables.T_Cps();
        cps.HandleResult.Value = -1;
        cps.HandlelDateTime.Value = DateTime.Now;
        long result = cps.Update("ID =" + cpsID);
        if (result < 0)
        {
            return "{\"result\":\"修改失败\",\"url\":\"" + url + "\"}";
        }
        return "{\"result\":\"修改成功\",\"url\":\"" + url + "\"}";
    }
    /// <summary>
    /// 启用或禁用商家
    /// </summary>
    /// <returns></returns>
    private string AgentManagementEnabled()
    {
        long userID = Shove._Convert.StrToLong(hc.Request.Form["userId"] + "", -1);
        string commandArgument = hc.Request.Form["commandArgument"];
        string url = hc.Request.Form["url"];
        DAL.Tables.T_Cps cps = new DAL.Tables.T_Cps();
        if (Convert.ToBoolean(commandArgument))
        {
            cps.ON.Value = false;
        }
        else
        {
            cps.ON.Value = true;
        }
        long result = cps.Update("OwnerUserID = " + userID);
        if (result < 0)
        {
            return "{\"result\":\"操作失败\",\"url\":\"" + url + "\"}";
        }
        return "{\"result\":\"操作成功\",\"url\":\"" + url + "\"}";
    }
    /// <summary>
    /// 退出系统
    /// </summary>
    /// <returns></returns>
    private string isLoginOut()
    {
        Users _user = new Users(1);
        string ReturnDescription = "";
        int result = 0;
        try
        {
            result = _user.Logout(ref ReturnDescription);
        }
        catch
        {

        }
        if (0 == result)
        {
            return "{\"url\":\"../Default.aspx\"}";
        }
        return "{\"url\":\"\"}";
    }
    /// <summary>
    /// 代理商通过审核
    /// </summary>
    /// <returns></returns>
    private string PassAudit()
    {
        string CpsID = hc.Request.Form["CpsID"];
        string Type = hc.Request.Form["Type"];
        string requestUrl = hc.Request.Form["RequestUrl"];
        long userId = Convert.ToInt64(hc.Request.Form["userId"]);
        string json = hc.Request.Form["json"];

        DataTable dt = new DAL.Tables.T_Cps().Open("ID,ParentID,OwnerUserID,HandleResult,[ON],HandlelDateTime", "ID=" + CpsID, "");
        if (null == dt || dt.Rows.Count == 0)
        {
            return "{\"result\":\"数据库繁忙，获取用户信息失败\",\"url\":\"\"}";
        }
        if ((dt.Rows[0]["HandleResult"].ToString() != "0" && (Type == "1" || Type == "3")) || (dt.Rows[0]["HandleResult"].ToString() != "-1" && (Type == "2" || Type == "4")))
        {
            return "{\"result\":\"该用户已经被处理\",\"url\":\"\"}";
        }
        //验证
        List<Commission> items = JsonConvert.DeserializeObject<List<Commission>>(json);
        /*
         *  得到上级，如果没有上级那就是-1 否则就是对应的id
         *  如果有上级，那么佣金比例不能大于上级的佣金比例
         */
        long ParentID = Shove._Convert.StrToLong(dt.Rows[0]["ParentID"].ToString(), -1);
        foreach (Commission item in items)
        {
            string lotteryID = item.LotteryID;
            string LotteryName = item.LotteryName;
            double Commission = item.CommissionRate;
            if (-1 == Commission)
            {
                return "{\"result\":\"请输入[" + LotteryName + "]的佣金比例\",\"url\":\"\"}";
            }
            if (Commission >= 1)
            {
                return "{\"result\":\"佣金比例不能于等于1\",\"url\":\"\"}";
            }
            if (-1 != ParentID)
            {
                double parentBonusScale = DAL.Functions.F_CpsGetBonusScale(ParentID, Convert.ToInt64(lotteryID));
                if (Commission > parentBonusScale && parentBonusScale != 0)
                {
                    return "{\"result\":\"[" + LotteryName + "]的佣金比例不能大于上级的佣金比例,上级佣金比例是[" + parentBonusScale + "]\",\"url\":\"\"}";
                }
            }
        }
        string url = "";
        int userType = -1;
        switch (Type)
        {
            case "1":
                url = "AgentAuditing.aspx";
                userType = 1;
                break;
            case "2":
                url = "AgentAuditingNotOK.aspx";
                userType = 1;
                break;
            case "3":
                url = "PromoteAuditing.aspx";
                userType = 2;
                break;
            case "4":
                url = "PromoteAuditingNotOK.aspx";
                userType = 2;
                break;
        }
        DAL.Tables.T_Cps cps = new DAL.Tables.T_Cps();

        cps.HandleResult.Value = 1;
        cps.HandlelDateTime.Value = DateTime.Now;
        cps.SerialNumber.Value = CPSBLL.CreateSerialNumber();

        if (cps.Update("ID =" + CpsID) < 0)
        {
            return "{\"result\":\"修改失败\",\"url\":\"" + url + "\"}";
        }
        long userID = Shove._Convert.StrToLong(dt.Rows[0]["OwnerUserID"].ToString(), -1);
        Users user = new Users(1)[1, userID];
        //写审核记录
        DAL.Tables.T_AuditingRecords record = new DAL.Tables.T_AuditingRecords();
        record.CpsID.Value = CpsID;
        record.OwnerUserID.Value = user.ID;
        record.OwnerUserName.Value = user.Name;
        record.OwnerUserMobile.Value = user.Mobile;
        record.ApplyDateTime.Value = dt.Rows[0]["HandlelDateTime"];
        record.HandlerDateTime.Value = DateTime.Now;
        record.HandlerResult.Value = 1;
        record.OperateID.Value = userId;
        long recordID = record.Insert();
        
        //写入会员转移表
        DAL.Tables.T_CpsUserChange tcpsUserChange = new DAL.Tables.T_CpsUserChange();
        if (tcpsUserChange.GetCount("ChangeType < 1 and Type = " + userType + " and UserID=" + CpsID + " ") < 1)//代理商
        {
            DataTable table = cps.Open("ParentID", "ID=" + CpsID, "");
            tcpsUserChange.UserID.Value = userID;
            tcpsUserChange.DateTime.Value = DateTime.Now;
            tcpsUserChange.Type.Value = userType;
            tcpsUserChange.OperatorID.Value = userId;
            tcpsUserChange.ChangeType.Value = -1;
            if (null != table && table.Rows.Count > 0)
            {
                tcpsUserChange.NowUserID.Value = CPSBLL.getUserIDByCpsID(table.Rows[0]["ParentID"] + "");
            }
            long v = tcpsUserChange.Insert();
            if (v < 0)
            {
                return "{\"result\":\"修改失败，数据库繁忙\",\"url\":\"" + requestUrl + "\"}";
            }
        }
        DataTable dt2 = Shove.Database.MSSQL.Select("select top 1 memo from T_CpsUsersBonusScaleLog order by id desc");

        long Scaleid = 1;
        if (dt2 != null && dt2.Rows.Count > 0)
        {
            Scaleid = Shove._Convert.StrToLong(dt2.Rows[0][0].ToString(), 1) + 1;
        }
        //设置佣金比例
        foreach (Commission item in items)
        {
            long lotteryID = Convert.ToInt64(item.LotteryID);
            double Commission = item.CommissionRate;
            string ReturnDesc = "";
            int v = CPSBLL.SetCommission(Convert.ToInt64(CpsID), lotteryID, Commission, Scaleid, ref ReturnDesc);
            if (v < 0)
            {
                return "{\"result\":\"修改失败，数据库繁忙\",\"url\":\"" + url + "\"}";
            }
        }
        string cpsRelSql = "update dbo.T_CPSRel set IsPassed=1 where UserID=" + user.ID.ToString();
        Shove.Database.MSSQL.ExecuteNonQuery(cpsRelSql);
        return "{\"result\":\"修改成功\",\"url\":\"" + url + "\"}";
    }
    /// <summary>
    /// 删除审核记录
    /// </summary>
    /// <returns></returns>
    private string DeleteAuditLog()
    {
        long id = Shove._Convert.StrToLong(hc.Request.Form["id"], -1);
        string url = hc.Request.Form["requestUrl"];
        if (-1 == id)
        {
            return "{\"result\":\"删除失败\",\"url\":\"" + url + "\"}";
        }
        DAL.Tables.T_AuditingRecords record = new DAL.Tables.T_AuditingRecords();
        long result = record.Delete("ID = " + id);
        if (result < -1)
        {
            return "{\"result\":\"删除失败\",\"url\":\"" + url + "\"}";
        }
        return "{\"result\":\"删除成功\",\"url\":\"" + url + "\"}";
    }
    /// <summary>
    /// 设置佣金比例
    /// </summary>
    /// <returns></returns>
    private string SetCommissionRate()
    {
        string CpsID = hc.Request.Form["CpsID"];
        string url = hc.Request.Form["url"];
        string json = hc.Request.Form["json"];

        DataTable dt = new DAL.Tables.T_Cps().Open("ParentID", "ID = " + CpsID + " and ID <> -1", "");
        if (null == dt && dt.Rows.Count == 0)
        {
            return "{\"result\":\"修改失败，数据库繁忙\",\"url\":\"\"}";
        }
        List<Commission> items = JsonConvert.DeserializeObject<List<Commission>>(json);
        foreach (Commission item in items)
        {
            string lotteryID = item.LotteryID;
            string LotteryName = item.LotteryName;
            double commission = item.CommissionRate;
            if (commission <= 0)
            {
                return "{\"result\":\"请输入正确的[" + LotteryName + "]佣金比例\",\"url\":\"\"}";
            }
            //上级ID，如果上级ID 不等于-1表示有上级
            long ParentID = Shove._Convert.StrToLong(dt.Rows[0]["ParentID"].ToString(), -1);
            if (-1 != ParentID)
            {
                double parentBonusScale = DAL.Functions.F_CpsGetBonusScale(ParentID, Convert.ToInt64(lotteryID));
                if (commission > parentBonusScale && parentBonusScale != 0)
                {
                    return "{\"result\":\"修改失败，当前用户的佣金比例不能大于上级代理商的佣金比例\",\"url\":\"\"}";
                }
            }
        }
        DataTable dt2 = Shove.Database.MSSQL.Select("select top 1 memo from T_CpsUsersBonusScaleLog order by id desc");

        long Scaleid = 1;
        if (dt2 != null && dt2.Rows.Count > 0)
        {
            Scaleid = Shove._Convert.StrToLong(dt2.Rows[0][0].ToString(), 1) + 1;
        }
        foreach (Commission item in items)
        {
            string lotteryID = item.LotteryID;
            string LotteryName = item.LotteryName;
            double commission = item.CommissionRate;

            string returnDesc = "";
            int v = CPSBLL.SetCommission(Convert.ToInt64(CpsID), Convert.ToInt64(lotteryID), commission, Scaleid, ref returnDesc);
            if (v < 0)
            {
                return "{\"result\":\"修改失败\",\"url\":\"\"}";
            }
        }
        return "{\"result\":\"修改成功\",\"url\":\"" + url + "\"}";
    }
    /// <summary>
    /// 发放佣金
    /// </summary>
    /// <returns></returns>
    private string PayOff()
    {
        int year = Shove._Convert.StrToInt(hc.Request.Form["year"], -1);
        int month = Shove._Convert.StrToInt(hc.Request.Form["month"], -1);
        int userId = Shove._Convert.StrToInt(hc.Request.Form["userId"], -1);
        string userName = hc.Request.Form["userName"];
        string url = hc.Request.Form["url"];
        if (year == -1 || month == -1)
        {
            return "{\"result\":\"佣金发放失败\",\"url\":\"" + url + "\"}";
        }
        string userListXML = "<Users><User UserID=\"" + userId + "\" UserName=\"" + userName + "\"/></Users>";
        int returnValue = -1;
        string returnDescription = "";
        int result = DAL.Procedures.P_CpsCalculateAllowBonus(year, month, userListXML, ref returnValue, ref returnDescription);
        if (result < 0)
        {
            return "{\"result\":\"佣金发放失败\",\"url\":\"\"}";
        }
        if (-1 == returnValue)
        {
            return "{\"result\":\"" + returnDescription + "\",\"url\":\"\"}";
        }
        string str = "佣金发放成功";
        if (returnValue < 0)
        {
            str += "\\n有以下用户佣金发放失败：\\n";
            str += returnDescription;
        }
        return "{\"result\":\"" + str + "\",\"url\":\"" + url + "\"}";
    }
    /// <summary>
    /// 发放佣金给所有人
    /// </summary>
    /// <returns></returns>
    private string PayOffAll()
    {
        int year = Shove._Convert.StrToInt(hc.Request.Form["year"], -1);
        int month = Shove._Convert.StrToInt(hc.Request.Form["month"], -1);
        string url = hc.Request.Form["url"];
        if (-1 == year || -1 == month)
        {
            return "{\"result\":\"佣金发放失败\",\"url\":\"" + url + "\"}";
        }
        DataTable table = new DAL.Tables.T_CpsAdminAccountByMonth().Open("(select Name from T_Users where ID = OwnerUserID)as UserName,OwnerUserID as UserID", "[Year] = " + year + " and [Month] = " + month + " and IsPayOff = 0", "");
        if (null == table || table.Rows.Count < 1)
        {
            return "{\"result\":\"获取佣金信息失败\",\"url\":\"" + url + "\"}";
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<Users>");
        foreach (DataRow dr in table.Rows)
        {
            sb.AppendFormat("<User UserID=\"{0}\" UserName=\"{1}\"/>", dr["UserID"], dr["UserName"]);
        }
        sb.AppendLine("</Users>");
        int returnValue = -1;
        string returnDescription = "";
        int result = DAL.Procedures.P_CpsCalculateAllowBonus(year, month, sb.ToString(), ref returnValue, ref returnDescription);
        if (result < 0)
        {
            return "{\"result\":\"佣金发放失败\",\"url\":\"\"}";
        }
        if (-1 == returnValue)
        {
            return "{\"result\":\"" + returnDescription + "\",\"url\":\"\"}";
        }
        string str = "佣金发放成功";
        if (returnValue < 0)
        {
            str += "\\n有以下用户佣金发放失败：\\n";
            str += returnDescription;
        }
        return "{\"result\":\"" + str + "\",\"url\":\"" + url + "\"}";
    }
    private string IgnoreApplication()
    {
        long cpsID = Shove._Convert.StrToLong(hc.Request.Form["cpsId"], -1);
        string url = hc.Request.Form["url"];
        if (-1 == cpsID)
        {
            return "{\"result\":\"修改失败\",\"url\":\"" + url + "\"}";
        }
        DataTable dt = new DAL.Tables.T_Cps().Open("ID,OwnerUserID,HandleResult,[ON],HandlelDateTime", "ID=" + cpsID, "");
        if (null == dt || dt.Rows.Count == 0)
        {
            return "{\"result\":\"数据库繁忙，获取用户信息失败\",\"url\":\"\"}";
        }
        DataRow dr = dt.Rows[0];
        if (Convert.ToInt32(dr["HandleResult"] + "") != 0)
        {
            return "{\"result\":\"该商家已经被处理\",\"url\":\"\"}";
        }

        Users user = new Users(1)[1, Shove._Convert.StrToLong(dt.Rows[0]["OwnerUserID"].ToString(), -1)];
        Users _User = new Users(1);
        _User = Users.GetCurrentUser(1);
        //写审核记录
        DAL.Tables.T_AuditingRecords record = new DAL.Tables.T_AuditingRecords();
        record.CpsID.Value = cpsID;
        record.OwnerUserID.Value = user.ID;
        record.OwnerUserName.Value = user.Name;
        record.OwnerUserMobile.Value = user.Mobile;
        record.ApplyDateTime.Value = dt.Rows[0]["HandlelDateTime"];
        record.HandlerDateTime.Value = DateTime.Now;
        record.HandlerResult.Value = -1;
        record.OperateID.Value = _User.ID;
        long recordID = record.Insert();

        //更新T_Cps表
        DAL.Tables.T_Cps cps = new DAL.Tables.T_Cps();
        cps.HandleResult.Value = -1;
        cps.HandlelDateTime.Value = DateTime.Now;
        long result = cps.Update("ID =" + cpsID);
        if (result < 0)
        {
            return "{\"result\":\"修改失败\",\"url\":\"" + url + "\"}";
        }
        return "{\"result\":\"修改成功\",\"url\":\"" + url + "\"}";
    }
    /// <summary>
    /// 设置佣金比例(单个)
    /// </summary>
    /// <returns></returns>
    private string SetCommissionRateSingle()
    {
        CPSBLL.Admin admin = new CPSBLL.Admin();
        long lotteryID = Convert.ToInt64(hc.Request.Form["lotteryID"]);
        double agent = Shove._Convert.StrToDouble(hc.Request.Form["agent"], 0.05);
        double promote = Shove._Convert.StrToDouble(hc.Request.Form["promote"], 0.02);
        string url = hc.Request.Form["url"];
        if (promote > agent)
        {
            return "{\"result\":\"推广员佣金比例不能大于代理商的佣金比例\",\"url\":\"" + url + "\"}";
        }
        string returnDesc = "";
        if (!admin.SetCpsSiteBonusScale(lotteryID, agent, promote, ref returnDesc))
        {
            return "{\"result\":\"数据库繁忙设置失败\",\"url\":\"" + url + "\"}";
        }
        return "{\"result\":\"设置成功\",\"url\":\"" + url + "\"}";
    }
    /// <summary>
    /// 设置佣金比例(所有)
    /// </summary>
    /// <returns></returns>
    private string SetCommissionRateAll()
    {
        string url = hc.Request.Form["url"];
        double agent = Convert.ToDouble(hc.Request.Form["agent"]);
        double promote = Convert.ToDouble(hc.Request.Form["promote"]);
        Sites _Site = new Sites(1)[1];
        string[] useLotteryArray = _Site.UseLotteryListQuickBuy.Split(',');
        CPSBLL.Admin admin = new CPSBLL.Admin();
        foreach (string item in useLotteryArray)
        {
            long lotteryID = Convert.ToInt64(item);
            string returnDesc = "";
            if (!admin.SetCpsSiteBonusScale(lotteryID, agent, promote, ref returnDesc))
            {
                return "{\"result\":\"数据库繁忙设置失败\",\"url\":\"" + url + "\"}";
            }
        }
        return "{\"result\":\"设置成功\",\"url\":\"" + url + "\"}";
    }
    /// <summary>
    /// 获取用户隐藏链接
    /// </summary>
    /// <returns></returns>
    private string GetHideUserList()
    {
        string userListTxt = hc.Request.Form["userListTxt"];
        string userList = Shove._Security.Encrypt.Encrypt3DES(PF.GetCallCert(), userListTxt, PF.DesKey);
        return "{\"result\":\"" + userList + "\",\"url\":\"\"}";
    }
    /// <summary>
    /// 保存单个用户的佣金比例
    /// </summary>
    /// <returns></returns>
    private string SaveCommissionRateSingleUser()
    {
        string lotteryID = hc.Request.Form["lotteryID"];
        string LotteryName = hc.Request.Form["LotteryName"];
        double commission = Shove._Convert.StrToDouble(hc.Request.Form["commission"], -1);
        string CpsID = hc.Request.Form["CpsID"];
        string url = hc.Request.Form["url"];

        DataTable dt = new DAL.Tables.T_Cps().Open("ParentID", "ID = " + CpsID + " and ID <> -1", "");
        if (null == dt && dt.Rows.Count == 0)
        {
            return "{\"result\":\"修改失败，数据库繁忙\",\"url\":\"\"}";
        }
        if (commission <= 0)
        {
            return "{\"result\":\"请输入正确的[" + LotteryName + "]佣金比例\",\"url\":\"\"}";
        }
        //上级ID，如果上级ID 不等于-1表示有上级
        long ParentID = Shove._Convert.StrToLong(dt.Rows[0]["ParentID"] + "", -1);
        if (-1 != ParentID)
        {
            double parentBonusScale = DAL.Functions.F_CpsGetBonusScale(ParentID, Convert.ToInt64(lotteryID));
            if (commission > parentBonusScale && parentBonusScale != 0)
            {
                return "{\"result\":\"修改失败,当前用户的佣金比例不能大于上级代理商的佣金比例\",\"url\":\"\"}";
            }
        }
        DataTable dt2 = Shove.Database.MSSQL.Select("select top 1 memo from T_CpsUsersBonusScaleLog order by id desc");

        long Scaleid = 1;
        if (dt2 != null && dt2.Rows.Count > 0)
        {
            Scaleid = Shove._Convert.StrToLong(dt2.Rows[0][0].ToString(), 1) + 1;
        }
        string returnDesc = "";
        int v = CPSBLL.SetCommission(Convert.ToInt64(CpsID), Convert.ToInt64(lotteryID), commission, Scaleid, ref returnDesc);
        if (v < 0)
        {
            return "{\"result\":\"修改失败\",\"url\":\"\"}";
        }
        return "{\"result\":\"修改成功\",\"url\":\"" + url + "\"}";
    }
    /// <summary>
    /// 保存所有用户的佣金比例
    /// </summary>
    /// <returns></returns>
    private string SaveCommissionRateAllUser()
    {
        string CpsID = hc.Request.Form["CpsID"];
        string url = hc.Request.Form["url"];
        string jsonStr = hc.Request.Form["jsonStr"];
        //isReset=0 否 ； isReset=1 是
        string isReset = hc.Request.Form["isReset"];
        string userType = hc.Request.Form["userType"];

        DataTable dt = new DAL.Tables.T_Cps().Open("ParentID", "ID = " + CpsID + " and ID <> -1", "");
        if (null == dt && dt.Rows.Count == 0)
        {
            return "{\"result\":\"修改失败，数据库繁忙\",\"url\":\"\"}";
        }
        List<Commission> items = JsonConvert.DeserializeObject<List<Commission>>(jsonStr);
        if (isReset == "1")
        {
            if (userType == "1")
            {
                //代理商  0.05
                for (int i = 0; i < items.Count; i++)
                {
                    items[i].CommissionRate = 0.05d;
                }
            }
            else
            {
                //推广员  0.02
                for (int i = 0; i < items.Count; i++)
                {
                    items[i].CommissionRate = 0.02d;
                }
            }
        }
        foreach (var item in items)
        {
            string lotteryID = item.LotteryID;
            string LotteryName = item.LotteryName;
            double commission = item.CommissionRate;
            if (commission <= 0)
            {
                return "{\"result\":\"请输入正确的[" + LotteryName + "]佣金比例\",\"url\":\"\"}";
            }
            //上级ID，如果上级ID 不等于-1表示有上级
            long ParentID = Shove._Convert.StrToLong(dt.Rows[0]["ParentID"] + "", -1);
            if (-1 != ParentID)
            {
                double parentBonusScale = DAL.Functions.F_CpsGetBonusScale(ParentID, Convert.ToInt64(lotteryID));
                if (commission > parentBonusScale && parentBonusScale != 0)
                {
                    return "{\"result\":\"修改失败,当前用户的佣金比例不能大于上级代理商的佣金比例\",\"url\":\"\"}";
                }
            }
        }
        DataTable dt2 = Shove.Database.MSSQL.Select("select top 1 memo from T_CpsUsersBonusScaleLog order by id desc");

        long Scaleid = 1;
        if (dt2 != null && dt2.Rows.Count > 0)
        {
            Scaleid = Shove._Convert.StrToLong(dt2.Rows[0][0].ToString(), 1) + 1;
        }
        foreach (var item in items)
        {
            string lotteryID = item.LotteryID;
            string LotteryName = item.LotteryName;
            double commission = item.CommissionRate;

            string returnDesc = "";
            int v = CPSBLL.SetCommission(Convert.ToInt64(CpsID), Convert.ToInt64(lotteryID), commission, Scaleid, ref returnDesc);
            if (v < 0)
            {
                return "{\"result\":\"修改失败\",\"url\":\"\"}";
            }
        }
        return "{\"result\":\"修改成功！\",\"url\":\"" + url + "\"}";
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}