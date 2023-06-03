using Quartz;
using ScanCode.Repository;
using StackExchange.Redis;

namespace ScanCode.Web.Admin.Quartzs
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
            var list = await _wLabelStorageRepository.GetYearMonthGroupByListAsync(-12);

            _logger.LogInformation($"初始化出库状态缓存数量：{list.Count()}个月");
            foreach (var item in list)
            {
                if (!int.TryParse(item.Year, out int year) || !int.TryParse(item.Month, out int month))
                {
                    _logger.LogWarning($"无法解析年月：{item.Year}-{item.Month}");
                    continue;
                }

                var qrcodelist = await _wLabelStorageRepository.GetMonthGroupByQRCodeListAsync(year, month);

                _logger.LogInformation($"正在缓存{item.Year}年-{item.Month}月的数据,数据量：{qrcodelist.Length}");

                var tran = _database.CreateTransaction();
                for (int i = 0; i < qrcodelist.Length; i++)
                {
                    var codekey = qrcodelist[i].Substring(0, 4);
                    var offset = qrcodelist[i].Substring(4, 7);

                    if (Int64.TryParse(offset, out long longOffset))
                    {
                        _ = tran.StringSetBitAsync(codekey, longOffset, true);
                    }
                    else
                    {
                        _logger.LogWarning($"无法解析offset：{offset} : {qrcodelist[i]}");
                    }
                }
                await tran.ExecuteAsync().ConfigureAwait(false);

                _logger.LogInformation($"缓存{item.Year}年-{item.Month}月的数据完成,数据量：{qrcodelist.Length}");
            }
        }
    }
}