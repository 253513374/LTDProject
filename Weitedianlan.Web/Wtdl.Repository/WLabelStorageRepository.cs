using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Internal;
using StackExchange.Redis;
using Weitedianlan.Model.Entity;
using Wtdl.Repository.Data;
using Wtdl.Repository.Interface;
using System.Linq;
using EFCore.BulkExtensions;
using Weitedianlan.Model.Entity.Analysis;

namespace Wtdl.Repository
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
                    .Where(x => x.OrderTime >= startTime && x.OrderTime <= endTime)
                    .OrderByDescending(o => o.OrderTime)
                    .GroupBy(x => new { x.OrderNumbels, x.OrderTime })
                    //(x.OrderTime))
                    .Select(g => new GroupByWLabelStorage()
                    {
                        OrderNumbels = g.Key.OrderNumbels,
                        Time = g.Key.OrderTime,
                        Count = g.Count(),
                    });

                var resultList = await groupedData.Join(context.Agents,
                        t => t.OrderNumbels,
                        a => a.AID,
                        (t, a) => new GroupByWLabelStorage
                        {
                            OrderNumbels = t.OrderNumbels,
                            Time = t.Time,
                            Count = t.Count,
                            AgentName = a.AName,
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

                    resultList = await groupedDataold.Join(context.Agents,
                            t => t.ID,
                            a => a.AID,
                            (t, a) => new GroupByWLabelStorage
                            {
                                OrderNumbels = t.OrderNumbels,
                                Time = t.Time,
                                Count = t.Count,
                                AgentName = a.AName,
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
                    var latestData = await context.WLabelStorages.AsNoTracking().OrderByDescending(x => x.ID).FirstOrDefaultAsync();
                    var thirtyDaysAgo = latestData.OutTime.AddDays(-30);// DateTime.Now.AddDays(-30);

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

                    context.BulkInsert(graupbylist);
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
                    return await context.WLabelStorages.AsNoTracking()
                        .Where(w => w.QRCode == qrcode).FirstOrDefaultAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"GetWLabelStorageAsync:{e.Message}");
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
                outtime = outtime.AddHours(24);
                //判断两个时间的大小
                if (DateTime.Compare(now, outtime) > 0)
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
        public async Task<IEnumerable<YMDGroupByCount>> GetYearMonthGroupByListAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var groupedData = await context.WLabelStorages.AsNoTracking()
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

        //返回当天的出库数量
        public async Task<int> GetTodayOutCountAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var groupedData = await context.WLabelStorages.AsNoTracking()
                    .Where(w => w.OrderTime.Year == DateTime.Now.Year && w.OrderTime.Month == DateTime.Now.Month && w.OrderTime.Day == DateTime.Now.Day)
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
                    .Where(w => w.OrderTime.Year == DateTime.Now.Year && w.OrderTime.Month == DateTime.Now.Month && w.OrderTime.Day == DateTime.Now.Day)
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
        public int GetCount()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var groupedData = context.WLabelStorages.AsNoTracking()
                    .Where(w => w.OrderTime.Year == DateTime.Now.Year && w.OrderTime.Month == DateTime.Now.Month && w.OrderTime.Day == DateTime.Now.Day)
                    .Count();
                return groupedData;
            }
        }

        /// <summary>
        /// 返回统计最新一年数量
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> GetCurrentYearCountAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var groupedData = await context.WLabelStorages.AsNoTracking()
                    .Where(w => w.OrderTime.Year == DateTime.Now.Year)
                    .CountAsync();
                return groupedData;
            }

            //  throw new NotImplementedException();
        }
    }
}