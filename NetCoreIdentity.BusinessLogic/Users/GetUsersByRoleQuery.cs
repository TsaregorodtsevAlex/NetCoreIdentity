using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetCoreCQRS.Queries;
using NetCoreDomain;
using NetCoreIdentity.BusinessLogic.Users.Dtos;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users
{
    public class GetUsersByRoleQuery : BaseQuery
    {
        public Result<List<UserDto>> Execute(string roleName)
        {
            try
            {
                var userRepository = Uow.GetRepository<User>();
                var response = userRepository
                    .AsQueryable()
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .Where(u => u.UserRoles.Any(ur => ur.Role.Name == roleName) && u.IsDeleted == false)
                    .AsEnumerable()
                    .Select(UserDto.MapFromUser)
                    .ToList();

                return Result<List<UserDto>>.Ok(response);
            }
            catch (Exception exception)
            {
                return Result<List<UserDto>>.Fail(null, exception.Message);
            }
        }
    }
}