using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wtdl.Share.SignalR
{
    public static class HubServerMethods
    {
        public static string SendOutStorageDayCount = "SendOutStorageDayCount";

        public static string SendLotteryWinCount = "SendLotteryWinCount";

        public static string SendLotteryCount = "SendLotteryCount";

        public static string SendRedPackedCount = "SendRedPackedCount";

        public static string SendRedpacketTotalAmount = "SendRedpacketTotalAmount";
    }
}