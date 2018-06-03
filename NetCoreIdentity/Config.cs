using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace NetCoreIdentity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static List<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("examinationapi", "Система тестирования")
            };
        }

        public static List<Client> GetClients(bool isProduction)
        {
            var redirectUrl = isProduction
                ? "https://192.168.45.99:44317"
                : "https://localhost:44317";

            return new List<Client>
            {
                new Client
                {
                    ClientName = "Система тестирования",
                    ClientId = "examinationclient",
                    AllowedGrantTypes = new[] {GrantType.Hybrid},
                    //AllowedGrantTypes = GrantTypes.Implicit,
                    //RequireConsent = false,
                    //AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string>
                    {
                        $"{redirectUrl}/signin-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "examinationapi"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        $"{redirectUrl}"
                    },
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireClientSecret = true,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{redirectUrl}/signout-callback-oidc"
                    }
                }
            };
        }
    }
}
