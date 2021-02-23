using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Exceptions;
using Payments.Application.IntegrationTests.NUnitTests;
using Payments.Application.Payments.Commands.CreatePayment;
using Payments.Application.Payments.Commands.UpdatePayment;
using Payments.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Payments.Application.IntegrationTests.Payments.Commands.UpdatePayment
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
                Name = "Do yet another thing for update."
            });

            var command = new UpdatePaymentCommand
            {
                Id = paymentId,
                Name = "This thing is also done.",
                IsComplete = true
            };

            await SendAsync(command);

            var payment = await FindAsync<Payment>(command.Id);
            payment.Should().NotBeNull();
            payment.Name.Should().Be(command.Name);
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
                Name = "This item doesn't exist.",
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
                Name = "New List"
            });

            await SendAsync(new CreatePaymentCommand
            {
                Name = "This thing is also done"
            });

            var command = new UpdatePaymentCommand
            {
                Id = paymentId,
                Name = "This thing is also done"
            };

            FluentActions.Invoking(() =>
                SendAsync(command))
                    .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Name"))
                    .And.Errors["Name"].Should().Contain("The specified name is already exists.");
        }
    }
}