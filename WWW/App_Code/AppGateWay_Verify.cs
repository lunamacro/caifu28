using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

/// <summary>
/// AppGateWay_Verify 的摘要说明
/// </summary>
public class AppGateWay_Verify
{
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    private static string RSAKEY = "Rafav5G4war7f3ee5gYZfavbngsgmnDv";

	public AppGateWay_Verify()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}


    public static string verifyForLogin( string oauthString, string info)
    {
        string verify = RSA(info);
        if(!verify.Equals(oauthString)){
            return "发送了非法数据"+" verify="+verify +" oauthString="+oauthString;
        }

        try
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            BaseParam param = jss.Deserialize<BaseParam>(info);


            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalSeconds; // 相差秒数
            long appTime = long.Parse(param.timeStamp);
            if (Math.Abs(timeStamp - appTime) > 60000)
            {
                return "请求发送超时";
            }
        }
        catch
        {
            return "auth 异常请求";
        }
        return "success";
    }



    public static string RSA(string val)
    {
        return UserMd5(val + RSAKEY);
    }


    public static string makeMD5(string Text)
    {
        byte[] buffer = System.Text.Encoding.Default.GetBytes(Text);
        try
        {
            System.Security.Cryptography.MD5CryptoServiceProvider check;
            check = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] somme = check.ComputeHash(buffer);
            string ret = "";
            foreach (byte a in somme)
            {
                if (a < 16)
                    ret += "0" + a.ToString("X");
                else
                    ret += a.ToString("X");
            }
            return ret.ToLower();
        }
        catch
        {
            throw;
        }
    }

    //支持中文加密
    public static string UserMd5(string str)
    {
        string cl = str;
        string pwd = "";
        MD5 md5 = MD5.Create();//实例化一个md5对像
        // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
        // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
        for (int i = 0; i < s.Length; i++)
        {
            // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
            pwd = pwd + s[i].ToString("x2");

        }
        return pwd;
    }


    class BaseParam
    {
        public string timeStamp, accessToken;
    }



}