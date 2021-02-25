using MediatR;
using System;

namespace Payments.Application.Payments.Commands.ProcessPayment
{
    public class ProcessPaymentCommand : IRequest<long>
    {
        public string CreditCardNumber { get; set; }
        public string CardHolder { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public decimal Amount { get; set; }
    }
}
