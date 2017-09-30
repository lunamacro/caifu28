<%@ WebHandler Language="C#" Class="upload" %>

using System;
using System.Web;

public class upload : IHttpHandler {

    long timeStamp = 0;
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        if (context.Request.Files.Count == 0)
        {
            context.Response.Write("fail");
            return;
        }

        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        timeStamp = (long)(DateTime.Now - startTime).TotalSeconds; // 相差秒数
        
        string uid = context.Request.Form["uid"];
        string host = HttpContext.Current.Request.Url.Host.ToString();
        string imageName = "icon_"+timeStamp.ToString()+".jpg";
        string uploadPath = "/Uploadfile/userIcon/" + imageName;
        string imageUrl = "http://"+host + uploadPath;

        Users _user = new Users(1)[1, int.Parse(uid)];
        if (_user == null)
        {
            context.Response.Write("fail");
            return;
        }
        string des = "";
        _user.HeadUrl = imageUrl;
        int result = _user.EditByID(ref des);
        if (result < 0)
        {
            context.Response.Write("fail");
            return;
        }

        HttpPostedFile f1 = context.Request.Files[0];
        System.Drawing.Image image = System.Drawing.Image.FromStream(f1.InputStream);
        string imageParh = HttpContext.Current.Server.MapPath(uploadPath);
        image.Save(imageParh);
        image.Dispose();
        context.Response.Write(imageUrl);  
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}