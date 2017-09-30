using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CPS_PromotionAgeement : CPSPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string key = "Opt_CPSRegisterAgreement";
        labAgreement.InnerHtml = _Site.SiteOptions[key].ToString("");
    }
}