using PaParchar.Utils.Results;
using System.Linq.Expressions;

namespace PaParchar.Application.Interfaces.Services
{
    public interface _IBaseService<T, ID>
    {
        Task<IResult<T>> DeleteAsync(ID id);
        Task<IResult<T>> GetByIdAsync(ID id);
        Task<IResult<T>> CreateAsync(T entity);
        Task<IResult<T>> UpdateAsync(T entity);
        Task<IResult<bool>> ExistsAsync(ID id);
        Task<IResult<IEnumerable<T>>> GetAllAsync();
        Task<IResult<IEnumerable<T>>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<IResult<TDto?>> GetProjectedByIdAsync<TDto>(ID id);
        Task<IResult<IEnumerable<TDto>>> GetProjectedAsync<TDto>(Expression<Func<T, bool>>? predicate = null);
        Task<IResult<(IEnumerable<TDto> Items, int TotalCount)>> GetProjectedPagedAsync<TDto>(int page, int pageSize, 
        Expression<Func<T, bool>>? predicate = null);
        Task<IResult<IEnumerable<TDto>>> GetProjectedOrderedAsync<TDto, TKey>(Expression<Func<T, TKey>> orderBy, 
        bool ascending = true, 
        Expression<Func<T, bool>>? predicate = null);
    }
}