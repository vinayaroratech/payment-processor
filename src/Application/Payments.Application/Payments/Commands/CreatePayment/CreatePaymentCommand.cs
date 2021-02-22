using MediatR;

namespace Payments.Application.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommand : IRequest<long>
    {
        public string Name { get; set; }
    }
}
