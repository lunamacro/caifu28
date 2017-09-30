using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace FlyFish.Common
{
    /// <summary>
    /// CPS常用类库
    /// </summary>
    public class CommonCode
    {
        #region 全局变量
        /// <summary>
        /// 用户名正则表达式
        /// </summary>
        private static readonly string UserNameReg = "[_0-9a-zA-Z\u4e00-\u9fa5]{1,18}";
        /// <summary>
        /// 用户密码正则表达式
        /// </summary>
        private static readonly string UserPwdReg = "[_0-9a-zA-Z]{1,18}";
        /// <summary>
        /// 手机号码正则表达式
        /// </summary>
        private static readonly string MobileReg = "1[0-9]{10}";

        #endregion

        #region 检查是否是合法用户名
        /// <summary>
        /// 检查是否是合法用户名
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>1表示用户名为空 2表示用户名不符合规则</returns>
        public static int CheckIsUserName(string userName)
        {
            int result = 0;
            if (string.IsNullOrEmpty(userName) || 0 == userName.Length)
            {
                result = 1;
            }
            else if (!(new Regex(CommonCode.UserNameReg).IsMatch(userName)))
            {
                result = 2;
            }
            return result;
        }
        #endregion

        #region 检查是否是合法用户密码
        /// <summary>
        /// 检查是否是合法用户密码
        /// </summary>
        /// <param name="userName">用户密码</param>
        /// <returns>1表示用户密码为空 2表示用户密码不符合规则</returns>
        public static int CheckIsUserPwd(string userPwd)
        {
            int result = 0;
            if (string.IsNullOrEmpty(userPwd) || 0 == userPwd.Length)
            {
                result = 1;
            }
            else if (!(new Regex(CommonCode.UserPwdReg).IsMatch(userPwd)))
            {
                result = 2;
            }
            return result;
        }
        #endregion

        #region 检查是否是手机号码
        /// <summary>
        /// 检查是否是手机号码
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns>1表示手机号码为空 2表示手机号码不符合规则</returns>
        public static int CheckIsMobile(string mobile)
        {
            int result = 0;
            if (string.IsNullOrEmpty(mobile) || 0 == mobile.Length)
            {
                result = 1;
            }
            else if (!(new Regex(CommonCode.MobileReg).IsMatch(mobile)))
            {
                result = 2;
            }
            return result;
        }
        #endregion

        #region 生成随机字母与数字
        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string getSerialNumber(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];

            }
            return result;
        }
        #endregion
    }
}
