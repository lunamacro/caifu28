using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Shove.Database;
using System.Text.RegularExpressions;

public partial class Admin_Scheme : AdminPageBase
{
    protected long SchemeID = -1;
    public string StrHMuser = "";
    public string StrNumber = "";

    public string LotteryID = "5";
    public string LotteryName;
    public int PlayTypeID;
    private string dingZhi = "";
    public string IsuseID = "";
    public string fqID = "";
    public string fqName = "";
	public string winResultDescription = "";
	
	
	
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Admin_Scheme), this.Page);

        SchemeID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);

        if (SchemeID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误(-26)", this.GetType().FullName);

            return;
        }

        if (!IsPostBack)
        {
            tbSchemeID.Text = SchemeID.ToString();
            BindData();
        }
    }

    private void BindData()
    {
        DataTable dt = new DAL.Views.V_SchemeSchedulesWithQuashed().Open("", "[id] = " + SchemeID, "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(-141)", this.GetType().BaseType.FullName);

            return;
        }
        DataRow dr = dt.Rows[0];


        #region 方案基本信息

        int Lid = Shove._Convert.StrToInt(dr["LotteryID"].ToString(), 0);

        long InitiateUserID = Shove._Convert.StrToLong(dr["InitiateUserID"].ToString(), 0);
        //if ((_User != null && InitiateUserID == _User.ID))
        //{
        //    this.btn_Single.Visible = false;
        //}

        HTScheme.InnerHtml = dr["LotteryName"].ToString() + "第" + "<span>" + dr["IsuseName"].ToString() + "</span>" + "期认购方案";

        string UserName = dr["InitiateName"].ToString();
        if (_User != null)
        {
            if (_User.ID != 1 && UserName != _User.Name)
            {
                UserName = UserName.Length > 3 ? UserName.Substring(0, 3) + "*" : UserName;
            }
        }
        else
        {
            UserName = UserName.Substring(0, 3) + "*";
        }

        bool isJoin = false;
        if (_User != null)
        {
            DataTable dt_Join = new DAL.Tables.T_BuyDetails().Open("", "UserID = " + _User.ID + " AND SchemeID = " + SchemeID, "");
            if (dt_Join != null && dt_Join.Rows.Count > 0)
            {
                isJoin = true;
            }
            else
            {
                isJoin = false;
            }
        }

        HDUserName.InnerHtml = UserName;
        fqID = dr["InitiateUserID"].ToString();
        fqName = dr["InitiateName"].ToString();

        hfID.Value = InitiateUserID.ToString();
        short SecrecyLevel = Shove._Convert.StrToShort(dr["SecrecyLevel"].ToString(), 0);
        double Money = Shove._Convert.StrToDouble(dr["Money"].ToString(), 0);
        int Share = Shove._Convert.StrToInt(dr["Share"].ToString(), 0);
        int BuyedShare = Shove._Convert.StrToInt(dr["BuyedShare"].ToString(), 0);
        double WinMoney = Shove._Convert.StrToDouble(dr["WinMoney"].ToString(), 0);
        int Multiple = Shove._Convert.StrToInt(dr["Multiple"].ToString(), 0);
        double AssureMoney = Shove._Convert.StrToDouble(dr["AssureMoney"].ToString(), 0);
        int All_QuashStatus = Shove._Convert.StrToShort(dr["QuashStatus"].ToString(), 0);
        bool Buyed = Shove._Convert.StrToBool(dr["Buyed"].ToString(), false);
        int Schedule = Shove._Convert.StrToInt(dr["Schedule"].ToString(), 0);
        double EachMoney = (Money / Share);
        hdEachMoney.Value = EachMoney.ToString("0.00");
        hidLotteryID.Value = dr["LotteryID"].ToString();
        LotteryID = hidLotteryID.Value;
        LotteryName = dr["LotteryName"].ToString();
        PlayTypeID = Shove._Convert.StrToInt(dr["PlayTypeID"].ToString(), 0);
        IsuseID = dr["IsuseID"].ToString();
        bool IsOpen = Shove._Convert.StrToBool(dr["SchemeIsOpened"].ToString(), false);//2015.3.31许振兴修改
        double WinMoneyNoWithTax = Shove._Convert.StrToDouble(dr["WinMoneyNoWithTax"].ToString(), 0);
        bool IsuseOpenedWined = false;
        bool Stop = false;
        DateTime labEndTime = Shove._Convert.StrToDateTime(dr["SystemEndTime"].ToString(), DateTime.Now.ToString());
		
		
		
		DataTable dtIsuse = dtIsuse = new DAL.Views.V_Isuses().Open("IsOpened, WinLotteryNumber,Code", "[id] = " + dr["IsuseID"].ToString(), "");

        if (dtIsuse == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(-213)", this.GetType().FullName);

            return;
        }

        if (dtIsuse.Rows.Count < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "系统错误(-220)", this.GetType().FullName);

            return;
        }
		
		
		
		
        //  SecrecyLevel 0 不保密 1 到截止 2 到开奖 3 永远
        if ((SecrecyLevel == SchemeSecrecyLevels.ToDeadline) && !Stop && ((_User == null) || ((_User != null) && (InitiateUserID != _User.ID) )))
        {
            if (isJoin)
            {
                if (dr["LotteryID"].ToString().Equals("72") || dr["LotteryID"].ToString().Equals("73"))
                {
                    DataTable DTMixcast = new DAL.Tables.T_SchemesMixcast().Open(" top 1 PlayTypeID ", "SchemeId=" + dr["ID"].ToString() + "", "");

                    //  labEndTime = Shove._Convert.StrToDateTime(GetScriptResTable(t_str), DateTime.Now.ToString());
                    StrNumber = PF.GetScriptResTable1(dr["ID"].ToString(), DTMixcast.Rows[0]["PlayTypeID"].ToString());
                    //PF.GetId7303("");
                }
                else if (dr["LotteryID"].ToString().Equals("45"))
                {
                    DataTable DTMixcast = new DAL.Tables.T_SchemesMixcast().Open(" top 1 PlayTypeID ", "SchemeId=" + dr["ID"].ToString() + "", "");
                    StrNumber = PF.GetBJDCResult(dr["ID"].ToString(), DTMixcast.Rows[0]["PlayTypeID"].ToString());
                }
                else
                {
                    StrNumber = BindNumber(PlayTypeID, LotteryName, Money,"");
                }
            }
            else
            {
                labLotteryNumber.Text = "该方案已被保密，跟单可查看投注详情";
            }
        }
        else if ((SecrecyLevel == SchemeSecrecyLevels.ToOpen) && !IsOpen && ((_User == null) || ((_User != null) && (InitiateUserID != _User.ID)  )))
        {
            labLotteryNumber.Text = "该方案已被保密，开奖后可查看投注详情";
        }
        else if ((SecrecyLevel == SchemeSecrecyLevels.Always) && ((_User == null) || ((_User != null) && (InitiateUserID != _User.ID))))
        {
            labLotteryNumber.Text = "该方案已被永久保密，无法查看投注详情";
        }
        else
        {
            DataTable DTMixcast = new DAL.Tables.T_SchemesMixcast().Open(" top 1 * ", "SchemeId=" + dr["ID"].ToString() + "", "");

            if (dr["LotteryID"].ToString().Equals("72") || dr["LotteryID"].ToString().Equals("73"))
            {


                //  labEndTime = Shove._Convert.StrToDateTime(GetScriptResTable(t_str), DateTime.Now.ToString());
                StrNumber = PF.GetScriptResTable1(dr["ID"].ToString(), DTMixcast.Rows[0]["PlayTypeID"].ToString());
                //PF.GetId7303("");
            }
            else if (dr["LotteryID"].ToString().Equals("45"))
            {
                DTMixcast = new DAL.Tables.T_SchemesMixcast().Open(" top 1 PlayTypeID ", "SchemeId=" + dr["ID"].ToString() + "", "");
                StrNumber = PF.GetBJDCResult(dr["ID"].ToString(), DTMixcast.Rows[0]["PlayTypeID"].ToString());
            }
            else
            {
                StrNumber = BindNumber(PlayTypeID, LotteryName, Money, dtIsuse.Rows[0]["WinLotteryNumber"].ToString());
            }

            if (string.IsNullOrEmpty(StrNumber))
            {
                if (labLotteryNumber.Text == "未上传")
                {
                    if (IsOpen)
                    {
                        labLotteryNumber.Text = "已截止，不能再上传！";
                        lbUploadScheme.Visible = false;
                    }

                }
                else
                {
                    float s = float.Parse(dt.Rows[0]["money"].ToString()) / float.Parse(dt.Rows[0]["Multiple"].ToString()) / 2;
                    float ss = float.Parse(_Site.SiteOptions["Opt_MaxShowLotteryNumberRows"].ToString(""));
                    if (float.Parse(dt.Rows[0]["money"].ToString()) / float.Parse(dt.Rows[0]["Multiple"].ToString()) / 2 >= float.Parse(_Site.SiteOptions["Opt_MaxShowLotteryNumberRows"].ToString("")))
                    {
                        labLotteryNumber.Text = "";
                    }
                    else
                    {
                        labLotteryNumber.Text = DTMixcast.Rows[0]["LotteryNumber"].ToString();
                    }
                }
            }
        }

        string labState = PF.SchemeState(Share, BuyedShare, Buyed, All_QuashStatus, IsOpen, WinMoney);
        this.hidSchemeState.Value = labState;
        switch (labState)
        {
            case "已中奖":
                Buyagain(Lid);
                break;
            case "未中奖":
                Buyagain(Lid);
                break;
            case "已出票":
                Buyagain(Lid);
                break;
            case "未出票":
                Buyagain(Lid);
                break;
            case "已流单":
                Buyagain(Lid);
                break;
            case "招募中":
                //this.trBuy.Visible = true;
                //this.divBuy.Visible = true;
                //this.divBuyagain.Visible = false;
                break;
            case "已撤单":
                Buyagain(Lid);
                break;
        }

        short AtTopStatus = Shove._Convert.StrToShort(dr["AtTopStatus"].ToString(), 0);
        bool AtTopApplication = (AtTopStatus != 0);

        //if (AtTopStatus == 0)
        //{
        //    cbAtTopApplication.Visible = ((All_QuashStatus == 0) && (!Buyed) && (Share > BuyedShare) && _User != null && (InitiateUserID == _User.ID));
        //    cbAtTopApplication.Checked = AtTopApplication;
        //}
        //else if (AtTopStatus == 1)
        //{
        //    cbAtTopApplication.Visible = false;
        //    labAtTop.Visible = true;
        //    labAtTop.Text = "已申请置顶";
        //}
        //else
        //{
        //    cbAtTopApplication.Visible = false;
        //    labAtTop.Visible = true;
        //    labAtTop.Text = "方案已置顶";
        //}

        //if (Share <= BuyedShare)
        //{
        //    labState = "<FONT color='red'>已满员</font>";
        //}
        //else {
        //    labState = "<FONT color='red'>招募中</font>";
        //}
        //if (Buyed)
        //{
        //    labState = "<FONT color='red'>已出票</font>";
        //}
        //else
        //{
        //    labState = "<FONT color='red'>未出票</font>";
        //}
        //if (All_QuashStatus > 0)
        //{
        //    if (All_QuashStatus == 2)
        //    {
        //        labState = "已撤单(系统撤单)";
        //    }
        //    else
        //    {
        //        labState = "已撤单";
        //    }
        //}
        //if (!Buyed && IsOpen)
        //{
        //    labState = "<FONT color='red'>已流单</font>";
        //}
        //if (WinMoneyNoWithTax == 0)
        //{

        //}

        //if (All_QuashStatus > 0)
        //{
        //    if (All_QuashStatus == 2)
        //    {
        //        labState = "已撤单(系统撤单)";
        //    }
        //    else
        //    {
        //        labState= "已撤单";
        //    }
        //}
        //else
        //{
        //    if (Buyed)
        //    {
        //       labState = "<FONT color='red'>已出票</font>";
        //    }
        //    else
        //    {
        //        if (DateTime.Now >= labEndTime)
        //        {
        //            labState = "已截止";
        //        }
        //        //if (Stop)
        //        //{
        //        //    labState= "已截止";
        //        //}
        //        else
        //        {
        //            if (Share <= BuyedShare)
        //            {
        //                labState = "<FONT color='red'>已满员</font>";
        //            }
        //            else
        //            {
        //               labState = "<font color='red'>招募中...</font>";
        //            }
        //        }
        //    }
        //}
        labSchemeNumber.Text = dr["SchemeNumber"].ToString();
        labSchemeMoney.Text = Money.ToString("0.00");
        labMultiple.Text = Multiple.ToString();
        labShare.Text = Share.ToString();
        labShareMoney.Text = EachMoney.ToString("0.00");
        lbSchemeBonus.Text = (Shove._Convert.StrToDouble(dr["SchemeBonusScale"].ToString(), 0) * 100).ToString();
        labAssureMoney.Text = (AssureMoney > 0 ? string.Format("<FONT color='red'>{0}</font> 元", AssureMoney.ToString("0.00")) : "未保底");
        labSchedule.Text = dr["Schedule"].ToString();
        lbState.Text = labState;

        

        Stop = IsuseOpenedWined = Shove._Convert.StrToBool(dt.Rows[0]["SchemeIsOpened"].ToString(), true);

        lbWinNumber.Text = dtIsuse.Rows[0]["WinLotteryNumber"].ToString();
        
        if (!dtIsuse.Rows[0]["WinLotteryNumber"].ToString().Equals(""))
        {
            if (LotteryID.Equals("98") || LotteryID.Equals("99"))
            {
                string allNum = dtIsuse.Rows[0]["WinLotteryNumber"].ToString();
                allNum = allNum.Trim();
                allNum = allNum.Replace(" ", "");
                int num1 = Convert.ToInt32(allNum.Substring(0, 1));
                int num2 = Convert.ToInt32(allNum.Substring(1, 1));
                int num3 = Convert.ToInt32(allNum.Substring(2, 1));
                int num4 = num1 + num2 + num3;
                lbWinNumber.Text = num1.ToString() + "+" + num2.ToString() + "+" + num3.ToString() + "=" + num4.ToString();
                lbWinNumber.Text = allNum;
            }

        }


        #endregion

        //#region 方案投注信息
        //labSchemeTitle.Text = dr["Title"].ToString() + "&nbsp;";
        //labSchemeDescription.Text = dr["Description"].ToString() + "&nbsp;";
        //labSchemeADUrl.Text = Shove._Web.Utility.GetUrl() + "/Home/Room/Scheme.aspx?id=" + SchemeID;
        //string StrUser = "";
        //if (_User != null)
        //{
        //    StrUser = "我的账户余额 <i class=\"col-red\" id=\"o_labBalance\">" + _User.Balance.ToString("0.00") + "</i> 元 ,";

        //}
        //StrUser += "此方案还有 <i class=\"col-red\" id=\"IShare\">" + (Share - BuyedShare) + "</i> 份可以认购,";

        //StrHMuser = StrUser;
        //#endregion

        #region 方案认购信息

        dt = MSSQL.Select("select U.Name,B.Share,B.DetailMoney,B.DateTime from T_BuyDetails as B inner join T_Users as U on B.UserID=U.ID  where B.schemeID=" + SchemeID + "");
        if ((dt == null) || (dt.Rows.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(-141) 方案认购信息", this.GetType().BaseType.FullName);

            return;
        }
        StringBuilder sb = new StringBuilder();
        int UserCount = dt.Rows.Count;
        lbUser.Text = UserCount.ToString();
        sb.Append("<table class=\"user-list\" cellspacing=\"0\" >");
        string Name = "";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            int cutindex = int.Parse(_Site.SiteOptions["Opt_UserControlHiedAndShow"].Value.ToString());
            if (cutindex <= 0)
            {
                Name = dt.Rows[i]["Name"].ToString();
            }
            else
            {
                Name = dt.Rows[i]["Name"].ToString().Substring(0, cutindex) + "***";
            }
            if (i % 2 == 0)
            {

                sb.Append("<td style=\"width:70px\">" + Name + "</td><td style=\"width:60px\"><i class=\"col-red\">" + dt.Rows[i]["Share"].ToString() + "</i> 份</td><td style=\"width:80px\"><i class=\"col-red\">" + Shove._Convert.StrToDouble(dt.Rows[i]["DetailMoney"].ToString(), 0).ToString("0.00") + "</i>元</td><td style=\"width:110px\">" + dt.Rows[i]["DateTime"].ToString() + "</td><td class=\"split\">&nbsp;</td>");
            }
            else
            {
                sb.Append("<td style=\"width:70px\">" + Name + "</td><td style=\"width:60px\"><i class=\"col-red\">" + dt.Rows[i]["Share"].ToString() + "</i> 份</td><td style=\"width:80px\"><i class=\"col-red\">" + Shove._Convert.StrToDouble(dt.Rows[i]["DetailMoney"].ToString(), 0).ToString("0.00") + "</i>元</td><td style=\"width:110px\">" + dt.Rows[i]["DateTime"].ToString() + "</td></tr>");
            }

        }
        if (UserCount % 2 != 0)
        {
            sb.Append("<td style=\"width:70px\"></td><td style=\"width:60px\"><i class=\"col-red\"></i></td><td style=\"width:80px\"><i class=\"col-red\"></i></td><td style=\"width:80px\"></td></tr>");
        }
        sb.Append("</table>");
        UserList.InnerHtml = sb.ToString();

        if (All_QuashStatus > 0)
        {
            if (All_QuashStatus == 2)
            {
                labWin.Text = "已撤单(系统撤单)";
            }
            else
            {
                labWin.Text = "已撤单";
            }
        }
        else
        {
            if (Stop)
            {
                labWin.Text = string.Format("<FONT color='red'>{0}</font> 元", WinMoney.ToString("0.00"));
                string WinDescription = dr["WinDescription"].ToString();

                if (WinDescription != "")
                {
                    labWin.Text += "<br />" + WinDescription;
					if (winResultDescription.Length > 0)
                    {
                        labWin.Text = labWin.Text + "<br>中奖描述：" + winResultDescription;
                    }
                }
                else
                {
                    if (IsuseOpenedWined)
                    {
                        labWin.Text += "  已开奖";

                    }
                    else
                    {
                        labWin.Text += "  <font color='red'>【注】</font>中奖结果在开奖后需要一段时间才能显示。";
                    }
                }
            }
            else
            {
                labWin.Text = "尚未截止";
            }
        }

        if (_User != null)
        {
            DataTable dtMyBuy = new DAL.Views.V_BuyDetailsWithQuashedAll().Open("[id],[DateTime],[Money],Share,SchemeShare,BuyedShare,QuashStatus,Buyed,IsuseID,Code,Schedule,DetailMoney,isWhenInitiate, WinMoneyNoWIthTax", "SiteID = " + _Site.ID.ToString() + " and SchemeID = " + SchemeID + " and [UserID] = " + _User.ID.ToString(), "[id]");

            if (dtMyBuy == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(-518)", this.GetType().FullName);

                return;
            }

            if (dtMyBuy.Rows.Count == 0)
            {
                labMyBuy.Text = "此方案还没有我的认购记录。";
                labMyBuy.Visible = true;

                g.Visible = false;
            }
            else
            {
                labMyBuy.Visible = false;

                g.Visible = true;

                PF.DataGridBindData(g, dtMyBuy);

                if (IsuseOpenedWined)
                {
                    double DetailMoney = 0;

                    for (int i = 0; i < dtMyBuy.Rows.Count; i++)
                    {
                        DetailMoney += double.Parse(dtMyBuy.Rows[i]["WinMoneyNoWIthTax"].ToString());
                    }

                    lbReward.Text = DetailMoney.ToString("0.00");
                }

            }
        }


        //绑定中奖积分
        DataTable DTScoring = null;

        if (_User != null)
        {
            DTScoring = new DAL.Tables.T_UserScoringDetails().Open(" isnull(sum(Scoring),0) as Scoring ", "SchemeID=" + SchemeID + " and UserID=" + _User.ID + "", "");
        }
        else
        {
            DTScoring = new DAL.Tables.T_UserScoringDetails().Open(" isnull(sum(Scoring),0) as Scoring ", "SchemeID=" + SchemeID + "", "");
        }

        if (DTScoring.Rows.Count > 0)
        {
            lbScoring.InnerHtml = DTScoring.Rows[0]["Scoring"].ToString();
        }
        #endregion
    }

    public string BindNumber(int playTypeid, string lotteryName, double Money, String WinNumber)
    {
        StringBuilder sb = new StringBuilder();
        DataTable DT = new DAL.Views.V_SchemeMixcast().Open("", "SchemeId=" + SchemeID + "", "ID");
        LotteryName = lotteryName;
        int MaxShowLotteryNumberRows = _Site.SiteOptions["Opt_MaxShowLotteryNumberRows"].ToShort(0);
        
        string str = "<tr><th align=\"center\">玩法</th><th align=\"center\">号码</th><th align=\"center\">注数</th><th align=\"center\">金额</th></tr>";
        if (DT == null || DT.Rows.Count <= 0)
        {
            //PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(-519)", this.GetType().FullName);

            //return"";

            labLotteryNumber.Text = "未上传";

            lbUploadScheme.Visible = true;

            DataTable dtPrepareBet = new DAL.Tables.T_PrepareBet().Open("", "SchemeID=" + SchemeID.ToString(), "");

            if (dtPrepareBet == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据访问错误(-364)", this.GetType().FullName);

                return "";
            }

            if (dtPrepareBet.Rows.Count < 1)
            {
                hidMaxMoney.Value = Money.ToString();
            }
            else
            {
                hidMaxMoney.Value = dtPrepareBet.Rows[0]["MaxMoney"].ToString();
            }
        }

        else
        {
            if (DT.Rows.Count < MaxShowLotteryNumberRows)
            {
                sb.Append("<table>");
                sb.Append(str);
                foreach (DataRow DR in DT.Rows)
                {
                    string playName = DR["PlayName"].ToString();
                    PlayTypeID = Shove._Convert.StrToInt(DR["PlayTypeID"].ToString(), 0);
                    string playMoney = DR["Money"].ToString();
                    if (PlayTypeID == 2803 || PlayTypeID == 6103)
                    {
                        int starLevel = Regex.Matches(DR["LotteryNumber"].ToString(), @"-").Count;
                        if (starLevel == 1)
                        {
                            playName = "四星直选";
                        }
                        else if (starLevel == 2)
                        {
                            playName = "三星直选";
                        }
                        else if (starLevel == 3)
                        {
                            playName = "二星直选";
                        }
                        else if (starLevel == 4)
                        {
                            playName = "一星直选";
                        }
                        else
                        {
                            playName = "五星直选";
                        }
                    }
					
					String[] weiNum = { "万位", "千位", "百位", "十位", "个位" };
                    String[] weiNumqw = { "冠军", "亚军", "季军", "第四名", "第五名" };
                    String[] weiNumhw = { "第六名", "第七名", "第八名", "第九名", "第十名" };
                    String LotteryNumber = DR["LotteryNumber"].ToString();
                    //时时彩定位胆处理
                    if (PlayTypeID == 2855 || PlayTypeID == 6655 || PlayTypeID == 9455 || PlayTypeID == 9456 || PlayTypeID == 10055)
                    {

                        string RegexString;

                        if (PlayTypeID == 2855 || PlayTypeID == 6655 || PlayTypeID == 10055)
                        {
                            RegexString = @"^(?<L0>([\d-])|([(][\d]+?[)]))(?<L1>([\d-])|([(][\d]+?[)]))(?<L2>([\d-])|([(][\d]+?[)]))(?<L3>([\d-])|([(][\d]+?[)]))(?<L4>([\d-])|([(][\d]+?[)]))";
                        }
                        else
                        {
                            RegexString = @"^(?<L0>([-])|(\d\d\s){0,9}\d\d)[,](?<L1>([-])|(\d\d\s){0,9}\d\d)[,](?<L2>([-])|(\d\d\s){0,9}\d\d)[,](?<L3>([-])|(\d\d\s){0,9}\d\d)[,](?<L4>([-])|(\d\d\s){0,9}\d\d)";
                        }
                        Regex regex = new Regex(RegexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        Match m = regex.Match(LotteryNumber);

                        LotteryNumber = "";
                        for (int j = 0; j < 5; j++)
                        {
                            String single = m.Groups["L" + j.ToString()].ToString().Trim();
                            String weiString = weiNum[j];
                            if (PlayTypeID == 9455)
                            {
                                weiString = weiNumqw[j];
                            }
                            else if (PlayTypeID == 9456)
                            {
                                weiString = weiNumhw[j];
                            }
                            if (single.Equals("-"))
                            {
                                single = "未选择";
                            }

                            //中奖号码验证
                            if (WinNumber.Length >= 5)
                            {
                                if (PlayTypeID == 2855 || PlayTypeID == 6655 || PlayTypeID == 10055)
                                {
                                    String singleWin = WinNumber.Substring(j, 1);
                                    if (single.IndexOf(singleWin) >= 0)
                                    {
                                        winResultDescription += weiNum[j] + " 中奖号码 - " + singleWin + "&nbsp;&nbsp;";
                                    }

                                    single = single.Replace(singleWin, " <b><span style='color:Red;'>" + singleWin + "</span></b> ");
                                }
                                else
                                {
                                    string[] WinNumArry;
                                    if (PlayTypeID == 9455)
                                    {

                                        WinNumArry = WinNumber.Substring(0, 14).Split(' ');
                                        if (single.IndexOf(WinNumArry[j].ToString()) >= 0)
                                        {
                                            winResultDescription += weiNumqw[j] + " 中奖号码 - " + WinNumArry[j].ToString() + "&nbsp;&nbsp;";
                                        }
                                    }
                                    else
                                    {
                                        WinNumArry = WinNumber.Substring(15).Split(' ');
                                        if (single.IndexOf(WinNumArry[j].ToString()) >= 0)
                                        {
                                            winResultDescription += weiNumhw[j] + " 中奖号码 - " + WinNumArry[j].ToString() + "&nbsp;&nbsp;";
                                        }
                                    }
                                    single = single.Replace(WinNumArry[j], " <b><span style='color:Red;'>" + WinNumArry[j] + "</span></b> ");
                                }
                            }

                            LotteryNumber += weiString + "：" + single + "<br>";

                        }

                    }
					
					
                    sb.Append("<tr><td>");
                    if (!DR["HomeIndex"].ToString().Equals(""))
                    {
                        if (playTypeid > 9800 && playTypeid < 9999)
                        {
                            int hall = (int)DR["HomeIndex"];
                            string hallName = "";
                            switch (hall)
                            {
                                case 0:
                                    hallName = "玩法一：";
                                    break;
                                case 1:
                                    hallName = "玩法二：";
                                    break;
                                case 2:
                                    hallName = "玩法三：";
                                    break;
                            }
                            sb.Append(hallName);
                        }
                    }
                    sb.Append(playName + "</td><td>" + LotteryNumber + "</td><td>" + DR["InvestNum"].ToString() + "</td><td>" + playMoney.Substring(0, playMoney.IndexOf('.')) + "</td></tr>");
                }
                sb.Append("</table>");
            }
            else
            {
                linkDownloadScheme.Visible = true;
                linkDownloadScheme.NavigateUrl = "../Home/Web/DownloadSchemeFile.aspx?id=" + tbSchemeID.Text;
            }
        }
        return sb.ToString();
    }

    #region  我的认购记录

    protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (e.CommandName == "QuashBuy")
        {
            //double Opt_Betting_ForbidenCancel_Percent = Shove._Convert.StrToDouble(new SystemOptions()["Betting_ForbidenCancel_Percent"].Value.ToString(), 0)*100;
            double Opt_Betting_ForbidenCancel_Percent = Shove._Convert.StrToDouble(_Site.SiteOptions["TheTopScheduleOfInitiateCanQuashScheme"].Value.ToString(), 0) * 100;

            //if (Opt_Betting_ForbidenCancel_Percent > 0 && (Shove._Convert.StrToDouble(labSchedule.Text, -1) + Shove._Convert.StrToDouble(hidAssureMoney.Value, 1) * 100 / Shove._Convert.StrToDouble(labSchemeMoney.Text, 1)) >= Opt_Betting_ForbidenCancel_Percent)
            //{
            //    Shove._Web.JavaScript.Alert(this.Page, "对不起，由于在使用保底的情况下本方案进度已经达到 " + Opt_Betting_ForbidenCancel_Percent.ToString("0.00") + "%，即将满员，不允许撤单。");

            //    return;
            //}

            //long BuyDetailID = Shove._Convert.StrToLong(e.Item.Cells[12].Text, 0);
            long BuyDetailID = long.Parse(Shove._Web.Utility.GetRequest("id"));

            if (BuyDetailID < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误(-694)", this.GetType().FullName);

                return;
            }
            //查询当前用户当前彩种当前期数已撤销次数
            DataTable numberDt = MSSQL.Select("SELECT COUNT(ID) FROM dbo.T_Schemes WHERE IsuseID=" + e.Item.Cells[8].Text + " AND QuashStatus>0 AND dbo.T_Schemes.InitiateUserID=" + _User.ID.ToString());
            int quashNumber = Convert.ToInt32(numberDt.Rows[0][0]);
            if (quashNumber >= Convert.ToInt32(_Site.SiteOptions["Opt_QuashSchemeMaxNum"].ToString("")))
            {
                Shove._Web.JavaScript.Alert(this.Page, "撤销失败！撤销次数已达系统限制的同一彩票种类撤销次数(允许撤销次数：" + _Site.SiteOptions["Opt_QuashSchemeMaxNum"].ToString("") + "次)。");
                return;
            }

            string ReturnDescription = "";

            if (_User.QuashScheme(BuyDetailID, false, ref ReturnDescription) < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription + "(-703)", this.GetType().FullName);
                Shove._Web.JavaScript.Alert(this.Page, "撤销失败！");
                return;
            }

            Shove._Web.JavaScript.Alert(this.Page, "撤销成功！");

            BindData();

            // Shove._Web.Cache.ClearCache("Home_Room_CoBuy_BindDataForType" + tbIsuseID.Text);
            //Shove._Web.Cache.ClearCache("Home_Room_SchemeAll_BindData" + tbIsuseID.Text);

            return;
        }
    }
    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            e.Item.Cells[0].Text = "<font color='red'>" + e.Item.Cells[0].Text + "</font> 份";

            double money;
            money = Shove._Convert.StrToDouble(e.Item.Cells[1].Text, 0);
            e.Item.Cells[1].Text = "<font color='red'>" + ((money == 0) ? "0.00" : money.ToString("0.00")) + "</font> 元";

            e.Item.Cells[2].Text = e.Item.Cells[8].Text + e.Item.Cells[9].Text + e.Item.Cells[12].Text;

            short QuashStatus = Shove._Convert.StrToShort(e.Item.Cells[6].Text, 0);
            bool Buyed = Shove._Convert.StrToBool(e.Item.Cells[7].Text, false);
            bool Stop = false; // Shove._Convert.StrToBool(tbStop.Text, false);
            double Schedule = Shove._Convert.StrToDouble(e.Item.Cells[11].Text, 0);
            int SchemeShare = Shove._Convert.StrToInt(e.Item.Cells[14].Text, 0);
            int BuyedShare = Shove._Convert.StrToInt(e.Item.Cells[10].Text, 0);

            Button btnQuashBuy = ((Button)e.Item.Cells[5].FindControl("btnQuashBuy"));

            if (QuashStatus > 0)
            {
                btnQuashBuy.Enabled = false;
                e.Item.Cells[4].Text = "<font color=\'Red\'>" + this.hidSchemeState.Value + "</font>";
            }
            else
            {
                e.Item.Cells[4].Text = "<font color=\'Red\'>" + this.hidSchemeState.Value + "</font>";

                btnQuashBuy.Enabled = false;

                bool isWhenInitiate = Shove._Convert.StrToBool(e.Item.Cells[13].Text, true);
                bool isFull = (SchemeShare <= BuyedShare);
                Double TheTopScheduleOfInitiateCanQuashScheme = Shove._Convert.StrToDouble(_Site.SiteOptions["TheTopScheduleOfInitiateCanQuashScheme"].ToString(""), 0);
                if (isWhenInitiate && (TheTopScheduleOfInitiateCanQuashScheme > (Schedule / 100)))
                {
                    btnQuashBuy.Enabled = true;
                }
                else
                {
                    btnQuashBuy.Enabled = false;
                }

            }
        }
    }
    #endregion

    /// <summary>
    /// 是否显示再次购买该方案
    /// </summary>
    /// <param name="lotteryId"></param>
    private void Buyagain(int lotteryId)
    {
        //this.trBuy.Visible = false;
        //this.divBuy.Visible = false;
        //if (lotteryId == 72 || lotteryId == 73 || lotteryId == 74 || lotteryId == 75 || lotteryId == 2 || lotteryId == 15) //数字彩以及高频彩再次购买该方案
        //{
        //    this.divBuyagain.Visible = false;
        //}
        //else
        //{
        //    this.divBuyagain.Visible = true;
        //}
    }

    /// <summary>
    /// 用于验证是否是管理员
    /// </summary>
    /// <param name="e"></param>
    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.ChaseAndPackageQuery, Competences.SchemeQuery, Competences.UserAccountDetails);

        base.OnInit(e);
    }

}