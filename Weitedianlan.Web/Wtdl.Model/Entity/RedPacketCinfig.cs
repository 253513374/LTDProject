using System;
using Wtdl.Model.Enum;

namespace Wtdl.Model.Entity
{
    /// <summary>
    /// 扫码得红包配置类
    /// </summary>
    public class RedPacketCinfig : IEntityBase
    {
        public RedPacketCinfig()
        {
            CreateTime = DateTime.Now;
            ScanRedPacketGuid = Guid.NewGuid().ToString("N"); //.Replace("_", ""); //设置数据库中只有一条数据
        }

        public string ScanRedPacketGuid { get; set; }

        public int Id { get; set; }

        /// <summary>
        /// 红包配置类型
        /// </summary>
        public RedPacketConfigType RedPacketConfigType { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 发放红包人名称,商户名称
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// 红包祝福语句
        /// </summary>
        public string WishingWord { get; set; }

        /// <summary>
        /// 是否启用现金红包
        /// </summary>
        public bool IsActivity { get; set; }

        /// <summary>
        /// 订阅
        /// </summary>
        public bool IsSubscribe { get; set; }

        /// <summary>
        /// 红包类型
        /// </summary>
        public RedPacketType RedPacketType { get; set; }

        /// <summary>
        /// 红包金额,单位（分）
        /// </summary>
        public int CashValue { get; set; } //s{ get; set; }

        /// <summary>
        /// 最小金额
        /// </summary>
        public int MinCashValue { get; set; }

        /// <summary>
        /// 最大金额
        /// </summary>
        public int MaxCashValue { get; set; }
    }

    public enum RedPacketConfigType
    {
        QRCode,

        ///验证码
        Captcha,
    }
}