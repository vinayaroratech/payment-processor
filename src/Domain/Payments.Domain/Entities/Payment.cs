using System;

namespace Payments.Domain.Entities
{
    public class Payment
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsComplete { get; set; }
    }
}
