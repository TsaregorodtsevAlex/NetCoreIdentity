using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetCoreCQRS;
using NetCoreIdentity.DataAccess;
using System.Threading.Tasks;

namespace NetCoreIdentity.BusinessLogic.UserClaims
{
    public class GetUserClaimsByUserIdQuery : BaseQuery
    {
        public async ValueTask<List<UserClaim>> ExecuteAsync(Guid userId)
        {
            var userClaimsRepository = Uow.GetRepository<UserClaim>();
            var userClaims = await userClaimsRepository
                .AsQueryable()
                .Where(u => u.UserId == userId)
                .ToListAsync();

            return userClaims;
        }
    }
}
