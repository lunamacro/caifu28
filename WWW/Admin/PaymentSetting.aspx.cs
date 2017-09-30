using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_PaymentSetting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {

        string sql = @"Select top 1 * from T_PaymentSetting ";

        DataTable dt = Shove.Database.MSSQL.Select(sql);

        if (null != dt && dt.Rows.Count > 0)
        {
            wxPayName.Text = dt.Rows[0]["wxpay_name"].ToString();
            wxPayMin.Text = dt.Rows[0]["wxpay_min"].ToString();
            wxPaySwitch.Checked = dt.Rows[0]["wxpay_switch"].ToString().Equals("True");
            wxQrcodeImage.ImageUrl = dt.Rows[0]["wxpay_qrcode"].ToString();

            aliPayName.Text = dt.Rows[0]["alipay_name"].ToString();
            aliPayMin.Text = dt.Rows[0]["alipay_min"].ToString();
            aliPaySwitch.Checked = dt.Rows[0]["alipay_switch"].ToString().Equals("True");
            aliQrcodeImage.ImageUrl = dt.Rows[0]["alipay_qrcode"].ToString();

            bankPayName.Text = dt.Rows[0]["bank_name"].ToString();
            bankPayBankName.Text = dt.Rows[0]["bank_bank"].ToString();
            bankPayCard.Text = dt.Rows[0]["bank_card"].ToString();
            bankPayShoukuanren.Text = dt.Rows[0]["bank_shoukuanren"].ToString();
            bankPayMin.Text = dt.Rows[0]["bank_min"].ToString();
            bankPaySwitch.Checked = dt.Rows[0]["bank_switch"].ToString().Equals("True");

        }
    }

    protected void btn_saveteul_Click(object sender, EventArgs e)
    {
        string wxpay_name = wxPayName.Text;
        string wxpay_min = wxPayMin.Text;
        string wxpay_switch = wxPaySwitch.Checked ? "1" : "0";

        string alipay_name = aliPayName.Text;
        string alipay_min = aliPayMin.Text;
        string alipay_switch = aliPaySwitch.Checked ? "1" : "0";

        string bank_name = bankPayName.Text;
        string bank_bank = bankPayBankName.Text;
        string bank_card = bankPayCard.Text;
        string bank_shoukuanren = bankPayShoukuanren.Text;
        string bank_min = bankPayMin.Text;
        string bank_switch = bankPaySwitch.Checked ? "1" : "0";


        string wcQrcodeUrl = wxQrcodeImage.ImageUrl;
        string aliQrcodeUrl = aliQrcodeImage.ImageUrl;
        //上传图片
        if (wxQrcode.PostedFile.FileName != "")
        { 
            if (wxQrcode.HasFile) //文件存在
            {
                SaveFile(wxQrcode.PostedFile);//保存上传文件
                wcQrcodeUrl = "http://www.300h.cn/Uploadfile/AppQrcode/" + wxQrcode.PostedFile.FileName;
            }
            else
            {
                Response.Write("<script>alert('上传文件不存在！')</script>");
            }
        }
        if (aliQrcode.PostedFile.FileName != "")
        {
            if (aliQrcode.HasFile) //文件存在
            {
                SaveFile(aliQrcode.PostedFile);//保存上传文件
                aliQrcodeUrl = "http://www.300h.cn/Uploadfile/AppQrcode/" + aliQrcode.PostedFile.FileName;
            }
            else
            {
                Response.Write("<script>alert('上传文件不存在！')</script>");
            }
        }


        string updateSQL = @"UPDATE T_PaymentSetting SET wxpay_name='" + wxpay_name + "',wxpay_min=" + wxpay_min + ",wxpay_switch=" + wxpay_switch + ",alipay_name='" + alipay_name + "',alipay_min=" + alipay_min + ",alipay_switch=" + alipay_switch + ",bank_name='" + bank_name + "',bank_bank='" + bank_bank + "',bank_card='" + bank_card + "',bank_shoukuanren='" + bank_shoukuanren + "',bank_min=" + bank_min + ",bank_switch=" + bank_switch + ",wxpay_qrcode='" + wcQrcodeUrl + "' ,alipay_qrcode='" + aliQrcodeUrl + "'  WHERE ID = 1";
        //Response.Write(updateSQL);
        //Response.End();
        int result = Shove.Database.MSSQL.ExecuteNonQuery(updateSQL);
        if (result < 0)
        {
            Shove._Web.JavaScript.Alert(this, "保存失败。");
            return;
        }
        Shove._Web.JavaScript.Alert(this, "保存成功。");
    }



    public void SaveFile(HttpPostedFile file)
    {
        string savePath = HttpContext.Current.Server.MapPath("/Uploadfile/AppQrcode/");
        //string savePath = "C:\\Users\\DJJ\\Desktop\\School_Canteen2\\Web\\images\\";
        string fileName = file.FileName;

        string pathToCheck = savePath + fileName;
        string tempfilename = "";
        if (System.IO.File.Exists(pathToCheck))
        {
            System.IO.File.Delete(pathToCheck);
        }
        savePath += fileName;
        file.SaveAs(savePath);
    }


}