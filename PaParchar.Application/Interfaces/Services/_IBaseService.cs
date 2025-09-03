using PaParchar.Utils.Results;
using System.Linq.Expressions;

namespace PaParchar.Application.Interfaces.Services
{
    public interface _IBaseService<T, ID>
    {
        Task<IResult<T>> CreateAsync(T entity);
        Task<IResult<T>> UpdateAsync(T entity);
        Task<IResult<T>> GetByIdAsync(ID id);
        Task<IResult<object>> DeleteAsync(ID id);
        Task<IResult<bool>> ExistsAsync(ID id);
        Task<IResult<IEnumerable<T>>> GetAllAsync();
        Task<IResult<IEnumerable<T>>> FindAsync(Expression<Func<T, bool>> predicate);

        // PROJECTION QUERIES
        Task<IResult<TDto>> GetProjectedByIdAsync<TDto>(ID id);
        Task<IResult<IEnumerable<TDto>>> GetProjectedAsync<TDto>(Expression<Func<T, bool>>? predicate = null);
        Task<IResult<(IEnumerable<TDto> Items, int TotalCount)>> GetProjectedPagedAsync<TDto>(int page, int pageSize, Expression<Func<T, bool>>? predicate = null);
        Task<IResult<IEnumerable<TDto>>> GetProjectedOrderedAsync<TDto, TKey>(Expression<Func<T, TKey>> orderBy,
        bool ascending = true,
        Expression<Func<T, bool>>? predicate = null);

        // PROJECTION COMMANDS
        Task<IResult<TShowDto>> CreateFromDto<TShowDto, TCreateDto>(TCreateDto createDto);

        Task<IResult<TShowDto>> UpdateFromDto<TShowDto, TUpdateDto>(ID entityId, TUpdateDto updateDto);
    }
}