using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;


namespace SLS
{
    public partial class Lottery
    {
        public class PlayType
        {
            public int ID;
            public string Name;

            public PlayType(int id, string name)
            {
                ID = id;
                Name = name;
            }
        }

        public class Ticket
        {
            public int PlayTypeID;
            public string Number;
            public int Multiple;
            public double Money;

            public Ticket(int playtype_id, string number, int multiple, double money)
            {
                PlayTypeID = playtype_id;
                Multiple = multiple;
                Number = number;
                Money = money;
            }

            public override string ToString()
            {
                return PlayTypeID.ToString() + "," + Multiple.ToString() + "," + Money.ToString() + "," + Number.Replace("\r\n", "\t").Replace("\n", "\t") + ";";
            }
        }

        public class LotteryBase
        {
            public int id;
            public string name;
            public string code;

            #region 虚拟方法
            public virtual bool CheckPlayType(int play_type)
            {
                return false;
            }

            public virtual string BuildNumber(int Num)
            {
                return "";
            }
            public virtual string BuildNumber(int Num, int Type)
            {
                return "";
            }
            public virtual string BuildNumber(int Red, int Blue, int Num)
            {
                return "";
            }

            public virtual string[] ToSingle(string Number, ref string CanonicalNumber, int PlayType)
            {
                return null;
            }
            public virtual double ComputeWin(int homeIndex,double allbet, double comboBet, double dxdsBet,string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, ref int WinCount, params double[] WinMoneyList)
            {
                return 0;
            }

            public virtual double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, ref int WinCount, params double[] WinMoneyList)
            {
                return 0;
            }

            public virtual double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params double[] WinMoneyList)
            {
                return 0;
            }

            public virtual double ComputeWin(string Number, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, params object[] WinMoneyList)
            {
                return 0;
            }

            public virtual double ComputeWin(string Scheme, string WinNumber, ref string Description, ref double WinMoneyNoWithTax, int PlayType, int CompetitionCount, string NoSignificance)
            {
                return 0;
            }

            public virtual string AnalyseScheme(string Content, int PlayType)
            {
                return "";
            }

            public virtual bool AnalyseWinNumber(string Number)
            {
                return true;
            }
            public virtual bool AnalyseWinNumber(string Number, int CompetitionCount)
            {
                return true;
            }

            public virtual int GetNum(int Number1, int Number2)
            {
                return 0;
            }

            public virtual string ShowNumber(string Number)
            {
                return "";
            }

            public virtual PlayType[] GetPlayTypeList()
            {
                return null;
            }

            public virtual string GetPrintKeyList(string Number, int PlayType_id, string LotteryMachine)
            {
                return "";
            }

            //足彩单场专用
            public virtual bool GetSchemeSplit(string Scheme, ref string BuyContent, ref string CnLocateWaysAndMultiples)
            {
                return true;
            }

            public virtual Ticket[] ToElectronicTicket_HPCQ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_HPSH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_HPJX(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_HPSD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_DYJ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_ZCW(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_ZZYTC(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_CTTCSD(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_BYSBJ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual string HPSH_ToElectronicTicket(int PlayTypeID, string Number, ref string TicketNumber, ref int NewPlayTypeID)
            {
                return "";
            }

            public virtual string HPJX_ToElectronicTicket(int PlayTypeID, string Number, ref string TicketNumber, ref int NewPlayTypeID)
            {
                return "";
            }

            public virtual Ticket[] ToElectronicTicket_XGCQ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_XGSH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_BJCP(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_HB(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }
            //北京单场
            public virtual string ToElectronicTicket_BJDC(int PlayTypeID, string Number, ref string TicketNumber, ref int NewPlayTypeID, ref string Rule, ref int Multiple, ref double Money, ref string GameNoList, ref string PassMode, ref int TicketCount)
            {
                return "";
            }

            #endregion

            protected class CompareToAscii : IComparer
            {
                int IComparer.Compare(Object x, Object y)
                {
                    return ((new CaseInsensitiveComparer()).Compare(x, y));
                }
            }

            protected bool isExistBall(ArrayList al, int Ball)
            {
                if (al.Count == 0)
                    return false;
                for (int i = 0; i < al.Count; i++)
                    if (int.Parse(al[i].ToString()) == Ball)
                        return true;
                return false;
            }

            protected string Sort(string str)
            {
                char[] ch = str.ToCharArray();
                Array.Sort(ch, new CompareToAscii());
                return new string(ch);
            }

            /// <summary>
            /// 默认根据'\n'来分割
            /// </summary>
            /// <param name="Number">投注号码</param>
            /// <param name="separator">分割字符</param>
            /// <returns></returns>
            protected string[] SplitLotteryNumber(string Number, char separator = '\n')
            {
                string[] s = Number.Split(separator);
                if (s.Length == 0)
                    return null;
                for (int i = 0; i < s.Length; i++)
                    s[i] = s[i].Trim();
                return s;
            }

            /// <summary>
            /// 默认根据'\n'来分割
            /// </summary>
            /// <param name="Number">投注号码</param>
            /// <param name="separator">分割字符</param>
            /// <returns></returns>
            protected string[] SplitNumber(string Number, char separator = '\n')
            {
                string[] nums = Number.Split(separator);

                for (int i = 0; i < nums.Length; i++)
                {
                    nums[i] = nums[i].Trim();
                }

                return nums;
            }

            /// <summary>
            /// 合并中奖描述
            /// </summary>
            protected void MergeWinDescription(ref string WinDescription, string AddDescription)
            {
                if (WinDescription != "")
                {
                    WinDescription += "，";
                }

                WinDescription += AddDescription;
            }

            /// <summary>
            /// 过滤掉彩票号前面以 [] 说明玩法的 [] 部分，如：时时乐，时时彩等使用
            /// </summary>
            protected string FilterPreFix(string Number)
            {
                if (!Number.StartsWith("[") && (Number.IndexOf("]") < 0))
                {
                    return Number;
                }

                return Number.Split(']')[1];
            }

            /// <summary>
            /// 获取彩票号前面以 [] 说明玩法的 [] 部分，如：时时乐，时时彩等使用
            /// </summary>
            protected string GetLotteryNumberPreFix(string Number)
            {
                if ((Number == null) || (Number == "") || (!Number.StartsWith("[")))
                {
                    return "";
                }

                return Number.Split(']')[0] + "]";
            }

            /// <summary>
            /// 合并彩票号前面以 [] 说明玩法的 [] 部分，如：时时乐，时时彩等使用
            /// </summary>
            protected string[] MergeLotteryNumberPreFix(string[] Numbers, string PreFix)
            {
                if ((Numbers == null) || (Numbers.Length == 0))
                {
                    return Numbers;
                }

                for (int i = 0; i < Numbers.Length; i++)
                {
                    Numbers[i] = PreFix + Numbers[i];
                }

                return Numbers;
            }

            /// <summary>
            /// 将开奖好按一定的格式输出给表现层,有些彩种会原样输出，有些彩种会增加空格等等。
            /// </summary>
            public string ShowNumber(string Number, string SpaceMark)
            {
                if (SpaceMark == "")
                    return Number;

                Number = Number.Replace(" ", "");
                string Result = "";
                for (int i = 0; i < Number.Length; i++)
                    Result += Number[i].ToString() + SpaceMark;

                return Result.Trim();
            }
            public virtual Ticket[] ToElectronicTicket_PLKJ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_PLKJ(DataTable dt, int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
            {
                return null;
            }

            public virtual Ticket[] ToElectronicTicket_PLKJ(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money, ref string PlayTypeNodes)
            {
                return null;
            }

        }

        public LotteryBase this[int Index]
        {
            get
            {
                switch (Index)
                {
                    
                    case 99:
                        return new BJXY28();
                    case 98:
                        return new JND28();
                    
                }
                return null;
            }
        }

        public LotteryBase this[string Name_or_Code_or_ID]
        {
            get
            {
                LotteryBase[] lotterys = GetLotterys();

                foreach (LotteryBase lottery in lotterys)
                {
                    if ((lottery.name == Name_or_Code_or_ID) || (lottery.code == Name_or_Code_or_ID) || (lottery.id.ToString() == Name_or_Code_or_ID))
                    {
                        return lottery;
                    }
                }

                return null;
            }
        }

        public LotteryBase[] GetLotterys()
        {
            int LotteryCount = 1;

            while (this[LotteryCount] != null)
            {
                LotteryCount++;
            }

            LotteryBase[] Lotterys = new LotteryBase[LotteryCount - 1];

            for (int i = 0; i < Lotterys.Length; i++)
            {
                Lotterys[i] = this[i + 1];
            }

            return Lotterys;
        }

        public string GetPlayTypeName(int PlayType)
        {
            LotteryBase[] lotterys = GetLotterys();

            foreach (LotteryBase lb in lotterys)
            {
                PlayType[] lbts = lb.GetPlayTypeList();

                foreach (PlayType lbt in lbts)
                {
                    if (lbt.ID == PlayType)
                    {
                        return lbt.Name;
                    }
                }
            }

            return "";
        }

        public int GetMaxLotteryID()
        {
            return this[GetLotterys().Length].id;
        }

        public bool ValidID(int LotteryID)
        {
            if (LotteryID==100) return true;

            if ((LotteryID < 1) || (LotteryID > GetMaxLotteryID()))
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// 统计中奖描述
        /// </summary>
        /// <param name="descList"></param>
        /// <returns></returns>
        public static string StatisticsWinDesc(List<string> descList)
        {
            try
            {
                for (int i = 0; i < descList.Count; i++)
                {
                    descList.Remove("");
                }
                StringBuilder totalDescription = new StringBuilder();
                List<string> noRepetList = new List<string>();
                for (int desListCount = 0; desListCount < descList.Count; desListCount++)
                {
                    if (!noRepetList.Contains(descList[desListCount]))
                    {
                        noRepetList.Add(descList[desListCount]);
                    }
                }
                for (int noReptIndex = 0; noReptIndex < noRepetList.Count; noReptIndex++)
                {
                    int zhuShu = descList.Where(p => p == noRepetList[noReptIndex]).Count();
                    Regex reg = new Regex(@"\d+注");
                    Match match = reg.Match(noRepetList[noReptIndex]);
                    zhuShu = zhuShu * Convert.ToInt32(match.Value.Replace("注", ""));
                    for (int k = 0; k < zhuShu; k++)
                    {
                        descList.Remove(noRepetList[noReptIndex]);
                    }
                    totalDescription.Append(reg.Replace(noRepetList[noReptIndex], zhuShu.ToString() + "注"));
                    totalDescription.Append("。");
                }
                return totalDescription.Replace("。。", "。").ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}