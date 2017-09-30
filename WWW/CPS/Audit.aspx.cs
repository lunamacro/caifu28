using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_Audit : CPSPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (_User == null || -1 == _User.ID)
        {
            Response.Redirect("/CPS/Default.aspx", true);
            return;
        }
        #region 查询申请身份、拒绝理由
        DataTable dt = new DataTable();
        DAL.Tables.T_Cps tcps = new DAL.Tables.T_Cps();
        dt = tcps.Open("ResultMemo,[Type]", "OwnerUserID=" + _User.ID, "");
        string strResultMemo = "";//拒绝理由
        string strType = "";//申请身份
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["Type"].ToString() == "1")//代理商
            {
                strType = "[ <span  font-size:13px;'>代理商</span> ]";
            }
            else//推广员
            {
                strType = "[ <span  font-size:13px;'>推广员</span> ]";
            }
            strResultMemo = "[ <span style='font-size:13px;'>理由：" + dt.Rows[0]["ResultMemo"] + "</span> ]";
        }
        #endregion
        string Type = Shove._Web.Utility.GetRequest("Type");
        if ("" == Type) Type = "Audit";
        switch (Type)
        {
            case "Audit":
                this.lbl_state.Text = "尊敬的用户，欢迎加入推广联盟。您申请的身份" + strType + " [ <span style='color:red; font-size:13px;'>正在审核中</span> ] ，请耐心等待。<br/>我们会在3-5个工作日之内完成您提交的申请。";
                break;
            case "Refused":
                this.lbl_state.Text = "<br/>尊敬的用户，欢迎加入推广联盟。您申请的身份" + strType + " [ <span style='color:red; font-size:13px;'>被拒绝</span> ] 。" + strResultMemo + "<br/><p class=\"reapplybtn\"><a href=\"NotCPS.aspx\" class='btnAction' style='display:block'>再次申请</a></p>";
                break;
            case "Disable":
                this.lbl_state.Text = "<br/>尊敬的用户，欢迎加入推广联盟。您的帐号状态 [ <span style='color:red; font-size:13px;'>被禁用</span> ] ，请联系上级代理商或系统管理员。<br/>";
                break;
        }
        if (!IsPostBack)
        {
            BindNews();
        }
    }

    private void BindNews()
    {
        DataTable dt = new DAL.Tables.T_News().Open("top 10 ID,DateTime,Title", "TypeID = 103002", "DateTime desc");
        if (null != dt && dt.Rows.Count > 0)
        {
            this.rpt_newsList.DataSource = dt;
            this.rpt_newsList.DataBind();
        }
    }
}