<%@ WebHandler Language="C#" Class="Api" %>

using System;
using System.Web;

public class Api : IHttpHandler {

    long timeStamp = 0;
    
    public void ProcessRequest (HttpContext context) {
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        context.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST");
        context.Response.Clear();
        context.Response.ContentType = "application/json";


        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            string oauth = HttpContext.Current.Server.UrlDecode(Shove._Web.Utility.GetRequest("oauth"));
            string info = HttpContext.Current.Server.UrlDecode(Shove._Web.Utility.GetRequest("info"));
            string opt = HttpContext.Current.Server.UrlDecode(Shove._Web.Utility.GetRequest("opt"));

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            timeStamp = (long)(DateTime.Now - startTime).TotalSeconds; // 相差秒数


            string checkResult = AppGateWay_Verify.verifyForLogin(oauth, info);
            
            if (!checkResult.Equals("success"))
            {
                sb.Append("{\"error\":\"-99\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"" + checkResult + "\"}");
                context.Response.Write(sb);
                return;
            }


            ApiJson jsonApi;
            try
            {
                jsonApi = (ApiJson)Newtonsoft.Json.JsonConvert.DeserializeObject(info, typeof(ApiJson));
            }
            catch
            {
                sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"info 信息异常\"}");
                new Log("Oauth_Exception").Write("info 信息异常 - Api.ashx - " + System.Web.HttpContext.Current.Request.UserHostAddress);
                context.Response.Write(sb);
                return;
            }

            Users _User = new Users(1)[1, int.Parse(jsonApi.uid)];
            if (_User.ID < 0)
            {
                sb.Append("{\"error\":\"-109\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"用户不存在\"}");
                new Log("Oauth_Exception").Write("用户不存在 - Api.ashx - " + System.Web.HttpContext.Current.Request.UserHostAddress);
                context.Response.Write(sb);
                return;
            }

            if (!_User.accessToken.Equals(jsonApi.accessToken))
            {
                sb.Append("{\"error\":\"-110\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"AccessToken已过期，请重新登录\"}");
                new Log("Oauth_Exception").Write("AccessToken已过期 - Api.ashx - " + System.Web.HttpContext.Current.Request.UserHostAddress);
                context.Response.Write(sb);
                return;
            }
            
            

            string strReturnValue = "";
            switch (opt) //操作类型
            {
                //获取用户余额和彩金
                case "1":
                    strReturnValue = new NewPc28Api().getUserBalance(context, _User);
                    break;
                //执行购彩操作
                case "11":
                    strReturnValue = new NewPc28Api().Buy(context, info);
                    break;

                //更改用户昵称和个签
                case "2":
                    strReturnValue = new NewPc28Api().updateNick(context, info);
                    break;
                //绑定手机
                case "3":
                    strReturnValue = new NewPc28Api().bindMobile(context, info);
                    break;
                //更改提现密码
                case "4":
                    strReturnValue = new NewPc28Api().tixianPass(context, info);
                    break;
                //更改登录密码
                case "5":
                    strReturnValue = new NewPc28Api().changePass(context, info);
                    break;
                //更改银行卡信息
                case "6":
                    strReturnValue = new NewPc28Api().bindBank(context, info);
                    break;
                //充值请求
                case "7":
                    strReturnValue = new NewPc28Api().recharge(context, info);
                    break;
                //提现请求
                case "8":
                    strReturnValue = new NewPc28Api().tixian(context, info);
                    break;

                //获取充值记录
                case "12":
                    strReturnValue = new NewPc28Api().payStatics(context, info);
                    break;
                //获取提现记录
                case "13":
                    strReturnValue = new NewPc28Api().distillStatics(context, info);
                    break;
                //获取帐变记录
                case "14":
                    strReturnValue = new NewPc28Api().ballanceStatics(context, info);
                    break;
                //获取购彩记录
                case "15":
                    strReturnValue = new NewPc28Api().getLotHis(context, info);
                    break;
                //获取网站公告
                case "16":
                    strReturnValue = new NewPc28Api().getNotice(context, info);
                    break;

                //绑定推荐人
                case "17":
                    strReturnValue = new NewPc28Api().bindReffer(context, info);
                    break;
                //获得推荐人和代理组信息
                case "18":
                    strReturnValue = new NewPc28Api().getReffer(context, info);
                    break;

                //获得回水记录
                case "19":
                    strReturnValue = new NewPc28Api().getBackWater(context, info);
                    break;

                //获得充值方式列表
                case "20":
                    strReturnValue = new NewPc28Api().getPayMethods(context, info);
                    break; 
                    
                default:
                    break;
            }


            context.Response.Clear();
            context.Response.Write(strReturnValue);
            context.ApplicationInstance.CompleteRequest();

            return;

        }
        catch (Exception ex)
        {
            new Log("Invest Exception").Write("Exception:" + ex.Message + "\t");
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("{\"error\":\"-100000\",");
            sb.Append("\"msg\":\"" + ex.Message + "\"");
            sb.Append("}");
            context.Response.Clear();
            context.Response.Write(sb.ToString());
            context.ApplicationInstance.CompleteRequest();
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }



    #region 模型
    //用户数据模型
    public struct ApiJson
    {
        public string accessToken { get; set; }
        public string timeStamp { get; set; }
        public string uid { get; set; }
    }



    #endregion
    
    
    
}