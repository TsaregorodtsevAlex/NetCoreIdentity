using System;
using System.Security.Claims;
using NetCoreIdentity.BusinessLogic.Users.Dtos;
using NetCoreIdentity.DataAccess;
using NetCoreIdentity.DataAccess.Enums;

namespace NetCoreIdentity.BusinessLogic.UserClaims.Dtos
{
    public class UserClaimDto
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

        public void UpdateUserClaim(UserClaim userClaim)
        {
            userClaim.ClaimValue = ClaimValue;
        }

        public static UserClaimDto UserNameClaim(UserDto user, Guid? userId = null)
        {
            return new UserClaimDto
            {
                UserId = userId ?? user.Id,
                ClaimName = ClaimTypes.Name,
                ClaimValue = user.UserFullName
            };
        }

        public static UserClaimDto UserRoleClaim(UserDto user, Guid? userId = null)
        {
            return new UserClaimDto
            {
                UserId = userId ?? user.Id,
                ClaimName = ClaimTypes.Role,
                ClaimValue = user.Role.Name
            };
        }

        public static UserClaimDto UserGenderClaim(UserDto user, Guid? userId = null)
        {
            return new UserClaimDto
            {
                UserId = userId ?? user.Id,
                ClaimName = ClaimTypes.Gender,
                ClaimValue = Enum.GetName(typeof(GenderType), GenderType.Man)
            };
        }
    }
}