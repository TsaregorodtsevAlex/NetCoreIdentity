using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetCoreCQRS;
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
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                //todo throw error
            }

            return user;
        }
    }
}
