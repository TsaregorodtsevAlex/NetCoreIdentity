using IdentityServer4.AccessTokenValidation;

namespace NetCoreIdentityClientExtensions.Configurations
{
    public class IdentityServerAuthenticationConfiguration
    {
        public string Authority { get; set; }
        public string ApiName { get; set; }
        public string ApiSecret { get; set; }
        public bool EnableCaching { get; set; }
        public bool RequireHttpsMetadata { get; set; }
        public SupportedTokens SupportedTokens { get; set; }
    }
}