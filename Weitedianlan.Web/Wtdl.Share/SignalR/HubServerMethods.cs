namespace Wtdl.Share.SignalR
{
    public static class HubServerMethods
    {
        public static string SendOutStorageDayCount = "SendOutStorageDayCount";

        public static string SendLotteryWinCount = "SendLotteryWinCount";

        public static string SendLotteryCount = "SendLotteryCount";

        public static string SendRedPackedCount = "SendRedPackedCount";

        public static string SendRedpacketTotalAmount = "SendRedpacketTotalAmount";

        /// <summary>
        /// 批量出库-
        /// </summary>
        public static string SendOutStorageBatch = "SendOutStorageBatchAsync";

        public static string SendOutStorage = "SendOutStorageAsync";

        public static string SendAgent = "SendAgentAsync";

        public static string SendDeleteSynchronizationData = "SendDeleteSynchronizationDataAsync";
    }
}