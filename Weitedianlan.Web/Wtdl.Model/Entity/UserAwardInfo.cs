using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wtdl.Model.Entity
{
    public class UserAwardInfo
    {
        public int Id { get; set; } // 用户获奖信息ID

        // 用户信息
        public string WeChatOpenId { get; set; } // 微信OpenID

        public string UserName { get; set; } // 用户名

        public string PhoneNumber { get; set; } // 联系电话

        // 获奖奖品信息
        public string AwardName { get; set; } // 奖品名称

        public string AwardDescription { get; set; } // 奖品描述
        public DateTime DateReceived { get; set; } // 获奖日期

        // 物流地址信息
        public string FullAddress { get; set; } // 完整地址（包括街道地址和小区/楼盘/单位）

        public string City { get; set; } = "海口市"; //城市
        public string ProvinceOrState { get; set; } = "海南省"; //省份/州
        public string Country { get; set; } = "中国"; //国家
        public string PostalCode { get; set; } //邮政编码

        // 是否已发货,默认未发货
        public bool IsShipped { get; set; } = false;

        // 无参构造函数
        public UserAwardInfo()
        { }

        // 带参数的构造函数
        public UserAwardInfo(
            string weChatOpenId, string userName, string phoneNumber,
            string awardName, string awardDescription, DateTime dateReceived,
            string fullAddress, string city, string provinceOrState, string country, string postalCode, bool isShipped = false)
        {
            WeChatOpenId = weChatOpenId;
            UserName = userName;
            PhoneNumber = phoneNumber;
            AwardName = awardName;
            AwardDescription = awardDescription;
            DateReceived = dateReceived;
            FullAddress = fullAddress;
            City = city;
            ProvinceOrState = provinceOrState;
            Country = country;
            PostalCode = postalCode;
            IsShipped = isShipped;
        }

        // 重写ToString()方法，方便调试和输出
        public override string ToString()
        {
            return $"用户获奖信息: {{ ID: {Id}, 微信OpenID: {WeChatOpenId}, 用户名: {UserName}, 联系电话: {PhoneNumber}, 奖品名称: {AwardName}, 奖品描述: {AwardDescription}, 获奖日期: {DateReceived}, 完整地址: {FullAddress}, 城市: {City}, 省份/州: {ProvinceOrState}, 国家: {Country}, 邮政编码: {PostalCode}, 是否已发货: {IsShipped} }}";
        }
    }
}