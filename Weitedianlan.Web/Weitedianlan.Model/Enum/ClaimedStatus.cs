namespace Weitedianlan.Model.Enum
{
    /// <summary>
    ///  领取奖品状态
    /// </summary>
    public enum ClaimedStatus
    {
        /// <summary>
        /// 用户尚未领取该奖品
        /// </summary>
        NotClaimed,//：

        /// <summary>
        /// 用户已经领取该奖品
        /// </summary>
        Claimed,//

        /// <summary>
        /// 该奖品不合法，不可领取
        /// </summary>
        Invalid,//

        /// <summary>
        /// 该奖品已过期，不可领取
        /// </summary>
        Expired//：
    }
}