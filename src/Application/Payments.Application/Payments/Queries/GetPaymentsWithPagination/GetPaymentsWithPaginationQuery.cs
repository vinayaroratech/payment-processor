using MediatR;
using Payments.Application.Common.Models;
using Payments.Application.Payments.Queries.GetPaymentsList;

namespace Payments.Application.Payments.Queries.GetPaymentsWithPagination
{
    public class GetPaymentsWithPaginationQuery : PaginationQuery, IRequest<PaginationResponse<PaymentDto>>
    {
        public string SearchText { get; set; }
    }
}