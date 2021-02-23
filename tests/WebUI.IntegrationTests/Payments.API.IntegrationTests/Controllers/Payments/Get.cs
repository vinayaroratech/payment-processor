using FluentAssertions;
using Payments.Application.Payments.Queries.GetPayment;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Payments.API.IntegrationTests.Controllers.Payments
{
    public class Get : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly string _paymentBaseUri;
        public Get(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _paymentBaseUri = $"/api/v1/payments";
        }

        [Fact]
        public async Task GivenValidId_ReturnsPaymentVm()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var validId = await new Create(_factory).GivenValidCreatePaymentCommand_ReturnsSuccessCode();

            var response = await client.GetAsync($"{_paymentBaseUri}/{validId}");

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<PaymentVm>(response);

            vm.Should().BeOfType<PaymentVm>();
            vm.Id.Should().Be(Convert.ToInt64(validId));
        }

        [Fact]
        public async Task GivenInvalidId_ReturnsNotFound()
        {
            var client = await _factory.GetAuthenticatedClientAsync();
            var invalidId = 99;

            var response = await client.GetAsync($"{_paymentBaseUri}/{invalidId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}