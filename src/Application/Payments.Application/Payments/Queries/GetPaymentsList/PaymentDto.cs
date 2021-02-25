using Payments.Application.Common.Mappings;
using Payments.Domain.Entities;
using System;

namespace Payments.Application.Payments.Queries.GetPaymentsList
{
    public class PaymentDto : IMapFrom<Payment>
    {
        public string CreditCardNumber { get; set; }
        public string CardHolder { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public decimal Amount { get; set; }

        public bool IsComplete { get; set; }

        public string Status { get; set; }
    }
}