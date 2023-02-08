using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weitedianlan.Model.Enum;

namespace Weitedianlan.Model.Entity
{
    public class ScanRedPacket : IEntityBase
    {

        public ScanRedPacket()
        {
            ScanRedPacketGuid = new Guid().ToString().Replace("_", ""); //设置数据库中只有一条数据
        }

        public string ScanRedPacketGuid { get; set; }

        public int Id { get; set; }

        /// <summary>
        /// 是否启用现金红包
        /// </summary>
        public bool IsActivity { get; set; }

        /// <summary>
        /// 红包类型
        /// </summary>
        public RedPacketType RedPacketType { get; set; }

        /// <summary>
        /// 红包金额
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
}