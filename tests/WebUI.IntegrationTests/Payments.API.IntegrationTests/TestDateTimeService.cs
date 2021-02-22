using Payments.Application.Common.Interfaces;
using System;

namespace Payments.API.IntegrationTests
{
    public class TestDateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}