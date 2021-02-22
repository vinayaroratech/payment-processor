using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Payments.Infrastructure.Seed
{
    public class InMemoryConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("payment", "Payments Service")
                {
                    Scopes = { "Payments.API" }
                },
                new ApiResource("order", "Order Service")
                {
                    Scopes = { "order", "Payments.API" }
                }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new ApiScope[] {
                 new ApiScope("Payments.API"),
                 new ApiScope("order"),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "credentials_client",
                    ClientName = "ClientCredentials Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "Payments.API"
                    },
                },
                new Client
                {
                    ClientId = "Payments.IntegrationTests",
                    ClientName = "Payments IntegrationTests",
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "order","Payments.API",
                    }
                },
                new Client
                {
                    ClientId = "main_client",
                    ClientName = "Implicit Client",
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = { "http://localhost:5020/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5020/signout-callback-oidc" },
                    RequireConsent = true,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                new Client
                {
                    ClientId = "order_client",
                    ClientName = "Order Client",
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = {
                        "order","goods",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    RedirectUris = { "http://localhost:5021/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5021/signout-callback-oidc" },
                    RequireConsent = true,
                },
                new Client
                {
                    ClientId = "goods_client",
                    ClientName = "Goods Client",
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "http://localhost:5022/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5022/signout-callback-oidc" },
                    RequireConsent = true,
                    AllowedScopes = {
                        "goods",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestUser> Users()
        {
            return new[]
            {
                new TestUser
                {
                    SubjectId = "f26da293-02fb-4c90-be75-e4aa51e0bb17",
                    Username = "vinay@arora",
                    Password = "Administrator1!",
                     Claims = new List<Claim>
                            {
                                new Claim(JwtClaimTypes.Email, "vinay@arora")
                            }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "chaney",
                    Password = "123"
                }
            };
        }
    }
}