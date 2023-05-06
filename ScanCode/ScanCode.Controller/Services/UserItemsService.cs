using ScanCode.Model.Entity;
using ScanCode.Model.ResponseModel;
using ScanCode.Repository;

namespace ScanCode.Controller.Services
{
    public class UserItemsService
    {
        private LotteryRecordRepository _lotteryRecordRepository;
        private RedPacketRecordRepository _redPacketRecordRepository;
        private readonly UserAwardInfoRepository _userAwardInfoRepository;

        private ILogger _logger;

        public UserItemsService(LotteryRecordRepository lotteryRecordRepository,
            RedPacketRecordRepository redPacketRecordRepository,
            UserAwardInfoRepository userAwardInfoRepository,
            ILogger<UserItemsService> logger)
        {
            _lotteryRecordRepository = lotteryRecordRepository;
            _redPacketRecordRepository = redPacketRecordRepository;
            _userAwardInfoRepository = userAwardInfoRepository;
            _logger = logger;
        }

        //返回用户抽奖信息
        public async Task<UserItemsRecordResult> GetUserLotteryinfo(string openid)
        {
            var lotteryInfo = await _lotteryRecordRepository.GetLotteryInfoAsync(openid);
            var redPacketInfo = await _redPacketRecordRepository.GetUserRedPacketInfoAsync(openid);
            return UserItemsRecordResult.SuccessResult(lotteryInfo, redPacketInfo);
        }

        //返回用户领取红包信息
        public async Task<UserRedPacketRecordResult> GetRedPacketRecord(string openid)
        {
            var redPacketInfo = await _redPacketRecordRepository.GetUserRedPacketInfoAsync(openid);
            return UserRedPacketRecordResult.SuccessResult(redPacketInfo);
        }

        //返回用户领取奖品信息
        public async Task<List<UserAwardInfo>> GetAwardInfo(string openid)
        {
            return await _userAwardInfoRepository.GetByOpenIdAsync(openid);
        }
    }
}