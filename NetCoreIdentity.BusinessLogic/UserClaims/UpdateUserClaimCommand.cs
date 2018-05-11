using System.Linq;
using NetCoreCQRS.Commands;
using NetCoreIdentity.BusinessLogic.UserClaims.Dtos;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.UserClaims
{
    public class UpdateUserClaimCommand : BaseCommand
    {
        public void Execute(UserClaimDto userClaimDto)
        {
            var userClaimsRepository = Uow.GetRepository<UserClaim>();

            var claim = userClaimsRepository
                .AsQueryable()
                .FirstOrDefault(c => c.UserId == userClaimDto.UserId && c.ClaimType == userClaimDto.ClaimName && c.IsDeleted == false);

            if (claim == null)
            {
                claim = userClaimDto.ToUserClaim;
                userClaimsRepository.Create(claim);
            }
            else
            {
                userClaimDto.UpdateUserClaim(claim);
                userClaimsRepository.Update(claim);
            }

            Uow.SaveChanges();
        }
    }
}