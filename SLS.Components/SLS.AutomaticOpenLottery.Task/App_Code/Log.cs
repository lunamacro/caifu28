using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace SLS.AutomaticOpenLottery.Task
{
    public class Log
    {
        private string _pathName;
        private string _fileName;

        /// <summary>
        /// 构造 Log
        /// </summary>
        /// <param name="pathname">相对于网站根目录 LogFiles 目录的相对路径，如： System， 就相当于 ~/LogFiles/System/</param>
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
                catch { }

                writer.Close();
            }
        }

        public void WriteIni(string message)
        {
            WriteIni("Log", message);
        }

        public void WriteIni(string section, string message)
        {
            if (String.IsNullOrEmpty(_fileName))
            {
                return;
            }

            Shove._IO.IniFile ini = new Shove._IO.IniFile(_fileName);

            ini.Write(section, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + System.DateTime.Now.Millisecond.ToString(), message);
        }
    }
}
