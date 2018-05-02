using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCQRS.Queries;
using NetCoreDomain;
using NetCoreIdentity.BusinessLogic.Roles.Dtos;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Roles
{
    public class GetRolesQuery : BaseQuery
    {
        public Result<List<RoleDto>> Execute()
        {
            try
            {
                var rolesRepository = Uow.GetRepository<Role>();
                return Result<List<RoleDto>>.Ok(rolesRepository
                    .AsQueryable()
                    .Where(r => r.IsDeleted == false)
                    .Select(RoleDto.MapFromRole)
                    .ToList());
            }
            catch (Exception exception)
            {
                return Result<List<RoleDto>>.Fail(null, $"{exception.Message}, {exception.StackTrace}");
            }
        }
    }
}