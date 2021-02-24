using Payments.Domain.Entities;
using Payments.Infrastructure.Persistence;

namespace Payments.Infrastructure.Data.Repositories
{
    public class EfPaymentRepository : EfRepository<Payment, ApplicationDbContext>
    {
        public EfPaymentRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
