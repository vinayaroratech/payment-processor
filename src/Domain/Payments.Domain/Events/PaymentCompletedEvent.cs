using Payments.Domain.Common;
using Payments.Domain.Entities;

namespace Payments.Domain.Events
{
    public class PaymentCompletedEvent : DomainEvent
    {
        public PaymentCompletedEvent(Payment payment)
        {
            Payment = payment;
        }

        public Payment Payment { get; }

    }
}
