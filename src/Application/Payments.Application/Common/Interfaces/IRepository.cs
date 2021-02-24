using Microsoft.EntityFrameworkCore;
using Payments.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Common.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : BaseEntity, IAggregateRoot
    {
        DbSet<TEntity> Entity { get; }

        Task<TEntity> GetByIdAsync(Guid id);

        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<TEntity> GetByIdAsync(long id);

        Task<TEntity> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task<List<TEntity>> ListAsync();

        Task<List<TEntity>> ListAsync(CancellationToken cancellationToken);

        Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> expression);

        Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);

        Task AddRangeAsync(IEnumerable<TEntity> items);

        Task AddRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);

        Task UpdateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

        Task DeleteAsync(Guid id);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        Task DeleteRangeAsync(IEnumerable<long> ids);

        Task DeleteRangeAsync(IEnumerable<long> ids, CancellationToken cancellationToken);
    }
}
