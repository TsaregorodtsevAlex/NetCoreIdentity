using System.Linq;
using FluentAssertions;
using NetCoreIdentity.BusinessLogic.Roles;
using NetCoreIdentity.BusinessLogic.Users;
using NetCoreIdentity.BusinessLogic.Users.Dtos;
using NetCoreIdentity.BusinessLogic.Users.Requests;
using NetCoreIdentity.UnitTests.TestData;
using NUnit.Framework;

namespace NetCoreIdentity.UnitTests
{
    [TestFixture]
    public class UserTests : BaseTest
    {


        [Test]
        public void GetUsersPagedList_ReturnList_Success()
        {
            var executor = GetExecutor();

            var userPagedListRequest = new GetUsersPagedListRequest
            {
                Skip = 0,
                Take = 10
            };
            var users = executor.GetQuery<GetUsersPagedListQuery>().Process(c => c.Execute(userPagedListRequest));

            users.Should().NotBeNull();
            users.IsFailure.Should().BeFalse(users.Error);
            users.IsSuccess.Should().BeTrue();
            users.Error.Should().BeNullOrEmpty();
            users.Value.Should().NotBeNull();
            users.Value.TotalCount.Should().BeGreaterThan(0);
            users.Value.Items.Should().NotBeNullOrEmpty();
            users.Value.Items.Length.Should().BeGreaterThan(0);
        }

        [Test]
        public void GetUserById_ReturnUserDto_Success()
        {
            var executor = GetExecutor();

            var userDto = executor.GetQuery<GetUserByIdQuery>().Process(q => q.Execute(CreatedUserIds.First()));

            userDto.IsFailure.Should().BeFalse(userDto.Error);
            userDto.IsSuccess.Should().BeTrue();
            userDto.Error.Should().BeNullOrEmpty();
            userDto.Value.Should().NotBeNull();
            userDto.Value.Should().NotBeNull();
            userDto.Value.Id.Should().Be(CreatedUserIds.First());
        }

        [Test]
        public void CreateUserCommand_UserCreated_Success()
        {
            var executor = GetExecutor();

            var roleResult = executor.GetQuery<GetRoleByNameQuery>().Process(q => q.Execute(RoleDtoTestData.AdminisrationRoleDto.Name));
            roleResult.Should().NotBeNull();
            var role = roleResult.Value;

            var userDto = new UserDto
            {
                FirstName = "CreatedUser",
                Role = role,
                IsActive = true
            };

            var createdUserIdResult = executor.GetCommand<CreateUserCommand>().Process(c => c.Execute(userDto.CreateUser()));
            createdUserIdResult.Should().NotBeNull($"{nameof(createdUserIdResult)} is null");

            var createdUserDtoResult = executor.GetQuery<GetUserByIdQuery>().Process(q => q.Execute(createdUserIdResult.Value));

            createdUserDtoResult.Should().NotBeNull();
            createdUserDtoResult.IsFailure.Should().BeFalse(createdUserDtoResult.Error);
            createdUserDtoResult.Value.Should().NotBeNull();
            createdUserDtoResult.Value.FirstName.Should().Be("CreatedUser");
        }

        [Test]
        public void UpdateUserCommand_UserUpdated_Success()
        {
            var executor = GetExecutor();

            var userDtoResult = executor.GetQuery<GetUserByIdQuery>().Process(q => q.Execute(CreatedUserIds.First()));
            userDtoResult.Should().NotBeNull();
            userDtoResult.IsFailure.Should().BeFalse(userDtoResult.Error);

            var userDto = userDtoResult.Value;
            userDto.Should().NotBeNull("UserDto is null");

            userDto.FirstName = "UpdatedUser";
            executor.GetCommand<UpdateUserCommand>().Process(c => c.Execute(userDto));

            var updatedUser = executor.GetQuery<GetUserByIdQuery>().Process(q => q.Execute(CreatedUserIds.First()));
            updatedUser.Should().NotBeNull();
            updatedUser.IsFailure.Should().BeFalse(updatedUser.Error);
            updatedUser.Value.Should().NotBeNull("UpdatedUser is null");
            updatedUser.Value.FirstName.Should().Be("UpdatedUser");
        }

        [Test]
        public void DeleteUserCommand_UserDeleted_Success()
        {
            var executor = GetExecutor();

            var roleResult = executor.GetQuery<GetRoleByNameQuery>().Process(q => q.Execute(RoleDtoTestData.AdminisrationRoleDto.Name));
            roleResult.Should().NotBeNull();
            var role = roleResult.Value;

            var userDto = new UserDto
            {
                FirstName = "CreatedUser",
                Role = role,
                IsActive = true
            };

            var createdUserIdResult = executor.GetCommand<CreateUserCommand>().Process(c => c.Execute(userDto.CreateUser()));
            createdUserIdResult.Should().NotBeNull($"{nameof(createdUserIdResult)} is null");

            var createdUserDtoResult = executor.GetQuery<GetUserByIdQuery>().Process(q => q.Execute(createdUserIdResult.Value));
            createdUserDtoResult.Should().NotBeNull();
            createdUserDtoResult.IsFailure.Should().BeFalse(createdUserDtoResult.Error);
            createdUserDtoResult.Value.Should().NotBeNull();
            createdUserDtoResult.Value.FirstName.Should().Be("CreatedUser");

            var createdUserDto = createdUserDtoResult.Value;
            executor.GetCommand<DeleteUserCommand>().Process(c => c.Execute(createdUserDto.Id));

            var deletedUser = executor.GetQuery<GetUserByIdQuery>().Process(c => c.Execute(createdUserDto.Id));
            deletedUser.Should().NotBeNull();
            deletedUser.IsFailure.Should().BeTrue();
            deletedUser.Error.Should().Be("User not found");
        }
    }
}