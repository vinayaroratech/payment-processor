using Payments.Application.Common.Interfaces;
using System.Threading.Tasks;

namespace Payments.API.IntegrationTests
{
    public class TestIdentityService : IIdentityService
    {
        public Task<string> GetUserNameAsync(string userId)
        {
            return Task.FromResult("vinay@arora");
        }
    }
}