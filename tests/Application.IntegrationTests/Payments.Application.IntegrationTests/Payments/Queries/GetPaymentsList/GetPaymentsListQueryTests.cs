using NUnit.Framework;
using Payments.Application.Payments.Queries.GetPaymentsList;
using Shouldly;
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
            result.ShouldBeOfType<PaymentsListVm>();
            result.Payments.Count.ShouldBe(4);
        }
    }
}