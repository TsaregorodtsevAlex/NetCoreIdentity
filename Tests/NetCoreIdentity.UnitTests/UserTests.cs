using System;
using System.Collections.Generic;
using FluentAssertions;
using NetCoreIdentity.BusinessLogic.Users;
using NetCoreIdentity.BusinessLogic.Users.Requests;
using NUnit.Framework;

namespace NetCoreIdentity.UnitTests
{
    [TestFixture]
    public class UserTests : BaseTest
    {
        private List<Guid> _createdUserIds;

        [OneTimeSetUp]
        public void UserTestsSetUp()
        {
            FillRoleTable();
            _createdUserIds = FillUserTable();
        }

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
            users.IsFailure.Should().BeFalse();
            users.IsSuccess.Should().BeTrue();
            users.Error.Should().BeNullOrEmpty();
            users.Value.Should().NotBeNull();
            users.Value.TotalCount.Should().BeGreaterThan(0);
            users.Value.Items.Should().NotBeNullOrEmpty();
            users.Value.Items.Length.Should().BeGreaterThan(0);
        }
    }
}