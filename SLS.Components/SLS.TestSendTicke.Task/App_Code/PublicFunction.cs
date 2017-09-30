using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace SLS.Score.Task
{
    public class PublicFunction
    {
        public static string GetCallCert()
        {
            string result = "";

            result = "ShoveSoft";
            result += " ";
            result += "CO.,Ltd ";

            string lastResult = Shove._String.Reverse(result);

            result = "--";
            result += " by Shove ";

            result = Shove._String.Reverse(lastResult) + result;

            lastResult = Shove._String.Reverse(result);

            result = "20050709";
            result += Shove._String.Reverse("圳深 ");
            result += Shove._String.Reverse("安宝");

            result = Shove._String.Reverse(result);

            result = Shove._String.Reverse(lastResult) + Shove._String.Reverse(result);

            return result;
        }

        // 获取 Url 返回的 Html 流
        public static string Post(string url, string requestString, int timeoutSeconds)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            if (timeoutSeconds > 0)
            {
                request.Timeout = 1000 * timeoutSeconds;
            }
            request.Method = "POST";
            request.AllowAutoRedirect = true;

            byte[] data = System.Text.Encoding.GetEncoding("GBK").GetBytes(requestString);

            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "";
            Stream outstream = request.GetRequestStream();

            outstream.Write(data, 0, data.Length);
            outstream.Close();

            HttpWebResponse hwrp = (HttpWebResponse)request.GetResponse();
            string contentType = hwrp.Headers.Get("Content-Type");

            StreamReader sr = null;

            if (contentType.IndexOf("text/html") > -1)
            {
                sr = new StreamReader(hwrp.GetResponseStream(), System.Text.Encoding.GetEncoding("GBK"));
            }
            else
            {
                sr = new StreamReader(new GZipStream(hwrp.GetResponseStream(), CompressionMode.Decompress), System.Text.Encoding.GetEncoding("GBK"));
            }

            return sr.ReadToEnd();
        }
    }
}
