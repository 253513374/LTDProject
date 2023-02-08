using System.Security.Cryptography;
using Weitedianlan.Model.Entity;
using Wtdl.Mvc.Models;
using Wtdl.Repository;

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
        /// 使用RNGCryptoServiceProvider 生成安全随机数
        /// </summary>
        /// <param name="minimumValue"></param>
        /// <param name="maximumValue"></param>
        /// <returns></returns>
        private Task<int> GetInt(int minimumValue, int maximumValue)
        {
            RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();

            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                byte[] fourBytes = new byte[4];
                _rng.GetBytes(fourBytes);

                scale = BitConverter.ToUInt32(fourBytes, 0);
            }

            return Task.FromResult((int)(minimumValue + (maximumValue - minimumValue + 1) *
                (scale / (double)uint.MaxValue)));
        }

        private async Task<PrizeViewModel> LuckyPrize(string openid, string qrcode)
        {
            var activity = await GetLotteryActivityAsync();

            double minProbability = 0.0;
            foreach (var VARIABLE in activity.Prizes)
            {
                Math.MinMagnitude(minProbability, VARIABLE.Probability);
            }

            return new PrizeViewModel();
        }
    }
}