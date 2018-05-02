using System;
using NetCoreCQRS.Commands;
using NetCoreDomain;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users
{
    public class DeleteUserCommand : BaseCommand
    {
        public Result<bool> Execute(Guid userId)
        {
            try
            {
                var userRepository = Uow.GetRepository<User>();
                var user = userRepository.GetById(userId);

                if (user == null)
                {
                    return Result<bool>.Ok(true);
                }

                user.IsActive = false;
                user.IsDeleted = true;
                userRepository.Update(user);
                Uow.SaveChanges();
                return Result<bool>.Ok(true);
            }
            catch (Exception exception)
            {
                return Result<bool>.Fail(false, exception.Message);
            }
        }
    }
}