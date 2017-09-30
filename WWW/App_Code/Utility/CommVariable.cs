using Shove.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// CommVariable 的摘要说明
/// </summary>
public class CommVariable
{
    public CommVariable()
    {
        DataTable dt = (DataTable)Shove._Web.Cache.GetCache("T_PushTemplate_apiKey");
        if (dt == null)
        {
            dt = new DAL.Tables.T_PushTemplate().Open("APIKey,SecretKey", "ID=1", "");
            Shove._Web.Cache.SetCache("T_PushTemplate_apiKey", dt, 3600);
        }
        if (dt.Rows.Count > 0 && dt != null)
        {
            apiKey = dt.Rows[0][0].ToString();
            secretKey = dt.Rows[0][1].ToString();
        }
    }

    #region 百度云推送 apiKey secretKey
    public string apiKey
    {
        get;
        set;
    }
    public string secretKey
    {
        get;
        set;
    }
    #endregion
}