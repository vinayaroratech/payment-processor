using Payments.Application.Payments.Queries.GetPaymentsList;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Payments.API.IntegrationTests.Controllers.Payments
{
    public class GetAll : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly string _paymentBaseUri;
        public GetAll(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _paymentBaseUri = $"/api/v1/payments";
        }

        [Fact]
        public async Task ReturnsPaymentsListVm()
        {
            var client = await _factory.GetAuthenticatedClientAsync();

            var response = await client.GetAsync(_paymentBaseUri);

            response.EnsureSuccessStatusCode();

            var vm = await IntegrationTestHelper.GetResponseContent<PaymentsListVm>(response);

            vm.ShouldBeOfType<PaymentsListVm>();
            vm.Payments.Count.ShouldBeGreaterThan(0);
        }
    }
}