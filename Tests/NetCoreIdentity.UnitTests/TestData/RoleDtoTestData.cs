using NetCoreIdentity.BusinessLogic.Roles.Dtos;

namespace NetCoreIdentity.UnitTests.TestData
{
    public class RoleDtoTestData
    {
        public static RoleDto AdminisrationRoleDto = new RoleDto
        {
            Name = "Administrator"
        };

        public static RoleDto EmployeeRoleDto = new RoleDto
        {
            Name = "Employee"
        };
    }
}