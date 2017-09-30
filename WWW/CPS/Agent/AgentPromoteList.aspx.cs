using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;

public partial class CPS_Agent_AgentPromoteList : CPSAgentBase
{
    public int pageSize = 15;
    public int PageIndex = 1;
    public long PageCount = 0;//总页数
    public long DataCount = 0;//总数据

    //public string ToolTips = "";

    //public string Moneys = "";

    public StringBuilder TDS = new StringBuilder();

    public StringBuilder Divs = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        //获得当前页码
        PageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("index"), 1);

        if (!this.IsPostBack)
        {
            BindDataForYearMonth0();
            BindDataForYearMonth1();
            BinDataForDay0();
            BinDataForDay1();
            //BindDataForYearMonth();
        }

        BindData();
    }


    private void BindDataForYearMonth0()
    {
        ddlYear0.Items.Clear();

        DateTime dt = DateTime.Now;
        int Year = dt.Year;
        int Month = dt.Month;

        if (Year < PF.SystemStartYear)
        {
            btnGO.Enabled = false;

            return;
        }

        for (int i = PF.SystemStartYear; i <= Year; i++)
        {
            ddlYear0.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear0.SelectedIndex = ddlYear0.Items.Count - 1;

        ddlMonth0.SelectedIndex = Month - 1;
    }

    private void BindDataForYearMonth1()
    {
        ddlYear1.Items.Clear();

        DateTime dt = DateTime.Now;
        int Year = dt.Year;
        int Month = dt.Month;

        if (Year < PF.SystemStartYear)
        {
            btnGO.Enabled = false;

            return;
        }

        for (int i = PF.SystemStartYear; i <= Year; i++)
        {
            ddlYear1.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear1.SelectedIndex = ddlYear1.Items.Count - 1;

        ddlMonth1.SelectedIndex = Month - 1;
    }

    private void BinDataForDay0()
    {
        ddlDay0.Items.Clear();

        int[] Month = new Int32[7];
        Month[0] = 1;
        Month[1] = 3;
        Month[2] = 5;
        Month[3] = 7;
        Month[4] = 8;
        Month[5] = 10;
        Month[6] = 12;

        DateTime dtTime = DateTime.Now;
        int Year = dtTime.Year;
        int Day = dtTime.Day;
        int i = int.Parse(ddlMonth0.SelectedValue);
        int MaxDay = 0;

        if (Month.Contains(i))
        {
            MaxDay = 31;
        }
        else if (i == 2)
        {
            if (((Year % 4) == 0) && ((Year % 100) != 0) && ((Year % 400) == 0))
            {
                MaxDay = 29;
            }
            else
            {
                MaxDay = 28;
            }
        }
        else
        {
            MaxDay = 30;
        }

        for (int j = 1; j <= MaxDay; j++)
        {
            ddlDay0.Items.Add(new ListItem(j.ToString() + "日", j.ToString()));
        }

        if (Day > MaxDay)
        {
            Day = MaxDay;
        }

        ddlDay0.SelectedIndex = Day - 1;
    }

    private void BinDataForDay1()
    {
        ddlDay1.Items.Clear();

        int[] Month = new Int32[7];
        Month[0] = 1;
        Month[1] = 3;
        Month[2] = 5;
        Month[3] = 7;
        Month[4] = 8;
        Month[5] = 10;
        Month[6] = 12;

        DateTime dtTime = DateTime.Now;
        int Year = dtTime.Year;
        int Day = dtTime.Day;
        int i = int.Parse(ddlMonth1.SelectedValue);
        int MaxDay = 0;

        if (Month.Contains(i))
        {
            MaxDay = 31;
        }
        else if (i == 2)
        {
            if (((Year % 4) == 0) && ((Year % 100) != 0) && ((Year % 400) == 0))
            {
                MaxDay = 29;
            }
            else
            {
                MaxDay = 28;
            }
        }
        else
        {
            MaxDay = 30;
        }

        for (int j = 1; j <= MaxDay; j++)
        {
            ddlDay1.Items.Add(new ListItem(j.ToString() + "日", j.ToString()));
        }

        if (Day > MaxDay)
        {
            Day = MaxDay;
        }

        ddlDay1.SelectedIndex = Day - 1;
    }

    protected void ddlMonth0_SelectedIndexChanged(object sender, EventArgs e)
    {
        BinDataForDay0();
    }

    protected void ddlMonth1_SelectedIndexChanged(object sender, EventArgs e)
    {
        BinDataForDay1();
    }

    private void BindDataForYearMonth()
    {
        ddlYear.Items.Clear();

        DateTime dt = DateTime.Now;
        int Year = dt.Year;
        int Month = dt.Month;

        if (Year < PF.SystemStartYear)
        {
            return;
        }

        for (int i = PF.SystemStartYear; i <= Year; i++)
        {
            ddlYear.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear.SelectedIndex = ddlYear.Items.Count - 1;

        ddlMonth.SelectedIndex = Month - 1;
    }

    private void BindData()
    {

        DateTime startTime = new DateTime(Convert.ToInt32(ddlYear0.SelectedValue), Convert.ToInt32(ddlMonth0.SelectedValue), Convert.ToInt32(ddlDay0.SelectedValue));
        DateTime endTime = new DateTime(Convert.ToInt32(ddlYear1.SelectedValue), Convert.ToInt32(ddlMonth1.SelectedValue), Convert.ToInt32(ddlDay1.SelectedValue)).AddDays(1).AddSeconds(-1);
        //String agentKeyword = txtAgentKeyword.Text.Trim();
        String agentKeyword = _User.Name;


        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
        SqlCommand cmd1 = new SqlCommand("P_GetFinanceBalanceAgent", conn);
        conn.Open();
        cmd1.CommandType = CommandType.StoredProcedure;
        cmd1.Parameters.Add("@siteID", SqlDbType.Int);
        cmd1.Parameters["@siteID"].Value = _Site.ID;

        cmd1.Parameters.Add("@startTime", SqlDbType.DateTime);
        cmd1.Parameters["@startTime"].Value = startTime;

        cmd1.Parameters.Add("@endTime", SqlDbType.DateTime);
        cmd1.Parameters["@endTime"].Value = endTime;

        cmd1.Parameters.Add("@agentKeyword", SqlDbType.VarChar);
        cmd1.Parameters["@agentKeyword"].Value = agentKeyword;

        //Response.Write("<script>alert('" + startTime.ToString() + "')</script>");


        DataSet ds = new DataSet();
        try
        {
            cmd1.CommandTimeout = 60;
            SqlDataReader sdr = cmd1.ExecuteReader();

            ds = ConvertDataReaderToDataSet(sdr);
        }
        catch (Exception e)
        {

            throw;
        }
        finally
        {
            conn.Close();
        }


        //Shove.Database.MSSQL.Parameter[] paramArray = new Shove.Database.MSSQL.Parameter[3];
        //paramArray[0] = new Shove.Database.MSSQL.Parameter("siteID", SqlDbType.BigInt, 0, ParameterDirection.Input, _Site.ID);
        //paramArray[1] = new Shove.Database.MSSQL.Parameter("startTime", SqlDbType.DateTime, 0, ParameterDirection.Input, startTime);
        //paramArray[2] = new Shove.Database.MSSQL.Parameter("endTime", SqlDbType.DateTime, 0, ParameterDirection.Input, endTime);
        //实例化分页对象
        PagedDataSource pg = new PagedDataSource();
        pg.AllowPaging = true;//设置启用分页
        pg.PageSize = pageSize;//设置每页显示数据
        pg.DataSource = new DataView();
        // pg.DataSource = ds;//分页数据源
        //设置分页索引
        pg.CurrentPageIndex = PageIndex - 1;
        //设置共总页数
        PageCount = pg.PageCount;
        DataCount = pg.DataSourceCount;

        this.rptSchemes.DataSource = ds;
        this.rptSchemes.DataBind();
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BindData();
    }

 
    protected void btnGO_Click(object sender, EventArgs e)
    {
        // 查询
        BindData();
    }

    public DataSet ConvertDataReaderToDataSet(SqlDataReader reader)
    {
        DataSet dataSet = new DataSet();
        do
        {
            // Create new data table  
            DataTable schemaTable = reader.GetSchemaTable();
            DataTable dataTable = new DataTable();
            if (schemaTable != null)
            {
                // A query returning records was executed   
                for (int i = 0; i < schemaTable.Rows.Count; i++)
                {
                    DataRow dataRow = schemaTable.Rows[i];
                    // Create a column name that is unique in the data table   
                    string columnName = (string)dataRow["ColumnName"]; //+ " // Add the column definition to the data table   
                    DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                    dataTable.Columns.Add(column);
                }
                dataSet.Tables.Add(dataTable);
                // Fill the data table we just created  
                while (reader.Read())
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dataRow[i] = reader.GetValue(i);
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }
            else
            {
                // No records were returned  
                DataColumn column = new DataColumn("RowsAffected");
                dataTable.Columns.Add(column);
                dataSet.Tables.Add(dataTable);
                DataRow dataRow = dataTable.NewRow();
                dataRow[0] = reader.RecordsAffected;
                dataTable.Rows.Add(dataRow);
            }
        }
        while (reader.NextResult());
        return dataSet;
    }

}