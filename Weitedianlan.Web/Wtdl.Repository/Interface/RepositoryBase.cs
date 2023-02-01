using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Wtdl.Repository.Interface
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        private IDbContextFactory<LotteryContext> _contextFactory;
        private readonly ILogger<T> _logger;
        private readonly IMediator _mediator;

        public RepositoryBase(IDbContextFactory<LotteryContext> dbContextFactory,
            IMediator Imediator,
            ILogger<T> logger
            )
        {
            _contextFactory = dbContextFactory;
            _logger = logger;
            _mediator = Imediator;
        }

        public virtual async Task<int> AddAsync(T entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                await context.Set<T>().AddAsync(entity);

                return await context.SaveChangesAsync();
            }
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            using var context = _contextFactory.CreateDbContext();
            //  await context.Set<T>().AnyAsync(entity);
            context.Set<T>().Remove(entity);
            await _mediator.Publish(entity);
            return await context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteExpressionAsync(Expression<Func<T, bool>> expression)
        {
            using var context = _contextFactory.CreateDbContext();
            var result = await context.Set<T>().Where(expression).ToListAsync();
            if (result is not null && result.Count > 0)
            {
                context.Set<T>().RemoveRange(result);
                return await context.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }

        public virtual async Task<bool> ExistAsync(Expression<Func<T, bool>> expression)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().AnyAsync(expression);
            }
        }

        /// <summary>
        /// 返回匹配的所有数据列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
            }

            //   return await context.FileUploadRecords.AsNoTracking().Where(predicate).ToListAsync();
        }

        ///// <summary>
        ///// 返回最新的一条数据
        ///// </summary>
        ///// <param name="predicate"></param>
        ///// <returns></returns>
        //public virtual async Task<T> LastAsync(Expression<Func<T, bool>> expression)
        //{
        //    using (var context = _contextFactory.CreateDbContext())
        //    {
        //        return await context.Set<T>().AsNoTracking().OrderByDescending(expression).FirstOrDefaultAsync();
        //    }
        //}

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().AsNoTracking().ToListAsync();
            }
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().FindAsync(id);
            }
        }

        public virtual async Task<int> UpdateAsync(T entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                context.Set<T>().Update(entity);

                return await context.SaveChangesAsync();
            }
        }
    }
}