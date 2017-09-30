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
using System.Collections.Generic;

public partial class Admin_FinanceAddMoney : AdminPageBase
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
            intil();
        }

        BindData();

    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isRequestLogin = true;
        RequestLoginPage = "Admin/FinanceAddMoney.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.RechargeDetails);

        base.OnInit(e);
    }

    #endregion
    private void intil()
    {
        cbIsPayTime.Checked = true;
        tbStartTime.Value = DateTime.Now.AddDays(-12).ToString("yyyy-MM-dd");
        tbEndTime.Value = DateTime.Now.ToString("yyyy-MM-dd");
        DataTable dtBanks = new DAL.Tables.T_Banks().Open("ID,Name", "", "[Order]");
        if (dtBanks == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库异常", this.GetType().FullName);
            return;
        }
        ddlBankName.Items.Add(new ListItem("所有", "0"));
        foreach (DataRow dr in dtBanks.Rows)
        {
            ddlBankName.Items.Add(new ListItem(dr["name"].ToString(), dr["id"].ToString()));
        }
        ddlBankName.Items.Add(new ListItem("支付宝", "99"));
        ddlBankName.Items.Add(new ListItem("快钱", "100"));
        ddlBankName.Items.Add(new ListItem("手工充值", "1000"));

    }
    private void BindData()
    {
        string userName = keyword1.Value.Trim();
        long userID = -1;
        if (!(userName == "" || userName == "输入用户名"))
        {
            object o = Shove.Database.MSSQL.ExecuteScalar("select id from T_users where name=@name", new Shove.Database.MSSQL.Parameter("@name", SqlDbType.VarChar, 0, ParameterDirection.Input, userName));
            if (o == null)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "123", "jBox.info('查找的用户名不存在,请重新输入!', '系统提示');", true);
                return;
            }
            userID = Shove._Convert.StrToLong(o.ToString(), -1);
        }
        int ReturnValue = -1;
        string ReturnDescription = "";

        DataSet ds = null;

        int Results = -1;
        string startTime = "";
        string endTime = "";
        if (cbIsPayTime.Checked)
        {
            startTime = tbStartTime.Value.Trim();
            endTime = tbEndTime.Value.Trim() + " 23:59:59";
        }
        else
        {
            startTime = DateTime.Now.AddYears(-100).ToString();
            endTime = DateTime.Now.ToString();
        }

        if (string.IsNullOrEmpty(startTime))
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "123", "jBox.error('开始时间不能空!', '系统提示');", true);
            return;
        }
        if (string.IsNullOrEmpty(endTime))
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "123", "jBox.error('结束时间不能空!', '系统提示');", true);
            return;
        }



        int result = Shove._Convert.StrToInt(ddlResult.SelectedValue, -1);



        Results = DAL.Procedures.P_GetFinanceAddMoneyAdmin(ref ds, _Site.ID, userID, startTime, endTime, result, ref ReturnValue, ref ReturnDescription);



        if (Results < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }


        if ((ds == null) || (ds.Tables.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_FinanceAddMoney");

            return;
        }

        if (ReturnValue < 0)
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_FinanceAddMoney");

            return;
        }

        long tradeID = -952473;
        if (!(keyword2.Value.Trim() == "" || keyword2.Value.Trim() == "输入流水号"))
        {
            tradeID = Shove._Convert.StrToLong(keyword2.Value.Trim(), -952473);
            if (tradeID == -952473)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "123", "jBox.error('请输入正确的交易明细编号(例如：234)!', '系统提示');", true);
                return;
            }
        }
        string paymentID = keyword3.Value.Trim();

        DataView dv = ds.Tables[0].DefaultView;

        string sqlStr = "";
        if (tradeID != -952473)
        {
            sqlStr += "PayNumber=" + tradeID;

        }
        else
        {
            if (!(paymentID == "" || paymentID == "输入支付商流水号"))
            {
                sqlStr += "AlipayNo='" + paymentID + "'";
            }
        }
        dv.RowFilter = sqlStr;
        int bankID = Shove._Convert.StrToInt(ddlBankName.SelectedValue, 0);
        if (bankID > 0)
        {

            dv.RowFilter = "paytype like '%" + getBankList()[bankID] + "%'";
        }
        DataTable dt = dv.ToTable();

        if (dt == null || dt.Rows.Count < 1)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "123", "jBox.tip('无符合条件数据');", true);
            rptSchemes.DataSource = null;
            this.rptSchemes.DataBind();
            this.lblSumPayMoney.Text = 0.ToString("0.00");
            btnOut.Enabled = false;
            return;

        }
        ViewState["AccountDetails"] = dt;
        lblSumPayMoney.Text = Shove._Convert.StrToDouble(dt.Compute("SUM(money)", "").ToString(), 0).ToString("0.00");

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

        btnOut.Enabled = true;
    }


    private Dictionary<int, string> getBankList()
    {
        Dictionary<int, string> bankList = new Dictionary<int, string>();
        bankList.Add(1, "ICBCB2C");
        bankList.Add(2, "ABC");
        bankList.Add(3, "BOCB2C");
        bankList.Add(4, "CCB");
        bankList.Add(5, "COMM");
        bankList.Add(6, "MS");
        bankList.Add(7, "CITIC");
        bankList.Add(8, "CMB");
        bankList.Add(9, "SPDB");
        bankList.Add(10, "SDB");
        bankList.Add(11, "CEBBANK");
        bankList.Add(12, "GDB");
        bankList.Add(13, "HX");
        bankList.Add(14, "HQ");
        bankList.Add(15, "HF");
        bankList.Add(99, "ALIPAY_alipay");
        bankList.Add(100, "kq");
        bankList.Add(1000, "手工充值");
        return bankList;
    }
    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            double money;

            e.Item.Cells[4].Text = getBankName(e.Item.Cells[4].Text);

            money = Shove._Convert.StrToDouble(e.Item.Cells[5].Text, 0);
            e.Item.Cells[5].Text = (money == 0) ? "" : money.ToString("0.00");

            money = Shove._Convert.StrToDouble(e.Item.Cells[6].Text, 0);
            e.Item.Cells[6].Text = (money == 0) ? "" : money.ToString("0.00");

            string strResult = e.Item.Cells[7].Text;

            if (strResult == "1")
            {
                strResult = "True";
            }

            e.Item.Cells[7].Text = Shove._Convert.StrToBool(strResult, false) ? "<font color='Red'>成功</font>" : "未成功";

            e.Item.Cells[0].Text = "<a href='UserDetail.aspx?SiteID=" + e.Item.Cells[9].Text + "&ID=" + e.Item.Cells[8].Text + "'>" + e.Item.Cells[0].Text + "</a>";
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    //根据支付方式来获取相应的中文说明
    private string getBankName(string bankCode)
    {

        if (bankCode.IndexOf("手工充值") >= 0)
        {
            return "手工充值";
        }
        string bankName = "";
        string[] banks = bankCode.Split('_');

        if (banks.Length < 2)
        {
            return "未知银行";
        }

        if (banks[0].ToUpper() == "ALIPAY")
        {
            switch (banks[1].ToUpper())
            {
                case "ALIPAY":
                    bankName = "支付宝";
                    break;

                case "ICBCB2C":
                    bankName = "中国工商银行";
                    break;
                case "GDB":
                    bankName = "广东发展银行";
                    break;
                case "CEBBANK":
                    bankName = "中国光大银行";
                    break;
                case "CCB":
                    bankName = "中国建设银行";
                    break;
                case "COMM":
                    bankName = "中国交通银行";
                    break;
                case "ABC":
                    bankName = "中国农业银行";
                    break;
                case "SPDB":
                    bankName = "上海浦发银行";
                    break;
                case "SDB":
                    bankName = "深圳发展银行";
                    break;
                case "CIB":
                    bankName = "兴业银行";
                    break;
                case "HZCBB2C":
                    bankName = "杭州银行";
                    break;
                case "CMBC":
                    bankName = "杭州银行";
                    break;
                case "BOCB2C":
                    bankName = "中国银行";
                    break;
                case "CMB":
                    bankName = "中国招商银行";
                    break;
                case "CITIC":
                    bankName = "中信银行";
                    break;
                default:
                    bankName = "支付宝";
                    break;
            }
        }
        else if (banks[0].ToUpper() == "TENPAY")
        {
            switch (banks[1].ToUpper())
            {
                case "0":
                    bankName = "财付通";

                    break;
                case "1001":
                    bankName = "招商银行";

                    break;
                case "1002":
                    bankName = "中国工商银行";

                    break;
                case "1003":
                    bankName = "中国建设银行";

                    break;
                case "1004":
                    bankName = "上海浦东发展银行";

                    break;
                case "1005":
                    bankName = "中国农业银行";

                    break;
                case "1006":
                    bankName = "中国民生银行";

                    break;
                case "1008":
                    bankName = "深圳发展银行";

                    break;
                case "1009":
                    bankName = "兴业银行";

                    break;
                case "1028":
                    bankName = "广州银联";

                    break;
                case "1032":
                    bankName = "北京银行";

                    break;
                case "1020":
                    bankName = "中国交通银行";

                    break;
                case "1022":
                    bankName = "中国光大银行";

                    break;
                default:
                    bankName = "财付通";
                    break;
            }
        }
        else if (banks[0].ToUpper() == "51ZFK")
        {
            switch (banks[1].ToUpper())
            {
                case "SZX":
                    bankName = "神州行充值卡";
                    break;

                case "ZFK":
                    bankName = "51支付卡";
                    break;
                default:
                    bankName = "神州行充值卡";
                    break;
            }
        }
        else if (banks[0].ToUpper() == "99BILL")
        {

            bankName = "快钱";


        }
        return bankName;

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void btnOut_Click(object sender, EventArgs e)
    {
        if (this.rptSchemes.Items.Count < 1)
        {
            return;
        }
        DataTable dt = ViewState["AccountDetails"] as DataTable;
        dt.Columns.Add("ResultStr", typeof(string));
        if (dt == null || dt.Rows.Count < 1)
        {
            return;
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["Result"].ToString() == "1")
            {
                dt.Rows[i]["ResultStr"] = "成功";
            }
            if (dt.Rows[i]["Result"].ToString() == "-1")
            {
                dt.Rows[i]["ResultStr"] = "拒绝";
            }
            else
            {
                dt.Rows[i]["ResultStr"] = "未成功";
            }
        }
        dt.Columns.Remove("Result");
        dt.Columns.Remove("SiteID");
        dt.Columns.Remove("UserID");
        dt.Columns["Name"].ColumnName = "用户名";
        dt.Columns["RealityName"].ColumnName = "微信名称";
        dt.Columns["DateTime"].ColumnName = "时间";
        dt.Columns["PayType"].ColumnName = "支付方式";
        dt.Columns["Money"].ColumnName = "支付金额";
        dt.Columns["ResultStr"].ColumnName = "状态";
        Shove._Excel.NPOIExcelHelper.RenderToExcel(dt, HttpContext.Current, "充值明细.xls");

    }

    public string getChargeStat(string stat)
    {
        if (stat.Equals("0"))
        {
            return "<font color='#ff0000'>申请中</font>";
        }
        else if (stat.Equals("1"))
        {
            return "<font color='#11cc22'>成功</font>";
        }
        else
        {
            return "<font color='#1122ff'>拒绝</font>";
        }
    }


    public string getPayType(string stat)
    {
        if (stat.Equals("0"))
        {
            return "<font color='#ff0000'>微信支付</font>";
        }
        else if (stat.Equals("1"))
        {
            return "<font color='#11cc22'>支付宝</font>";
        }
        else if (stat.Equals("2"))
        {
            return "<font color='#1122ff'>银行转账</font>";
        }
        else
        {
            return stat;
        }
    }
}
