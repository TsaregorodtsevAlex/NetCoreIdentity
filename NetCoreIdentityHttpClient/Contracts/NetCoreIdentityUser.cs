using System;
using System.Collections.Generic;

namespace NetCoreIdentityHttpClient.Contracts
{
    public class NetCoreIdentityUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<string> Roles { get; set; }
        public string Gender { get; set; }
    }
}