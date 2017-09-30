<%@ WebHandler Language="C#" Class="UploadFile" %>

using System;
using System.Web;
using System.IO;

public class UploadFile : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        HttpPostedFile fs = context.Request.Files["newFile"];
        string uploadType = context.Request.Form["uploadType"];
        string fileName = "";
        string error = "0";
        if (fs.ContentLength > 0)
        {
            try
            {
                Stream imageStream = fs.InputStream;
                System.Drawing.Image.FromStream(imageStream);
            }
            catch (Exception)
            {
                error = "-1";
                context.Response.Redirect("../" + uploadType + "PicManage.aspx?filename=" + fileName + "&error=" + error);
            }
            string hz = fs.ContentType.Split('/')[1];
            if (hz == "jpeg" || hz == "png" || hz == "jpg" || hz == "gif" || hz == "bmp")
            {
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString() + "." + hz;
                string path = AppDomain.CurrentDomain.BaseDirectory + "/Uploadfile/" + uploadType + "Pic/" + fileName;
                fs.SaveAs(path);
                if (!PF.IsSafe(path))
                {
                    error = "-1";
                }
            }
            else
            {
                error = "-1";
            }
        }
        context.Response.Redirect("../" + uploadType + "PicManage.aspx?filename=" + fileName + "&error=" + error);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}