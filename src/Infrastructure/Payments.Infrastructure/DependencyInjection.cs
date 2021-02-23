using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Payments.Application.Common.Interfaces;
using Payments.Infrastructure.Identity;
using Payments.Infrastructure.Persistence;
using Payments.Infrastructure.Seed;
using Payments.Infrastructure.Services;
using System.Linq;
namespace Payments.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
#if DEBUG
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("PaymentProcessorDb"));
#else
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
#endif
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>())
                .AddScoped<IDomainEventService, DomainEventService>();

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                // Basic built in validations
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IDateTime, DateTimeService>()
                    .AddTransient<IIdentityService, IdentityService>();

            AddIdentityServer(services, environment);

            services.AddAuthentication()
                .AddIdentityServerJwt();

            return services;
        }

        private static void AddIdentityServer(IServiceCollection services, IWebHostEnvironment environment)
        {
            if (environment.IsEnvironment("Test"))
            {
                services.AddIdentityServer()
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>()
              //api resources
              .AddInMemoryApiResources(InMemoryConfig.GetApiResources())
              .AddInMemoryApiScopes(InMemoryConfig.GetApiScopes())
              .AddTestUsers(InMemoryConfig.Users().ToList())
              .AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
              .AddInMemoryClients(InMemoryConfig.GetClients());
            }
            else
            {
                services.AddIdentityServer()
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            }
        }

        public static IServiceCollection AddUserService(this IServiceCollection services)
        {
            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}