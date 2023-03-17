using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using Wtdl.Model.Entity;
using Wtdl.Model.Enum;
using Wtdl.Controller.Services;

using Wtdl.Model.ResponseModel;
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
        private readonly WLabelStorageRepository _wLabelStorageRepository;

        //private readonly IDatabase _database;
        private readonly ILogger<LotteryService> _logger;

        private readonly IMemoryCache _memoryCache;

        private HubService _hubService;

        public LotteryService(LotteryActivityRepository lotteryActivityRepository,
            WLabelStorageRepository wLabelStorageRepository,
            LotteryRecordRepository recordRepository,
            VerificationCodeRepository verificationCodeRepository,
            ActivityPrizeRepository repository,
            HubService hubService,
        //IDistributedCache distributedCache,
        //  IConnectionMultiplexer connectionMultiplexer,
        ILogger<LotteryService> logger,
            IMemoryCache cache)
        {
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
                var result = await _lotteryActivityRepository.GetLotteryActivityAsync();

                var activiprize =
                  await _activityPrizeRepository.FindAsync(f => f.LotteryActivityId == result.Id && f.IsActive == true);

                result.Prizes = activiprize.ToList();

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
        /// 返回活动奖品
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        //private async Task<ActivityPrize> GetActivityPrize(string a)
        //{
        //}

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
        /// 验证产品是否已经扫码出库，并且已经出库超过指定间隔时间
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        private async Task<VerifyResult> VerifyOut(string qrcode)
        {
            // var outqrcode = await  _wLabelStorageRepository.FindSingleAsync(f => f.QRCode.Contains(qrcode));
            var outqrcode = await _wLabelStorageRepository.AnyRedPacket(qrcode);
            if (!outqrcode)
            {
                //var outOKTime =  outqrcode.OutTime.AddDays(1);
                return new VerifyResult()
                {
                    IsSuccess = false,
                    Message = "无法参与抽奖",
                };
            }
            else
            {
                return new VerifyResult()
                {
                    IsSuccess = true,
                    Message = "可以参加活动",
                };
            }

            ////抽奖限制：出库24小时才能抽奖
            //var qrcodekey = qrcode.Substring(0, 4);
            //var offset = qrcode.Substring(4, 7);

            ////获取出库状态 偏移量的位置为 1,说明已经出库
            //var bitValue = (await _database.StringGetBitAsync(qrcodekey, Convert.ToInt32(offset)));//.get(100);
            //if (bitValue)
            //{
            //    //获取出库数据是否超过24小时
            //    var value = await _database.StringGetAsync(qrcode);
            //    if (value.HasValue)
            //    {
            //        // key 存在，说明还没有超过24小时,继续处理
            //        return new VerifyResult()
            //        {
            //            IsSuccess = false,
            //            Message = "标签已经扫码出库，但是还没有到抽奖时间",
            //        };
            //    }
            //    else
            //    {
            //        return new VerifyResult()
            //        {
            //            IsSuccess = true,
            //            Message = "标签已经扫码出库，但是还没有到抽奖时间",
            //        };
            //    }
            //}
            //else
            //{
            //    // 偏移量的位置值为 0，说明数据还没有出库
            //    return new VerifyResult()
            //    {
            //        IsSuccess = false,
            //        Message = "标签还没有扫码出库，无法参与活动",
            //    };
            //}
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
                // var verify = await VerifyLottery(openid, qrcode // 验证是否有抽奖资格
                var verify = await VerifyLottery(openid, qrcode, prizenumber);
                if (!verify.IsSuccess)
                {
                    return new LotteryResult() { IsSuccess = false, Message = verify.Message };
                }

                // 随机抽取一个参加活动的奖品
                var prize = await GetLuckyPrize(prizenumber);

                await _hubService.SendLotteryCountAsync();

                if (prize is null)
                {
                    return new LotteryResult() { IsSuccess = false, Message = "奖品已经抽完" };
                }
                //根据奖品的中奖概率随机求得一个中奖数值号码
                var randomPrizeNumber = await GlobalUtility.GetRandomInt(prize.Probability);

                if (randomPrizeNumber == prize.UniqueNumber)
                {
                    await _hubService.SendLotteryWinCountAsync();

                    // 记录抽奖数据
                    await RecordLotterySuccess(openid, qrcode, prize);

                    //减少可抽奖奖品数量，更新到数据库
                    prize.Unredeemed++;
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