using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// NewPc28Api 的摘要说明
/// </summary>
public class NewPc28Api
{
    Log log = new Log("AppGateway_API");

    StackTrace stackTrace;
    StackFrame stackFrame;
    string typeName;
    long timeStamp = 0;

    //分页的数据条数
    int pageSize = 20;

	public NewPc28Api()
	{
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        timeStamp = (long)(DateTime.Now - startTime).TotalSeconds; // 相差秒数
	}



    //获取用户余额和彩金
    public string getUserBalance(HttpContext context, Users _User)
    {
        StringBuilder sb = new StringBuilder();
        
        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"\",");
        sb.Append("\"timeStamp\":\"" + timeStamp + "\",");
        sb.Append("\"Balance\":\"" + Shove._Convert.StrToDouble(_User.Balance.ToString(), 0).ToString("0.00") + "\",");
        sb.Append("\"HandselAmount\":\"" + Shove._Convert.StrToDouble(_User.HandselAmount.ToString(), 0).ToString("0.00") + "\"");
        sb.Append("}");
        return sb.ToString();
    }

    //购买彩票
    public string Buy(HttpContext context, string strInfo)
    {
        double Price = 10;
        string NumberContent = string.Empty;
        StringBuilder sb = new StringBuilder();
        BuyParameter buyParameter = new BuyParameter();
        try
        {
            buyParameter = (BuyParameter)JsonConvert.DeserializeObject(strInfo, typeof(BuyParameter));
        }
        catch
        {
            return buildCallBack(false, "投注参数异常，不能转换");
        }
        //补充一些默认值
        if (buyParameter.OpenUserList == null)
        {
            buyParameter.OpenUserList = "";
        }
        if (buyParameter.LimitMoney == null)
        {
            buyParameter.LimitMoney = 0.0;
        }
        long UserID = Shove._Convert.StrToLong(buyParameter.uid, -1);
        Sites _Site = new Sites()[1];
        if (UserID < 1)
        {
            stackTrace = new StackTrace(true);
            stackFrame = stackTrace.GetFrame(0);
            typeName = HttpContext.Current.Request.Url.AbsolutePath;

            return buildCallBack(false, "异常用户信息");
        }

        Users _User = new Users(1)[1, UserID];
        if (_User == null)
        {
            stackTrace = new StackTrace(true);
            stackFrame = stackTrace.GetFrame(0);
            typeName = HttpContext.Current.Request.Url.AbsolutePath;

            return buildCallBack(false, "异常用户信息");
        }
        //订单锁处理,打开订单锁
        bool orderLockState = PF.OrderLock(_User.ID.ToString(), _User.Name, false, "");
        if (!orderLockState)
        {
            return buildCallBack(false, "订单在处理中，请不要重复提交订单。");
        }


        if (buyParameter.BuyShare < 1)
        {
            buyParameter.BuyShare = buyParameter.TotalShare;
        }

        if (buyParameter.AssureMoney < 0)
        {
            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "保底金额有误");
            return buildCallBack(false, "保底金额有误。");
        }

        if (buyParameter.TotalShare < 1)
        {
            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "方案总份数有误");
            return buildCallBack(false, "TotalShare方案总份数有误。");
        }

        if ((buyParameter.BuyShare == buyParameter.TotalShare) && (buyParameter.AssureMoney == 0))
        {
            buyParameter.TotalShare = 1;
            buyParameter.BuyShare = 1;
        }

        if ((buyParameter.SumMoney / buyParameter.TotalShare) < 1)
        {
            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "每份金额最低不能少于1元");
            return buildCallBack(false, "每份金额最低不能少于1元");
        }
        string LotteryNumber = buyParameter.SchemeContent;

        if (LotteryNumber.Length != 0 && LotteryNumber[LotteryNumber.Length - 1] == '\n')
        {
            LotteryNumber = LotteryNumber.Substring(0, LotteryNumber.Length - 1);
        }
        double BuyMoney = buyParameter.BuyShare * (buyParameter.SumMoney / buyParameter.TotalShare) + buyParameter.AssureMoney;

        if ((BuyMoney > _User.Balance + _User.HandselAmount && buyParameter.IsChase.Equals("0")) || (buyParameter.ChaseSumMoney > _User.Balance + _User.HandselAmount && buyParameter.IsChase.Equals("1")))
        {
            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "用户账户余额不足");
            return buildCallBack(false, "用户账户余额不足，请充值");
        }



        if (BuyMoney > PF.SchemeMaxBettingMoney)
        {
            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "投注金额不能大于" + PF.SchemeMaxBettingMoney.ToString());
            return buildCallBack(false, "投注金额不能大于" + PF.SchemeMaxBettingMoney.ToString() + "，谢谢");
        }




        /*
        if (buyParameter.HomeIndex.Equals("1") && BuyMoney < 50)
        {
            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "玩法二投注不能小于50积分");
            return buildCallBack(false, "玩法二投注不能小于50积分");
        }

        if (buyParameter.HomeIndex.Equals("2") && BuyMoney < 500)
        {
            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "玩法三投注不能小于500积分");
            return buildCallBack(false, "玩法三投注不能小于500积分");
        }
        */




        #region 对彩票号码进行分析，判断注数
        //先发起后上传则跳过此分析
        if (buyParameter.isNullBuyContent == 1)
        {
            goto lab_NullBuyContent;
        }

        SLS.Lottery slsLottery = new SLS.Lottery();
        string[] t_lotterys = SplitLotteryNumber(LotteryNumber);

        if ((t_lotterys == null) || (t_lotterys.Length < 1))
        {
            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "选号异常");
            return buildCallBack(false, "彩票号码进行分析发生异常" + LotteryNumber);
        }

        int TotalValidNum = 0;//总注数
        int ValidNum = 0;
        double MixcastMoney = 0;

        /*
         *   玩法                                  格式
         *   601,602,603,605,607,608,609,610       0,1,2|玩法|单倍金额|注数
         *   604,606  那个位没有数值默认为*例如：  *,1,2|玩法|单倍金额|注数 
         *   613~616                       例如:   大|玩法|单倍金额|注数
         */

        StringBuilder sbXML = new StringBuilder();
        sbXML.Append("<?xml version = \"1.0\" encoding = \"gb2312\"?>");
        sbXML.Append("<Mixcasts>");
        foreach (string str in t_lotterys)
        {
            //获得号码和玩法
            string[] NumberInfo = str.Split('|');
            if (NumberInfo.Length < 4)
            {
                continue;
            }
            // buyParameter.PlayTypeID = int.Parse(NumberInfo[1]);

            string Number = string.Empty;
            try
            {
                if (NumberInfo[1].Equals("9902") || NumberInfo[1].Equals("9802"))
                {
                    NumberInfo[0] = NumberInfo[0].ToString().PadLeft(2, '0');
                }
                Number = slsLottery[buyParameter.LotteryID].AnalyseScheme(NumberInfo[0], int.Parse(NumberInfo[1]));
            }
            catch
            {
                //关闭订单锁
                PF.OrderLock(_User.ID.ToString(), _User.Name, false, "选号异常");
            }

            if (string.IsNullOrEmpty(Number))
            {
                continue;
            }

            string[] str_s = Number.Split('|');

            if (str_s == null || str_s.Length < 1)
            {
                continue;
            }

            ValidNum = Shove._Convert.StrToInt(str_s[str_s.Length - 1], 0);

            //计算投注金额是否正确
            double moeny = Shove._Convert.StrToDouble(NumberInfo[2], 0);
            if (ValidNum * Price * buyParameter.Multiple == moeny * buyParameter.Multiple || (buyParameter.LotteryID == 99 || buyParameter.LotteryID == 98))
            {
                MixcastMoney += moeny * buyParameter.Multiple;

                sbXML.Append("<Mixcast PlayTypeID=\"" + NumberInfo[1] + "\" LotteryNumber=\"" + NumberInfo[0] + "\" Money=\"" + moeny * buyParameter.Multiple + "\" Multiple=\"" + buyParameter.Multiple + "\" InvestNum=\"" + NumberInfo[3] + "\" ></Mixcast>");

            }

            TotalValidNum += ValidNum;
        }



        sbXML.Append("</Mixcasts>");
        NumberContent = sbXML.ToString();
        
        #endregion

        if (buyParameter.SumNum != TotalValidNum)
        {
            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "选号异常");
            return buildCallBack(false, "!buyParameter.SumNum.Equals(TotalValidNum.ToString())");
        }

        string AdditionasXml = "";
        string ReturnDescription = "";

        #region 追号
        if (buyParameter.IsChase.Equals("1"))
        {
            if (buyParameter.SumMoney < 2)
            {
                //关闭订单锁
                PF.OrderLock(_User.ID.ToString(), _User.Name, false, "方案总金额有误");
                return buildCallBack(false, "追号方案总金额有误。");
            }

            //strChaseContent 格式：期号ID,倍数,金额
            string[] XML = buyParameter.ChaseContent.Split(';');
            int CompetitionCount = XML.Length;

            string[] Xmlparams = new string[CompetitionCount * 6];

            string str_EndTime = DAL.Functions.F_GetIsuseSystemEndTime(long.Parse(XML[0].Split(',')[0]), buyParameter.PlayTypeID).ToString();
            DateTime EndTime = DateTime.Parse(str_EndTime);

            if (DateTime.Now >= EndTime)
            {
                //关闭订单锁
                PF.OrderLock(_User.ID.ToString(), _User.Name, false, "追号期号中包含已截止的期");
                return buildCallBack(false, "您选择的追号期号中包含已截止的期，请重新选择。");
            }

            ///*追号注数*/
            //string[] MultipleList = strChaseContent.Split(';');
            //int tags = 0;

            //构建格式：期号,玩法类别,方案,倍数,金额,方案保密级别
            double tempSumMoney = 0;
            for (int i = 0; i < CompetitionCount; i++)
            {
                StringBuilder XMLsb = new StringBuilder();
                XMLsb.Append("<?xml version = \"1.0\" encoding = \"gb2312\"?>");
                XMLsb.Append("<Mixcasts>");
                foreach (string str in t_lotterys)
                {
                    //获得号码和玩法
                    string[] NumberInfos = str.Split('|');
                    if (NumberInfos.Length < 4)
                    {
                        continue;
                    }
                    buyParameter.PlayTypeID = int.Parse(NumberInfos[1]);
                    string Number = string.Empty;
                    try
                    {
                        Number = slsLottery[buyParameter.LotteryID].AnalyseScheme(NumberInfos[0], int.Parse(NumberInfos[1]));
                    }
                    catch
                    {
                        //关闭订单锁
                        PF.OrderLock(_User.ID.ToString(), _User.Name, false, "选号异常");
                    }
                    if (string.IsNullOrEmpty(Number))
                    {
                        continue;
                    }

                    string[] str_s = Number.Split('|');

                    if (str_s == null || str_s.Length < 1)
                    {
                        continue;
                    }


                    XMLsb.Append("<Mixcast PlayTypeID=\"" + buyParameter.PlayTypeID.ToString() + "\" LotteryNumber=\"" + NumberInfos[0] + "\" Money=\"" + XML[i].Split(',')[2] + "\" Multiple=\"" + XML[i].Split(',')[1] + "\" InvestNum=\"" + NumberInfos[3] + "\" ></Mixcast>");

                }
                //tags++;
                XMLsb.Append("</Mixcasts>");
                //LotteryNumber = XMLsb.ToString();
                LotteryNumber = "";

                Xmlparams[i * 6] = XML[i].Split(',')[0];        //期号
                Xmlparams[i * 6 + 1] = buyParameter.PlayTypeID.ToString();   //玩法类别
                Xmlparams[i * 6 + 2] = LotteryNumber;           //方案
                Xmlparams[i * 6 + 3] = XML[i].Split(',')[1];    //倍数
                Xmlparams[i * 6 + 4] = XML[i].Split(',')[2];    //金额
                Xmlparams[i * 6 + 5] = buyParameter.SecrecyLevel.ToString();

                if (Shove._Convert.StrToDouble(Xmlparams[i * 6 + 3], 0) * buyParameter.SumMoney != Shove._Convert.StrToDouble(Xmlparams[i * 6 + 4], 1))
                {
                    //关闭订单锁
                    PF.OrderLock(_User.ID.ToString(), _User.Name, false, "追号明细信息有误");
                    return buildCallBack(false, "追号明细信息有误");
                }

                if (Shove._Convert.StrToDouble(Xmlparams[i * 6 + 3], 0) < 1)
                {
                    //关闭订单锁
                    PF.OrderLock(_User.ID.ToString(), _User.Name, false, "追号明细信息有误");
                    return buildCallBack(false, "追号明细信息有误");
                }

                if (double.Parse(Xmlparams[i * 6 + 3]) * TotalValidNum * Price != double.Parse(Xmlparams[i * 6 + 4]))
                {
                    //关闭订单锁
                    PF.OrderLock(_User.ID.ToString(), _User.Name, false, "追号明细信息有误");
                    return buildCallBack(false, "追号明细信息有误");
                }

                tempSumMoney += double.Parse(Xmlparams[i * 6 + 4]);
            }

            if (buyParameter.ChaseSumMoney != tempSumMoney)
            {
                //关闭订单锁
                PF.OrderLock(_User.ID.ToString(), _User.Name, false, "追号总金额有误");
                return buildCallBack(false, "追号总金额有误。");
            }

            AdditionasXml = PF.BuildIsuseAdditionasXmlForChase(Xmlparams);

            if (AdditionasXml == "")
            {
                //关闭订单锁
                PF.OrderLock(_User.ID.ToString(), _User.Name, false, "追号明细信息有误");
                return buildCallBack(false, "AdditionasXml追号明细信息有误。");
            }

            long ChaseTaskSchemeID = -1;


            long ChaseTaskID = _User.InitiateChaseTask(
                (buyParameter.Title == "" ? "(无标题)" : buyParameter.Title),
                buyParameter.Description,
                buyParameter.LotteryID,
                buyParameter.AutoStopAtWinMoney,
                AdditionasXml,
                NumberContent,
                buyParameter.SchemeBonusScale / 100, 1,
                ref ChaseTaskSchemeID,
                ref ReturnDescription);

            if (ChaseTaskID < 0)
            {
                log.Write(ReturnDescription);

                sb.Append("{\"error\":\"" + ChaseTaskID + "\",");
                sb.Append("\"buyID\":\"-1\",");
                sb.Append("\"msg\":\"" + ReturnDescription + "\"}");

                //关闭订单锁
                PF.OrderLock(_User.ID.ToString(), _User.Name, false, ReturnDescription);
                return buildCallBack(false, ReturnDescription);
            }

            Shove._Web.Cache.ClearCache("Home_Room_CoBuy_BindDataForType" + buyParameter.IsuseID.ToString());
            Shove._Web.Cache.ClearCache("Home_Room_SchemeAll_BindData" + buyParameter.IsuseID.ToString());
            Shove._Web.Cache.ClearCache("1AccountFreezeDetail_" + _User.ID.ToString());
            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "系统异常");
            return buildCallBack(false, "BuyId=-1系统异常");
        }
        #endregion

        // 代购/合买
        if (DateTime.Now >= buyParameter.IsuseEndTime)
        {
            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "本期投注已截止");
            return buildCallBack(false, "本期投注已截止，谢谢");
        }

        if (MixcastMoney != buyParameter.SumMoney)
        {
            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "选号异常");
            return buildCallBack(false, "SumMoney选号发生异常，请重新选择号码投注");
        }
        

        lab_NullBuyContent://先发起后上传
        if (buyParameter.BuyShare +
            (buyParameter.AssureMoney / (buyParameter.SumMoney / buyParameter.TotalShare))
            > buyParameter.TotalShare)
        {
            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, "认购份数加保底份数不能大于方案总份数");
            return buildCallBack(false, "认购份数加保底份数不能大于方案总份数");
        }

        ReturnDescription = "";

        long SchemeID = 0;
        if (buyParameter.LimitMoney > 0)
        {
            if (buyParameter.LimitMoney < 2)
            {
                //关闭订单锁
                PF.OrderLock(_User.ID.ToString(), _User.Name, false, "认购份数加保底份数不能大于方案总份数");
                return buildCallBack(false, "上限值不能小于2");
            }
            SchemeID = _User.InitiateSchemeYT(
                buyParameter.IsuseID, buyParameter.PlayTypeID, (buyParameter.Title == "" ? "(无标题)" : buyParameter.Title),
                buyParameter.Description, NumberContent, buyParameter.OpenUserList,
                buyParameter.Multiple,
                buyParameter.SumMoney,
                buyParameter.AssureMoney,
                buyParameter.TotalShare,
                buyParameter.BuyShare, "",
                short.Parse(buyParameter.SecrecyLevel.ToString()),
                buyParameter.SchemeBonusScale * 0.01,
                1,
                buyParameter.LimitMoney,
                ref ReturnDescription);
        }
        else
        {
            
            SchemeID = _User.InitiateScheme(
                buyParameter.IsuseID,
                buyParameter.PlayTypeID,
                (buyParameter.Title == "" ? "(无标题)" : buyParameter.Title),
                buyParameter.Description,
                NumberContent,
                buyParameter.OpenUserList,
                buyParameter.Multiple,
                buyParameter.SumMoney,
                buyParameter.AssureMoney,
                buyParameter.TotalShare,
                buyParameter.BuyShare, "",
                short.Parse(buyParameter.SecrecyLevel.ToString()),
                buyParameter.SchemeBonusScale * 0.01, 1, ref ReturnDescription);
            
        }
        if (SchemeID < 0)
        {
            //关闭订单锁
            PF.OrderLock(_User.ID.ToString(), _User.Name, false, ReturnDescription);
            return buildCallBack(false, ReturnDescription);
        }

        Shove._Web.Cache.ClearCache("Home_Room_CoBuy_BindDataForType" + buyParameter.IsuseID.ToString());
        Shove._Web.Cache.ClearCache("Home_Room_SchemeAll_BindData" + buyParameter.IsuseID.ToString());

        if (buyParameter.SumMoney > 50 && buyParameter.TotalShare > 1)
        {
            Shove._Web.Cache.ClearCache("Home_Room_JoinAllBuy_BindData");
        }
        if (buyParameter.LotteryID == 99 || buyParameter.LotteryID == 98)
        {
            string sql = "insert into T_SchemesFrom values({0},{1},{2})";
            Shove.Database.MSSQL.ExecuteNonQuery(string.Format(sql, SchemeID, buyParameter.HomeIndex, buyParameter.VIPIndex));
        }

        //_User.SendMsg(PlayTypeID, SchemeID.ToString(), NumberContent, Multiple, SumMoney, AssureMoney, Share, BuyShare, "");

        //关闭订单锁
        PF.OrderLock(_User.ID.ToString(), _User.Name, false, "");
        //_User.Balance + _User.HandselAmount

        double[] balanceArray = GetUserBalance(_User.ID);
        double userAccount = balanceArray[0] + balanceArray[1];
        return buildCallBack(true, ""+userAccount+"");
    }

    //更改用户昵称和个签
    public string updateNick(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        Users _User = new Users(1)[1, int.Parse(uData.uid)];
        if (_User.ID < 0)
        {
            sb.Append("{\"error\":\"-109\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }

        _User.NickName = uData.NickName;
        _User.GexingQianming = uData.GexingQianming;
        string des = "";
        int result =  _User.EditByID(ref des);
        if (result < 0)
        {
            sb.Append("{\"error\":\"-110\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"数据更新失败\"}");
            return sb.ToString();
        }


        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"\",");
        sb.Append("\"timeStamp\":\"" + timeStamp + "\",");
        sb.Append("\"NickName\":\"" + _User.NickName + "\",");
        sb.Append("\"GexingQianming\":\"" + _User.GexingQianming + "\"");
        sb.Append("}");
        return sb.ToString();
    }


    //绑定手机 
    public string bindMobile(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        Users _User = new Users(1)[1, int.Parse(uData.uid)];
        if (_User.ID < 0)
        {
            sb.Append("{\"error\":\"-109\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }
        if (_User.isMobileValided)
        {
            sb.Append("{\"error\":\"-1091\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"手机已经绑定\"}");
            return sb.ToString();
        }


        _User.Mobile = uData.Mobile;
        _User.isMobileValided = true;
        string des = "";
        int result = _User.EditByID(ref des);
        if (result < 0)
        {
            sb.Append("{\"error\":\"-110\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"数据更新失败\"}");
            return sb.ToString();
        }


        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"\",");
        sb.Append("\"timeStamp\":\"" + timeStamp + "\",");
        sb.Append("\"Mobile\":\"" + _User.Mobile + "\",");
        sb.Append("\"isMobileValided\":\"True\"");
        sb.Append("}");
        return sb.ToString();
    }


    //更改提现密码
    public string tixianPass(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        Users _User = new Users(1)[1, int.Parse(uData.uid)];
        if (_User.ID < 0)
        {
            sb.Append("{\"error\":\"-109\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }
        if (_User.PasswordAdv.Length>10)
        {
            if (!_User.PasswordAdv.Equals(uData.Password))
            {
                sb.Append("{\"error\":\"-1091\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"旧提现密码输入错误\"}");
                return sb.ToString();
            }
        }


        _User._passwordadv = uData.PasswordAdv;
        string des = "";
        int result = _User.EditByID(ref des);
        if (result < 0)
        {
            sb.Append("{\"error\":\"-110\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"数据更新失败\"}");
            return sb.ToString();
        }


        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"\",");
        sb.Append("\"timeStamp\":\"" + timeStamp + "\"");
        sb.Append("}");
        return sb.ToString();
    }

    //更改登录密码
    public string changePass(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        Users _User = new Users(1)[1, int.Parse(uData.uid)];
        if (_User.ID < 0)
        {
            sb.Append("{\"error\":\"-109\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }
        
        if (!_User.Password.Equals(uData.PasswordAdv))
        {
            sb.Append("{\"error\":\"-1091\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"旧登录密码输入错误\"}");
            return sb.ToString();
        }
        


        _User._password = uData.Password;
        string des = "";
        int result = _User.EditByID(ref des);
        if (result < 0)
        {
            sb.Append("{\"error\":\"-110\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"数据更新失败\"}");
            return sb.ToString();
        }


        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"\",");
        sb.Append("\"timeStamp\":\"" + timeStamp + "\"");
        sb.Append("}");
        return sb.ToString();
    }


    //更改银行卡信息
    public string bindBank(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        Users _User = new Users(1)[1, int.Parse(uData.uid)];
        if (_User.ID < 0)
        {
            sb.Append("{\"error\":\"-109\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }

        if (!_User.PasswordAdv.Equals(uData.PasswordAdv))
        {
            sb.Append("{\"error\":\"-1091\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"提现密码输入错误\"}");
            return sb.ToString();
        }



        _User.BankName = uData.BankName;
        _User.BankCardNumber = uData.BankCardNumber;
        _User.BankAddress = uData.BankAddress;
        _User.UserCradName = uData.UserCradName;
        string des = "";
        int result = _User.EditByID(ref des);
        if (result < 0)
        {
            sb.Append("{\"error\":\"-110\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"数据更新失败\"}");
            return sb.ToString();
        }


        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"\",");
        sb.Append("\"timeStamp\":\"" + timeStamp + "\"");
        sb.Append("}");
        return sb.ToString();
    }


    //充值请求
    public string recharge(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        Users _User = new Users(1)[1, int.Parse(uData.uid)];
        if (_User.ID < 0)
        {
            sb.Append("{\"error\":\"-109\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }
        double money = double.Parse(uData.Balance);
        string PayType = uData.Level;
        string payAccount = uData.GexingQianming;
        if (money < 1)
        {
            sb.Append("{\"error\":\"-1092\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"充值金额最少为20元\"}");
            return sb.ToString();
        }


        DateTime opTime = DateTime.Now;
        string addSql = "insert into T_UserPayDetails ([SiteID],[UserID],[DateTime],[PayType],[Money],AlipayNo) values(1," + uData.uid + ",'" + opTime + "'," + PayType + "," + money + ",'" + payAccount + "')";
        int result = Shove.Database.MSSQL.ExecuteNonQuery(addSql);

        if (result < 0)
        {
            sb.Append("{\"error\":\"-110\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"数据更新失败\"}" );
            return sb.ToString();
        }


        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"\",");
        sb.Append("\"timeStamp\":\"" + timeStamp + "\"");
        sb.Append("}");
        return sb.ToString();
    }

    //提现请求
    public string tixian(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        long uid = Shove._Convert.StrToLong(uData.uid, -1);
        Users user = new Users(1)[1, uid];
        if (user == null)
        {
            sb.Append("{\"error\":\"-174\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }

        double distillMoney = 0;
        double fee = 0;
        distillMoney = Shove._Convert.StrToDouble(uData.Balance, 0);
        if (distillMoney < 20)
        {
            sb.Append("{\"error\":\"-177\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"提款金额至少为20元\"}");
            return sb.ToString();
        }

        if (distillMoney > user.Balance - PF.GetNoCash(Convert.ToInt32(user.ID)))
        {
            sb.Append("{\"error\":\"-178\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"余额不足\"}");
            return sb.ToString();
        }

        //计算银行卡提款手续费
       //待确定

        bool IsCps = false;
        string returnDescription = "";
        int Result = user.Distill(2, distillMoney, fee, user.UserCradName, user.BankName, user.BankCardNumber, "", "", "银行卡提款", IsCps, ref returnDescription, 0);
        if (Result < 0)
        {
            if (!string.IsNullOrEmpty(returnDescription))
            {
                sb.Append("{\"error\":\"-180\",\"timeStamp\":\"" + timeStamp + "\",");
                sb.Append("\"msg\":\"" + returnDescription + "\"}");
                return sb.ToString();
            }

            sb.Append("{\"error\":\"-180\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"提款失败\"}");
            return sb.ToString();
        }

        DataTable dt = new DAL.Tables.T_Users().Open("name,CAST(balance as decimal(18,2)) as balance,freeze", "ID =" + user.ID, "");
        if (dt == null)
        {
            sb.Append("{\"error\":\"-124\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"数据库读写失败\"}");
            return sb.ToString();
        }

        if (dt.Rows.Count < 1)
        {
            sb.Append("{\"error\":\"-125\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户信息读取失败\"}");
            return sb.ToString();
        }

        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"\",");
        sb.Append("\"timeStamp\":\"" + timeStamp + "\"");
        sb.Append("}");
        return sb.ToString();
    }

    //获取充值记录
    public string payStatics(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        long uid = Shove._Convert.StrToLong(uData.uid, -1);
        Users user = new Users(1)[1, uid];
        if (user == null)
        {
            sb.Append("{\"error\":\"-174\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }

        string page = uData.Level;
        StringBuilder sbData = new StringBuilder();

        DataTable dt = Shove.Database.MSSQL.Select(@"select top "+pageSize+" *  from  (select row_number() over(order by ID desc) as rownumber,* from T_UserPayDetails where [UserID]="+ uid +") A where rownumber > " + pageSize*(int.Parse(page)-1));
        if (dt.Rows.Count == 0)
        {
            sb.Append("{\"error\":\"-175\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"没有更多数据了\"}");
            return sb.ToString();
        }
        sbData.Append("[");
        int index = 0;
        foreach (DataRow row in dt.Rows)
        {
            index++;
            string jine = Convert.ToDouble(row["Money"].ToString()).ToString("0.00");
            string nDate = DateTime.Parse(row["DateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            sbData.Append("{\"ID\":\"" + row["ID"] + "\",\"sjine\":\"+" + jine + "\",\"stime\":\"" + nDate + "\",\"sstat\":\"" + getPayStatus(row["Result"].ToString()) + "\",\"sreason\":\"\"}");
            if (index < dt.Rows.Count)
            {
                sbData.Append(",");
            }
        }

        sbData.Append("]");

        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"success\",");
        sb.Append("\"data\":" + sbData.ToString() + "");
        sb.Append("}");
        return sb.ToString();

    }

    //获取提现记录
    public string distillStatics(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        long uid = Shove._Convert.StrToLong(uData.uid, -1);
        Users user = new Users(1)[1, uid];
        if (user == null)
        {
            sb.Append("{\"error\":\"-174\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }

        string page = uData.Level;
        StringBuilder sbData = new StringBuilder();

        DataTable dt = Shove.Database.MSSQL.Select(@"select top " + pageSize + " *  from  (select row_number() over(order by ID desc) as rownumber,* from T_UserDistills where [UserID]=" + uid + ") A where rownumber > " + pageSize * (int.Parse(page) - 1));
        if (dt.Rows.Count == 0)
        {
            sb.Append("{\"error\":\"-175\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"没有更多数据了\"}");
            return sb.ToString();
        }
        sbData.Append("[");
        int index = 0;
        foreach (DataRow row in dt.Rows)
        {
            index++;
            string jine = Convert.ToDouble(row["Money"].ToString()).ToString("0.00");
            string nDate = DateTime.Parse(row["DateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            string memo = row["Result"].ToString().Equals("-1") ? row["Memo"].ToString() : "";
            sbData.Append("{\"ID\":\"" + row["ID"] + "\",\"sjine\":\"-" + jine + "\",\"stime\":\"" + nDate + "\",\"sstat\":\"" + getPayStatus(row["Result"].ToString()) + "\",\"sreason\":\"" + memo + "\"}");
            if (index < dt.Rows.Count)
            {
                sbData.Append(",");
            }
        }

        sbData.Append("]");

        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"success\",");
        sb.Append("\"data\":" + sbData.ToString() + "");
        sb.Append("}");
        return sb.ToString();

    }


    //获取帐变记录
    public string ballanceStatics(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        long uid = Shove._Convert.StrToLong(uData.uid, -1);
        Users user = new Users(1)[1, uid];
        if (user == null)
        {
            sb.Append("{\"error\":\"-174\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }

        string page = uData.Level;
        StringBuilder sbData = new StringBuilder();

        DataTable dt = Shove.Database.MSSQL.Select(@"select top " + pageSize + " *  from  (select row_number() over(order by ID desc) as rownumber,* from T_UserDetails where [UserID]=" + uid + ") A where rownumber > " + pageSize * (int.Parse(page) - 1));
        if (dt.Rows.Count == 0)
        {
            sb.Append("{\"error\":\"-175\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"没有更多数据了\"}");
            return sb.ToString();
        }
        sbData.Append("[");
        int index = 0;
        foreach (DataRow row in dt.Rows)
        {
            index++;
            string jine = Convert.ToDouble(row["Money"].ToString()).ToString("0.00");
            string nDate = DateTime.Parse(row["DateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            string optype = "+";
            if (row["OperatorType"].ToString().Equals("101") || row["OperatorType"].ToString().Equals("104") || row["OperatorType"].ToString().Equals("107"))
            {
                optype = "-";
            }
            string stat = getBalanceStatus(row["OperatorType"].ToString());
            if (Double.Parse(jine) == 0 && Double.Parse(row["HandselAmount"].ToString())>0)
            {
                stat = "红包";
                optype = "+";
                jine = Convert.ToDouble(row["HandselAmount"].ToString()).ToString("0.00");
            }

            sbData.Append("{\"ID\":\"" + row["ID"] + "\",\"sjine\":\"" + optype + jine + "\",\"stime\":\"" + nDate + "\",\"sstat\":\"" + stat + "\",\"sreason\":\"\"}");
            if (index < dt.Rows.Count)
            {
                sbData.Append(",");
            }
        }

        sbData.Append("]");

        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"success\",");
        sb.Append("\"data\":" + sbData.ToString() + "");
        sb.Append("}");
        return sb.ToString();

    }

    //获取购彩记录
    public string getLotHis(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        long uid = Shove._Convert.StrToLong(uData.uid, -1);
        Users user = new Users(1)[1, uid];
        if (user == null)
        {
            sb.Append("{\"error\":\"-174\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }

        string page = uData.Level;
        string lotteryId = uData.Name;
        string starttime = uData.BankAddress+" 00:00:00";
        string endtime = uData.BankName + " 23:59:59";

        StringBuilder sbData = new StringBuilder();
        string where = " 1=1";
        if(lotteryId.Equals("98")){
            where = " (s.PlayTypeID between 9800 and 9900 )";
        }
        if (lotteryId.Equals("99"))
        {
            where = " (s.PlayTypeID between 9900 and 10000 )";
        }

        string sql = @"select top " + pageSize + " *  from  (select row_number() over(order by s.ID desc) as rownumber,s.ID,s.[DateTime],s.PlayTypeID,s.isOpened,s.WinMoney,m.LotteryNumber,m.[Money],t.Name,t.WinLotteryNumber from T_Schemes s left join T_SchemesMixcast m on s.ID=m.SchemeId left join T_Isuses t on s.IsuseID=t.ID   where s.[InitiateUserID]=" + uid + " and (s.[DateTime] BETWEEN '" + starttime + "' and '" + endtime + "') and " + where + ") A where rownumber > " + pageSize * (int.Parse(page) - 1);
        DataTable dt = Shove.Database.MSSQL.Select(sql);
        if (dt.Rows.Count == 0)
        {
            sb.Append("{\"error\":\"-175\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"没有更多数据了\"}");
            return sb.ToString();
        }
        sbData.Append("[");
        int index = 0;
        foreach (DataRow row in dt.Rows)
        {
            index++;
            string jine = Convert.ToDouble(row["Money"].ToString()).ToString("0.00");
            string win = Convert.ToDouble(row["WinMoney"].ToString()).ToString("0.00");
            string nDate = DateTime.Parse(row["DateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            string isOpen = row["isOpened"].ToString().Equals("False")?"未开奖":"已开奖";
            string lotteryName = int.Parse(row["PlayTypeID"].ToString())>9900 ? "北京幸运28" : "加拿大幸运28";


            sbData.Append("{\"ID\":\"" + row["ID"] + "\",\"lotteryName\":\"" + lotteryName + "\",\"lotteryTime\":\"" + nDate + "\",\"lotteryTouzhu\":\"" + row["LotteryNumber"].ToString() + "\",\"lotteryJine\":\"" + jine + "\",\"lotteryWin\":\"" + win + "\",\"lotteryQishu\":\"" + row["Name"].ToString() + "\",\"lotteryOpen\":\"" + isOpen + "\",\"lotteryKai\":\"" + row["WinLotteryNumber"].ToString() + "\"}");
            if (index < dt.Rows.Count)
            {
                sbData.Append(",");
            }
        }

        sbData.Append("]");

        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"success\",");
        sb.Append("\"data\":" + sbData.ToString() + "");
        sb.Append("}");
        return sb.ToString();

    }

    //获取网站公告
    public string getNotice(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        long uid = Shove._Convert.StrToLong(uData.uid, -1);
        Users user = new Users(1)[1, uid];
        if (user == null)
        {
            sb.Append("{\"error\":\"-174\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }

        string page = uData.Level;
        StringBuilder sbData = new StringBuilder();

        DataTable dt = Shove.Database.MSSQL.Select(@"select top " + pageSize + " *  from  (select row_number() over(order by ID desc) as rownumber,* from T_SiteAffiches where isShow=1) A where rownumber > " + pageSize * (int.Parse(page) - 1));
        if (dt.Rows.Count == 0)
        {
            sb.Append("{\"error\":\"-175\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"没有更多数据了\"}");
            return sb.ToString();
        }
        sbData.Append("[");
        int index = 0;
        foreach (DataRow row in dt.Rows)
        {
            index++;
            string nDate = DateTime.Parse(row["DateTime"].ToString()).ToString("yyyy-MM-dd");
            sbData.Append("{\"ID\":\"" + row["ID"] + "\",\"noticeTitle\":\"" + row["Title"].ToString() + "\",\"noticeTime\":\"" + nDate + "\"}");
            if (index < dt.Rows.Count)
            {
                sbData.Append(",");
            }
        }

        sbData.Append("]");

        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"success\",");
        sb.Append("\"data\":" + sbData.ToString() + "");
        sb.Append("}");
        return sb.ToString();

    }


    //绑定推荐人
    public string bindReffer(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        Users _User = new Users(1)[1, int.Parse(uData.uid)];
        if (_User.ID < 0)
        {
            sb.Append("{\"error\":\"-109\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }
        

        if (!_User.PasswordAdv.Equals(uData.PasswordAdv))
        {
            sb.Append("{\"error\":\"-1091\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"提现密码输入错误\"}");
            return sb.ToString();
        }

        Users _refUser = new Users(1)[1, int.Parse(uData.ReferId)];
        if (_refUser.ID < 0)
        {
            sb.Append("{\"error\":\"-129\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"此推荐人ID不存在\"}");
            return sb.ToString();
        }
        if (_refUser.isAgent == 0)
        {
            sb.Append("{\"error\":\"-130\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"此推荐人没有推广权限\"}");
            return sb.ToString();
        }



        _User.ReferId = int.Parse(uData.ReferId);
        _User.agentGroup = _refUser.agentGroup;

        string des = "";
        int result = _User.EditByID(ref des);
        if (result < 0)
        {
            sb.Append("{\"error\":\"-110\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"数据更新失败\"}");
            return sb.ToString();
        }


        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"\",");
        sb.Append("\"timeStamp\":\"" + timeStamp + "\"");
        sb.Append("}");
        return sb.ToString();
    }


    //获得推荐人和代理组信息
    public string getReffer(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        Users _User = new Users(1)[1, int.Parse(uData.uid)];
        if (_User.ID < 0)
        {
            sb.Append("{\"error\":\"-109\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }

        Users _refUser = new Users(1)[1, _User.ReferId];
        if (_refUser.ID < 0)
        {
            sb.Append("{\"error\":\"-129\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"推荐人信息异常\"}");
            return sb.ToString();
        }

        Users _groupUser = new Users(1)[1, _User.agentGroup];
        if (_groupUser.ID < 0)
        {
            sb.Append("{\"error\":\"-129\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"代理组信息异常\"}");
            return sb.ToString();
        }

        DataTable dt = Shove.Database.MSSQL.Select(@"select count(ID) from T_Users where agentGroup="+_User.agentGroup);
        int groupNum = int.Parse(dt.Rows[0][0].ToString());


        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"\",");
        sb.Append("\"referName\":\""+_refUser.NickName+"\",");
        sb.Append("\"groupName\":\"" + _groupUser.NickName + "\",");
        sb.Append("\"groupNum\":\"" + groupNum + "\",");
        sb.Append("\"timeStamp\":\"" + timeStamp + "\"");
        sb.Append("}");
        return sb.ToString();
    }


    //获得回水记录
    public string getBackWater(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        long uid = Shove._Convert.StrToLong(uData.uid, -1);
        string HomeIndex = uData.FromClient;
        Users user = new Users(1)[1, uid];
        if (user == null)
        {
            sb.Append("{\"error\":\"-174\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }

        string page = uData.Level;
        StringBuilder sbData = new StringBuilder();

        DataTable dt = Shove.Database.MSSQL.Select(@"select top " + pageSize + " *  from  (select row_number() over(order by ID desc) as rownumber,* from T_UserDetails where [UserID]=" + uid + " and HomeIndex=" + HomeIndex + " and OperatorType=14) A where rownumber > " + pageSize * (int.Parse(page) - 1));
        if (dt.Rows.Count == 0)
        {
            sb.Append("{\"error\":\"-175\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"没有更多数据了\"}");
            return sb.ToString();
        }
        sbData.Append("[");
        int index = 0;
        foreach (DataRow row in dt.Rows)
        {
            index++;
            string jine = Convert.ToDouble(row["Money"].ToString()).ToString("0.00");
            string nDate = DateTime.Parse(row["DateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            

            sbData.Append("{\"ID\":\"" + row["ID"] + "\",\"sjine\":\"" + jine + "\",\"stime\":\"" + nDate + "\",\"sstat\":\"成功\",\"sreason\":\"\"}");
            if (index < dt.Rows.Count)
            {
                sbData.Append(",");
            }
        }

        sbData.Append("]");

        sb.Append("{\"error\":\"0\",");
        sb.Append("\"msg\":\"success\",");
        sb.Append("\"data\":" + sbData.ToString() + "");
        sb.Append("}");
        return sb.ToString();

    }



    //获得充值方式列表
    public string getPayMethods(HttpContext context, string strInfo)
    {
        StringBuilder sb = new StringBuilder();

        UserInfo uData;
        try
        {
            uData = (UserInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(strInfo, typeof(UserInfo));
        }
        catch
        {
            sb.Append("{\"error\":\"-108\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"info 信息异常\"}");
            return sb.ToString();
        }

        long uid = Shove._Convert.StrToLong(uData.uid, -1);
        string HomeIndex = uData.FromClient;
        Users user = new Users(1)[1, uid];
        if (user == null)
        {
            sb.Append("{\"error\":\"-174\",\"timeStamp\":\"" + timeStamp + "\",");
            sb.Append("\"msg\":\"用户不存在\"}");
            return sb.ToString();
        }
        System.Data.DataTable dt = Shove.Database.MSSQL.Select("Select top 1 * from T_PaymentSetting");

        sb.Append("{\"error\":\"0\",");
        sb.Append("\"wxpay_name\":\"" + dt.Rows[0]["wxpay_name"] + "\",");
        sb.Append("\"wxpay_qrcode\":\"" + dt.Rows[0]["wxpay_qrcode"] + "\",");
        sb.Append("\"wxpay_min\":\"" + dt.Rows[0]["wxpay_min"] + "\",");
        sb.Append("\"wxpay_switch\":\"" + dt.Rows[0]["wxpay_switch"] + "\",");
        sb.Append("\"alipay_name\":\"" + dt.Rows[0]["alipay_name"] + "\",");
        sb.Append("\"alipay_qrcode\":\"" + dt.Rows[0]["alipay_qrcode"] + "\",");
        sb.Append("\"alipay_min\":\"" + dt.Rows[0]["alipay_min"] + "\",");
        sb.Append("\"alipay_switch\":\"" + dt.Rows[0]["alipay_switch"] + "\",");
        sb.Append("\"bank_name\":\"" + dt.Rows[0]["bank_name"] + "\",");
        sb.Append("\"bank_bank\":\"" + dt.Rows[0]["bank_bank"] + "\",");
        sb.Append("\"bank_card\":\"" + dt.Rows[0]["bank_card"] + "\",");
        sb.Append("\"bank_shoukuanren\":\"" + dt.Rows[0]["bank_shoukuanren"] + "\",");
        sb.Append("\"bank_min\":\"" + dt.Rows[0]["bank_min"] + "\",");
        sb.Append("\"bank_switch\":\"" + dt.Rows[0]["bank_switch"] + "\"");
        sb.Append("}");
        return sb.ToString();

    }








    public double[] GetUserBalance(long id) {
        double[] balanceArray = {0.00,0.00};
       DataTable dt_Users = new DAL.Tables.T_Users().Open("Balance,HandselAmount","ID="+id,"");
       if (dt_Users == null)
                {
                    new Log("System").Write("T_Users表繁忙，请稍候再读");
                    return balanceArray;
                }

        balanceArray[0] = Shove._Convert.StrToDouble(dt_Users.Rows[0]["Balance"].ToString(), 0.00);
        balanceArray[1] = Shove._Convert.StrToDouble(dt_Users.Rows[0]["HandselAmount"].ToString(),0.00);
        return balanceArray;
    }

    public string[] SplitLotteryNumber(string Number)
    {
        string[] s = Number.Split('\n');
        if (s.Length == 0)
        {
            return null;
        }
        for (int i = 0; i < s.Length; i++)
        {
            s[i] = s[i].Trim();
        }

        return s;
    }
    public string buildCallBack(bool isSuccess, string text)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{\"error\":\"" + (isSuccess ? "0" : "89") + "\",");
        sb.Append("\"msg\":\"" + text + "\"}");
        return sb.ToString();
    }

    public string getPayStatus(string stat)
    {
        switch (stat)
        {
            case "0":
                return "申请中";
                break;
            case "1":
                return "成功";
                break;
            case "-1":
                return "拒绝";
                break;
            default:
                return "未知";
        }
    }


    public string getBalanceStatus(string stat)
    {
        switch (stat)
        {
            case "101":
                return "购彩";
            case "2":
                return "中奖";
            case "104":
                return "提款冻结";
            case "6":
                return "提款冻结解除";
            case "107":
                return "提款成功";
            case "1":
                return "充值";
            default:
                return "未知";
        }
    }

    #region 模型
    public struct BuyParameter
    {
        public string uid { get; set; }
        public long IsuseID { get; set; }
        public DateTime IsuseEndTime { get; set; }
        public int PlayTypeID { get; set; }
        public int TotalShare { get; set; }
        public int BuyShare { get; set; }
        public int SecrecyLevel { get; set; }
        public string SchemeContent { get; set; }
        public double SumMoney { get; set; }
        public int SumNum { get; set; }
        public double AssureMoney { get; set; }
        public int LotteryID { get; set; }
        public int Multiple { get; set; }
        public int SchemeBonusScale { get; set; }
        public int IsChase { get; set; }
        public string ChaseContent { get; set; }
        public double ChaseSumMoney { get; set; }
        public int Cobuy { get; set; }
        public string OpenUserList { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double AutoStopAtWinMoney { get; set; }
        public int isNullBuyContent { get; set; }
        public double LimitMoney { get; set; }
        public string HomeIndex { get; set; }
        public string VIPIndex { get; set; }

    }


    //用户数据模型
    public struct UserInfo
    {
        public string accessToken { get; set; }
        public string timeStamp { get; set; }
        public string uid { get; set; }
        /// <summary>
        /// 推荐人ID
        /// </summary>
        public string Name { get; set; }
        public string Password { get; set; }
        public string PasswordAdv { get; set; }
        public string NickName { get; set; }
        public string Mobile { get; set; }
        public string HeadUrl { get; set; }
        public string GexingQianming { get; set; }
        public string Balance { get; set; }
        public string HandselAmount { get; set; }
        public string Level { get; set; }
        public string isAgent { get; set; }
        public string ReferId { get; set; }
        public string agentGroup { get; set; }
        public string isWXBind { get; set; }
        public string FromClient { get; set; }

        public string BankAddress { get; set; }
        public string BankName { get; set; }
        public string BankCardNumber { get; set; }
        public string UserCradName { get; set; }
    }




    #endregion
    

}