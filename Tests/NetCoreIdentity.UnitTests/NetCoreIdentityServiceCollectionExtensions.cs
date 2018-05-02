using Microsoft.Extensions.DependencyInjection;
using NetCoreCQRS.Commands;
using NetCoreCQRS.Handlers;
using NetCoreCQRS.Queries;

namespace NetCoreIdentity.UnitTests
{
    public static class NetCoreIdentityUnitTestServiceCollectionExtensions
    {
        public static IServiceCollection AddNetCoreIdentityTestDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerNetCoreIdentityUnitTest>()
                .AddClasses(classes => classes.AssignableTo<BaseQuery>())
                .AsSelf()
                .WithTransientLifetime());

            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerNetCoreIdentityUnitTest>()
                .AddClasses(classes => classes.AssignableTo<BaseCommand>())
                .AsSelf()
                .WithTransientLifetime());

            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerNetCoreIdentityUnitTest>()
                .AddClasses(classes => classes.AssignableTo<BaseHandler>())
                .AsSelf()
                .WithTransientLifetime());

            return serviceCollection;
        }
    }
}
