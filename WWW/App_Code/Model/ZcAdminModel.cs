using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class SfcDataModel
{
    int lotteryID = 0;
    string isuseName = string.Empty;

    public int LotteryID { get { return lotteryID; } set { lotteryID = value; } }
    public string IsuseName { get { return isuseName; } set { isuseName = value; } }
}

public class SfcXMLModel
{
    string leaguename = string.Empty;
    string hometeam = string.Empty;
    string awayteam = string.Empty;
    string matchtime = string.Empty;
    public string Leaguename { get { return leaguename; } set { leaguename = value; } }
    public string Hometeam { get { return hometeam; } set { hometeam = value; } }
    public string Awayteam { get { return awayteam; } set { awayteam = value; } }
    public string Matchtime { get { return matchtime; } set { matchtime = value; } }
}

public class SfcListModel
{
    List<SfcXMLModel> list = new List<SfcXMLModel>();
    string starttime = "";
    string endtime = "";
    public List<SfcXMLModel> List { get { return list; } set { list = value; } }
    public string Starttime { get { return starttime; } set { starttime = value; } }
    public string Endtime { get { return endtime; } set { endtime = value; } }
}