using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// YiLouZhi 的摘要说明
/// </summary>
public class YiLouZhi
{
    public YiLouZhi()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    #region 获取时时彩遗漏值
    /// <summary>
    /// 获取时时彩遗漏值
    /// </summary>
    /// <param name="lotteryId">彩种ID</param>
    /// <param name="sscBuyType">1：大小单双；2：组三包胆；3：组三和值；4：二星和值；5：直选；；6：组三；7：组六；8：二星组选；...</param>
    /// <returns></returns>
    public static Dictionary<string, string> SSCMiss(int lotteryId, int sscBuyType)
    {
        //未结束的期
        DataTable dt = new DAL.Tables.T_Isuses().Open("Top 100 WinLotteryNumber", "LotteryID=" + lotteryId + " and IsOpened = 1 and WinLotteryNumber <> ''", "[EndTime] desc");

        List<List<int>> getMissingDetail = new List<List<int>>();
        Dictionary<string, string> getMissingDetailDict = new Dictionary<string, string>();

        #region sscBuyType=1  大小单双
        if (sscBuyType == 1)
        {
            //十位个位的大小单双
            int[] shiDXDS = new int[] { 100, 100, 100, 100 };
            int[] geDXDS = new int[] { 100, 100, 100, 100 };
            List<int> temp = new List<int>();
            StringBuilder tempSb = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string lostPrieod = dt.Rows[i]["WinLotteryNumber"].ToString().Trim();
                if (lostPrieod.Length != 5)
                    continue;
                int shi = int.Parse(lostPrieod.Substring(3, 1));
                int ge = int.Parse(lostPrieod.Substring(4, 1));

                //十位大、小没出的期数
                if (shi > 4)
                {
                    if (shiDXDS[0] == 100)
                        shiDXDS[0] = i;
                }
                else
                {
                    if (shiDXDS[1] == 100)
                        shiDXDS[1] = i;
                }

                //个位大、小没出的期数
                if (ge > 4)
                {
                    if (geDXDS[0] == 100)
                        geDXDS[0] = i;
                }
                else
                {
                    if (geDXDS[1] == 100)
                        geDXDS[1] = i;
                }

                //十位单、双没出的期数
                if (shi % 2 == 1)
                {
                    if (shiDXDS[2] == 100)
                        shiDXDS[2] = i;
                }
                else
                {
                    if (shiDXDS[3] == 100)
                        shiDXDS[3] = i;
                }

                //个位单、双没出的期数
                if (ge % 2 == 1)
                {
                    if (geDXDS[2] == 100)
                        geDXDS[2] = i;
                }
                else
                {
                    if (geDXDS[3] == 100)
                        geDXDS[3] = i;
                }
            }

            int[] Nums = new int[4];
            for (int i = 0; i < 4; i++)
            {
                Nums[i] = shiDXDS[i];
            }
            for (int k = 0; k < 4; k++)
            {
                temp.Add(shiDXDS[k]);
                tempSb.Append(shiDXDS[k] + ",");
            }
            for (int i = 0; i < 4; i++)
            {
                Nums[i] = geDXDS[i];
            }
            for (int k = 0; k < 4; k++)
            {
                temp.Add(geDXDS[k]);
                tempSb.Append(geDXDS[k] + ",");
            }
            getMissingDetail.Add(temp);
            tempSb = tempSb.Remove(tempSb.Length - 1, 1);
            getMissingDetailDict.Add("1", tempSb.ToString());
        }
        #endregion

        #region sscBuyType == 2 组三包胆
        else if (sscBuyType == 2)
        {
            #region Update By Lusw
            int[] missingNumer = new int[10];//对应的遗漏次数
            List<int> temp = new List<int>();
            StringBuilder tempSb = new StringBuilder();
            for (int i = 0; i <= 9; i++)
            {
                int missCount = 0;//遗漏次数

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string winLotteryNumber = dt.Rows[j]["WinLotteryNumber"].ToString();//开奖号码
                    string lastThreeNum = winLotteryNumber.Substring(2);//取后三位

                    if (lastThreeNum.IndexOf(i.ToString()) != -1)
                    {
                        missingNumer[i] = missCount;
                        break;
                    }

                    missCount++;//累加遗漏次数
                }
            }

            for (int i = 0; i < 10; i++)
            {
                temp.Add(missingNumer[i]);
                tempSb.Append(missingNumer[i].ToString() + ",");
            }
            getMissingDetailDict.Add("1", tempSb.Remove(tempSb.Length - 1, 1).ToString());
            getMissingDetail.Add(temp);
            #endregion
        }
        #endregion

        #region sscBuyType == 3 组三和值
        else if (sscBuyType == 3)
        {
            List<int> temp = new List<int>();
            StringBuilder tempSb = new StringBuilder();
            string winLotteryNumber = string.Empty;
            int number1 = 0;
            int number2 = 0;
            int number3 = 0;
            int sum = 0;
            int count = dt.Rows.Count;
            int[] omission = new int[28];
            InitOmissionArray(omission, count);
            //标识是否已经出现
            bool[] omission1 = new bool[28];
            Regex reg = new Regex("\\d");
            for (int i = 0; i < count; i++)
            {
                winLotteryNumber = dt.Rows[i]["WinLotteryNumber"].ToString();
                MatchCollection matches = reg.Matches(winLotteryNumber);
                number1 = int.Parse(matches[2].Groups[0].Value);
                number2 = int.Parse(matches[3].Groups[0].Value);
                number3 = int.Parse(matches[4].Groups[0].Value);
                sum = (number1 + number2 + number3);
                //如果遗漏已经出现过一次了就不取遗漏数据
                if (!omission1[sum])
                {
                    omission[sum] = i;
                    omission1[sum] = true;
                }
            }
            //从遗漏数组中获取到最大的遗漏值
            int maxOmission = omission.Max();
            for (int i = 0; i < omission.Length; i++)
            {
                temp.Add(omission[i]);
                tempSb.Append(omission[i].ToString() + ",");
            }
            getMissingDetail.Add(temp);
            getMissingDetailDict.Add("1", tempSb.Remove(tempSb.Length - 1, 1).ToString());
        }
        #endregion

        #region sscBuyType == 4 二星和值
        else if (sscBuyType == 4)
        {
            DataSet ds = new DataSet();
            DataRow[] dr;
            List<int> temp = new List<int>();
            StringBuilder tempSb = new StringBuilder();
            if (DAL.Procedures.P_TrendChart_CQSSC_2XHZZST(ref ds, 100) < 0)
            {
                return null;
            }
            else
            {

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dr = ds.Tables[0].Select("", "isuse desc");
                    if (dr != null && dr.Length >= 1)
                    {
                        int[] Nums = new int[19];
                        for (int i = 0; i < 19; i++)
                        {
                            Nums[i] = Shove._Convert.StrToInt(dr[0]["Z_" + i].ToString(), 0);
                        }
                        for (int i = 0; i < 19; i++)
                        {
                            temp.Add(Convert.ToInt32(dr[0]["Z_" + i]));
                            tempSb.Append(dr[0]["Z_" + i] + ",");
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < 19; i++)
                    {
                        temp.Add(0);
                        tempSb.Append("0,");
                    }
                }
            }
            getMissingDetailDict.Add("1", tempSb.Remove(tempSb.Length - 1, 1).ToString());
            getMissingDetail.Add(temp);
        }
        #endregion

        #region sscBuyType == 5  直选
        else if (sscBuyType == 5)
        {
            //百十个位从0到9有多少期没出
            int[] missingNumber = new int[50];
            int Num = dt.Rows.Count;
            List<int> temp = new List<int>();
            StringBuilder tempSb = new StringBuilder();
            for (int i = 0; i < missingNumber.Length; i++)
                missingNumber[i] = Num;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string lostPrieod = dt.Rows[i]["WinLotteryNumber"].ToString().Trim();
                if (lostPrieod.Length != 5)
                    continue;
                int wan = int.Parse(lostPrieod.Substring(0, 1));
                int qian = int.Parse(lostPrieod.Substring(1, 1));
                int bai = int.Parse(lostPrieod.Substring(2, 1));
                int shi = int.Parse(lostPrieod.Substring(3, 1));
                int ge = int.Parse(lostPrieod.Substring(4, 1));

                for (int j = 0; j <= 9; j++)
                {
                    if (wan == j)
                    {
                        if (missingNumber[j] == Num)
                            missingNumber[j] = i;
                    }

                    if (qian == j)
                    {
                        if (missingNumber[10 + j] == Num)
                            missingNumber[10 + j] = i;
                    }

                    if (bai == j)
                    {
                        if (missingNumber[20 + j] == Num)
                            missingNumber[20 + j] = i;
                    }

                    if (shi == j)
                    {
                        if (missingNumber[30 + j] == Num)
                            missingNumber[30 + j] = i;
                    }

                    if (ge == j)
                    {
                        if (missingNumber[40 + j] == Num)
                            missingNumber[40 + j] = i;
                    }
                }
            }


            int[] Nums = new int[10];
            for (int i = 0; i < 10; i++)
            {
                Nums[i] = missingNumber[i];
            }
            for (int i = 0; i < 10; i++)
            {
                temp.Add(missingNumber[i]);
                tempSb.Append(missingNumber[i].ToString() + ",");
            }
            getMissingDetail.Add(temp.ToList());
            getMissingDetailDict.Add("1", tempSb.Remove(tempSb.Length - 1, 1).ToString());
            temp.Clear();
            tempSb.Clear();
            for (int i = 10; i < 20; i++)
            {
                Nums[i - 10] = missingNumber[i];
            }
            for (int i = 10; i < 20; i++)
            {
                temp.Add(missingNumber[i]);
                tempSb.Append(missingNumber[i].ToString() + ",");
            }
            getMissingDetail.Add(temp.ToList());
            getMissingDetailDict.Add("2", tempSb.Remove(tempSb.Length - 1, 1).ToString());
            temp.Clear();
            tempSb.Clear();
            for (int i = 20; i < 30; i++)
            {
                Nums[i - 20] = missingNumber[i];
            }
            for (int i = 20; i < 30; i++)
            {
                temp.Add(missingNumber[i]);
                tempSb.Append(missingNumber[i].ToString() + ",");
            }
            getMissingDetail.Add(temp.ToList());
            getMissingDetailDict.Add("3", tempSb.Remove(tempSb.Length - 1, 1).ToString());
            temp.Clear();
            tempSb.Clear();
            for (int i = 30; i < 40; i++)
            {
                Nums[i - 30] = missingNumber[i];
            }
            for (int i = 30; i < 40; i++)
            {
                temp.Add(missingNumber[i]);
                tempSb.Append(missingNumber[i].ToString() + ",");
            }
            getMissingDetail.Add(temp.ToList());
            getMissingDetailDict.Add("4", tempSb.Remove(tempSb.Length - 1, 1).ToString());
            temp.Clear();
            tempSb.Clear();
            for (int i = 40; i < 50; i++)
            {
                Nums[i - 40] = missingNumber[i];
            }
            for (int i = 40; i < 50; i++)
            {
                temp.Add(missingNumber[i]);
                tempSb.Append(missingNumber[i].ToString() + ",");
            }
            getMissingDetail.Add(temp.ToList());
            getMissingDetailDict.Add("5", tempSb.Remove(tempSb.Length - 1, 1).ToString());
            temp.Clear();
            tempSb.Clear();
        }
        #endregion

        #region sscBuyType==6 组三 sscBuyType==7 组六
        else if (sscBuyType == 6 || sscBuyType == 7)
        {
            DataTable dtOmission = new DataTable("SHSSL_Omission");//用临时表SHSSL_Omission初始化
            dtOmission.Columns.Add("Number", System.Type.GetType("System.String"));//期号
            dtOmission.Columns.Add("WinNumber", System.Type.GetType("System.String"));//中奖号码
            dtOmission.Columns.Add("Type", System.Type.GetType("System.Int32"));//是否是组三或主六 0表示都组三，1表示组六

            DataRow dr = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dtOmission.NewRow();
                //dr["Number"] = dt.Rows[i]["Name"].ToString();
                dr["WinNumber"] = dt.Rows[i]["WinLotteryNumber"].ToString().Substring(2, 3);
                if (Get3XZSorZL(dt.Rows[i]["WinLotteryNumber"].ToString().Substring(2, 3)) == "0")
                {
                    dr["Type"] = 0;
                }
                else
                {
                    dr["Type"] = 1;
                }
                dtOmission.Rows.Add(dr);
            }
            int[] missingNumber = new int[10];//保存遗漏期数数组
            int Num = dtOmission.Rows.Count;//查询最近200期开奖号码，将遗漏期数默认为200
            #region 组三
            if (sscBuyType == 6)
            {
                for (int i = 0; i < missingNumber.Length; i++)
                    missingNumber[i] = Num;

                for (int i = 0; i < dtOmission.Rows.Count; i++)
                {
                    string lostPrieod = dtOmission.Rows[i]["WinNumber"].ToString().Trim();
                    string strType = dtOmission.Rows[i]["Type"].ToString().Trim();
                    if (lostPrieod.Length != 3)
                        continue;

                    for (int j = 0; j <= 9; j++)
                    {
                        if (lostPrieod.Contains(j.ToString()))
                        {
                            if (missingNumber[j] == Num && strType == "0")
                                missingNumber[j] = i;
                        }
                    }
                }
                int[] Nums = new int[10];
                for (int i = 0; i < 10; i++)
                {
                    Nums[i] = missingNumber[i];
                }
                List<int> temp = new List<int>();
                StringBuilder tempSb = new StringBuilder();
                for (int i = 0; i < 10; i++)
                {
                    temp.Add(missingNumber[i]);
                    tempSb.Append(missingNumber[i].ToString() + ",");
                }
                getMissingDetail.Add(temp);
                getMissingDetailDict.Add("1", tempSb.Remove(tempSb.Length - 1, 1).ToString());
            }
            #endregion

            #region  (组六：后三位数字中多不相同相同)
            else
            {
                dtOmission = new DataTable("SHSSL_Omission");//用临时表SHSSL_Omission初始化

                dtOmission.Columns.Add("Number", System.Type.GetType("System.String"));//期号
                dtOmission.Columns.Add("WinNumber", System.Type.GetType("System.String"));//中奖号码
                dtOmission.Columns.Add("Type", System.Type.GetType("System.Int32"));//是否是组三或主六 0表示都组三，1表示组六

                dr = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dtOmission.NewRow();
                    //dr["Number"] = dt.Rows[i]["Name"].ToString();
                    dr["WinNumber"] = dt.Rows[i]["WinLotteryNumber"].ToString().Substring(2, 3);
                    if (Get3XZSorZL(dt.Rows[i]["WinLotteryNumber"].ToString().Substring(2, 3)) == "0")
                    {
                        dr["Type"] = 0;
                    }
                    else
                    {
                        dr["Type"] = 1;
                    }
                    dtOmission.Rows.Add(dr);
                }
            #endregion


                missingNumber = new int[10];//保存遗漏期数数组

                Num = dtOmission.Rows.Count;//查询最近100期开奖号码，将遗漏期数默认为200
                for (int i = 0; i < missingNumber.Length; i++)
                    missingNumber[i] = Num;

                for (int i = 0; i < dtOmission.Rows.Count; i++)
                {
                    string lostPrieod = dtOmission.Rows[i]["WinNumber"].ToString().Trim();
                    string strType = dtOmission.Rows[i]["Type"].ToString().Trim();
                    if (lostPrieod.Length != 3)
                        continue;

                    for (int j = 0; j <= 9; j++)
                    {
                        if (lostPrieod.Contains(j.ToString()))
                        {
                            if (missingNumber[j] == Num && strType == "1")
                                missingNumber[j] = i;
                        }
                    }
                }
                int[] Nums = new int[10];
                List<int> temp = new List<int>();
                StringBuilder tempSb = new StringBuilder();
                for (int i = 0; i < 10; i++)
                {
                    Nums[i] = missingNumber[i];
                }
                for (int i = 0; i < 10; i++)
                {
                    temp.Add(missingNumber[i]);
                    tempSb.Append(missingNumber[i].ToString() + ",");
                }
                getMissingDetail.Add(temp);
                getMissingDetailDict.Add("1", tempSb.Remove(tempSb.Length - 1, 1).ToString());
            }
        }
        #endregion

        #region sscBuyType=8 二星组选
        else if (sscBuyType == 8)
        {
            string temp = WriteOmssionFor28For2806(dt);
            //getMissingDetail.Add(temp);
            getMissingDetailDict.Add("1", temp);
        }
        #endregion

        #region sscBuyType=9 五星组选120遗漏
        else if (sscBuyType == 9)
        {
            getMissingDetailDict = YL5XZX120Mission(dt);
        }
        #endregion

        #region sscBuyType=10 五星组选60遗漏
        else if (sscBuyType == 10)
        {
            getMissingDetailDict = YL5XZX60Mission(dt);
        }
        #endregion

        #region sscBuyType=11 五星组选组选30遗漏
        else if (sscBuyType == 11)
        {
            getMissingDetailDict = YL5XZX30Mission(dt);
        }
        #endregion

        #region sscBuyType=12 五星组选组选20遗漏
        else if (sscBuyType == 12)
        {
            getMissingDetailDict = YL5XZX20Mission(dt);
        }
        #endregion

        #region sscBuyType=13 五星组选组选10遗漏
        else if (sscBuyType == 13)
        {
            getMissingDetailDict = YL5XZX10Mission(dt);
        }
        #endregion

        #region sscBuyType=14 五星组选组选5遗漏
        else if (sscBuyType == 14)
        {
            getMissingDetailDict = YL5XZX5Mission(dt);
        }
        #endregion

        #region sscBuyType=15 五星好事成双遗漏
        else if (sscBuyType == 15)
        {
            getMissingDetailDict = YL5XHSCSMission(dt);
        }
        #endregion

        #region sscBuyType=16 五星三星报喜遗漏
        else if (sscBuyType == 16)
        {
            getMissingDetailDict = YL5XSXBXMission(dt);
        }
        #endregion

        #region sscBuyType=17 五星四季发财遗漏
        else if (sscBuyType == 17)
        {
            getMissingDetailDict = YL5X4JFCMission(dt);
        }
        #endregion

        #region sscBuyType=18 四星组选24遗漏
        else if (sscBuyType == 18)
        {
            getMissingDetailDict = YL4XZXZH24Mission(dt);
        }
        #endregion

        #region sscBuyType=19 四星组选12遗漏
        else if (sscBuyType == 19)
        {
            getMissingDetailDict = YL4XZXZH12Mission(dt);
        }
        #endregion

        #region sscBuyType=20 四星组选组选6遗漏
        else if (sscBuyType == 20)
        {
            getMissingDetailDict = YL4XZXZHZX6Mission(dt);
        }
        #endregion

        #region sscBuyType=21 四星组选组选4遗漏
        else if (sscBuyType == 21)
        {
            getMissingDetailDict = YL4XZXZHZX4Mission(dt);
        }
        #endregion

        return getMissingDetailDict;
    }
    #endregion

    #region 广东11选5、江西11选5、十一运夺金 遗漏值
    /// <summary>
    /// 广东11选5、江西11选5、十一运夺金 遗漏值
    /// </summary>
    /// <param name="lotteryId">彩种ID</param>
    /// <param name="sscBuyType"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GaoPinMiss(int lotteryId, int sscBuyType)
    {
        //未结束的期
        DataTable dt = new DAL.Tables.T_Isuses().Open("Top 100 WinLotteryNumber", "LotteryID=" + lotteryId.ToString() + " and IsOpened = 1 and WinLotteryNumber <> ''", "[EndTime] desc");
        List<string> numList = new List<string>() { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11" };
        Dictionary<string, string> resultList = new Dictionary<string, string>();
        if (sscBuyType == 1)
        {
            int[] missNum = new int[11];
            //任选二到任选8的遗漏值计算
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    List<string> listLotteryNum = dt.Rows[j]["WinLotteryNumber"].ToString().Split(' ').ToList();//开奖号码

                    if (listLotteryNum.Contains(numList[i]) == true)
                    {
                        missNum[i] = j;
                        break;
                    }
                    //遗漏值
                    missNum[i] = j + 1;
                }
            }
            StringBuilder tempList = new StringBuilder();
            for (int i = 0; i < missNum.Length; i++)
            {
                tempList.Append(missNum[i].ToString() + ",");
            }
            resultList.Add("1", tempList.Remove(tempList.Length - 1, 1).ToString());
        }
        //前三普通投注
        if (sscBuyType == 2)
        {
            //万位
            resultList.Add("1", ReturnMissNums(dt, numList, 0));
            //千位
            resultList.Add("2", ReturnMissNums(dt, numList, 1));
            //百位
            resultList.Add("3", ReturnMissNums(dt, numList, 2));
        }
        //前三胆拖计算
        if (sscBuyType == 3)
        {
            int[] missNum = new int[11];
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    List<string> listLotteryNum = dt.Rows[j]["WinLotteryNumber"].ToString().Split(' ').ToList();//开奖号码
                    List<string> listQianSan = new List<string>() { listLotteryNum[0], listLotteryNum[1], listLotteryNum[2] };
                    if (listQianSan.Contains(numList[i]) == true)
                    {
                        missNum[i] = j;
                        break;
                    }
                    missNum[i] = j + 1;//遗漏值
                }
            }
            StringBuilder tempList = new StringBuilder();
            for (int i = 0; i < missNum.Length; i++)
            {
                tempList.Append(missNum[i].ToString() + ",");
            }
            resultList.Add("1", tempList.Remove(tempList.Length - 1, 1).ToString());
        }
        //前二胆拖计算
        if (sscBuyType == 4)
        {
            int[] missNum = new int[11];
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    //开奖号码
                    List<string> listLotteryNum = dt.Rows[j]["WinLotteryNumber"].ToString().Split(' ').ToList();
                    List<string> listQianSan = new List<string>() { listLotteryNum[0], listLotteryNum[1] };
                    if (listQianSan.Contains(numList[i]) == true)
                    {
                        missNum[i] = j;
                        break;
                    }
                    //遗漏值
                    missNum[i] = j + 1;
                }
            }
            StringBuilder tempList = new StringBuilder();
            for (int i = 0; i < missNum.Length; i++)
            {
                tempList.Append(missNum[i].ToString() + ",");
            }
            resultList.Add("1", tempList.Remove(tempList.Length - 1, 1).ToString());
        }
        return resultList;
    }
    #endregion

    #region 江苏快三 遗漏值
    public static Dictionary<string, string> JSKMiss(int lotteryId, int sscBuyType)
    {
        //未结束的期
        DataTable dt = new DAL.Tables.T_Isuses().Open("Top 100 WinLotteryNumber", "LotteryID=" + lotteryId.ToString() + " and IsOpened = 1 and WinLotteryNumber <> ''", "[EndTime] desc");

        Dictionary<string, string> resultList = new Dictionary<string, string>();
        if (sscBuyType == 6 || sscBuyType == 7)
        {
            int[] missingNumber = new int[6];
            string[] number = new string[6] { "11", "22", "33", "44", "55", "66" };
            int count = dt.Rows.Count;
            for (int i = 0; i < missingNumber.Length; i++)
                missingNumber[i] = count;


            for (int i = 1; i <= count; i++)
            {
                string lostPrieod = dt.Rows[i - 1]["WinLotteryNumber"].ToString().Trim();
                if (lostPrieod.Length != 3)
                    continue;

                for (int x = 0; x < number.Length; x++)
                {
                    if (lostPrieod.IndexOf(number[x]) >= 0)
                    {
                        if (missingNumber[x] != count)
                        {
                            continue;
                        }
                        missingNumber[x] = i - 1;
                    }
                }
            }
            StringBuilder tempList = new StringBuilder();
            for (int k = 0; k < 6; k++)
            {
                tempList.Append(missingNumber[k].ToString() + ",");
            }
            resultList.Add("1", tempList.Remove(tempList.Length - 1, 1).ToString());
            //如果是二同号单选
            if (sscBuyType == 7)
            {
                int[] missingNumber1 = new int[6];
                string[] number1 = new string[6] { "1", "2", "3", "4", "5", "6" };
                int count1 = dt.Rows.Count;
                for (int i = 0; i < missingNumber1.Length; i++)
                    missingNumber1[i] = count1;
                //missingNumber
                //number
                //count
                for (int i = 1; i <= count1; i++)
                {
                    string lostPrieod = dt.Rows[i - 1]["WinLotteryNumber"].ToString().Trim();
                    if (lostPrieod.Length != 3)
                        continue;

                    for (int x = 0; x < number1.Length; x++)
                    {
                        if (lostPrieod.IndexOf(number1[x]) >= 0)
                        {
                            if (missingNumber1[x] != count1)
                            {
                                continue;
                            }
                            missingNumber1[x] = i - 1;
                        }
                    }
                }
                int[] Nums = new int[6];
                StringBuilder tempList1 = new StringBuilder();
                for (int i = 0; i < 6; i++)
                {
                    Nums[i] = missingNumber1[i];
                }
                for (int k = 0; k < 6; k++)
                {
                    tempList1.Append(missingNumber1[k].ToString() + ",");
                }
                int tempI = 1;
                if (resultList.Count() > 0)
                {
                    tempI++;
                }
                resultList.Add(tempI.ToString(), tempList1.Remove(tempList1.Length - 1, 1).ToString());

            }
        }
        if (sscBuyType == 4 || sscBuyType == 8)
        {
            int[] missingNumber = new int[6];
            string[] number = new string[6] { "1", "2", "3", "4", "5", "6" };
            int count = dt.Rows.Count;
            for (int i = 0; i < missingNumber.Length; i++)
                missingNumber[i] = count;


            for (int i = 1; i <= count; i++)
            {
                string lostPrieod = dt.Rows[i - 1]["WinLotteryNumber"].ToString().Trim();
                if (lostPrieod.Length != 3)
                    continue;

                for (int x = 0; x < number.Length; x++)
                {
                    if (lostPrieod.IndexOf(number[x]) >= 0)
                    {
                        if (missingNumber[x] != count)
                        {
                            continue;
                        }
                        missingNumber[x] = i - 1;
                    }
                }
            }

            int[] Nums = new int[6];
            StringBuilder tempList = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                Nums[i] = missingNumber[i];
            }
            for (int k = 0; k < 6; k++)
            {
                tempList.Append(missingNumber[k].ToString() + ",");
            }
            resultList.Add("1", tempList.Remove(tempList.Length - 1, 1).ToString());
        }
        else if (sscBuyType == 1)//和值
        {
            DataRow[] dr;
            DataSet ds = null;
            if (DAL.Procedures.P_TrendChart_JSK3_HZ(ref ds, 100) < 0)
            {
                return null;
            }
            else
            {
                if (ds.Tables.Count > 0)
                {
                    dr = ds.Tables[0].Select("", "isuse desc");
                    StringBuilder tempList = new StringBuilder();
                    if (dr != null && dr.Length >= 1)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            tempList.Append(dr[0]["H_" + (i + 3)] + ",");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            tempList.Append("0,");
                        }
                    }
                    resultList.Add("1", tempList.Remove(tempList.Length - 1, 1).ToString());
                }
            }
        }
        if (sscBuyType == 2)
        {
            string[] number = new string[6] { "111", "222", "333", "444", "555", "666" };
            int count = dt.Rows.Count;
            int missCount = count;

            for (int i = 1; i <= count; i++)
            {
                string lostPrieod = dt.Rows[i - 1]["WinLotteryNumber"].ToString().Trim();
                if (lostPrieod.Length != 3)
                    continue;

                for (int x = 0; x < number.Length; x++)
                {
                    if (lostPrieod.IndexOf(number[x]) >= 0)
                    {
                        missCount++;
                    }
                }
            }

            StringBuilder tempList = new StringBuilder();
            tempList.Append(missCount);
            resultList.Add("1", tempList.ToString());
        }
        if (sscBuyType == 3)
        {
            int[] missingNumber = new int[6];
            string[] number = new string[6] { "111", "222", "333", "444", "555", "666" };
            int count = dt.Rows.Count;
            for (int i = 0; i < missingNumber.Length; i++)
                missingNumber[i] = count;

            for (int i = 1; i <= count; i++)
            {
                string lostPrieod = dt.Rows[i - 1]["WinLotteryNumber"].ToString().Trim();
                if (lostPrieod.Length != 3)
                    continue;

                for (int x = 0; x < number.Length; x++)
                {
                    if (lostPrieod.IndexOf(number[x]) >= 0)
                    {
                        if (missingNumber[x] != count)
                        {
                            continue;
                        }
                        missingNumber[x] = i - 1;
                    }
                }
            }
            StringBuilder tempList = new StringBuilder();
            for (int k = 0; k < 6; k++)
            {
                tempList.Append(missingNumber[k].ToString() + ",");
            }
            resultList.Add("1", tempList.Remove(tempList.Length - 1, 1).ToString());
        }
        if (sscBuyType == 5)//三连号
        {
            int missingNumber = 0;
            string[] number = new string[4] { "123", "234", "345", "456" };
            int count = dt.Rows.Count;
            missingNumber = count;

            int missCount = 0;
            for (int i = 1; i <= count; i++)
            {
                string lostPrieod = dt.Rows[i - 1]["WinLotteryNumber"].ToString().Trim();
                if (lostPrieod.Length != 3)
                    continue;

                for (int x = 0; x < number.Length; x++)
                {
                    if (lostPrieod.IndexOf(number[x]) >= 0)
                    {
                        missCount = i - 1;
                    }
                }

            }
            StringBuilder tempList = new StringBuilder();
            tempList.Append(missCount.ToString());
            resultList.Add("1", tempList.Remove(tempList.Length - 1, 1).ToString());
        }
        return resultList;
    }
    #endregion

    private static string ReturnMissNums(DataTable dt, List<string> numList, int index)
    {
        //遗漏值
        List<int> missNum = new List<int>(11);
        for (int i = 0; i < numList.Count; i++)
        {
            missNum.Add(0);
        }
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                //开奖号码
                List<string> listLotteryNum = dt.Rows[j]["WinLotteryNumber"].ToString().Split(' ').ToList();
                string num = listLotteryNum[index];

                if (numList[i] == num)
                {
                    missNum[i] = j;
                    break;
                }
                //万位遗漏值
                missNum[i] = j + 1;
            }
        }
        StringBuilder tempSb = new StringBuilder();
        for (int i = 0; i < missNum.Count; i++)
        {
            tempSb.Append(missNum[i].ToString() + ",");
        }
        return tempSb.Remove(tempSb.Length - 1, 1).ToString();
    }

    #region 江苏快三遗漏值

    #endregion

    #region 五星组选120遗漏
    private static Dictionary<string, string> YL5XZX120Mission(DataTable dt)
    {
        int[] missingNumber = new int[10];
        int Num = dt.Rows.Count;

        for (int i = 0; i < missingNumber.Length; i++)
            missingNumber[i] = Num;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string lostPrieod = dt.Rows[i]["WinLotteryNumber"].ToString().Trim();
            if (lostPrieod.Length != 5)
                continue;

            for (int j = 0; j <= 9; j++)
            {
                if (lostPrieod.Contains(j.ToString()))
                {
                    if (missingNumber[j] == Num)
                        missingNumber[j] = i;
                }

            }
        }

        int[] Nums = new int[10];
        Dictionary<string, string> resultList = new Dictionary<string, string>();
        StringBuilder temp = new StringBuilder();
        for (int i = 0; i < 10; i++)
        {
            Nums[i] = missingNumber[i];
        }
        for (int i = 0; i < 10; i++)
        {
            temp.Append(missingNumber[i].ToString() + ",");
        }
        resultList.Add("1", temp.Remove(temp.Length - 1, 1).ToString());
        return resultList;
    }
    #endregion

    #region 五星组选60遗漏
    /// <summary>
    /// 五星组选60遗漏
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private static Dictionary<string, string> YL5XZX60Mission(DataTable dt)
    {
        int[] missingNumber = new int[20];
        int Num = dt.Rows.Count;

        for (int i = 0; i < missingNumber.Length; i++)
            missingNumber[i] = Num;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string lostPrieod = dt.Rows[i]["WinLotteryNumber"].ToString().Trim();
            if (lostPrieod.Length != 5)
                continue;
            if (Get5XZSorXT(lostPrieod, 2, 1) != "1")
                continue;

            for (int j = 0; j <= 9; j++)
            {
                //二重
                if (GetCharInStringCount(j.ToString(), lostPrieod) == 2)
                {
                    if (missingNumber[j] == Num)
                        missingNumber[j] = i;
                }
                //三单
                if (trimXT(lostPrieod, 2).Contains(j.ToString()))
                {
                    if (missingNumber[j + 10] == Num)
                        missingNumber[j + 10] = i;
                }
            }
        }


        int[] Nums = new int[20];
        Dictionary<string, string> resultList = new Dictionary<string, string>();
        for (int i = 0; i < 20; i++)
        {
            Nums[i] = missingNumber[i];
        }
        StringBuilder temp1 = new StringBuilder();
        StringBuilder temp2 = new StringBuilder();
        for (int i = 0; i < 20; i++)
        {
            if (i <= 9)
            {
                temp1.Append(missingNumber[i].ToString() + ",");
            }
            else
            {
                temp2.Append(missingNumber[i].ToString() + ",");
            }
        }
        resultList.Add("1", temp1.Remove(temp1.Length - 1, 1).ToString());
        resultList.Add("2", temp2.Remove(temp2.Length - 1, 1).ToString());
        return resultList;
    }
    #endregion

    #region 五星组选组选30遗漏
    /// <summary>
    /// 五星组选组选30遗漏
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private static Dictionary<string, string> YL5XZX30Mission(DataTable dt)
    {
        int[] missingNumber = new int[20];
        int Num = dt.Rows.Count;

        for (int i = 0; i < missingNumber.Length; i++)
            missingNumber[i] = Num;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string lostPrieod = dt.Rows[i]["WinLotteryNumber"].ToString().Trim();
            if (lostPrieod.Length != 5)
                continue;
            if (Get5XZSorXT(lostPrieod, 2, 2) != "1")
                continue;

            string numbs = lostPrieod;

            for (int j = 0; j <= 9; j++)
            {
                //二重
                if (GetCharInStringCount(j.ToString(), numbs) == 2)
                {
                    if (missingNumber[j] == Num)
                        missingNumber[j] = i;
                }
                //一单
                if (trimXT(numbs, 2).Contains(j.ToString()))
                {
                    if (missingNumber[j + 10] == Num)
                        missingNumber[j + 10] = i;
                }
            }
        }

        int[] Nums = new int[20];
        Dictionary<string, string> resultList = new Dictionary<string, string>();
        StringBuilder temp1 = new StringBuilder();
        StringBuilder temp2 = new StringBuilder();
        StringBuilder temp3 = new StringBuilder();
        for (int i = 0; i < 20; i++)
        {
            Nums[i] = missingNumber[i];
        }
        for (int i = 0; i < 20; i++)
        {
            if (i <= 9)
            {
                temp1.Append(missingNumber[i].ToString() + ",");
                temp2.Append(missingNumber[i].ToString() + ",");
            }
            else
            {
                temp3.Append(missingNumber[i].ToString() + ",");
            }
        }
        resultList.Add("1", temp1.Remove(temp1.Length - 1, 1).ToString());
        resultList.Add("2", temp2.Remove(temp2.Length - 1, 1).ToString());
        resultList.Add("3", temp3.Remove(temp3.Length - 1, 1).ToString());
        return resultList;
    }
    #endregion

    #region 五星组选组选20遗漏
    /// <summary>
    /// 五星组选组选20遗漏
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private static Dictionary<string, string> YL5XZX20Mission(DataTable dt)
    {
        int[] missingNumber = new int[20];
        int Num = dt.Rows.Count;

        for (int i = 0; i < missingNumber.Length; i++)
            missingNumber[i] = Num;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string lostPrieod = dt.Rows[i]["WinLotteryNumber"].ToString().Trim();
            if (lostPrieod.Length != 5)
                continue;
            if (Get5XZSorXT(lostPrieod, 3, 1) != "1")
                continue;

            string numbs = lostPrieod;

            for (int j = 0; j <= 9; j++)
            {
                //三重
                if (GetCharInStringCount(j.ToString(), numbs) == 3)
                {
                    if (missingNumber[j] == Num)
                        missingNumber[j] = i;
                }
                //二单
                if (trimXT(numbs, 3).Contains(j.ToString()))
                {
                    if (missingNumber[j + 10] == Num)
                        missingNumber[j + 10] = i;
                }
            }
        }

        int[] Nums = new int[20];
        Dictionary<string, string> resultList = new Dictionary<string, string>();
        StringBuilder temp1 = new StringBuilder();
        StringBuilder temp2 = new StringBuilder();
        for (int i = 0; i < 20; i++)
        {
            Nums[i] = missingNumber[i];
        }
        for (int i = 0; i < 20; i++)
        {
            if (i <= 9)
            {
                temp1.Append(missingNumber[i].ToString() + ",");
            }
            else
            {
                temp2.Append(missingNumber[i].ToString() + ",");
            }
        }
        resultList.Add("1", temp1.Remove(temp1.Length - 1, 1).ToString());
        resultList.Add("2", temp2.Remove(temp2.Length - 1, 1).ToString());
        return resultList;
    }
    #endregion

    #region 五星组选组选10遗漏
    /// <summary>
    /// 五星组选组选10遗漏
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private static Dictionary<string, string> YL5XZX10Mission(DataTable dt)
    {
        int[] missingNumber = new int[20];
        int Num = dt.Rows.Count;

        for (int i = 0; i < missingNumber.Length; i++)
            missingNumber[i] = Num;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string lostPrieod = dt.Rows[i]["WinLotteryNumber"].ToString().Trim();
            if (lostPrieod.Length != 5)
                continue;
            if (Get5XZSorXT(lostPrieod, 3, 1) != "1")//三重
                continue;

            string strtmp = trimXT(lostPrieod, 3);

            if (Get5XZSorXT(lostPrieod, 2, 1) != "1")//二重
                continue;

            string numbs = lostPrieod;

            for (int j = 0; j <= 9; j++)
            {
                //三重
                if (GetCharInStringCount(j.ToString(), numbs) == 3)
                {
                    if (missingNumber[j] == Num)
                        missingNumber[j] = i;
                }
                //二重
                if (GetCharInStringCount(j.ToString(), numbs) == 2)
                {
                    if (missingNumber[j + 10] == Num)
                        missingNumber[j + 10] = i;
                }
            }
        }

        int[] Nums = new int[20];
        Dictionary<string, string> resultList = new Dictionary<string, string>();
        StringBuilder temp1 = new StringBuilder();
        StringBuilder temp2 = new StringBuilder();
        for (int i = 0; i < 20; i++)
        {
            Nums[i] = missingNumber[i];
        }
        for (int i = 0; i < 20; i++)
        {
            if (i <= 9)
            {
                temp1.Append(missingNumber[i].ToString() + ",");
            }
            else
            {
                temp2.Append(missingNumber[i].ToString() + ",");
            }
        }
        resultList.Add("1", temp1.Remove(temp1.Length - 1, 1).ToString());
        resultList.Add("2", temp2.Remove(temp2.Length - 1, 1).ToString());
        return resultList;
    }
    #endregion

    #region 五星组选组选5遗漏
    /// <summary>
    /// 五星组选组选5遗漏
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private static Dictionary<string, string> YL5XZX5Mission(DataTable dt)
    {
        int[] missingNumber = new int[20];
        int Num = dt.Rows.Count;

        for (int i = 0; i < missingNumber.Length; i++)
            missingNumber[i] = Num;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string lostPrieod = dt.Rows[i]["WinLotteryNumber"].ToString().Trim();
            if (lostPrieod.Length != 5)
                continue;
            if (Get5XZSorXT(lostPrieod, 4, 1) != "1")//四重
                continue;

            string numbs = lostPrieod;

            for (int j = 0; j <= 9; j++)
            {
                //四重
                if (GetCharInStringCount(j.ToString(), numbs) == 4)
                {
                    if (missingNumber[j] == Num)
                        missingNumber[j] = i;
                }
                //一单
                if (trimXT(lostPrieod, 4).Contains(j.ToString()))
                {
                    if (missingNumber[j + 10] == Num)
                        missingNumber[j + 10] = i;
                }
            }
        }

        int[] Nums = new int[20];
        Dictionary<string, string> resultList = new Dictionary<string, string>();
        StringBuilder temp1 = new StringBuilder();
        StringBuilder temp2 = new StringBuilder();
        for (int i = 0; i < 20; i++)
        {
            Nums[i] = missingNumber[i];
        }
        for (int i = 0; i < 20; i++)
        {
            if (i <= 9)
            {
                temp1.Append(missingNumber[i].ToString() + ",");
            }
            else
            {
                temp2.Append(missingNumber[i].ToString() + ",");
            }
        }
        resultList.Add("1", temp1.Remove(temp1.Length - 1, 1).ToString());
        resultList.Add("2", temp2.Remove(temp2.Length - 1, 1).ToString());
        return resultList;
    }
    #endregion

    #region 五星好事成双遗漏
    /// <summary>
    /// 五星好事成双遗漏
    /// </summary>
    /// <param name="dt"></param>
    private static Dictionary<string, string> YL5XHSCSMission(DataTable dt)
    {
        int[] missingNumber = new int[10];
        int Num = dt.Rows.Count;

        for (int i = 0; i < missingNumber.Length; i++)
            missingNumber[i] = Num;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string lostPrieod = dt.Rows[i]["WinLotteryNumber"].ToString().Trim();
            if (lostPrieod.Length != 5)
                continue;
            string numbs = lostPrieod;


            for (int j = 0; j <= 9; j++)
            {
                if (GetCharInStringCount(j.ToString(), numbs) > 1)
                {
                    if (missingNumber[j] == Num)
                        missingNumber[j] = i;
                }
            }
        }

        int[] Nums = new int[10];
        Dictionary<string, string> resultList = new Dictionary<string, string>();
        StringBuilder temp = new StringBuilder();
        for (int i = 0; i < 10; i++)
        {
            Nums[i] = missingNumber[i];
        }
        for (int i = 0; i < 10; i++)
        {
            temp.Append(missingNumber[i].ToString() + ",");
        }
        resultList.Add("1", temp.Remove(temp.Length - 1, 1).ToString());
        return resultList;
    }
    #endregion

    #region 五星三星报喜遗漏
    /// <summary>
    /// 五星三星报喜遗漏
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private static Dictionary<string, string> YL5XSXBXMission(DataTable dt)
    {
        int[] missingNumber = new int[10];
        int Num = dt.Rows.Count;

        for (int i = 0; i < missingNumber.Length; i++)
            missingNumber[i] = Num;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string lostPrieod = dt.Rows[i]["WinLotteryNumber"].ToString().Trim();
            if (lostPrieod.Length != 5)
                continue;
            string numbs = lostPrieod;


            for (int j = 0; j <= 9; j++)
            {
                if (GetCharInStringCount(j.ToString(), numbs) > 2)
                {
                    if (missingNumber[j] == Num)
                        missingNumber[j] = i;
                }
            }
        }

        int[] Nums = new int[10];
        Dictionary<string, string> resultList = new Dictionary<string, string>();
        StringBuilder temp = new StringBuilder();
        for (int i = 0; i < 10; i++)
        {
            Nums[i] = missingNumber[i];
        }
        for (int i = 0; i < 10; i++)
        {
            temp.Append(missingNumber[i].ToString() + ",");
        }
        resultList.Add("1", temp.Remove(temp.Length - 1, 1).ToString());
        return resultList;
    }
    #endregion

    #region 五星四季发财遗漏
    /// <summary>
    /// 五星四季发财遗漏
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private static Dictionary<string, string> YL5X4JFCMission(DataTable dt)
    {
        int[] missingNumber = new int[10];
        int Num = dt.Rows.Count;

        for (int i = 0; i < missingNumber.Length; i++)
            missingNumber[i] = Num;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string lostPrieod = dt.Rows[i]["WinLotteryNumber"].ToString().Trim();
            if (lostPrieod.Length != 5)
                continue;
            string numbs = lostPrieod;


            for (int j = 0; j <= 9; j++)
            {
                if (GetCharInStringCount(j.ToString(), numbs) > 3)
                {
                    if (missingNumber[j] == Num)
                        missingNumber[j] = i;
                }
            }
        }

        int[] Nums = new int[10];
        Dictionary<string, string> resultList = new Dictionary<string, string>();
        StringBuilder temp = new StringBuilder();
        for (int i = 0; i < 10; i++)
        {
            Nums[i] = missingNumber[i];
        }
        for (int i = 0; i < 10; i++)
        {
            temp.Append(missingNumber[i].ToString() + ",");
        }
        resultList.Add("1", temp.Remove(temp.Length - 1, 1).ToString());
        return resultList;
    }
    #endregion

    #region 四星组选24遗漏
    /// <summary>
    /// 四星组选24遗漏
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private static Dictionary<string, string> YL4XZXZH24Mission(DataTable dt)
    {
        int[] missingNumber = new int[10];
        int Num = dt.Rows.Count;

        for (int i = 0; i < missingNumber.Length; i++)
            missingNumber[i] = Num;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string lostPrieod = dt.Rows[i]["WinLotteryNumber"].ToString().Trim();
            if (lostPrieod.Length != 5)
                continue;
            string numbs = lostPrieod.Substring(1, 4);

            for (int j = 0; j <= 9; j++)
            {
                if (numbs.Contains(j.ToString()))
                {
                    if (missingNumber[j] == Num)
                        missingNumber[j] = i;
                }
            }
        }

        int[] Nums = new int[10];
        Dictionary<string, string> resultList = new Dictionary<string, string>();
        StringBuilder temp = new StringBuilder();
        for (int i = 0; i < 10; i++)
        {
            Nums[i] = missingNumber[i];
        }
        for (int i = 0; i < 10; i++)
        {
            temp.Append(missingNumber[i].ToString() + ",");
        }
        resultList.Add("1", temp.Remove(temp.Length - 1, 1).ToString());
        return resultList;
    }
    #endregion

    #region 四星组选12遗漏
    /// <summary>
    /// 四星组选12遗漏
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private static Dictionary<string, string> YL4XZXZH12Mission(DataTable dt)
    {
        int[] missingNumber = new int[20];
        int Num = dt.Rows.Count;

        for (int i = 0; i < missingNumber.Length; i++)
            missingNumber[i] = Num;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string lostPrieod = dt.Rows[i]["WinLotteryNumber"].ToString().Trim();
            if (lostPrieod.Length != 5)
                continue;

            string numbs = lostPrieod.Substring(1, 4);

            if (Get4XZSorXT(numbs, 2, 1) != "1")
                continue;

            for (int j = 0; j <= 9; j++)
            {
                //二重
                if (GetCharInStringCount(j.ToString(), numbs) == 2)
                {
                    if (missingNumber[j] == Num)
                        missingNumber[j] = i;
                }
                //二单
                if (trimXT(numbs, 2).Contains(j.ToString()))
                {
                    if (missingNumber[j + 10] == Num)
                        missingNumber[j + 10] = i;
                }
            }
        }

        int[] Nums = new int[20];
        Dictionary<string, string> resultList = new Dictionary<string, string>();
        StringBuilder temp1 = new StringBuilder();
        StringBuilder temp2 = new StringBuilder();
        for (int i = 0; i < 20; i++)
        {
            Nums[i] = missingNumber[i];
        }
        for (int i = 0; i < 20; i++)
        {
            if (i <= 9)
            {
                temp1.Append(missingNumber[i].ToString() + ",");
            }
            else
            {
                temp2.Append(missingNumber[i].ToString() + ",");
            }
        }
        resultList.Add("1", temp1.Remove(temp1.Length - 1, 1).ToString());
        resultList.Add("2", temp2.Remove(temp2.Length - 1, 1).ToString());
        return resultList;
    }
    #endregion

    #region 四星组选组选6遗漏
    /// <summary>
    /// 四星组选组选6遗漏
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private static Dictionary<string, string> YL4XZXZHZX6Mission(DataTable dt)
    {
        int[] missingNumber = new int[10];
        int Num = dt.Rows.Count;

        for (int i = 0; i < missingNumber.Length; i++)
            missingNumber[i] = Num;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string lostPrieod = dt.Rows[i]["WinLotteryNumber"].ToString().Trim();
            if (lostPrieod.Length != 5)
                continue;

            string numbs = lostPrieod.Substring(1, 4);

            if (Get4XZSorXT(numbs, 2, 2) != "1")
                continue;

            for (int j = 0; j <= 9; j++)
            {
                //二重 二重
                if (GetCharInStringCount(j.ToString(), numbs) == 2)
                {
                    if (missingNumber[j] == Num)
                        missingNumber[j] = i;
                }
            }
        }

        int[] Nums = new int[10];
        Dictionary<string, string> resultList = new Dictionary<string, string>();
        StringBuilder temp1 = new StringBuilder();
        StringBuilder temp2 = new StringBuilder();
        for (int i = 0; i < 10; i++)
        {
            Nums[i] = missingNumber[i];
        }
        for (int i = 0; i < 10; i++)
        {
            temp1.Append(missingNumber[i].ToString() + ",");
            temp2.Append(missingNumber[i].ToString() + ",");
        }
        resultList.Add("1", temp1.Remove(temp1.Length - 1, 1).ToString());
        resultList.Add("2", temp2.Remove(temp2.Length - 1, 1).ToString());
        return resultList;
    }
    #endregion

    #region 四星组选组选4遗漏
    /// <summary>
    /// 四星组选组选4遗漏
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private static Dictionary<string, string> YL4XZXZHZX4Mission(DataTable dt)
    {
        int[] missingNumber = new int[20];
        int Num = dt.Rows.Count;

        for (int i = 0; i < missingNumber.Length; i++)
            missingNumber[i] = Num;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string lostPrieod = dt.Rows[i]["WinLotteryNumber"].ToString().Trim();
            if (lostPrieod.Length != 5)
                continue;

            string numbs = lostPrieod.Substring(1, 4);

            if (Get4XZSorXT(numbs, 3, 1) != "1")
                continue;

            for (int j = 0; j <= 9; j++)
            {
                //三重
                if (GetCharInStringCount(j.ToString(), numbs) == 3)
                {
                    if (missingNumber[j] == Num)
                        missingNumber[j] = i;
                }
                //一单
                if (trimXT(numbs, 3).Contains(j.ToString()))
                {
                    if (missingNumber[j + 10] == Num)
                        missingNumber[j + 10] = i;
                }
            }
        }

        int[] Nums = new int[20];
        StringBuilder temp1 = new StringBuilder();
        StringBuilder temp2 = new StringBuilder();
        Dictionary<string, string> resultList = new Dictionary<string, string>();
        for (int i = 0; i < 20; i++)
        {
            Nums[i] = missingNumber[i];
        }
        for (int i = 0; i < 20; i++)
        {
            if (i <= 9)
            {
                temp1.Append(missingNumber[i].ToString() + ",");
            }
            else
            {
                temp2.Append(missingNumber[i].ToString() + ",");
            }
        }
        resultList.Add("1", temp1.ToString());
        resultList.Add("2", temp2.ToString());
        return resultList;
    }
    #endregion

    #region 输入5位数字，返回是否存在只有多少个相同的数字
    /// <summary>
    /// 输入5位数字，返回是否存在只有多少个相同的数字
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="num">相同的位数</param>
    /// <param name="count">对应的相同位数出现次数</param>
    /// <returns></returns>
    private static string Get5XZSorXT(string str, int num, int count)
    {
        string result = "0";
        string strTemp = "";
        if (str.Length == 5)
        {
            for (int i = 0; i < 10; i++)
            {
                if (GetCharInStringCount(i.ToString(), str) == num)
                {
                    strTemp += "1";
                }
            }
        }
        if (strTemp.Length == count)
        {
            result = "1";
        }
        return result;
    }
    #endregion

    #region 返回字符串在字符串中出现的次数
    /// <summary>
    /// 返回字符串在字符串中出现的次数
    /// </summary>
    /// <param name="Char">要检测出现的字符</param>
    /// <param name="String">要检测的字符串</param>
    /// <returns>出现次数</returns>

    private static int GetCharInStringCount(string Char, string String)
    {
        string str = String.Replace(Char, "");
        return (String.Length - str.Length) / Char.Length;
    }
    #endregion

    #region 去除字符串中多个个相同的字符
    /// <summary>
    /// 去除字符串中多个个相同的字符
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="count">出现次数</param>
    public static string trimXT(string str, int count)
    {
        string result = "";
        for (int i = 0; i < 10; i++)
        {
            if (GetCharInStringCount(i.ToString(), str) == count)
            {
                str = str.Replace(i.ToString(), "");
            }
        }

        result = str;
        return result;
    }
    #endregion

    #region 输入4位数字，返回是否存在只有多少个相同的数字
    /// <summary>
    /// 输入4位数字，返回是否存在只有多少个相同的数字
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="num">相同的位数</param>
    /// <param name="count">对应的相同位数出现次数</param>
    /// <returns></returns>
    private static string Get4XZSorXT(string str, int num, int count)
    {
        string result = "0";
        string strTemp = "";
        if (str.Length == 4)
        {
            for (int i = 0; i < 10; i++)
            {
                if (GetCharInStringCount(i.ToString(), str) == num)
                {
                    strTemp += "1";
                }
            }
        }
        if (strTemp.Length == count)
        {
            result = "1";
        }
        return result;
    }
    #endregion

    #region 初始化遗漏值
    /// <summary>
    /// 初始化遗漏值
    /// </summary>
    /// <param name="omission">遗漏值数组</param>
    /// <param name="defaultValue">默认值</param>
    private static void InitOmissionArray(int[] omission, int defaultValue)
    {
        for (int i = 0; i < omission.Length; i++)
        {
            omission[i] = defaultValue;
        }
    }
    #endregion

    #region 输入3位数字，返回组或组六
    /// <summary>
    /// 输入3位数字，返回组三或组六
    /// </summary>
    /// <param name="str"></param>
    /// <returns>0表示组三，1表示组六</returns>
    private static string Get3XZSorZL(string str)
    {
        if (str.Length == 3)
        {
            string Bai = str.Substring(0, 1);
            string Shi = str.Substring(1, 1);
            string Ge = str.Substring(2, 1);

            if (Bai == Shi || Bai == Ge || Shi == Ge)
            {
                return "0";
            }
            else
            {
                return "1";
            }
        }
        else
        {
            return "";
        }
    }
    #endregion

    #region 写入遗漏值28-2806（二星组选）
    /// <summary>
    /// 写入遗漏值28-2806（二星组选）
    /// </summary>
    /// <param name="dt">数据源</param>
    /// <returns></returns>
    private static string WriteOmssionFor28For2806(DataTable dt)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        string winLotteryNumber = "";
        int number1 = 0;
        int number2 = 0;
        int count = dt.Rows.Count;
        int[] omission = new int[10];
        InitOmissionArray(omission, count);
        bool[] omission1 = new bool[10];
        Regex reg = new Regex("\\d");
        for (int i = 0; i < count; i++)
        {
            winLotteryNumber = dt.Rows[i]["WinLotteryNumber"].ToString();
            MatchCollection matches = reg.Matches(winLotteryNumber);
            number1 = int.Parse(matches[3].Groups[0].Value);
            number2 = int.Parse(matches[4].Groups[0].Value);
            if (!omission1[number1])
            {
                omission1[number1] = true;
                omission[number1] = i;
            }
            if (!omission1[number2])
            {
                omission1[number2] = true;
                omission[number2] = i;
            }
        }
        //List<int> resultList = new List<int>();
        StringBuilder tempSb = new StringBuilder();
        //从遗漏数组中获取到最大的遗漏值
        int maxOmission = omission.Max();
        for (int i = 0; i < omission.Length; i++)
        {
            tempSb.Append(omission[i].ToString() + ",");
            //resultList.Add(omission[i]);
            //sb.Append("{");
            //sb.AppendFormat("\"number\":\"{0}\",\"omission\":\"{1}\",\"isMax\":\"{2}\"", i, omission[i], maxOmission == omission[i]);
            //sb.Append("}");
            //sb.Append(",");
        }
        //if (sb.Length > 1) sb.Remove(sb.Length - 1, 1);
        //sb.Append("]");
        return tempSb.Remove(tempSb.Length - 1, 1).ToString();
    }
    #endregion
}