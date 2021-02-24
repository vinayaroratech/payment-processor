using Microsoft.EntityFrameworkCore;
using Payments.Application.Common.Interfaces;
using Payments.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Data.Repositories
{
    public abstract class EfRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : BaseEntity, IAggregateRoot
        where TContext : IApplicationDbContext
    {
        bool disposing;
        private readonly DbContext _dbContext;

        public DbSet<TEntity> Entity => _dbContext.Set<TEntity>();

        public EfRepository(TContext dbContext)
        {
            _dbContext = dbContext.DbContext;
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await Entity.FindAsync(id);
        }

        public virtual Task<TEntity> GetByIdAsync(long id)
        {
            return Entity.SingleOrDefaultAsync(e => e.Id == id);
        }

        public virtual Task<List<TEntity>> ListAsync()
        {
            return Entity.ToListAsync();
        }

        public virtual async Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> expression)
        {
            return await Entity.AsNoTracking().Where(expression).ToListAsync();
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> items)
        {
            await Entity.AddRangeAsync(items);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await Entity.AddAsync(entity);
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
            Entity.Remove(entity);
            return _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var item = await Entity.FindAsync(id);
            if (item != null)
            {
                Entity.Remove(item);
            }
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<long> ids)
        {
            var items = await Entity.Where(e => ids.Contains(e.Id)).ToListAsync();
            if (items != null && items.Any())
            {
                Entity.RemoveRange(items);
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
