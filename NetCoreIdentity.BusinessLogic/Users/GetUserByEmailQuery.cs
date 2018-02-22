using System.Linq;
using NetCoreCQRS;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users
{
    public class GetUserByEmailQuery : BaseQuery
    {
        public User Execute(string userEmail)
        {
            var userRepository = Uow.GetRepository<User>();
            var user = userRepository
                .AsQueryable()
                .FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                //todo throw error
            }

            return user;
        }
    }
}
