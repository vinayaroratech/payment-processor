using Payments.Domain.Common;
using Payments.Domain.Entities;

namespace Payments.Domain.Events
{
    public class PaymentCreatedEvent : DomainEvent
    {
        public PaymentCreatedEvent(Payment payment)
        {
            Payment = payment;
        }

        public Payment Payment { get; }

    }
}
