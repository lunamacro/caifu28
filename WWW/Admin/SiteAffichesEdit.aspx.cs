﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

public partial class Admin_SiteAffichesEdit : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();
        }
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
        long SiteAfficheID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);

        if (SiteAfficheID < 0)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_SiteAffichesEdit");

            return;
        }

        tbID.Text = SiteAfficheID.ToString();

        DataTable dt = new DAL.Tables.T_SiteAffiches().Open("", "SiteID = " + _Site.ID.ToString() + " and [ID] = " + SiteAfficheID.ToString(), "isCommend desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SiteAffichesEdit");

            return;
        }

        tbDateTime.Text = dt.Rows[0]["DateTime"].ToString();
        cbisShow.Checked = Shove._Convert.StrToBool(dt.Rows[0]["isShow"].ToString(), true);
        cbisCommend.Checked = Shove._Convert.StrToBool(dt.Rows[0]["isCommend"].ToString(), true);
        tbTitle.Text = dt.Rows[0]["Title"].ToString();

        if (tbTitle.Text.IndexOf("</font>") > -1)
        {
            cbTitleRed.Checked = true;
            tbTitle.Text = tbTitle.Text.Replace("<font color=\"red\">", "").Replace("</font>","");
        }
        else
        {
            cbTitleRed.Checked = false;
        }

        string UC = dt.Rows[0]["Content"].ToString();

        Regex regex = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Match m = regex.Match(UC);

        if (bool.Parse(dt.Rows[0]["isUrlType"].ToString()))
        {
            trContent.Visible = false;
            trUrl.Visible = true;
            trUrlOK.Visible = true;

            tbUrl.Text = dt.Rows[0]["UrlContent"].ToString();
            rblType.SelectedValue = "1";

        }
        else
        {
            trContent.Visible = true;
            trUrl.Visible = false;
            trUrlOK.Visible = false;

            tbContent.Value = UC;
            rblType.SelectedValue = "2";
        }
    }

    protected void btnSave_Click(object sender, System.EventArgs e)
    {
        DateTime dt = System.DateTime.Now;

        try
        {
            dt = System.DateTime.Parse(tbDateTime.Text);
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "时间格式错误，请输入如“" + dt.ToString() + "”的时间格式。");

            return;
        }

        string Title = tbTitle.Text.Trim();

        if (Title == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入标题。");

            return;
        }

        int ReturnValue = -1;
        string ReturnDescription = "";
        string UC = tbContent.Value.Trim();
        string Url = "";
        bool isUrl = false;

        if (rblType.SelectedValue == "1")
        {
            Url = tbUrl.Text.Trim();
            isUrl = true;
            UC = "";
            Regex regex = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = regex.Match(Url);

            if (!m.Success || (Url.IndexOf("http") != 0))
            {
                Shove._Web.JavaScript.Alert(this, "地址格式错误，请仔细检查。");

                return;
            }
        }
        else
        {
            UC = tbContent.Value.Trim();

        }

        if (cbTitleRed.Checked)
        {
            Title = "<font color=\"red\">" + Title + "</font>";
        }

        if (Shove._String.GetLength(Title) > 100)
        {
            Shove._Web.JavaScript.Alert(this.Page, "标题长度超过限制！");

            return;
        }

        int Results = -1;
        Results = DAL.Procedures.P_SiteAfficheEdit(_Site.ID, long.Parse(tbID.Text), dt, Title, UC, Url, isUrl, cbisShow.Checked, cbisCommend.Checked, ref ReturnValue, ref ReturnDescription);
        if (Results < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SiteAffichesEdit");

            return;
        }

        if (ReturnValue < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }
        Shove._Web.Cache.ClearCache(CacheKey.SiteAffiches);
        Shove._Web.Cache.ClearCache("Default_GetSiteAffiches");

        this.Response.Redirect("SiteAffiches.aspx", true);
       
    }

    protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblType.SelectedValue == "1")
        {
            trUrl.Visible = true;
            trUrlOK.Visible = true;
            trContent.Visible = false;
        }
        else
        {
            trUrl.Visible = false;
            trUrlOK.Visible = false;
            trContent.Visible = true;
        }
    }
}
