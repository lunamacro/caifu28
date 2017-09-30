using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace SLS.Score.Task
{
    /// <summary>
    ///Log 的摘要说明
    /// </summary>
    public class Log
    {
        private string _pathName;
        private string _fileName;

        /// <summary>
        /// 构造 Log
        /// </summary>
        /// <param name="pathname">相对于当前程序目录下 LogFiles 目录的相对路径，如：System， 就相当于 .\LogFiles\System\</param>
        public Log(string pathname)
        {
            if (String.IsNullOrEmpty(pathname))
            {
                throw new Exception("没有初始化 Log 类的 PathName 变量");
            }

            _pathName = System.AppDomain.CurrentDomain.BaseDirectory + "LogFiles\\" + pathname;

            if (!Directory.Exists(_pathName))
            {
                try
                {
                    Directory.CreateDirectory(_pathName);
                }
                catch { }
            }

            if (!Directory.Exists(_pathName))
            {
                _pathName = System.AppDomain.CurrentDomain.BaseDirectory + "LogFiles";

                if (!Directory.Exists(_pathName))
                {
                    try
                    {
                        Directory.CreateDirectory(_pathName);
                    }
                    catch { }
                }

                if (!Directory.Exists(_pathName))
                {
                    _pathName = System.AppDomain.CurrentDomain.BaseDirectory;
                }
            }

            _fileName = _pathName + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
        }

        public void Write(string message)
        {
            _fileName = _pathName + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            if (String.IsNullOrEmpty(_fileName))
            {
                return;
            }

            using (FileStream fs = new FileStream(_fileName, FileMode.Append, FileAccess.Write, FileShare.Write))
            {
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.GetEncoding("GBK"));

                try
                {
                    writer.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + System.DateTime.Now.Millisecond.ToString() + "\t\t" + message + "\r\n");
                }
                catch{ }

                writer.Close();
            }
        }
    }
}