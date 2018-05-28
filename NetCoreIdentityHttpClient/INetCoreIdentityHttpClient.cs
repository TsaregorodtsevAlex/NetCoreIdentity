using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetCoreDataAccess.BaseResponses;
using NetCoreDomain;
using NetCoreIdentityHttpClient.Dtos;
using NetCoreIdentityHttpClient.Requests;

namespace NetCoreIdentityHttpClient
{
    public interface INetCoreIdentityHttpClient
    {
        ValueTask<Result<PagedListResponse<UserDto>>> GetUsersPagedList(GetUsersPagedListRequest usersPagedListRequest);
        ValueTask<Result<List<UserDto>>> GetUsersByRole(string roleName);
        ValueTask<Result<UserDto>> GetUserById(Guid userId);
        ValueTask<Result<Guid>> CreateUser(UserDto userDto);
        ValueTask<Result<bool>> UpdateUser(UserDto userDto);
        ValueTask<Result<bool>> DeleteUser(Guid userId);

        ValueTask<Result<List<RoleDto>>> GetRoles();
    }
}