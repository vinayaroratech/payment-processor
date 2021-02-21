using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Payments.Application.Common.Interfaces;
using Payments.Infrastructure.Identity;
using Payments.Infrastructure.Persistence;
using Payments.Infrastructure.Services;
using System.Collections.Generic;
using System.Security.Claims;

namespace Payments.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            
            AddIdentityServer(services, environment);
            
            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddTransient<IDateTime, DateTimeService>();

            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetService<ApplicationDbContext>());

            return services;
        }

        private static void AddIdentityServer(IServiceCollection services, IWebHostEnvironment environment)
        {
            if (environment.IsEnvironment("Test"))
            {
                services.AddIdentityServer()
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
                    {
                        options.Clients.Add(new Client
                        {
                            ClientId = "Payments.IntegrationTests",
                            AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                            ClientSecrets = { new Secret("secret".Sha256()) },
                            AllowedScopes = { "Payments.API", "openid", "profile" }
                        });
                    }).AddTestUsers(new List<TestUser>
                    {
                        new TestUser
                        {
                            SubjectId = "f26da293-02fb-4c90-be75-e4aa51e0bb17",
                            Username = "vinay@arora",
                            Password = "PaymentProcessor!",
                            Claims = new List<Claim>
                            {
                                new Claim(JwtClaimTypes.Email, "vinay@arora")
                            }
                        }
                    });
            }
            else
            {
                services.AddIdentityServer()
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            }
        }

        public static IServiceCollection AddUserService(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}