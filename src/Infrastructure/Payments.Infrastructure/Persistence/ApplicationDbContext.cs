using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Payments.Application.Common.Interfaces;
using Payments.Domain.Common;
using Payments.Domain.Entities;
using Payments.Infrastructure.Identity;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Persistence
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        public ApplicationDbContext(
             DbContextOptions options,
             IOptions<OperationalStoreOptions> operationalStoreOptions,
             ICurrentUserService currentUserService,
             IDateTime dateTime) : base(options, operationalStoreOptions)
        {
            {
                _currentUserService = currentUserService;
                _dateTime = dateTime;
            }
        }

        public DbSet<Payment> Payments { get; set; }

        public IDbConnection Connection => throw new System.NotImplementedException();

        public bool HasChanges => throw new System.NotImplementedException();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly); 
            base.OnModelCreating(builder);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}