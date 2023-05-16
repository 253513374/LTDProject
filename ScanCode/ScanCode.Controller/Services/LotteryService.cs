using Microsoft.Extensions.Caching.Memory;
using ScanCode.Controller.Services;
using ScanCode.Model;
using ScanCode.Model.Entity;
using ScanCode.Model.Enum;
using ScanCode.Model.ResponseModel;
using ScanCode.Mvc.Models;
using ScanCode.RedisCache;
using ScanCode.Repository;
using ScanCode.Repository.Utility;

namespace ScanCode.Mvc.Services
{
    /// <summary>
    /// 抽奖服务
    /// </summary>
    public class LotteryService
    {
        private readonly LotteryActivityRepository _lotteryActivityRepository;
        private readonly LotteryRecordRepository _lotteryRecordRepository;
        private readonly VerificationCodeRepository _verificationCodeRepository;
        private readonly ActivityPrizeRepository _activityPrizeRepository;
        private readonly WLabelStorageRepository _wLabelStorageRepository;

        //private readonly IDatabase _database;
        private readonly ILogger<LotteryService> _logger;

        private readonly IMemoryCache _memoryCache;

        private HubService _hubService;

        private IRedisCache _redisCache;

        public LotteryService(LotteryActivityRepository lotteryActivityRepository,
            WLabelStorageRepository wLabelStorageRepository,
            LotteryRecordRepository recordRepository,
            VerificationCodeRepository verificationCodeRepository,
            ActivityPrizeRepository repository,
            HubService hubService,
            IRedisCache redisCache,
        //IDistributedCache distributedCache,
        //  IConnectionMultiplexer connectionMultiplexer,
        ILogger<LotteryService> logger,
            IMemoryCache cache)
        {
            _redisCache = redisCache;
            _wLabelStorageRepository = wLabelStorageRepository;
            _memoryCache = cache;
            _hubService = hubService;
            //  _distributedCache = distributedCache;
            //  _database = connectionMultiplexer.GetDatabase();
            _activityPrizeRepository = repository;
            _lotteryActivityRepository = lotteryActivityRepository;
            _lotteryRecordRepository = recordRepository;
            _verificationCodeRepository = verificationCodeRepository;
            _logger = logger;
        }

        /// <summary>
        /// 获取当前活动数据
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public async Task<ActivityResult> GetLotteryActivityAsync()
        {
            try
            {
                var result = await _redisCache.GetObjectAsync<LotteryActivity>(CacheKeys.LoteryActive); ///await _lotteryActivityRepository.GetLotteryActivityAsync();

                //var activiprize =
                //  await _activityPrizeRepository.FindAsync(f => f.LotteryActivityId == result.Id && f.IsActive == true);

                // result.Prizes = activiprize.ToList();

                if (result is not null)
                {
                    var view = new ActivityResult()
                    {
                        IsSuccess = true,
                        Name = result.Name,
                        Status = result.Status,
                        ActivityImage = result.ActivityImage,
                        Description = result.Description,
                        StartTime = result.StartTime,
                        EndTime = result.EndTime,
                        ActivityNumber = result.ActivityNumber,
                        IsActive = result.IsActive
                        //Prizes = result.Prizes.ToList(),
                    };
                    var prizeList = result.Prizes.ToList();
                    foreach (var prize in prizeList)
                    {
                        view.Prizes.Add(new PrizeResult()
                        {
                            Probability = prize.Probability,
                            ImageUrl = prize.ImageUrl,
                            Name = prize.Name,
                            Description = prize.Description,
                            PrizeNumber = prize.PrizeNumber,
                        });
                    }

                    return view;
                }

                return new ActivityResult() { Msg = "没有活动" };
            }
            catch (Exception e)
            {
                _logger.LogError($"服务器错误：{e}");
                return new ActivityResult() { Msg = $"服务器错误：{e.Message}" };
            }
        }

        /// <summary>
        /// 抽取一个奖品
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private async Task<ActivityPrize> GetLuckyPrize(string prizenumber)
        {
            var prize = await _activityPrizeRepository.FindSingleAsync(f => f.PrizeNumber.Trim() == prizenumber.Trim());

            //var prize = activity.Prizes.FirstOrDefault(f => f.PrizeNumber.Contains(prizenumber));

            if (prize.Unredeemed >= prize.Amount)
            {
                return null;
            }
            else
            {
                return prize;
            }
        }

        /// <summary>
        /// 抽奖之前验证用户与标签序号是否有抽奖资格
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        private async Task<VerifyResult> VerifyLottery(string openid, string qrcode, string prizenumber)
        {
            var activityPrize = await _activityPrizeRepository.ExistAsync(a => a.PrizeNumber == prizenumber && a.IsActive == true);
            if (!activityPrize)
            {
                return new VerifyResult() { IsSuccess = false, Message = "抽奖奖品不存在" };
            }

            // 当前活动是否存在与活动是否处于激活状态
            var activity = await GetLotteryActivityAsync();
            if (activity is not null)
            {
                if (!activity.IsActive)
                {
                    return new VerifyResult() { IsSuccess = false, Message = "当前活动未激活" };
                }
                var now = DateTime.Now;
                if (now < activity.StartTime || now > activity.EndTime)
                {
                    return new VerifyResult() { IsSuccess = false, Message = "当前活动未开始或已结束" };
                }
            }

            // 当前用户是否已经对标签序号抽过奖了
            var lotteryRecord = await _lotteryRecordRepository.AnyQRCodeRecordsAsync(openid, qrcode);
            if (lotteryRecord)
            {
                return new VerifyResult() { IsSuccess = false, Message = "当前用户已经对标签序号抽过奖了" };
            }

            return new VerifyResult() { IsSuccess = true, Message = "可以参与抽奖活动" };
        }

        /// <summary>
        /// 优化后的方法，同时启动多个异步操作，这样可以在一定程度上提高效率
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="qrcode"></param>
        /// <param name="prizenumber"></param>
        /// <returns></returns>
        private async Task<VerifyResult> VerifyLotteryAsync(string openid, string qrcode, string prizenumber)
        {
            // perform all async operations first
            var activityPrizeTask = _activityPrizeRepository.ExistAsync(a => a.PrizeNumber == prizenumber && a.IsActive == true);
            var activityTask = GetLotteryActivityAsync();
            var lotteryRecordTask = _lotteryRecordRepository.AnyQRCodeRecordsAsync(openid, qrcode);

            var activityPrize = await activityPrizeTask;
            if (!activityPrize)
            {
                return VerifyResult.PrizeNotExist;
            }

            var activity = await activityTask;
            if (activity is null || !activity.IsActive)
            {
                return VerifyResult.ActivityNotActive;
            }

            var now = DateTime.Now;
            if (now < activity.StartTime || now > activity.EndTime)
            {
                return VerifyResult.ActivityNotStartedOrFinished;
            }

            var lotteryRecord = await lotteryRecordTask;
            if (lotteryRecord)
            {
                return VerifyResult.AlreadyParticipated;
            }

            return VerifyResult.CanParticipate;
        }

        private async Task RecordLotterySuccess(string openid, string qrcode, ActivityPrize prize)
        {
            await RecordLottery(openid, qrcode, prize, true);
            return;
        }

        private async Task RecordLotteryFail(string openid, string qrcode, ActivityPrize prize)
        {
            await RecordLottery(openid, qrcode, prize, false);
            return;
        }

        /// <summary>
        /// 记录抽奖数据
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="qrcode"></param>
        /// <param name="prizeId"></param>
        /// <returns></returns>
        private async Task RecordLottery(string openid, string qrcode, ActivityPrize prize, bool issuccess)
        {
            var activity = await _lotteryActivityRepository.FindSingleAsync(f => f.Id == prize.LotteryActivityId);

            var lotteryRecord = new LotteryRecord()
            {
                OpenId = openid,
                Claimed = ClaimedStatus.NotClaimed,
                QRCode = qrcode,
                ActivityNumber = activity.ActivityNumber,
                ActivityName = activity.Name,
                ActivityDescription = activity.Description,
                Type = prize.Type,
                Time = DateTime.Now,
                PrizeNumber = prize.PrizeNumber,
                PrizeName = prize.Name,
                PrizeDescription = prize.Description,
                CashValue = prize.CashValue,
                IsSuccessPrize = issuccess,
            };
            await _lotteryRecordRepository.AddAsync(lotteryRecord);
        }

        /// <summary>
        /// 抽奖
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public async Task<LotteryResult> GetLotteryResultAsync(string openid, string qrcode, string prizenumber)
        {
            try
            {
                // var verify = await VerifyLottery(openid, qrcode
                // // 验证是否有抽奖资格
                var verify = await VerifyLottery(openid, qrcode, prizenumber);
                if (!verify.IsSuccess)
                {
                    return new LotteryResult() { IsSuccess = false, Message = verify.Message };
                }

                // 随机抽取一个参加活动的奖品
                var prize = await GetLuckyPrize(prizenumber);
                if (prize is null)
                {
                    return new LotteryResult() { IsSuccess = false, Message = "奖品已经抽完,请重新抽奖" };
                }
                //根据奖品的中奖概率随机求得一个中奖数值号码
                var randomPrizeNumber = await GlobalUtility.GetRandomInt(prize.Probability);

                //发送信息,有人参与抽奖了
                await _hubService.SendLotteryCountAsync();
                if (randomPrizeNumber == prize.UniqueNumber)
                {
                    //发送信息,有人中奖了
                    await _hubService.SendLotteryWinCountAsync();

                    // 记录中奖数据
                    await RecordLotterySuccess(openid, qrcode, prize);

                    //减少可抽奖奖品数量，更新到数据库
                    prize.Unredeemed++;
                    var updateResult = await _activityPrizeRepository.UpdateActivityPrizeAsync(prize);

                    //返回抽奖结果
                    return new LotteryResult()
                    {
                        IsSuccess = true,
                        Message = $"恭喜你，中大奖啦",
                        PrizeType = prize.Type.ToString(),
                        PrizeName = prize.Name,
                        PrizeDescription = prize.Description,
                        PrizeImage = prize.ImageUrl,
                        PrizeNumber = prize.PrizeNumber
                    };
                }
                else
                {
                    // 记录没有中奖数据
                    await RecordLotteryFail(openid, qrcode, prize);
                    //返回抽奖结果
                    return new LotteryResult()
                    {
                        IsSuccess = false,
                        Message = $"很遗憾！没有中奖。",
                        PrizeType = prize.Type.ToString(),
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"服务器错误：{e}");
                return new LotteryResult() { IsSuccess = false, Message = $"服务器错误：{e.Message}" };
            }
        }

        public async Task<LotteryResult> ExecuteLotteryAsync(string openid, string qrcode, string prizenumber)
        {
            try
            {
                // 验证是否有抽奖资格
                var verify = await VerifyLotteryAsync(openid, qrcode, prizenumber);
                if (!verify.IsSuccess)
                {
                    return LotteryResult.VerificationFailed(verify.Message);
                }

                // 随机抽取一个参加活动的奖品
                var prize = await GetLuckyPrize(prizenumber);
                if (prize is null)
                {
                    return LotteryResult.NoPrizeAvailable();
                }

                // 根据奖品的中奖概率随机求得一个中奖数值号码
                var randomPrizeNumber = await GlobalUtility.GetRandomInt(prize.Probability);

                // 发送信息,有人参与抽奖了
                await _hubService.SendLotteryCountAsync();

                if (randomPrizeNumber == prize.UniqueNumber)
                {
                    // 发送信息,有人中奖了
                    await _hubService.SendLotteryWinCountAsync();

                    // 记录中奖数据，减少可抽奖奖品数量，更新到数据库
                    prize.Unredeemed++;
                    var recordTask = RecordLotterySuccess(openid, qrcode, prize);
                    var updateTask = _activityPrizeRepository.UpdateActivityPrizeAsync(prize);

                    await Task.WhenAll(recordTask, updateTask);

                    // 返回抽奖结果
                    return LotteryResult.PrizeWon(prize);
                }
                else
                {
                    // 记录没有中奖数据
                    await RecordLotteryFail(openid, qrcode, prize);

                    // 返回抽奖结果
                    return LotteryResult.PrizeNotWon(prize);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"服务器错误：{e}");
                return LotteryResult.ServerError(e);
            }
        }
    }
}