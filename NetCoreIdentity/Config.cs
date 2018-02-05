using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.Test;
using IdentityServer4.Models;

namespace NetCoreIdentity
{
    public static class Config
    {

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "Alex",
                    Password = "Alex",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Alex"),
                        new Claim("family_name", "Alex")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources() 
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static List<Client> GetClients() 
        {
            return new List<Client>();
        }
    }
}
