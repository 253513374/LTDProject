using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Weitedianlan.Model.Entity;
using Wtdl.Repository.Data;
using Wtdl.Repository.Interface;

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

        public async Task<List<W_LabelStorage>> GetLatestRecordsAsync(int count)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.WLabelStorages
                    .OrderByDescending(x => x.OrderTime)
                    .Take(count)
                    .ToListAsync();
            }
        }

        public async Task<List<GroupByWLabelStorage>> GetLatest30DaysGroupByOrderTimeAsync()
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var latestData = await context.WLabelStorages.AsNoTracking().OrderByDescending(x => x.ID).FirstOrDefaultAsync();

                    var thirtyDaysAgo = latestData.OutTime.AddDays(-30);// DateTime.Now.AddDays(-30);

                    var groupedData = context.WLabelStorages
                        .Where(x => x.OrderTime > thirtyDaysAgo)
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

                    return resultList;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new List<GroupByWLabelStorage>();
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
    }
}