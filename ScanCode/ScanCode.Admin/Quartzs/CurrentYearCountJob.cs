using Microsoft.Extensions.Caching.Memory;
using Quartz;
using ScanCode.Model;
using ScanCode.Repository;

namespace ScanCode.Web.Admin.Quartzs
{
    /// <summary>
    /// Quartz 定时任务每天凌晨统计当年数据
    /// </summary>
    [DisallowConcurrentExecution]
    public class CurrentYearCountJob : IJob
    {
        private WLabelStorageRepository _storageRepository;
        private IMemoryCache _cache;
        private object _cacheLock = new object();
        private ILogger<CurrentYearCountJob> _logger;

        public CurrentYearCountJob(IMemoryCache cache,
            WLabelStorageRepository repository,
            ILogger<CurrentYearCountJob> logger)
        {
            _logger = logger;
            _storageRepository = repository;
            _cache = cache;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(DateTimeOffset.Now);
            int newValue;

            int counter;
            if (!_cache.TryGetValue(CacheKeys.DayCacheKey, out counter))
            {
                _logger.LogInformation("从数据库获取数据");
                //获取当前数据
                counter = await _storageRepository.GetThisYearOutCountAsync();
                _cache.Set(CacheKeys.YearChaeKey, counter);
                _logger.LogInformation("从数据库获取数据完成");
            }
            newValue = Interlocked.Increment(ref counter);
            _cache.Set(CacheKeys.DayCacheKey, newValue, options);
        }
    }
}