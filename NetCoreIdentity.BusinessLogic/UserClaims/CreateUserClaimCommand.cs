using NetCoreCQRS.Commands;
using NetCoreIdentity.BusinessLogic.UserClaims.Dtos;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.UserClaims
{
    public class CreateUserClaimCommand : BaseCommand
    {
        public void Execute(UserClaimDto userClaimDto)
        {
            var userClaimsRepository = Uow.GetRepository<UserClaim>();
            userClaimsRepository.Create(userClaimDto.ToUserClaim);
            Uow.SaveChanges();
        }
    }
}