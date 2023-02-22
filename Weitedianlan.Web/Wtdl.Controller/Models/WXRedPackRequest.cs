using Senparc.Weixin.TenPay;

namespace Wtdl.Mvc.Models
{
    public class WXRedPackRequest
    {
        /// <summary>
        /// 公众账号AppID
        /// </summary>
        public string appId { set; get; }

        /// <summary>
        /// 商户号mchId
        /// </summary>
        public string mchId { set; get; }

        /// <summary>
        /// 支付密钥，微信商户平台(pay.weixin.qq.com)-->账户设置-->API安全-->密钥设置
        /// </summary>
        public string tenPayKey { set; get; }

        /// <summary>
        /// 要发红包的用户的OpenID
        /// </summary>
        public string openId { set; get; }

        /// <summary>
        /// 红包发送者名称，会显示给接收红包的用户
        /// </summary>
        public string senderName { set; get; }

        /// <summary>
        /// 发送红包的服务器地址</param>
        /// </summary>
        public string iP { set; get; }

        /// <summary>
        /// 付款金额 单位分
        /// </summary>
        public int redPackAmount { set; get; }

        /// <summary>
        /// 祝福语
        /// </summary>
        public string wishingWord { set; get; }

        /// <summary>
        /// 活动名称（请注意活动名称长度，官方文档提示为32个字符，实际限制不足32个字符）
        /// </summary>
        public string actionName { set; get; }

        /// <summary>
        /// 活动描述，用于低版本微信显示
        /// </summary>
        public string remark { set; get; }

        /// <summary>
        /// 将nonceStr随机字符串返回，开发者可以存到数据库用于校验
        /// </summary>
        public string nonceStr { set; get; }

        /// <summary>
        /// 将支付签名返回，开发者可以存到数据库用于校验
        /// </summary>
        public string paySign { set; get; }

        /// <summary>
        /// 商户订单号，新的订单号可以从RedPackApi.GetNewBillNo(mchId)方法获得，如果传入null，则系统自动生成
        /// </summary>
        public string mchBillNo { set; get; }

        /// <summary>
        /// 场景id（非必填），红包金额大于200时，请求参数scene必传
        /// </summary>
        public RedPack_Scene? scene { set; get; } = null;

        /// <summary>
        /// 活动信息（非必填）,String(128)posttime:用户操作的时间戳。
        /// </summary>
        public string riskInfo { set; get; } = null;

        /// <summary>
        /// 资金授权商户号，服务商替特约商户发放时使用（非必填），String(32)。示例：1222000096
        /// </summary>
        public string consumeMchId { set; get; } = null;

        /// <summary>
        /// 4位验证码
        /// </summary>
        public string Captcha { get; internal set; }

        /// <summary>
        /// 二维码标签序号
        /// </summary>
        public string QRCode { get; internal set; }

        /// <summary>
        /// API 证书地址（硬盘物理地址，形如E:\\cert\\apiclient_cert.p12）
        /// </summary>
        public string TenPayCertPath { set; get; }
    }
}