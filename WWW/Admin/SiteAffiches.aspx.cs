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
public partial class Admin_SiteAffiches : AdminPageBase
{
    public int PageIndex;//当前页
    public int PageCount;//总页数
    public int DataCount;//总数据
    protected void Page_Load(object sender, EventArgs e)
    {
        BindData();
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.SiteNotice);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        int intResult = -1;
        int TotalRowCount = 0;
        int PageCountt = 0;
        PageIndex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("pageindex"), 1);
        DataSet ds = null;
        intResult = DAL.Procedures.P_Pager(ref ds, PageIndex, 10, 0, "*",
             "T_SiteAffiches", "DateTime desc", " SiteID = " + _Site.ID.ToString() + "", "[DateTime] desc", ref TotalRowCount, ref PageCountt);

        DataTable dp = null;
        if (ds.Tables.Count > 0)
        {

            dp = ds.Tables[0];
        }

        DataTable dt = MSSQL.Select("select count(*) as coun from T_SiteAffiches ");
        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        DataCount = Shove._Convert.StrToInt(dt.Rows[0]["coun"].ToString(), 0);

        int pagsizeq = DataCount % 10;

        if (pagsizeq == 0)
        {

            PageCount = DataCount / 10;

        }
        else
        {
            PageCount = (DataCount / 10) + 1;
        }


        this.rptSchemes.DataSource = dp;
        this.rptSchemes.DataBind();

    }

    protected void rptSchemes_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "edit")
        {

            this.Response.Redirect("SiteAffichesEdit.aspx?id=" + e.CommandArgument, true);
            return;
        }
        if (e.CommandName == "Del")
        {

            int ReturnValue = -1;
            string ReturnDescription = "";
            int Results = -1;
            Results = DAL.Procedures.P_SiteAfficheDelete(_Site.ID, long.Parse(e.CommandArgument.ToString()), ref ReturnValue, ref ReturnDescription);

            if (Results < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SiteAffiches");

                return;
            }

            if (ReturnValue < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_SiteAffiches");

                return;
            }
            Shove._Web.Cache.ClearCache(CacheKey.SiteAffiches);
            Shove._Web.Cache.ClearCache("Default_GetSiteAffiches");


            BindData();

            return;

        }



    }

    //protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    //{
    //    if (e.CommandName == "Edit")
    //    {
    //        this.Response.Redirect("SiteAffichesEdit.aspx?id=" + e.Item.Cells[5].Text, true);

    //        return;
    //    }

    //    if (e.CommandName == "Del")
    //    {
    //        int ReturnValue = -1;
    //        string ReturnDescription = "";
    //        int Results = -1;
    //        Results = DAL.Procedures.P_SiteAfficheDelete(_Site.ID, long.Parse(e.Item.Cells[5].Text), ref ReturnValue, ref ReturnDescription);

    //        if (Results < 0)
    //        {
    //            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SiteAffiches");

    //            return;
    //        }

    //        if (ReturnValue < 0)
    //        {
    //            PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_SiteAffiches");

    //            return;
    //        }
    //        Shove._Web.Cache.ClearCache(CacheKey.SiteAffiches);
    //        Shove._Web.Cache.ClearCache("Default_GetSiteAffiches");

    //        BindData();

    //        return;
    //    }
    //}

    protected void btn_Click(object sender, System.EventArgs e)
    {
        this.Response.Redirect("SiteAffichesAdd.aspx", true);
    }

    //protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    //{
    //    BindData();
    //}

    //protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    //{
    //    BindData();
    //}
}
