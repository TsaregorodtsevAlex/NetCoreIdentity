using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCQRS;
using NetCoreIdentity.BusinessLogic.Users;
using NetCoreIdentity.BusinessLogic.UserClaims;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddCustomUserStore(this IIdentityServerBuilder builder)
        {
            builder.AddProfileService<UserProfileService>();
            return builder;
        }
    }

    public class UserProfileService : IProfileService
    {
        private readonly IExecutor _executor;

        public UserProfileService(IExecutor executor)
        {
            _executor = executor;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sujectId = context.Subject.GetSubjectId();
            var userId = Guid.Parse(sujectId);
            var userClaims = await _executor
                .GetQuery<GetUserClaimsByUserIdQuery>()
                .Process<UserClaim, Claim>(async query => await query.ExecuteAsync(userId), userClaim => userClaim.ToClaim());

            context.IssuedClaims = userClaims.ToList();
        }

        public Task IsActiveAsync(IsActiveContext context)

        {
            var subject = context.Subject.GetSubjectId();
            var userId = Guid.Parse(subject);
            context.IsActive = _executor.GetQuery<IsUserActiveCheckQuery>().Process(query => query.Execute(userId));

            return Task.FromResult(0);
        }
    }
}
