using AutoMapper;
using PaParchar.Application.Interfaces.Repositories;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Domain.Entities;
using PaParchar.Utils.Results;
using System.Linq.Expressions;

namespace PaParchar.Infrastructure.Services
{
    public class _BaseService<T, ID> : _IBaseService<T, ID>
        where T : _BaseEntity<ID>
        where ID : notnull
    {
        protected readonly _IBaseRepository<T, ID> _repository;
        protected readonly IMapper _mapper;

        public _BaseService(_IBaseRepository<T, ID> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<IResult<IEnumerable<T>>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();

                if (entities == null || !entities.Any())
                    return Result<IEnumerable<T>>.NotFound("No se encontraron registros.");

                return Result<IEnumerable<T>>.Success(entities);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<T>>.Failure("Error al obtener los registros.", ex.Message);
            }
        }

        public virtual async Task<IResult<T>> GetByIdAsync(ID id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);

                if (entity == null)
                    return Result<T>.NotFound($"No se encontró el registro con Id: {id}");

                return Result<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Error al obtener el registro con Id: {id}", ex.Message);
            }
        }

        public virtual async Task<IResult<T>> CreateAsync(T entity)
        {
            try
            {
                var result = await _repository.AddAsync(entity);
                await _repository.SaveChangesAsync();

                return Result<T>.Success(result, "Registro creado exitosamente.");
            }
            catch (Exception ex)
            {
                return Result<T>.Failure("Error al crear el registro.", ex.Message);
            }
        }

        public virtual async Task<IResult<T>> UpdateAsync(T entity)
        {
            try
            {
                var exists = await _repository.ExistsAsync(entity.Id);
                if (!exists)
                    return Result<T>.NotFound($"No se encontró el registro con Id: {entity.Id}");

                await _repository.UpdateAsync(entity);
                await _repository.SaveChangesAsync();

                return Result<T>.Success(entity, "Registro actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Error al actualizar el registro con Id: {entity.Id}", ex.Message);
            }
        }

        public virtual async Task<IResult<object>> DeleteAsync(ID id)
        {
            try
            {
                T? entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return Result<object>.NotFound($"No se encontró el registro con Id: {id}");

                await _repository.DeleteAsync(id);
                await _repository.SaveChangesAsync();

                return Result<object>.Success();
            }
            catch (Exception ex)
            {
                return Result<object>.Failure($"Error al eliminar el registro con Id: {id}", ex.Message);
            }
        }

        public virtual async Task<IResult<IEnumerable<T>>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entities = await _repository.FindAsync(predicate);

                if (entities == null || !entities.Any())
                    return Result<IEnumerable<T>>.NotFound("No se encontraron registros con los criterios especificados.");

                return Result<IEnumerable<T>>.Success(entities);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<T>>.Failure("Error al buscar registros.", ex.Message);
            }
        }

        public virtual async Task<IResult<bool>> ExistsAsync(ID id)
        {
            try
            {
                var exists = await _repository.ExistsAsync(id);

                return Result<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al verificar la existencia del registro con Id: {id}", ex.Message);
            }
        }

        public virtual async Task<IResult<TDto>> GetProjectedByIdAsync<TDto>(ID id)
        {
            try
            {
                TDto? result = await this._repository.GetProjectedByIdAsync<TDto>(id);

                if (result == null) return Result<TDto>.NotFound($"No se ha encontrado el registro con id: {id}");

                return Result<TDto>.Success(result);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return Result<TDto>.Failure($"Error obteniendo el registro con id: {id}");
            }
        }

        public virtual async Task<IResult<IEnumerable<TDto>>> GetProjectedAsync<TDto>(Expression<Func<T, bool>>? predicate = null)
        {
            try
            {
                IEnumerable<TDto> result = await _repository.GetProjectedAsync<TDto>(predicate);
                
                if (result == null || !result.Any())
                    return Result<IEnumerable<TDto>>.NotFound("No se encontraron registros con los criterios especificados.");
                
                return Result<IEnumerable<TDto>>.Success(result);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return Result<IEnumerable<TDto>>.Failure("Error obteniendo los registros proyectados", ex.Message);
            }
        }

        public virtual async Task<IResult<(IEnumerable<TDto> Items, int TotalCount)>> GetProjectedPagedAsync<TDto>(int page, int pageSize, Expression<Func<T, bool>>? predicate = null)
        {
            try
            {
                (IEnumerable<TDto> Items, int TotalCount) result = await _repository.GetProjectedPagedAsync<TDto>(page, pageSize, predicate);
                
                if (result.Items == null || !result.Items.Any())
                    return Result<(IEnumerable<TDto> Items, int TotalCount)>.Failure("No se encontraron registros con los criterios especificados.");

                return Result<(IEnumerable<TDto> Items, int TotalCount)>.Failure(result);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return Result<(IEnumerable<TDto> Items, int TotalCount)>.Failure("Error obteniendo los registros paginados", ex.Message);
            }
        }

        public virtual async Task<IResult<IEnumerable<TDto>>> GetProjectedOrderedAsync<TDto, TKey>(Expression<Func<T, TKey>> orderBy, bool ascending = true, Expression<Func<T, bool>>? predicate = null)
        {
            try
            {
                IEnumerable<TDto> result = await _repository.GetProjectedOrderedAsync<TDto, TKey>(orderBy, ascending, predicate);
                
                if (result == null || !result.Any())
                    return Result<IEnumerable<TDto>>.NotFound("No se encontraron registros con los criterios especificados.");
                
                return Result<IEnumerable<TDto>>.Success(result);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return Result<IEnumerable<TDto>>.Failure("Error obteniendo los registros ordenados", ex.Message);
            }
        }

        public virtual async Task<IResult<TShowDto>> CreateFromDto<TShowDto, TCreateDto>(TCreateDto createDto)
        {
            try
            {
                T entity = _mapper.Map<T>(createDto);

                T createdEntity = await _repository.AddAsync(entity);
                await _repository.SaveChangesAsync();

                TShowDto responseDto = _mapper.Map<TShowDto>(createdEntity);
                return Result<TShowDto>.Success(responseDto);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return Result<TShowDto>.Failure("Error creando registro");
            }
        }

        public virtual async Task<IResult<TShowDto>> UpdateFromDto<TShowDto, TUpdateDto>(ID entityId, TUpdateDto updateDto)
        {
            try
            {
                IResult<T> entityResult = await GetByIdAsync(entityId);
                if (!entityResult.Successful.GetValueOrDefault() || entityResult.Data == null) return Result<TShowDto>.Failure("Error actualizando registro");
                T entity = entityResult.Data;

                _mapper.Map(updateDto, entity);

                await _repository.UpdateAsync(entity);

                await _repository.SaveChangesAsync();

                TShowDto responseDto = _mapper.Map<TShowDto>(entity);
                return Result<TShowDto>.Success(responseDto);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return Result<TShowDto>.Failure("Error creando registro");
            }
        }
    }
}
