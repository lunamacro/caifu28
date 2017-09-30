using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_userControls_IndexHeader : UserControlBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        this.img_logo.ImageUrl = GetImg();
        string url = this.Request.Url.ToString();
        if (url.IndexOf("Default") > -1)
        {
            this.Default.Attributes.Add("class", "curr");
        }
        if (url.IndexOf("News") > -1)
        {
            this.News.Attributes.Add("class", "curr");
        }

        if (url.IndexOf("Contact") > -1)
        {
            this.Contact.Attributes.Add("class", "curr");
        }
    }

}