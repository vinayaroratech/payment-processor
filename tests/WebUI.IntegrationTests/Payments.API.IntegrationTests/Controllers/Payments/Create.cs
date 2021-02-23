using FluentAssertions;
using Payments.Application.Payments.Commands.CreatePayment;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Payments.API.IntegrationTests.Controllers.Payments
{
    public class Create : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly string _paymentBaseUri;

        public Create(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _paymentBaseUri = $"/api/v1/payments";
        }

        [Fact]
        public async Task<string> GivenValidCreatePaymentCommand_ReturnsSuccessCode()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new CreatePaymentCommand
            {
                Name = $"Do yet another thing - {System.DateTime.Now.Ticks}."
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync(_paymentBaseUri, content);

            response.EnsureSuccessStatusCode();
            var id = await response.Content.ReadAsStringAsync();
            id.Should().NotBeNullOrEmpty();
            return id;
        }

        [Fact]
        public async Task GivenInvalidCreatePaymentCommand_ReturnsBadRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new CreatePaymentCommand
            {
                Name = "This description of this thing will exceed the maximum length. This description of this thing will exceed the maximum length. This description of this thing will exceed the maximum length. This description of this thing will exceed the maximum length."
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync(_paymentBaseUri, content);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenDuplicateCreatePaymentCommand_ReturnsBadRequest()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var command = new CreatePaymentCommand
            {
                Name = "Do this thing."
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync(_paymentBaseUri, content);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}