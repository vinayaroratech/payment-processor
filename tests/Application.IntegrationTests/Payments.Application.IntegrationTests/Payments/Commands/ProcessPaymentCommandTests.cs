using FluentAssertions;
using NUnit.Framework;
using Payments.Application.Common.Exceptions;
using Payments.Application.IntegrationTests.NUnitTests;
using Payments.Application.Payments.Commands.ProcessPayment;
using System;
using System.Threading.Tasks;

namespace Payments.Application.IntegrationTests.Payments.Commands
{
    using static Testing;
    public class ProcessPaymentCommandTests : TestBase
    {
        [Test]
        public async Task ShouldRequireValidCardNumberThrowValidationError()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new ProcessPaymentCommand()
            {

                Amount = 1234,
                CardHolder = "Vinay Arora",
                CreditCardNumber = "1234567890",
                ExpirationDate = DateTime.Now.AddMonths(1),
                SecurityCode = "",
            };


            var exception = FluentActions.Invoking(() => SendAsync(command))
                      .Should().Throw<ValidationException>();

            exception.Where(ex => ex.Errors.Count == 1);
            exception.Where(ex => ex.Errors.ContainsKey("CreditCardNumber"))
                    .And.Errors["CreditCardNumber"].Should().Contain("Credit Card Number must be 16 digits.");
            exception.Where(ex => !ex.Errors.ContainsKey("SecurityCode"));
        }

        [Test]
        public async Task ShouldRequireValidAmountThrowValidationError()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new ProcessPaymentCommand()
            {

                Amount = -1234,
                CardHolder = "Vinay Arora",
                CreditCardNumber = "1234567891234567",
                ExpirationDate = DateTime.Now.AddMonths(1),
                SecurityCode = "",
            };


            var exception = FluentActions.Invoking(() => SendAsync(command))
                      .Should().Throw<ValidationException>();

            exception.Where(ex => ex.Errors.Count == 1);
            exception.Where(ex => !ex.Errors.ContainsKey("SecurityCode"));
            exception.Where(ex => ex.Errors.ContainsKey("Amount"))
                     .And.Errors["Amount"].Should().Contain("Amount must be greater than zero.");
        }

        [Test]
        public async Task ShouldRequireValidExpirationThrowValidationError()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new ProcessPaymentCommand()
            {

                Amount = 1234,
                CardHolder = "Vinay Arora",
                CreditCardNumber = "1234567891234567",
                ExpirationDate = DateTime.Now.AddMonths(-1),
                SecurityCode = "",
            };


            var exception = FluentActions.Invoking(() => SendAsync(command))
                      .Should().Throw<ValidationException>();

            exception.Where(ex => ex.Errors.Count == 1);
            exception.Where(ex => !ex.Errors.ContainsKey("SecurityCode"));
            exception.Where(ex => ex.Errors.ContainsKey("ExpirationDate"))
                     .And.Errors["ExpirationDate"].Should().Contain("Expiration Date can not be in past.");
        }

        [Test]
        public async Task ShouldProcessPayment()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new ProcessPaymentCommand()
            {
                Amount = 1234,
                CardHolder = "Vinay Arora",
                CreditCardNumber = "1234567891234567",
                ExpirationDate = DateTime.Now,
                SecurityCode = "",
            };


            var paymentId = await SendAsync(command);
        }

        [Test]
        public async Task ShouldNotProcessAndThrowValidationError()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new ProcessPaymentCommand();

            var exception = FluentActions.Invoking(() => SendAsync(command))
                      .Should().Throw<ValidationException>();

            exception.Where(ex => ex.Errors.Count == 4);
            exception.Where(ex => ex.Errors.ContainsKey("CreditCardNumber"))
                     .And.Errors["CreditCardNumber"].Should().Contain("Credit Card Number is required.");
            exception.Where(ex => ex.Errors.ContainsKey("CardHolder"))
                     .And.Errors["CardHolder"].Should().Contain("Card Holder is required.");
            exception.Where(ex => ex.Errors.ContainsKey("ExpirationDate"))
                     .And.Errors["ExpirationDate"].Should().Contain("Expiration Date is required.");
            exception.Where(ex => ex.Errors.ContainsKey("ExpirationDate"))
                     .And.Errors["ExpirationDate"].Should().Contain("Expiration Date can not be in past.");
            exception.Where(ex => !ex.Errors.ContainsKey("SecurityCode"));
            exception.Where(ex => ex.Errors.ContainsKey("Amount"))
                     .And.Errors["Amount"].Should().Contain("Amount is required.");
            exception.Where(ex => ex.Errors.ContainsKey("Amount"))
                     .And.Errors["Amount"].Should().Contain("Amount must be greater than zero.");

        }

        [Test]
        public async Task ShouldRequireValidSecurityCodeThrowValidationError()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new ProcessPaymentCommand()
            {

                Amount = 1234,
                CardHolder = "Vinay Arora",
                CreditCardNumber = "1234567891234567",
                ExpirationDate = DateTime.Now,
                SecurityCode = "1",
            };


            var exception = FluentActions.Invoking(() => SendAsync(command))
                      .Should().Throw<ValidationException>();

            exception.Where(ex => ex.Errors.Count == 1);
            exception.Where(ex => ex.Errors.ContainsKey("SecurityCode"))
                     .And.Errors["SecurityCode"].Should().Contain("Security Code must be 3 characters.");
        }
    }
}