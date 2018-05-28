using System;
using Microsoft.AspNetCore.Mvc;
using NetCoreDataAccess.BaseResponses;
using NetCoreDomain;
using NetCoreIdentity.BusinessLogic.Roles;
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
                if (CanSaveUserValidation(userDto, out var failResult))
                {
                    return failResult;
                }

                var user = userDto.CreateUser();

                var roleName = "";
                if (userDto.Role != null)
                {
                    var roleNameResult = Executor.GetQuery<GetRoleByIdQuery>().Process(q => q.Execute(userDto.Role.Id));
                    if (roleNameResult.IsFailure)
                    {
                        return Result<Guid>.Fail(Guid.Empty, "Выбранная роль не найдена");
                    }

                    roleName = roleNameResult.Value.Name;
                }

                Executor.CommandChain()
                    .AddCommand<CreateUserCommand>(c => c.Execute(user))
                    .AddCommand<CreateUserClaimCommand>(c => c.Execute(UserClaimDto.UserNameClaim(userDto, user.Id)))
                    .AddCommand<CreateUserClaimCommand>(c => c.Execute(UserClaimDto.UserRoleClaim(roleName, user.Id)))
                    .AddCommand<CreateUserClaimCommand>(c => c.Execute(UserClaimDto.UserGenderClaim(userDto, user.Id)))
                    .ExecuteAllWithTransaction();

                return Result<Guid>.Ok(user.Id);
            }
            catch (Exception exception)
            {
                return Result<Guid>.Fail(Guid.Empty, $"{exception.Message}, {exception.StackTrace}");
            }
        }

        private bool CanSaveUserValidation(UserDto userDto, out Result<Guid> fail)
        {
            fail = null;

            var isInnAlreadyExistsResult = Executor.GetQuery<IsUserInnAlreadyExistsQuery>().Process(q => q.Execute(userDto));
            if (isInnAlreadyExistsResult.IsFailure)
            {
                {
                    fail = Result<Guid>.Fail(Guid.Empty, isInnAlreadyExistsResult.Error);
                    return true;
                }
            }

            if (isInnAlreadyExistsResult.Value)
            {
                {
                    fail = Result<Guid>.Fail(Guid.Empty, "Пользователь с таким иин уже существует");
                    return true;
                }
            }

            return false;
        }

        [HttpPost]
        [Route("update")]
        public Result<bool> UpdateUser([FromBody]UserDto userDto)
        {
            try
            {
                if (CanUpdateUserValidation(userDto, out var failResult))
                {
                    return failResult;
                }

                var roleName = "";
                if (userDto.Role != null)
                {
                    var roleNameResult = Executor.GetQuery<GetRoleByIdQuery>().Process(q => q.Execute(userDto.Role.Id));
                    if (roleNameResult.IsFailure)
                    {
                        return Result<bool>.Fail(false, "Выбранная роль не найдена");
                    }

                    roleName = roleNameResult.Value.Name;
                }

                Executor.CommandChain()
                    .AddCommand<UpdateUserCommand>(c => c.Execute(userDto))
                    .AddCommand<UpdateUserClaimCommand>(c => c.Execute(UserClaimDto.UserNameClaim(userDto)))
                    .AddCommand<UpdateUserClaimCommand>(c => c.Execute(UserClaimDto.UserRoleClaim(roleName, userDto.Id)))
                    .AddCommand<UpdateUserClaimCommand>(c => c.Execute(UserClaimDto.UserGenderClaim(userDto)))
                    .ExecuteAllWithTransaction();

                return Result<bool>.Ok(true);
            }
            catch (Exception exception)
            {
                return Result<bool>.Fail(false, $"{exception.Message}, {exception.StackTrace}");
            }
        }

        private bool CanUpdateUserValidation(UserDto userDto, out Result<bool> fail)
        {
            fail = null;

            var isInnAlreadyExistsResult = Executor.GetQuery<IsUserInnAlreadyExistsQuery>().Process(q => q.Execute(userDto));
            if (isInnAlreadyExistsResult.IsFailure)
            {
                {
                    fail = Result<bool>.Fail(false, isInnAlreadyExistsResult.Error);
                    return true;
                }
            }

            if (isInnAlreadyExistsResult.Value)
            {
                {
                    fail = Result<bool>.Fail(false, "Пользователь с таким иин уже существует");
                    return true;
                }
            }

            return false;
        }

        [HttpPost]
        [Route("delete")]
        public Result<bool> DeleteUser([FromBody]Guid userId)
        {
            return Executor.GetCommand<DeleteUserCommand>().Process(c => c.Execute(userId));
        }
    }
}
