using System;
using System.Linq;
using NetCoreCQRS.Queries;
using NetCoreDomain;
using NetCoreIdentity.BusinessLogic.Roles.Dtos;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Roles
{
    public class GetRoleByIdQuery : BaseQuery
    {
        public Result<RoleDto> Execute(Guid roleId)
        {
            try
            {
                var roleRepository = Uow.GetRepository<Role>();
                var roleDto = roleRepository
                    .AsQueryable()
                    .Where(r => r.Id == roleId && r.IsDeleted == false)
                    .Select(RoleDto.MapFromRole)
                    .FirstOrDefault();

                return roleDto == null
                    ? Result<RoleDto>.Fail(null, $"Role with id {roleId} not found")
                    : Result<RoleDto>.Ok(roleDto);
            }
            catch (Exception exception)
            {
                return Result<RoleDto>.Fail(null, $"{exception.Message}, {exception.StackTrace}");
            }
        }
    }
}