using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScanCode.Model.Entity;
using ScanCode.Repository.Interface;

namespace ScanCode.Repository
{
    public class PrizeRepository : RepositoryBase<Prize>
    {
        private readonly IDbContextFactory<LotteryContext> _contextFactory;
        private readonly ILogger<Prize> _logger;
        private readonly IMediator _mediator;

        public PrizeRepository(IDbContextFactory<LotteryContext> context,
            ILogger<Prize> logger,
            IMediator mediator
           ) : base(context, mediator, logger)
        {
            _contextFactory = context;
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        ///  返回最新的前N条数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<List<Prize>> GetLatestRecordsAsync(int count = 0)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                //if (count == 0)
                //{
                //    return await context.Prizes.AsNoTracking().ToListAsync();
                //}
                return await context.Prizes.AsNoTracking()
                    //.OrderByDescending(x => x.CreateTime)
                    //.Take(count)
                    .ToListAsync();
            }
        }

        /// <summary>
        /// 分页返回奖品列表
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">返回多少条数据</param>
        /// <returns></returns>
        public async Task<List<Prize>> GetAllWithPaginationAsync(int pageIndex, int pageSize)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Prizes
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 根据主键删除数据
        /// </summary>
        /// <param name="entityid"></param>
        /// <returns></returns>
        public async Task<int> DeleteByIdAsync(int entityid)
        {
            using var context = _contextFactory.CreateDbContext();
            var prize = await context.Prizes.FindAsync(entityid);
            context.Prizes.Remove(prize);
            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// 返回所有参加活动的奖品
        /// </summary>
        /// <returns></returns>
        //public async Task<IEnumerable<Prize>> GetActivePrizes()
        //{
        //    using var context = _contextFactory.CreateDbContext();
        //    return await context.Prizes.AsNoTracking()
        //        .Where(w => w.IsActive == true).ToListAsync();
        //}

        /// <summary>
        /// 返回能参加活动的奖品列表，这个方法给属性Identifier赋值默认值
        /// </summary>
        /// <returns></returns>
        //public async Task<IEnumerable<Prize>> GetActivityPrizeAsync()
        //{
        //    using (var context = _contextFactory.CreateDbContext())
        //    {
        //        return await context.Prizes.AsNoTracking().Select(s => new Prize()
        //        {
        //             Id = s.Id,
        //            Name = s.Name,
        //            Identifier = s.Id.ToString(),
        //            IsJoinActivity = s.IsJoinActivity,
        //            IsActive = s.IsActive,
        //            CreateTime = s.CreateTime,
        //            LotteryActivityId = s.LotteryActivityId,
        //            LotteryActivity = s.LotteryActivity,
        //            Type = s.Type,
        //            Description = s.Description,
        //            ImageUrl = s.ImageUrl,
        //            Probability = s.Probability,

        //        }).ToListAsync();
        //    }
        //    }
        //}
        /// <summary>
        /// 返回参加指定活动的奖品
        /// </summary>
        /// <returns></returns>
        //public async Task<IEnumerable<Prize>> GetJoinActivity(int Activityid)
        //{
        //    using var context = _contextFactory.CreateDbContext();
        //    return await context.Prizes.AsNoTracking()
        //        .Where(w => w.LotteryActivityId == Activityid).ToListAsync();
        //}
    }
}