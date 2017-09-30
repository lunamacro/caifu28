using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;

public partial class CPS_Apply_Promote : CPSPage
{
    public string type = "tgy";
    protected void Page_Load(object sender, EventArgs e)
    {
        type = Shove._Web.Utility.GetRequest("type");
        if (string.IsNullOrEmpty(type))
        {
            type = "tgy";
        }
        switch (type)
        {
            case "tgy": //推广员
                this.tgy.Attributes.Add("class", "curr");
                this.dls.Attributes.Add("class", "");
                this.div_tgy.Style.Add("display", "block");
                this.div_dls.Style.Add("display", "none");
                break;
            case "dls": //代理商
                this.tgy.Attributes.Add("class", "");
                this.dls.Attributes.Add("class", "curr");
                this.div_tgy.Style.Add("display", "none");
                this.div_dls.Style.Add("display", "block");
                break;
        }
    }

    /// <summary>
    /// 注册按钮-推广员
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRegister_tgy_Click(object sender, EventArgs e)
    {
        try
        {
            CPSBLL cps = new CPSBLL();
            string returnDesc = "";
            string name = Shove._Web.Utility.FilteSqlInfusion(this.txtName_tgy.Text.Trim());
            string pwd = Shove._Web.Utility.FilteSqlInfusion(this.txtPwd_tgy.Text.Trim());
            string phone = Shove._Web.Utility.FilteSqlInfusion(this.txtPhone_tgy.Text.Trim());

            long result1 = new DAL.Tables.T_Users().GetCount("Name='" + name + "' or or Mobile = '" + name + "'");
            if (result1 > 0)
            {
                Shove._Web.JavaScript.Alert(this, "用户名已经被使用");
                return;
            }

            CPSBLL.CPSRegisterEnum result = cps.CPSRegister("tgy", name, pwd, phone, -1, ref returnDesc);
            switch (result)
            {
                case CPSBLL.CPSRegisterEnum.UserNameNull:
                    Shove._Web.JavaScript.Alert(this, "请输入用户名");
                    break;
                case CPSBLL.CPSRegisterEnum.UserNameNoOK:
                    Shove._Web.JavaScript.Alert(this, "用户名只能是 中文、_ 、数字、大小写字母组成");
                    break;
                case CPSBLL.CPSRegisterEnum.UserPwdNull:
                    Shove._Web.JavaScript.Alert(this, "请输入密码");
                    break;
                case CPSBLL.CPSRegisterEnum.UserPwdNoOK:
                    Shove._Web.JavaScript.Alert(this, "密码只能是 _ 、数字、大小写字母组成");
                    break;
                case CPSBLL.CPSRegisterEnum.RegisterFail:
                    Shove._Web.JavaScript.Alert(this, returnDesc);
                    break;
                case CPSBLL.CPSRegisterEnum.MobileNull:
                    Shove._Web.JavaScript.Alert(this, "请输入手机号码");
                    break;
                case CPSBLL.CPSRegisterEnum.MobileNoOK:
                    Shove._Web.JavaScript.Alert(this, "手机号码只能11位数字组成");
                    break;
                case CPSBLL.CPSRegisterEnum.RegisterOK:
                    Shove._Web.JavaScript.Alert(this, "注册成功,注册信息审核中", "Default.aspx?userName=" + this.txtName_tgy.Text.Trim());
                    break;
            }
        }
        catch (Exception ex)
        {
            Shove._Web.JavaScript.Alert(this, "注册失败，服务器繁忙");
        }
    }

    /// <summary>
    /// 注册按钮-代理商
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRegister_dls_Click(object sender, EventArgs e)
    {
        try
        {
            CPSBLL cps = new CPSBLL();
            string returnDesc = "";
            string name = Shove._Web.Utility.FilteSqlInfusion(this.txtName_dls.Text.Trim());
            string pwd = Shove._Web.Utility.FilteSqlInfusion(this.txtPwd_dls.Text.Trim());
            string phone = Shove._Web.Utility.FilteSqlInfusion(this.txtPhone_dls.Text.Trim());

            CPSBLL.CPSRegisterEnum result = cps.CPSRegister("dls", name, pwd, phone, -1, ref returnDesc);
            switch (result)
            {
                case CPSBLL.CPSRegisterEnum.UserNameNull:
                    Shove._Web.JavaScript.Alert(this, "请输入用户名");
                    break;
                case CPSBLL.CPSRegisterEnum.UserNameNoOK:
                    Shove._Web.JavaScript.Alert(this, "用户名只能是 中文、_ 、数字、大小写字母组成");
                    break;
                case CPSBLL.CPSRegisterEnum.UserPwdNull:
                    Shove._Web.JavaScript.Alert(this, "请输入密码");
                    break;
                case CPSBLL.CPSRegisterEnum.UserPwdNoOK:
                    Shove._Web.JavaScript.Alert(this, "密码只能是 _ 、数字、大小写字母组成");
                    break;
                case CPSBLL.CPSRegisterEnum.RegisterFail:
                    Shove._Web.JavaScript.Alert(this, returnDesc);
                    break;
                case CPSBLL.CPSRegisterEnum.MobileNull:
                    Shove._Web.JavaScript.Alert(this, "请输入手机号码");
                    break;
                case CPSBLL.CPSRegisterEnum.MobileNoOK:
                    Shove._Web.JavaScript.Alert(this, "手机号码只能11位数字组成");
                    break;
                case CPSBLL.CPSRegisterEnum.RegisterOK:
                    Shove._Web.JavaScript.Alert(this, "注册成功,注册信息审核中", "Default.aspx?userName=" + this.txtName_dls.Text.Trim());
                    break;
            }
        }
        catch (Exception ex)
        {
            Shove._Web.JavaScript.Alert(this, "注册失败，服务器繁忙");
        }
    }
}