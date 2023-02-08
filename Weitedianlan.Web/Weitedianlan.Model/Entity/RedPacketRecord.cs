using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weitedianlan.Model.Entity
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
        /// 领取人
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 标签序号
        /// </summary>
        public string QrCode { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string? VerificationCode { get; set; }

        /// <summary>
        /// 红包金额
        /// </summary>
        public decimal CashAmount { get; set; }
    }
}