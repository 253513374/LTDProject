using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Senparc.Weixin.TenPay.V2;
using StackExchange.Redis;
using Weitedianlan.Model.Entity;
using Weitedianlan.Model.Enum;
using Wtdl.Mvc.Models;
using Wtdl.Repository;
using Wtdl.Repository.Utility;

namespace Wtdl.Mvc.Services
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
        private readonly IDatabase _database;
        private readonly ILogger<LotteryService> _logger;

        private readonly IMemoryCache _memoryCache;

        public LotteryService(LotteryActivityRepository lotteryActivityRepository,
            LotteryRecordRepository recordRepository,
            VerificationCodeRepository verificationCodeRepository,
            ActivityPrizeRepository repository,
            //IDistributedCache distributedCache,
            IConnectionMultiplexer connectionMultiplexer,
        ILogger<LotteryService> logger,
            IMemoryCache cache)
        {
            _memoryCache = cache;
            //  _distributedCache = distributedCache;
            _database = connectionMultiplexer.GetDatabase();
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
                //LotteryActivity result;
                //if (!_memoryCache.TryGetValue("LotteryActivity", out result))
                //{
                //    // 如果缓存中没有数据，则从数据库或其他数据源获取数据并存入缓存中
                //    result = await _lotteryActivityRepository.GetLotteryActivityAsync();

                //    var cacheOptions = new MemoryCacheEntryOptions
                //    {
                //        AbsoluteExpiration = DateTimeOffset.MaxValue,
                //        // AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddYears(1).Year, 1, 1, 0, 0, 0, TimeSpan.Zero)
                //    };
                //    _memoryCache.Set("LotteryActivity", result, cacheOptions);
                //}
                ////获取缓存活信息
                //var cache = await _memoryCache.GetOrCreateAsync("LotteryActivity", async entry =>
                //{
                //    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                //    return await GetLotteryActivityAsync();
                //});

                var result = await _lotteryActivityRepository.GetLotteryActivityAsync();

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
                        Id = result.Id,
                        IsActive = result.IsActive
                        //Prizes = result.Prizes.ToList(),
                    };
                    var prizeList = result.Prizes.ToList();
                    for (int i = 0; i < prizeList.Count; i++)
                    {
                        var prize = prizeList[i];
                        view.Prizes.Add(new PrizeResult()
                        {
                            Probability = prize.Probability,
                            ImageUrl = prize.ImageUrl,
                            Name = prize.Name,
                            Description = prize.Description,
                            Id = prize.Id,
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
        /// 随机抽取一个奖品
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private async Task<ActivityPrize> GetLuckyPrize(string prizenumber)
        {
            var activity = await _lotteryActivityRepository.GetLotteryActivityAsync();

            var prize = activity.Prizes.FirstOrDefault(f => f.PrizeNumber.Contains(prizenumber));

            if (prize.Unredeemed >= prize.Amount)
            {
                //prize.amount -= 1;
                //await _lotteryActivityRepository.UpdateAsync(activity);
                return null;
            }
            else
            {
                return prize;
            }
        }

        /// <summary>
        /// 验证产品是否已经扫码出库，并且已经出库超过指定间隔时间
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        private async Task<VerifyResult> VerifyOut(string qrcode)
        {
            //抽奖限制：出库24小时才能抽奖
            var qrcodekey = qrcode.Substring(0, 4);
            var offset = qrcode.Substring(4, 7);

            //获取出库状态 偏移量的位置为 1,说明已经出库
            var bitValue = (await _database.StringGetBitAsync(qrcodekey, Convert.ToInt32(offset)));//.get(100);
            if (bitValue)
            {
                //获取出库数据是否超过24小时
                var value = await _database.StringGetAsync(qrcode);
                if (value.HasValue)
                {
                    // key 存在，说明还没有超过24小时,继续处理
                    return new VerifyResult()
                    {
                        IsSuccess = false,
                        Message = "标签已经扫码出库，但是还没有到抽奖时间",
                    };
                }
                else
                {
                    return new VerifyResult()
                    {
                        IsSuccess = true,
                        Message = "标签已经扫码出库，但是还没有到抽奖时间",
                    };
                }
            }
            else
            {
                // 偏移量的位置值为 0，说明数据还没有出库
                return new VerifyResult()
                {
                    IsSuccess = false,
                    Message = "标签还没有扫码出库，无法参与活动",
                };
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
            var value = await VerifyOut(qrcode);
            if (!value.IsSuccess)
            {
                return value;
            }

            var activityPrize = await _activityPrizeRepository.ExistAsync(a => a.PrizeNumber == prizenumber);
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

            return new VerifyResult() { IsSuccess = true, Message = "" };
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
            var lotteryRecord = new LotteryRecord()
            {
                OpenId = openid,
                Claimed = ClaimedStatus.NotClaimed,
                QRCode = qrcode,
                ActivityNumber = prize.LotteryActivity.ActivityNumber,
                ActivityName = prize.LotteryActivity.Name,
                ActivityDescription = prize.LotteryActivity.Description,
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
                // var verify = await VerifyLottery(openid, qrcode // 验证是否有抽奖资格
                var verify = await VerifyLottery(openid, qrcode, prizenumber);
                if (!verify.IsSuccess)
                {
                    return new LotteryResult() { IsSuccess = false, Message = verify.Message };
                }

                // 随机抽取一个参加活动的奖品
                var prize = await GetLuckyPrize(prizenumber);
                if (prize is null)
                {
                    return new LotteryResult() { IsSuccess = false, Message = "奖品已经抽完" };
                }
                //根据奖品的中奖概率随机求得一个中奖数值号码
                var randomPrizeNumber = await GlobalUtility.GetRandomInt(prize.Probability);

                if (randomPrizeNumber == prize.UniqueNumber)
                {
                    // 记录抽奖数据
                    await RecordLotterySuccess(openid, qrcode, prize);
                    prize.Unredeemed = prize.Unredeemed + 1;
                    await _activityPrizeRepository.UpdateActivityPrizeAsync(prize);
                    return new LotteryResult()
                    {
                        IsSuccess = true,
                        Message = $"恭喜你抽中了奖品:{prize.Name}",
                        PrizeType = prize.Type.ToString(),
                        PrizeName = prize.Name,
                        PrizeDescription = prize.Description,
                        PrizeImage = prize.ImageUrl,
                    };
                }
                else
                {
                    await RecordLotteryFail(openid, qrcode, prize);
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
    }
}