using Payments.Application.Common.Interfaces;
using Payments.Application.Common.Models;
using System;
using System.Threading.Tasks;

namespace Payments.API.IntegrationTests
{
    public class TestIdentityService : IIdentityService
    {
        public Task<string> GetUserNameAsync(string userId)
        {
            return Task.FromResult("vinay@arora");
        }

        public Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}