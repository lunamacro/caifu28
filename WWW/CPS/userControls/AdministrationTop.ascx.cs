using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CPS_userControls_AdministrationTop : UserControlBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.img_logo.ImageUrl = GetImg();
        string url = this.Request.Url.ToString();
        if (url.IndexOf("AgentAuditing") > -1 || url.IndexOf("AgentAuditingNotOK") > -1 || url.IndexOf("PromoteAuditing") > -1 || url.IndexOf("PromoteAuditingNotOK") > -1 || url.IndexOf("Auditing") > -1)
        {
            this.sjAuditing.Attributes.Add("class", "curr");
        }
        if (url.IndexOf("PromoteManagement") > -1 || url.IndexOf("AgentManagement") > -1 || url.IndexOf("AgentThePromoteManagement") > -1 || url.IndexOf("MemberList") > -1)
        {
            this.sjManagement.Attributes.Add("class", "curr");
        }
        if (url.IndexOf("SystemParamSet") > -1 || url.IndexOf("CommissionScale") > -1 || url.IndexOf("CommissionEdit") > -1 || url.IndexOf("UserTransfer") > -1 || url.IndexOf("UserTransferTwo") > -1)
        {
            this.xtSet.Attributes.Add("class", "curr");
        }
        if (url.IndexOf("CommissionManagement") > -1 || url.IndexOf("CommissionDetail") > -1 || url.IndexOf("CommissionGiveOut") > -1 || url.IndexOf("CommissionGiveOutTwo") > -1 || url.IndexOf("CPSAccountBook") > -1 || url.IndexOf("UserBuyLotteryDetails") > -1)
        {
            this.yjManagement.Attributes.Add("class", "curr");
        }
    }

    protected void btnLoginOut_Click(object sender, EventArgs e)
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
            Response.Redirect("../Default.aspx", true);
        }
    }
}