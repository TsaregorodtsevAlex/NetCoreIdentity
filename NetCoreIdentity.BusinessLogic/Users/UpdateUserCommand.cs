using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetCoreCQRS.Commands;
using NetCoreDomain;
using NetCoreIdentity.BusinessLogic.Users.Dtos;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users
{
    public class UpdateUserCommand : BaseCommand
    {
        public Result<bool> Execute(UserDto userDto)
        {
            try
            {
                var userRepository = Uow.GetRepository<User>();
                var user = userRepository
                    .AsQueryable()
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefault(u => u.Id == userDto.Id);

                if (user == null)
                {
                    return Result<bool>.Fail(false, "User not found");
                }

                userDto.UpdateUser(user);
                userRepository.Update(user);
                Uow.SaveChanges();
                return Result<bool>.Ok(true);
            }
            catch (Exception exception)
            {
                return Result<bool>.Fail(false, $"{exception.Message}, {exception.StackTrace}");
            }
        }
    }
}