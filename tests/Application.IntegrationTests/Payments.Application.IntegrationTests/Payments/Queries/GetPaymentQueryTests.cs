using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Exceptions;
using Payments.Application.IntegrationTests.NUnitTests;
using Payments.Application.Payments.Commands.CreatePayment;
using Payments.Application.Payments.Queries.GetPayment;
using System;
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
                CardHolder = "Do yet another thing for Get By id.",
                Amount = 100,
                CreditCardNumber = "1234567812345678",
                ExpirationDate = DateTime.Now.AddYears(1),
                SecurityCode = "123"
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