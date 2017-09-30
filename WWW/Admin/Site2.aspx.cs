using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using System.Web.Script.Serialization;
using System.Text;

public partial class Admin_Site2 : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Admin_Site2), this.Page);

            BindData();

            InitQRCode();
        }
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public void SaveQRCodeImage(string qrValue)
    {
		txt_siteName.Text="finish";
		
        try
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            QRCode entity = js.Deserialize<QRCode>(qrValue);
            if (entity != null)
            {
                Shove._IO.IniFile ini = new Shove._IO.IniFile(System.AppDomain.CurrentDomain.BaseDirectory + "Admin/QRCode/Config.ini");
                ini.Write(entity.Option, "Size", entity.Size);
                ini.Write(entity.Option, "BGColor", entity.BGColor);
                ini.Write(entity.Option, "QRCodeColor", entity.QRCodeColor);
                ini.Write(entity.Option, "Url", entity.Url);
                ini.Write(entity.Option, "QRCodeType", entity.QRCodeType);
                ini.Write(entity.Option, "ImageSrc", entity.ImageSrc);
                ini.Write(entity.Option, "QRCodeImg", entity.QRCodeImg);
                ini.Write(entity.Option, "FontContent", entity.FontContent);
                ini.Write(entity.Option, "FontFamily", entity.FontFamily);
                ini.Write(entity.Option, "FontColor", entity.FontColor);
                ini.Write(entity.Option, "FontSize", entity.FontSize);
            }
        }
        catch (Exception ex)
        {
        }
    }

    /// <summary>
    /// 加载二维码配置
    /// </summary>
    private void InitQRCode()
    {
        try
        {
            Shove._IO.IniFile ini = new Shove._IO.IniFile(System.AppDomain.CurrentDomain.BaseDirectory + "Admin/QRCode/Config.ini");
            hf_weixin.Value = StrToJson(ini, "WeiXin");
            //img_weixin.ImageUrl = ini.Read("WeiXin", "QRCodeImg");
			img_weixin.ImageUrl = System.AppDomain.CurrentDomain.BaseDirectory + "Admin/QRCode/qrcode.png";
			

            hf_h5.Value = StrToJson(ini, "H5");
            h5_img.ImageUrl = ini.Read("H5", "QRCodeImg");

            hf_app.Value = StrToJson(ini, "APP");
            app_img.ImageUrl = ini.Read("APP", "QRCodeImg");
        }
        catch (Exception)
        {

        }
    }

    private string StrToJson(Shove._IO.IniFile ini, string option)
    {
        StringBuilder strQRValue = new StringBuilder();
        QRCode qrCode = new QRCode();
        qrCode.Option = option;
        qrCode.Size = ini.Read(option, "Size");
        qrCode.BGColor = ini.Read(option, "BGColor");
        qrCode.QRCodeColor = ini.Read(option, "QRCodeColor");
        qrCode.Url = ini.Read(option, "Url");
        qrCode.QRCodeType = ini.Read(option, "QRCodeType");
        qrCode.ImageSrc = ini.Read(option, "ImageSrc");
        qrCode.QRCodeImg = ini.Read(option, "QRCodeImg");
        qrCode.FontContent = ini.Read(option, "FontContent");
        qrCode.FontFamily = ini.Read(option, "FontFamily");
        qrCode.FontColor = ini.Read(option, "FontColor");
        qrCode.FontSize = ini.Read(option, "FontSize");

        JavaScriptSerializer js = new JavaScriptSerializer();

        return js.Serialize(qrCode);
    }

    #region 页面加载绑定数据
    /// <summary>
    /// 页面加载绑定数据
    /// </summary>
    public void BindData()
    {
        txt_siteName.Text = _Site.Name;
        txt_company.Text = _Site.Company;
        txt_address.Text = _Site.Address;
        txt_post.Text = _Site.PostCode;
        txt_mobile.Text = _Site.Mobile;
        txt_email.Text = _Site.Email;
        txt_QQ.Text = _Site.QQ;
        txt_serverMobile.Text = _Site.ServiceTelephone;
        txt_ICP.Text = _Site.ICPCert;
        txt_siteURL.Text = _Site.Url;

        



        string[] lotteryList = _Site.UseLotteryList.Split(',');
        int len = lotteryList.Length;
        if (null != lotteryList || len > 0)
        {
            CheckBox chk = null;
            for (int i = 0; i < len; i++)
            {
                switch (lotteryList[i])
                {
                    case "72":  //竞彩足球
                        chk_jczq.Checked = true;
                        chk = chk_jczq;
                        break;
                    case "73":  //竞彩篮球
                        chk_jclq.Checked = true;
                        chk = chk_jclq;
                        break;

                    case "74":  //胜负彩
                        chk_sfc.Checked = true;
                        chk = chk_sfc;
                        break;
                    case "75":  //任九场
                        chk_rjc.Checked = true;
                        chk = chk_rjc;
                        break;
                   
                    case "45":  //北京单场
                        chk_zqdc.Checked = true;
                        chk = chk_zqdc;
                        break;

                    
                    case "5":   //双色球
                        chk_ssq.Checked = true;
                        chk = chk_ssq;
                        break;
                   
                    case "39":   //超级大乐透
                        chk_cjdlt.Checked = true;
                        chk = chk_cjdlt;
                        break;

                    case "28":   //重庆时时彩3D
                        chk_ssc.Checked = true;
                        chk = chk_ssc;
                        break;
                    case "66":   //新疆时时彩
                        chk_xjssc.Checked = true;
                        chk = chk_xjssc;
                        break;
                    case "62":   //十一运夺金
                        chk_syydj.Checked = true;
                        chk = chk_syydj;
                        break;
                   
                    case "78":   //广东11选5
                        chk_gd11x5.Checked = true;
                        chk = chk_gd11x5;
                        break;
                    case "83":   //江苏快3
                        chk_jsk3.Checked = true;
                        chk = chk_jsk3;
                        break;

                    case "94":   //北京PK10
                        chk_bjpk10.Checked = true;
                        chk = chk_bjpk10;
                        break;

                    case "98":   //加拿大28
                        chk_jndxy28.Checked = true;
                        chk = chk_jndxy28;
                        break;
                    case "99":   //北京28
                        chk_bjxy28.Checked = true;
                        chk = chk_bjxy28;
                        break;
                    case "100":   //腾讯分分彩
                        chk_txffc.Checked = true;
                        chk = chk_txffc;
                        break;
                }
                if (null != chk)
                {
                    //chk.Attributes.Add("lotID", lotteryList[i]);
                }
            }
        }

    }
    #endregion

    #region 获得用户启用的彩种列表
    /// <summary>
    /// 获得用户启用的彩种列表
    /// </summary>
    /// <returns></returns>
    private string GetUseLotteryList()
    {
        string UseLotteryList = "";
        ControlCollection controls = useLotteryList.Controls;
        foreach (Control c in controls)
        {
            if (c is CheckBox)
            {
                CheckBox chk = c as CheckBox;
                if (chk.Checked)
                {
                    UseLotteryList += chk.Attributes["lotID"] + ",";
                }
            }
        }
        return UseLotteryList.EndsWith(",") ? UseLotteryList.Substring(0, UseLotteryList.Length - 1) : UseLotteryList;
    }
    #endregion


    /// <summary>
    /// 修改按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_update_Click(object sender, EventArgs e)
    {
        this.UpdateSiteInfo();
    }


    protected void btn_upload_Click(object sender, EventArgs e)
    {
        //上传图片
        if (wxQrcode.PostedFile.FileName != "")
        {
            if (wxQrcode.HasFile) //文件存在
            {
                SaveFile(wxQrcode.PostedFile);//保存上传文件
                ClearClientPageCache();
                Response.Write("<script>alert('文件上传成功，清理浏览器缓存后刷新本页面以查看效果！')</script>");
            }
            else
            {
                Response.Write("<script>alert('上传文件不存在！')</script>");
            }
        }
    }

    public void SaveFile(HttpPostedFile file)
    {
        string savePath = HttpContext.Current.Server.MapPath("/Images/qrcode.png");
        //string savePath = "C:\\Users\\DJJ\\Desktop\\School_Canteen2\\Web\\images\\";
        //string fileName = file.FileName;

        //string pathToCheck = savePath + fileName;
        //string tempfilename = "";
        if (System.IO.File.Exists(savePath))
        {
            System.IO.File.Delete(savePath);
        }
        //savePath += fileName;
        file.SaveAs(savePath);
    }

    public void ClearClientPageCache() { 
        HttpContext.Current.Response.Buffer = true; 
        HttpContext.Current.Response.Expires = 0; 
        HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1); 
        HttpContext.Current.Response.AddHeader("pragma", "no-cache"); 
        HttpContext.Current.Response.AddHeader("cache-control", "private"); 
        HttpContext.Current.Response.CacheControl = "no-cache"; 
    } 


    /// <summary>
    /// 检查站点输入是否完整
    /// </summary>
    /// <returns></returns>
    public void UpdateSiteInfo()
    {
        string siteName = txt_siteName.Text.Trim();
        string company = txt_company.Text.Trim();
        string address = txt_address.Text.Trim();
        string post = txt_post.Text.Trim();
        string mobile = txt_mobile.Text.Trim();
        string email = txt_email.Text.Trim();
        string QQ = txt_QQ.Text.Trim();
        string serverMobile = txt_serverMobile.Text.Trim();
        string ICP = txt_ICP.Text.Trim();
        string siteURL = txt_siteURL.Text.Trim();
        



        Regex reg = null;
        if ("" == siteName)
        {
            Shove._Web.JavaScript.Alert(this, "站点名称不能为空。");
            return;
        }
        if ("" == company)
        {
            Shove._Web.JavaScript.Alert(this, "公司名称不能为空。");
            return;
        }
        if ("" == address)
        {
            Shove._Web.JavaScript.Alert(this, "公司地址不能为空。");
            return;
        }
        if ("" == post)
        {
            Shove._Web.JavaScript.Alert(this, "邮编不能为空，格式为：5位数字。");
            return;
        }
        else
        {
            reg = new Regex("\\d{5}");
            if (!(reg.Match(post).Success))
            {
                Shove._Web.JavaScript.Alert(this, "请输入正确的邮编,邮编格式为：5位数字。");
                return;
            }
        }
        if ("" == mobile)
        {
            Shove._Web.JavaScript.Alert(this, "电话号码不能为空。");
            return;
        }

        /*
        if ("" == email)
        {
            Shove._Web.JavaScript.Alert(this, "邮箱不能为空。");
            return;
        }
        else
        {
            //[\d\w]+\@[\d\w]+(\.((com)|(cn)))*
            //reg = new Regex("[\\d\\w]+@(\\w+(\\.)?)+");
            reg = new Regex(@"[\d\w]+\@[\d\w]+(\.((com)|(cn))){1,}");
            Match m = reg.Match(email);
            if (!(m.Success && m.Groups[0].Length == email.Length))
            {
                Shove._Web.JavaScript.Alert(this, "邮箱格式不正确，正确的邮箱格式如：cpkf@eims.com.cn。");
                return;
            }
        }
         */
        if ("" == QQ)
        {
            Shove._Web.JavaScript.Alert(this, "QQ不能为空。");
            return;
        }
        if ("" == serverMobile)
        {
            Shove._Web.JavaScript.Alert(this, "服务电话不能为空。");
            return;
        }
        if ("" == ICP)
        {
            Shove._Web.JavaScript.Alert(this, "ICP不能为空。");
            return;
        }
        if ("" == siteURL)
        {
            Shove._Web.JavaScript.Alert(this, "站点域名不能为空。");
            return;
        }
        // 开始保存
        Sites ts = new Sites();
        _Site.Clone(ts);

        _Site.Name = siteName;
        _Site.Company = company;
        _Site.ResponsiblePerson = "";
        _Site.Address = address;
        _Site.PostCode = post;
        _Site.ContactPerson = "";
        _Site.Telephone = "";
        _Site.Fax = "";
        _Site.Mobile = mobile;
        _Site.Email = email;
        _Site.QQ = QQ;
        _Site.ServiceTelephone = serverMobile;
        _Site.ICPCert = ICP;
        _Site.Urls = siteURL;
        string tempList = GetUseLotteryList();
        _Site.UseLotteryListRestrictions = tempList;
        _Site.UseLotteryList = tempList;
        _Site.UseLotteryListQuickBuy = tempList;

        //强制更新缓存值
        
        Shove._Web.Cache.SetCache("Site_UseLotteryList" + _Site.ID, _Site.UseLotteryList);
        

        string ReturnDescription = "";

        if (_Site.EditByID(ref ReturnDescription) < 0)
        {
            ts.Clone(_Site);
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);
            return;
        }
        Shove._Web.JavaScript.Alert(this.Page, "站点资料已经保存成功。");
    }





    /// <summary>
    /// 用于验证是否是管理员
    /// </summary>
    /// <param name="e"></param>
    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.SiteInfo);

        base.OnInit(e);
    }


    public class QRCode
    {
        public string Option { get; set; }
        public string Size { get; set; }
        public string BGColor { get; set; }
        public string QRCodeColor { get; set; }
        public string Url { get; set; }
        public string QRCodeType { get; set; }
        public string ImageSrc { get; set; }
        public string QRCodeImg { get; set; }
        public string FontContent { get; set; }
        public string FontSize { get; set; }
        public string FontFamily { get; set; }
        public string FontColor { get; set; }
    }
}