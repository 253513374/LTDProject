namespace Wtdl.Repository.Interface
{
    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
    {
        //Task<T> GetByIdAsync(int id);

        //Task<IEnumerable<T>> GetAllAsync();

        //Task<int> AddAsync(T entity);

        //Task<int> UpdateAsync(T entity);

        //Task<int> DeleteAsync(T entity);

        //Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}