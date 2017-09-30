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
using System.Linq;

public partial class Admin_UserAccountDetail : AdminPageBase
{
    public int pageSize = 15;
    public int PageIndex = 1;
    public long PageCount = 0;//总页数
    public long DataCount = 0;//总数据

    private DataSet ds = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        //获得当前页码
        PageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("index"), 1);

        if (!string.IsNullOrEmpty(Shove._Web.Utility.GetRequest("UserName")))
            this.tbUserName.Text = Shove._Web.Utility.GetRequest("UserName");
        if (!string.IsNullOrEmpty(Shove._Web.Utility.GetRequest("ID")))
            this.tbID.Text = Shove._Web.Utility.GetRequest("ID");
        if (!this.IsPostBack)
        {
            BindDataForYearMonth();
            BinDataForDay();
        }
        BindData();
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.UserAccountDetails);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForYearMonth()
    {
        ddlYear.Items.Clear();

        DateTime dt = DateTime.Now;
        int Year = dt.Year;
        int Month = dt.Month;

        if (Year < PF.SystemStartYear)
        {
            btnRead.Enabled = false;

            return;
        }

        for (int i = PF.SystemStartYear; i <= Year; i++)
        {
            ddlYear.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear.SelectedIndex = ddlYear.Items.Count - 1;

        ddlMonth.SelectedIndex = Month - 1;
    }

    private void BinDataForDay()
    {
        ddlDay.Items.Clear();

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
        int i = int.Parse(ddlMonth.SelectedValue);
        int MaxDay = 0;

        foreach (int j in Month)
        {
            if (i == j)
            {
                MaxDay = 31;
                break;
            }
            else if (i == 2)
            {
                if (((Year % 4) == 0) && ((Year % 100) != 0) && ((Year % 400) == 0))
                {
                    MaxDay = 29;
                    break;
                }
                else
                {
                    MaxDay = 28;
                    break;
                }
            }
            else
            {
                MaxDay = 30;
            }
        }

        for (int j = 1; j <= MaxDay; j++)
        {
            ddlDay.Items.Add(new ListItem(j.ToString() + "日", j.ToString()));
        }

        if (Day > MaxDay)
        {
            Day = MaxDay;
        }

        ddlDay.SelectedIndex = Day - 1;
    }

    private void BindData()
    {
        if (ddlYear.Items.Count == 0)
        {
            return;
        }

        if (tbID.Text == "")
        {
            tbID.Text = Users.GetCurrentUser(1).ID.ToString();

            if (tbID.Text == "")
            {
                return;
            }
        }

        long UserID = Shove._Convert.StrToLong(tbID.Text, -1);

        if (UserID < 0)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_UserAccountDetail");

            return;
        }

        if (tbUserName.Text.Trim() == "")
        {
            Users tu = new Users(_Site.ID)[_Site.ID, UserID];

            if (tu == null)
            {
                Shove._Web.JavaScript.Alert(this.Page, "用户名不存在。");

                return;
            }

            tbUserName.Text = tu.Name;
        }

        DateTime dtStart;
        DateTime dtEnd;
        //string  st=ddlYear.SelectedValue.ToString() + "-" + ddlMonth.SelectedValue.ToString();
        //dtStart = Shove._Convert.StrToDateTime(st, "");
        //dtEnd = (Shove._Convert.StrToDateTime(st, "")).AddMonths(1);
        int ReturnValue = -1;
        string ReturnDescription = "";

        //ReturnValue = DAL.Procedures.P_GetUserAccountDetail(ref ds, _Site.ID, UserID, int.Parse(ddlYear.SelectedValue), int.Parse(ddlMonth.SelectedValue), int.Parse(ddlDay.SelectedValue), ref ReturnValue, ref ReturnDescription);
        if (this.tbStartTime.Text.Equals(""))
        {
            dtStart = Shove._Convert.StrToDateTime(DateTime.Now.ToShortTimeString(), "").AddMonths(-1);
        }
        else
        {
            dtStart = Shove._Convert.StrToDateTime(this.tbStartTime.Text, "");
        }
        if (this.tbEndTime.Text.Equals(""))
        {
            dtEnd = Shove._Convert.StrToDateTime(DateTime.Now.ToShortTimeString(), "");
        }
        else
        {
            dtEnd = Shove._Convert.StrToDateTime(this.tbEndTime.Text, "");
        }
        ds = null;
        DAL.Procedures.P_GetUserAccountDetails(ref ds, 1, UserID, dtStart, dtEnd, ref ReturnValue, ref ReturnDescription);

        if (ReturnValue < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }




        if ((ds == null) || (ds.Tables.Count < 1))
        {

            return;
        }

        DataTable dts = null;

        dts = ds.Tables[0];

        Shove._Web.Cache.SetCache(Shove._Web.WebConfig.GetAppSettingsString("SystemPreFix") + _Site.ID.ToString() + "AccountDetail_" + _User.ID.ToString(), dts);

        DataTable newDatable = dts.Clone();
        if (selMemo.Value != "0")
        {
            //Condition += " and OperatorType in '(" + selMemo.Value + ")'";
            //if (selMemo.Value == "保底冻结" || selMemo.Value == "追号冻结" || selMemo.Value == "提款冻结")
            //    Condition += " and Memo like '%" + selMemo.Value + "%' and Memo not like '%" + selMemo.Value + "解除%'";
            var newData = dts.AsEnumerable().Where(a => selMemo.Value.Contains("," + a["OperatorType"].ToString() + ","));
            newDatable = newData.AsDataView().ToTable().Copy();
        }
        else
        {
            newDatable = dts.Copy();
        }
        //DataTable dtData = dts.Clone();

        //foreach (DataRow dr in dts.Select(Condition, "DateTime desc"))
        //{
        //    dtData.Rows.Add(dr.ItemArray);
        //}

        //实例化分页对象
        PagedDataSource pg = new PagedDataSource();
        pg.AllowPaging = true;//设置启用分页
        pg.PageSize = pageSize;//设置每页显示数据
        pg.DataSource = newDatable.DefaultView;//分页数据源
        //设置分页索引
        pg.CurrentPageIndex = PageIndex - 1;
        //设置共总页数
        PageCount = pg.PageCount;
        DataCount = pg.DataSourceCount;
        this.rptSchemes.DataSource = pg;
        this.rptSchemes.DataBind();
        //PF.DataGridBindData(g, ds.Tables[0], gPager);
    }



    protected void rptSchemes_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label shcid = e.Item.FindControl("schid") as Label;

        Label idd = e.Item.FindControl("ids") as Label;

        Label id = e.Item.FindControl("id") as Label;

        if (Shove._Convert.StrToInt(shcid.Text.ToString(), 0) > 0)
        {

            id.Visible = false;
            idd.Visible = true;
            idd.Text = "<a href='Scheme.aspx?ID=" + shcid.Text.ToString() + "' runat=\"server\" id=\"idd\">" + id.Text.ToString() + "</a> ";

        }


    }


    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            double money;

            money = Shove._Convert.StrToDouble((e.Item.Cells[2].FindControl("lblIn") as Label).Text, 0);
            e.Item.Cells[2].Text = (money == 0) ? "" : money.ToString("0.00");

            money = Shove._Convert.StrToDouble((e.Item.Cells[3].FindControl("Label2") as Label).Text, 0);
            e.Item.Cells[3].Text = (money == 0) ? "" : money.ToString("0.00");

            money = Shove._Convert.StrToDouble((e.Item.Cells[4].FindControl("Label3") as Label).Text, 0);
            e.Item.Cells[4].Text = (money == 0) ? "" : money.ToString("0.00");

            money = Shove._Convert.StrToDouble((e.Item.Cells[5].FindControl("Label4") as Label).Text, 0);
            e.Item.Cells[5].Text = (money == 0) ? "" : money.ToString("0.00");

            money = Shove._Convert.StrToDouble((e.Item.Cells[6].FindControl("Label5") as Label).Text, 0);
            e.Item.Cells[6].Text = (money == 0) ? "" : money.ToString("0.00");

            money = Shove._Convert.StrToDouble((e.Item.Cells[7].FindControl("Label6") as Label).Text, 0);
            e.Item.Cells[7].Text = (money == 0) ? "" : money.ToString("0.00");

            long SchemeID = Shove._Convert.StrToLong((e.Item.Cells[8].FindControl("Label7") as Label).Text, -1);

            if (SchemeID >= 0)
            {
                (e.Item.Cells[1].FindControl("lblMemo") as Label).Text = "<a href='../Home/Room/Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'><font color=\"#330099\">" + (e.Item.Cells[1].FindControl("lblMemo") as Label).Text + "</Font></a>";
            }
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            e.Item.Cells[0].ColumnSpan = 2;
            e.Item.Cells.RemoveAt(8);
            e.Item.Cells[0].Text = "合计";
            //e.Item.Cells[1].Text = PF.GetSumByColumn(ds.Tables[0], 3, false, 30, gPager.PageIndex).ToString("0.00");
            //e.Item.Cells[2].Text = PF.GetSumByColumn(ds.Tables[0], 4, false, 30, gPager.PageIndex).ToString("0.00");
            //e.Item.Cells[3].Text = PF.GetSumByColumn(ds.Tables[0], 5, false, 30, gPager.PageIndex).ToString("0.00");
            //e.Item.Cells[5].Text = PF.GetSumByColumn(ds.Tables[0], 7, false, 30, gPager.PageIndex).ToString("0.00");
            e.Item.Cells[7].Visible = false;
            //e.Item.Cells[6].Text = PF.GetSumByColumn(ds.Tables[0], 7, false, 30, gPager.PageIndex).ToString();
        }
    }

    protected void btnRead_Click(object sender, System.EventArgs e)
    {
        if (tbUserName.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入用户名。");

            return;
        }

        Users tu = new Users(_Site.ID)[_Site.ID, tbUserName.Text.Trim()];

        if (tu == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "用户名不存在。");

            return;
        }
        if (!this.tbStartTime.Text.Equals("") && !this.tbEndTime.Text.Equals(""))
        {
            if (DateTime.Parse(this.tbStartTime.Text) >= DateTime.Parse(this.tbEndTime.Text))
            {
                Shove._Web.JavaScript.Alert(this.Page, "截止时间不能小于开始时间。");

                return;
            }
        }
        tbID.Text = tu.ID.ToString();

        BindData();
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        BinDataForDay();
    }
    public string ReWriteStr(string str)
    {
        if (str == "非数字" || str == "0.00")
        {
            return "";
        }
        else
        {
            return str;
        }
    }
    public string Calculation(object one, object two)
    {
        if (two.ToString() == "")
        {
            return "";
        }
        double temp = Convert.ToDouble(one) / Convert.ToDouble(two);
        if (temp.ToString("0.00") == "0.00" || temp.ToString("0.00") == "非数字")
        {
            return "";
        }
        return temp.ToString("0.00");
    }
    ////首页
    //protected void LinkButton1_Click(object sender, EventArgs e)
    //{
    //    this.lbl_this.Text = "1";
    //    BindData();
    //}
    ////上一页
    //protected void LinkButton2_Click(object sender, EventArgs e)
    //{
    //    if (this.lbl_this.Text == "1")
    //    {
    //        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('当前已经是第一页')", true);
    //        BindData();
    //        return;
    //    }
    //    this.lbl_this.Text = (int.Parse(this.lbl_this.Text) - 1).ToString();
    //    BindData();
    //}
    ////下一页
    //protected void LinkButton3_Click(object sender, EventArgs e)
    //{
    //    if (this.lbl_this.Text == this.lbl_sum.Text)
    //    {
    //        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('当前已经是最后一页')", true);
    //        BindData();
    //        return;
    //    }
    //    this.lbl_this.Text = (int.Parse(this.lbl_this.Text) + 1).ToString();
    //    BindData();
    //}
    ////尾页
    //protected void LinkButton4_Click(object sender, EventArgs e)
    //{
    //    this.lbl_this.Text = this.lbl_sum.Text;
    //    BindData();
    //}
}
