using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AppSetting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {

        string sql = @"Select top 1 * from T_AppSetting ";

        DataTable dt = Shove.Database.MSSQL.Select(sql);

        if (null != dt && dt.Rows.Count > 0)
        {
            androidVersion.Text = dt.Rows[0]["androidVersion"].ToString();
            androidUrl.Text = dt.Rows[0]["androidUrl"].ToString();
            androidDes.Text = dt.Rows[0]["androidDes"].ToString();
            androidForce.Checked = dt.Rows[0]["androidForce"].ToString().Equals("1");
            androidType0.Checked = dt.Rows[0]["androidType"].ToString().Equals("0");
            androidType1.Checked = dt.Rows[0]["androidType"].ToString().Equals("1");


            IOSVersion.Text = dt.Rows[0]["IOSVersion"].ToString();
            IOSUrl.Text = dt.Rows[0]["IOSUrl"].ToString();
            IOSDes.Text = dt.Rows[0]["IOSDes"].ToString();
            IOSForce.Checked = dt.Rows[0]["IOSForce"].ToString().Equals("1");

            qrCodeUrl.Text = dt.Rows[0]["qrCodeUrl"].ToString();

            welcomeText.Text = dt.Rows[0]["welcomeText"].ToString();
            welcomeSwitch.Checked = int.Parse(dt.Rows[0]["welcomeSwitch"].ToString()) == 1;
        }
    }

    protected void btn_saveteul_Click(object sender, EventArgs e)
    {
        string S_androidVersion = androidVersion.Text;
        string S_androidUrl = androidUrl.Text;
        string S_androidDes = androidDes.Text;
        string S_androidForce = androidForce.Checked?"1":"0";
        string S_androidType = androidType0.Checked ? "0" : "1";

        string S_IOSVersion = IOSVersion.Text;
        string S_IOSUrl = IOSUrl.Text;
        string S_IOSDes = IOSDes.Text;
        string S_IOSForce = IOSForce.Checked ? "1" : "0";

        string qrCodeUrlString = qrCodeUrl.Text;
        string welcomeTextString = welcomeText.Text;
        int welcomtSwitchVal = welcomeSwitch.Checked ? 1 : 0;


        string updateSQL = @"UPDATE T_AppSetting SET androidVersion='" + S_androidVersion + "',androidUrl='" + S_androidUrl + "',androidDes='" + S_androidDes + "',androidForce=" + S_androidForce + ",androidType=" + S_androidType + ",IOSVersion='" + S_IOSVersion + "',IOSUrl='" + S_IOSUrl + "',IOSDes='" + S_IOSDes + "',IOSForce=" + S_IOSForce + ",qrCodeUrl='" + qrCodeUrlString + "',welcomeText='" + welcomeTextString + "',welcomeSwitch=" + welcomtSwitchVal + "  WHERE ID = 1";

        int result = Shove.Database.MSSQL.ExecuteNonQuery(updateSQL);
        if (result < 0)
        {
            Shove._Web.JavaScript.Alert(this, "保存失败。");
            return;
        }
        Shove._Web.JavaScript.Alert(this, "保存成功。");
    }

}