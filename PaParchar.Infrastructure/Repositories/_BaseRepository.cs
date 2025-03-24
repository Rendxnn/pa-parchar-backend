using Microsoft.EntityFrameworkCore;
using PaParchar.Application.Interfaces.Repositories;
using PaParchar.Domain.Entities;
using System.Linq.Expressions;

namespace PaParchar.Infrastructure.Repositories
{
    public class _BaseRepository<T, ID> : _IBaseRepository<T, ID> where T : _BaseEntity<ID> where ID : notnull
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public _BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(ID id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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
    }
}
