using System;
using NetCoreCQRS.Queries;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users
{
    public class IsUserActiveCheckQuery : BaseQuery
    {
        public bool Execute(Guid userId)
        {
            var userRepository = Uow.GetRepository<User>();
            var user = userRepository.GetById(userId);
            return user != null && user.IsActive;
        }
    }
}
