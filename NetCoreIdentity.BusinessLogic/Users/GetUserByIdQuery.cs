using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetCoreCQRS.Queries;
using NetCoreDomain;
using NetCoreIdentity.BusinessLogic.Users.Dtos;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users
{
    public class GetUserByIdQuery : BaseQuery
    {
        public Result<UserDto> Execute(Guid userId)
        {
            try
            {
                var userRepository = Uow.GetRepository<User>();
                var user = userRepository
                    .AsQueryable()
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefault(u => u.Id == userId && u.IsDeleted == false);

                if (user == null)
                {
                    return Result<UserDto>.Fail(null, "User not found");
                }

                var userDto = UserDto.MapFromUser(user);
                return Result<UserDto>.Ok(userDto);
            }
            catch (Exception exception)
            {
                return Result<UserDto>.Fail(null, $"{exception.Message}, {exception.StackTrace}");
            }
        }
    }
}
