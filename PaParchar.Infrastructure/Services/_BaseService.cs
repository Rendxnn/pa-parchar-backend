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

        public _BaseService(_IBaseRepository<T, ID> repository)
        {
            _repository = repository;
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

        public virtual async Task<IResult<T>> DeleteAsync(ID id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return Result<T>.NotFound($"No se encontró el registro con Id: {id}");

                await _repository.DeleteAsync(id);
                await _repository.SaveChangesAsync();

                return Result<T>.Success(entity, "Registro eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Error al eliminar el registro con Id: {id}", ex.Message);
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
    }
}
