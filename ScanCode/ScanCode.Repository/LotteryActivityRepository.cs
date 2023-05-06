using EFCore.BulkExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScanCode.Model.Entity;
using ScanCode.Repository.Interface;

namespace ScanCode.Repository
{
    /// <summary>
    /// 抽奖活动数据仓储
    /// </summary>
    public class LotteryActivityRepository : RepositoryBase<LotteryActivity>
    {
        private readonly IDbContextFactory<LotteryContext> _contextFactory;
        private readonly ILogger<LotteryActivity> _logger;
        private readonly IMediator _mediator;

        private readonly PrizeRepository _prizeRepository;
        //private IWebHostEnvironment _env;

        public LotteryActivityRepository(IDbContextFactory<LotteryContext> context,
            IMediator mediator,
            ILogger<LotteryActivity> logger,
            PrizeRepository prizeRepository) : base(context, mediator, logger)
        {
            _contextFactory = context;
            _mediator = mediator;
            _logger = logger;
            _prizeRepository = prizeRepository;
        }

        /// <summary>
        /// 返回最新的前N条数据，默认100条数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LotteryActivity>> GetLatestRecordsAsync(int count = 100)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.LotteryActivities.AsNoTracking()
                .OrderByDescending(x => x.Id)
                // .Include(c => c.Prizes)
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// 返回当前激活的唯一活动信息
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<LotteryActivity> GetLotteryActivityAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.LotteryActivities.AsNoTracking()
                .Where(w => w.IsActive)
                .Include(c => c.Prizes.Where(w => w.IsActive == true))

                .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateActiveStatus(LotteryActivity activity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                //批量更新IsActive状态
                var update = await context.LotteryActivities.Where(w => w.IsActive)
                    .BatchUpdateAsync(u => new LotteryActivity { IsActive = false });

                if (update >= 0)
                {
                    //更新
                    context.LotteryActivities.Update(activity);
                    return await context.SaveChangesAsync();
                }

                return update;
                //   context.LotteryActivities.BatchUpdateAsync(u => u.IsActive == true);
            }
        }
    }
}