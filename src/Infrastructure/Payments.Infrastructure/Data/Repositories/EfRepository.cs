using Microsoft.EntityFrameworkCore;
using Payments.Application.Common.Interfaces;
using Payments.Domain.Common;
using Payments.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Data.Repositories
{
    public abstract class EfRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : BaseEntity, IAggregateRoot
        where TContext : DbContext
    {
        bool disposing;
        private readonly DbContext _dbContext;

        public EfRepository(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual Task<TEntity> GetByIdAsync(long id)
        {
            return _dbContext.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == id);
        }

        public virtual Task<List<TEntity>> ListAsync()
        {
            return _dbContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbContext.Set<TEntity>().AsNoTracking().Where(expression).ToListAsync();
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> items)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(items);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return _dbContext.SaveChangesAsync();
        }

        public virtual Task DeleteAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            return _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var item = await _dbContext.Set<TEntity>().FindAsync(id);
            if (item != null)
            {
                _dbContext.Set<TEntity>().Remove(item);
            }
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<long> ids)
        {
            var items = await _dbContext.Set<TEntity>().Where(e => ids.Contains(e.Id)).ToListAsync();
            if (items != null && items.Any())
            {
                _dbContext.Set<TEntity>().RemoveRange(items);
            }
        }

        /// <summary>
        /// Dispose the repository and inner components (dbContext)
        /// </summary>
        public virtual void Dispose()
        {
            if (!disposing)
            {
                _dbContext.Dispose();
                disposing = true;
            }
        }
    }
}
