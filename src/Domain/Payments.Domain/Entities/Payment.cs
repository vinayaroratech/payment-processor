using Payments.Domain.Common;
using Payments.Domain.Events;
using Payments.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Payments.Domain.Entities
{
    public class Payment : AuditableEntity, IHasDomainEvent, IAggregateRoot
    {
        public string CreditCardNumber { get; set; }
        public string CardHolder { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public decimal Amount { get; set; }

        public bool IsComplete { get; set; }
        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();

        public bool IsDone { get; private set; }

        public void MarkComplete()
        {
            if (IsDone == false)
            {
                DomainEvents.Add(new PaymentCompletedEvent(this));
            }

            IsDone = true;
        }

        public Status Status { get; set; } = Status.Pending;

        public override string ToString()
        {
            string status = IsDone ? "Done!" : "Not done.";
            return $"{Id}: Status: {status} - {CardHolder}";
        }
    }
}
