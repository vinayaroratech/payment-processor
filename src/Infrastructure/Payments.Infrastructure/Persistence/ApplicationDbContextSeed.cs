using Microsoft.AspNetCore.Identity;
using Payments.Application.Common.Interfaces;
using Payments.Domain.Entities;
using Payments.Domain.ValueObjects;
using Payments.Infrastructure.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(IApplicationDbContext context)
        {
            if (!context.Payments.Any())
            {
                context.Payments.AddRange(
                        new Payment { Name = "Payment 1", IsComplete = true },
                        new Payment { Name = "Payment 2", IsComplete = true },
                        new Payment { Name = "Payment 3", IsComplete = true },
                        new Payment { Name = "Payment 4", Status = Status.Completed },
                        new Payment { Name = "Payment 5", Status = Status.OnHold },
                        new Payment { Name = "Payment 6", Status = Status.Rejected },
                        new Payment { Name = "Payment 7" },
                        new Payment { Name = "Payment 8" }
                    );

                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var administratorRole = new IdentityRole("Administrator");

            if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await roleManager.CreateAsync(administratorRole);
            }

            var administrator = new ApplicationUser { UserName = "vinay@arora", Email = "vinay@arora" };

            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await userManager.CreateAsync(administrator, "Administrator1!");
                await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }
    }
}