using System.Linq.Expressions;

namespace ScanCode.Repository.Interface
{
    public interface IReadRepository<T>
    {
        /// <summary>
        /// 根据主键id返回数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        ///  根据where条件返回数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<bool> ExistAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 根据查询条件，返回数据库中最新一条数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        //Task<T> LastAsync(Expression<Func<T, bool>> expression);
    }
}