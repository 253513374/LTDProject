using ScanCode.Repository;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ScanCode.RedisCache
{
    public class RedisCacheService : IRedisCache
    {
        private readonly IDatabase _database;
        private readonly WLabelStorageRepository _wLabelStorageRepository;
        private readonly OutStorageRepository _outStorageRepository;
        private readonly ILogger _logger;

        public RedisCacheService(IConnectionMultiplexer multiplexer,
            WLabelStorageRepository wLabelStorageRepository,
                OutStorageRepository outStorageRepository, ILogger<RedisCacheService> logger)
        {
            _wLabelStorageRepository = wLabelStorageRepository;
            _outStorageRepository = outStorageRepository;
            _logger = logger;
            _database = multiplexer.GetDatabase();
        }

        /// <summary>
        /// 向 Redis 缓存中追加数据。
        /// </summary>
        /// <param name="key">缓存键。</param>
        /// <param name="data">要追加的数据。</param>
        public void AppendData(string key, string data)
        {
            _database.StringAppend(key, data);
        }

        /// <summary>
        /// 异步从 Redis 缓存中获取对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="key">缓存键。</param>
        /// <returns>获取到的对象。</returns>
        public async Task<T?> GetObjectAsync<T>(string key)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // 处理循环引用
                PropertyNameCaseInsensitive = true, // 忽略属性名大小写
                WriteIndented = true // 使输出的 JSON 格式化，可选
            };
            string? jsonData = await _database.StringGetAsync(key);
            if (jsonData is null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(jsonData, options);
        }

        /// <summary>
        /// 异步将对象存储到 Redis 缓存中。
        /// </summary>
        /// <param name="key">缓存键。</param>
        /// <param name="data">要存储的对象。</param>
        /// <param name="expiry">过期时间。</param>
        /// <returns>表示异步操作的任务。</returns>
        public Task SetObjectAsync(string key, object data, TimeSpan? expiry = null)
        {
            //计算当前时间到明年1月1日还有多少时间
            DateTime now = DateTime.Now;
            DateTime nextYearFirstDay = new DateTime(now.Year + 1, 1, 1);
            TimeSpan timeToNextYearFirstDay = nextYearFirstDay - now;

            // 如果没有提供过期时间，那么设置为2年
            expiry ??= TimeSpan.FromDays(365 * 2);
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // 处理循环引用
                PropertyNameCaseInsensitive = true, // 忽略属性名大小写
                WriteIndented = true // 使输出的 JSON 格式化，可选
            };

            string jsonData = JsonSerializer.Serialize(data, options);
            _database.StringSet(key, jsonData, expiry);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 设置出库状态，该方法使用位图来保存状态。
        /// </summary>
        /// <param name="qrcode">二维码。</param>
        /// <param name="bit">位状态。</param>
        /// <returns>表示异步操作的任务。</returns>
        public async Task<bool> SetBitAsync(string qrcode, bool bit = true)
        {
            var offsetString = qrcode.Substring(4, 7);//位图下标
            var key = qrcode.Substring(0, 4);

            if (!await _database.KeyExistsAsync(key))
            {
                await _database.ExecuteAsync("BITFIELD", key, "SET", "u32", "10000000", "0");
            }

            return await _database.StringSetBitAsync(key, long.Parse(offsetString), bit);
        }

        /// <summary>
        /// 异步批量设置位图状态。
        /// </summary>
        /// <param name="qrcodesList">二维码列表。</param>
        /// <param name="bitstatus">位状态。</param>
        /// <returns>表示异步操作的任务。</returns>
        public async Task SetBulkBitAsync(List<string> qrcodesList, bool bitstatus)
        {
            try
            {
                foreach (var code in qrcodesList)
                {
                    if (code.Length < 11)
                    {
                        _logger.LogWarning($"跳过代码 {code} 因为它太短了。");
                        continue;
                    }

                    var key = code.Substring(0, 4);
                    var offset = code.Substring(4, 7);

                    _logger.LogInformation($"代码 {code} 的设置位。key:{key},offset:{offset},bitstatus:{bitstatus}");
                    await _database.StringSetBitAsync(key, long.Parse(offset), bitstatus);
                }

                _logger.LogInformation("批量完成订单退货位图状态设置");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "设置位时出错。");
                throw;
            }
        }

        /// <summary>
        /// 异步获取位图状态。
        /// </summary>
        /// <param name="qrcode">二维码。</param>
        /// <returns>位状态。</returns>
        public async Task<bool> GetBitAsync(string qrcode)
        {
            var key = qrcode.Substring(0, 4);
            var offset = qrcode.Substring(4, 7);//位图下标
            bool result = await _database.StringGetBitAsync(key, Convert.ToInt64(offset));
            return result ? true : false;
        }

        /// <summary>
        /// 缓存出库的过期时间，红包使用。出库超过24小时才可以领取红包。
        /// </summary>
        /// <param name="qrcode">二维码。</param>
        /// <returns>表示异步操作的任务。</returns>
        public virtual async Task<bool> SetRedPacketAsync(string qrcode)
        {
            return await _database.StringSetAsync(qrcode, DateTime.Now.ToString(), TimeSpan.FromHours(24));
        }

        /// <summary>
        /// 异步保存验证码。
        /// </summary>
        /// <param name="phoneNumber">手机号码。</param>
        /// <param name="verificationCode">验证码。</param>
        /// <param name="expiry">过期时间。</param>
        /// <returns>表示异步操作的任务。</returns>
        public Task SaveVerificationCodeAsync(string phoneNumber, string verificationCode, TimeSpan? expiry = null)
        {
            // 如果没有提供过期时间，那么设置为5分钟
            expiry ??= TimeSpan.FromMinutes(5);
            _database.StringSet(phoneNumber, verificationCode, expiry);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 异步验证验证码。
        /// </summary>
        /// <param name="phoneNumber">手机号码。</param>
        /// <param name="userEnteredCode">用户输入的验证码。</param>
        /// <returns>验证码验证结果。</returns>
        public async Task<VerificationResult> VerifyCodeAsync(string phoneNumber, string userEnteredCode)
        {
            var storedCode = await _database.StringGetAsync(phoneNumber);
            if (storedCode.IsNullOrEmpty)
            {
                return new VerificationResult { IsSuccess = false, ErrorMessage = "验证码过期" };
            }
            if (storedCode.ToString() != userEnteredCode)
            {
                return new VerificationResult { IsSuccess = false, ErrorMessage = "验证码错误" };
            }

            await _database.KeyDeleteAsync(phoneNumber);
            return new VerificationResult { IsSuccess = true };
        }
    }
}