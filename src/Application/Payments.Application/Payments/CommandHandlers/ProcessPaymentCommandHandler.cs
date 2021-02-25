using MediatR;
using Payments.Application.Payments.Commands.ProcessPayment;
using Payments.Application.Services.PaymentGateway;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Payments.CommandHandlers
{
    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, long>
    {
        private readonly IPaymentStrategy _paymentStrategy;

        public ProcessPaymentCommandHandler(IPaymentStrategy paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }

        public async Task<long> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            long paymentId = await _paymentStrategy.MakePaymentAsync(new CreditCardModel()
            {
                Amount = request.Amount,
                CardHolder = request.CardHolder,
                CreditCardNumber = request.CreditCardNumber,
                ExpirationDate = request.ExpirationDate,
                SecurityCode = request.SecurityCode
            });

            return paymentId;
        }
    }
}