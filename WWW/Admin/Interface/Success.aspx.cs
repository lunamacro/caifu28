using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Interface_Success : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Tip = Shove._Web.Utility.GetRequest("Tip");

        labTip.Text = Tip;
    }
}