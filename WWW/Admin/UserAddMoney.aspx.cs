using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

public partial class Admin_UserAddMoney : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isRequestLogin = true;
        RequestLoginPage = "Admin/UserAddMoney.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.UserRecharge);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = new DAL.Tables.T_Sites().Open("[ID], [Name]", "", "[Level], [ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_UserAddMoney");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlSites, dt, "Name", "ID");
    }

    protected void btnGO_Click(object sender, EventArgs e)
    {
        long SiteID = Shove._Convert.StrToLong(ddlSites.SelectedValue, -1);

        Regex r = new Regex(@"([1-9]\d*\.?\d*)|(0\.\d*[1-9])");

        if (SiteID < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入有效的站点编号。");

            return;
        }

        if (tbUserName.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入用户名称。");

            return;
        }

        if (tbMoney.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入充值金额。");

            return;
        }

        Match m = r.Match(tbMoney.Text.Trim());
        if (!m.Success)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的充值金额。");

            return;
        }

        double Money = Shove._Convert.StrToDouble(tbMoney.Text, 0);

        //if (Money <= 0)
        //{
        //    Shove._Web.JavaScript.Alert(this.Page, "请输入有效的金额。");

        //    return;
        //}

        //判断用户余额是否大于输入金额 如果大于则充值成功 否则失败

        double UserMoney;
        string strWhere = tbUserName.Text.Trim();
        DataTable user = new DAL.Tables.T_Users().Open(" ID, Balance ", " Name= '" + strWhere + "'", "");
        if (user.Rows.Count > 0)
        {
            UserMoney = Shove._Convert.StrToDouble(user.Rows[0]["Balance"].ToString(), 0);
        }

        else
        {
            Shove._Web.JavaScript.Alert(this.Page, "用户名不存在");
            return;
        }
        if ((Money * (-1)) > UserMoney)
        {

            Shove._Web.JavaScript.Alert(this.Page, "您的余额不足，谢谢!");
            this.tbMoney.Focus();
            return;
        }
        if ((Money * (-1)) < UserMoney)
        {

            Users tu = new Users(SiteID);

            if (user.Rows.Count < 1)
            {
                Shove._Web.JavaScript.Alert(this.Page, "您输入的用户名不存在，请重新输入。");
                this.tbUserName.Focus();

                return;
            }

            long uid = Shove._Convert.StrToLong(user.Rows[0]["ID"].ToString(), -1);

            string Message = "手工充值";
            string ReturnDescription = "";

            if (rb2.Checked)
            {
                Message += "，获得的奖励";
            }
            else if (rb3.Checked)
            {
                Message += "，购彩";
            }
            else if (rb4.Checked)
            {
                Message += "，预付款";
            }
            else if (rb5.Checked)
            {
                Message += "，转帐户";
            }
            else if (rb6.Checked)
            {
                Message += "，其它";
            }

            if (tbMessage.Text.Trim() != "")
            {
                Message += "，" + tbMessage.Text.Trim();
            }
            int isHand = 0;
            if (IsHandY.Checked)
            {
                isHand = 1;
            }
            if (tu.AddUserBalanceManual(Money, uid, Message, _User.ID, isHand, ref ReturnDescription) < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_UserAddMoney");

                return;
            }

            tbUserName.Text = "";
            tbMoney.Text = "";
            tbMessage.Text = "";

            Shove._Web.JavaScript.Alert(this, "为用户充值成功。");

        }

        /////////////////////////////////////////////////
        else
        {

            Users tu = new Users(SiteID)[SiteID, tbUserName.Text.Trim()];

            if (user.Rows.Count < 1)
            {
                Shove._Web.JavaScript.Alert(this.Page, "您输入的用户名不存在，请重新输入。");
                this.tbUserName.Focus();

                return;
            }

            long uid = Shove._Convert.StrToLong(user.Rows[0]["ID"].ToString(), -1);

            string Message = "手工充值";
            string ReturnDescription = "";

            if (rb2.Checked)
            {
                Message += "，获得的奖励";
            }
            else if (rb3.Checked)
            {
                Message += "，购彩";
            }
            else if (rb4.Checked)
            {
                Message += "，预付款";
            }
            else if (rb5.Checked)
            {
                Message += "，转帐户";
            }
            else if (rb6.Checked)
            {
                Message += "，其它";
            }

            if (tbMessage.Text.Trim() != "")
            {
                Message += "，" + tbMessage.Text.Trim();
            }
            int isHand = 0;
            if (IsHandY.Checked)
            {
                isHand = 1;
            }
            if (tu.AddUserBalanceManual(Money, uid, Message, _User.ID, isHand, ref ReturnDescription) < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_UserAddMoney");

                return;
            }

            tbUserName.Text = "";
            tbMoney.Text = "";
            tbMessage.Text = "";

            Shove._Web.JavaScript.Alert(this, "为用户充值成功。");
        }
    }
}
