using FluentAssertions;
using Payments.Application.Payments.Commands.ProcessPayment;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Payments.API.IntegrationTests.Controllers.Payments
{
    public class Process : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly string _paymentBaseUri;

        public Process(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _paymentBaseUri = $"/api/v1/payments/process";
        }

        [Fact]
        public async Task GivenValidProcessPaymentCommand_ReturnsSuccessCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new ProcessPaymentCommand
            {
                CardHolder = $"Vinay - {System.DateTime.Now.Ticks}.",
                Amount = 1234,
                CreditCardNumber = "1234567812345678",
                ExpirationDate = DateTime.Now.AddMonths(1),
                SecurityCode = "123",
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync(_paymentBaseUri, content);

            response.EnsureSuccessStatusCode();
            var id = await response.Content.ReadAsStringAsync();
            id.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GivenInvalidProcessPaymentCommand_ReturnsBadRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new ProcessPaymentCommand
            {
                CardHolder = "This description of this thing will exceed the maximum length. This description of this thing will exceed the maximum length. This description of this thing will exceed the maximum length. This description of this thing will exceed the maximum length."
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync(_paymentBaseUri, content);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenNegitiveAmountProcessPaymentCommand_ReturnsBadRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new ProcessPaymentCommand
            {
                Amount = -1003
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync(_paymentBaseUri, content);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}