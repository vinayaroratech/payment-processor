using MediatR;

namespace Payments.Application.Payments.Commands.DeletePayment
{
    public class DeletePaymentCommand : IRequest<long>
    {
        public long Id { get; set; }
    }
}
