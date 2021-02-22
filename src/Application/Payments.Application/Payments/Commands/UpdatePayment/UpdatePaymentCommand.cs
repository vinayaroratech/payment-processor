using MediatR;

namespace Payments.Application.Payments.Commands.UpdatePayment
{
    public class UpdatePaymentCommand : IRequest<long>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsComplete { get; set; }
    }
}
