using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Payments.Application.Common.Interfaces;
using Payments.Domain.Entities;
using Payments.Infrastructure.Persistence;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Payments.API.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .ConfigureServices(services =>
                {
                    // Create a new service provider.
                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();

                    // Add a database context using an in-memory 
                    // database for testing.
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        options.UseInternalServiceProvider(serviceProvider);
                    });

                    services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

                    services.AddScoped<ICurrentUserService, TestCurrentUserService>();
                    services.AddScoped<IDateTime, TestDateTimeService>();
                    services.AddScoped<IIdentityService, TestIdentityService>();

                    var sp = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database
                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<ApplicationDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    context.Database.EnsureCreated();

                    try
                    {
                        SeedSampleData(context);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"An error occurred seeding the database with sample data. Error: {ex.Message}.");
                    }
                })
                .UseEnvironment("Test");
        }

        public HttpClient GetAnonymousClient()
        {
            return CreateClient();
        }

        public async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            return await GetAuthenticatedClientAsync("vinay@arora", "Administrator1!");
        }

        public async Task<HttpClient> GetAuthenticatedClientAsync(string userName, string password)
        {
            var client = CreateClient();

            var token = await GetAccessTokenAsync(client, userName, password);

            client.SetBearerToken(token);

            return client;
        }

        private async Task<string> GetAccessTokenAsync(HttpClient client, string userName, string password)
        {
            var disco = await client.GetDiscoveryDocumentAsync();

            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "Payments.IntegrationTests",
                ClientSecret = "secret",

                Scope = "Payments.API openid profile",
                UserName = userName,
                Password = password
            });

            if (response.IsError)
            {
                throw new Exception(response.Error);
            }

            return response.AccessToken;
        }

        public static void SeedSampleData(ApplicationDbContext context)
        {
            context.Payments.AddRange(
                new Payment { Id = 1, Name = "Do this thing." },
                new Payment { Id = 2, Name = "Do this thing too." },
                new Payment { Id = 3, Name = "Do many, many things." },
                new Payment { Id = 4, Name = "This thing is done!", IsComplete = true }
            );

            context.SaveChanges();
        }
    }
}