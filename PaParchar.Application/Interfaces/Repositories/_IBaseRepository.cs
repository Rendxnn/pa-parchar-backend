using System.Linq.Expressions;

namespace PaParchar.Application.Interfaces.Repositories
{
    public interface _IBaseRepository<T, ID>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(ID id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(ID id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(ID id);
        Task SaveChangesAsync();


        Task<TDto?> GetProjectedByIdAsync<TDto>(ID id);
        Task<IEnumerable<TDto>> GetProjectedAsync<TDto>(Expression<Func<T, bool>>? predicate = null);
        Task<(IEnumerable<TDto> Items, int TotalCount)> GetProjectedPagedAsync<TDto>(int page, int pageSize, Expression<Func<T, bool>>? predicate = null);
        Task<IEnumerable<TDto>> GetProjectedOrderedAsync<TDto, TKey>(Expression<Func<T, TKey>> orderBy, bool ascending = true, Expression<Func<T, bool>>? predicate = null);
    }
}
