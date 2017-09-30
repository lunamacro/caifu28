using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using System.Text;
using System.IO;

public partial class CPS_Default : CPSPage
{
    string userName = "";
    public string promotionAlliance = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            userName = Shove._Web.Utility.GetRequest("userName");
            if ("" != userName)
            {
                this.txtName.Value = userName;
            }
            DataTable dt = new DAL.Tables.T_Sites().Open("PromotionAlliance", "ID=1", "");
            if (dt != null && dt.Rows.Count > 0)
            {
                promotionAlliance = dt.Rows[0][0].ToString();
            }
        }
    }

    /// <summary>
    /// 登录按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string name = Shove._Web.Utility.FilteSqlInfusion(this.txtName.Value.Trim());
        string pwd = Shove._Web.Utility.FilteSqlInfusion(this.txtPwd.Value.Trim());
        string ReturnDescription = "";
        CPSBLL cps = new CPSBLL();
        string showMessage = "";
        try
        {
            CPSBLL.CPSLoginEnum result = cps.CPSLogin(name, pwd, ref ReturnDescription);
           // this.ShowLoginTip("jg=" + result.ToString());
            //return;

            switch (result)
            {
                case CPSBLL.CPSLoginEnum.UserNameNull:
                    this.ShowLoginTip("请输入用户名");
                    return;
                case CPSBLL.CPSLoginEnum.UserNameNoOK:
                    this.ShowLoginTip("用户名只能是 中文、_ 、数字、大小写字母组成");
                    return;
                case CPSBLL.CPSLoginEnum.UserPwdNull:
                    this.ShowLoginTip("请输入密码");
                    return;
                case CPSBLL.CPSLoginEnum.UserPwdNoOK:
                    this.ShowLoginTip("密码只能是 _ 、数字、大小写字母组成");
                    return;
                case CPSBLL.CPSLoginEnum.LoginFail://登陆失败
                    showMessage = ReturnDescription == "" ? "登陆失败" : ReturnDescription;
                    break;
                case CPSBLL.CPSLoginEnum.AdministratorLogin://超级管理员登录
                    Response.Redirect("admin/AgentAuditing.aspx", true);
                    break;
                case CPSBLL.CPSLoginEnum.NotCPS://不是CPS用户
                    Shove._Web.JavaScript.Alert(this, "你还不是CPS用户", "NotCPS.aspx");
                    return;
                case CPSBLL.CPSLoginEnum.UserDisable://用户被禁用
                    Response.Redirect("Audit.aspx?Type=Disable", true);
                    break;
                case CPSBLL.CPSLoginEnum.Audit://审核中
                    Response.Redirect("Audit.aspx?Type=Audit", true);
                    return;
                case CPSBLL.CPSLoginEnum.Refuse://拒绝                                                                                                                                       
                    Response.Redirect("Audit.aspx?Type=Refused", true);
                    break;
                case CPSBLL.CPSLoginEnum.PromoteLogin://推广员登录
                    Response.Redirect("Promote/PromoteIndex.aspx", true);
                    break;
                case CPSBLL.CPSLoginEnum.AgentLogin://代理商登录
                    Response.Redirect("Agent/AgentIndex.aspx", true);
                    break;
                case CPSBLL.CPSLoginEnum.isMobileValided://手机未验证
                    Shove._Web.JavaScript.Alert(this, "请先绑定手机号码", "MobileValided.aspx?action=login");
                    return;
            }
            Shove._Web.JavaScript.Alert(this, showMessage);
        }
        catch (Exception ex)
        {
            Shove._Web.JavaScript.Alert(this, "登录失败," + ReturnDescription);
        }
        this.loginTip.InnerText = "";
        this.loginTip.Style.Add("display", "none");
    }

    /// <summary>
    /// 登陆错误提示
    /// </summary>
    /// <param name="tip"></param>
    public void ShowLoginTip(string tip)
    {
        this.loginTip.InnerText = tip;
        this.loginTip.Style.Add("display", "block");
    }

    public string GetPics()
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + "/Uploadfile/CPSPic";
        string[] fileArray = Directory.GetFiles(path);
        StringBuilder picSb = new StringBuilder();
        for (int i = 0; i < fileArray.Length; i++)
        {
            FileInfo fileInfo = new FileInfo(fileArray[i]);
            picSb.Append("<li><img src=\"http://" + Context.Request.Url.Authority + "/Uploadfile/CPSPic/" + fileInfo.Name + "\" alt=\"\" /></li>");
        }
        return picSb.ToString();
    }
}