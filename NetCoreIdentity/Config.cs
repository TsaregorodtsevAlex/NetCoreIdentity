using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Test;
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
                new ApiResource("examinationapi", "Examination API")
            };
        }

        public static List<Client> GetClients() 
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "Examination client",
                    ClientId = "examinationclient",
                    AllowedGrantTypes = new[] {GrantType.Hybrid},
                    RedirectUris = new List<string>
                    {
                        "http://localhost:44317/signin-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "examinationapi"
                    },
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequireClientSecret = false,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:44317/signout-callback-oidc"
                    }
                }
            };
        }
    }
}
