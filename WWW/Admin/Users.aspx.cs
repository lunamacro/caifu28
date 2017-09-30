using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Shove.Database;

public partial class Admin_Users : AdminPageBase
{
    public int pageSize = 15;
    public int PageIndex = 1;
    public long PageCount = 0;//总页数
    public long DataCount = 0;//总数据

    protected void Page_Load(object sender, EventArgs e)
    {
        //获得当前页码
        PageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("index"), 1);

        if (!this.IsPostBack)
        {
            Shove._Web.Cache.SetCache("FirstQuery_" + _User.ID, "1");
            DateTime DateTimeNow = System.DateTime.Now;
            tbBeginTime.Text = DateTimeNow.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
            tbEndTime.Text = DateTimeNow.ToString("yyyy-MM-dd HH:mm:ss");
        }
        BindData();
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.UserList);

        base.OnInit(e);
    }

    #endregion

    private void GetCompetence()
    {

        DataTable dt = Shove.Database.MSSQL.Select("select * from T_UserInGroups  where GroupID =1 and UserID=" + _User.ID + " ");


        if (dt != null && dt.Rows.Count > 0)
        {
            this.btnDownload.Visible = true;
            this.btnSelect.Visible = true;
        }
        else
        {
            this.btnDownload.Visible = false;
            this.btnSelect.Visible = false;
        }
    }

    private void BindData()
    {
        DataTable dt = null;
        string sql = "";
        string user = tbUserName.Text.Trim();

        
        if (Shove._Web.Cache.GetCache("FirstQuery_" + _User.ID) != null)
        {
            if (Shove._Web.Cache.GetCache("FirstQuery_" + _User.ID).ToString() == "1")    //默认情况， 显示所有会员
            {
                sql = @"select tb1.*,ISNULL(tb2.NickName,'') as ParentName, ISNULL(tb3.NickName,'') as GroupName from V_Users tb1 
left join T_Users tb2 on tb1.ReferId=tb2.ID left join T_Users tb3 on tb1.agentGroup=tb3.ID where  tb1.SiteID = @SiteID order by  tb1.RegisterTime desc";
                dt = MSSQL.Select(sql, new MSSQL.Parameter("SiteID", SqlDbType.Int, 0, ParameterDirection.Input, _Site.ID));
                //Response.Write("<script>alert('" + dt.Rows.Count + "')</script>");
                //Response.End();

            }
            else if (Shove._Web.Cache.GetCache("FirstQuery_" + _User.ID).ToString() == "2") //根据注册时间搜索
            {
                if (tbBeginTime.Text.Trim() == "" || tbEndTime.Text.Trim() == "")
                {
                    sql = "select tb1.*,ISNULL(tb2.NickName,'') as ParentName, ISNULL(tb3.NickName,'') as GroupName from V_Users tb1 left join T_Users tb2 on tb1.ReferId=tb2.ID left join T_Users tb3 on tb1.agentGroup=tb3.ID where  tb1.SiteID = @SiteID order by  tb1.RegisterTime desc";
                
                    dt = MSSQL.Select(sql, new MSSQL.Parameter("SiteID", SqlDbType.Int, 0, ParameterDirection.Input, _Site.ID));
                }
                else
                {

                    sql = @"select tb1.*,ISNULL(tb2.NickName,'') as ParentName, ISNULL(tb3.NickName,'') as GroupName from V_Users tb1 
            left join T_Users tb2 on tb1.ReferId=tb2.ID left join T_Users tb3 on tb1.agentGroup=tb3.ID 
            where (CONVERT(datetime,tb1.RegisterTime) between @StartDate and @EndDate or CONVERT(datetime,tb1.RegisterTime) = @EndDate) order by  tb1.RegisterTime desc";


                    DateTime dtBegin = Shove._Convert.StrToDateTime(Shove._Web.Utility.FilteSqlInfusion(tbBeginTime.Text), DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"));
                    DateTime dtEnd = Shove._Convert.StrToDateTime(Shove._Web.Utility.FilteSqlInfusion(tbEndTime.Text), DateTime.Now.ToString("yyyy-MM-dd")).AddDays(1);
                    dt = MSSQL.Select(sql,
                        new MSSQL.Parameter("StartDate", SqlDbType.DateTime, 0, ParameterDirection.Input, dtBegin),
                        new MSSQL.Parameter("EndDate", SqlDbType.DateTime, 0, ParameterDirection.Input, dtEnd));
                    
                }
            }
            else if (Shove._Web.Cache.GetCache("FirstQuery_" + _User.ID).ToString() == "3") //注册未充值会员
            {
                sql = @"select tb1.*,ISNULL(tb2.NickName,'') as ParentName, ISNULL(tb3.NickName,'') as GroupName from V_Users tb1 
left join T_Users tb2 on tb1.ReferId=tb2.ID left join T_Users tb3 on tb1.agentGroup=tb3.ID where  not exists ( select UserID from T_UserPayDetails PayDetails where tb1.ID=PayDetails.UserID and Result = 1 ) order by  tb1.RegisterTime desc";
                dt = MSSQL.Select(sql);



            }
            else   //所有会员
            {
                sql = "select * from V_Users where SiteID = @SiteID order by RegisterTime desc";
                dt = MSSQL.Select(sql, new MSSQL.Parameter("SiteID", SqlDbType.Int, 0, ParameterDirection.Input, _Site.ID));
            }
        }
        else  //所有会员
        {
            sql = "select * from V_Users where SiteID = @SiteID order by RegisterTime desc";
            dt = MSSQL.Select(sql, new MSSQL.Parameter("SiteID", SqlDbType.Int, 0, ParameterDirection.Input, _Site.ID));
        }

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_Users");

            return;
        }
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        //实例化分页对象
        PagedDataSource pg = new PagedDataSource();
        pg.AllowPaging = true;//设置启用分页
        pg.PageSize = pageSize;//设置每页显示数据
        pg.DataSource = ds.Tables[0].DefaultView;//分页数据源
        //设置分页索引
        pg.CurrentPageIndex = PageIndex - 1;
        //设置共总页数
        PageCount = pg.PageCount;
        DataCount = pg.DataSourceCount;

        this.rptSchemes.DataSource = pg;
        this.rptSchemes.DataBind();
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            e.Item.Cells[0].Text = "<a href='UserDetail.aspx?SiteID=" + e.Item.Cells[26].Text + "&ID=" + e.Item.Cells[24].Text + "'>" + e.Item.Cells[0].Text + "</a>";

            CheckBox cb = (CheckBox)e.Item.Cells[17].FindControl("cbisCanLogin");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[25].Text, true);

            CheckBox cbisEmailValided = (CheckBox)e.Item.Cells[4].FindControl("cbisEmailValided");
            if (cbisEmailValided != null)
            {
                cbisEmailValided.Checked = Shove._Convert.StrToBool(e.Item.Cells[27].Text, false);
            }

            CheckBox cbisMobileValided = (CheckBox)e.Item.Cells[6].FindControl("cbisMobileValided");
            if (cbisMobileValided != null)
            {
                cbisMobileValided.Checked = Shove._Convert.StrToBool(e.Item.Cells[28].Text, false);
            }

            e.Item.Cells[21].Text = e.Item.Cells[21].Text.Trim() == "2" ? "<font color='red'>高级</font>" : e.Item.Cells[21].Text.Trim() == "3" ? "<font color='blue'>VIP</font>" : "普通";

            double money = Shove._Convert.StrToDouble(e.Item.Cells[10].Text, 0);
            e.Item.Cells[10].Text = money.ToString("0.00");

            money = Shove._Convert.StrToDouble(e.Item.Cells[11].Text, 0);
            e.Item.Cells[11].Text = money.ToString("0.00");
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        if (tbUserName.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入用户名称");
            DataBind();

        }

        DataTable dt = null;
        string sql = "";
        sql = @"select tb1.*,ISNULL(tb2.NickName,'') as ParentName, ISNULL(tb3.NickName,'') as GroupName from V_Users tb1 
            left join T_Users tb2 on tb1.ReferId=tb2.ID left join T_Users tb3 on tb1.agentGroup=tb3.ID 
            where  tb1.name like '%" + tbUserName.Text.Trim() + "%' order by  tb1.RegisterTime desc";
        
        dt = MSSQL.Select(sql);
        this.rptSchemes.DataSource = dt;
        this.rptSchemes.DataBind();

    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        DataTable dt = new DAL.Views.V_Users().Open("Name AS '用户名',NickName AS '真实姓名',Sex AS '性别',BirthDay AS '生日',Email AS '邮箱',Mobile AS '手机',RegisterTime AS '注册时间',Province AS '省份',City AS '城市',BankCardNumber AS '银行卡号'", "", "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

            return;
        }

        string FileName = "T_Users.xls";

        HttpResponse response = Page.Response;

        response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
        Response.ContentType = "application/ms-excel";
        response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");

        foreach (DataColumn dc in dt.Columns)
        {
            response.Write(dc.ColumnName + "\t");
        }

        response.Write("\n");

        foreach (DataRow dr in dt.Rows)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                response.Write(dr[i].ToString() + "\t");
            }

            response.Write("\n");
        }

        response.End();
    }
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        Shove._Web.Cache.ClearCache("FirstQuery_" + _User.ID);
        BindData();
    }
    protected void btnSearchByRegDate_Click(object sender, EventArgs e)
    {
        DateTime BeginTime = Shove._Convert.StrToDateTime(this.tbBeginTime.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        DateTime EndTime = Shove._Convert.StrToDateTime(this.tbEndTime.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        TimeSpan ts = EndTime.Subtract(BeginTime);
        int DifferTime = Shove._Convert.StrToInt(ts.Days.ToString(), 0);
        if (DifferTime < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "开始时间不能大于结束时间！");
            return;
        }
        Shove._Web.Cache.SetCache("FirstQuery_" + _User.ID, "2");

        string sql = "";

        sql = @"select tb1.*,ISNULL(tb2.NickName,'') as ParentName, ISNULL(tb3.NickName,'') as GroupName from V_Users tb1 
            left join T_Users tb2 on tb1.ReferId=tb2.ID left join T_Users tb3 on tb1.agentGroup=tb3.ID 
            where   tb1.RegisterTime>'" + this.tbBeginTime.Text + "' and tb1.RegisterTime<'" + this.tbEndTime.Text + "' order by  tb1.RegisterTime desc";


        DataTable dt = null;
        dt = MSSQL.Select(sql);
        BindData();
        /*
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        //实例化分页对象
        PagedDataSource pg = new PagedDataSource();
        pg.AllowPaging = true;//设置启用分页
        pg.PageSize = pageSize;//设置每页显示数据
        pg.DataSource = ds.Tables[0].DefaultView;//分页数据源
        //设置分页索引
        pg.CurrentPageIndex = PageIndex - 1;
        //设置共总页数
        PageCount = pg.PageCount;
        DataCount = pg.DataSourceCount;

        this.rptSchemes.DataSource = pg;
        this.rptSchemes.DataBind();
        */
    }

    protected void btnSearchNoPay_Click(object sender, EventArgs e)
    {

        Shove._Web.Cache.SetCache("FirstQuery_" + _User.ID, "3");
        if (tbBeginTime.Text.Trim() != "" && tbEndTime.Text.Trim() != "")
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!DateTime.TryParse(tbBeginTime.Text, out fromDate) && !DateTime.TryParse(tbEndTime.Text, out toDate))
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入正确的日期格式:2008-8-8");
                return;
            }
        }
        BindData();
    }

    protected void ShoveConfirmButton3_Click(object sender, EventArgs e)
    {
        if (tbMobile.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入QQ号码");
            DataBind();

        }

        DataTable dt = null;
        string sql = "";
        sql = @"select tb1.*,ISNULL(tb2.NickName,'') as ParentName, ISNULL(tb3.NickName,'') as GroupName from V_Users tb1 
            left join T_Users tb2 on tb1.ReferId=tb2.ID left join T_Users tb3 on tb1.agentGroup=tb3.ID 
            where  tb1.[NickName] like '%" + tbMobile.Text.Trim() + "%' order by  tb1.RegisterTime desc";
        
        dt = MSSQL.Select(sql);
        this.rptSchemes.DataSource = dt;
        this.rptSchemes.DataBind();
    }
    public string TransStr(string fromClient)
    {
        switch (fromClient)
        {
            case "1":
                return "网页版";
            case "2":
                return "安卓手机";
            case "3":
                return "苹果手机";
            case "4":
                return "手机站点";
            default:
                return "其它";
        }
    }
    protected void ShoveConfirmButton4_Click(object sender, EventArgs e)
    {
        if (tbDaili.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入上级代理账号");
            DataBind();

        }

        DataTable dt = null;
        string sql = "";

        sql = @"select tb1.*,ISNULL(tb2.NickName,'') as ParentName, ISNULL(tb3.NickName,'') as GroupName from V_Users tb1 
            left join T_Users tb2 on tb1.ReferId=tb2.ID left join T_Users tb3 on tb1.agentGroup=tb3.ID 
            where  (tb2.[NickName] like '%" + tbDaili.Text.Trim() + "%' or tb2.[Name] like '%" + tbDaili.Text.Trim() + "%') order by  tb1.RegisterTime desc";

        dt = MSSQL.Select(sql);
        this.rptSchemes.DataSource = dt;
        this.rptSchemes.DataBind();
    }

    public string getAgentRank(string rankID)
    {
        if (rankID.Equals("2"))
        {
            return "<font color=#11cc00>代理组管理</font>";
        }
        else if (rankID.Equals("1"))
        {
            return "<font color=#ff0000>代理会员</font>";
        }
        else
        {
            return "普通会员";
        }
    }
}
