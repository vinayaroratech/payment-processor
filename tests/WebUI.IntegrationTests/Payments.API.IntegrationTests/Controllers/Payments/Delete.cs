using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Payments.API.IntegrationTests.Controllers.Payments
{
    public class Delete : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly string _paymentBaseUri;
        public Delete(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _paymentBaseUri = $"/api/v1/payments";
        }

        [Fact]
        public async Task GivenValidId_ReturnsSuccessStatusCode()
        {
            var validId = await new Create(_factory).GivenValidCreatePaymentCommand_ReturnsSuccessCode();

            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.DeleteAsync($"{_paymentBaseUri}/{validId}");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GivenInvalidId_ReturnsNotFound()
        {
            var invalidId = 99;

            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.DeleteAsync($"{_paymentBaseUri}/{invalidId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}