using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_MobileValided : CPSPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (_User == null || _User.ID == -1)
        {
            Response.Redirect("/CPS/Default.aspx", true);
        }
    }

    /// <summary>
    /// 立即绑定按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnMobileBind_Click(object sender, EventArgs e)
    {
        string mobile = Shove._Web.Utility.FilteSqlInfusion(this.txtMobile.Text.Trim());
        string code = Shove._Web.Utility.FilteSqlInfusion(this.txtCode.Text.Trim());
        if ("" == mobile)
        {
            Shove._Web.JavaScript.Alert(this, "请输入手机号码");
            return;
        }
        int result = FlyFish.Common.CommonCode.CheckIsMobile(mobile);
        if (result < 0)
        {
            Shove._Web.JavaScript.Alert(this, "请输入正确的手机号码");
            return;
        }
        if (CPSBLL.CheckMobileIsUse(Convert.ToInt64(mobile)))
        {
            Shove._Web.JavaScript.Alert(this, "手机号码已经被使用");
            return;
        }
        if ("" == code)
        {
            Shove._Web.JavaScript.Alert(this, "请输入验证码");
            return;
        }
        DataTable dt = new DAL.Tables.T_SMS().Open(" top 1 ID", " [DateTime] > DATEADD(MINUTE,-10,GETDATE()) and [To] = '" + mobile + "' and VerifyCode = '" + code + "'", " [DateTime] desc");

        if (dt == null || dt.Rows.Count <= 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "验证码错误");
            return;
        }

        DAL.Tables.T_Users user = new DAL.Tables.T_Users();
        user.Mobile.Value = mobile;
        user.isMobileValided.Value = true;
        long result1 = user.Update("ID=" + _User.ID);
        if (result1 < 0)
        {
            Shove._Web.JavaScript.Alert(this, "手机验证失败");
            return;
        }
        string returnUrl = Shove._Web.Utility.GetRequest("returnUrl");
        string action = Shove._Web.Utility.GetRequest("action");
        if ("login" == action)
        {
            dt = new DAL.Tables.T_Cps().Open("ID,OwnerUserID,[ON],HandleResult,Type,HandlelDateTime", "OwnerUserID=" + _User.ID, "");
            DataRow dr = dt.Rows[0];
            if (null != dt && dt.Rows.Count > 0 && Convert.ToInt32(dr["HandleResult"]) == 1)
            {

                //验证帐号是否被禁用
                if (!Shove._Convert.StrToBool(dr["ON"].ToString(), false))
                {
                    Shove._Web.JavaScript.Alert(this, "验证成功", "Audit.aspx?Type=Disable");
                }
                //处理结果 0表示还没有审核完成   1表示审核完成 
                string HandleResult = dt.Rows[0]["HandleResult"].ToString();
                switch (HandleResult)
                {
                    case "0":
                        Shove._Web.JavaScript.Alert(this, "验证成功", "Audit.aspx?Type=Audit");
                        break;
                    case "1":
                        //判断用户属于那个类型的，1是代理商 2是推广员
                        switch (dr["Type"].ToString())
                        {
                            case "2":
                                Shove._Web.JavaScript.Alert(this, "验证成功", "Promote/PromoteIndex.aspx");
                                break;
                            case "1":
                                Shove._Web.JavaScript.Alert(this, "验证成功", "Agent/AgentIndex.aspx");
                                break;
                        }
                        break;
                    default:
                        Shove._Web.JavaScript.Alert(this, "验证成功", "Audit.aspx?Type=Refused");
                        break;
                }
            }
            else
            {
                Shove._Web.JavaScript.Alert(this, "验证成功", "Audit.aspx?Type=Audit");
            }
        }
        Shove._Web.JavaScript.Alert(this, "验证成功", (returnUrl == "" ? "Default.aspx" : returnUrl));
    }
}