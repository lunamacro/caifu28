using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Shove;
using System.Data;

public partial class Admin_AppPicManage : AdminPageBase
{
    string path = AppDomain.CurrentDomain.BaseDirectory + "/Uploadfile/AppPic";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string fileName = Request.QueryString["filename"];
            string delFile = Request.QueryString["delfile"];
            string error = Request.QueryString["error"];
            if (error == "-1")
            {
                Shove._Web.JavaScript.Alert(this.Page, "上传失败,请上传图片文件");
            }
            if (fileName != null && fileName != "")
            {
                Shove._Web.JavaScript.Alert(this.Page, "上传成功");
            }
            if (delFile != null && delFile != "")
            {
                File.Delete(path + "/" + delFile);
                Shove._Web.JavaScript.Alert(this.Page, "删除成功");
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FillData();
        }
    }


    /// <summary>
    /// 用于验证是否是管理员
    /// </summary>
    /// <param name="e"></param>
    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.AppImage);

        base.OnInit(e);
    }
    private void FillData()
    {
        string[] fileArray = Directory.GetFiles(path);
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("FileName");
        dt.Columns.Add("FileSize");
        dt.Columns.Add("CurrentUrl");
        for (int i = 0; i < fileArray.Length; i++)
        {
            FileInfo fileInfo = new FileInfo(fileArray[i]);
            DataRow dr = dt.NewRow();
            dr["ID"] = i;
            dr["FileName"] = fileInfo.Name;
            dr["FileSize"] = fileInfo.Length / 1024;
            dr["CurrentUrl"] = "http://" + Request.Url.Authority + "/Uploadfile/AppPic/" + fileInfo.Name;
            dt.Rows.Add(dr.ItemArray);
        }
        rptSchemes.DataSource = dt;
        rptSchemes.DataBind();
    }
}