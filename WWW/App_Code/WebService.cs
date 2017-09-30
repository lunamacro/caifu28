using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// WebService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{

    public WebService()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    /// <summary>
    /// APP 充值接口
    /// </summary>
    /// <param name="username">用户</param>
    /// <param name="paytime">充值时间</param>
    /// <param name="amount">金额</param>
    /// <param name="accountName">付款姓名</param>
    /// <param name="accountNumber">银行帐号</param>
    /// <returns></returns>
    [WebMethod]
    public string UserAddMoney(string username, DateTime paytime, double amount, string accountName, string accountNumber)
    {
        long SiteID = 1;

        if (string.IsNullOrEmpty(username))
            return "请输入用户名称。";

        if (0 == amount)
            return "请输入充值金额。";

        DataTable user = new DAL.Tables.T_Users().Open(" ID, Balance ", string.Format(" Name= '{0}'", username), "");
        if (0 >= user.Rows.Count)
            return "用户名不存在";

        Users tu = new Users(SiteID);
        long uid = Shove._Convert.StrToLong(user.Rows[0]["ID"].ToString(), -1);
        string Message = "手工充值";
        string ReturnDescription = "";
        if (tu.AddUserBalanceManualByPayType(PayTypeEnum.APP端充值.ToString(), amount, uid, Message, uid, 0, ref ReturnDescription) < 0)
            return ReturnDescription;

        return "提交充值订单成功。";
    }
}
