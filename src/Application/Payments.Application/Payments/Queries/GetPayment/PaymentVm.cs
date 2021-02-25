using Payments.Application.Common.Mappings;
using Payments.Domain.Entities;
using System;

namespace Payments.Application.Payments.Queries.GetPayment
{
    public class PaymentVm : IMapFrom<Payment>
    {
        public long Id { get; set; }
        public string CreditCardNumber { get; set; }
        public string CardHolder { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public decimal Amount { get; set; }
        public bool IsComplete { get; set; }

    }
}