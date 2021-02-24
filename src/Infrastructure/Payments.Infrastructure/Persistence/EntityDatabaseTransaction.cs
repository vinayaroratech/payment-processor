using Microsoft.EntityFrameworkCore.Storage;
using Payments.Application.Common.Interfaces;

namespace Payments.Infrastructure.Persistence
{
    /// <summary>
    /// Database transaction conrete implementation
    /// </summary>
    public class EntityDatabaseTransaction : IDatabaseTransaction
    {
        private IDbContextTransaction _transaction;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Database context to apply transaction to</param>
        public EntityDatabaseTransaction(IApplicationDbContext context)
        {
            _transaction = context.Database.BeginTransaction();
        }

        /// <summary>
        /// Commit dbConext changes done in transaction
        /// </summary>
        public void Commit()
        {
            _transaction.Commit();
        }

        /// <summary>
        /// Rollback dbConext changes done in transaction
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
        }

        /// <summary>
        /// Closes and disposes current transaction
        /// </summary>
        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}
