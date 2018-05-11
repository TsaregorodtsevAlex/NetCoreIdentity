using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCQRS;
using NetCoreDataAccess.UnitOfWork;
using NetCoreDI;
using NetCoreIdentity.BusinessLogic;
using NetCoreIdentity.BusinessLogic.Roles;
using NetCoreIdentity.BusinessLogic.Users;
using NetCoreIdentity.DataAccess;
using NetCoreIdentity.UnitTests.TestData;
using NUnit.Framework;

namespace NetCoreIdentity.UnitTests
{

    [TestFixture]
    public class BaseTest
    {
        protected IServiceCollection ServiceCollection;
        protected ServiceProvider ServiceProvider;

        protected List<Guid> CreatedUserIds;

        [OneTimeSetUp]
        public void SetUp()
        {
            ServiceCollection = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<NetCoreIdentityDbContext>(opt => opt.UseInMemoryDatabase("Add_writes_to_database")
                    .ConfigureWarnings(config => config.Ignore(InMemoryEventId.TransactionIgnoredWarning)))
                .AddScoped<DbContext, NetCoreIdentityDbContext>()
                .AddTransient<IExecutor, Executor>()
                .AddTransient<IAmbientContext, AmbientContext>()
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<IObjectResolver, ObjectResolver>();

            ServiceCollection
                .AddNetCoreIdentityBusinessLogicDependencies()
                .AddNetCoreIdentityTestDependencies();


            ServiceProvider = ServiceCollection.BuildServiceProvider();
            var _ = new AmbientContext(ServiceProvider);

            FillRoleTable();
            CreatedUserIds = FillUserTable();
        }

        public IExecutor GetExecutor()
        {
            return ServiceProvider.GetService<IExecutor>();
        }

        public void ClearInMemotyDb()
        {
            //var executor = GetExecutor();
            //executor.GetCommand<DeleteAllTestEntitiesCommand>().Process(c => c.Execute());
        }

        public void FillRoleTable()
        {
            var executor = GetExecutor();
            executor
                .CommandChain()
                .AddCommand<CreateRoleCommand>(c => c.Execute(RoleDtoTestData.AdminisrationRoleDto))
                .AddCommand<CreateRoleCommand>(c => c.Execute(RoleDtoTestData.EmployeeRoleDto))
                .ExecuteAllWithTransaction();
        }

        public List<Guid> FillUserTable()
        {
            var executor = GetExecutor();
            var administratorRoleResult = executor.GetQuery<GetRoleByNameQuery>().Process(q => q.Execute(RoleDtoTestData.AdminisrationRoleDto.Name));

            if (administratorRoleResult.IsFailure)
            {
                throw new Exception(administratorRoleResult.Error);
            }

            var createdUserIds = new List<Guid>();

            var testUser = UserTestData.TestUserDto;
            testUser.Role = administratorRoleResult.Value;

            var createUserResult = executor.GetCommand<CreateUserCommand>().Process(c => c.Execute(testUser.ToUser()));

            if (createUserResult.IsFailure)
            {
                throw new Exception(createUserResult.Error);
            }

            createdUserIds.Add(createUserResult.Value);
            return createdUserIds;
        }
    }
}
