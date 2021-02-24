using Microsoft.EntityFrameworkCore;
using Payments.Application.Common.Interfaces;
using Payments.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
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
            return await GetByIdAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Entity.FindAsync(id, cancellationToken).ConfigureAwait(false);
        }

        public virtual Task<TEntity> GetByIdAsync(long id)
        {
            return GetByIdAsync(id, default);
        }

        public virtual Task<TEntity> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return Entity.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public virtual Task<List<TEntity>> ListAsync()
        {
            return ListAsync(default);
        }

        public virtual Task<List<TEntity>> ListAsync(CancellationToken cancellationToken)
        {
            return Entity.ToListAsync(cancellationToken);
        }

        public virtual async Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> expression)
        {
            return await Find(expression, default).ConfigureAwait(false);
        }

        public virtual async Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        {
            return await Entity.AsNoTracking().Where(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> items)
        {
            await AddRangeAsync(items, default).ConfigureAwait(false);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
        {
            await Entity.AddRangeAsync(items, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            return await AddAsync(entity, default).ConfigureAwait(false);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await Entity.AddAsync(entity);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return entity;
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            return UpdateAsync(entity, default);
        }

        public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual Task DeleteAsync(TEntity entity)
        {
            return DeleteAsync(entity, default);
        }

        public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            Entity.Remove(entity);
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await DeleteAsync(id, default);
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var item = await Entity.FindAsync(id, cancellationToken).ConfigureAwait(false);
            if (item != null)
            {
                Entity.Remove(item);
            }
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<long> ids)
        {
            await DeleteRangeAsync(ids);
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<long> ids, CancellationToken cancellationToken)
        {
            var items = await Entity.Where(e => ids.Contains(e.Id))
                .ToListAsync(cancellationToken).ConfigureAwait(false);
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
