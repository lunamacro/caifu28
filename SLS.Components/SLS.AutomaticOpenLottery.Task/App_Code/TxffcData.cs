using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLS.AutomaticOpenLottery.Task.App_Code
{
    public class TxffcData
    {
        public TxffcData() { }

        public TxffcData(DateTime onlinetime, string onlinenumber, string onlinechange) {
            this.onlinetime = onlinetime;
            this.onlinenumber = onlinenumber;
            this.onlinechange = onlinechange;
        }
        public DateTime onlinetime { get; set; }
        public string onlinenumber { get; set; }
        public string onlinechange { get; set; }
    }
}
