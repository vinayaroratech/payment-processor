using Payments.Application.Common.Models;
using Payments.Application.Payments.Queries.GetPaymentsList;
using Payments.Application.Payments.Queries.GetPaymentsWithPagination;
using Payments.Domain.Entities;
using System.Threading.Tasks;

namespace Payments.Application.Common.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<PaginationResponse<PaymentDto>> GetPaymentsWithPaginationQuery(GetPaymentsWithPaginationQuery request);
    }
}
