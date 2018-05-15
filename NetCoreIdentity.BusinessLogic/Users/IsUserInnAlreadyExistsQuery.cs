using System;
using System.Linq;
using NetCoreCQRS.Queries;
using NetCoreDomain;
using NetCoreIdentity.BusinessLogic.Users.Dtos;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users
{
    public class IsUserInnAlreadyExistsQuery : BaseQuery
    {
        public Result<bool> Execute(UserDto userDto)
        {
            try
            {
                var userRepository = Uow.GetRepository<User>();

                var query = userRepository
                    .AsQueryable()
                    .Where(u => u.Inn == userDto.Inn && u.IsDeleted == false);

                if (userDto.IsExsistedUser)
                {
                    query = query
                        .Where(u => u.Id != userDto.Id);
                }

                var isInnExists = query.Any();

                return Result<bool>.Ok(isInnExists);
            }
            catch (Exception exception)
            {
                return Result<bool>.Fail(true, $"{exception.Message}, {exception.StackTrace}");
            }
        }
    }
}