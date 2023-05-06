namespace ScanCode.Share.SignalR
{
    public static class HubClientMethods
    {
        /// <summary>
        /// 更新 当天销量数 方法
        /// </summary>
        public static string OnOutStorageDayCount = "OnOutStorageDayCount";

        /// <summary>
        /// 更新实时中奖人数 方法
        /// </summary>
        public static string OnLotteryWinCount = "OnLotteryWinCount";

        /// <summary>
        /// 更新抽奖人数数量 方法
        /// </summary>
        public static string OnLotteryCount = "OnLotteryCount";

        /// <summary>
        /// 更新 红包领取数量 方法
        /// </summary>
        public static string OnRedPackedCount = "OnRedPackedCount";

        /// <summary>
        /// 更新 红包领取金额 方法
        /// </summary>
        public static string OnRedpacketTotalAmount = "OnRedpacketTotalAmount";

        /// <summary>
        /// 删除已经上传的离线数据。
        /// </summary>
        public static string OnDeleteSynchronizationData = "DeleteSynchronizationData";
    }
}