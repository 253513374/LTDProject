using Microsoft.Extensions.Caching.Memory;
using Quartz;
using Wtdl.Admin.Data;
using static Quartz.Logging.OperationName;

namespace Wtdl.Admin.Quartzs
{
    /// <summary>
    /// Quartz 定时任务每天凌晨统计当年数据
    /// </summary>
    public class CurrentYearCountJob : Job
    {
        private readonly IMemoryCache _cache;

        public CurrentYearCountJob(IMemoryCache cache)
        {
            _cache = cache;
        }

        public override async Task Execute(IJobExecutionContext context)
        {
            var now = DateTime.Now;
            var timeToMidnight = new TimeSpan(24, 0, 0) - now.TimeOfDay;
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(timeToMidnight);
            int newValue;
            lock (_cacheLock)
            {
                int counter;
                if (!_cache.TryGetValue(CacheKeys.DayCacheKey, out counter))
                {
                    _logger.LogInformation("从数据库获取数据");
                    //获取当前数据
                    counter = _storageRepository.GetCount();
                    _cache.Set(CacheKeys.DayCacheKey, counter);
                    _logger.LogInformation("从数据库获取数据完成");
                }
                newValue = Interlocked.Increment(ref counter);
                _cache.Set(CacheKeys.DayCacheKey, newValue, options);
            }

            //_logger.LogInformation("缓存更新完成");
        }
    }
}