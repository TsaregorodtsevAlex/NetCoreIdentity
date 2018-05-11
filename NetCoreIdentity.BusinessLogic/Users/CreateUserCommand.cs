using System;
using NetCoreCQRS.Commands;
using NetCoreDomain;
using NetCoreIdentity.BusinessLogic.Users.Dtos;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users
{
    public class CreateUserCommand : BaseCommand
    {
        public Result<Guid> Execute(User user)
        {
            try
            {
                var userRepository = Uow.GetRepository<User>();
                userRepository.Create(user);
                Uow.SaveChanges();
                return Result<Guid>.Ok(user.Id);
            }
            catch (Exception exception)
            {
                return Result<Guid>.Fail(Guid.Empty, $"{exception.Message}, {exception.StackTrace}");
            }
        }
    }
}