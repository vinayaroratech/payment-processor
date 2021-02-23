using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Exceptions;
using Payments.Application.IntegrationTests.NUnitTests;
using Payments.Application.Payments.Queries.GetPayment;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Payments.Application.IntegrationTests.Payments.Queries.GetPayment
{
    using static Testing;

    public class GetPaymentQueryTests: TestBase
    {
        [Test]
        public async Task ShouldGetPaymentById()
        {
            var query = new GetPaymentQuery
            {
                Id = 1
            };

            var result = await SendAsync(query);

            result.ShouldBeOfType<PaymentVm>();
            result.Id.ShouldBe(1);
        }

        [Test]
        public void Handle_GivenInvalidId_ThrowsException()
        {
            var query = new GetPaymentQuery
            {
                Id = 99
            };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().Throw<NotFoundException>();
        }
    }
}