using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
///ZcAdminDAL 的摘要说明
/// </summary>
public class ZcAdminDAL
{
	public ZcAdminDAL()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public SfcXMLModel GetModel(DataRow dr)
    {
        SfcXMLModel model = new SfcXMLModel();
        model.Awayteam = Convert.ToString(dr["Awayteam"]).Split(',')[0];
        model.Hometeam = Convert.ToString(dr["Hometeam"]).Split(',')[0];
        model.Leaguename = Convert.ToString(dr["Leaguename"]).Split(',')[0];
        model.Matchtime = Convert.ToString(dr["Matchtime"]);
        return model;
    }
    public SfcXMLModel GetModelJQC(DataRow dr)
    {
        SfcXMLModel model = new SfcXMLModel();
        model.Hometeam = Convert.ToString(dr["Team"]).Split(',')[0];
        model.Matchtime = Convert.ToString(dr["Datetime"]);
        return model;
    }

    public List<SfcXMLModel> GetList(DataSet ds, string type)
    {
        List<SfcXMLModel> list = new List<SfcXMLModel>();
        if (ds.Tables.Count > 0)
        {
            if (type == "SFC")
                foreach (DataRow dr in ds.Tables[0].Rows)
                    list.Add(GetModel(dr));
            else if (type == "JQC")
                foreach (DataRow dr in ds.Tables[0].Rows)
                    list.Add(GetModelJQC(dr));
            else if (type == "LCBQC")
                foreach (DataRow dr in ds.Tables[0].Rows)
                    list.Add(GetModel(dr));
        }
        return list;
    }
}