using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCQRS;
using NetCoreIdentity.BusinessLogic.Users;
using NetCoreIdentity.BusinessLogic.UserClaims;

namespace NetCoreIdentity
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddCustomUserStore(this IIdentityServerBuilder builder)
        {
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

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sujectId = context.Subject.GetSubjectId();
            var userId = Guid.Parse(sujectId);
            context.IssuedClaims = _executor
                .GetQuery<GetUserClaimsByUserIdQuery>()
                .Process(q => q.Execute(userId), userClaim => userClaim.ToClaim())
                .ToList();

            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject.GetSubjectId();
            var userId = Guid.Parse(subject);
            context.IsActive = _executor.GetQuery<IsUserActiveCheckQuery>().Process(q => q.Execute(userId));

            return Task.FromResult(0);
        }
    }
}
