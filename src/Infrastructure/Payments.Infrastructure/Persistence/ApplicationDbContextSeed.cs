using Microsoft.AspNetCore.Identity;
using Payments.Application.Common.Interfaces;
using Payments.Domain.Entities;
using Payments.Domain.ValueObjects;
using Payments.Infrastructure.Identity;
using System;
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
                    new Payment { CardHolder = "Payment 1", Amount = 10, CreditCardNumber = "1234567812345678", SecurityCode = "123", ExpirationDate = DateTime.Now.AddYears(1), IsComplete = true },
                    new Payment { CardHolder = "Payment 2", Amount = 10, CreditCardNumber = "1234567812345678", SecurityCode = "123", ExpirationDate = DateTime.Now.AddYears(1), IsComplete = true },
                    new Payment { CardHolder = "Payment 3", Amount = 10, CreditCardNumber = "1234567812345678", SecurityCode = "123", ExpirationDate = DateTime.Now.AddYears(1), IsComplete = true },
                    new Payment { CardHolder = "Payment 4", Amount = 10, CreditCardNumber = "1234567812345678", SecurityCode = "123", ExpirationDate = DateTime.Now.AddYears(1), Status = Status.Completed },
                    new Payment { CardHolder = "Payment 5", Amount = 10, CreditCardNumber = "1234567812345678", SecurityCode = "123", ExpirationDate = DateTime.Now.AddYears(1), Status = Status.OnHold },
                    new Payment { CardHolder = "Payment 6", Amount = 10, CreditCardNumber = "1234567812345678", SecurityCode = "123", ExpirationDate = DateTime.Now.AddYears(1), Status = Status.Rejected },
                    new Payment { CardHolder = "Payment 7", Amount = 10, CreditCardNumber = "1234567812345678", SecurityCode = "123", ExpirationDate = DateTime.Now.AddYears(1) },
                    new Payment { CardHolder = "Payment 8", Amount = 10, CreditCardNumber = "1234567812345678", SecurityCode = "123", ExpirationDate = DateTime.Now.AddYears(1) });

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