using Payments.Application.Common.Interfaces;

namespace Payments.API.IntegrationTests
{
    public class TestCurrentUserService : ICurrentUserService
    {
        public string UserId => "f26da293-02fb-4c90-be75-e4aa51e0bb17";
    }
}