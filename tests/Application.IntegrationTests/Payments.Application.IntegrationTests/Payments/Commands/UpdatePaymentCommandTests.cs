using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Exceptions;
using Payments.Application.IntegrationTests.NUnitTests;
using Payments.Application.Payments.Commands.CreatePayment;
using Payments.Application.Payments.Commands.UpdatePayment;
using Payments.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Payments.Application.IntegrationTests.Payments.Commands
{
    using static Testing;
    public class UpdatePaymentCommandTests : TestBase
    {
        [Test]
        public async Task ShouldUpdatePersistedPayment()
        {
            var userId = await RunAsDefaultUserAsync();

            var paymentId = await SendAsync(new CreatePaymentCommand
            {
                CardHolder = "Do yet another thing for update.",
                Amount = 100,
                CreditCardNumber = "1234567812345678",
                ExpirationDate = DateTime.Now.AddYears(1),
                SecurityCode = "123"
            });

            var command = new UpdatePaymentCommand
            {
                Id = paymentId,
                CardHolder = "This thing is also done.",
                Amount = 100,
                CreditCardNumber = "1234567812345678",
                ExpirationDate = DateTime.Now.AddYears(1),
                SecurityCode = "123",
                IsComplete = true
            };

            await SendAsync(command);

            var payment = await FindAsync<Payment>(command.Id);
            payment.Should().NotBeNull();
            payment.CardHolder.Should().Be(command.CardHolder);
            payment.IsComplete.Should().BeTrue();
            payment.LastModifiedBy.Should().NotBeNull();
            payment.LastModifiedBy.Should().Be(userId);
            payment.LastModified.Should().NotBeNull();
            payment.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
        }

        [Test]
        public async Task ShouldRequireValidPaymentId()
        {
            var userId = await RunAsDefaultUserAsync();
            var command = new UpdatePaymentCommand
            {
                Id = 99,
                CardHolder = "This item doesn't exist.",
                IsComplete = false
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldRequireUniqueTitle()
        {
            var paymentId = await SendAsync(new CreatePaymentCommand
            {
                CardHolder = "New List",
                Amount = 100,
                CreditCardNumber = "1234567812345678",
                ExpirationDate = DateTime.Now.AddYears(1),
                SecurityCode = "123"
            });

            await SendAsync(new CreatePaymentCommand
            {
                CardHolder = "This thing is also done",
                Amount = 100,
                CreditCardNumber = "1234567812345678",
                ExpirationDate = DateTime.Now.AddYears(1),
                SecurityCode = "123"
            });

            var command = new UpdatePaymentCommand
            {
                Id = paymentId,
                CardHolder = "This thing is also done",
                Amount = 200,
                CreditCardNumber = "1234567812345678",
                ExpirationDate = DateTime.Now.AddYears(1),
                SecurityCode = "123"
            };

            FluentActions.Invoking(() =>
                SendAsync(command))
                    .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Name"))
                    .And.Errors["Name"].Should().Contain("The specified name is already exists.");
        }
    }
}