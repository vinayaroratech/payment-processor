using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Common.Interfaces;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) { return string.Empty; }
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }
    }
}
