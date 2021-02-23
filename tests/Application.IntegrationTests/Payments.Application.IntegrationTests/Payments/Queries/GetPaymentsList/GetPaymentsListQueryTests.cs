using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Payments.Queries.GetPaymentsList;
using System.Threading.Tasks;

namespace Payments.Application.IntegrationTests.Payments.Queries.GetPaymentsList
{
    using static Testing;
    public class GetPaymentsListQueryTests
    {
        [Test]
        public async Task ShouldGetAllPayments()
        {
            var result = await SendAsync(new GetPaymentsListQuery());
            result.Should().BeOfType<PaymentsListVm>();
            result.Payments.Count.Should().BeGreaterThan(0);
        }
    }
}