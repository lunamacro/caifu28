
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Shove.Database;
public partial class Join_Default : SitePageBase
{
    public string lottidt = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            databand();
            this.lid.Value = Shove._Web.Utility.GetRequest("LotteryID");
        }
    }


    public void databand()
    {
        //绑定彩种

        StringBuilder sbb = new StringBuilder();

        DataTable db = MSSQL.Select("select UseLotteryListRestrictions from T_Sites ");

        string id = db.Rows[0]["UseLotteryListRestrictions"].ToString();

        DataTable dt = MSSQL.Select(" select ID,Name from T_Lotteries where ID in (" + id + ") order by [order] asc ");

        for (int i = 0; i < dt.Rows.Count; i++)
        {

            sbb.Append(" <li  id=\"li" + dt.Rows[i]["ID"] + "\" onclick=\"uid=0;pags=1; clik(" + dt.Rows[i]["ID"] + "); \"><a>" + dt.Rows[i]["Name"].ToString() + "</a>|<span></span><input type=\"text\"  id=\"td\"  value=" + dt.Rows[i]["ID"] + " style=\" display:none;\"  /></li>");

        }
        lottidt = sbb.ToString();

    }
}





