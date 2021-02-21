using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Payments.Domain.Entities;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        IDbConnection Connection { get; }
        bool HasChanges { get; }

        EntityEntry Entry(object entity);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DbSet<Payment> Payments { get; set; }
    }
}
