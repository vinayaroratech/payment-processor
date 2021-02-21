using Payments.Application.Common.Interfaces;
using System;

namespace Payments.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
