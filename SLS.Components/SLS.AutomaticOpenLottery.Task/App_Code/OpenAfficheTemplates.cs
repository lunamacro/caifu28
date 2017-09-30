using System;
using System.Data;
using System.Configuration;
using System.Web;

using Shove.Database;

namespace SLS.AutomaticOpenLottery.Task
{
    class OpenAfficheTemplates
    {
        Shove._IO.IniFile ini = new Shove._IO.IniFile(System.AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
        private string _connectionString = "";
        public OpenAfficheTemplates()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public string this[int LotteryID]
        {
            get
            {
                _connectionString = ini.Read("Options", "ConnectionString");

                object obj = MSSQL.ExecuteScalar(_connectionString, "select OpenAfficheTemplate from T_Lotteries where [ID] = " + LotteryID.ToString());

                if (obj == null)
                {
                    return "";
                }

                string Result = "";

                try
                {
                    Result = obj.ToString() ;
                }
                catch { }

                return Result;
            }
            set
            {
                _connectionString = ini.Read("Options", "ConnectionString");

                MSSQL.ExecuteNonQuery(_connectionString, "update T_Lotteries set OpenAfficheTemplate = @OpenAfficheTemplate where [ID] = " + LotteryID.ToString(),
                    new MSSQL.Parameter("OpenAfficheTemplate", SqlDbType.VarChar, 0, ParameterDirection.Input, value));
            }
        }
    }
}
