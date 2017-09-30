using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;

namespace SLS.Score.Task
{
    public partial class MainService : ServiceBase
    {
        private string _connectionString = "";
        private Task _task = null;
        private int _timeGap = 0;

        public MainService()
        {
            InitializeComponent();
           
        }

        protected override void OnStart(string[] args)
        {

            // 自动任务
            try
            {
                _task = new Task();
                _task.Run();
            }
            catch (Exception ex)
            {
                new Log("Task").Write("Task 启动失败：" + ex.Message);
            }
        }

        protected override void OnStop()
        {
            if (_task != null)
            {
                _task.Exit();
            }

            while ((_task != null) && (_task._state != 0)) { System.Threading.Thread.Sleep(500); };
        }
    }
}
