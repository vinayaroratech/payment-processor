using MediatR;
using System;

namespace Payments.Application.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommand : IRequest<long>
    {
        public string CreditCardNumber { get; set; }
        public string CardHolder { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public decimal Amount { get; set; }

        public bool IsComplete { get; set; }
    }
}
