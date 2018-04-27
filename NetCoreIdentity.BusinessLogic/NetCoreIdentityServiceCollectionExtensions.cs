using Microsoft.Extensions.DependencyInjection;
using NetCoreIdentity.BusinessLogic.UserClaims;
using NetCoreIdentity.BusinessLogic.Users;

namespace NetCoreIdentity.BusinessLogic
{
    public static class NetCoreIdentityServiceCollectionExtensions
    {
        public static IServiceCollection AddNetCoreIdentityBusinessLogicQueries(this IServiceCollection serviceCollection)
        {
            AddUsersQueries(serviceCollection);
            AddUsersCommands(serviceCollection);
            AddUserClaimsQueries(serviceCollection);
            return serviceCollection;
        }

        public static IServiceCollection AddNetCoreIdentityBusinessLogicCommands(this IServiceCollection serviceCollection)
        {
            return serviceCollection;
        }

        private static void AddUsersQueries(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<GetUserByEmailQuery>()
                .AddTransient<GetUserByIdQuery>()
                .AddTransient<GetUserByNameQuery>()
                .AddTransient<GetUsersPagedListQuery>()
                .AddTransient<IsUserActiveCheckQuery>()
                .AddTransient<IsUserCredentialsValidQuery>();
        }

        private static void AddUsersCommands(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<CreateUserCommand>()
                .AddTransient<CreateUserClaimCommand>();
        }

        private static void AddUserClaimsQueries(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<GetUserClaimsByUserIdQuery>();
        }
    }
}
