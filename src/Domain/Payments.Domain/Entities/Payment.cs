using Payments.Domain.Common;

namespace Payments.Domain.Entities
{
    public class Payment: AuditableEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsComplete { get; set; }
    }
}
