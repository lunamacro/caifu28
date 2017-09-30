using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Shove.Database;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using donet.io.rong.messages;
using donet.io.rong;
using donet.io.rong.models;
using SLS.AutomaticOpenLottery.Task.App_Code;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SLS.AutomaticOpenLottery.Task
{
    /// <summary>
    /// 高频彩自动开奖任务器
    /// </summary>
    public class AutoMaticOpenLotteryTask
    {
        /// <summary>
        /// 自动开奖计数器
        /// </summary>
        private long counter = 0;

        private Log LogOpenNumber
        {
            get { return new Log("GetLotteryOpenNumber"); }
        }

        private Log LogPushMsg
        {
            get { return new Log("LogPushMsg"); }
        }

        #region 配置信息
        private SqlConnection conn;
        private static Shove._IO.IniFile ini = new Shove._IO.IniFile(System.AppDomain.CurrentDomain.BaseDirectory + "Config.ini");

        private string automaticOpenLottery = ini.Read("Options", "AutomaticOpenLottery");
        private string bjkl8Url = ini.Read("Options", "BJKL8_Open_URL");
        private string jndkl8Url = ini.Read("Options", "JNDKL8_Open_URL");


        private string porxyAddress = ini.Read("Options", "PorXYAddress");
        private string bjkl8Switch = ini.Read("Options", "GP_Open_RulBJPK8");
        private string jndkl8Switch = ini.Read("Options", "GP_Open_RulJNDPK8");


        //融云IM的两个值
        private static string appKey = ini.Read("Options", "AppKey");
        private static string appSecret = ini.Read("Options", "AppSecret");
        RongCloud rongcloud = RongCloud.getInstance(appKey, appSecret);
        
        //房间数组
        String[] bjRooms = {
                "9901", "9905", "9909" };
        String[] jndRooms = {
                "9801", "9805","9809" };

        #endregion
        private System.Threading.Thread thread;
        private string conStr;
        // 0 停止 1 运行中 2 置为停止
        public int state = 0;
        public AutoMaticOpenLotteryTask()
        { }
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString"></param>
        public AutoMaticOpenLotteryTask(string connectionString)
        {
            conStr = connectionString;
            conn = Shove.Database.MSSQL.CreateDataConnection<System.Data.SqlClient.SqlConnection>(connectionString);
        }
        #endregion

        #region 启动服务
        public void Run()
        {
            // 已经启动
            if (state == 1)
            {
                return;
            }
            // 确保临界区被一个 Thread 所占用
            lock (this)
            {
                state = 1;
                counter = 0;
                thread = new System.Threading.Thread(new System.Threading.ThreadStart(Do));
                thread.IsBackground = true;
                thread.Start();
                new Log("System").Write("AutoMaticOpenLottery_Task.");
            }
        }
        #endregion

        #region 退出服务
        public void Exit()
        {
            state = 2;
        }
        #endregion

        #region 服务开始执行
        public void Do()
        {

            while (true)
            {
                if (state == 2)
                {
                    state = 0;
                    Stop();
                    return;
                }

                #region Config.ini 配置时间单位：秒, 取一次开奖号码并开奖,默认设置为30秒

                try
                {
                    //抓取开奖号码并开奖
                    GetLotteryOpenNumber();
                }
                catch (Exception e)
                {
                    new Log("System").Write("GetLotteryOpenNumber Fail:" + e.Message);
                }
                System.Threading.Thread.Sleep(10000);   // 10秒为单位

                

                /**
                if (System.DateTime.Now.Hour == 1)
                {
                    try
                    {
                        new Log("EvenIfTheRebate").Write("执行回水begin");
                        //计算回水返利
                        EvenIfTheRebate();
                        new Log("EvenIfTheRebate").Write("执行回水end");
                    }
                    catch (Exception e)
                    {
                        new Log("EvenIfTheRebate").Write("EvenIfTheRebate Fail:" + e.Message);
                    }
                }
                */
                #endregion
            }
        }
        #endregion

        #region 停止服务
        private void Stop()
        {
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }
        #endregion

       

        #region 开奖主方法
        /// <summary>
        /// 开奖主方法
        /// </summary>
        private void GetLotteryOpenNumber()
        {


            //查询自动开奖的彩种的期号
            DAL.Tables.T_Isuses tIsuses = new DAL.Tables.T_Isuses();

            if ((automaticOpenLottery == null) || (automaticOpenLottery == ""))
            {
                LogOpenNumber.Write("没有可自动开奖的彩种");

                return;
            }

            string[] lotteryIdStrArray = automaticOpenLottery.Split(',');
            if (lotteryIdStrArray.Length == 0) Stop();
            int[] lotteryIdArray = new int[lotteryIdStrArray.Length];
            for (int i = 0; i < lotteryIdStrArray.Length; i++)
            {
                lotteryIdArray[i] = Convert.ToInt32(lotteryIdStrArray[i]);
            }
            for (int i = 0; i < lotteryIdArray.Length; i++)
            {
                int lotteryID = lotteryIdArray[i];
                switch (lotteryID)
                {
                    
                    case 98:
                        try
                        {
                            GetLotteryOpenNumberForJNDKL8(lotteryID);
                        }
                        catch (Exception ex)
                        {
                            LogOpenNumber.Write("开奖的彩种" + lotteryID.ToString() + ",  " + ex.Message);
                            break;
                        }
                        break;
                    case 99:
                        try
                        {
                            GetLotteryOpenNumberForBJKL8(lotteryID);
                        }
                        catch (Exception ex)
                        {
                            LogOpenNumber.Write("开奖的彩种" + lotteryID.ToString() + ",  " + ex.Message);
                            break;
                        }
                        break;
                         
                }
            }
        }
        #endregion

        #region 加拿大幸运28
        /// <summary>
        /// 加拿大幸运28
        /// </summary>
        /// <param name="lotteryID"></param>
        private void GetLotteryOpenNumberForJNDKL8(int lotteryID)
        {
            //判断时间，减少采集次数
            DateTime nowDateTime = DateTime.Now;
            if (nowDateTime.Hour == 19 && nowDateTime.Minute> 1 && nowDateTime.Minute < 50)
            {
                return;
            }

            DAL.Tables.T_Isuses tIsuses = new DAL.Tables.T_Isuses();
            DataSet ds = new DataSet();
            try
            {
                ds = GetHtmlByUrl(jndkl8Url);
            }
            catch (Exception e)
            {
                LogOpenNumber.Write("加拿大幸运28获取开奖号码页面异常1" + e.Message);
                return;
            }
            if ((ds == null) || (ds.Tables.Count < 1) || (ds.Tables[1].Rows.Count < 1))
            {
                LogOpenNumber.Write("加拿大幸运28获取开奖号码页面异常2");
                return;
            }
            else
            {
                DataRow dr = ds.Tables[1].Rows[0];


                string tIsuseName = dr["expect"].ToString(); //期数

                long expect = long.Parse(tIsuseName);
                DateTime opentime = Convert.ToDateTime(dr["opentime"].ToString());

                if (!IsuseAdd(expect, opentime))
                {
                    LogOpenNumber.Write("加拿大幸运28无法创建期号");
                    return;
                }

                //开奖号码opencode="13,15,22,26,32,33,36,40,41,42,43,46,50,53,56,62,66,67,69,70"
                // string winLotteryNumber = "13,15,22,26,32,33,36,40,41,42,43,46,50,53,56,62,66,67,69,70";
                string winLotteryNumber = dr["opencode"].ToString();
                string[] allCodeStrArray = winLotteryNumber.Split(',');
                int[] allCodeArray = new int[allCodeStrArray.Length];

                for (int i = 0; i < allCodeArray.Length; i++)
                {
                    allCodeArray[i] = Convert.ToInt32(allCodeStrArray[i]);
                }
                int a = allCodeArray[1] + allCodeArray[4] + allCodeArray[7] + allCodeArray[10] + allCodeArray[13] + allCodeArray[16];
               int b = allCodeArray[2] + allCodeArray[5] + allCodeArray[8] + allCodeArray[11] + allCodeArray[14] + allCodeArray[17];
                int c = allCodeArray[3] + allCodeArray[6] + allCodeArray[9] + allCodeArray[12] + allCodeArray[15] + allCodeArray[18];

                //a = 7; b = 5; c = 5;

                winLotteryNumber = (a % 10).ToString() + (b % 10).ToString() + (c % 10).ToString();



                tIsuses = new DAL.Tables.T_Isuses();
                long res = tIsuses.GetCount(conn, "LotteryID = " + lotteryID.ToString() + " and [Name] = '" + tIsuseName + "' and WinLotteryNumber is null");
                if (res < 1)
                {
                    return;
                }
                else
                {
                    LogOpenNumber.Write(res + "加拿大幸运28：" + tIsuseName + "，期，" + lotteryID.ToString() + "开奖： " + winLotteryNumber);

                }
                try
                {

                    tIsuses.WinLotteryNumber.Value = winLotteryNumber;
                    tIsuses.Update(conn, "LotteryID = " + lotteryID.ToString() + " and [Name] = '" + tIsuseName + "' and WinLotteryNumber is null");
                    if (jndkl8Switch == "1")
                    {
                        //开奖信息推送到聊天室
                        pushOpen(lotteryID, jndRooms, PushMsgType.OPEN, tIsuseName, winLotteryNumber);
                        //开奖
                        BJXY28DrawingLottery(lotteryID, tIsuseName, winLotteryNumber);
                       
                        
                    }
                }
                catch (Exception ex)
                {
                    LogOpenNumber.Write("加拿大幸运28获取开奖号码异常：" + ex.Message);
                }
            }
        }

        #endregion
        #region 北京幸运28
        /// <summary>
        /// 北京幸运28
        /// </summary>
        /// <param name="lotteryID"></param>
        private void GetLotteryOpenNumberForBJKL8(int lotteryID)
        {
            DAL.Tables.T_Isuses tIsuses = new DAL.Tables.T_Isuses();
            DataSet ds = new DataSet();
            try
            {
                ds = GetHtmlByUrl(bjkl8Url);
            }
            catch
            {
                LogOpenNumber.Write("北京幸运28获取开奖号码页面异常1");
                return;
            }
            if ((ds == null) || (ds.Tables.Count < 1) || (ds.Tables[1].Rows.Count < 1))
            {
                LogOpenNumber.Write("北京幸运28获取开奖号码页面异常2");
                return;
            }
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                string[] winNumber = new string[3];
                string tIsuseName = dr["expect"].ToString();
                //开奖号码
                string winLotteryNumber = dr["opencode"].ToString();
                string[] allCodeStrArray = winLotteryNumber.Split(',');
                int[] allCodeArray = new int[allCodeStrArray.Length];

                for (int i = 0; i < allCodeArray.Length-1; i++)
                {
                    allCodeArray[i] = Convert.ToInt32(allCodeStrArray[i]);
                }
                int a = allCodeArray[0] + allCodeArray[1] + allCodeArray[2] + allCodeArray[3] + allCodeArray[4] + allCodeArray[5];
                int b = allCodeArray[6] + allCodeArray[7] + allCodeArray[8] + allCodeArray[9] + allCodeArray[10] + allCodeArray[11];
                int c = allCodeArray[12] + allCodeArray[13] + allCodeArray[14] + allCodeArray[15] + allCodeArray[16] + allCodeArray[17];

              //  a = 1; b = 0; c = 0;
                winLotteryNumber = (a % 10).ToString() + (b % 10).ToString() + (c % 10).ToString();
              
            //    winLotteryNumber = winNumber[0].ToString() + winNumber[1].ToString() + winNumber[2].ToString();

                if (tIsuses.GetCount(conn, "LotteryID = " + lotteryID.ToString() + " and [Name] = '" + tIsuseName + "' and WinLotteryNumber is null and year(StartTime) = YEAR(GETDATE())") < 1)
                {
                    continue;
                }
                try
                {
                    tIsuses.WinLotteryNumber.Value = winLotteryNumber;
                    tIsuses.Update(conn, "LotteryID = " + lotteryID.ToString() + " and [Name] = '" + tIsuseName + "' and WinLotteryNumber is null and year(StartTime) = YEAR(GETDATE())");
                    if (bjkl8Switch == "1")
                    {
                        //开奖信息推送到聊天室
                        pushOpen(lotteryID, bjRooms, PushMsgType.OPEN, tIsuseName, winLotteryNumber);
                        //开奖
                        BJXY28DrawingLottery(lotteryID, tIsuseName, winLotteryNumber);
                       
                    }
                }
                catch (Exception ex)
                {
                    LogOpenNumber.Write("北京幸运28获取开奖号码异常：" + ex.Message);
                }
            }
        }
        #endregion

     

        #region 已有开奖号码开奖
        /// <summary>
        /// 已有开奖号码开奖
        /// </summary>
        private void OpenWinNumber()
        {
            DAL.Tables.T_Isuses tIsuses = new DAL.Tables.T_Isuses();
            DataTable dt = tIsuses.Open(conn, "name,winlotterynumber,lotteryid", "LotteryID in (" + automaticOpenLottery + ") and IsOpened = 0 and EndTime < Getdate() and month(getdate()) = MONTH(StartTime) and YEAR(GETDATE()) = YEAR(StartTime) and isnull(WinLotteryNumber, '') > ''", "");
            if (dt == null || dt.Rows.Count < 1)
            {
                return;
            }
            foreach (DataRow dr in dt.Rows)
            {
                string tIsuseName = dr["name"].ToString();
                string tWinLotteryNumber = dr["winlotterynumber"].ToString();
                int lotteryID = Convert.ToInt32(dr["lotteryid"].ToString());
                //开奖
                //DrawingLottery(lotteryID, tIsuseName, tWinLotteryNumber);
                BJXY28DrawingLottery(lotteryID, tIsuseName, tWinLotteryNumber);
            }
        }
        #endregion

        #region 开奖派奖
        /// <summary>
        /// 开奖派奖
        /// </summary>
        /// <param name="lotteryID"></param>
        /// <param name="isuseName"></param>
        /// <param name="winLotteryNumber"></param>
        public void DrawingLottery(int lotteryID, string isuseName, string winLotteryNumber)
        {
            string connStr = ini.Read("Options", "ConnectionString");
            conn = new SqlConnection(connStr);
            conn.Open();
            Log log = LogOpenNumber;
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
            DataTable dt = null;
            dt = new DAL.Tables.T_Schemes().Open(conn, "ID, InitiateUserID", "IsuseID = " + isuseID.ToString() + " and isOpened = 0", "");
            //取得投注号码
            DataTable mixcast = new DAL.Tables.T_SchemesMixcast().Open(conn, "ID,LotteryNumber,Multiple,PlayTypeID,SchemeId,WinMoney,WinMoneyNoWithTax,WinDescription", "SchemeId in (select id from T_schemes where isuseid=" + isuseID.ToString() + ")", "[ID] desc");
            if (dt == null)
            {
                log.Write("数据读写错误002");
                return;
            }
            // 准备开奖，开奖之前，对出票不完整的方案进行出票处理
            returnValue = 0;
            returnDescription = "";
            DataTable dtWinTypes = new DAL.Tables.T_WinTypes().Open(conn, "[DefaultMoney],[DefaultMoneyNoWithTax]", "LotteryID = " + lotteryID.ToString(), "[Order]");
            if (dtWinTypes == null)
            {
                log.Write("奖金读取数据读写错误");
                return;
            }
            #region 计算方案奖金
            //是否需要扣个税
            if (PF.GetTaxSwitch() == 1)
            {
                //扣税
                for (int i = 0; i < dtWinTypes.Rows.Count; i++)
                {
                    if (dtWinTypes.Rows[i]["DefaultMoney"] != null)
                    {
                        //大于一万将进行扣税
                        if (Convert.ToDouble(dtWinTypes.Rows[i]["DefaultMoney"]) >= 10000)
                        {
                            dtWinTypes.Rows[i]["DefaultMoneyNoWithTax"] = Convert.ToDouble(dtWinTypes.Rows[i]["DefaultMoney"]) * 0.8d;
                        }
                        else
                        {
                            dtWinTypes.Rows[i]["DefaultMoneyNoWithTax"] = dtWinTypes.Rows[i]["DefaultMoney"];
                        }
                    }
                }
            }
            else
            {
                //免税
                for (int i = 0; i < dtWinTypes.Rows.Count; i++)
                {
                    if (dtWinTypes.Rows[i]["DefaultMoney"] != null)
                    {
                        dtWinTypes.Rows[i]["DefaultMoneyNoWithTax"] = dtWinTypes.Rows[i]["DefaultMoney"];
                    }
                }
            }
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
            DataTable dt_Users = new DAL.Tables.T_Users().Open(conn, "ID,RegisterTime", "ID in (select InitiateUserID from T_Schemes where isuseid=" + isuseID.ToString() + ")", "");
            int count = 0;
            StringBuilder sb = new StringBuilder();
            //中奖明细表
            DataTable winMoneyDetailDtTemp = new DataTable();
            winMoneyDetailDtTemp = new DAL.Tables.T_WinMoneyDetail().Open(conn, "ID,SchemesID,PlaysID,WinMoney,WinMoneyNoWithTax", "1<>1", "");
            DataTable winMoneyDetailDt = winMoneyDetailDtTemp.Clone();
            List<string> executeSqlList = new List<string>();
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
                //用户注册的时间
                DateTime registerTime;
                Regex re = new Regex(@"\D");
                string userID = dt.Rows[i]["InitiateUserID"].ToString();
                try
                {
                    registerTime = Shove._Convert.StrToDateTime(dt_Users.Select("ID=" + userID)[0]["RegisterTime"].ToString(), "");
                    //获取最小注数信息ID
                    StringBuilder mixIDsSb = new StringBuilder();
                    foreach (DataRow item in mixcast.Select("SchemeId=" + schemeID))
                    {
                        mixIDsSb.Append(item["ID"].ToString() + ",");
                    }
                    if (mixIDsSb.Length > 0)
                    {
                        mixIDsSb = mixIDsSb.Remove(mixIDsSb.Length - 1, 1);
                    }
                    #region 方案号码计算奖金
                    foreach (DataRow item in mixcast.Select("SchemeId=" + schemeID))
                    {
                        string description = "";
                        string lotteryNumber = item["LotteryNumber"].ToString();
                        int multiple = Shove._Convert.StrToInt(item["Multiple"].ToString(), 1);
                        string lotteryId = item["PlayTypeID"].ToString();
                        lotteryId = lotteryId.Substring(0, lotteryId.Length - 2);
                        DateTime startTime = DateTime.Now.AddDays(3);
                        //加奖金额
                        double addWinMoney = 0;
                        //加奖金额不含税
                        double addWinMoneyNoWithTax = 0;
                        int winCounts = 0;
                        double winMoneyNoWithTax = 0;
                        double winMoney = 0;
                        winMoney = new SLS.Lottery()[lotteryID].ComputeWin(lotteryNumber, winLotteryNumber, ref description, ref winMoneyNoWithTax, int.Parse(item["PlayTypeID"].ToString()), ref winCounts, winMoneyList);
                        //加上此段代码原因：排除开奖奖金错误，因为有的彩票类计算奖金会出现负数。
                        winMoney = winMoney <= 0 ? 0 : winMoney;
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
                        executeSqlList.Add("update T_SchemesMixcast set WinMoney=" + winMoney + ", WinMoneyNoWithTax=" + winMoneyNoWithTax + ",WinDescription='" + description + "加奖：" + addWinMoney * multiple + "' where [ID] = " + item["ID"] + ";");
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
                sb.Append("update T_Schemes set PreWinMoney=").Append(totalWinMoney)
                        .Append(",PreWinMoneyNoWithTax=").Append(totalWinMoneyNoWithTax)
                        .Append(",EditWinMoney=").Append(totalWinMoney)
                        .Append(",EditWinMoneyNoWithTax=").Append(totalWinMoneyNoWithTax)
                        .Append(",LotteryNumber='").Append(oneNumber + "," + twoNumber + "," + winNumber).Append("'")
                        .Append(",WinDescription='").Append(Lottery.StatisticsWinDesc(descriptionList)).Append("'")
                        .Append(",Description='").Append("加奖：" + totalAddWinMoney).Append("'")
                        .Append(" where [ID] =").AppendLine(schemeID.ToString() + ";");
                executeSqlList.Add(sb.ToString());
            }
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
                PF.SendWinNotification(dsWin, conn);
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

        #region 北京幸运28开奖派奖
        /// <summary>
        /// 北京幸运28开奖派奖
        /// </summary>
        /// <param name="lotteryID"></param>
        /// <param name="isuseName"></param>
        /// <param name="winLotteryNumber"></param>
        public void BJXY28DrawingLottery(int lotteryID, string isuseName, string winLotteryNumber)
        {
            string connStr = ini.Read("Options", "ConnectionString");
            conn = new SqlConnection(connStr);
            conn.Open();
            Log log = LogOpenNumber;
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

                    if (dt_allBet != null && dt_allBet.Rows.Count > 0) {
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

                           winMoney = new SLS.Lottery()[lotteryID].ComputeWin(k,allBet,comboBet,dxdsBet,lotteryNumber, winLotteryNumber, ref description, ref winMoneyNoWithTax, int.Parse(item["PlayTypeID"].ToString()), ref winCounts, winMoneyList);

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
                            executeSqlList.Add("update T_SchemesMixcast set WinMoney=" + winMoney + ", WinMoneyNoWithTax=" + winMoneyNoWithTax + ",WinDescription='" + description + "加奖：" + addWinMoney * multiple + "' where [ID] = " + isuseID + ";");
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
                PF.SendWinNotification(dsWin, conn);
            }
            #endregion
            log.Write("开奖-----------------------------5");

            conn.Close();

        }
        #endregion


        #region 根据网站路径获取HTML返回数据集
        /// <summary>
        /// 根据网站路径获取HTML返回数据集
        /// </summary>
        /// <returns></returns>
        private DataSet GetHtmlByUrl(string fileName)
        {
            DataSet ds = new DataSet();
          
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                string xmlStr = PF.GetHtml(fileName).Trim().ToString();
                stream = new StringReader(xmlStr);
                //从stream装载到XmlTextReader
                reader = new XmlTextReader(stream);
                ds.ReadXml(reader);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return ds;
           
        }
        #endregion

        //计算回水返利
        private void EvenIfTheRebate()
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureNonQuery(conn, "P_GetRebateMoney", ref Outputs,
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );


            ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            new Log("EvenIfTheRebate").Write("回水过程返回：" + ReturnValue + "," + ReturnDescription);





            if (ReturnValue < 0)
            {
                new Log("EvenIfTheRebate").Write("Exec CalculateScore: Procedure \"P_CalculateScore\" Return: " + ReturnDescription);
            }
        }

        public void pushOpen(string[] rooms, PushMsgType type, string qihao, string lucknumber)
        {
            
            //测试融云发送服务
            Log log = LogPushMsg;
           
            PushMsg pushMsg = new PushMsg();
            pushMsg.type = type.GetHashCode();
            pushMsg.time = DateTime.Now;
            pushMsg.senderUserId = 66;
            pushMsg.nickname = "系统";
            pushMsg.periods = qihao;
            pushMsg.open_result = lucknumber;
            
            string json = JsonConvert.SerializeObject(pushMsg);
            TxtMessage messagepublishChatroomTxtMessage = new TxtMessage("开盘信息", json);
           
            for(int i=0;i<=rooms.Length-3;i+=3){
                string[] tempRooms = new string[3];
                tempRooms[0] = rooms[i];
                tempRooms[1] = rooms[i+1];
                tempRooms[2] = rooms[i+2];

                CodeSuccessReslut messagepublishChatroomResult = rongcloud.message.publishGroup("66", tempRooms, messagepublishChatroomTxtMessage, null, null, 0, 0);
                log.Write("publishGroup:[code][" + messagepublishChatroomResult.getCode() + "][" + messagepublishChatroomResult.getErrorMessage() + "]");
            }
           
        }

        public void pushOpen(int lotteryID,string[] rooms, PushMsgType type, string qihao, string lucknumber)
        {

            //融云发送服务，新
            Log log = LogPushMsg;

            PushMsg pushMsg = new PushMsg();
            pushMsg.type = type.GetHashCode();
            pushMsg.time = DateTime.Now;
            pushMsg.senderUserId = 66;
            pushMsg.nickname = "系统";
            pushMsg.periods = qihao;
            pushMsg.open_result = lucknumber;
            //新增的
            pushMsg.lotteryID = lotteryID;
            pushMsg.homeIndex = 1;

           // CodeSuccessReslut messagepublishChatroomResult = rongcloud.message.publishChatroom("66", rooms, messagepublishChatroomTxtMessage);
          //  log.Write("publishChatroom:[code][" + messagepublishChatroomResult.getCode() + "][" + messagepublishChatroomResult.getErrorMessage() + "]");

            for (int i = 0; i <3; i++)
            {
                pushMsg.homeIndex = i;
                string json = JsonConvert.SerializeObject(pushMsg);
                TxtMessage messagepublishChatroomTxtMessage = new TxtMessage("开盘信息", json);
                //string groupId = (lotteryID*100+(i+1)).ToString();
                //string[] roomArray = {groupId};

                CodeSuccessReslut messagepublishChatroomResult = rongcloud.message.publishGroup("66", new string[]{rooms[i]}, messagepublishChatroomTxtMessage, null, null, 0, 0);
                log.Write("publishGroup:[code][" + messagepublishChatroomResult.getCode() + "][" + messagepublishChatroomResult.getErrorMessage() + "]");
            }

        }

        #region 加拿大新增排期
        /// <summary>
        /// 根据最新一期，去排当天的期号
        /// </summary>
        /// <param name="qihao">期号</param>
        /// <param name="endTime">对应开奖时间</param>
        public bool IsuseAdd(long qihao, DateTime endTime)
        {
            bool result = false;
            TimeSpan ts = DateTime.Now - endTime;
            if (ts.TotalMinutes > 5)
            {
                //太旧的数据不需要
                return result;
            }

            Log log = LogOpenNumber;
            string connStr = ini.Read("Options", "ConnectionString");
            conn = new SqlConnection(connStr);

            conn.Open();
            double Interval = 3.5; //间隔
            DateTime dateTime = DateTime.Now;
            string todayEndTime = dateTime.ToShortDateString().ToString() + " 19:00:00"; //每天结束都是19点
            string tomorrowEndTime = dateTime.AddDays(1).ToShortDateString().ToString() + " 19:00:00";
           
            long NewIsuseID = -1;
            string ReturnDescription = "";

            //判断这个参数期号是否已经有了
            DataTable dtNow = new DAL.Tables.T_Isuses().Open(conn, "[ID]", "[Name]=" + qihao + " and LotteryID = 98", "");
            if (dtNow != null && dtNow.Rows.Count > 0)
            {
                //如果当天19点已经有排期，则跳过
                return true;
            }
            if (endTime < Convert.ToDateTime(todayEndTime))
            {
                //两种触发时间：1、当天19：00之前，就排期到当天的19：00

                DataTable dt = new DAL.Tables.T_Isuses().Open(conn, "[ID]", "EndTime >= '" + todayEndTime + "' and LotteryID = 98", "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    //如果当天19点已经有排期，则跳过
                    return true;
                }
                else
                {
                    //如果当天19点无排期，则排期
                    int i = 0;
                    int addCount = 0;
                    while (true)
                    {
                        i++;
                        qihao++;

                        DateTime tempEndTime = endTime.AddMinutes(i * Interval);
                        DateTime tempStartTime = endTime.AddMinutes(i * Interval - Interval);
                        int Results = -1;
                        Results = DAL.Procedures.P_IsuseAdd(conn, 98, qihao.ToString(), tempStartTime, tempEndTime, tempEndTime, "", "", ref NewIsuseID, ref ReturnDescription);

                        if (Results < 0)
                        {
                            break;
                        }
                        if (tempEndTime.Hour == 19)
                        {
                            break;
                        }
                        addCount += Results;
                    }
                    if (addCount > 0) return true;
                }

            }
            else
            {
                //两种触发时间：2、当天19：50以后，就排期到明天的19：00
                DataTable dt = new DAL.Tables.T_Isuses().Open(conn, "[ID]", "EndTime >= '" + tomorrowEndTime + "' and LotteryID = 98", "");

                if (dt != null && dt.Rows.Count > 0)
                {
                    //如果当天19点已经有排期，则跳过
                    return true;
                }
                else
                {
                    //如果当天19点无排期，则排期
                    int i = 0;
                    int addCount = 0;
                    while (true)
                    {
                        DateTime tempEndTime = endTime.AddMinutes(i * Interval);
                        DateTime tempStartTime = endTime.AddMinutes(i * Interval - Interval);
                        int Results = -1;
                        Results = DAL.Procedures.P_IsuseAdd(conn, 98, qihao.ToString(), tempStartTime, tempEndTime, tempEndTime, "", "", ref NewIsuseID, ref ReturnDescription);

                        if (Results < 0)
                        {
                            break;
                        }
                        if (tempEndTime >= Convert.ToDateTime(tomorrowEndTime))
                        {
                            break;
                        }
                        i++;
                        qihao++;
                        addCount += Results;
                    }
                    if (addCount > 0) return true;
                }
            }

            conn.Close();
            return result;
        }
        #endregion


    }
}