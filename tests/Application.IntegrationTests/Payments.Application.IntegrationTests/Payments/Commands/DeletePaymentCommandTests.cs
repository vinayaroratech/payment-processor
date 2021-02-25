using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Exceptions;
using Payments.Application.IntegrationTests.NUnitTests;
using Payments.Application.Payments.Commands.CreatePayment;
using Payments.Application.Payments.Commands.DeletePayment;
using Payments.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Payments.Application.IntegrationTests.Payments.Commands
{
    using static Testing;
    public class DeletePaymentCommandTests : TestBase
    {
        [Test]
        public async Task ShouldDeletePayment()
        {
            var command = new CreatePaymentCommand
            {
                CardHolder = "Do yet another thing for delete.",
                Amount = 100,
                CreditCardNumber = "1234567812345678",
                ExpirationDate = DateTime.Now.AddYears(1),
                SecurityCode = "123"
            };

            var paymentId = await SendAsync(command);

            await SendAsync(new DeletePaymentCommand { Id = paymentId });

            var payment = await FindAsync<Payment>(paymentId);

            payment.Should().BeNull();
        }

        [Test]
        public void ShouldRequireValidPaymentId()
        {
            var command = new DeletePaymentCommand { Id = 99 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();

        }
    }
}