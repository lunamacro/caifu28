using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLS
{
    public class Comm
    {
        public Comm() { }
        /// <summary>
        /// 数组去重
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string[] RemoveAgain(string[] arr)
        {
            List<string> tempList = new List<string>();
            for (int i = 0; i < arr.Length; i++)
            {
                tempList.Add(arr[i]);
            }
            return tempList.Distinct().ToArray<string>();
        }
    }
}
