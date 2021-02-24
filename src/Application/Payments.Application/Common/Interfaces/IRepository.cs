using Payments.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Payments.Application.Common.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity: BaseEntity, IAggregateRoot
    {
        Task<TEntity> GetByIdAsync(Guid id);

        Task<TEntity> GetByIdAsync(long id);

        Task<List<TEntity>> ListAsync();

        Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> expression);

        Task AddRangeAsync(IEnumerable<TEntity> items);

        Task<TEntity> AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(Guid id);

        Task DeleteRangeAsync(IEnumerable<long> ids);
    }
}
