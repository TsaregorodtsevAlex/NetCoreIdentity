﻿using Microsoft.Extensions.DependencyInjection;
using NetCoreCQRS.Commands;
using NetCoreCQRS.Handlers;
using NetCoreCQRS.Queries;

namespace NetCoreIdentity.BusinessLogic
{
    public static class NetCoreIdentityServiceCollectionExtensions
    {
        public static IServiceCollection AddNetCoreIdentityBusinessLogicDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerNetCoreIdentityBusinessLogic>()
                .AddClasses(classes => classes.AssignableTo<BaseQuery>())
                .AsSelf()
                .WithTransientLifetime());

            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerNetCoreIdentityBusinessLogic>()
                .AddClasses(classes => classes.AssignableTo<BaseCommand>())
                .AsSelf()
                .WithTransientLifetime());

            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerNetCoreIdentityBusinessLogic>()
                .AddClasses(classes => classes.AssignableTo<BaseHandler>())
                .AsSelf()
                .WithTransientLifetime());

            return serviceCollection;
        }
    }
}
