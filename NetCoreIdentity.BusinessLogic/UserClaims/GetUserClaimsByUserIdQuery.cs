using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreCQRS;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.UserClaims
{
    public class GetUserClaimsByUserIdQuery : BaseQuery
    {
        public List<UserClaim> Execute(Guid userId)
        {
            var userClaimsRepository = Uow.GetRepository<UserClaim>();
            var userClaims = userClaimsRepository
                .AsQueryable()
                .Where(u => u.UserId == userId)
                .ToList();

            if (userClaims == null)
            {
                //todo throw error
            }

            return userClaims;
        }
    }
}
