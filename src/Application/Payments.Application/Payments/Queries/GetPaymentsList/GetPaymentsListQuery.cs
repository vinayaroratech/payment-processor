using MediatR;

namespace Payments.Application.Payments.Queries.GetPaymentsList
{
    public class GetPaymentsListQuery : IRequest<PaymentsListVm>
    {
        public long Id { get; set; }
    }
}