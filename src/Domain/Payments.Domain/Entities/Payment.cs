using Payments.Domain.Common;
using Payments.Domain.Events;
using Payments.Domain.ValueObjects;
using System.Collections.Generic;

namespace Payments.Domain.Entities
{
    public class Payment : AuditableEntity, IHasDomainEvent
    {
        private bool _done;

        public Payment()
        {
            DomainEvents = new List<DomainEvent>();
        }
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsComplete { get; set; }
        public List<DomainEvent> DomainEvents { get; set; }
        public bool Done
        {
            get => _done;
            set
            {
                if (value == true && _done == false)
                {
                    DomainEvents.Add(new PaymentCompletedEvent(this));
                }
                _done = value;
            }
        }

        public Status Status { get; set; } = Status.Pending;

    }
}
