using System.Linq.Expressions;
using EFCore.BulkExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Weitedianlan.Model.Entity;
using Weitedianlan.Model.Enum;
using Wtdl.Repository.Interface;
using Wtdl.Repository.MediatRHandler.Events;
using Wtdl.Repository.Utility;

namespace Wtdl.Repository
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
                .Include(c => c.Prizes)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<LotteryActivity>> GetAllWithPaginationAsync(int pageIndex, int pageSize)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.LotteryActivities.AsNoTracking()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 根据状态返回活动列表
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LotteryActivity>> GetActivityStatusAsync(ActivityStatus status)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.LotteryActivities
                .Where(x => x.Status == status)
                .ToListAsync();
        }

        /// <summary>
        /// 更新活动状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<bool> UpdateActivityStatusAsync(int id, ActivityStatus status)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.LotteryActivities.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return false;
            }

            entity.Status = status;
            context.LotteryActivities.Update(entity);
            var updateint = await context.SaveChangesAsync();
            if (updateint > 0)
            {
                await _mediator.Publish(new LoggerEvent()
                {
                    TypeData = entity.GetType(),
                    JsonData = GlobalUtility.SerializeObject(entity),
                    OperationType = OperationType.Insert
                });
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 返回指定活动信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<LotteryActivity> GetLotteryActivityAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.LotteryActivities
                .Where(x => x.Id == id)
                .Include(x => x.Prizes)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 返回符合查询条件的活动信息
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LotteryActivity>> GetAllLotteryActivityAsync(Expression<Func<LotteryActivity, bool>> expression)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.LotteryActivities.AsNoTracking()
                .Where(expression)
                .Include(x => x.Prizes)
                .ToListAsync();
        }

        /// <summary>
        /// 返回符合条件的一条信息
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<LotteryActivity> GetLotteryActivityAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.LotteryActivities.AsNoTracking()
                .Where(w => w.IsActive)
                .Include(c => c.Prizes)
                .OrderByDescending(o => o.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<LotteryActivity> GetLotteryActivityWithPrizesAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.LotteryActivities
                .Where(x => x.Id == id)
                .Include(x => x.Prizes)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<LotteryActivity>> GetLotteryActivityWithPrizesAsync(Expression<Func<LotteryActivity, bool>> expression)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.LotteryActivities
                .Where(expression)
                .Include(x => x.Prizes)
                .ToListAsync();
        }
    }
}