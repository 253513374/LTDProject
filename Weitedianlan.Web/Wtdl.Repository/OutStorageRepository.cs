using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Weitedianlan.Model.Entity;
using Weitedianlan.Model.Entity.Analysis;
using Wtdl.Repository.Interface;
using Wtdl.Repository.MediatRHandler.Events;
using Wtdl.Repository.Utility;

namespace Wtdl.Repository
{
    public class OutStorageRepository : RepositoryBase<OutStorageAnalysis>
    {
        private readonly IDbContextFactory<LotteryContext> _contextFactory;
        private readonly ILogger<OutStorageAnalysis> _logger;
        private readonly IMediator _mediator;

        //public OutStorageRepository(IDbContextFactory<LotteryContext> dbContextFactory, IMediator Imediator, ILogger<ActivityPrize> logger) : base(dbContextFactory, Imediator, logger)
        //{
        //}
        public OutStorageRepository(IDbContextFactory<LotteryContext> dbContextFactory, IMediator Imediator, ILogger<OutStorageAnalysis> logger) : base(dbContextFactory, Imediator, logger)
        {
            _contextFactory = dbContextFactory;
            _logger = logger;
            _mediator = Imediator;
        }

        //批量插入数据
        public async Task<int> BatchInsertOutStorageAsync(List<OutStorageAnalysis> outStorageAnalysis)
        {
            using var context = _contextFactory.CreateDbContext();
            try
            {
                await context.BulkInsertAsync(outStorageAnalysis);
                var re = await context.SaveChangesAsync();
                if (re > 0)
                {
                    await _mediator.Publish(new LoggerEvent()
                    {
                        TypeData = outStorageAnalysis.GetType(),
                        CreateTime = DateTime.Now,
                        OperationType = OperationType.Insert,
                        JsonData = GlobalUtility.SerializeObject(outStorageAnalysis)
                    });
                    return re;
                }
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return 0;
            }
        }

        //返回每年下单数量
        public async Task<List<OutStorageAnalysis>> GetGrapByYearAndOrderAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.OutStorageAnalyses.AsNoTracking()
                    .GroupBy(g => g.Year)
                    .Select(s => new OutStorageAnalysis()
                    {
                        Year = s.Key,
                        Count = s.Count(),
                    }).OrderBy(o => o.Year).ToListAsync();
            }
        }

        //返回每年扫码销量
        public async Task<List<OutStorageAnalysis>> GetGrapByYearAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.OutStorageAnalyses.AsNoTracking()
                    .GroupBy(g => new { g.Year, g.OrderNumbels })
                    .Select(s => new OutStorageAnalysis()
                    {
                        Year = s.Key.Year,
                        Count = s.Sum(s => s.Count),
                    }).ToListAsync();
            }
        }
    }
}