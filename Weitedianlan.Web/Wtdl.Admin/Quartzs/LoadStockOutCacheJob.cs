using Quartz;
using StackExchange.Redis;
using Wtdl.Repository;

namespace Wtdl.Admin.Quartzs
{
    [DisallowConcurrentExecution]
    public class LoadStockOutCacheJob : IJob
    {
        private readonly WLabelStorageRepository _wLabelStorageRepository;
        private readonly IDatabase _database;

        private readonly ILogger<LoadStockOutCacheJob> _logger;

        public LoadStockOutCacheJob(WLabelStorageRepository repository,
            IConnectionMultiplexer connectionMultiplexer,
            ILogger<LoadStockOutCacheJob> logger)
        {
            _wLabelStorageRepository = repository;
            _database = connectionMultiplexer.GetDatabase();
            _logger = logger;
        }

        /// <summary>
        /// 查询  W_LabelStorage 表中的数据，将数据存入缓存中
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("初始化出库状态缓存开始");
            var list = await _wLabelStorageRepository.GetYearMonthGroupByListAsync();

            _logger.LogInformation($"初始化出库状态缓存数量：{list.Count()}个月");
            foreach (var item in list)
            {
                //按年份查询每月的数据

                var qrcodelist = await _wLabelStorageRepository.GetMonthGroupByQRCodeListAsync(Int32.Parse(item.Year), Int32.Parse(item.Month));

                _logger.LogInformation($"正在缓存{item.Year}年-{item.Month}月的数据,数据量：{qrcodelist.Length}");
                for (int i = 0; i < qrcodelist.Length; i++)
                {
                    var codekey = qrcodelist[i].Substring(0, 4);
                    var offset = qrcodelist[i].Substring(4, 7);

                    //将数据出库状态存入位图缓存中
                    await _database.StringSetBitAsync(codekey, Convert.ToInt64(offset), true);
                }

                _logger.LogInformation($"缓存{item.Year}年-{item.Month}月的数据完成,数据量：{qrcodelist.Length}");
                //  await _database.StringSetAsync(item., item);
            }
        }
    }
}