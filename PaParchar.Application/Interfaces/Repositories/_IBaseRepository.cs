using System.Linq.Expressions;

namespace PaParchar.Application.Interfaces.Repositories
{
    public interface _IBaseRepository<T, ID>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(ID id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(ID id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(ID id);
        Task SaveChangesAsync();
    }
}
