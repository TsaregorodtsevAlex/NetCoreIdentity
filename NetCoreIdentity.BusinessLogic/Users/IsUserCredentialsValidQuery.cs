using System.Linq;
using NetCoreCQRS;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users
{
    public class IsUserCredentialsValidQuery : BaseQuery
    {
        public bool Execute(string userName, string userPassword)
        {
            var userRepository = Uow.GetRepository<User>();

            var user = userRepository
                .AsQueryable()
                .FirstOrDefault(u => u.UserName == userName && u.PasswordHash == userPassword);

            return user != null;
        }
    }
}
