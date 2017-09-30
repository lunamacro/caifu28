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

using System.Text;
using System.Text.RegularExpressions;
using Shove.Database;
public partial class Admin_UserDetail : AdminPageBase
{
    public int uid;
    public string siteUrl = "http://cf28.560651.com/";


    protected void Page_Load(object sender, EventArgs e)
    {
        uid = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("id"), -1);

        AjaxPro.Utility.RegisterTypeForAjax(typeof(Admin_UserDetail), this.Page);



        if (!this.IsPostBack)
        {
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.UserList);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        long SiteID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("SiteID"), -1);
        long UserID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);

        if ((SiteID < 1) || (UserID < 1))
        {
            this.Response.Redirect("Users.aspx", true);

            return;
        }

        DataTable dt = new DAL.Views.V_Users().Open("", "SiteID = " + SiteID.ToString() + " and [ID] = " + UserID.ToString(), "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_UserDetail");

            return;
        }

        if (dt.Rows.Count < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "用户不存在", "Admin_UserDetail");

            return;
        }

        tbSiteID.Text = SiteID.ToString();
        tbUserID.Text = UserID.ToString();

        Users tu = new Users(SiteID)[SiteID, UserID];

        if (tu.ID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "用户不存在", "Admin_UserDetail");

            return;
        }

        if (tu.UserType == 99)
        {
            adminsetting2.Checked = true;
        }
        else
        {
            adminsetting1.Checked = true;
        }

        tbUserName.Text = tu.Name;
        tbUserRealityName.Text = tu.NickName;

        //代理相关
        if (tu.isAgent ==2)
        {
            dlrb3.Checked = true;
        }
        else if (tu.isAgent == 1)
        {
            dlrb2.Checked = true;
        }
        else
        {
            dlrb1.Checked = true;
        }

        string sql = @"select ID,NickName from T_Users where isAgent=2";
        DataTable dtGroup = MSSQL.Select(sql);
        int selectIndex = 0;
        int autoIndex = 0;
        foreach (DataRow row in dtGroup.Rows)
        {
            ListItem item = new ListItem(row["NickName"].ToString(), row["ID"].ToString());
            selectGroup.Items.Add(item);
            if (int.Parse(row["ID"].ToString()) == tu.agentGroup)
            {
                selectIndex=autoIndex;
            }
            autoIndex++;
        }
        selectGroup.SelectedIndex=selectIndex;


        tbUserMobile.Text = Shove._Convert.ToDBC(tu.Mobile);
        cbUserMobileValid.Checked = (tu.isMobileValided && (tu.Mobile != ""));
        


        tbBankName.Text = tu.BankName;
        tbUserBankCardNumber.Text = Shove._Convert.ToDBC(tu.BankCardNumber).Trim();
        tbBankAddress.Text = tu.BankAddress;
        tbUserCradName.Text = tu.UserCradName;


        cbisCanLogin.Checked = tu.isCanLogin;

    }

    protected void btnUserAccount_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("UserAccountDetail.aspx?SiteID=" + tbSiteID.Text + "&ID=" + tbUserID.Text + "&UserName=" + tbUserName.Text);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {


        long SiteID = Shove._Convert.StrToLong(tbSiteID.Text, -1);
        long UserID = Shove._Convert.StrToLong(tbUserID.Text, -1);


        if ((SiteID < 1) || (UserID < 1))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_UserDetail");

            return;
        }

        Users tu = new Users(SiteID)[SiteID, UserID];

        if (tu.ID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "用户不存在", "Admin_UserDetail");

            return;
        }


        tu.Name = tbUserName.Text.Trim();
        tu.NickName = tbUserRealityName.Text.Trim();

        string password = tbUserPassword.Text.Trim();
        if(!String.IsNullOrEmpty(password)){
            tu.Password = password;
        }

        string passwordadv = tbUserPasswordAdv.Text.Trim();
        if (!String.IsNullOrEmpty(passwordadv))
        {
            tu.PasswordAdv = passwordadv;
        }

        short userType = 1;
        if (adminsetting2.Checked)
        {
            userType = 99;
        }
        tu.UserType = userType;

        int isAgent = 0;
        if (dlrb3.Checked)
        {
            isAgent = 2;
            tu.ReferId = 0;
            tu.agentGroup = (int)tu.ID;
        }
        else if (dlrb2.Checked)
        {
            isAgent =1;
            tu.ReferId = int.Parse(Request["selectGroup"]);
            tu.agentGroup = int.Parse(Request["selectGroup"]);
        }
        else
        {
            isAgent = 0;
            tu.ReferId = int.Parse(Request["selectRefer"]);
            tu.agentGroup = int.Parse(Request["selectGroup"]);
        }
        tu.isAgent = isAgent;
         



        tu.Mobile = Shove._Convert.ToDBC(tbUserMobile.Text.Trim()).Trim();
        tu.isMobileValided = (cbUserMobileValid.Checked && (tu.Mobile != ""));


        tu.BankName = tbBankName.Text.Trim();
        tu.BankCardNumber = Shove._Convert.ToDBC(tbUserBankCardNumber.Text.Trim()).Trim();
        tu.UserCradName = tbUserCradName.Text.Trim();
        tu.BankAddress = tbBankAddress.Text.Trim();
        tu.isCanLogin = cbisCanLogin.Checked;





        /*
        tu.PromotionMemberBonusScale = Shove._Convert.StrToDouble(tbPromotionMemberBonusScale.Text, 0);
        tu.PromotionSiteBonusScale = Shove._Convert.StrToDouble(tbPromotionSiteBonusScale.Text, 0);
        */

        string ReturnDescription = "";
        int returnValue = -1;

        if (tu.EditByID(ref ReturnDescription) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        

       
        Shove._Web.JavaScript.Alert(this.Page, "用户资料已经保存成功。");
    }

    protected void btnResetPassword_Click(object sender, EventArgs e)
    {
        long SiteID = Shove._Convert.StrToLong(tbSiteID.Text, -1);
        long UserID = Shove._Convert.StrToLong(tbUserID.Text, -1);

        if ((SiteID < 1) || (UserID < 1))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_UserDetail");

            return;
        }

        Users tu = new Users(SiteID)[SiteID, UserID];

        if (tu.ID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "用户不存在", "Admin_UserDetail");

            return;
        }

        string Password = GetRandPassword();

        tu.Password = Password;
        tu.PasswordAdv = Password;

        string ReturnDescription = "";

        if (tu.EditByID(ref ReturnDescription) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "用户密码已经被重置为：" + Password + "，请牢记。");
    }

    private string GetRandPassword()
    {
        string CharSet = "0123456789";
        string Password = "";
        Random rand = new Random(DateTime.Now.Millisecond);

        for (int i = 0; i < 6; i++)
        {
            Password += CharSet[rand.Next(0, 9)].ToString();
        }

        return Password;
    }
    protected void EmptyQuestn_Click(object sender, EventArgs e)
    {
        long SiteID = Shove._Convert.StrToLong(tbSiteID.Text, -1);
        long UserID = Shove._Convert.StrToLong(tbUserID.Text, -1);

        if ((SiteID < 1) || (UserID < 1))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_UserDetail");

            return;
        }

        Users tu = new Users(SiteID)[SiteID, UserID];

        if (tu.ID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "用户不存在", "Admin_UserDetail");

            return;
        }

        DAL.Tables.T_Users user = new DAL.Tables.T_Users();

        user.SecurityQuestion.Value = "";
        user.SecurityAnswer.Value = "";

        long Result = user.Update("ID=" + UserID);

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "清空安全问题失败");

            return;
        }
        Shove._Web.JavaScript.Alert(this.Page, "清空安全问题成功");
    }



    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string GetProvinceList()
    {
        string BANK_PROVINCE_LIST = "Home_Room_BindBankCard_BankInProvince";

        string provinceList = Shove._Web.Cache.GetCacheAsString(BANK_PROVINCE_LIST, "");
        if (string.IsNullOrEmpty(provinceList))
        {
            DataTable dt = Shove.Database.MSSQL.Select("select distinct ProvinceName from T_BankDetails order by ProvinceName ", new Shove.Database.MSSQL.Parameter[0]);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                sb.Append(row["ProvinceName"].ToString() + "|");
            }
            provinceList = sb.ToString();
            Shove._Web.Cache.SetCache(BANK_PROVINCE_LIST, provinceList);
        }

        return provinceList;

    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string GetCityList(string ProvinceName)
    {
        string BANK_PROVINCE_CITY_LIST = "BANK_PROVINCE_CITY_LIST" + ProvinceName;

        string cityList = Shove._Web.Cache.GetCacheAsString(BANK_PROVINCE_CITY_LIST, "");
        if (string.IsNullOrEmpty(cityList))
        {
            DataTable dt = Shove.Database.MSSQL.Select("select distinct CityName from T_BankDetails where ProvinceName='" + Shove._Web.Utility.FilteSqlInfusion(ProvinceName) + "' order by CityName ", new Shove.Database.MSSQL.Parameter[0]);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                sb.Append(row["CityName"].ToString() + "|");
            }
            cityList = sb.ToString();
            Shove._Web.Cache.SetCache(BANK_PROVINCE_CITY_LIST, cityList);
        }

        return cityList;

    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string GetBankTypeList()
    {
        string cacheKey = "Home_Room_BindBankCard_GetBankTypeList";

        string listStr = Shove._Web.Cache.GetCacheAsString(cacheKey, "");
        if (string.IsNullOrEmpty(listStr))
        {
            DataTable dt = Shove.Database.MSSQL.Select("select distinct  BankTypeName  from T_BankDetails order by BankTypeName ", new Shove.Database.MSSQL.Parameter[0]);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                sb.Append(row["BankTypeName"].ToString() + "|");
            }
            listStr = sb.ToString();
            Shove._Web.Cache.SetCache(cacheKey, listStr, 600);
        }

        return listStr;

    }


    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string GetBankBranchNameList(string cityName, string bankTypeName)
    {
        string cacheKey = "Home_Room_BindBankCard_GetBankBranchNameList_" + cityName + "_" + bankTypeName;

        string listStr = Shove._Web.Cache.GetCacheAsString(cacheKey, "");
        if (string.IsNullOrEmpty(listStr))
        {
            DataTable dt = Shove.Database.MSSQL.Select("select   BankName  from T_BankDetails where BankTypeName='" + bankTypeName + "' and CityName='" + cityName + "'   order by BankName ", new Shove.Database.MSSQL.Parameter[0]);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                sb.Append(row["BankName"].ToString() + "|");
            }
            listStr = sb.ToString();
            Shove._Web.Cache.SetCache(cacheKey, listStr, 600);
        }

        return listStr;

    }
}
