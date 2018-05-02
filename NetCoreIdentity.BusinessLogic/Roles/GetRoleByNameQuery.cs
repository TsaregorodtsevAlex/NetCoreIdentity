using System;
using System.Linq;
using NetCoreCQRS.Queries;
using NetCoreDomain;
using NetCoreIdentity.BusinessLogic.Roles.Dtos;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Roles
{
    public class GetRoleByNameQuery : BaseQuery
    {
        public Result<RoleDto> Execute(string roleName)
        {
            try
            {
                var roleRepository = Uow.GetRepository<Role>();
                var roleDto = roleRepository
                    .AsQueryable()
                    .Where(r => r.Name == roleName && r.IsDeleted == false)
                    .Select(RoleDto.MapFromRole)
                    .FirstOrDefault();

                return roleDto == null
                    ? Result<RoleDto>.Fail(null, $"Role with name {roleName} not found")
                    : Result<RoleDto>.Ok(roleDto);
            }
            catch (Exception exception)
            {
                return Result<RoleDto>.Fail(null, $"{exception.Message}, {exception.StackTrace}");
            }
        }
    }
}