using System;
using System.Linq.Expressions;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Roles.Dtos
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public static Func<UserRole, RoleDto> Map = uerRole => new RoleDto
        {
            Id = uerRole.Role.Id,
            Name = uerRole.Role.Name
        };

        public static Expression<Func<Role, RoleDto>> MapFromRole = role => new RoleDto
        {
            Id = role.Id,
            Name = role.Name
        };

        public UserRole ToUserRole()
        {
            return new UserRole
            {
                RoleId = Id
            };
        }

        public Role ToRole()
        {
            return new Role
            {
                Name = Name,
                NormalizedName = Name
            };
        }
    }
}