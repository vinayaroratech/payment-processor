using Microsoft.AspNetCore.Identity;
using Payments.Domain.Entities;
using Payments.Infrastructure.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            if (!context.Payments.Any())
            {
                context.Payments.AddRange(
                        new Payment { Name = "Payment 1", IsComplete = true },
                        new Payment { Name = "Payment 2", IsComplete = true },
                        new Payment { Name = "Payment 3", IsComplete = true },
                        new Payment { Name = "Payment 4" },
                        new Payment { Name = "Payment 5" },
                        new Payment { Name = "Payment 6" },
                        new Payment { Name = "Payment 7" },
                        new Payment { Name = "Payment 8" }
                    );

                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser { UserName = "vinay@arora", Email = "vinay@arora" };

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, "Administrator1!");
            }
        }
    }
}