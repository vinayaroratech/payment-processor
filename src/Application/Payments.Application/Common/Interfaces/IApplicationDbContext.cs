using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Payments.Domain.Entities;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Common.Interfaces
{
    public interface IApplicationDbContext : IDisposable
    {
        DbContext DbContext { get; }
        DatabaseFacade Database { get; }
        IDbConnection Connection { get; }
        bool HasChanges { get; }

        EntityEntry Entry(object entity);

        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DbSet<Payment> Payments { get; set; }
    }
}
