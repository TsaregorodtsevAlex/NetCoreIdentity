using System;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetCoreIdentityClientExtensions.Configurations;

namespace NetCoreIdentityClientExtensions
{
    public static class ServiceCollectionExtensions
    {
        private static IConfigurationRoot _configuration;

        public static IServiceCollection AddNetCoreIdentityAuthentication(this IServiceCollection serviceCollection, IConfigurationRoot configuration)
        {
            _configuration = configuration;

            serviceCollection
                .AddAuthentication(ConfigureAuthentication)
                .AddCookie(ConfigureCookieAuthentication)
                .AddOpenIdConnect("oidc", ConfigureOpenIdConnect)
                .AddIdentityServerAuthentication(ConfigureIdentityServerAuthentication);

            return serviceCollection;
        }

        private static void ConfigureAuthentication(AuthenticationOptions options)
        {
            var authenticationConfiguration = new AuthenticationConfiguration();
            _configuration.GetSection(nameof(AuthenticationConfiguration)).Bind(authenticationConfiguration);
            options.DefaultScheme = authenticationConfiguration.DefaultScheme;
            options.DefaultChallengeScheme = authenticationConfiguration.DefaultChallengeScheme;
        }

        private static void ConfigureCookieAuthentication(CookieAuthenticationOptions options)
        {
            var cookieAuthenticationConfiguration = new CookieAuthenticationConfiguration();
            _configuration.GetSection(nameof(CookieAuthenticationConfiguration)).Bind(cookieAuthenticationConfiguration);
            options.ExpireTimeSpan = TimeSpan.FromHours(cookieAuthenticationConfiguration.ExpireTimeSpan);
            options.Cookie.Name = cookieAuthenticationConfiguration.Name;
        }

        private static void ConfigureOpenIdConnect(OpenIdConnectOptions options)
        {
            var openIdConfiguration = new OpenIdConfiguration();
            _configuration.GetSection(nameof(OpenIdConfiguration)).Bind(openIdConfiguration);

            options.Authority = openIdConfiguration.Authority;
            options.RequireHttpsMetadata = openIdConfiguration.RequireHttpsMetadata;

            options.ClientSecret = openIdConfiguration.ClientSecret;
            options.ClientId = openIdConfiguration.ClientId;

            options.ResponseType = openIdConfiguration.ResponseType;

            options.Scope.Clear();
            foreach (var scope in openIdConfiguration.ScopesAsList)
            {
                options.Scope.Add(scope);
            }



            options.ClaimActions.Remove("amr");
            options.ClaimActions.MapJsonKey("website", "website");

            options.GetClaimsFromUserInfoEndpoint = openIdConfiguration.GetClaimsFromUserInfoEndpoint;
            options.SaveTokens = openIdConfiguration.SaveTokens;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = JwtClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role
            };
        }

        public static void ConfigureIdentityServerAuthentication(IdentityServerAuthenticationOptions identityServerAuthenticationOptions)
        {
            var identityServerAuthenticationConfiguration = new IdentityServerAuthenticationConfiguration();
            _configuration.GetSection(nameof(IdentityServerAuthenticationConfiguration)).Bind(identityServerAuthenticationConfiguration);

            identityServerAuthenticationOptions.Authority = identityServerAuthenticationConfiguration.Authority;
            identityServerAuthenticationOptions.ApiName = identityServerAuthenticationConfiguration.ApiName;
            identityServerAuthenticationOptions.ApiSecret = identityServerAuthenticationConfiguration.ApiSecret;
            identityServerAuthenticationOptions.EnableCaching = identityServerAuthenticationConfiguration.EnableCaching;
            identityServerAuthenticationOptions.RequireHttpsMetadata = identityServerAuthenticationConfiguration.RequireHttpsMetadata;
            identityServerAuthenticationOptions.SupportedTokens = identityServerAuthenticationConfiguration.SupportedTokens;
        }
    }
}