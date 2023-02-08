using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Weitedianlan.Model.Entity;
using static System.Net.Mime.MediaTypeNames;

namespace Wtdl.Repository.Utility
{
    public static class GlobalUtility
    {
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
                MaxCashValue = activityPrize.MaxCashValue,
                MinCashValue = activityPrize.MinCashValue,
                CashValue = activityPrize.CashValue,
                Identifier = activityPrize.Identifier,
                Amount = activityPrize.Amount,
                Probability = activityPrize.Probability,
                IsActive = activityPrize.IsActive,
                CreateTime = activityPrize.CreateTime,
                AdminUser = activityPrize.AdminUser,
                LotteryActivityId = activityPrize.LotteryActivityId,
                Description = activityPrize.Description,
                EndTime = activityPrize.EndTime,
                StartTime = activityPrize.StartTime,
                IsJoinActivity = activityPrize.IsJoinActivity,
                TotalLimit = activityPrize.TotalLimit,
                UserLimit = activityPrize.UserLimit,
                WinnerCount = activityPrize.WinnerCount,
                LotteryActivity = activityPrize.LotteryActivity,
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
                ImageUrl = prize.ImageUrl,
                Name = prize.Name,
                Type = prize.Type,
                MaxCashValue = prize.MaxCashValue,
                MinCashValue = prize.MinCashValue,
                CashValue = prize.CashValue,
                Identifier = prize.Identifier,
                Amount = prize.Amount,
                Probability = prize.Probability,
                IsActive = prize.IsActive,
                CreateTime = prize.CreateTime,
                AdminUser = prize.AdminUser,
                LotteryActivityId = prize.LotteryActivityId,
                Description = prize.Description,
                EndTime = prize.EndTime,
                StartTime = prize.StartTime,
                IsJoinActivity = prize.IsJoinActivity,
                TotalLimit = prize.TotalLimit,
                UserLimit = prize.UserLimit,
                WinnerCount = prize.WinnerCount,
                LotteryActivity = prize.LotteryActivity,
            };
        }
    }
}