using System;
using NetCoreCQRS.Commands;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users
{
    public class RegisterUserCommand : BaseCommand
    {
        public Guid Execute(User user)
        {
            var userRepository = Uow.GetRepository<User>();
            userRepository.Create(user);
            Uow.SaveChanges();
            return user.Id;
        }
    }
}