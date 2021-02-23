using FluentAssertions;
using Payments.Application.Payments.Commands.UpdatePayment;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Payments.API.IntegrationTests.Controllers.Payments
{
    public class Update : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly string _paymentBaseUri;

        public Update(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _paymentBaseUri = $"/api/v1/payments";
        }

        [Fact]
        public async Task GivenValidUpdatePaymentCommand_ReturnsSuccessCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var validId = await new Create(_factory).GivenValidCreatePaymentCommand_ReturnsSuccessCode();
            var command = new UpdatePaymentCommand
            {
                Id = Convert.ToInt64(validId),
                Name = $"Do this thing - {DateTime.Now.Ticks}.",
                IsComplete = true
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"{_paymentBaseUri}/{command.Id}", content);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GivenValidUpdatePaymentCommand_ReturnsBadRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var validId = await new Create(_factory).GivenValidCreatePaymentCommand_ReturnsSuccessCode();
            var command = new UpdatePaymentCommand
            {
                Id = Convert.ToInt64(validId),
                Name = "Do this thing.",
                IsComplete = true
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PutAsync($"{_paymentBaseUri}/{command.Id}", content);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}