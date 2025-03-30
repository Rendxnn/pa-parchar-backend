using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PaParchar.Application.Interfaces.Repositories;
using PaParchar.Domain.Entities;
using System.Linq.Expressions;
using PaParchar.Infrastructure.Configuration.Contexts;

namespace PaParchar.Infrastructure.Repositories
{
    public class _BaseRepository<T, ID> : _IBaseRepository<T, ID> where T : _BaseEntity<ID> where ID : notnull
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly IMapper _mapper;

        public _BaseRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(ID id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T?> GetByIdWithIncludeAsync(ID id, string includeProperties)
        {
            IQueryable<T> query = _dbSet;
            
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            
            return await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public virtual DbContext GetContext()
        {
            return _context;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(ID id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<bool> ExistsAsync(ID id)
        {
            return await _dbSet.AnyAsync(e => e.Id.Equals(id));
        }

        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Consulta proyectada a un DTO específico para mejorar la eficiencia.
        /// Solo selecciona los campos necesarios para el DTO.
        /// </summary>
        /// <typeparam name="TDto">Tipo del DTO al que se proyectará la entidad</typeparam>
        /// <param name="predicate">Expresión de filtro opcional</param>
        /// <returns>Colección de DTOs proyectados</returns>
        public virtual async Task<IEnumerable<TDto>> GetProjectedAsync<TDto>(Expression<Func<T, bool>>? predicate = null)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);

            return await query
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un DTO proyectado por ID
        /// </summary>
        /// <typeparam name="TDto">Tipo del DTO</typeparam>
        /// <param name="id">ID de la entidad</param>
        /// <returns>DTO proyectado</returns>
        public virtual async Task<TDto?> GetProjectedByIdAsync<TDto>(ID id)
        {
            return await _dbSet
                .Where(e => e.Id.Equals(id))
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Consulta proyectada con paginación
        /// </summary>
        /// <typeparam name="TDto">Tipo del DTO</typeparam>
        /// <param name="page">Número de página (comienza en 1)</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <param name="predicate">Expresión de filtro opcional</param>
        /// <returns>Colección paginada de DTOs</returns>
        public virtual async Task<(IEnumerable<TDto> Items, int TotalCount)> GetProjectedPagedAsync<TDto>(
            int page,
            int pageSize,
            Expression<Func<T, bool>>? predicate = null)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return (items, totalCount);
        }

        /// <summary>
        /// Consulta proyectada con ordenamiento
        /// </summary>
        /// <typeparam name="TDto">Tipo del DTO</typeparam>
        /// <typeparam name="TKey">Tipo de la propiedad por la que se ordenará</typeparam>
        /// <param name="orderBy">Expresión de ordenamiento</param>
        /// <param name="ascending">Verdadero para ordenar ascendente, falso para descendente</param>
        /// <param name="predicate">Expresión de filtro opcional</param>
        /// <returns>Colección ordenada de DTOs</returns>
        public virtual async Task<IEnumerable<TDto>> GetProjectedOrderedAsync<TDto, TKey>(
            Expression<Func<T, TKey>> orderBy,
            bool ascending = true,
            Expression<Func<T, bool>>? predicate = null)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);

            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            return await query
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}