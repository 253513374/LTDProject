namespace ScanCode.Share.SignalR
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
        public static string Returns_OutStorage = "ReturnsOutStorage";

        public static string SendAgent = "SendAgentAsync";

        public static string SendDeleteSynchronizationData = "SendDeleteSynchronizationDataAsync";

        /// <summary>
        /// 获取erp 出库单统计集合列表
        /// </summary>
        public static string GROUPED_ORDERS = "SendGroupedBdxOrderAsync";

        public static string BDXORDER_TOTAL_COUNT = "SendOrderCountByDDNOAsync";
        public static string BDXORDER_LIST = "SendBdxOrderListAsync";

        public static string Grouped_DDNO = "SendGroupedBdxOrdersDDNOAsync";

        public static string SENDTRACEABILITYRESULT = "SendGetWLabelStorageAsync";
    }
}