using System;
using System.Collections.Generic;

namespace Payments.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public string CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
