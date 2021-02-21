using System;

namespace Payments.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        public string UserId { get; }
    }
}
