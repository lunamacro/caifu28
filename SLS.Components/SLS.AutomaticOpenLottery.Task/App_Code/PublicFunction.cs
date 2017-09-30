using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Data;
using System.Data.SqlClient;


namespace SLS.AutomaticOpenLottery.Task
{
    public class PF
    {
        public PF() { }
        // 获取 Url 返回的 Html 流
        public static string Post(string Url, string RequestString, int TimeoutSeconds)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

            if (TimeoutSeconds > 0)
            {
                request.Timeout = 1000 * TimeoutSeconds;
            }
            request.Method = "POST";
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] data = System.Text.Encoding.GetEncoding("gb2312").GetBytes(RequestString);

            Stream outstream = request.GetRequestStream();
            outstream.Write(data, 0, data.Length);
            outstream.Close();

            HttpWebResponse hwrp = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(hwrp.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));

            return sr.ReadToEnd();
        }

        // 获取 Url 返回的 Html 流
        public static string GetHtml(string Url, string encodeing, int TimeoutSeconds)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(Url);
                request.UserAgent = "www.svnhost.cn";
                if (TimeoutSeconds > 0)
                {
                    request.Timeout = 1000 * TimeoutSeconds;
                }
                request.AllowAutoRedirect = false;

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(encodeing));
                    string html = reader.ReadToEnd();
                    return html;
                }
                else
                {
                    return "";
                }
            }
            catch (SystemException)
            {
                return "";
            }
        }

        public static string GetHtml(string Url, string encodeing, int TimeoutSeconds, string PorXY, int Port)
        {
            string html = string.Empty;

            HttpWebResponse response = null;
            StreamReader reader = null;
            HttpWebRequest request = null;

            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(Url);      //建立HttpWebRequest對象
                request.Timeout = TimeoutSeconds * 1000;                              //定義服務器超時時間
                request.AllowAutoRedirect = true;

                if (!PorXY.Equals(""))
                {
                    WebProxy proxy = new WebProxy(PorXY, Port);        //定義一個網關對象
                    request.UseDefaultCredentials = true;                  //啟用網關認証
                    request.Proxy = proxy;
                }

                try
                {
                    response = (HttpWebResponse)request.GetResponse();        //取得回应
                }
                catch
                {
                    html = GetHtml(Url, encodeing, TimeoutSeconds);
                    return html;
                }

                //判断HTTP响应状态 
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return "";
                }

                reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(encodeing));
                html = reader.ReadToEnd();
                return html;
            }
            catch (SystemException)
            {
                return null;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                {
                    reader.Close();
                }

                if (request != null)
                {
                    request = null;
                }
            }
        }

        public static string GetHtml(string Url)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(Url);
                request.UserAgent = "www.svnhost.cn";
                request.Timeout = 60000;//60秒
                request.AllowAutoRedirect = false;

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));
                    string html = reader.ReadToEnd();
                    if (html.Length < 100)
                    {
                        return null;
                    }
                    else
                    {
                        return html;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (SystemException ex)
            {
                return null;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                {
                    reader.Close();
                }

                if (request != null)
                {
                    request = null;
                }
            }
        }

        //正则表达式替换通用方法
        public static string RegexReplace(string StrReplace, string strRegex, string NewStr)
        {
            Regex regex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return regex.Replace(StrReplace, NewStr);
        }

        //正则表达式
        public static string[] strRegex(string Str, string StrRegex)
        {
            Regex regex = new Regex(StrRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = regex.Match(Str);

            string[] strings = new string[m.Length];
            int i = 0;

            while (m.Success && i < m.Length)
            {
                strings[i] = m.Value;
                m = m.NextMatch();
                i++;
            }

            return strings;
        }
        private class ReplaceKey
        {
            public string[,] Key;

            public ReplaceKey()
            {
                Key = new string[2, 84];

                Key[0, 0] = "00"; Key[1, 0] = "A";
                Key[0, 1] = "11"; Key[1, 1] = "B";
                Key[0, 2] = "22"; Key[1, 2] = "H";
                Key[0, 3] = "33"; Key[1, 3] = "e";
                Key[0, 4] = "44"; Key[1, 4] = "F";
                Key[0, 5] = "55"; Key[1, 5] = "E";
                Key[0, 6] = "66"; Key[1, 6] = "M";
                Key[0, 7] = "77"; Key[1, 7] = "z";
                Key[0, 8] = "88"; Key[1, 8] = "I";
                Key[0, 9] = "99"; Key[1, 9] = "b";

                Key[0, 10] = "10"; Key[1, 10] = "K";
                Key[0, 11] = "20"; Key[1, 11] = "L";
                Key[0, 12] = "30"; Key[1, 12] = "C";
                Key[0, 13] = "40"; Key[1, 13] = "N";
                Key[0, 14] = "50"; Key[1, 14] = "l";
                Key[0, 15] = "60"; Key[1, 15] = "n";
                Key[0, 16] = "70"; Key[1, 16] = "m";
                Key[0, 17] = "80"; Key[1, 17] = "R";
                Key[0, 18] = "90"; Key[1, 18] = "a";

                Key[0, 19] = "01"; Key[1, 19] = "T";
                Key[0, 20] = "21"; Key[1, 20] = "U";
                Key[0, 21] = "31"; Key[1, 21] = "j";
                Key[0, 22] = "41"; Key[1, 22] = "W";
                Key[0, 23] = "51"; Key[1, 23] = "X";
                Key[0, 24] = "61"; Key[1, 24] = "V";
                Key[0, 25] = "71"; Key[1, 25] = "Z";
                Key[0, 26] = "81"; Key[1, 26] = "S";
                Key[0, 27] = "91"; Key[1, 27] = "J";

                Key[0, 28] = "02"; Key[1, 28] = "c";
                Key[0, 29] = "12"; Key[1, 29] = "d";
                Key[0, 30] = "32"; Key[1, 30] = "D";
                Key[0, 31] = "42"; Key[1, 31] = "f";
                Key[0, 32] = "52"; Key[1, 32] = "G";
                Key[0, 33] = "62"; Key[1, 33] = "h";
                Key[0, 34] = "72"; Key[1, 34] = "i";
                Key[0, 35] = "82"; Key[1, 35] = "w";
                Key[0, 36] = "92"; Key[1, 36] = "k";

                Key[0, 37] = "03"; Key[1, 37] = "O";
                Key[0, 38] = "13"; Key[1, 38] = "Q";
                Key[0, 39] = "23"; Key[1, 39] = "P";
                Key[0, 40] = "43"; Key[1, 40] = "o";
                Key[0, 41] = "53"; Key[1, 41] = "p";
                Key[0, 42] = "63"; Key[1, 42] = "x";
                Key[0, 43] = "73"; Key[1, 43] = "t";
                Key[0, 44] = "83"; Key[1, 44] = "s";
                Key[0, 45] = "93"; Key[1, 45] = "r";

                Key[0, 46] = "04"; Key[1, 46] = "u";
                Key[0, 47] = "14"; Key[1, 47] = "v";
                Key[0, 48] = "24"; Key[1, 48] = "Y";
                Key[0, 49] = "34"; Key[1, 49] = "q";
                Key[0, 50] = "54"; Key[1, 50] = "y";
                Key[0, 51] = "64"; Key[1, 51] = "g";
                Key[0, 52] = "74"; Key[1, 52] = "!";
                Key[0, 53] = "84"; Key[1, 53] = "@";
                Key[0, 54] = "94"; Key[1, 54] = "#";

                Key[0, 55] = "05"; Key[1, 55] = "$";
                Key[0, 56] = "15"; Key[1, 56] = "%";
                Key[0, 57] = "25"; Key[1, 57] = "^";
                Key[0, 58] = "35"; Key[1, 58] = "&";
                Key[0, 59] = "45"; Key[1, 59] = "*";
                Key[0, 60] = "65"; Key[1, 60] = "(";
                Key[0, 61] = "75"; Key[1, 61] = ")";
                Key[0, 62] = "85"; Key[1, 62] = "_";
                Key[0, 63] = "95"; Key[1, 63] = "-";

                Key[0, 64] = "06"; Key[1, 64] = "+";
                Key[0, 65] = "16"; Key[1, 65] = "=";
                Key[0, 66] = "26"; Key[1, 66] = "|";
                Key[0, 67] = "36"; Key[1, 67] = "\\";
                Key[0, 68] = "46"; Key[1, 68] = "<";
                Key[0, 69] = "56"; Key[1, 69] = ">";
                Key[0, 70] = "76"; Key[1, 70] = ",";
                Key[0, 71] = "86"; Key[1, 71] = ".";
                Key[0, 72] = "96"; Key[1, 72] = "?";

                Key[0, 73] = "07"; Key[1, 73] = "/";
                Key[0, 74] = "17"; Key[1, 74] = "[";
                Key[0, 75] = "27"; Key[1, 75] = "]";
                Key[0, 76] = "37"; Key[1, 76] = "{";
                Key[0, 77] = "47"; Key[1, 77] = "}";
                Key[0, 78] = "57"; Key[1, 78] = ":";
                Key[0, 79] = "67"; Key[1, 79] = ";";
                Key[0, 80] = "87"; Key[1, 80] = "\"";
                Key[0, 81] = "97"; Key[1, 81] = "\'";

                Key[0, 82] = "08"; Key[1, 82] = "`";
                Key[0, 83] = "18"; Key[1, 83] = "~";
            }
        }
        #region 获取是否扣税属性
        public static int GetTaxSwitch()
        {
            Shove._IO.IniFile ini = new Shove._IO.IniFile(System.AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
            string ConnectionString = ini.Read("Options", "ConnectionString");
            SqlConnection conn = new SqlConnection(ConnectionString);
            string sqlStr = "SELECT TaxSwitch FROM dbo.T_Sites";
            DataTable dt = new DataTable();
            dt = Shove.Database.MSSQL.Select(conn, sqlStr);
            return Convert.ToInt32(dt.Rows[0][0]);
        }
        #endregion

        // 中奖的记录，发送通知
        public static void SendWinNotification(DataSet ds, SqlConnection conn)
        {
            new Log("TeST").Write("进来了");
            try
            {
                if (ds == null)
                {
                    return;
                }
                new Log("TeST").Write("ds不为null");
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    new Log("TeST").Write("dt行数：" + dt.Rows.Count.ToString());
                    if (dt.Rows.Count < 1)
                    {
                        continue;
                    }
                    string siteName = "";
                    DataTable sitesDt = new DAL.Tables.T_Sites().Open(conn, "Name", "ID=1", "");
                    if (sitesDt != null && sitesDt.Rows.Count > 0)
                    {
                        siteName = sitesDt.Rows[0][0].ToString();
                    }
                    new Log("TeST").Write("siteName:" + siteName);
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        DataRow dr = dt.Rows[j];

                        string userID = dr["UserID"].ToString();
                        string userName = "";
                        string userMobile = "";
                        string schemeNo = "";
                        DataTable usersDt = new DAL.Tables.T_Users().Open(conn, "Name,Mobile", "ID=" + userID, "");
                        if (usersDt != null && usersDt.Rows.Count > 0)
                        {
                            userName = usersDt.Rows[0]["Name"].ToString();
                            userMobile = usersDt.Rows[0]["Mobile"].ToString();
                        }
                        if (userName.Length < 1)
                        {
                            userName = userMobile;
                        }
                        DataTable schemeDt = new DAL.Tables.T_Schemes().Open(conn, "SchemeNumber", "ID=" + dr["SchemeID"].ToString(), "");
                        if (schemeDt != null && schemeDt.Rows.Count > 0)
                        {
                            schemeNo = schemeDt.Rows[0][0].ToString();
                        }
                        string content = MessageTemplate.SchemeWinning.Replace("[WebSiteName]", siteName).Replace("[UserName]", userName).Replace("[SchemeNo]", schemeNo);
                        string resultMsg = "";
                        string tempStr = "userID:{0},content:{1},resultMsg:{2}";
                        bool resultBool = Message.Send(conn, Convert.ToInt32(userID), "", "", content, "", MessageType.SchemeWinning, SendTypeSingle.SelfAdaption, ref resultMsg);
                        new Log("TeST").Write(string.Format(tempStr, userID, content, resultMsg));
                    }
                }
            }
            catch (Exception ex)
            {
                new Log("TeST").Write(ex.ToString());
            }
        }
    }
}
