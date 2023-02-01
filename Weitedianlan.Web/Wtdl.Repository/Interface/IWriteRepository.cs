using System.Linq.Expressions;

namespace Wtdl.Repository.Interface
{
    public interface IWriteRepository<T>
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> AddAsync(T entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(T entity);

        /// <summary>
        /// 根据实体对象删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(T entity);

        /// <summary>
        /// 根据条件批量删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> DeleteExpressionAsync(Expression<Func<T, bool>> expression);
    }
}