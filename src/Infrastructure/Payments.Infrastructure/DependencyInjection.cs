using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Payments.Application.Common.Interfaces;
using Payments.Infrastructure.Data.Repositories;
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
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("DefaultConnection"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddTransient<IDatabaseTransaction, EntityDatabaseTransaction>();
            services.AddScoped<EfPaymentRepository>();

            services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    // Basic built in validations
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IDateTime, DateTimeService>()
                    .AddTransient<IIdentityService, IdentityService>();

            AddIdentityServer(services, environment);
            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            });

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
    }
}