<%@ WebHandler Language="C#" Class="Oauth" %>

using System;
using System.Web;

public class Oauth : IHttpHandler {

    string domainName = "cf28.560651.com";
    long timeStamp = 0;
    
    public void ProcessRequest (HttpContext context) {
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        context.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST");
        context.Response.Clear();
        context.Response.ContentType = "application/json";

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        
        string opt = Shove._Web.Utility.GetRequest("opt");
        //验证字符串
        string oauth = Shove._Web.Utility.GetRequest("oauth");
        string info = Shove._Web.Utility.GetRequest("info");

        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        timeStamp = (long)(DateTime.Now - startTime).TotalSeconds; // 相差秒数
        string checkResult = AppGateWay_Verify.verifyForLogin(oauth, info);
        if(!checkResult.Equals("success")){
            sb.Append("{\"error\":\"-99\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"" + checkResult + "\"}");
            context.Response.Write(sb);
            return;
        }


        //用户登录
        #region 用户登录
        if (opt.Equals("1"))
        {
            UserInfo userInfo;
            try
            {
                userInfo = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(info, typeof(UserInfo));
            }
            catch
            {
                sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"info 信息异常\"}");
                new Log("Oauth_Exception").Write("info 信息异常 - Oauth.ashx - " + System.Web.HttpContext.Current.Request.UserHostAddress);
                context.Response.Write(sb);
                return;
            }

            if (string.IsNullOrEmpty(userInfo.Name))
            {
                sb.Append("{\"error\":\"-109\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"用户名不能为空\"}");
                context.Response.Write(sb);
                return;

            }

            if (string.IsNullOrEmpty(userInfo.Password))
            {
                sb.Append("{\"error\":\"-110\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"密码不能为空\"}");
                context.Response.Write(sb);
                return;

            }

            System.Data.DataTable dt = Shove.Database.MSSQL.Select("Select top 1 * from T_Users where [Name] = @p1 or Mobile=@p1",
                new Shove.Database.MSSQL.Parameter("@p1", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, userInfo.Name));


            if (dt == null)
            {
                sb.Append("{\"error\":\"-111\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"数据库繁忙，请稍后再试\"}");
                context.Response.Write(sb);
                return;

            }

            if (dt.Rows.Count <= 0)
            {
                sb.Append("{\"error\":\"-112\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"用户名不存在\"}");
                context.Response.Write(sb);
                return;

            }
            //验证用户是否被允许登录
            if (dt.Rows[0]["isCanLogin"].ToString().ToLower() == "false")
            {
                sb.Append("{\"error\":\"-116\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"该帐户已注销，无法登录，谢谢！\"}");
                context.Response.Write(sb);
                return;
            }

            if (!userInfo.Password.ToLower().Equals(dt.Rows[0]["Password"].ToString().ToLower()))
            {
                //返回剩余次数
                sb.Append("{\"error\":\"-113\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"登录密码错误\"}");
                new Log("Oauth_Exception").Write("用户密码错误 - Oauth.ashx - " + System.Web.HttpContext.Current.Request.UserHostAddress);
                context.Response.Write(sb);
                return;
            }

            #region 获取站点

            Sites _Site = new Sites()[1];

            if (_Site == null)
            {
                sb.Append("{\"error\":\"-114\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"未知错误\"}");
                new Log("Oauth_Exception").Write( "获取站点信息异常");
                context.Response.Write(sb);
                return;

            }

            #endregion
            

            string accessToken = PF.EncryptPassword(timeStamp.ToString());
            string ReturnDescription="";
            Users _users = new Users(1)[1, long.Parse(dt.Rows[0]["ID"].ToString())];
            _users.accessToken = accessToken;
            _users.EditByID(ref ReturnDescription);
            
            sb.Append("{\"error\":\"0\",");
            sb.Append("\"msg\":\"\",");
            sb.Append("\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"accessToken\":\"" + accessToken + "\",");
            sb.Append("\"uid\":\"" + dt.Rows[0]["ID"].ToString() + "\",");
            sb.Append("\"Password\":\"" + dt.Rows[0]["Password"].ToString() + "\",");
            sb.Append("\"PasswordAdv\":\"" + dt.Rows[0]["PasswordAdv"].ToString() + "\",");
            sb.Append("\"Name\":\"" + dt.Rows[0]["Name"].ToString() + "\",");
            sb.Append("\"NickName\":\"" + dt.Rows[0]["NickName"].ToString() + "\",");
            sb.Append("\"Mobile\":\"" + dt.Rows[0]["Mobile"].ToString() + "\",");
            sb.Append("\"Level\":\"" + dt.Rows[0]["Level"].ToString() + "\",");
            sb.Append("\"isMobileValided\":\"" + dt.Rows[0]["isMobileValided"].ToString() + "\",");
            sb.Append("\"HeadUrl\":\"" + dt.Rows[0]["HeadUrl"].ToString() + "\",");
            sb.Append("\"GexingQianming\":\"" + dt.Rows[0]["GexingQianming"].ToString() + "\",");
            sb.Append("\"BankAddress\":\"" + dt.Rows[0]["BankAddress"].ToString() + "\",");
            sb.Append("\"BankName\":\"" + dt.Rows[0]["BankName"].ToString() + "\",");
            sb.Append("\"BankCardNumber\":\"" + dt.Rows[0]["BankCardNumber"].ToString() + "\",");
            sb.Append("\"UserCradName\":\"" + dt.Rows[0]["UserCradName"].ToString() + "\",");
            sb.Append("\"Balance\":\"" + Shove._Convert.StrToDouble(dt.Rows[0]["Balance"].ToString(), 0).ToString("0.00") + "\",");
            sb.Append("\"HandselAmount\":\"" + Shove._Convert.StrToDouble(dt.Rows[0]["HandselAmount"].ToString(), 0).ToString("0.00") + "\",");
            sb.Append("\"isAgent\":\"" + dt.Rows[0]["isAgent"].ToString() + "\",");
            sb.Append("\"ReferId\":\"" + dt.Rows[0]["ReferId"].ToString() + "\",");
            sb.Append("\"agentGroup\":\"" + dt.Rows[0]["agentGroup"].ToString() + "\",");
            sb.Append("\"isWXBind\":\"" + dt.Rows[0]["isWXBind"].ToString() + "\",");
            sb.Append("\"RefferUrl\":\"http://"+domainName+"/AppGateWay/?pid=" + dt.Rows[0]["ID"].ToString() + "\",");
            sb.Append("\"msgCount\":\"0\"}");
            context.Response.Write(sb);
            return;
        }
        #endregion


        //会员注册
        #region 会员注册
        else if (opt.Equals("2"))
        {
            UserInfo userInfo;
            try
            {
                userInfo = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(info, typeof(UserInfo));
            }
            catch
            {
                sb.Append("{\"error\":\"-101\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"info 信息异常\"}");
                new Log("Oauth_Exception").Write("info 信息异常 - Oauth.ashx - " + System.Web.HttpContext.Current.Request.UserHostAddress);
                context.Response.Write(sb);
                return;
            }

            if (string.IsNullOrEmpty(userInfo.Name))
            {
                sb.Append("{\"error\":\"-102\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"用户名不能为空\"}");
                context.Response.Write(sb);
                return;

            }

            if (string.IsNullOrEmpty(userInfo.Password))
            {
                sb.Append("{\"error\":\"-103\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"密码不能为空\"}");
                context.Response.Write(sb);
                return;

            }
            string Name = userInfo.Name;
            string Password = userInfo.Password;
            string ReferId = userInfo.ReferId;
            int referIdNum = int.Parse(ReferId);
            if (!new CheckName_ashx().IsKeyWords(Name) || !new CheckName_ashx().IsKeyWords(Password) || !new CheckName_ashx().IsKeyWords(ReferId))
            {
                sb.Append("{\"error\":\"-104\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"您的输入包含敏感词，请重新输入\"}");
                new Log("Oauth_Exception").Write("您的输入包含敏感词，请重新输入" + System.Web.HttpContext.Current.Request.UserHostAddress);
                context.Response.Write(sb);
                return;
            }

            //如果没有推荐人，分配到总代理组
            if (referIdNum <= 0)
            {
                sb.Append("{\"error\":\"-107\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"推荐人ID不正确\"}");
                context.Response.Write(sb);
                return;
            }

            Users refferUser = new Users(1)[1, referIdNum];
            if (refferUser.ID < 0)
            {
                sb.Append("{\"error\":\"-105\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"推荐人ID不存在\"}");
                context.Response.Write(sb);
                return;
            }
            if (refferUser.isAgent == 0)
            {
                sb.Append("{\"error\":\"-106\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"此推荐人无推荐资格\"}");
                context.Response.Write(sb);
                return;
            }
            
            string accessToken = PF.EncryptPassword(timeStamp.ToString());
            Users user = new Users(1);
            user.Name = userInfo.Name;
            user._password = userInfo.Password;
            user.accessToken=accessToken;
            user.FromClient = int.Parse(userInfo.FromClient);
            user.isAgent = 0;
            user.agentGroup = refferUser.agentGroup; 
            user.ReferId = referIdNum;

            string msg = "";
            int Result = Add(user, userInfo.Password, ref msg);

            if (Result < 0)
            {
                sb.Append("{\"error\":\"-110\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"" + msg + "\"}");
                new Log("Oauth_Exception").Write(msg + System.Web.HttpContext.Current.Request.UserHostAddress);
                context.Response.Write(sb);
                return;

            }
            sb.Append(getUserInfoAfterReg(Result));
            context.Response.Write(sb);
            return;

        }
        #endregion

        //发送手机验证码
        #region 发送手机验证码
        else if (opt.Equals("3"))
        {
            SMSObj obj;
            try
            {

                obj = (SMSObj)Newtonsoft.Json.JsonConvert.DeserializeObject(info, typeof(SMSObj));
            }
            catch
            {
                sb.Append("{\"error\":\"-101\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"info 信息异常\"}");
                new Log("Oauth_Exception").Write("info 信息异常 - Oauth.ashx - " + System.Web.HttpContext.Current.Request.UserHostAddress);
                context.Response.Write(sb);
                return;
            }

            string username = obj.username;
            string mobile = obj.mobile;

            Users user = new Users(1)[1, username];
            if (user==null)
            {
                sb.Append("{\"error\":\"-102\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"用户名不存在\"}");
                context.Response.Write(sb);
                return;
            }

            if (!user.isMobileValided)
            {
                sb.Append("{\"error\":\"-103\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"您尚未绑定手机，请联系客服\"}");
                context.Response.Write(sb);
                return;
            }

            if (!user.Mobile.Equals(mobile))
            {
                sb.Append("{\"error\":\"-104\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"手机号码与账号绑定不一致\"}");
                context.Response.Write(sb);
                return;
            }
            
            string smscode = sendSMS(mobile);
            if (!smscode.Equals(""))
            {
                sb.Append("{\"error\":\"0\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"" + AppGateWay_Verify.RSA(smscode) + "\"}");
                context.Response.Write(sb);
                return;
            }
            else
            {
                sb.Append("{\"error\":\"-105\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"验证码发送失败\"}");
                context.Response.Write(sb);
                return;
            }
            

        }
        #endregion


        //找回密码
        #region 找回密码
        else if (opt.Equals("4"))
        {
            SMSObj obj;
            try
            {

                obj = (SMSObj)Newtonsoft.Json.JsonConvert.DeserializeObject(info, typeof(SMSObj));
            }
            catch
            {
                sb.Append("{\"error\":\"-101\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"info 信息异常\"}");
                new Log("Oauth_Exception").Write("info 信息异常 - Oauth.ashx - " + System.Web.HttpContext.Current.Request.UserHostAddress);
                context.Response.Write(sb);
                return;
            }

            string username = obj.username;
            string mobile = obj.mobile;
            string password = obj.password;

            Users user = new Users(1)[1, username];
            if (user == null)
            {
                sb.Append("{\"error\":\"-102\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"用户名不存在\"}");
                context.Response.Write(sb);
                return;
            }

            if (!user.isMobileValided)
            {
                sb.Append("{\"error\":\"-103\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"您尚未绑定手机，请联系客服\"}");
                context.Response.Write(sb);
                return;
            }

            if (!user.Mobile.Equals(mobile))
            {
                sb.Append("{\"error\":\"-104\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"手机号码与账号绑定不一致\"}");
                context.Response.Write(sb);
                return;
            }

            user._password = password;
            string refDes = "";
            int result = user.EditByID(ref refDes);
            if (result < 0)
            {
                sb.Append("{\"error\":\"-105\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"密码更改失败:" + refDes + "\"}");
                context.Response.Write(sb);
                return;
                
            }
            else
            {
                sb.Append("{\"error\":\"0\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"密码更改成功\"}");
                context.Response.Write(sb);
                return;
            }


        }
        #endregion


        //发送手机验证码用于绑定手机
        #region 发送手机验证码用于绑定手机
        else if (opt.Equals("5"))
        {
            SMSObj obj;
            try
            {

                obj = (SMSObj)Newtonsoft.Json.JsonConvert.DeserializeObject(info, typeof(SMSObj));
            }
            catch
            {
                sb.Append("{\"error\":\"-101\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"info 信息异常\"}");
                new Log("Oauth_Exception").Write("info 信息异常 - Oauth.ashx - " + System.Web.HttpContext.Current.Request.UserHostAddress);
                context.Response.Write(sb);
                return;
            }

            string username = obj.username;
            string mobile = obj.mobile;

            Users user = new Users(1)[1, username];
            if (user == null)
            {
                sb.Append("{\"error\":\"-102\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"用户名不存在\"}");
                context.Response.Write(sb);
                return;
            }

            if (user.isMobileValided)
            {
                sb.Append("{\"error\":\"-103\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"您已经绑定手机了\"}");
                context.Response.Write(sb);
                return;
            }


            string smscode = sendSMS(mobile);
            if (!smscode.Equals(""))
            {
                sb.Append("{\"error\":\"0\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"" + AppGateWay_Verify.RSA(smscode) + "\"}");
                context.Response.Write(sb);
                return;
            }
            else
            {
                sb.Append("{\"error\":\"-105\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"验证码发送失败\"}");
                context.Response.Write(sb);
                return;
            }


        }
        #endregion
        
        
        
        
        
        


    }



    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="ReturnDescription"></param>
    /// <returns></returns>
    private int Add(Users _User, string Password, ref string ReturnDescription)
    {
        DateTime RegisterTime = DateTime.Now;
        ReturnDescription = "";
        long _id = 0;
        int ReturnValue = DAL.Procedures.P_UserAdd(_User.SiteID, _User.Name, _User.Name, Password, "", _User.Mobile, _User.isMobileValided,
             _User.UserType, _User.accessToken, _User.FromClient, _User.ReferId, _User.wxopenid, _User.isAgent, _User.agentGroup, _User.isWXBind,_User.HeadUrl, ref _id, ref ReturnDescription);

        if (ReturnValue < 0)
        {
            ReturnDescription = ReturnDescription == "" ? "数据库读写错误" : ReturnDescription;

            return ReturnValue;
        }

        return (int)_id;
    }
    

    //注册成功后获取用户信息
    private string getUserInfoAfterReg(long uid)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        System.Data.DataTable dt = Shove.Database.MSSQL.Select("Select top 1 * from T_Users where ID = @p1",
                    new Shove.Database.MSSQL.Parameter("@p1", System.Data.SqlDbType.VarChar, 0, System.Data.ParameterDirection.Input, uid.ToString()));

        if (dt == null)
        {
            sb.Append("{\"error\":\"-111\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"数据库繁忙，请稍后再试\"}");
            new Log("Oauth_Exception").Write("数据库繁忙，请稍后再试_293" + System.Web.HttpContext.Current.Request.UserHostAddress);
            return sb.ToString();
        }

        if (dt.Rows.Count <= 0)
        {
            sb.Append("{\"error\":\"-112\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户名不存在\"}");
            new Log("Oauth_Exception").Write("用户名不存在_302" + System.Web.HttpContext.Current.Request.UserHostAddress);
            return sb.ToString();
        }

        #region 获取站点

        Sites _Site = new Sites()[1];

        if (_Site == null)
        {
            sb.Append("{\"error\":\"-114\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"未知错误\"}");
            return sb.ToString();
        }

        #endregion

        ////获取站内消息条数
        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"\",");
        sb.Append("\"timeStamp\":\"" + timeStamp + "\",");
        sb.Append("\"accessToken\":\"" + dt.Rows[0]["accessToken"].ToString() + "\",");
        sb.Append("\"uid\":\"" + dt.Rows[0]["ID"].ToString() + "\",");
        sb.Append("\"Name\":\"" + dt.Rows[0]["Name"].ToString() + "\",");
        sb.Append("\"Password\":\"" + dt.Rows[0]["Password"].ToString() + "\",");
        sb.Append("\"PasswordAdv\":\"" + dt.Rows[0]["PasswordAdv"].ToString() + "\",");
        sb.Append("\"NickName\":\"" + dt.Rows[0]["NickName"].ToString() + "\",");
        sb.Append("\"Mobile\":\"" + dt.Rows[0]["Mobile"].ToString() + "\",");
        sb.Append("\"Level\":\"" + dt.Rows[0]["Level"].ToString() + "\",");
        sb.Append("\"isMobileValided\":\"" + dt.Rows[0]["isMobileValided"].ToString() + "\",");
        sb.Append("\"HeadUrl\":\"" + dt.Rows[0]["HeadUrl"].ToString() + "\",");
        sb.Append("\"GexingQianming\":\"" + dt.Rows[0]["GexingQianming"].ToString() + "\",");
        sb.Append("\"BankAddress\":\"" + dt.Rows[0]["BankAddress"].ToString() + "\",");
        sb.Append("\"BankName\":\"" + dt.Rows[0]["BankName"].ToString() + "\",");
        sb.Append("\"BankCardNumber\":\"" + dt.Rows[0]["BankCardNumber"].ToString() + "\",");
        sb.Append("\"UserCradName\":\"" + dt.Rows[0]["UserCradName"].ToString() + "\",");
        sb.Append("\"Balance\":\"" + Shove._Convert.StrToDouble(dt.Rows[0]["Balance"].ToString(), 0).ToString("0.00") + "\",");
        sb.Append("\"HandselAmount\":\"" + Shove._Convert.StrToDouble(dt.Rows[0]["HandselAmount"].ToString(), 0).ToString("0.00") + "\",");
        sb.Append("\"isAgent\":\"" + dt.Rows[0]["isAgent"].ToString() + "\",");
        sb.Append("\"ReferId\":\"" + dt.Rows[0]["ReferId"].ToString() + "\",");
        sb.Append("\"agentGroup\":\"" + dt.Rows[0]["agentGroup"].ToString() + "\",");
        sb.Append("\"isWXBind\":\"" + dt.Rows[0]["isWXBind"].ToString() + "\",");
        sb.Append("\"RefferUrl\":\"http://" + domainName + "/AppGateWay/?pid=" + dt.Rows[0]["ID"].ToString() + "\",");
        sb.Append("\"msgCount\":\"0\"}");

        return sb.ToString();

    }
    
    
    //发送短信验证码
    private string sendSMS(string toPhone)
    {
        string serverIp = "api.ucpaas.com";
        string serverPort = "443";
        string account = "8e0be84a2fb5d1700a19fbdea9fe2ccf";    //用户sid
        string token = "0f2ed286d1b81aba3400737b66aad1fa";      //用户sid对应的token
        string appId = "b38a91e6a55d445cb093ebb9ccddcf7f";      //对应的应用id，非测试应用需上线使用
        //string toPhone = toPhone;  //发送短信手机号码，群发逗号区分
        string templatedId = "135021";                               //短信模板id，需通过审核

        Random ran = new Random();
        string smscode = ran.Next(100000, 999999).ToString();
        string param = smscode;                                     //短信参数


        UCSRestRequest.UCSRestRequest api = new UCSRestRequest.UCSRestRequest();
        api.init(serverIp, serverPort);
        api.setAccount(account, token);
        api.enabeLog(false);
        api.setAppId(appId);
        api.enabeLog(true);

        string result = api.SendSMS(toPhone, templatedId, param);
        if (result.IndexOf("000000") >= 0)
        {
            return smscode;
        }else{
            return "";
        }
    }
    
    
    
 
    public bool IsReusable {
        get {
            return false;
        }
    }




    #region 模型
    //用户数据模型
    public struct UserInfo
    {
        public string accessToken { get; set; }
        public string timeStamp { get; set; }
        public string uid { get; set; }
        /// <summary>
        /// 推荐人ID
        /// </summary>
        public string Name { get; set; }
        public string Password { get; set; }
        public string PasswordAdv { get; set; }
        public string NickName { get; set; }
        public string Mobile { get; set; }
        public string HeadUrl { get; set; }
        public string GexingQianming { get; set; }
        public string Balance { get; set; }
        public string HandselAmount { get; set; }
        public string Level { get; set; }
        public string isAgent { get; set; }
        public string ReferId { get; set; }
        public string agentGroup { get; set; }
        public string isWXBind { get; set; }
        public string FromClient { get; set; }

        public string BankAddress { get; set; }
        public string BankName { get; set; }
        public string BankCardNumber { get; set; }
        public string UserCradName { get; set; }
    }

    //短信发送接收以及密码更改
    public struct SMSObj
    {
        public string username { get; set; }
        public string mobile { get; set; }
        public string password { get; set; }
    }

    
    
    #endregion


}