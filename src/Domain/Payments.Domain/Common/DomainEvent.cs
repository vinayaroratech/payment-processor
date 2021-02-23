using System;

namespace Payments.Domain.Common
{
    public abstract class DomainEvent
    {
        public bool IsPublished { get; set; }
        public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}
