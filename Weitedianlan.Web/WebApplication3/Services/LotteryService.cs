using System.Diagnostics;
using System.Security.Cryptography;
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

        private readonly ILogger<LotteryService> _logger;

        public LotteryService(LotteryActivityRepository lotteryActivityRepository,
            LotteryRecordRepository recordRepository,
            VerificationCodeRepository verificationCodeRepository,
            ILogger<LotteryService> logger)
        {
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
        public async Task<ActivityViewModel> GetLotteryActivityAsync()
        {
            try
            {
                var result = await _lotteryActivityRepository.GetLotteryActivityAsync(a => a.IsActive == true);

                if (result is not null)
                {
                    var view = new ActivityViewModel()
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
                        view.Prizes.Add(new PrizeViewModel()
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

                return new ActivityViewModel() { Msg = "没有活动" };
            }
            catch (Exception e)
            {
                _logger.LogError($"服务器错误：{e}");
                return new ActivityViewModel() { Msg = $"服务器错误：{e.Message}" };
            }
        }

        /// <summary>
        /// 随机抽取一个奖品
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        private async Task<PrizeViewModel> LuckyPrize(string openid, string qrcode)
        {
            ; ;

            //  var selectPrize = GetRandomPrize(activity);

            return new PrizeViewModel();
        }

        /// <summary>
        /// 随机抽取一个奖品
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private async Task<ActivityPrize> GetRandomPrize()
        {
            var activity = await _lotteryActivityRepository.GetLotteryActivityAsync(a => a.IsActive == true);
            var arr = activity.Prizes.Select(s => s.Id).ToArray();

            Random random = new Random();
            int index = random.Next(0, arr.Length);

            return activity.Prizes.ToList()[index];
        }

        /// <summary>
        /// 抽奖之前验证用户与标签序号是否有抽奖资格
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        private async Task<bool> VerifyLottery(string openid, string qrcode)
        {
            // 标签序号是否存在
            var verificationCode = await _verificationCodeRepository.AnyAsync(a => a.AntiForgeryCode == qrcode);
            if (!verificationCode)
            {
                return false;
            }

            // 当前活动是否存在与活动是否处于激活状态
            var activity = await GetLotteryActivityAsync();
            if (activity is not null)
            {
                if (!activity.IsActive)
                {
                    return false;
                }
            }

            // 当前用户是否已经对标签序号抽过奖了
            var lotteryRecord = await _lotteryRecordRepository.AnyQRCodeRecordsAsync(openid, qrcode);
            if (lotteryRecord)
            {
                return false;
            }

            return true;
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
            };
            await _lotteryRecordRepository.AddAsync(lotteryRecord);
        }

        /// <summary>
        /// 抽奖
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public async Task<LotteryViewModel> GetLotteryResultAsync(string openid, string qrcode)
        {
            try
            {
                // var verify = await VerifyLottery(openid, qrcode // 验证是否有抽奖资格
                var verify = await VerifyLottery(openid, qrcode);
                if (!verify)
                {
                    return new LotteryViewModel() { Msg = "没有抽奖资格" };
                }

                // 随机抽取一个参加活动的奖品
                var prize = await GetRandomPrize();

                //根据奖品的中奖概率随机求得一个中奖数值号码
                var randomPrizeNumber = await GlobalUtility.GetRandomInt(prize.Probability);

                if (randomPrizeNumber == prize.UniqueNumber)
                {
                    // 记录抽奖数据
                    await RecordLotterySuccess(openid, qrcode, prize);
                    return new LotteryViewModel()
                    {
                        IsSuccess = true,
                        Msg = $"恭喜你抽中了奖品:{prize.Name}",
                        PrizeType = prize.Type.ToString(),
                        PrizeName = prize.Name,
                        PrizeDescription = prize.Description,
                        PrizeImage = prize.ImageUrl,
                    };
                }
                else
                {
                    await RecordLotteryFail(openid, qrcode, prize);
                    return new LotteryViewModel()
                    {
                        IsSuccess = false,
                        Msg = $"很遗憾没有中奖",
                        PrizeType = prize.Type.ToString(),
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"服务器错误：{e}");
                return new LotteryViewModel() { Msg = $"服务器错误：{e.Message}" };
            }
        }
    }
}