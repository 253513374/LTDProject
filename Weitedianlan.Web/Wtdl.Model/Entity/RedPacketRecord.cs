using System;

namespace Wtdl.Model.Entity
{
    /// <summary>
    /// 红包发放记录
    /// </summary>
    public class RedPacketRecord : IEntityBase
    {
        /// <summary>
        /// 主键key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 标签序号
        /// </summary>
        public string QrCode { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string? Captcha { get; set; }

        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 红包金额
        /// </summary>
        public decimal CashAmount { get; set; }

        /// <summary>
        /// 领取红包的时间
        /// </summary>
        public DateTime ReceiveTime { get; set; }

        /// <summary>
        /// 红包发放时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string MchbillNo { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string MchId { get; set; }

        /// <summary>
        /// 公众账号appid
        /// </summary>
        public string WxAppId { get; set; }

        /// <summary>
        /// 用户openid
        /// </summary>
        public string ReOpenId { get; set; }

        /// <summary>
        /// 付款金额
        /// </summary>
        public string TotalAmount { get; set; }

        /// <summary>
        /// 红包订单的微信单号
        /// </summary>
        public string SendListid { get; set; }

        /// <summary>
        /// //随机字符串
        /// </summary>
        public string NonceStr { get; set; }

        /// <summary>
        /// //签名
        /// </summary>
        public string PaySign { get; set; }
    }
}