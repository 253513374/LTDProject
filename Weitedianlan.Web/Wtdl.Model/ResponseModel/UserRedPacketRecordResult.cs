using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wtdl.Model.Entity;

namespace Wtdl.Model.ResponseModel
{
    public class UserRedPacketRecordResult
    {
        public UserRedPacketRecordResult()
        {
            PacketRecordDtos = new List<RedPacketRecordDto>();
        }

        /// <summary>
        /// 领取红包状态
        /// </summary>
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public List<RedPacketRecordDto> PacketRecordDtos { get; set; }

        public static UserRedPacketRecordResult SuccessResult(IEnumerable<RedPacketRecord> redPacketInfo)
        {
            var redpacketResult = new List<RedPacketRecordDto>();
            foreach (var item in redPacketInfo)
            {
                var model = new RedPacketRecordDto();
                model.IssueTime = item.IssueTime;
                model.TotalAmount = item.TotalAmount;

                redpacketResult.Add(model);
            }

            return new UserRedPacketRecordResult()
            {
                IsSuccess = true,
                Message = "Success",
                PacketRecordDtos = redpacketResult
            };
        }

        public static UserRedPacketRecordResult FailResult(string message)
        {
            var model = new UserRedPacketRecordResult();
            model.IsSuccess = false;
            model.Message = message;
            return model;
        }
    }

    public class RedPacketRecordDto
    {
        /// <summary>
        /// 红包发放时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        /// <summary>
        /// 红包付款金额
        /// </summary>
        public string TotalAmount { get; set; }
    }
}