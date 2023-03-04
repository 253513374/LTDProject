using Wtdl.Mvc.Models;
using Wtdl.Repository;

namespace Wtdl.Controller.Services
{
    public class UserItemsService
    {
        private LotteryRecordRepository _lotteryRecordRepository;
        private RedPacketRecordRepository _redPacketRecordRepository;

        public UserItemsService(LotteryRecordRepository lotteryRecordRepository,
            RedPacketRecordRepository redPacketRecordRepository)
        {
            _lotteryRecordRepository = lotteryRecordRepository;
            _redPacketRecordRepository = redPacketRecordRepository;
        }

        //返回用户抽奖信息
        public async Task<UserItemsRecordResult> GetUserLotteryinfo(string openid)
        {
            var lotteryInfo = await _lotteryRecordRepository.GetLotteryInfoAsync(openid);
            var redPacketInfo = await _redPacketRecordRepository.GetUserRedPacketInfoAsync(openid);

            return UserItemsRecordResult.SuccessResult(lotteryInfo, redPacketInfo);
        }
    }
}