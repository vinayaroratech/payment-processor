using MediatR;

namespace Payments.Application.Payments.Queries.GetPayment
{
    public class GetPaymentQuery : IRequest<PaymentVm>
    {
        public long Id { get; set; }
    }
}