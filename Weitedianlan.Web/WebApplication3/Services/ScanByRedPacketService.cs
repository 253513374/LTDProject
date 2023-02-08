using Wtdl.Mvc.Models;
using Wtdl.Repository;

namespace Wtdl.Mvc.Services
{
    public class ScanByRedPacketService
    {
        private readonly RedPacketRecordRepository _redPacketRecordRepository;
        private readonly VerificationCodeRepository _verificationCodeRepository;

        public ScanByRedPacketService(RedPacketRecordRepository redPacketRecordRepository,
            VerificationCodeRepository verificationCodeRepository)
        {
            _redPacketRecordRepository = redPacketRecordRepository;
            _verificationCodeRepository = verificationCodeRepository;
        }

        /// <summary>
        /// 发放红包
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public async Task<RedPacketViewModel> OutRedPackets(string qrcode, string code)
        {
            //判断标签序号与验证码是否匹配
            var validation = await _verificationCodeRepository.AnyAsync(a => a.AntiForgeryCode == qrcode && a.VCode == code);
            if (!validation)
            {
                return new RedPacketViewModel
                {
                    IsSuccess = false,
                    Message = "标签序号或者验证码错误"
                };
            }

            //判断标签序号是否已经领取过红包，一枚标签只能领取一次红包
            var reslut = await _redPacketRecordRepository.AnyAsync(qrcode, code);

            if (reslut)
            {
                return new RedPacketViewModel
                {
                    IsSuccess = false,
                    Message = "该验证码已经领取过红包"
                };
            }

            //发放红包
            return new RedPacketViewModel
            {
                IsSuccess = true,
                Message = "领取成功",
                CashAmount = 1
            };
        }
    }
}