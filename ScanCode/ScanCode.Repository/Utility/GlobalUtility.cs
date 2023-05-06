using ScanCode.Model.Entity;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ScanCode.Repository.Utility
{
    public static class GlobalUtility
    {
        private static JsonSerializerOptions Options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        /// <summary>
        /// 计算文件哈希值
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ComputeFileHash(string filePath)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }

        public static string ComputeFileHash(Stream stream)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// 返回正则过滤后的字符串，正则【@"[\d]{12,20}$"】
        /// </summary>
        public static string GetRegexString(string str)
        {
            var pattern = @"[\d]{12,20}$";
            var match = Regex.Match(str, pattern);
            if (match.Success)
            {
                var result = match.Value;
                return result;
            }
            return "";
        }

        /// <summary>
        /// ActivityPrize类转换成为Prize类
        /// </summary>
        /// <param name="activityPrize"></param>
        /// <returns></returns>
        public static Prize MapperToPrize(ActivityPrize activityPrize)
        {
            return new Prize()
            {
                Id = activityPrize.Id,
                ImageUrl = activityPrize.ImageUrl,
                Name = activityPrize.Name,
                Type = activityPrize.Type,
                CashValue = activityPrize.CashValue,
                Identifier = activityPrize.Identifier,
                //Amount = activityPrize.Amount,
                Probability = activityPrize.Probability,
                IsActive = activityPrize.IsActive,
                CreateTime = activityPrize.CreateTime,
                AdminUser = activityPrize.AdminUser,
                //LotteryActivityId = activityPrize.LotteryActivityId,
                Description = activityPrize.Description,
                //EndTime = activityPrize.EndTime,
                //StartTime = activityPrize.StartTime,
                IsJoinActivity = activityPrize.IsJoinActivity,
                //TotalLimit = activityPrize.TotalLimit,
                //UserLimit = activityPrize.UserLimit,
                //WinnerCount = activityPrize.WinnerCount,
                //LotteryActivity = activityPrize.LotteryActivity,
            };
        }

        /// <summary>
        /// Prize类转换成为ActivityPrize类
        /// </summary>
        /// <param name="activityPrize"></param>
        /// <returns></returns>
        public static ActivityPrize MapperToActivityPrize(Prize prize)
        {
            return new ActivityPrize()
            {
                // PrizeNumber = prize.UniqueNumber,
                ImageUrl = prize.ImageUrl,
                Name = prize.Name,
                Type = prize.Type,
                CashValue = prize.CashValue,
                Identifier = prize.Identifier,
                //Amount = prize.Amount,
                Probability = prize.Probability,
                IsActive = prize.IsActive,
                CreateTime = prize.CreateTime,
                AdminUser = prize.AdminUser,
                // LotteryActivityId = prize.LotteryActivityId,
                Description = prize.Description,
                //EndTime = prize.EndTime,
                //StartTime = prize.StartTime,
                IsJoinActivity = prize.IsJoinActivity,
                //TotalLimit = prize.TotalLimit,
                //UserLimit = prize.UserLimit,
                //WinnerCount = prize.WinnerCount,
                //LotteryActivity = prize.LotteryActivity,
            };
        }

        public static string SerializeObject(object obj)
        {
            return JsonSerializer.Serialize(obj, Options);
        }

        /// <summary>
        /// 使用RNGCryptoServiceProvider 生成安全随机数
        /// </summary>
        /// <param name="minimumValue">最小整数</param>
        /// <param name="maximumValue">最大整数</param>
        /// <returns>返回一个随机整数</returns>
        public static Task<int> GetRandomInt(int minimumValue, int maximumValue)
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

        /// <summary>
        /// 返回概率范围区间内的随机整数
        /// </summary>
        /// <param name="probability"></param>
        /// <returns></returns>
        public static async Task<int> GetRandomInt(double probability)
        {
            //求得概率的最大整数范围。
            int multiplier = (int)(1 / probability);

            return await GetRandomInt(1, multiplier);
        }
    }
}