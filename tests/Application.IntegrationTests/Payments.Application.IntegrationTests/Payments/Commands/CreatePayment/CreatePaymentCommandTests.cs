using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Exceptions;
using Payments.Application.IntegrationTests.NUnitTests;
using Payments.Application.Payments.Commands.CreatePayment;
using Payments.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Payments.Application.IntegrationTests.Payments.Commands.CreatePayment
{
    using static Testing;
    public class CreatePaymentCommandTests : TestBase
    {
        [Test]
        public async Task ShouldCreatePayment()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new CreatePaymentCommand
            {
                Name = "Do yet another thing."
            };

            var paymentId = await SendAsync(command);

            var payment = await FindAsync<Payment>(paymentId);

            payment.Should().NotBeNull();
            payment.IsComplete.Should().BeFalse();
            payment.Should().NotBeNull();
            payment.Id.Should().Be(paymentId);
            payment.Name.Should().Be(command.Name);
            payment.CreatedBy.Should().Be(userId);
            payment.Created.Should().BeCloseTo(DateTime.Now, 10000);
            payment.LastModifiedBy.Should().BeNull();
            payment.LastModified.Should().BeNull();
        }

        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreatePaymentCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}