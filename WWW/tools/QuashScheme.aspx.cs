using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class tools_QuashScheme : System.Web.UI.Page
{
    private SqlConnection conn;


    protected void Page_Load(object sender, EventArgs e)
    {
        String connectionString = @"Data Source=.;Initial Catalog=SLS53_WangXianSheng;User ID=sa;Password=Vhk8kA5#w^;Connect Timeout=60";
        conn = Shove.Database.MSSQL.CreateDataConnection<System.Data.SqlClient.SqlConnection>(connectionString);


        int quashSchemeID = 0;

        DAL.Tables.T_Schemes dtSchemes = new DAL.Tables.T_Schemes();
        dtSchemes.Buyed.Value = 0;
        dtSchemes.isOpened.Value = 0;
        dtSchemes.Update(conn, "[ID]=" + quashSchemeID);

        int ReturnValue = -1;
        String ReturnDescription = "";
        int result = DAL.Procedures.P_QuashScheme(1L, quashSchemeID, true, true, ref ReturnValue, ref ReturnDescription);

        Response.Write("result = " + result + "<br>ReturnValue = " + ReturnValue + "<br>ReturnDescription = " + ReturnDescription);
    }
}