using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NetCoreDataAccess.BaseResponses;
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
        public PagedListResponse<UserDto> GetUsersPagedList([FromBody]GetUsersPagedListRequest request)
        {
            return Executor.GetQuery<GetUsersPagedListQuery>().Process(q => q.Execute(request));
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
