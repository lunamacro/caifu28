using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_userControls_Footer : System.Web.UI.UserControl
{
    //public string CopyRight = "深圳晓风软件公司";
    //public string ICP = "粤ICP备090151372";
    //public string Address = "深圳宝安中心区宝源路F518时尚创意园6栋/15栋/17栋";
    public string CopyRight = "";
    public string ICP = "";
    public string Address = "";
    public string Email = "";
    public string ServiceTelephone = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        string sql = "select [Address],ICPCert,Company,ServiceTelephone,Email from T_Sites";
        DataTable dt = Shove.Database.MSSQL.Select(sql);
        if (null != dt && dt.Rows.Count > 0)
        {
            CopyRight = dt.Rows[0]["Company"].ToString();
            ICP = dt.Rows[0]["ICPCert"].ToString();
            Address = dt.Rows[0]["Address"].ToString();
            ServiceTelephone = dt.Rows[0]["ServiceTelephone"].ToString();
            Email = dt.Rows[0]["Email"].ToString();
        }
    }
}