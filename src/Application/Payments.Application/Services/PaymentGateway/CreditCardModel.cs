using System;

namespace Payments.Application.Services.PaymentGateway
{
    public class CreditCardModel : IPaymentModel
    {
        public string CreditCardNumber { get; set; }
        public string CardHolder { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public decimal Amount { get; set; }
    }
}
