using System;
using Microsoft.AspNetCore.Mvc;
using NetCoreDataAccess.BaseResponses;
using NetCoreDomain;
using NetCoreIdentity.BusinessLogic.UserClaims;
using NetCoreIdentity.BusinessLogic.UserClaims.Dtos;
using NetCoreIdentity.BusinessLogic.Users;
using NetCoreIdentity.BusinessLogic.Users.Dtos;
using NetCoreIdentity.BusinessLogic.Users.Requests;

namespace NetCoreIdentity.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/users")]
    public class UserApiController : BaseApiController
    {
        // GET: api/UserApi/5
        [HttpPost]
        [Route("getUsersPagedList")]
        public Result<PagedListResponse<UserDto>> GetUsersPagedList([FromBody]GetUsersPagedListRequest request)
        {
            return Executor.GetQuery<GetUsersPagedListQuery>().Process(q => q.Execute(request));
        }

        [HttpPost]
        [Route("getById")]
        public Result<UserDto> GetUserById([FromBody]Guid userId)
        {
            return Executor.GetQuery<GetUserByIdQuery>().Process(q => q.Execute(userId));
        }

        [HttpPost]
        [Route("create")]
        public Result<Guid> CreateUser([FromBody]UserDto userDto)
        {
            try
            {
                var user = userDto.ToUser();

                Executor.CommandChain()
                    .AddCommand<CreateUserCommand>(c => c.Execute(user))
                    .AddCommand<CreateUserClaimCommand>(c => c.Execute(UserClaimDto.UserNameClaim(userDto, user.Id)))
                    .AddCommand<CreateUserClaimCommand>(c => c.Execute(UserClaimDto.UserRoleClaim(userDto, user.Id)))
                    .AddCommand<CreateUserClaimCommand>(c => c.Execute(UserClaimDto.UserGenderClaim(userDto, user.Id)))
                    .ExecuteAllWithTransaction();

                return Result<Guid>.Ok(user.Id);
            }
            catch (Exception exception)
            {
                return Result<Guid>.Fail(Guid.Empty, $"{exception.Message}, {exception.StackTrace}");
            }
        }

        [HttpPost]
        [Route("update")]
        public Result<bool> UpdateUser([FromBody]UserDto userDto)
        {
            try
            {
                Executor.CommandChain()
                    .AddCommand<UpdateUserCommand>(c => c.Execute(userDto))
                    .AddCommand<UpdateUserClaimCommand>(c => c.Execute(UserClaimDto.UserNameClaim(userDto)))
                    .AddCommand<UpdateUserClaimCommand>(c => c.Execute(UserClaimDto.UserRoleClaim(userDto)))
                    .AddCommand<UpdateUserClaimCommand>(c => c.Execute(UserClaimDto.UserGenderClaim(userDto)))
                    .ExecuteAllWithTransaction();

                return Result<bool>.Ok(true);
            }
            catch (Exception exception)
            {
                return Result<bool>.Fail(false, $"{exception.Message}, {exception.StackTrace}");
            }

            
        }

        [HttpPost]
        [Route("delete")]
        public Result<bool> DeleteUser([FromBody]Guid userId)
        {
            return Executor.GetCommand<DeleteUserCommand>().Process(c => c.Execute(userId));
        }
    }
}
