using ScanCode.Model.ResponseModel;
using System;

namespace ScanCode.Model.Entity
{
    public class UserAwardInfo : TResult
    {
        public int Id { get; set; } // 用户获奖信息ID

        /// <summary>
        /// 微信OpenID
        /// </summary>
        public string WeChatOpenId { get; set; } //

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } //

        /// <summary>
        /// 联系电话
        /// </summary>
        public string PhoneNumber { get; set; } // 联系电话

        /// <summary>
        /// 获奖奖品名称
        /// </summary>
        public string AwardName { get; set; } // 奖品名称

        /// <summary>
        /// 奖品描述
        /// </summary>
        public string AwardDescription { get; set; } // 奖品描述

        /// <summary>
        /// 奖品类型
        /// </summary>
        public string PrizeType { get; set; }

        /// <summary>
        /// 奖品编号
        /// </summary>
        public string PrizeNumber { get; set; }

        /// <summary>
        /// 活动编号
        /// </summary>
        public string ActivityNumber { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 获奖日期
        /// </summary>
        public DateTime DateReceived { get; set; } // 获奖日期

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; } = "中国"; //国家

        /// <summary>
        /// 省
        /// </summary>
        public string ProvinceOrState { get; set; } = "海南省"; //省份/州

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; } = "海口市"; //城市

        /// <summary>
        /// 区
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 物流详细地址
        /// </summary>
        public string FullAddress { get; set; } // 完整地址（包括街道地址和小区/楼盘/单位）

        /// <summary>
        /// 抽奖二维码
        /// </summary>
        public string QrCode { get; set; }

        /// <summary>
        /// 是否已邮寄发货,默认未发货
        /// </summary>
        public bool IsShipped { get; set; } = false;

        public static UserAwardInfo Create()
        {
            return new UserAwardInfo()
            {
                DateReceived = DateTime.Now
            };
        }
    }
}