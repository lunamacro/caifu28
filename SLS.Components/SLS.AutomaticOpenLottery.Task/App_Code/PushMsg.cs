using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLS.AutomaticOpenLottery.Task.App_Code
{
    public class PushMsg
    {
        /**
        						type: extraObj.type,
						time: format_02(message.receivedTime),
						senderUserId: message.senderUserId,
						nickname: extraObj.nickname,
						avatar: extraObj.avatar,
						periods: extraObj.periods,
						betType: extraObj.betType,
						money: extraObj.money,
						open_result: extraObj.open_result,
						welcome_user: extraObj.welcome_user
         * */

        public int type { get; set; }
        public DateTime time { get; set; }
        public int senderUserId { get; set; }
        public string nickname { get; set; }
        public string avatar { get; set; }
        public string periods { get; set; }
        public string betType { get; set; }
        public int money { get; set; }
        public string open_result { get; set; }
        public int welcome_user { get; set; }

        public int lotteryID { get; set; }

        public int homeIndex { get; set; }
    }
}
