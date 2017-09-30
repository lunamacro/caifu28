using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlyFish.Common;
using System.Data;
using System.Text;

/// <summary>
/// CPS 业务逻辑类
/// </summary>
public class CPSBLL
{

    #region CPS登陆
    /// <summary>
    /// CPS登陆
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="userPwd">用户密码</param>
    /// <param name="ReturnDescription">返回描述，出现登陆失败的时候会用到</param>
    /// <returns></returns>
    public CPSLoginEnum CPSLogin(string userName, string userPwd, ref string ReturnDescription)
    {
        try
        {
            //先验证用户名
            int result = CommonCode.CheckIsUserName(userName);
            if (0 != result)
            {
                switch (result)
                {
                    case 1:
                        return CPSLoginEnum.UserNameNull;
                    case 2:
                        return CPSLoginEnum.UserNameNoOK;
                }
            }
            //验证用户密码
            //result = CommonCode.CheckIsUserPwd(userPwd);
            //if (0 != result)
            //{
            //    switch (result)
            //    {
            //        case 1:
            //            return CPSLoginEnum.UserPwdNull;
            //        case 2:
            //            return CPSLoginEnum.UserPwdNoOK;
            //    }
            //}
            Users _user = new Users(1);
            _user.Name = userName;
            _user.Password = userPwd;
            result = _user.Login(ref ReturnDescription);
            if (result < 0) //登陆失败
            {
                return CPSLoginEnum.LoginFail;
            }

            //检测是否为代理商
            Users _userCPS = new Users(1)[1,_user.ID];
            if(_userCPS.isAgent==2){
                return CPSLoginEnum.AgentLogin;
            }
            else if (_userCPS.isAgent == 1)
            {
                return CPSLoginEnum.PromoteLogin;
            }
            else
            {
                return CPSLoginEnum.NotCPS;
            }

            /*
             *  登录完成后进行以下逻辑操作
             *  1、验证是否是管理员和CPS管理员登录
             *  2、是否已经是CPS用户（如果不是就跳转到NotCps.aspx页面）
             *  3、账户是否被禁用
             *  4、账户审核情况（0,-1 未审核,1 审核完成,-2 申请被拒绝）
             *  5、是否绑定手机号码（如果没有绑定就跳转到MobileValided.aspx页面）
             *  6、登录账户类型（1 代理商 2 推广员）
             *
            if (_user.Competences.IsOwnedCompetences("[Administrator][CpsManage]"))
            {
                return CPSLoginEnum.AdministratorLogin;
            }
            DataTable dt = new DAL.Tables.T_Cps().Open("ID,OwnerUserID,[ON],HandleResult,Type,HandlelDateTime", "OwnerUserID=" + _user.ID, "");
            if (null == dt)
            {
                ReturnDescription = "数据库繁忙";
                return CPSLoginEnum.LoginFail;
            }
            if (null != dt && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                if (!Shove._Convert.StrToBool(dr["ON"].ToString(), false))
                {
                    return CPSLoginEnum.UserDisable;
                }
                DAL.Tables.T_Users t_users = new DAL.Tables.T_Users();
                string HandleResult = dt.Rows[0]["HandleResult"].ToString();
                switch (HandleResult)
                {
                    case "0":
                    case "-1":
                        if (t_users.GetCount("isMobileValided = 1 and id = " + _user.ID) < 1)
                        {
                            return CPSLoginEnum.isMobileValided;
                        }
                        return CPSLoginEnum.Audit;
                    case "1":
                        if (t_users.GetCount("isMobileValided = 1 and id = " + _user.ID) < 1)
                        {
                            return CPSLoginEnum.isMobileValided;
                        }
                        switch (dr["Type"].ToString())
                        {
                            case "2":
                                return CPSLoginEnum.PromoteLogin;
                            case "1":
                                return CPSLoginEnum.AgentLogin;
                        }
                        ReturnDescription = "无法验证登录者身份信息";
                        return CPSLoginEnum.LoginFail;
                    default:
                        return CPSLoginEnum.Refuse;
                }
            }
            else
            {
                return CPSLoginEnum.NotCPS;
            }

            */
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    #region 登陆枚举
    /// <summary>
    /// 登陆枚举
    /// </summary>
    public enum CPSLoginEnum
    {
        /// <summary>
        /// 用户名为空
        /// </summary>
        UserNameNull = -1,
        /// <summary>
        /// 用户名不合法
        /// </summary>
        UserNameNoOK = -2,
        /// <summary>
        /// 用户密码为空
        /// </summary>
        UserPwdNull = -3,
        /// <summary>
        /// 用户密码不合法
        /// </summary>
        UserPwdNoOK = -4,
        /// <summary>
        /// 登陆失败
        /// </summary>
        LoginFail = -5,
        /// <summary>
        /// 超级管理员登陆
        /// </summary>
        AdministratorLogin = -6,
        /// <summary>
        /// 不是CPS用户
        /// </summary>
        NotCPS = -7,
        /// <summary>
        /// 用户被禁用
        /// </summary>
        UserDisable = -8,
        /// <summary>
        /// 审核中
        /// </summary>
        Audit = -9,
        /// <summary>
        /// 拒绝
        /// </summary>
        Refuse = -10,
        /// <summary>
        /// 推广员登陆
        /// </summary>
        PromoteLogin = -11,
        /// <summary>
        /// 代理商登陆
        /// </summary>
        AgentLogin = -12,
        /// <summary>
        /// 手机号码没验证
        /// </summary>
        isMobileValided = -13
    }
    #endregion

    #endregion

    #region CPS注册
    /// <summary>
    /// CPS注册
    /// </summary>
    /// <param name="type">注册类型 dls表示代理商 tgy表示推广员</param>
    /// <param name="loginName">登陆名称</param>
    /// <param name="loginPwd">登陆密码</param>
    /// <param name="mobile">手机号码</param>
    /// <param name="parentID">父级ID</param>
    /// <param name="returnDesc">返回描述</param>
    /// <returns>是否注册成功</returns>
    public CPSRegisterEnum CPSRegister(string type, string loginName, string loginPwd, string mobile, long parentID, ref string returnDesc)
    {
        try
        {
            switch (type)
            {
                case "tgy":
                    return CPSRegisterTGY(loginName, loginPwd, mobile, parentID, ref returnDesc);
                case "dls":
                    return CPSRegisterDLS(loginName, loginPwd, mobile, ref returnDesc);
                default:
                    returnDesc = "注册类型未知";
                    return CPSRegisterEnum.RegisterFail;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    #region 注册代理商
    /// <summary>
    /// 注册代理商
    /// </summary>
    /// <param name="loginName">登陆名称</param>
    /// <param name="loginPwd">登陆密码</param>
    /// <param name="mobile">手机号码</param>
    /// <param name="returnDesc">错误描述</param>
    /// <returns>注册结果</returns>
    private CPSRegisterEnum CPSRegisterDLS(string loginName, string loginPwd, string mobile, ref string returnDesc)
    {
        //先验证用户名
        int checkResult = CommonCode.CheckIsUserName(loginName);
        if (0 != checkResult)
        {
            switch (checkResult)
            {
                case 1:
                    return CPSRegisterEnum.UserNameNull;
                case 2:
                    return CPSRegisterEnum.UserNameNoOK;
            }
        }
        //验证用户密码
        checkResult = CommonCode.CheckIsUserPwd(loginPwd);
        if (0 != checkResult)
        {
            switch (checkResult)
            {
                case 1:
                    return CPSRegisterEnum.UserPwdNull;
                case 2:
                    return CPSRegisterEnum.UserPwdNoOK;
            }
        }
        //验证手机号码
        checkResult = CommonCode.CheckIsMobile(mobile);
        if (0 != checkResult)
        {
            switch (checkResult)
            {
                case 1:
                    return CPSRegisterEnum.MobileNull;
                case 2:
                    return CPSRegisterEnum.MobileNoOK;
            }
        }
        Users user = new Users(1);
        user.Name = loginName;
        user.Password = loginPwd;
        user.PasswordAdv = loginPwd;
        user.UserType = 2;
        user.Mobile = mobile;
        user.isMobileValided = true;
        long userID = user.Add(ref returnDesc);
        if (userID < 0)
        {
            return CPSRegisterEnum.RegisterFail;
        }
        DAL.Tables.T_Cps cps = new DAL.Tables.T_Cps();
        cps.SiteID.Value = 1;
        cps.OwnerUserID.Value = userID;
        cps.DateTime.Value = DateTime.Now.ToString();
        cps.ON.Value = true;
        cps.Type.Value = 1;
        cps.ParentID.Value = -1;
        cps.OperatorID.Value = 1;   //操作员

        DAL.Tables.T_CPSRel cpsRel = new DAL.Tables.T_CPSRel();
        cpsRel.UserID.Value = userID;
        cpsRel.ParentID.Value = -1;
        cpsRel.MemberType.Value = 1;

        //检查成为代理商是否需要审核
        string sql = "select AgentAuditings from T_Sites";
        DataTable dt = Shove.Database.MSSQL.Select(sql);
        if (null != dt && dt.Rows.Count > 0 && !(Shove._Convert.StrToBool(dt.Rows[0]["AgentAuditings"].ToString(), true)))//不需要审核
        {
            cps.HandleResult.Value = 1;
            cps.SerialNumber.Value = CreateSerialNumber();
            cpsRel.IsPassed.Value = 1;
        }
        else//需要审核
        {
            cps.HandleResult.Value = 0;
            cpsRel.IsPassed.Value = 0;
        }

        long cpsID = cps.Insert();
        cpsRel.Insert();
        if (cpsID < 0)
        {
            returnDesc = "添加CPS代理商失败";
            return CPSRegisterEnum.RegisterFail;
        }

        if (!Shove._Convert.StrToBool(dt.Rows[0]["AgentAuditings"].ToString(), true))//不需要审核
        {
            //写入会员转移表
            DAL.Tables.T_CpsUserChange tcpsUserChange = new DAL.Tables.T_CpsUserChange();
            if (tcpsUserChange.GetCount("ChangeType < 1 and Type = 1 and UserID=" + userID + " ") < 1)//代理商
            {
                tcpsUserChange.UserID.Value = userID;
                tcpsUserChange.DateTime.Value = DateTime.Now;
                tcpsUserChange.Type.Value = 1;
                tcpsUserChange.OperatorID.Value = 1;
                tcpsUserChange.ChangeType.Value = -1;

                tcpsUserChange.Insert();
            }
            DAL.Tables.T_CpsSiteBonusScale tCpsSiteBonusScale = new DAL.Tables.T_CpsSiteBonusScale();
            dt = tCpsSiteBonusScale.Open("LotteryID,AgentBonusScale", "LotteryID in (" + new Sites()[1].UseLotteryListQuickBuy + ")", "id asc");
            if (null != dt && dt.Rows.Count > 0)
            {
                DataTable dt2 = Shove.Database.MSSQL.Select("select top 1 memo from T_CpsUsersBonusScaleLog order by id desc");

                long Scaleid = 1;
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    Scaleid = Shove._Convert.StrToLong(dt2.Rows[0][0].ToString(), 1) + 1;
                }
                foreach (DataRow dr in dt.Rows)
                {
                    int v = SetCommission(cpsID, Convert.ToInt64(dr["LotteryID"]), Convert.ToDouble(dr["AgentBonusScale"]), Scaleid, ref returnDesc);
                    if (v < 0)
                    {
                        returnDesc = "佣金比例设置失败";
                        return CPSRegisterEnum.RegisterFail;
                    }
                }
            }
        }
        return CPSRegisterEnum.RegisterOK;
    }
    #endregion

    #region 注册推广员
    /// <summary>
    /// 注册推广员
    /// </summary>
    /// <param name="loginName">登陆名称</param>
    /// <param name="loginPwd">登陆密码</param>
    /// <param name="mobile">手机号码</param>
    /// <param name="returnDesc">错误描述</param>
    /// <returns></returns>
    private CPSRegisterEnum CPSRegisterTGY(string loginName, string loginPwd, string mobile, long parentID, ref string returnDesc)
    {
        //先验证用户名
        int checkResult = CommonCode.CheckIsUserName(loginName);
        if (0 != checkResult)
        {
            switch (checkResult)
            {
                case 1:
                    return CPSRegisterEnum.UserNameNull;
                case 2:
                    return CPSRegisterEnum.UserNameNoOK;
            }
        }
        //验证用户密码
        checkResult = CommonCode.CheckIsUserPwd(loginPwd);
        if (0 != checkResult)
        {
            switch (checkResult)
            {
                case 1:
                    return CPSRegisterEnum.UserPwdNull;
                case 2:
                    return CPSRegisterEnum.UserPwdNoOK;
            }
        }
        //验证手机号码
        checkResult = CommonCode.CheckIsMobile(mobile);
        if (0 != checkResult)
        {
            switch (checkResult)
            {
                case 1:
                    return CPSRegisterEnum.MobileNull;
                case 2:
                    return CPSRegisterEnum.MobileNoOK;
            }
        }
        Users user = new Users(1);
        user.Name = loginName;
        user.Password = loginPwd;
        user.PasswordAdv = loginPwd;
        user.UserType = 2;
        user.Mobile = mobile;
        user.isMobileValided = true;
        long userID = user.Add(ref returnDesc);
        if (userID < 0)
        {
            return CPSRegisterEnum.RegisterFail;
        }

        DAL.Tables.T_Cps cps = new DAL.Tables.T_Cps();
        cps.SiteID.Value = 1;
        cps.OwnerUserID.Value = userID;
        cps.DateTime.Value = DateTime.Now.ToString();
        cps.ON.Value = true;
        cps.Type.Value = 2;
        cps.ParentID.Value = parentID;
        cps.OperatorID.Value = 1;   //操作员
        cps.HandlelDateTime.Value = DateTime.Now;

        // 写入cps关系表
        DAL.Tables.T_CPSRel cpsRel = new DAL.Tables.T_CPSRel();
        cpsRel.UserID.Value = userID;
        cpsRel.ParentID.Value = parentID;
        cpsRel.MemberType.Value = 2;

        //检查成为推广员是否需要审核
        string sql = "select PromoterAuditings from T_Sites";
        DataTable dt = Shove.Database.MSSQL.Select(sql);
        if (null != dt && dt.Rows.Count > 0 && !(Shove._Convert.StrToBool(dt.Rows[0]["PromoterAuditings"].ToString(), true)))//不需要审核
        {
            cps.HandleResult.Value = 1;
            cpsRel.IsPassed.Value = 1;
            cps.SerialNumber.Value = CreateSerialNumber();
        }
        else//需要审核
        {
            cps.HandleResult.Value = 0;
            cpsRel.IsPassed.Value = 0;
        }
        long cpsid = cps.Insert();
        cpsRel.Insert();
        if (cpsid < 0)
        {
            returnDesc = "添加CPS推广员失败";
            return CPSRegisterEnum.RegisterFail;
        }

        //不需要审核
        if (!Shove._Convert.StrToBool(dt.Rows[0]["PromoterAuditings"].ToString(), true))
        {
            //写入会员转移表
            DAL.Tables.T_CpsUserChange tcpsUserChange = new DAL.Tables.T_CpsUserChange();
            if (tcpsUserChange.GetCount("ChangeType < 1 and Type = 2 and UserID=" + userID + " ") < 1)//推广员
            {
                tcpsUserChange.UserID.Value = userID;
                tcpsUserChange.DateTime.Value = DateTime.Now;
                tcpsUserChange.Type.Value = 2;
                tcpsUserChange.OperatorID.Value = 1;
                tcpsUserChange.ChangeType.Value = -1;

                tcpsUserChange.Insert();
            }
            DAL.Tables.T_CpsSiteBonusScale tCpsSiteBonusScale = new DAL.Tables.T_CpsSiteBonusScale();
            dt = tCpsSiteBonusScale.Open("LotteryID,PromoterBonusScale", "LotteryID in (" + new Sites()[1].UseLotteryListQuickBuy + ")", "id asc");
            if (null != dt && dt.Rows.Count > 0)
            {
                DataTable dt2 = Shove.Database.MSSQL.Select("select top 1 memo from T_CpsUsersBonusScaleLog order by id desc");

                long Scaleid = 1;
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    Scaleid = Shove._Convert.StrToLong(dt2.Rows[0][0].ToString(), 1) + 1;
                }
                foreach (DataRow dr in dt.Rows)
                {
                    int v = SetCommission(cpsid, Convert.ToInt64(dr["LotteryID"]), Convert.ToDouble(dr["PromoterBonusScale"]), Scaleid, ref returnDesc);
                    if (v < 0)
                    {
                        returnDesc = "佣金比例设置失败";
                        return CPSRegisterEnum.RegisterFail;
                    }
                }
            }
        }
        return CPSRegisterEnum.RegisterOK;
    }
    #endregion

    #region 注册枚举
    /// <summary>
    /// 登陆枚举
    /// </summary>
    public enum CPSRegisterEnum
    {
        /// <summary>
        /// 用户名为空
        /// </summary>
        UserNameNull = -1,
        /// <summary>
        /// 用户名不合法
        /// </summary>
        UserNameNoOK = -2,

        /// <summary>
        /// 用户密码为空
        /// </summary>
        UserPwdNull = -3,
        /// <summary>
        /// 用户密码不合法
        /// </summary>
        UserPwdNoOK = -4,

        /// <summary>
        /// 注册失败
        /// </summary>
        RegisterFail = -5,

        /// <summary>
        /// 手机为空
        /// </summary>
        MobileNull = -6,
        /// <summary>
        /// 手机不合法
        /// </summary>
        MobileNoOK = -7,

        /// <summary>
        /// 注册成功
        /// </summary>
        RegisterOK = 0
    }
    #endregion
    #endregion

    #region 生成注册码
    /// <summary>
    /// 生成注册码
    /// </summary>
    /// <returns></returns>
    public static string CreateSerialNumber()
    {
        bool boolSerialNumber = false;
        string strTempSerialNumber = "";
        DAL.Tables.T_Cps tcps = new DAL.Tables.T_Cps();
        while (!boolSerialNumber)
        {
            strTempSerialNumber = FlyFish.Common.CommonCode.getSerialNumber(5, true);
            if (tcps.GetCount("SerialNumber='" + strTempSerialNumber + "'") < 1)
            {
                boolSerialNumber = true;
            }
        }
        return strTempSerialNumber;
    }
    #endregion

    #region 设置佣金比例
    /// <summary>
    /// 设置佣金比例
    /// </summary>
    /// <param name="userID">用户id</param>
    /// <param name="lotteryID">彩种id</param>
    /// <param name="bonusScale">佣金比例</param>
    /// <param name="returnDesc">返回描述，异常时赋值</param>
    /// <returns></returns>
    public static int SetCommission(long userID, long lotteryID, double bonusScale, long parentid, ref string returnDesc)
    {
        int ReturnValue = -1;
        return DAL.Procedures.P_CPSSetCommission(userID, lotteryID, bonusScale, parentid, ref ReturnValue, ref returnDesc);
    }
    #endregion

    #region 计算总页数
    /// <summary>
    /// 计算总页数
    /// </summary>
    /// <param name="pageSize">一页显示多少条</param>
    /// <param name="dataCount">总数据量</param>
    /// <returns>返回总页数</returns>
    public static long CalculateSumPageCount(int pageSize, long dataCount)
    {
        return ((dataCount - 1) / pageSize) + 1;
    }
    #endregion

    #region CPS获得佣金比例
    /// <summary>
    /// 获得佣金比例
    /// </summary>
    /// <param name="agentOrPromoteID">代理商或推广员id</param>
    /// <param name="LotteryID">彩种id</param>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    /// <returns>返回这个代理商或推广杨这个彩种这年这月的佣金</returns>
    public static double GetCommissionScale(string agentOrPromoteID, string LotteryID, string year, string month)
    {
        double TempCommission = 0;
        Shove.Database.MSSQL.OutputParameter output = new Shove.Database.MSSQL.OutputParameter();
        Shove.Database.MSSQL.Parameter[] paramArray = new Shove.Database.MSSQL.Parameter[] { 
                    new Shove.Database.MSSQL.Parameter("@agentOrPromoteID",SqlDbType.VarChar,100,ParameterDirection.Input,agentOrPromoteID),
                    new Shove.Database.MSSQL.Parameter("@lotteryID",SqlDbType.VarChar,100,ParameterDirection.Input,LotteryID),
                    new Shove.Database.MSSQL.Parameter("@year",SqlDbType.VarChar,100,ParameterDirection.Input,year),
                    new Shove.Database.MSSQL.Parameter("@month",SqlDbType.VarChar,300,ParameterDirection.Input,month),
                    new Shove.Database.MSSQL.Parameter("@Commission",SqlDbType.Float,300,ParameterDirection.Output,TempCommission),
                };
        int result = Shove.Database.MSSQL.ExecuteStoredProcedureNonQuery("P_GetAgentOrPromoteCommission", ref output, paramArray);
        if (result < 0)
        {
            TempCommission = -1;
        }
        else
        {
            TempCommission = Shove._Convert.StrToDouble(output[0].ToString(), 0);
        }
        return TempCommission;
    }
    #endregion

    #region cps新闻
    /// <summary>
    /// cps新闻
    /// </summary>
    public class CPSNews
    {
        #region 获得CPS新闻
        /// <summary>
        /// 获得CPS新闻列表
        /// </summary>
        /// <param name="type">类型：新闻公告(xwgg)，推广指南(tgzn)</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">一页显示多少条</param>
        /// <returns>返回新闻列表，有可能为null，或者DataTable.Rows.Count = 0</returns>
        public DataTable GetCPSNews(string type, int pageIndex, int pageSize)
        {
            string selectColumns = "ID,[DateTime],Title,Content,ROW_NUMBER() OVER(order by [DateTime])AS RowNumber";
            try
            {
                string TypeID = "";
                switch (type)
                {
                    case "xwgg":
                        TypeID = "103003";
                        break;
                    case "tgzn":
                        TypeID = "103002";
                        break;
                }
                return Shove.Database.MSSQL.Select("select * from (select " + selectColumns + " from t_news where TypeID = " + TypeID + ")as NewTable where RowNumber > (" + pageIndex + " - 1) * " + pageSize + " and RowNumber <=  " + pageIndex + " * " + pageSize + ""); ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 获得cps新闻总数量
        /// <summary>
        /// 获得cps新闻总数量
        /// </summary>
        /// <param name="type">类型：新闻公告(xwgg)，推广指南(tgzn)</param>
        /// <returns>返回总数量，有可能为空 或者 0</returns>
        public long GetSumNewsCount(string type)
        {
            try
            {
                string TypeID = "";
                switch (type)
                {
                    case "xwgg":
                        TypeID = "103003";
                        break;
                    case "tgzn":
                        TypeID = "103002";
                        break;
                }
                return new DAL.Tables.T_News().GetCount("TypeID = " + TypeID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
    #endregion

    #region 通过CpsID 返回此推广的用户id
    /// <summary>
    /// 通过CpsID 返回此推广的用户id
    /// </summary>
    /// <param name="cpsid">cps表中的id</param>
    /// <returns></returns>
    public static long getUserIDByCpsID(string cpsid)
    {

        DAL.Tables.T_Cps t_cps = new DAL.Tables.T_Cps();
        DataTable dt = new DataTable();
        dt = t_cps.Open("OwnerUserID", "id=" + cpsid, "");
        if (dt != null && dt.Rows.Count > 0)
        {
            return Shove._Convert.StrToLong(dt.Rows[0][0].ToString(), -1);
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// 通过用户ID获得推广ID
    /// </summary>
    /// <param name="cpsid">cps表中的id</param>
    /// <returns></returns>
    public static long getCpsIDByUserID(long userId)
    {

        DAL.Tables.T_Cps t_cps = new DAL.Tables.T_Cps();
        DataTable dt = new DataTable();
        dt = t_cps.Open("ID", "OwnerUserID=" + userId, "");
        if (dt != null && dt.Rows.Count > 0)
        {
            return Shove._Convert.StrToLong(dt.Rows[0][0].ToString(), -1);
        }
        else
        {
            return -1;
        }
    }
    #endregion

    #region 检查手机号码是否使用
    /// <summary>
    /// 检查手机号码是否使用
    /// </summary>
    /// <param name="mobile"></param>
    /// <returns></returns>
    public static bool CheckMobileIsUse(long mobile)
    {
        DataTable dt = new DAL.Tables.T_Users().Open("Mobile", "Mobile='" + mobile + "' and isMobileValided=1", "");
        if (dt != null && dt.Rows.Count > 0)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region 验证是否绑定手机号码
    /// <summary>
    /// 验证是否绑定手机号码
    /// </summary>
    /// <param name="userID">用户id</param>
    /// <returns></returns>
    public static bool CheckIsBindMobile(long userID)
    {
        DataTable dt = new DAL.Tables.T_Users().Open("isMobileValided", "ID = '" + userID + "'", "");
        if (null != dt && dt.Rows.Count > 0 && dt.Rows[0]["isMobileValided"].ToString().ToLower() != "false")
        {
            return true;
        }
        return false;
    }
    #endregion

    #region 管理员
    /// <summary>
    /// 管理员
    /// </summary>
    public class Admin
    {
        /// <summary>
        /// 设置默认佣金比例
        /// </summary>
        /// <param name="lotteryID">彩种id</param>
        /// <param name="agentBonusScale">代理商佣金比例</param>
        /// <param name="promoterBonusScale">推广员佣金比例</param>
        /// <param name="returnDesc">返回描述，当出现错误的时候需要用到</param>
        /// <returns>是否修改成功</returns>
        public bool SetCpsSiteBonusScale(long lotteryID, double agentBonusScale, double promoterBonusScale, ref string returnDesc)
        {
            if (promoterBonusScale > agentBonusScale)
            {
                returnDesc = "推广员的佣金比例不能大于代理商的佣金比例";
                return false;
            }
            int returnValue = -1;
            int result = DAL.Procedures.P_SetCpsSiteBonusScale(lotteryID, agentBonusScale, promoterBonusScale, ref returnValue, ref returnDesc);
            if (result < 0 || returnValue < 0)
            {
                return false;
            }
            return true;
        }
    }
    #endregion

    #region 根据推广编码获取CPSID
    public static long GetCpsIDByCode(string Code)
    {
        long cpsID = -1;

        DAL.Tables.T_Cps tcps = new DAL.Tables.T_Cps();

        System.Data.DataTable dt = tcps.Open("ID", "SerialNumber='" + Code + "'", "");

        if (dt == null || dt.Rows.Count <= 0) return cpsID;

        cpsID = Shove._Convert.StrToLong(dt.Rows[0][0].ToString(), 0);

        return cpsID;
    }
    #endregion
}

