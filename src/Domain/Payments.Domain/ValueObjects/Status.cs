using Payments.Domain.Common;
using Payments.Domain.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Payments.Domain.ValueObjects
{
    public class Status : ValueObject
    {
        static Status()
        {
        }

        private Status()
        {
        }

        private Status(int code)
        {
            Code = code;
        }

        public static Status From(int code)
        {
            var Status = new Status { Code = code };

            if (!SupportedStatuss.Contains(Status))
            {
                throw new UnsupportedStatusException(code);
            }

            return Status;
        }

        public static Status Pending => new Status(0);

        public static Status Accepted => new Status(1);

        public static Status InProcess => new Status(2);

        public static Status Completed => new Status(3);

        public static Status OnHold => new Status(4);

        public static Status Rejected => new Status(5);

        public int Code { get; private set; }

        public static implicit operator string(Status Status)
        {
            return Status.ToString();
        }

        public static explicit operator Status(int code)
        {
            return From(code);
        }

        public override string ToString()
        {
            return Code.ToString();
        }

        protected static IEnumerable<Status> SupportedStatuss
        {
            get
            {
                yield return Pending;
                yield return Accepted;
                yield return InProcess;
                yield return Completed;
                yield return OnHold;
                yield return Rejected;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}