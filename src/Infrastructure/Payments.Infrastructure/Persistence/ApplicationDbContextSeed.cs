using Microsoft.AspNetCore.Identity;
using Payments.Infrastructure.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser { UserName = "vinay@arora", Email = "vinay@arora" };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                await userManager.CreateAsync(defaultUser, "Administrator1!");
            }
        }
    }
}