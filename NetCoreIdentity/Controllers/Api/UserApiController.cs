using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NetCoreDataAccess.BaseResponses;
using NetCoreDomain;
using NetCoreIdentity.BusinessLogic.Users;
using NetCoreIdentity.BusinessLogic.Users.Dtos;
using NetCoreIdentity.BusinessLogic.Users.Requests;

namespace NetCoreIdentity.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/users")]
    public class UserApiController : BaseApiController
    {
        // GET: api/UserApi
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

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
            return Executor.GetQuery<CreateUserCommand>().Process(q => q.Execute(userDto));
        }

        [HttpPost]
        [Route("update")]
        public Result<bool> UpdateUser([FromBody]UserDto userDto)
        {
            return Executor.GetQuery<UpdateUserCommand>().Process(q => q.Execute(userDto));
        }

        [HttpPost]
        [Route("delete")]
        public Result<bool> DeleteUser([FromBody]Guid userId)
        {
            return Executor.GetQuery<DeleteUserCommand>().Process(q => q.Execute(userId));
        }

        //// POST: api/UserApi
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/UserApi/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
