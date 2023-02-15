using EFCore.BulkExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weitedianlan.Model.Entity;
using Wtdl.Repository.Interface;
using Wtdl.Repository.MediatRHandler.Events;
using Wtdl.Repository.Utility;

namespace Wtdl.Repository
{
    public class ActivityPrizeRepository : RepositoryBase<ActivityPrize>
    {
        private readonly IDbContextFactory<LotteryContext> _contextFactory;
        private readonly ILogger<ActivityPrize> _logger;
        private readonly IMediator _mediator;

        public ActivityPrizeRepository(IDbContextFactory<LotteryContext> context,
            IMediator mediator,
            ILogger<ActivityPrize> logger) : base(context, mediator, logger)
        {
            _contextFactory = context;
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// 采用乐观锁并发控制。保证更新数据的一致性
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateActivityPrizeAsync(ActivityPrize entity)
        {
            using var context = _contextFactory.CreateDbContext();

            try
            {
                context.ActivityPrizes.Update(entity);
                var re = await context.SaveChangesAsync();
                if (re > 0)
                {
                    await _mediator.Publish(new LoggerEvent()
                    {
                        TypeData = entity.GetType(),
                        CreateTime = DateTime.Now,
                        OperationType = OperationType.Update,
                        JsonData = GlobalUtility.SerializeObject(entity)
                    });
                    return re;
                }
                return 0;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (ActivityPrize)entry.Entity;
                var databaseValues = (ActivityPrize)entry.GetDatabaseValues().ToObject();

                if (databaseValues.Unredeemed != clientValues.Unredeemed)
                {
                    // 解决并发冲突
                    clientValues.Unredeemed = databaseValues.Unredeemed;
                    return await context.SaveChangesAsync();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}