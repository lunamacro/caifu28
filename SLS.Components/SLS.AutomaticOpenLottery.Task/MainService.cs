using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;


namespace SLS.AutomaticOpenLottery.Task
{
    public partial class MainService : ServiceBase
    {
        private string ConnectionString = "";
        private AutoMaticOpenLotteryTask AutoMaticOpenLottery_Task = null;

        public MainService()
        {
            InitializeComponent();

            Shove._IO.IniFile ini = new Shove._IO.IniFile(System.AppDomain.CurrentDomain.BaseDirectory + "Config.ini");
            ConnectionString = ini.Read("Options", "ConnectionString");
            new Log("System").Write("MainService--ConnectionString--" + ConnectionString);
        }

        protected override void OnStart(string[] args)
        {
            //Service调试
        //Debugger.Launch();
            try
            {

                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConnectionString);

                int i = 0;
                while (conn.State != ConnectionState.Open && i < 5)
                {
                    conn.Open();

                    new Log("System").Write("测试数据库连接失败" + i++);

                    System.Threading.Thread.Sleep(5000);

                }

                conn.Close();
            }
            catch (Exception e)
            {
                new Log("System").Write(e.Message);
            }

            // 自动任务
            try
            {
                AutoMaticOpenLottery_Task = new AutoMaticOpenLotteryTask(ConnectionString);
                AutoMaticOpenLottery_Task.Run();
            }
            catch (Exception e)
            {
                new Log("System").Write("AutoMaticOpenLottery_Task 启动失败：" + e.Message);
            }

        }

        protected override void OnStop()
        {
            if (AutoMaticOpenLottery_Task != null)
            {
                AutoMaticOpenLottery_Task.Exit();
            }
        }
    }
}
