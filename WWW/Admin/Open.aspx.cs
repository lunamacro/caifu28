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
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Shove.Database;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public partial class Admin_Open : AdminPageBase
{
    private bool Step1IsOpen;
    private bool Step2IsOpen;
    public static int taxSwitch;

    private string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Server.ScriptTimeout = 60 * 10;

        if (!this.IsPostBack)
        {
            BindDataForLottery();

            BindDataForIsuse();

            h_SchemeID.Value = "0";
        }
        btnGO_Step1.AlertText = "确信输入无误，并立即执行开奖第一步骤吗？";
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.OpenAndDistribute);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ")", "Sort asc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }


        Shove.ControlExt.FillDropDownList(ddlLottery, dt, "Name", "ID");

        if (ddlLottery.Items.Count < 1)
        {
            btnGO.Enabled = false;
            btnGO_Step1.Enabled = false;
            btnGO_Step2.Enabled = false;
            btnGO_Step3.Enabled = false;
            tbWinNumber.Enabled = false;
        }
        else
        {
            ddlLottery_SelectedIndexChanged(ddlLottery, new EventArgs());
        }
    }

    private void BindDataForIsuse()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        string Code = "";

        DataTable dt = new DAL.Tables.T_Isuses().Open("[ID], [Name]", "LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) + Code + " and EndTime<getdate() and isOpened = 0", "EndTime DESC");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlIsuse, dt, "Name", "ID");

        if (ddlIsuse.Items.Count > 0)
        {


            WinNumberOther.Visible = true;
            btnGO.Enabled = true;
            btnGO_Step1.Enabled = true;
            btnGO_Step2.Enabled = false;
            btnGO_Step3.Enabled = false;
            tbWinNumber.Enabled = true;
        }
        else
        {
            WinNumberOther.Visible = true;
            btnGO.Enabled = false;
            btnGO_Step1.Enabled = false;
            btnGO_Step2.Enabled = false;
            btnGO_Step3.Enabled = false;
            tbWinNumber.Enabled = false;
        }
    }

    private void BindDataForWinMoney()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }
        taxSwitch = PF.GetTaxSwitch();
        DataTable dt = new DAL.Tables.T_WinTypes().Open("", "LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue), "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (taxSwitch == 1)
            {
                if (dt.Rows[i]["DefaultMoney"] != null)
                {
                    if (Convert.ToDouble(dt.Rows[i]["DefaultMoney"]) > 10000d)
                    {
                        dt.Rows[i]["DefaultMoneyNoWithTax"] = Convert.ToDouble(dt.Rows[i]["DefaultMoney"]) * 0.8;
                    }
                }
            }
            else
            {
                dt.Rows[i]["DefaultMoneyNoWithTax"] = dt.Rows[i]["DefaultMoney"];
            }
        }
        g.DataSource = dt.DefaultView;
        g.DataBind();

        if (ddlLottery.SelectedValue.Equals("74"))
        {
            DataTable dtrjc = new DAL.Tables.T_WinTypes().Open("", "LotteryID = " + Shove._Web.Utility.FilteSqlInfusion("75"), "[Order]");

            if (dtrjc == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }
            for (int i = 0; i < dtrjc.Rows.Count; i++)
            {
                if (taxSwitch == 1)
                {
                    if (dtrjc.Rows[i]["DefaultMoney"] != null)
                    {
                        if (Convert.ToDouble(dtrjc.Rows[i]["DefaultMoney"]) > 10000d)
                        {
                            dtrjc.Rows[i]["DefaultMoneyNoWithTax"] = Convert.ToDouble(dtrjc.Rows[i]["DefaultMoney"]) * 0.8;
                        }
                    }
                }
                else
                {
                    dtrjc.Rows[i]["DefaultMoneyNoWithTax"] = dtrjc.Rows[i]["DefaultMoney"];
                }
            }
            g1.DataSource = dtrjc.DefaultView;
            g1.DataBind();
            g1.Visible = true;
            sfcname.Visible = true;
            rjcname.Visible = true;
        }
        else
        {
            g1.Visible = false;
            sfcname.Visible = false;
            rjcname.Visible = false;
        }
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindDataForWinMoney();

        string WinLotteryExemple = "格式：" + DAL.Functions.F_GetLotteryWinNumberExemple(int.Parse(ddlLottery.SelectedValue));

        labTip.Text = WinLotteryExemple;

        BindDataForIsuse();
    }

    protected void ddlIsuse_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("WinLotteryNumber, isOpened", "[ID] = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue), "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        bool isOpened = Shove._Convert.StrToBool(dt.Rows[0]["isOpened"].ToString(), true);
        string WinLotteryNumber = dt.Rows[0]["WinLotteryNumber"].ToString();

        if (isOpened)
        {
            btnGO.Enabled = false;
            btnGO_Step1.Enabled = false;
            btnGO_Step2.Enabled = false;
            btnGO_Step3.Enabled = false;
            tbWinNumber.Enabled = false;

            PF.GoError(ErrorNumber.Unknow, "此期已经开奖了，不能重复开奖。", this.GetType().BaseType.FullName);

            return;
        }


        WinNumberOther.Visible = true;
        btnGO.Enabled = true;
        btnGO_Step1.Enabled = true;
        btnGO_Step2.Enabled = false;
        btnGO_Step3.Enabled = false;
        tbWinNumber.Enabled = true;
        DAL.Tables.T_IsuseBonuses T_IsuseBonuses = new DAL.Tables.T_IsuseBonuses();
        DataTable DT = T_IsuseBonuses.Open(" top  1 * ", "IsuseID=" + Shove._Convert.StrToLong(ddlIsuse.SelectedValue, 0) + " and UserID=" + _User.ID + "", "");

        if (DT.Rows.Count > 0)
        {
            WinLotteryNumber = DT.Rows[0]["WinNumber"].ToString(); ;
            if (WinLotteryNumber != "")
            {
                tbWinNumber.Text = WinLotteryNumber;
            }

        }
    }

    protected void g_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataKey key = g.DataKeys[g.DataKeys.Count - 1];

            double money = Shove._Convert.StrToDouble(key.Values[0].ToString(), 0);
            ((TextBox)e.Row.Cells[1].FindControl("tbMoney")).Text = (money == 0 ? "" : money.ToString());

            money = Shove._Convert.StrToDouble(key.Values[1].ToString(), 0);
            ((TextBox)e.Row.Cells[2].FindControl("tbMoneyNoWithTax")).Text = (money == 0 ? "" : money.ToString());
        }
    }

    protected void g1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataKey key = g1.DataKeys[g1.DataKeys.Count - 1];

            double money = Shove._Convert.StrToDouble(key.Values[0].ToString(), 0);
            ((TextBox)e.Row.Cells[1].FindControl("tbMoney")).Text = (money == 0 ? "" : money.ToString());

            money = Shove._Convert.StrToDouble(key.Values[1].ToString(), 0);
            ((TextBox)e.Row.Cells[2].FindControl("tbMoneyNoWithTax")).Text = (money == 0 ? "" : money.ToString());
        }
    }

    protected void btnGO_Click(object sender, System.EventArgs e)
    {
        tbWinNumber.Text = Shove._Convert.ToDBC(tbWinNumber.Text.Trim().Replace("　", " ")).Trim();


        //处理福彩3D格式
        string windNumber = tbWinNumber.Text;
        if (ddlLottery.SelectedValue == "6")
        {
            windNumber = "";
            for (int i = 0; i < tbWinNumber.Text.Length; i++)
            {
                windNumber += tbWinNumber.Text.Substring(i, 1) + ",";
            }
            windNumber = windNumber.Substring(0, 5);
        }

        if (!new SLS.Lottery()[int.Parse(ddlLottery.SelectedValue)].AnalyseWinNumber(windNumber))
        {
            Shove._Web.JavaScript.Alert(this.Page, "开奖号码不正确！");

            return;
        }

        SystemOptions so = new SystemOptions();
        bool isCompareWinMoneyNoWithFax = so["isCompareWinMoneyNoWithFax"].ToBoolean(true);

        double[] WinMoneyList = new double[g.Rows.Count * 2];

        for (int i = 0; i < g.Rows.Count; i++)
        {
            WinMoneyList[i * 2] = Shove._Convert.StrToDouble(((TextBox)g.Rows[i].Cells[1].FindControl("tbMoney")).Text, 0);
            WinMoneyList[i * 2 + 1] = Shove._Convert.StrToDouble(((TextBox)g.Rows[i].Cells[2].FindControl("tbMoneyNoWithTax")).Text, 0);

            if (WinMoneyList[i * 2] < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "第 " + (i + 1).ToString() + " 项奖金输入错误！");

                return;
            }

            if (WinMoneyList[i * 2] < WinMoneyList[i * 2 + 1])
            {
                if (isCompareWinMoneyNoWithFax)
                {
                    Shove._Web.JavaScript.Alert(this.Page, "第 " + (i + 1).ToString() + " 项税后奖金输入错误(不能大于税前奖金)！");

                    return;
                }
            }
        }

        DataTable dt = new DAL.Tables.T_Schemes().Open("", "IsuseID = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue) + " and isOpened = 0", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {

            string totalDescription = "";
            double totalWinMoneyNoWithTax = 0;
            double totalWinMoney = 0;


            //取得投注号码
            DataTable mixcast = new DAL.Tables.T_SchemesMixcast().Open("", "SchemeId=" + dt.Rows[i]["ID"].ToString(), "[ID]");
            List<WinDesc> winDescList = new List<WinDesc>();
            foreach (DataRow item in mixcast.Rows)
            {
                string description = "";
                string LotteryNumber = item["LotteryNumber"].ToString();
                double winMoneyNoWithTax = 0;
                double winMoney = new SLS.Lottery()[int.Parse(ddlLottery.SelectedValue)].ComputeWin(LotteryNumber, tbWinNumber.Text.Trim(), ref description, ref winMoneyNoWithTax, int.Parse(item["PlayTypeID"].ToString()), WinMoneyList);
                WinDesc winDescModel = new WinDesc();
                winDescModel.schemeId = dt.Rows[i]["ID"].ToString();
                winDescModel.desc = description;
                winDescList.Add(winDescModel);
                totalWinMoneyNoWithTax += winMoneyNoWithTax;
                totalWinMoney += winMoney;

                Shove.Database.MSSQL.ExecuteNonQuery("update T_SchemesMixcast set WinMoneyNoWithTax = @p1,WinMoney=@p2,WinDescription=@p3 where [ID] = " + item["ID"],
                    new Shove.Database.MSSQL.Parameter("p1", SqlDbType.Money, 0, ParameterDirection.Input, winMoney * Shove._Convert.StrToInt(dt.Rows[i]["Multiple"].ToString(), 1)),
                    new Shove.Database.MSSQL.Parameter("p2", SqlDbType.Money, 0, ParameterDirection.Input, winMoneyNoWithTax * Shove._Convert.StrToInt(dt.Rows[i]["Multiple"].ToString(), 1)),
                    new Shove.Database.MSSQL.Parameter("p3", SqlDbType.VarChar, 0, ParameterDirection.Input, description));

                totalDescription += description + (description.Length == 0 ? "" : ",");
            }
            if (totalDescription.Length > 1)
                totalDescription = totalDescription.Substring(0, totalDescription.Length - 1);

            List<WinDesc> winDescListSing = winDescList.Where(p => p.schemeId == dt.Rows[i]["ID"].ToString()).ToList();
            List<string> winDescTemp = new List<string>();
            for (int winDescIndex = 0; winDescIndex < winDescListSing.Count(); winDescIndex++)
            {
                for (int k = 0; k < winDescListSing[winDescIndex].desc.ToString().Split('，').Length; k++)
                {
                    winDescTemp.Add(winDescListSing[winDescIndex].desc.ToString().Split('，')[k]);
                }
            }

            Shove.Database.MSSQL.ExecuteNonQuery("update T_Schemes set PreWinMoney = @p1, PreWinMoneyNoWithTax = @p2, EditWinMoney = @p3, EditWinMoneyNoWithTax = @p4, WinDescription = @p5 where [ID] = " + dt.Rows[i]["ID"].ToString(),
                new Shove.Database.MSSQL.Parameter("p1", SqlDbType.Money, 0, ParameterDirection.Input, totalWinMoney * Shove._Convert.StrToInt(dt.Rows[i]["Multiple"].ToString(), 1)),
                new Shove.Database.MSSQL.Parameter("p2", SqlDbType.Money, 0, ParameterDirection.Input, totalWinMoneyNoWithTax * Shove._Convert.StrToInt(dt.Rows[i]["Multiple"].ToString(), 1)),
                new Shove.Database.MSSQL.Parameter("p3", SqlDbType.Money, 0, ParameterDirection.Input, totalWinMoney * Shove._Convert.StrToInt(dt.Rows[i]["Multiple"].ToString(), 1)),
                new Shove.Database.MSSQL.Parameter("p4", SqlDbType.Money, 0, ParameterDirection.Input, totalWinMoneyNoWithTax * Shove._Convert.StrToInt(dt.Rows[i]["Multiple"].ToString(), 1)),
                new Shove.Database.MSSQL.Parameter("p5", SqlDbType.VarChar, 0, ParameterDirection.Input, SLS.Lottery.StatisticsWinDesc(winDescTemp)));

        }

        Shove._Web.JavaScript.Alert(this.Page, "奖金已经计算完成，请执行第三步进行派奖!。");

        BindDataForIsuse();

    }

    protected void btnGO_Step1_Click(object sender, EventArgs e)
    {

        tbWinNumber.Text = Shove._Convert.ToDBC(tbWinNumber.Text.Trim().Replace("　", " ")).Trim();
        string WinNumber = tbWinNumber.Text;
        BJXY28DrawingLottery(int.Parse(ddlLottery.SelectedValue), ddlIsuse.SelectedItem.Text, WinNumber);
        Shove._Web.JavaScript.Alert(this.Page, "开奖已完成.");
    }

    protected void btnGO_Step2_Click(object sender, EventArgs e)
    {
        btnGO_Step1.AlertText = "";


        tbWinNumber.Text = Shove._Convert.ToDBC(tbWinNumber.Text.Trim().Replace("　", " ")).Trim();

        string WinNumber = tbWinNumber.Text;
        if (ddlLottery.SelectedValue == "6")
        {
            WinNumber = "";
            for (int i = 0; i < tbWinNumber.Text.Length; i++)
            {
                WinNumber += tbWinNumber.Text.Substring(i, 1) + ",";
            }
            WinNumber = WinNumber.Substring(0, 5);
        }
        if (!new SLS.Lottery()[int.Parse(ddlLottery.SelectedValue)].AnalyseWinNumber(WinNumber))
        {
            Shove._Web.JavaScript.Alert(this.Page, "开奖号码不正确！");

            return;
        }
        SystemOptions so = new SystemOptions();
        bool isCompareWinMoneyNoWithFax = so["isCompareWinMoneyNoWithFax"].ToBoolean(true);

        double[] WinMoneyList = new double[g.Rows.Count * 2];

        for (int i = 0; i < g.Rows.Count; i++)
        {
            WinMoneyList[i * 2] = Shove._Convert.StrToDouble(((TextBox)g.Rows[i].Cells[1].FindControl("tbMoney")).Text, 0);
            WinMoneyList[i * 2 + 1] = Shove._Convert.StrToDouble(((TextBox)g.Rows[i].Cells[2].FindControl("tbMoneyNoWithTax")).Text, 0);

            if (WinMoneyList[i * 2] < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "第 " + (i + 1).ToString() + " 项奖金输入错误！");

                return;
            }

            if (WinMoneyList[i * 2] < WinMoneyList[i * 2 + 1])
            {
                if (isCompareWinMoneyNoWithFax)
                {
                    Shove._Web.JavaScript.Alert(this.Page, "第 " + (i + 1).ToString() + " 项税后奖金输入错误(不能大于税前奖金)！");

                    return;
                }
            }
        }


        

        Step2IsOpen =false;

        btnGO_Step2.Enabled = Step2IsOpen;
        btnGO_Step3.Enabled = (!Step2IsOpen);

        string Message = "请再次执行第二步";

        if (!Step2IsOpen)
        {
            Message = "开奖步骤二已经完成，请执行第三步.";
        }

        Shove._Web.JavaScript.Alert(this.Page, Message);
    }

    protected void btnGO_Step3_Click(object sender, EventArgs e)
    {
        btnGO_Step1.AlertText = "";

        tbWinNumber.Text = Shove._Convert.ToDBC(tbWinNumber.Text.Trim().Replace("　", " ")).Trim();

        string WinNumber = tbWinNumber.Text;

        if (!new SLS.Lottery()[int.Parse(ddlLottery.SelectedValue)].AnalyseWinNumber(WinNumber))
        {
            Shove._Web.JavaScript.Alert(this.Page, "开奖号码不正确！");

            return;
        }
        bool isEndOpen = true;


        btnGO_Step1.Enabled = false;
        btnGO_Step2.Enabled = false;
        btnGO_Step3.Enabled = true;

        string Message = "开奖成功。本期开奖还未全部完成, 请继续操作第三步。";

        if (isEndOpen)
        {
            BindDataForIsuse();
            btnGO_Step3.Enabled = false;
            Message = "开奖成功,本期开奖已全部完成。";
        }

        DataTable dt = new DAL.Tables.T_Schemes().Open("", "IsuseID=" + Shove._Convert.StrToLong(ddlIsuse.SelectedValue, 0).ToString(), "");

        if (dt == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "开奖出现异常!请联系开发维护人员");
            return;
        }


        Shove._Web.Cache.ClearCache(DataCache.IsusesInfo + ddlLottery.SelectedValue);

        Shove._Web.JavaScript.Alert(this.Page, Message);
    }



    #region 北京幸运28开奖派奖
    /// <summary>
    /// 北京幸运28开奖派奖
    /// </summary>
    /// <param name="lotteryID"></param>
    /// <param name="isuseName"></param>
    /// <param name="winLotteryNumber"></param>
    public void BJXY28DrawingLottery(int lotteryID, string isuseName, string winLotteryNumber)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        conn.Open();
        Log log = new Log("openLottery");
        int returnValue = 0;
        string returnDescription = "";
        DataTable dtIsuse = new DAL.Tables.T_Isuses().Open(conn, "top 1 [ID], IsOpened", "LotteryID = " + lotteryID.ToString() + " and [Name] = '" + isuseName + "' and IsOpened=0 and year(StartTime) = YEAR(GETDATE())", "");
        if (dtIsuse == null)
        {
            log.Write("数据读写错误001");
            return;
        }
        if (dtIsuse.Rows.Count <= 0)
        {
            log.Write("暂无对应期号信息，彩种ID：" + lotteryID.ToString() + "， 期号：" + isuseName);
            return;
        }
        if (Shove._Convert.StrToBool(dtIsuse.Rows[0]["IsOpened"].ToString(), false))
        {
            log.Write("彩种ID：" + lotteryID + "第" + isuseName + "期已开奖");
            return;
        }
        long isuseID = Shove._Convert.StrToLong(dtIsuse.Rows[0]["ID"].ToString(), -1);


        for (int k = 0; k < 3; k++)
        {

            DataTable dt = null;
            dt = Shove.Database.MSSQL.Select(conn, "select b.ID,b.InitiateUserID from T_schemesFrom as a left join T_schemes as b on b.id=a.schemesid where b.isOpened=0 and IsuseID='" + isuseID.ToString() + "' and a.HomeIndex=" + k);
            //取得投注号码
            DataTable mixcast = Shove.Database.MSSQL.Select(conn, "select b.ID,c.LotteryNumber,c.Multiple,c.PlayTypeID,c.Money,c.SchemeId,c.WinMoney,c.WinMoneyNoWithTax,c.WinDescription from T_schemesFrom as a left join T_schemes as b on b.id=a.schemesid left join T_SchemesMixcast as c on c.SchemeId=b.ID where b.isOpened=0 and IsuseID='" + isuseID.ToString() + "' and a.HomeIndex=" + k + " and c.PlayTypeID in(9901,9902,9903,9801,9802,9803) order by ID asc");

            

            if (dt == null)
            {
                log.Write("数据读写错误002");
                return;
            }
            // 准备开奖，开奖之前，对出票不完整的方案进行出票处理
            returnValue = 0;
            returnDescription = "";

            int lottid = Shove._Convert.StrToInt(lotteryID + "01", 0);
            int minID = lottid + (19 * k);
            int maxID = lottid + (19 * (k + 1));
            DataTable dtWinTypes = new DAL.Tables.T_WinTypes().Open(conn, "[DefaultMoney],[DefaultMoneyNoWithTax]", "LotteryID = " + lotteryID.ToString() + " and [Hall] = " + k, "[Order]");
            if (dtWinTypes == null)
            {
                log.Write("奖金读取数据读写错误");
                return;
            }
            #region 计算方案奖金

            double[] winMoneyList = new double[dtWinTypes.Rows.Count * 2];
            for (int y = 0; y < dtWinTypes.Rows.Count; y++)
            {
                winMoneyList[y * 2] = Shove._Convert.StrToDouble(dtWinTypes.Rows[y]["DefaultMoney"].ToString(), 0);
                winMoneyList[y * 2 + 1] = Shove._Convert.StrToDouble(dtWinTypes.Rows[y]["DefaultMoneyNoWithTax"].ToString(), 0);
                if (winMoneyList[y * 2] < 0)
                {
                    log.Write("第 " + (y + 1).ToString() + " 项奖金输入错误！");
                    return;
                }
            }

          
            DataTable dt_Addaward = new DAL.Tables.T_Addaward().Open(conn, "", "GETDATE() BETWEEN StartTime AND EndTime", "");//加奖
            string noWinSchemeID = "";

            int count = 0;
            StringBuilder sb = new StringBuilder();
            //中奖明细表
            DataTable winMoneyDetailDtTemp = new DataTable();
            winMoneyDetailDtTemp = new DAL.Tables.T_WinMoneyDetail().Open(conn, "ID,SchemesID,PlaysID,WinMoney,WinMoneyNoWithTax", "1<>1", "");
            DataTable winMoneyDetailDt = winMoneyDetailDtTemp.Clone();
            List<string> executeSqlList = new List<string>();

            double allBet = 0; //个人本期总投注
            double comboBet = 0; //个人本期组合投注
            double dxdsBet = 0; //个人本期大小单双投注

            #region suanj
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int schemeID = Convert.ToInt32(dt.Rows[i]["ID"]);

                List<string> descriptionList = new List<string>();
                StringBuilder totalDescription = new StringBuilder();
                double totalWinMoneyNoWithTax = 0;
                double totalWinMoney = 0;
                double totalAddWinMoney = 0;
                int oneNumber = 0;
                int twoNumber = 0;
                int winNumber = 0;

                string userID = dt.Rows[i]["InitiateUserID"].ToString();
                StringBuilder sql_allBet = new StringBuilder();
                sql_allBet.Append("select SUM(s.Money) as bet from T_Schemes as s");
                sql_allBet.Append(" left join T_SchemesFrom as sf on sf.SchemesID=s.ID ");
                sql_allBet.Append(" left join T_SchemesMixcast as smc on smc.SchemeId=s.ID ");
                sql_allBet.Append("  where InitiateUserID=" + userID + " and IsuseID='" + isuseID + "' and sf.HomeIndex=" + k);
                StringBuilder sql_comboBet = new StringBuilder();
                sql_comboBet.Append(sql_allBet.ToString());
                sql_comboBet.Append(" and smc.LotteryNumber in ('大单','大双','小单','小双')");
                StringBuilder sql_dxdsBet = new StringBuilder();
                sql_dxdsBet.Append(sql_allBet.ToString());
                sql_dxdsBet.Append(" and smc.LotteryNumber in ('大','小','单','双') ");

                DataTable dt_allBet = MSSQL.Select(conn, sql_allBet.ToString());
                DataTable dt_comboBet = MSSQL.Select(conn, sql_comboBet.ToString());
                DataTable dt_dxdsBet = MSSQL.Select(conn, sql_dxdsBet.ToString());

                if (dt_allBet != null && dt_allBet.Rows.Count > 0)
                {
                    allBet = Shove._Convert.StrToDouble((Shove._Convert.StrToDouble(dt_allBet.Rows[0]["bet"].ToString(), 0).ToString("0.00")), 0);
                }
                if (dt_comboBet != null && dt_comboBet.Rows.Count > 0)
                {
                    comboBet = Shove._Convert.StrToDouble((Shove._Convert.StrToDouble(dt_comboBet.Rows[0]["bet"].ToString(), 0).ToString("0.00")), 0);
                }
                if (dt_dxdsBet != null && dt_dxdsBet.Rows.Count > 0)
                {
                    dxdsBet = Shove._Convert.StrToDouble((Shove._Convert.StrToDouble(dt_dxdsBet.Rows[0]["bet"].ToString(), 0).ToString("0.00")), 0);
                }
                try
                {
                    #region 方案号码计算奖金

                    foreach (DataRow item in mixcast.Select("SchemeId=" + schemeID))
                    {
                        string description = "";
                        string lotteryNumber = item["LotteryNumber"].ToString();
                        int multiple = Shove._Convert.StrToInt(item["Multiple"].ToString(), 1);
                        double money = Shove._Convert.StrToDouble(item["Money"].ToString(), 1);
                        string lotteryId = item["PlayTypeID"].ToString();
                        lotteryId = lotteryId.Substring(0, lotteryId.Length - 2);


                        //加奖金额
                        double addWinMoney = 0, addWinMoneyNoWithTax = 0;
                        int winCounts = 0;
                        double winMoney = 0, winMoneyNoWithTax = 0;

                        winMoney = new SLS.Lottery()[lotteryID].ComputeWin(k, allBet, comboBet, dxdsBet, lotteryNumber, winLotteryNumber, ref description, ref winMoneyNoWithTax, int.Parse(item["PlayTypeID"].ToString()), ref winCounts, winMoneyList);

                        //加上此段代码原因：排除开奖奖金错误，因为有的彩票类计算奖金会出现负数。
                        winMoney = winMoney <= 0 ? 0 : winMoney;

                        winMoney = money * winMoney;//投注金额 * 倍率 
                        // winMoneyNoWithTax = money * winMoneyNoWithTax;//投注金额 * 倍率
                        winMoneyNoWithTax = winMoney;//测试



                        //获取加奖表数据具体玩法
                        DataRow[] drs = dt_Addaward.Select("LotteryID=" + lotteryId, "");
                        if (drs != null && drs.Length != 0)
                        {
                            string addInfo = drs[0]["AddInfo"].ToString();
                            string lotteryPlayID = item["PlayTypeID"].ToString();
                            if (addInfo != "")
                            {
                                string[] list = addInfo.Split('|');
                                for (int i1 = 0; i1 < list.Length; i1++)
                                {
                                    if (list[i1].Split(',')[0].Equals(lotteryPlayID))
                                    {
                                        addWinMoney = double.Parse(list[i1].Split(',')[1]);
                                        addWinMoneyNoWithTax = double.Parse(list[i1].Split(',')[1]);
                                    }
                                }
                            }
                        }

                        //总加奖金额
                        totalAddWinMoney += addWinMoney * winCounts * multiple;
                        //总奖金(包含加奖金额)
                        totalWinMoney += winMoney * multiple + addWinMoney * winCounts * multiple;
                        //总奖金税后
                        totalWinMoneyNoWithTax += winMoneyNoWithTax + winMoney * (multiple - 1) + addWinMoneyNoWithTax * winCounts * multiple;
                        //单注奖金
                        winMoney = winMoney * multiple + addWinMoney * winCounts * multiple;
                        //单注奖金税后
                        winMoneyNoWithTax = winMoneyNoWithTax + winMoney * (multiple - 1) + addWinMoneyNoWithTax * winCounts * multiple;
                        //中奖说明
                        descriptionList.Add(description.Replace(" ", ""));

                        count++;
                        //写入需要插入T_WinMoneyDetail的数值
                        DataRow winMoneyDetailDr = winMoneyDetailDt.NewRow();
                        winMoneyDetailDr["SchemesID"] = schemeID;
                        winMoneyDetailDr["PlaysID"] = item["PlayTypeID"];
                        winMoneyDetailDr["WinMoney"] = winMoney;
                        winMoneyDetailDr["WinMoneyNoWithTax"] = winMoneyNoWithTax;
                        winMoneyDetailDt.Rows.Add(winMoneyDetailDr);
                        count++;
                        //更新T_SchemesMixcast的值
                        executeSqlList.Add("update T_SchemesMixcast set WinMoney=" + winMoney + ", WinMoneyNoWithTax=" + winMoneyNoWithTax + ",WinDescription='" + description + "加奖：" + addWinMoney * multiple + "' where [SchemeId] = " + schemeID + ";");
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    totalWinMoney = 0;
                    log.Write("方案 ID:" + schemeID + " 算奖出现错误!" + "，出错行：" + ex.StackTrace + "，方法：" + ex.TargetSite);
                    return;
                }
                if (totalWinMoney == 0)
                {
                    noWinSchemeID += dt.Rows[i]["ID"].ToString() + ",";
                    continue;
                }
                count++;
                //更新T_Schemes表
                sb.Clear();
                for (int d = 0; d < descriptionList.Count; d++)
                {
                    descriptionList.Remove("");
                }
                sb.Append("update T_Schemes set PreWinMoney=").Append(totalWinMoney)
                        .Append(",PreWinMoneyNoWithTax=").Append(totalWinMoneyNoWithTax)
                        .Append(",EditWinMoney=").Append(totalWinMoney)
                        .Append(",EditWinMoneyNoWithTax=").Append(totalWinMoneyNoWithTax)
                        .Append(",LotteryNumber='").Append(oneNumber + "," + twoNumber + "," + winNumber).Append("'")
                        .Append(",WinDescription='").Append(string.Join(",", descriptionList.ToArray())).Append("'")
                        .Append(",Description='").Append("加奖：" + totalAddWinMoney).Append("'")
                        .Append(" where [ID] =").AppendLine(schemeID.ToString() + ";");
                executeSqlList.Add(sb.ToString());
                
            }
            #endregion
            



            if (noWinSchemeID.EndsWith(","))
            {
                noWinSchemeID = noWinSchemeID.Substring(0, noWinSchemeID.Length - 1);
            }
            if (!string.IsNullOrEmpty(noWinSchemeID))
            {
                sb.Clear();
                sb.Append("update T_Schemes set PreWinMoney = 0")
                    .Append(", PreWinMoneyNoWithTax = 0")
                    .Append(", EditWinMoney = 0")
                    .Append(", EditWinMoneyNoWithTax = 0")
                    .Append(", WinDescription = ''")
                    .Append(" where [ID] in (" + noWinSchemeID + ");");
                executeSqlList.Add(sb.ToString());
            }
            if (count > 0)
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(conn))
                {
                    try
                    {
                        sqlBulkCopy.DestinationTableName = "T_WinMoneyDetail";
                        sqlBulkCopy.ColumnMappings.Add("ID", "ID");
                        sqlBulkCopy.ColumnMappings.Add("SchemesID", "SchemesID");
                        sqlBulkCopy.ColumnMappings.Add("PlaysID", "PlaysID");
                        sqlBulkCopy.ColumnMappings.Add("WinMoney", "WinMoney");
                        sqlBulkCopy.ColumnMappings.Add("WinMoneyNoWithTax", "WinMoneyNoWithTax");
                        sqlBulkCopy.WriteToServer(winMoneyDetailDt);
                    }
                    catch (Exception ex)
                    {
                        log.Write("批量插入数据出错");
                        throw ex;
                    }
                }
                if (executeSqlList.Count > 0)
                {
                    //每次执行额定条数
                    int schemeCount = 200;
                    int executeTimes = executeSqlList.Count / schemeCount;
                    StringBuilder tempSqlSb = new StringBuilder();
                    if (executeTimes == 0)
                    {
                        try
                        {
                            //不足额定条数SQL直接执行
                            for (int i = 0; i < executeSqlList.Count; i++)
                            {
                                tempSqlSb.Append(executeSqlList[i]);
                            }
                            MSSQL.Parameter[] noParamYu = { };
                            Shove.Database.MSSQL.ExecuteNonQuery(conn, tempSqlSb.ToString(), noParamYu);
                            tempSqlSb.Clear();
                        }
                        catch (Exception ex)
                        {
                            tempSqlSb.Clear();
                            log.Write("(不足" + schemeCount.ToString() + "条)更新数据库错误，请重试。ex:" + ex.ToString());
                        }
                    }
                    else
                    {
                        int yuShu = executeSqlList.Count % schemeCount;
                        for (int i = 0; i < executeTimes; i++)
                        {
                            try
                            {
                                for (int j = i * schemeCount; j < schemeCount * i + schemeCount; j++)
                                {
                                    tempSqlSb.Append(executeSqlList[j]);
                                }
                                MSSQL.Parameter[] noParam = { };

                                Shove.Database.MSSQL.ExecuteNonQuery(conn, tempSqlSb.ToString(), noParam);
                                tempSqlSb.Clear();
                            }
                            catch (Exception ex)
                            {
                                tempSqlSb.Clear();
                                log.Write("(" + schemeCount.ToString() + "条)更新数据库错误，请重试。ex:" + ex.ToString());
                            }
                        }
                        try
                        {
                            for (int i = executeTimes * schemeCount; i < executeTimes * schemeCount + yuShu; i++)
                            {
                                tempSqlSb.Append(executeSqlList[i]);
                            }
                            MSSQL.Parameter[] noParamYu = { };
                            Shove.Database.MSSQL.ExecuteNonQuery(conn, tempSqlSb.ToString(), noParamYu);
                            tempSqlSb.Clear();
                        }
                        catch (Exception ex)
                        {
                            tempSqlSb.Clear();
                            log.Write("(余数)更新数据库错误，请重试。ex:" + ex.ToString());
                        }
                    }
                }
                count = 0;
                sb.Clear();
            }
        }

            #endregion
        log.Write("开奖的彩种：" + lotteryID.ToString() + "，期号：" + isuseName + "，期号ID：" + isuseID);
        log.Write("开奖-----------------------------4");
        #region 开奖第三步
        string openAffiche = new OpenAfficheTemplates()[lotteryID];
        int schemeCountNum, quashCountNum, winCountNum, winNoBuyCountNum;
        bool isEndOpen = false;
        while (!isEndOpen)
        {
            //总方案数，处理时撤单数，中奖数，中奖但未成功数
            schemeCountNum = 0;
            quashCountNum = 0;
            winCountNum = 0;
            winNoBuyCountNum = 0;
            returnValue = 0;
            returnDescription = "";
            DataSet dsWin = null;
            P_Win(conn, ref dsWin,
                 isuseID,
                 winLotteryNumber,
                 openAffiche,
                 1,
                 true,
                 ref schemeCountNum, ref quashCountNum, ref winCountNum, ref winNoBuyCountNum,
                 ref isEndOpen,
                 ref returnValue, ref returnDescription);
            

            if ((dsWin == null) || (returnDescription != ""))
            {
                log.Write(returnDescription);
                return;
            }
            #region 自动填写高频彩种后台App开奖公告功能
            DataTable winTypesDt = new DAL.Tables.T_WinTypes().Open(conn, "Name,DefaultMoney", " LotteryID=" + lotteryID + "", "id");
            if (winTypesDt.Rows.Count > 0)
            {
                string windetails = "";
                foreach (DataRow row in winTypesDt.Rows)
                {
                    windetails += row["Name"].ToString() + "|" + row["DefaultMoney"].ToString() + "|1;";
                }
                DAL.Tables.T_Isuses t_issue = new DAL.Tables.T_Isuses();
                t_issue.WinDetail.Value = windetails.TrimEnd(';');
                if (t_issue.Update(conn, "ID =" + isuseID) < 0)
                {
                    log.Write("彩种：" + lotteryID + " 期号ID：" + isuseID + " 开奖添加开奖公告失败");
                    return;
                }
            }
            #endregion
            string message = "彩种ID：{0},开奖号码：{1},总方案 {2} 个，撤单未满员或未出票方案 {3} 个，有效中奖方案 {4} 个，中奖但未成功方案 {5} 个。本期开奖还未全部完成, 请继续操作第三步。";
            if (isEndOpen)
            {
                message = "彩种ID：{0},开奖成功，开奖号码：{1},总方案 {2} 个，撤单未满员或未出票方案 {3} 个，有效中奖方案 {4} 个，中奖但未成功方案 {5} 个。本期开奖已全部完成。";
            }
            log.Write(String.Format(message, lotteryID, winLotteryNumber, schemeCountNum, quashCountNum, winCountNum, winNoBuyCountNum));
            //PF.SendWinNotification(dsWin, conn);
        }
        #endregion
        log.Write("开奖-----------------------------5");

        conn.Close();

    }
    #endregion


    #region 派奖方法
    /// <summary>
    /// 派奖方法
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="ds"></param>
    /// <param name="isuseID"></param>
    /// <param name="winLotteryNumber"></param>
    /// <param name="openAffiche"></param>
    /// <param name="openOperatorID"></param>
    /// <param name="isEndTheIsuse"></param>
    /// <param name="schemeCount"></param>
    /// <param name="quashCount"></param>
    /// <param name="winCount"></param>
    /// <param name="winNoBuyCount"></param>
    /// <param name="isEndOpen"></param>
    /// <param name="returnValue"></param>
    /// <param name="returnDescription"></param>
    /// <returns></returns>
    private int P_Win(SqlConnection conn, ref DataSet ds, long isuseID, string winLotteryNumber, string openAffiche, long openOperatorID, bool isEndTheIsuse, ref int schemeCount, ref int quashCount, ref int winCount, ref int winNoBuyCount, ref bool isEndOpen, ref int returnValue, ref string returnDescription)
    {
        MSSQL.OutputParameter outputs = new MSSQL.OutputParameter();
        int callResult = MSSQL.ExecuteStoredProcedureWithQuery(conn, "P_Win", ref ds, ref outputs,
            new MSSQL.Parameter("IsuseID", SqlDbType.BigInt, 0, ParameterDirection.Input, isuseID),
            new MSSQL.Parameter("WinLotteryNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, winLotteryNumber),
            new MSSQL.Parameter("OpenAffiche", SqlDbType.VarChar, 0, ParameterDirection.Input, openAffiche),
            new MSSQL.Parameter("OpenOperatorID", SqlDbType.BigInt, 0, ParameterDirection.Input, openOperatorID),
            new MSSQL.Parameter("isEndTheIsuse", SqlDbType.Bit, 0, ParameterDirection.Input, isEndTheIsuse),
            new MSSQL.Parameter("SchemeCount", SqlDbType.Int, 4, ParameterDirection.Output, schemeCount),
            new MSSQL.Parameter("QuashCount", SqlDbType.Int, 4, ParameterDirection.Output, quashCount),
            new MSSQL.Parameter("WinCount", SqlDbType.Int, 4, ParameterDirection.Output, winCount),
            new MSSQL.Parameter("WinNoBuyCount", SqlDbType.Int, 4, ParameterDirection.Output, winNoBuyCount),
            new MSSQL.Parameter("isEndOpen", SqlDbType.Bit, 0, ParameterDirection.Output, isEndOpen),
            new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, returnValue),
            new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, returnDescription)
            );
        try
        {
            schemeCount = System.Convert.ToInt32(outputs["SchemeCount"]);
        }
        catch { }
        try
        {
            quashCount = System.Convert.ToInt32(outputs["QuashCount"]);
        }
        catch { }
        try
        {
            winCount = System.Convert.ToInt32(outputs["WinCount"]);
        }
        catch { }
        try
        {
            winNoBuyCount = System.Convert.ToInt32(outputs["WinNoBuyCount"]);
        }
        catch { }
        try
        {
            isEndOpen = System.Convert.ToBoolean(outputs["isEndOpen"]);
        }
        catch { }
        try
        {
            returnValue = System.Convert.ToInt32(outputs["ReturnValue"]);
        }
        catch { }
        try
        {
            returnDescription = System.Convert.ToString(outputs["ReturnDescription"]);
        }
        catch { }
        return callResult;
    }
    #endregion




    private bool IsFormatNumber(string lotid, string number)
    {
        string ss = Regex.Match("", @"(\D)").Groups[0].Value;
        if (ss.Length > 0)
        {
            return false;
        }
        bool tag = false;
        switch (lotid)
        {
            case "2":
                tag = number.Length == 8 ? true : false;
                break;
            case "3":
                tag = number.Length == 7 ? true : false;
                break;
            case "5":
                tag = number.Length == 22 ? true : false;
                break;
            case "6":
                tag = number.Length == 3 ? true : false;
                break;
            case "13":
                tag = number.Length == 25 ? true : false;
                break;
            case "15":
                tag = number.Length == 12 ? true : false;
                break;
            case "28":
                tag = number.Length == 5 ? true : false;
                break;
            case "39":
                tag = number.Length == 22 ? true : false;
                break;
            case "61":
                tag = number.Length == 5 ? true : false;
                break;
            case "62":
                tag = number.Length == 14 ? true : false;
                break;
            case "63":
                tag = number.Length == 3 ? true : false;
                break;
            case "64":
                tag = number.Length == 5 ? true : false;
                break;
            case "70":
                tag = number.Length == 14 ? true : false;
                break;
            case "74":
                tag = number.Length == 14 ? true : false;
                break;
            case "75":
                tag = number.Length == 14 ? true : false;
                break;
            case "78":
                tag = number.Length == 14 ? true : false;
                break;
            case "83":
                tag = number.Length == 3 ? true : false;
                break;
            default:
                break;
        }

        return tag;
    }

    private void OpenWin(string winLotteryNum, double[] winMoneyList)
    {
        try
        {
            #region 根据期号查询未开奖的方案信息
            string sql = string.Empty;
            
                sql = string.Format("Select Id,InitiateUserID,PreWinMoney,PreWinMoneyNoWithTax,EditWinMoney,"
                                         + "EditWinMoneyNoWithTax,LotteryNumber,WinDescription,Description from T_Schemes"
                                         + " where isOpened=0 and Buyed=1 and IsuseID={0}", ddlIsuse.SelectedValue);
            
            DataTable dtSchemes = MSSQL.Select(sql);

            if (dtSchemes == null || dtSchemes.Rows.Count == 0)
            {
                return;
            }
            #endregion

            #region 根据未开奖的方案ID查询方案详情
            if (ddlLottery.SelectedValue == "74")
            {
                sql = string.Format("SELECT ID,LotteryNumber,Multiple,PlayTypeID,SchemeId,WinMoney,WinMoneyNoWithTax,WinDescription FROM T_SchemesMixcast WHERE   SchemeId IN (SELECT  ID FROM T_Schemes WHERE   isOpened = 0 AND Buyed = 1 AND IsuseID IN (SELECT  ID FROM    dbo.T_Isuses  WHERE   Name = ( SELECT TOP 1  Name FROM   dbo.T_Isuses WHERE  ID = {0} ) AND dbo.T_Isuses.LotteryID IN ( 74, 75 ) ) )", ddlIsuse.SelectedValue);
            }
            else
            {
                sql = string.Format("Select ID,LotteryNumber,Multiple,PlayTypeID,SchemeId,WinMoney,WinMoneyNoWithTax,WinDescription from T_SchemesMixcast "
                                  + "where SchemeId in(select ID from T_Schemes where isOpened=0 and Buyed=1 and IsuseID={0})", ddlIsuse.SelectedValue);
            }
            DataTable dtMixcasts = MSSQL.Select(sql);

            if (dtMixcasts == null || dtMixcasts.Rows.Count == 0)
            {
                return;
            }
            #endregion

            //查询是否有加奖活动
            DataTable dtAddAward = MSSQL.Select("select * from T_Addaward where GETDATE() BETWEEN StartTime AND EndTime");

            if (dtAddAward == null)
            {
                //访问T_Addaward表失败
                return;
            }

            //需要更新的方案数据
            List<MySchemes> listSchemes = new List<MySchemes>();

            //T_WinMoneyDetail需要新增的数据集合
            DataTable dtAddWinDetail = new DataTable();
            dtAddWinDetail.Columns.AddRange(new DataColumn[] 
                            {                                 
                                new DataColumn("Id", typeof(long)),
                                new DataColumn("SchemesID", typeof(long)),
                                new DataColumn("PlaysID", typeof(long)),
                                new DataColumn("WinMoney", typeof(double)),
                                new DataColumn("WinMoneyNoWithTax", typeof(double))
                            });

            StringBuilder strUpdateMixcast = new StringBuilder();

            StringBuilder strUpdateScheme = new StringBuilder();
            List<WinDesc> winDescList = new List<WinDesc>();
            foreach (DataRow item in dtMixcasts.Rows)
            {
                string schemeID = item["SchemeID"].ToString();

                string description = "";

                string lotteryNumber = item["LotteryNumber"].ToString();//投注号码

                int multiple = Shove._Convert.StrToInt(item["Multiple"].ToString(), 1);//投注倍数

                string playTypeID = item["PlayTypeID"].ToString();//玩法ID

                string lotteryId = playTypeID.Substring(0, playTypeID.Length - 2);//彩种ID

                double winMoney = 0;//中奖金额

                double winMoneyNoWithTax = 0;//税后奖金

                int winCount = 0;//中奖注数

                double addWinMoney = 0;//加奖金额

                double addWinMoneyNoWithTax = 0;//加奖金额不含税

                winMoney = new SLS.Lottery()[Convert.ToInt32(lotteryId)].ComputeWin(lotteryNumber, winLotteryNum, ref description, ref winMoneyNoWithTax, int.Parse(playTypeID), ref winCount, winMoneyList);

                if (winMoney < 0)
                {
                    //奖金计算错误，不予开奖
                    continue;
                }

                #region 加奖计算
                DataRow[] drAddAward = dtAddAward.Select("LotteryID=" + lotteryId);

                if (drAddAward.Length > 0)
                {
                    string AddInfo = drAddAward[0]["AddInfo"].ToString();

                    string LotteryPlayID = item["PlayTypeID"].ToString();

                    if (string.IsNullOrEmpty(AddInfo) == false)
                    {
                        string[] list = AddInfo.Split('|');
                        for (int i1 = 0; i1 < list.Length; i1++)
                        {
                            if (list[i1].Split(',')[0].Equals(LotteryPlayID))
                            {
                                addWinMoney = double.Parse(list[i1].Split(',')[1]);
                                addWinMoneyNoWithTax = double.Parse(list[i1].Split(',')[1]);
                            }
                        }
                    }

                    addWinMoney = addWinMoney * winCount * multiple;

                    addWinMoneyNoWithTax = addWinMoneyNoWithTax * winCount * multiple;
                }
                #endregion

                #region 给dtAddWinDetail赋值
                //TODO:将新增的数据保存到DataTable
                DataRow dr = dtAddWinDetail.NewRow();
                dr["SchemesID"] = schemeID;
                dr["PlaysID"] = item["PlayTypeID"];
                dr["WinMoney"] = winMoney * multiple + addWinMoney;
                dr["WinMoneyNoWithTax"] = winMoneyNoWithTax + (multiple - 1) * winMoney + addWinMoneyNoWithTax;
                dtAddWinDetail.Rows.Add(dr);
                #endregion

                #region 给DataRow赋值
                double MixcastWinMoney = winMoney * multiple + addWinMoney;
                double MixcastWinMoneyNoWithTax = winMoneyNoWithTax + (multiple - 1) * winMoney + addWinMoneyNoWithTax;
                strUpdateMixcast.Append("update T_SchemesMixcast set WinMoney=" + MixcastWinMoney + ", WinMoneyNoWithTax=" + MixcastWinMoneyNoWithTax + ",WinDescription='" + description + "加奖：" + addWinMoney + "' where [ID] = " + item["ID"] + ";");
                #endregion

                #region 给listSchemes赋值

                MySchemes mySchemes = null;
                WinDesc winModel = new WinDesc();
                winModel.schemeId = schemeID;
                winModel.desc = description.Replace(" ", "") + "加奖：" + addWinMoney; ;
                winDescList.Add(winModel);
                if (listSchemes.Exists(itemScheme => itemScheme.SchemeID == schemeID))
                {
                    mySchemes = listSchemes.Find(itemScheme => itemScheme.SchemeID == schemeID);

                    mySchemes.PreWinMoney += winMoney * multiple + addWinMoney;

                    mySchemes.PreWinMoneyNoWithTax += winMoneyNoWithTax + (multiple - 1) * winMoney + addWinMoneyNoWithTax;

                    mySchemes.EditWinMoney = mySchemes.PreWinMoney;

                    mySchemes.EditWinMoneyNoWithTax = mySchemes.PreWinMoneyNoWithTax;

                    mySchemes.WinDescription += description;

                    mySchemes.Description += addWinMoney;

                }
                else
                {
                    mySchemes = new MySchemes();

                    mySchemes.SchemeID = schemeID;

                    mySchemes.PreWinMoney = winMoney * multiple + addWinMoney;

                    mySchemes.PreWinMoneyNoWithTax = winMoneyNoWithTax + (multiple - 1) * winMoney + addWinMoneyNoWithTax;

                    mySchemes.EditWinMoney = mySchemes.PreWinMoney;

                    mySchemes.EditWinMoneyNoWithTax = mySchemes.PreWinMoneyNoWithTax;

                    mySchemes.LotteryNumber = winLotteryNum;

                    mySchemes.WinDescription = description;

                    mySchemes.Description = addWinMoney;

                    listSchemes.Add(mySchemes);
                }
                #endregion

                
            }
            SqlConnection conn = Shove.Database.MSSQL.CreateDataConnection<SqlConnection>(Shove._Web.WebConfig.GetAppSettingsString("ConnectionString"));

            SqlTransaction sqlTran = conn.BeginTransaction();
            if (strUpdateMixcast.Length > 0)
            {
                try
                {
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter();

                    #region 更新T_SchemesMixcast

                    SqlCommand cmd = new SqlCommand(strUpdateMixcast.ToString(), conn);

                    cmd.Transaction = sqlTran;

                    int result = cmd.ExecuteNonQuery();

                    if (result < 0)
                    {
                        //更新异常，回滚事物。
                        sqlTran.Rollback();
                        throw new Exception("开奖出现异常!请联系开发维护人员。");
                    }

                    #endregion

                    #region 更新T_Schemes
                    foreach (DataRow item in dtSchemes.Rows)
                    {
                        string schemeId = item["ID"].ToString();

                        MySchemes scheme = listSchemes.Find(itemScheme => itemScheme.SchemeID == schemeId);
                        List<WinDesc> winDescListSing = winDescList.Where(p => p.schemeId == schemeId).ToList();
                        List<string> winDescTemp = new List<string>();
                        for (int winDescIndex = 0; winDescIndex < winDescListSing.Count(); winDescIndex++)
                        {
                            for (int k = 0; k < winDescListSing[winDescIndex].desc.ToString().Split('，').Length; k++)
                            {
                                winDescTemp.Add(winDescListSing[winDescIndex].desc.ToString().Split('，')[k]);
                            }
                        }
                        if (scheme != null)
                        {
                            strUpdateScheme.Append("update T_Schemes set PreWinMoney=").Append(scheme.PreWinMoney)
                                           .Append(",PreWinMoneyNoWithTax=").Append(scheme.PreWinMoneyNoWithTax)
                                           .Append(",EditWinMoney=").Append(scheme.EditWinMoney)
                                           .Append(",EditWinMoneyNoWithTax=").Append(scheme.EditWinMoneyNoWithTax)
                                           .Append(",LotteryNumber='").Append(scheme.LotteryNumber).Append("'")
                                           .Append(",WinDescription='").Append(SLS.Lottery.StatisticsWinDesc(winDescTemp)).Append("'")
                                           .Append(",Description='").Append("加奖：" + scheme.Description).Append("'")
                                           .Append(" where [ID] =").AppendLine(scheme.SchemeID + ";");
                        }
                    }

                    cmd = new SqlCommand(strUpdateScheme.ToString(), conn);

                    cmd.Transaction = sqlTran;

                    int result2 = cmd.ExecuteNonQuery();

                    if (result2 < 0)
                    {
                        //更新异常，回滚事物。
                        sqlTran.Rollback();
                        throw new Exception("开奖出现异常!请联系开发维护人员。");
                    }

                    #endregion

                    #region 批量新增中奖信息

                    if (dtAddWinDetail != null && dtAddWinDetail.Rows.Count > 0)
                    {
                        SqlBulkCopy sqlCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, sqlTran);
                        sqlCopy.BatchSize = dtAddWinDetail.Rows.Count;
                        sqlCopy.DestinationTableName = "T_WinMoneyDetail";
                        sqlCopy.WriteToServer(dtAddWinDetail);
                        sqlCopy.Close();
                    }
                    #endregion

                    sqlTran.Commit();
                }
                catch (Exception)
                {
                    sqlTran.Rollback();
                    throw new Exception("开奖出现异常!请联系开发维护人员。");
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}

class MySchemes
{
    public double PreWinMoney { get; set; }
    public double PreWinMoneyNoWithTax { get; set; }
    public double EditWinMoney { get; set; }
    public double EditWinMoneyNoWithTax { get; set; }
    public string LotteryNumber { get; set; }
    public string WinDescription { get; set; }
    public double Description { get; set; }
    public string SchemeID { get; set; }
}
class WinDesc
{
    public string schemeId { get; set; }
    public string desc { get; set; }
}
