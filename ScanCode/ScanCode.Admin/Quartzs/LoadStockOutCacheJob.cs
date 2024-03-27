using Quartz;
using ScanCode.Repository;
using StackExchange.Redis;

namespace ScanCode.Web.Admin.Quartzs
{
    /// <summary>
    /// 这个任务是，查询数据库中已经出库的数据，
    /// 然后使用redis位图功能，标记标签的出库位为1(已经出库状态)
    /// </summary>
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

        ///<summary>
        /// 执行任务的方法。
        /// </summary>
        /// <param name="context">任务执行上下文。</param>
        /// <returns>返回一个异步任务。</returns>
        public async Task Execute(IJobExecutionContext context)
        {
            // 记录开始初始化出库状态缓存的信息
            _logger.LogInformation("初始化出库状态缓存开始");

            // 获取过去12个月的年份和月份列表
            var list = await _wLabelStorageRepository.GetYearMonthGroupByListAsync(-12);

            // 记录初始化出库状态缓存的数量
            _logger.LogInformation($"初始化出库状态缓存数量：{list.Count()}个月");

            // 遍历年份和月份列表
            foreach (var item in list)
            {
                // 尝试解析年份和月份
                if (!int.TryParse(item.Year, out int year) || !int.TryParse(item.Month, out int month))
                {
                    // 如果解析失败，记录警告信息并跳过当前循环
                    _logger.LogWarning($"无法解析年月：{item.Year}-{item.Month}");
                    continue;
                }

                // 获取指定年份和月份的二维码列表
                var qrcodelist = await _wLabelStorageRepository.GetMonthGroupByQRCodeListAsync(year, month);

                // 记录正在缓存的年份、月份和数据量
                _logger.LogInformation($"正在缓存{item.Year}年-{item.Month}月的数据,数据量：{qrcodelist.Length}");

                // 创建Redis事务
                var tran = _database.CreateTransaction();

                // 遍历二维码列表
                for (int i = 0; i < qrcodelist.Length; i++)
                {
                    // 获取二维码的前4位作为键
                    var codekey = qrcodelist[i].Substring(0, 4);

                    // 获取二维码的第5位到第11位作为偏移量
                    var offset = qrcodelist[i].Substring(4, 7);

                    // 尝试解析偏移量
                    if (Int64.TryParse(offset, out long longOffset))
                    {
                        // 如果解析成功，将键对应的位图的偏移量位置设为1
                        _ = tran.StringSetBitAsync(codekey, longOffset, true);
                    }
                    else
                    {
                        // 如果解析失败，记录警告信息
                        _logger.LogWarning($"无法解析offset：{offset} : {qrcodelist[i]}");
                    }
                }

                // 尝试执行Redis事务
                try
                {
                    await tran.ExecuteAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // 如果执行失败，记录错误信息
                    _logger.LogError(ex, $"在执行Redis事务时发生错误：{ex.Message}");
                }

                // 记录完成缓存的年份、月份和数据量
                _logger.LogInformation($"缓存{item.Year}年-{item.Month}月的数据完成,数据量：{qrcodelist.Length}");
            }
        }
    }
}