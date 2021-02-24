using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Exceptions;
using Payments.Application.IntegrationTests.NUnitTests;
using Payments.Application.Payments.Commands.CreatePayment;
using Payments.Application.Payments.Queries.GetPayment;
using System.Threading.Tasks;

namespace Payments.Application.IntegrationTests.Payments.Queries
{
    using static Testing;

    public class GetPaymentQueryTests: TestBase
    {
        [Test]
        public async Task ShouldGetPaymentById()
        {
            var paymentId = await SendAsync(new CreatePaymentCommand
            {
                Name = "Do yet another thing for Get By id."
            });

            var query = new GetPaymentQuery
            {
                Id = paymentId
            };

            var result = await SendAsync(query);

            result.Should().BeOfType<PaymentVm>();
            result.Id.Should().Be(paymentId);
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