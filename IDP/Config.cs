using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var result = new List<IdentityResource>
            {
                new IdentityResources.OpenId(), // OpenId connect
                new IdentityResources.Profile(),
                new IdentityResource(
                    "roles",
                    "Custom roles",
                    new List<string> { "role" })
            };

            return result;
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            var result = new List<ApiResource>
            {
                new ApiResource("srcapi", "Src API")
            };

            return result;
        }

        public static IEnumerable<Client> GetClients()
        {
            var result = new List<Client>
            {
                new Client
                {
                    ClientId = "srcclient",
                    ClientName = "Src client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AccessTokenLifetime = 120,
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "roles",
                        "srcapi"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                }
            };

            return result;
        }

        public static List<TestUser> GetTestUsers()
        {
            var result = new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = Guid.NewGuid().ToString(),
                    Username = "Test",
                    Password = "test123",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Test")
                    }
                }
            };

            return result;
        }
    }
}
