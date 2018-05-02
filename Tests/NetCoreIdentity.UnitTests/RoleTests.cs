using FluentAssertions;
using NetCoreIdentity.BusinessLogic.Roles;
using NUnit.Framework;

namespace NetCoreIdentity.UnitTests
{
    [TestFixture]
    public class RoleTests : BaseTest
    {
        [Test]
        public void GetRolesQuery_ReturnRoles_Success()
        {
            var executor = GetExecutor();
            var roles = executor.GetQuery<GetRolesQuery>().Process(q => q.Execute());
            roles.Should().NotBeNull();
            roles.IsFailure.Should().BeFalse(roles.Error);
            roles.Value.Should().NotBeNull();
            roles.Value.Count.Should().BeGreaterThan(0);
        }
    }
}