using System.Collections.Generic;
using System.Linq;

namespace NetCoreIdentityClientExtensions.Configurations
{
    public class OpenIdConfiguration
    {
        public string Authority { get; set; }
        public bool RequireHttpsMetadata { get; set; }

        public string ClientSecret { get; set; }
        public string ClientId { get; set; }

        public string ResponseType { get; set; }

        public string Scopes { get; set; }

        public bool GetClaimsFromUserInfoEndpoint { get; set; }
        public bool SaveTokens { get; set; }

        public List<string> ScopesAsList => Scopes.Split(',', ';').ToList();
    }
}