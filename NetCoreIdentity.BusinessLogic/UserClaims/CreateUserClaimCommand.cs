using System;
using NetCoreCQRS.Commands;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.UserClaims
{
    public class CreateUserClaimRequest
    {
        public Guid UserId { get; set; }
        public string ClaimName { get; set; }
        public string ClaimValue { get; set; }

        public UserClaim ToUserClaim => new UserClaim
        {
            UserId = UserId,
            ClaimType = ClaimName,
            ClaimValue = ClaimValue
        };
    }

    public class CreateUserClaimCommand : BaseCommand
    {
        public void Execute(CreateUserClaimRequest createUserClaimRequest)
        {
            var userClaimsRepository = Uow.GetRepository<UserClaim>();
            userClaimsRepository.Create(createUserClaimRequest.ToUserClaim);
            Uow.SaveChanges();
        }
    }
}