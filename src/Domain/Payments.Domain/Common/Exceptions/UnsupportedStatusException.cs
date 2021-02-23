using System;

namespace Payments.Domain.Common.Exceptions
{
    public class UnsupportedStatusException : Exception
    {
        public UnsupportedStatusException(int code)
            : base($"Status \"{code}\" is unsupported.")
        {
        }
    }
}