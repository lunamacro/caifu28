using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;

namespace SLS
{
    public partial class Lottery
    {
        /// <summary>
        /// 加拿大幸运28
        /// </summary>
        public partial class JND28 : LotteryBase
        {
            //注：加 * 号的为使用中的玩法
            #region 静态变量
            private const int PlayType_DX = 9801;
            private const int PlayType_CaiShuZi = 9802;
            private const int PlayType_TeShu = 9803;

            public const int ID = 98;
            private const string Name = "加拿大幸运28";
            private const string Code = "JND28";
            #endregion

            public JND28()
            {
                base.id = ID;
                base.name = Name;
                base.code = Code;
            }

            public override bool CheckPlayType(int play_type)
            {
                return ((play_type >= 9801) && (play_type <= 9803));
            }

            public override PlayType[] GetPlayTypeList()
            {
                PlayType[] Result = new PlayType[3];

                Result[0] = new PlayType(PlayType_DX, "大小单双");
                Result[1] = new PlayType(PlayType_CaiShuZi, "猜数字");
                Result[2] = new PlayType(PlayType_TeShu, "特殊");

                return Result;
            }

            public override string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)
            {
                if (PlayType == PlayType_DX)
                    return ToSingle_DX(Number, ref CanonicalNumber);

                if (PlayType == PlayType_CaiShuZi)
                    return ToSingle_CaiShuZi(Number, ref CanonicalNumber);

                if (PlayType == PlayType_TeShu)
                    return ToSingle_TeShu(Number, ref CanonicalNumber);

                return null;
            }

            #region ToSingle 的具体方法

            #region 大小单双
            private string[] ToSingle_DX(string Number, ref string CanonicalNumber)
            {
                CanonicalNumber = "";

                string[] strArr = { "大", "小", "单", "双", "大单", "大双", "小单", "小双", "极大", "极小" };
                if (!(Array.IndexOf(strArr, Number) >= 0))
                {
                    return null;
                }

                CanonicalNumber = Number;

                string[] Result = new string[1];
                Result[0] = CanonicalNumber;

                return Result;
            }

            #endregion

            #region 猜数字
            private string[] ToSingle_CaiShuZi(string Number, ref string CanonicalNumber)
            {
                return null;
            }

            #endregion

            #region 特殊玩法
            private string[] ToSingle_TeShu(string Number, ref string CanonicalNumber)
            {
                return null;
            }
            #endregion


            #endregion

            public override double ComputeWin(int homeIndex, double allBet, double comboBet, double dxdsBet, string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, ref int WinCount, params double[] WinMoneyList)
            {
                Description = "";
               /**
                * switch (homeIndex)
                {
                    case 0:
                        Description = "玩法一";
                        break;
                    case 1:
                        Description = "玩法二";
                        break;
                    case 2:
                        Description = "玩法三";
                        break;

                }*/
                WinMoneyNoWithTax = 0;

                if ((WinMoneyList == null) || (WinMoneyList.Length < 20))
                    return -3;

                if (PlayType == PlayType_DX)
                    return ComputeWin_DX(homeIndex, allBet, comboBet, dxdsBet, Number, WinNumber, ref Description, ref WinMoneyNoWithTax, ref WinCount,
                          WinMoneyList[16], WinMoneyList[17], WinMoneyList[18], WinMoneyList[19], WinMoneyList[20], WinMoneyList[21],
                          WinMoneyList[22], WinMoneyList[23], WinMoneyList[24], WinMoneyList[25], WinMoneyList[26], WinMoneyList[27],
                          WinMoneyList[28], WinMoneyList[29]);

                if (PlayType == PlayType_CaiShuZi)
                    return ComputeWin_CaiShuZi(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, ref WinCount,
                        WinMoneyList[0], WinMoneyList[1], WinMoneyList[2], WinMoneyList[3], WinMoneyList[4], WinMoneyList[5],
                         WinMoneyList[6], WinMoneyList[7], WinMoneyList[8], WinMoneyList[9], WinMoneyList[10], WinMoneyList[11],
                          WinMoneyList[12], WinMoneyList[13], WinMoneyList[14], WinMoneyList[15]
                        );

                if (PlayType == PlayType_TeShu)
                    return ComputeWin_TeShu(Number, WinNumber, ref Description, ref WinMoneyNoWithTax, ref WinCount, WinMoneyList[30], WinMoneyList[31], WinMoneyList[32], WinMoneyList[33]);

                return 0;
            }



            #region ComputeWin  的具体方法

            private double ComputeWin_DX(int homeIndex, double allBet, double comboBet, double dxdsBet, string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, ref int WinCount, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, double WinMoney6, double WinMoneyNoWithTax6, double WinMoney7, double WinMoneyNoWithTax7)
            {

                WinNumber = WinNumber.Trim();

                if (WinNumber.Length != 3)
                    return 0;

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return -2;
                if (Lotterys.Length < 1)
                    return -2;

                char[] c = WinNumber.ToCharArray();

                //大：14 - 27 小：0 - 13 单：%2  双：%2  大单：15 17 19 21 23 25 27  小单：1 3 5 7 9 11 13
                //大双：14 16 18 20 22 24 26  小双：0 2 4 6 8 10 12  极大：22 23 24 25 26 27 极小：0 1 2 3 4 5
                int sumWin = 0;
                for (int i = 0; i < c.Length; i++)
                {
                    sumWin += Shove._Convert.StrToInt(c[i].ToString(), 0);
                }

                bool isBig = (sumWin >= 14 && sumWin <= 27);
                bool isSamll = (sumWin >= 0 && sumWin <= 13);
                bool isSingle = sumWin % 2 != 0;
                bool isDouble = sumWin % 2 == 0;
                bool isBigSingle = isBig && isSingle;//大单
                bool isSamllSingle = isSamll && isSingle;//
                bool isBigDouble = isBig && isDouble;//大双
                bool isSamllDouble = isSamll && isDouble;//小双
                bool isGreat = (sumWin >= 22 && sumWin <= 27);//极大
                bool isTiny = (sumWin >= 0 && sumWin <= 5);//极小
                /*
                开13只有：  小   单   小单 中奖
                开14只有：  大   双   大双 中奖
                */
                bool is13 = (13 == sumWin);
                bool is14 = (14 == sumWin);

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                for (int ii = 0; ii < Lotterys.Length; ii++)
                {
                    string t_str = "";
                    string[] Lottery = ToSingle_DX(Lotterys[ii], ref t_str);
                    if (Lottery == null)
                        continue;
                    if (Lottery.Length < 1)
                        continue;

                    for (int i = 0; i < Lottery.Length; i++)
                    {
                        if (Lottery[i].Length <= 0 || string.IsNullOrWhiteSpace(Lottery[i])) continue;
                        string lottStr = Lottery[i];
                        if (lottStr == "大" && isBig)
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            #region 大，开14
                            if (is14)
                            {
                                switch (homeIndex)
                                {
                                    case 0:
                                        //玩法一
                                        if (allBet <= 10000)
                                        {
                                            WinMoney = 1.5;
                                            WinMoneyNoWithTax = 1.5;
                                        }
                                        else
                                        {
                                            WinMoney = 1;
                                            WinMoneyNoWithTax = 1;
                                        }
                                        break;
                                    case 1:
                                        //玩法二
                                        if (dxdsBet <= 1000)
                                        {
                                            WinMoney = 1.6;
                                            WinMoneyNoWithTax = 1.6;
                                        }
                                        else if (dxdsBet > 1000 && dxdsBet <= 10000)
                                        {
                                            WinMoney = 1.5;
                                            WinMoneyNoWithTax = 1.5;
                                        }

                                        if (allBet > 10000)
                                        {
                                            WinMoney = 1;
                                            WinMoneyNoWithTax = 1;
                                        }
                                        break;
                                    case 2:
                                        //玩法三
                                        if (dxdsBet <= 10000)
                                        {
                                            WinMoney = 1.6;
                                            WinMoneyNoWithTax = 1.6;
                                        }
                                        else if (dxdsBet > 10000)
                                        {
                                            WinMoney = 1;
                                            WinMoneyNoWithTax = 1;
                                        }
                                        break;
                                }
                                string buchong = "大小单双投注金额" + dxdsBet + "，组合投注金额" + comboBet + "，总投注金额" + allBet;
                                MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注，" + buchong);
                            }
                            #endregion
                            else
                                MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                        }
                        if (lottStr == "小" && isSamll)
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            #region 小，开13
                            if (is13)
                            {

                                switch (homeIndex)
                                {
                                    case 0:
                                        //玩法一
                                        if (allBet <= 10000)
                                        {
                                            WinMoney = 1.5;
                                            WinMoneyNoWithTax = 1.5;
                                        }
                                        else
                                        {
                                            WinMoney = 1;
                                            WinMoneyNoWithTax = 1;
                                        }
                                        break;
                                    case 1:
                                        //玩法二
                                        if (dxdsBet <= 1000)
                                        {
                                            WinMoney = 1.6;
                                            WinMoneyNoWithTax = 1.6;
                                        }
                                        else if (dxdsBet > 1000 && dxdsBet <= 10000)
                                        {
                                            WinMoney = 1.5;
                                            WinMoneyNoWithTax = 1.5;
                                        }

                                        if (allBet > 10000)
                                        {
                                            WinMoney = 1;
                                            WinMoneyNoWithTax = 1;
                                        }
                                        break;
                                    case 2:
                                        //玩法三
                                        if (dxdsBet <= 10000)
                                        {
                                            WinMoney = 1.6;
                                            WinMoneyNoWithTax = 1.6;
                                        }
                                        else if (dxdsBet > 10000)
                                        {
                                            WinMoney = 1;
                                            WinMoneyNoWithTax = 1;
                                        }
                                        break;
                                }
                                string buchong = "大小单双投注金额" + dxdsBet + "，组合投注金额" + comboBet + "，总投注金额" + allBet;
                                MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注，" + buchong);
                            }
                            #endregion
                            else
                                MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                        }
                        if (lottStr == "单" && isSingle)
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            #region 单，开13
                            if (is13)
                            {

                                switch (homeIndex)
                                {
                                    case 0:
                                        //玩法一
                                        if (allBet <= 10000)
                                        {
                                            WinMoney = 1.5;
                                            WinMoneyNoWithTax = 1.5;
                                        }
                                        else
                                        {
                                            WinMoney = 1;
                                            WinMoneyNoWithTax = 1;
                                        }
                                        break;
                                    case 1:
                                        //玩法二
                                        if (dxdsBet <= 1000)
                                        {
                                            WinMoney = 1.6;
                                            WinMoneyNoWithTax = 1.6;
                                        }
                                        else if (dxdsBet > 1000 && dxdsBet <= 10000)
                                        {
                                            WinMoney = 1.5;
                                            WinMoneyNoWithTax = 1.5;
                                        }

                                        if (allBet > 10000)
                                        {
                                            WinMoney = 1;
                                            WinMoneyNoWithTax = 1;
                                        }
                                        break;
                                    case 2:
                                        //玩法三
                                        if (dxdsBet <= 10000)
                                        {
                                            WinMoney = 1.6;
                                            WinMoneyNoWithTax = 1.6;
                                        }
                                        else if (dxdsBet > 10000)
                                        {
                                            WinMoney = 1;
                                            WinMoneyNoWithTax = 1;
                                        }
                                        break;
                                }
                                string buchong = "大小单双投注金额" + dxdsBet + "，组合投注金额" + comboBet + "，总投注金额" + allBet;
                                MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注，" + buchong);
                            }
                            #endregion
                            else
                                MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                        }
                        if (lottStr == "双" && isDouble)
                        {
                            WinCount++;
                            WinMoney += WinMoney1;
                            WinMoneyNoWithTax += WinMoneyNoWithTax1;
                            #region 双，开14
                            if (is14)
                            {

                                switch (homeIndex)
                                {
                                    case 0:
                                        //玩法一
                                        if (allBet <= 10000)
                                        {
                                            WinMoney = 1.5;
                                            WinMoneyNoWithTax = 1.5;
                                        }
                                        else
                                        {
                                            WinMoney = 1;
                                            WinMoneyNoWithTax = 1;
                                        }
                                        break;
                                    case 1:
                                        //玩法二
                                        if (dxdsBet <= 1000)
                                        {
                                            WinMoney = 1.6;
                                            WinMoneyNoWithTax = 1.6;
                                        }
                                        else if (dxdsBet > 1000 && dxdsBet <= 10000)
                                        {
                                            WinMoney = 1.5;
                                            WinMoneyNoWithTax = 1.5;
                                        }

                                        if (allBet > 10000)
                                        {
                                            WinMoney = 1;
                                            WinMoneyNoWithTax = 1;
                                        }
                                        break;
                                    case 2:
                                        //玩法三
                                        if (dxdsBet <= 10000)
                                        {
                                            WinMoney = 1.6;
                                            WinMoneyNoWithTax = 1.6;
                                        }
                                        else if (dxdsBet > 10000)
                                        {
                                            WinMoney = 1;
                                            WinMoneyNoWithTax = 1;
                                        }
                                        break;
                                }
                                string buchong = "大小单双投注金额" + dxdsBet + "，组合投注金额" + comboBet + "，总投注金额" + allBet;
                                MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注，" + buchong);
                            }
                            #endregion
                            else
                                MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                        }
                        if (lottStr == "大单" && isBigSingle)
                        {
                            WinCount++;
                            WinMoney += WinMoney2;
                            WinMoneyNoWithTax += WinMoneyNoWithTax2;
                            MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                        }
                        if (lottStr == "小单" && isSamllSingle)
                        {
                            WinCount++;
                            WinMoney += WinMoney4;
                            WinMoneyNoWithTax += WinMoneyNoWithTax4;
                            #region 小单，开13
                            if (is13)
                            {

                                switch (homeIndex)
                                {
                                    case 0:
                                        //玩法一

                                        WinMoney = 1;
                                        WinMoneyNoWithTax = 1;

                                        break;
                                    case 1:
                                        //玩法二
                                        if (comboBet >= 50 && comboBet <= 10000)
                                        {
                                            WinMoney = 2.5;
                                            WinMoneyNoWithTax = 2.5;
                                        }
                                        if (allBet > 10000)
                                        {
                                            WinMoney = 1;
                                            WinMoneyNoWithTax = 1;
                                        }
                                        break;
                                    case 2:
                                        //玩法二
                                        WinMoney = 0;
                                        WinMoneyNoWithTax = 0;
                                        break;
                                }
                                string buchong = "大小单双投注金额" + dxdsBet + "，组合投注金额" + comboBet + "，总投注金额" + allBet;
                                MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注，" + buchong);
                            }
                            #endregion
                            else
                                MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                        }
                        if (lottStr == "大双" && isBigDouble)
                        {
                            WinCount++;
                            WinMoney += WinMoney3;
                            WinMoneyNoWithTax += WinMoneyNoWithTax3;
                            #region 大双，开14
                            if (is14)
                            {

                                switch (homeIndex)
                                {
                                    case 0:
                                        //玩法一
                                        WinMoney = 1;
                                        WinMoneyNoWithTax = 1;

                                        break;
                                    case 1:
                                        //玩法二
                                        if (comboBet >= 50 && comboBet <= 10000)
                                        {
                                            WinMoney = 2.5;
                                            WinMoneyNoWithTax = 2.5;
                                        }
                                        if (allBet > 10000)
                                        {
                                            WinMoney = 1;
                                            WinMoneyNoWithTax = 1;
                                        }
                                        break;
                                    case 2:
                                        //玩法二
                                        WinMoney = 0;
                                        WinMoneyNoWithTax = 0;
                                        break;
                                }
                                string buchong = "大小单双投注金额" + dxdsBet + "，组合投注金额" + comboBet + "，总投注金额" + allBet;
                                MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注，" + buchong);
                            }
                            #endregion
                            else
                                MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                        }
                        if (lottStr == "小双" && isSamllDouble)
                        {
                            WinCount++;
                            WinMoney += WinMoney5;
                            WinMoneyNoWithTax += WinMoneyNoWithTax5;
                            MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                        }
                        if (lottStr == "极大" && isGreat)
                        {
                            WinCount++;
                            WinMoney += WinMoney6;
                            WinMoneyNoWithTax += WinMoneyNoWithTax6;
                            MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                        }
                        if (lottStr == "极小" && isTiny)
                        {
                            WinCount++;
                            WinMoney += WinMoney7;
                            WinMoneyNoWithTax += WinMoneyNoWithTax7;
                            MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                        }

                    }

                }


                if (Description != "")
                    Description += "。";

                return WinMoney;
            }

            private double ComputeWin_CaiShuZi(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, ref int WinCount, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2, double WinMoney3, double WinMoneyNoWithTax3, double WinMoney4, double WinMoneyNoWithTax4, double WinMoney5, double WinMoneyNoWithTax5, double WinMoney6, double WinMoneyNoWithTax6, double WinMoney7, double WinMoneyNoWithTax7, double WinMoney8, double WinMoneyNoWithTax8)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length != 3)	//123
                    return 0;

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return 0;
                if (Lotterys.Length < 1)
                    return 0;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                int sumWin = 0;
                char[] c = WinNumber.ToCharArray();
                for (int i = 0; i < c.Length; i++)
                {
                    sumWin += Shove._Convert.StrToInt(c[i].ToString(), 0);
                }

                string[] number1 = { "00", "27" };
                string[] number2 = { "01", "26" };
                string[] number3 = { "02", "25" };
                string[] number4 = { "03", "24" };
                string[] number5 = { "04", "23" };
                string[] number6 = { "05", "22" };
                string[] number7 = { "06", "07", "08", "09", "10", "11", "16", "17", "18", "19", "20", "21" };
                string[] number8 = { "12", "13", "14", "15" };

                string strWinNumber = sumWin.ToString().PadLeft(2, '0');

                for (int i = 0; i < Lotterys.Length; i++)
                {
                    if (Lotterys[i].Length <= 0 || string.IsNullOrWhiteSpace(Lotterys[i])) continue;

                    string lottStr = Lotterys[i].ToString().PadLeft(2, '0');

                    bool isWin = lottStr == strWinNumber;//10  05
                    if (!isWin) continue;

                    if (Array.IndexOf(number1, lottStr) >= 0)
                    {
                        WinCount++;
                        WinMoney += WinMoney1;
                        WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                    }
                    if (Array.IndexOf(number2, lottStr) >= 0)
                    {
                        WinCount++;
                        WinMoney += WinMoney2;
                        WinMoneyNoWithTax += WinMoneyNoWithTax2;
                        MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                    }
                    if (Array.IndexOf(number3, lottStr) >= 0)
                    {
                        WinCount++;
                        WinMoney += WinMoney3;
                        WinMoneyNoWithTax += WinMoneyNoWithTax3;
                        MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                    }
                    if (Array.IndexOf(number4, lottStr) >= 0)
                    {
                        WinCount++;
                        WinMoney += WinMoney4;
                        WinMoneyNoWithTax += WinMoneyNoWithTax4;
                        MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                    }
                    if (Array.IndexOf(number5, lottStr) >= 0)
                    {
                        WinCount++;
                        WinMoney += WinMoney5;
                        WinMoneyNoWithTax += WinMoneyNoWithTax5;
                        MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                    }
                    if (Array.IndexOf(number6, lottStr) >= 0)
                    {
                        WinCount++;
                        WinMoney += WinMoney6;
                        WinMoneyNoWithTax += WinMoneyNoWithTax6;
                        MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                    }
                    if (Array.IndexOf(number7, lottStr) >= 0)
                    {
                        WinCount++;
                        WinMoney += WinMoney7;
                        WinMoneyNoWithTax += WinMoneyNoWithTax7;
                        MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                    }
                    if (Array.IndexOf(number8, lottStr) >= 0)
                    {
                        WinCount++;
                        WinMoney += WinMoney8;
                        WinMoneyNoWithTax += WinMoneyNoWithTax8;
                        MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                    }
                }
                /**
                if (WinCount > 0)
                {
                    MergeWinDescription(ref Description, "猜数字，中" + WinCount.ToString() + "注");
                }
                */
                if (Description != "")
                    Description += "。";

                return WinMoney;
            }

            private double ComputeWin_TeShu(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, ref int WinCount, double WinMoney1, double WinMoneyNoWithTax1, double WinMoney2, double WinMoneyNoWithTax2)
            {
                WinNumber = WinNumber.Trim();
                if (WinNumber.Length != 3)	//123
                    return 0;

                string[] Lotterys = SplitLotteryNumber(Number);
                if (Lotterys == null)
                    return 0;
                if (Lotterys.Length < 1)
                    return 0;

                double WinMoney = 0;
                WinMoneyNoWithTax = 0;

                int sumWin = 0;
                char[] c = WinNumber.ToCharArray();
                for (int i = 0; i < c.Length; i++)
                {
                    sumWin += Shove._Convert.StrToInt(c[i].ToString(), 0);
                }

                string strWinNumber = sumWin.ToString().PadLeft(2, '0');

                string[] number1 = { "03", "06", "09", "12", "15", "18", "21", "24" };
                string[] number2 = { "01", "04", "07", "10", "16", "19", "22", "25" };
                string[] number3 = { "02", "05", "08", "11", "17", "20", "23", "26" };

                bool isBaozi = false;

                if (c[0].ToString() == c[1].ToString() && c[1].ToString() == c[2].ToString())
                {
                    isBaozi = true;
                }

                for (int i = 0; i < Lotterys.Length; i++)
                {
                    if (Lotterys[i].Length <= 0 || string.IsNullOrWhiteSpace(Lotterys[i])) continue;

                    string lottStr = Lotterys[i];

                    if (lottStr == "豹子" && isBaozi)
                    {
                        WinCount++;
                        WinMoney += WinMoney1;
                        WinMoneyNoWithTax += WinMoneyNoWithTax1;
                        MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                    }

                    if (Array.IndexOf(number1, strWinNumber) >= 0 && lottStr == "红")
                    {
                        WinCount++;
                        WinMoney += WinMoney2;
                        WinMoneyNoWithTax += WinMoneyNoWithTax2;
                        MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                    }

                    if (Array.IndexOf(number2, strWinNumber) >= 0 && lottStr == "绿")
                    {
                        WinCount++;
                        WinMoney += WinMoney2;
                        WinMoneyNoWithTax += WinMoneyNoWithTax2;
                        MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                    }

                    if (Array.IndexOf(number3, strWinNumber) >= 0 && lottStr == "蓝")
                    {
                        WinCount++;
                        WinMoney += WinMoney2;
                        WinMoneyNoWithTax += WinMoneyNoWithTax2;
                        MergeWinDescription(ref Description, lottStr + "，中" + WinCount.ToString() + "注");
                    }
                }
                /**
                if (WinCount > 0)
                {
                    MergeWinDescription(ref Description, "特殊玩法，中" + WinCount.ToString() + "注");
                }
                */
                if (Description != "")
                    Description += "。";

                return WinMoney;
            }
            #endregion

            public override string AnalyseScheme(string Content, int PlayType)
            {
                if (PlayType == PlayType_DX)
                    return AnalyseScheme_DX(Content, PlayType);

                if (PlayType == PlayType_CaiShuZi)
                    return AnalyseScheme_CaiShuZi(Content, PlayType);

                if (PlayType == PlayType_TeShu)
                    return AnalyseScheme_TeShu(Content, PlayType);

                return "";
            }

            #region AnalyseScheme 的具体方法

            private string AnalyseScheme_DX(string Content, int PlayType)
            {
                string[] strs = Content.Split('\n');
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string Result = "";

                for (int i = 0; i < strs.Length; i++)
                {
                    if (strs[i].Trim().Length == 0) continue;

                    string CanonicalNumber = "";
                    string[] singles = ToSingle_DX(strs[i], ref CanonicalNumber);

                    if (singles == null)
                        continue;
                    if (singles.Length >= 1)
                        Result += CanonicalNumber + "|" + singles.Length.ToString() + "\n";
                }

                if (Result.EndsWith("\n"))
                    Result = Result.Substring(0, Result.Length - 1);
                return Result;
            }

            private string AnalyseScheme_CaiShuZi(string Content, int PlayType)
            {
                string[] strs = SplitLotteryNumber(Content);
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string result = "";

                Regex regex = new Regex(@"^\d{1,2}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                for (int i = 0; i < strs.Length; i++)
                {
                    Match m = regex.Match(strs[i]);
                    if (m.Success)
                    {
                        int _sum = Shove._Convert.StrToInt(strs[i].ToString(), -1);
                        if (_sum < 0 || _sum > 27)
                            continue;
                        result += strs[i] + "|1\n";
                    }
                }

                if (result.EndsWith("\n"))
                    result = result.Substring(0, result.Length - 1);
                return result;
            }

            private string AnalyseScheme_TeShu(string Content, int PlayType)
            {
                string[] strs = SplitLotteryNumber(Content);
                if (strs == null)
                    return "";
                if (strs.Length == 0)
                    return "";

                string result = "";

                string[] strArr = { "豹子", "红", "绿", "蓝" };
                for (int i = 0; i < strs.Length; i++)
                {
                    if (!(Array.IndexOf(strArr, strs[i]) >= 0))
                    {
                        return null;
                    }
                    result += strs[i] + "|1\n";

                }

                if (result.EndsWith("\n"))
                    result = result.Substring(0, result.Length - 1);
                return result;
            }

            #endregion

            public override int GetNum(int Number1, int Number2)
            {
                if (Number2 < Number1)
                {
                    return -1;
                }

                if (Number2 == Number1)
                {
                    return 1;
                }

                int i = 1;
                int j = 1;

                while (Number1 > 0)
                {
                    i = i * (Number2 + 1 - Number1);
                    j = j * (Number1);

                    Number1--;
                }

                return i / j;
            }

            private string FilterRepeated(string NumberPart)
            {
                string Result = "";
                for (int i = 0; i < NumberPart.Length; i++)
                {
                    if ((Result.IndexOf(NumberPart.Substring(i, 1)) == -1) && ("0123456789-".IndexOf(NumberPart.Substring(i, 1)) >= 0))
                        Result += NumberPart.Substring(i, 1);
                }
                return Sort(Result);
            }

        }
    }
}
