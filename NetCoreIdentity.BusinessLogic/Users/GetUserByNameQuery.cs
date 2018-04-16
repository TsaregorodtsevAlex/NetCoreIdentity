using System.Linq;
using NetCoreCQRS.Queries;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users
{
    public class GetUserByNameQuery : BaseQuery
    {
        public User Execute(string userName)
        {
            var userRepository = Uow.GetRepository<User>();
            var user = userRepository
                .AsQueryable()
                .FirstOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                //todo throw error
            }

            return user;
        }
    }
}
