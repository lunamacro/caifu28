using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
///开奖公告详细帮助类
/// </summary>
public class OpenLotteryHelp
{
    public OpenLotteryHelp()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    #region 获取中奖信息bak
    /// <summary>
    /// 获取中奖信息
    /// </summary>
    /// <param name="lotteryId">彩种ID</param>
    /// <param name="isusesId">期号ID</param>
    /// <returns></returns>
    //public static List<WinEntity> GetWinLotteryInfo(string lotteryId, string isusesId)
    //{
    //    List<WinEntity> listWin = new List<WinEntity>();//中奖信息

    //    DataTable dtWin = new DAL.Tables.T_SchemesMixcast().Open("WinDescription",
    //        string.Format("WinMoney>0 and PlayTypeID in(select ID from T_PlayTypes where LotteryID in({0})) and "
    //        + " SchemeId in(SELECT ID FROM T_Schemes where IsuseID={1} and PlayTypeID in(select ID from T_PlayTypes where LotteryID in({0})))", lotteryId, isusesId), "");

    //    if (dtWin == null)
    //    {
    //        return listWin;
    //    }

    //    //查询数据库固定奖金
    //    DataTable dtFixedMoney = new DAL.Tables.T_WinTypes().Open("", string.Format("LotteryID={0}", lotteryId), "[Order]");

    //    //查询开奖时的奖金
    //    DataTable dtOpenMoney = new DAL.Tables.T_IsuseBonuses().Open("", string.Format("IsuseID={0}", isusesId), "");

    //    if (dtOpenMoney == null)
    //    {
    //        return listWin;
    //    }

    //    if (dtOpenMoney.Rows.Count != dtFixedMoney.Rows.Count)
    //    {
    //        return listWin;
    //    }

    //    if (dtWin.Rows.Count > 0 && dtFixedMoney.Rows.Count > 0)
    //    {
    //        foreach (DataRow row in dtWin.Rows)
    //        {
    //            //将中奖描述进行分割（一等奖1注，三等奖12注，四等奖15注。加奖：10）
    //            string[] desArr = row["WinDescription"].ToString().Split('。');

    //            if (desArr.Length == 2)
    //            {
    //                //获取中奖信息
    //                string[] winArr = desArr[0].Split('，');

    //                for (int i = 0; i < winArr.Length; i++)
    //                {
    //                    WinEntity entity = new WinEntity();

    //                    //获取奖项名称
    //                    entity.Name = winArr[i].Substring(0, winArr[i].LastIndexOf("奖") + 1);

    //                    //获取注数
    //                    int index = winArr[i].LastIndexOf("奖");

    //                    DataRow[] dr = dtFixedMoney.Select(string.Format("Name='{0}'", entity.Name));

    //                    entity.WinMoney = "0";

    //                    if (dr.Length == 1)
    //                    {
    //                        int rowIndex = dtFixedMoney.Rows.IndexOf(dr[0]);

    //                        entity.WinMoney = dr.Length > 0 ? dtOpenMoney.Rows[rowIndex]["defaultMoney"].ToString() : "";
    //                    }

    //                    entity.Order = dr.Length > 0 ? Shove._Convert.StrToInt(dr[0]["Order"].ToString(), 0) : 0;

    //                    string temp = winArr[i].Remove(0, index + 1);

    //                    temp = temp.Substring(0, temp.LastIndexOf("注"));

    //                    entity.InvestNum = Shove._Convert.StrToInt(temp, 0);

    //                    if (listWin.Exists(item => item.Name == entity.Name))
    //                    {
    //                        //存在相同的奖项名称，注数进行累加
    //                        listWin.First(item => item.Name == entity.Name).InvestNum++;
    //                    }
    //                    else
    //                    {
    //                        listWin.Add(entity);
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    listWin.Sort(delegate(WinEntity x, WinEntity y) { return x.Order - y.Order; });

    //    return listWin;
    //}
    #endregion

    public static DataTable GetWinLotteryInfo(string lotteryId, string isusesId)
    {
        string sqlStr = "SELECT TotalSales,WinType,WinCount,WinMoney,OpenNumber,Progressive,IssueID,IssueName,OpenTime FROM dbo.T_LotteryAnnouncement where LotteryID={0} and IssueID={1}";
        sqlStr = string.Format(sqlStr, lotteryId, isusesId);
        DataTable dt = new DataTable();
        dt = Shove.Database.MSSQL.Select(sqlStr);
        return dt;
    }
}
public class WinEntity
{
    /// <summary>
    /// 中奖详情
    /// </summary>
    public string WinType { get; set; }

    /// <summary>
    /// 中奖注数(注)
    /// </summary>
    public double TotalSales { get; set; }

    /// <summary>
    /// 每注金额(元)
    /// </summary>
    public string Progressive { get; set; }
}