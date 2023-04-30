using System.Text.Json;
using System.Text.Json.Serialization;
using StackExchange.Redis;
using Wtdl.Model.Entity.Analysis;
using Wtdl.Repository;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Wtdl.RedisCache
{
    public class RedisCacheService : IRedisCache
    {
        private readonly IDatabase _database;
        private readonly WLabelStorageRepository _WLabelStorageRepository;
        private readonly OutStorageRepository _outStorageRepository;

        public RedisCacheService(IConnectionMultiplexer multiplexer,
            WLabelStorageRepository wLabelStorageRepository,
                OutStorageRepository outStorageRepository)
        {
            _WLabelStorageRepository = wLabelStorageRepository;
            _outStorageRepository = outStorageRepository;
            _database = multiplexer.GetDatabase();
        }

        public void AppendData(string key, string data)
        {
            _database.StringAppend(key, data);
        }

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
            // return JsonConvert.DeserializeObject<ReportFormsNever>(jsonData);
        }

        public Task SetObjectAsync(string key, object data)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // 处理循环引用
                PropertyNameCaseInsensitive = true, // 忽略属性名大小写
                WriteIndented = true // 使输出的 JSON 格式化，可选
            };

            string jsonData = JsonSerializer.Serialize(data, options); // JsonConvert.SerializeObject(data, settings);
            _database.StringSet(key, jsonData);

            return Task.CompletedTask;
        }

        public async Task<bool> SetBitAsync(string qrcode, bool bit = true)
        {
            // var redisdb = _redis.GetDatabase(); //RedisClientFactory.GetDatabase();
            //截取qrcode 中的偏移量
            var offset = qrcode.Substring(4, 7);//位图下标
            var key = qrcode.Substring(0, 4);

            //判断是否存在该key
            if (!_database.KeyExists(key))
            {
                /*使用 BITFIELD 命令初始化一个名为 [key] 的位图，使用 CREATE 子命令指定创建一个新的位图，
                 使用 u32 类型表示位图使用 32 位整数存储，使用 #10000000 表示位图的大小为 10000000。
                执行完成后，位图的初始状态全部为 0。*/
                // await _database.ExecuteAsync("BITFIELD", key, "CREATE", "u32", "#10000000");

                await _database.ExecuteAsync("BITFIELD", key, "SET", "u32", "10000000", "0");
            }
            ///设置位图状态
            return await _database.StringSetBitAsync(key, Convert.ToInt64(offset), bit);
        }

        public async Task SetBulkBitAsync(List<string> qrcodesList)
        {
            for (int i = 0; i < qrcodesList.Count; i++)
            {
                var qrcode = qrcodesList[i];
                // var redisdb = _redis.GetDatabase(); //RedisClientFactory.GetDatabase();
                //截取qrcode 中的偏移量
                var offset = qrcode.Substring(4, 7);//位图下标
                var key = qrcode.Substring(0, 4);

                //判断是否存在该key
                //if (!_database.KeyExists(key))
                //{
                //    /*使用 BITFIELD 命令初始化一个名为 [key] 的位图，使用 CREATE 子命令指定创建一个新的位图，
                //     使用 u32 类型表示位图使用 32 位整数存储，使用 #10000000 表示位图的大小为 10000000。
                //    执行完成后，位图的初始状态全部为 0。*/
                //    _database.Execute("BITFIELD", key, "CREATE", "u32", "#10000000");
                //}

                ///设置位图状态为true
                await _database.StringSetBitAsync(key, Convert.ToInt64(offset), true);
            }
        }

        public async Task<bool> GetBitAsync(string qrcode)
        {
            var key = qrcode.Substring(0, 4);
            var offset = qrcode.Substring(4, 7);//位图下标
            // 获取key为"bitstring"，位于偏移量5的位的值
            bool result = await _database.StringGetBitAsync(key, Convert.ToInt64(offset));
            return result ? true : false;
        }

        /// <summary>
        /// 缓存出库的过期时间，红包使用。出库超过24小时才可以领取红包
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public virtual async Task<bool> SetRedPacketAsync(string qrcode)
        {
            // var redisdb = _database.GetDatabase(); //RedisClientFactory.GetDatabase();
            return await _database.StringSetAsync(qrcode, DateTime.Now.ToString(), TimeSpan.FromHours(24));
        }

        //public void SetObjectAsync(string key, ReportFormsNever data)
        //{
        //    throw new NotImplementedException();
        //}

        //private async Task<ReportFormsNever> GetDataFromStorage()
        //{
        //    var formsNever = new ReportFormsNever();

        //    formsNever.GroupByOrderCounts = await _outStorageRepository.GetGrapByYearAndOrderAsync();

        //    if (formsNever.GroupByOrderCounts is null || formsNever.GroupByOrderCounts.Count == 0)
        //    {
        //        formsNever.GroupByOrderCounts = await _WLabelStorageRepository.GetGroupByOrderNumbelsAsync(DateTime.Now.Year);
        //    }

        //    var groupbyyearorders = await _outStorageRepository.GetGrapByYearAsync();
        //    formsNever.GroupByYearCounts = groupbyyearorders.GroupBy(g => g.Year).Select(s =>
        //        new OutStorageAnalysis
        //        {
        //            Year = s.Key,
        //            Count = s.Sum(s => s.Count),
        //        }).ToList();

        //    return formsNever;
        //}
    }
}