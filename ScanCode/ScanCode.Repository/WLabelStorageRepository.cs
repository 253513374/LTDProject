using EFCore.BulkExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScanCode.Model.Entity;
using ScanCode.Model.Entity.Analysis;
using ScanCode.Repository.Data;
using ScanCode.Repository.Interface;
using System.Linq.Expressions;

namespace ScanCode.Repository
{
    public class WLabelStorageRepository : RepositoryBase<W_LabelStorage>
    {
        private readonly IDbContextFactory<LotteryContext> _contextFactory;
        private readonly ILogger<W_LabelStorage> _logger;
        private readonly IMediator _mediator;
        //private IWebHostEnvironment _env;

        public WLabelStorageRepository(IDbContextFactory<LotteryContext> context,
            IMediator mediator,
            ILogger<W_LabelStorage> logger) : base(context, mediator, logger)
        {
            _contextFactory = context;
            _mediator = mediator;
            _logger = logger;
        }

        //批量插入数据
        public async Task BulkInsertAsync(List<W_LabelStorage> list)
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    await context.BulkInsertOrUpdateAsync(list);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"批量出库异常：BulkInsertAsync：{e.Message}");
            }
        }

        //public async Task<List<W_LabelStorage>> GetLatestRecordsAsync(int count)
        //{
        //    using (var context = _contextFactory.CreateDbContext())
        //    {
        //        return await context.WLabelStorages
        //            .OrderByDescending(x => x.OrderTime)
        //            .Take(count)
        //            .ToListAsync();
        //    }
        //}

        //返回时间范围内的数据
        public async Task<List<GroupByWLabelStorage>> GetGroupByTimeRecordsAsync(DateTime startTime, DateTime endTime)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var groupedData = context.WLabelStorages.AsNoTracking()
                    .Where(x => x.OutTime >= startTime && x.OutTime < endTime.AddDays(1))
                    .OrderByDescending(o => o.OutTime)
                    .GroupBy(x => new { x.OrderNumbels, x.OrderTime })
                    //(x.OrderTime))
                    .Select(g => new GroupByWLabelStorage()
                    {
                        OrderNumbels = g.Key.OrderNumbels,
                        Time = g.Key.OrderTime,
                        Count = g.Count(),
                    });

                var resultList = await groupedData.GroupJoin(context.Agents,
                        t => t.OrderNumbels,
                a => a.AID,
                        (t, a) => new { outstorage = t, agents = a })
                    .SelectMany(s => s.agents.DefaultIfEmpty(), (s, suagent) => new GroupByWLabelStorage()
                    {
                        OrderNumbels = s.outstorage.OrderNumbels,
                        Time = s.outstorage.Time,
                        Count = s.outstorage.Count,
                        AgentName = suagent.AName,
                    }).Distinct()
                    .OrderByDescending(o => o.Time).ToListAsync();

                if (resultList is null || resultList.Count == 0)
                {
                    //匹配18年以前的数据
                    var groupedDataold = context.WLabelStorages.AsNoTracking()
                        .Where(x => x.OrderTime >= startTime && x.OrderTime <= endTime)
                        .OrderByDescending(o => o.OrderTime)
                        .GroupBy(x => new { x.OrderNumbels, x.OrderTime, x.Dealers })
                        //(x.OrderTime))
                        .Select(g => new GroupByWLabelStorage()
                        {
                            OrderNumbels = g.Key.OrderNumbels,
                            ID = g.Key.Dealers,
                            Time = g.Key.OrderTime,
                            Count = g.Count(),
                        });

                    resultList = await groupedDataold.GroupJoin(context.Agents,
                               t => t.OrderNumbels,
                               a => a.AID,
                               (t, a) => new { outstorage = t, agents = a })
                           .SelectMany(s => s.agents.DefaultIfEmpty(), (s, suagent) => new GroupByWLabelStorage()
                           {
                               OrderNumbels = s.outstorage.OrderNumbels,
                               Time = s.outstorage.Time,
                               Count = s.outstorage.Count,
                               AgentName = suagent.AName,
                           }).Distinct()
                           .OrderByDescending(o => o.Time).ToListAsync();
                }

                return resultList;
            }
        }

        public async Task<List<GroupByWLabelStorage>> GetLatestGroupByTimeRecordsAsync()
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var latestData = context.WLabelStorages.Select(e => e.OutTime).Max();
                    //var latestData = context.WLabelStorages.FromSqlRaw("SELECT MAX(ID) FROM W_LabelStorage");
                    //var latestData = context.WLabelStorages.FirstOrDefault(e => e.ID == maxId);
                    //   var latestData = await context.WLabelStorages.AsNoTracking().OrderByDescending(x => x.OutTime).FirstOrDefaultAsync();
                    var thirtyDaysAgo = latestData.AddDays(-30);// DateTime.Now.AddDays(-30);

                    return await GetGroupByTimeRecordsAsync(thirtyDaysAgo, DateTime.Now);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new List<GroupByWLabelStorage>();
                //re  throw;
            }
        }

        /// <summary>
        /// 统计W_LabelStorage 表中，每年的OrderNumbels总量
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<OutStorageAnalysis>> GetGroupByOrderNumbelsAsync(int Year)
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    ///分组统计年订单数
                    var graupbylist = await context.WLabelStorages
                        .GroupBy(x => new { x.OrderNumbels, x.OrderTime.Year, x.OrderTime.Month })
                        .Select(g => new OutStorageAnalysis()
                        {
                            OrderNumbels = g.Key.OrderNumbels,
                            Month = g.Key.Month,
                            Year = g.Key.Year,
                            Count = g.Count(),
                        }).ToListAsync();

                    context.BulkInsert(graupbylist.Where(w => w.Year >= 2015 && w.Year < DateTime.Now.Year).ToList());
                    context.SaveChanges();

                    var listGroupBy = graupbylist.GroupBy(g => g.Year).Select(s => new OutStorageAnalysis
                    {
                        Year = s.Key,
                        Count = s.Count(),
                    }).ToList();

                    return listGroupBy.Where(w => w.Year >= 2016 && w.Year < Year).ToList();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new List<OutStorageAnalysis>();
                //re  throw;
            }
        }

        public async Task<List<GroupByWLabelStorage>> GetGroupByOrderTimeAsync(Expression<Func<W_LabelStorage, bool>> predicate)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                //var thirtyDaysAgo = DateTime.Now.AddDays(-30);
                var groupedData = context.WLabelStorages
                    .Where(predicate)
                    .OrderByDescending(o => o.OutTime)
                    .GroupBy(x => new { x.OrderNumbels, x.OrderTime })
                    //(x.OrderTime))
                    .Select(g => new GroupByWLabelStorage()
                    {
                        OrderNumbels = g.Key.OrderNumbels,
                        Time = g.Key.OrderTime,
                        Count = g.Count(),
                    })
                    .OrderByDescending(o => o.Time);

                var resultList = await groupedData.Join(context.Agents,
                    t => t.OrderNumbels,
                    a => a.AID,
                    (t, a) => new GroupByWLabelStorage
                    {
                        OrderNumbels = t.OrderNumbels,
                        Time = t.Time,
                        Count = t.Count,
                        AgentName = a.AName,
                    }).ToListAsync();
                return resultList;
            }
        }

        public async Task<W_LabelStorage> GetWLabelStorageAsync(string qrcode)
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var result = await context.WLabelStorages.AsNoTracking()
                        // .Select(s =>new { s.OutTime,s.OrderNumbels,s.QRCode,s.Dealers})
                        .Where(w => w.QRCode == qrcode).FirstOrDefaultAsync();

                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"溯源信息查询异常:{e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 验证二维码标签能否参加活动或者参加扫码领现金红包，
        /// 标签出库时间没有超过24小时，不能参加活动
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AnyRedPacket(string qrcode)
        {
            DateTime now = DateTime.Now;
            using (var context = _contextFactory.CreateDbContext())
            {
                var outtime = await context.WLabelStorages.AsNoTracking()
                    .Where(w => w.QRCode.Trim() == qrcode.Trim())
                    .Select(s => s.OutTime)
                    .FirstOrDefaultAsync();
                outtime = outtime.AddDays(1);
                //判断两个时间的大小
                if (DateTime.Now.Date > outtime.Date)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 按年分组统计数量
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<YMDGroupByCount>> GetYearGroupByListAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var groupedData = await context.WLabelStorages.AsNoTracking()
                    .GroupBy(x => new { x.OrderTime.Year })
                    .Select(g => new YMDGroupByCount()
                    {
                        Year = g.Key.Year.ToString(),
                        Count = g.Count(),
                    })
                    .OrderBy(o => o.Year).ToListAsync();

                return groupedData;
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 按年月分组统计数量
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<object>> GetYearMGroupByListAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var groupedData = await context.WLabelStorages.AsNoTracking()
                    .GroupBy(x => new { x.OrderTime.Year, x.OrderTime.Month })
                    .Select(g => new YMDGroupByCount()
                    {
                        Year = g.Key.Year.ToString(),
                        Month = g.Key.Month.ToString(),
                        Count = g.Count(),
                    })
                    .OrderByDescending(o => o.Year).ToListAsync();
                return groupedData;
            }
        }

        /// <summary>
        /// 按年月分组统计数量
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<YMDGroupByCount>> GetYearMonthGroupByListAsync(int addmonths)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var twelveMonthsAgo = DateTime.Now.AddMonths(addmonths);
                var groupedData = await context.WLabelStorages.AsNoTracking()
                    .Where(w => w.OrderTime >= twelveMonthsAgo)
                    .GroupBy(x => new { x.OrderTime.Year, x.OrderTime.Month })
                    .Select(g => new YMDGroupByCount()
                    {
                        Year = g.Key.Year.ToString(),
                        Month = g.Key.Month.ToString(),
                        // OrderNumbels = g.Key.OrderNumbels,
                        Count = g.Count(),
                    })
                    .OrderByDescending(o => o.Year).ToListAsync();
                return groupedData;
            }
        }

        /// <summary>
        /// 按年月日分组统计出库数量
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<YMDGroupByCount>> GetYMDGroupByListAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var groupedData = await context.WLabelStorages.AsNoTracking()
                    .GroupBy(x => new { x.OrderTime.Year, x.OrderTime.Month, x.OrderTime.Day })
                    .Select(g => new YMDGroupByCount()
                    {
                        Year = g.Key.Year.ToString(),
                        Month = g.Key.Month.ToString(),
                        Day = g.Key.Day.ToString(),
                        Count = g.Count(),
                    })
                    .OrderByDescending(o => o.Year).ToListAsync();
                return groupedData;
            }
        }

        //按年月分组统计订单号的数量
        public async Task<IEnumerable<YMDGroupByCount>> GetYMOrderListAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var groupedData = await context.WLabelStorages.AsNoTracking()
                    .GroupBy(x => new { x.OrderTime.Year, x.OrderTime.Month, x.OrderNumbels })
                    .Select(g => new YMDGroupByCount()
                    {
                        Year = g.Key.Year.ToString(),
                        Month = g.Key.Month.ToString(),

                        OrderNumbels = g.Key.OrderNumbels,
                        Count = g.Count(),
                    })
                    .OrderByDescending(o => o.Year).ToListAsync();
                return groupedData;
            }
        }

        /// <summary>
        /// 返回当天的出库数量
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetTodayOutCountAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var groupedData = await context.WLabelStorages.AsNoTracking()
                    .Select(s => s.OutTime)
                    .Where(w => w.Date == DateTime.Now.Date)
                    .CountAsync();
                return groupedData;
            }
        }

        //返回当天订单数量
        public async Task<int> GetTodayOrderCountAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var groupedData = await context.WLabelStorages.AsNoTracking()
                    .Select(s => new { s.OrderTime, s.OrderNumbels })
                    .Where(w => w.OrderTime.Date == DateTime.Now.Date)
                    .GroupBy(g => g.OrderNumbels)
                    .CountAsync();
                return groupedData;
            }
        }

        /// <summary>
        /// 返回按月份与年份查询的二维码出库数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<string[]> GetMonthGroupByQRCodeListAsync(int year, int month)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var groupedData = await context.WLabelStorages.AsNoTracking()
                    .Where(w => w.OrderTime.Year == year && w.OrderTime.Month == month)
                    .Select(s => s.QRCode)
                    .ToArrayAsync();
                return groupedData;
            }
        }

        /// <summary>
        /// 返回当天时间的数据量
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> GetCount()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var groupedData = await context.WLabelStorages.AsNoTracking().Select(s => s.OutTime)
                    .Where(w => w.Date == DateTime.Now.Date)
                    //.Where(w => w.OrderTime.Year == DateTime.Now.Year && w.OrderTime.Month == DateTime.Now.Month && w.OrderTime.Day == DateTime.Now.Day)
                    .CountAsync();
                return groupedData;
            }
        }

        /// <summary>
        /// 返回今年扫码出库量
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> GetThisYearOutCountAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var groupedData = await context.WLabelStorages.AsNoTracking()
                    .Select(s => s.OutTime)
                    .Where(w => w.Year == DateTime.Now.Year)
                    .CountAsync();
                return groupedData;
            }

            //  throw new NotImplementedException();
        }

        /// <summary>
        /// 查询出库时间
        /// </summary>
        /// <returns></returns>
        public async Task<DateTime?> FindOutDateTime(string qrcode)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var groupedData = await context.WLabelStorages.AsNoTracking()
                    .Select(s => new { s.QRCode, s.OutTime })
                    .Where(w => w.QRCode == qrcode)
                    .OrderBy(o => o.OutTime)
                    .FirstOrDefaultAsync();
                if (groupedData != null) return groupedData.OutTime;
            }
            return default;
        }

        ///删除指定数据
        public async Task<bool> DeleteAsync(string qrcode)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var data = await context.WLabelStorages.AsNoTracking()
                    .Where(w => w.QRCode == qrcode)
                    .FirstOrDefaultAsync();
                if (data != null)
                {
                    context.WLabelStorages.Remove(data);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        //返回订单号的数量
        public async Task<int> GetOrderCountByDDNOAsync(string ddno)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.WLabelStorages.AsNoTracking()
            .Where(order => order.OrderNumbels == ddno)
            .CountAsync();
        }
    }
}