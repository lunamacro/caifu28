using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
///CheckName_ashx 的摘要说明
/// </summary>
public class CheckName_ashx
{
	public CheckName_ashx()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public bool IsKeyWords(string UserName)
    {
        DataTable dtKeyWords = new DAL.Tables.T_Sensitivekeywords().Open("", "", "");

        string KeyWords = "";

        if (dtKeyWords == null || dtKeyWords.Rows.Count < 1)
        {
            return true;
        }

        KeyWords = dtKeyWords.Rows[0]["KeyWords"].ToString().Replace("<p>", "").Replace("</p>", "");

        foreach (string str in KeyWords.Split(','))
        {
            if (string.IsNullOrEmpty(str))
                continue;
            if (UserName.IndexOf(str) >= 0)
            {
                return false;
            }
        }

        return true;
    }

    public string RandomNumber()
    {
        int Number = 0;
        string Str = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        for (int i = 0; i < 1; i++)
        {
            Random rd = new Random();　　//无参即为使用系统时钟为种子
            Number += rd.Next(0, 61);
        }

        System.Threading.Thread.Sleep(100);

        return Str.Substring(Number, 1);
    }
}