using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NetCoreDomain;
using NetCoreIdentity.BusinessLogic.Roles;
using NetCoreIdentity.BusinessLogic.Roles.Dtos;

namespace NetCoreIdentity.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/roles")]
    public class RoleController : BaseApiController
    {
        // GET: api/Role
        [HttpPost]
        [Route("getRoles")]
        public Result<List<RoleDto>> GetRoles()
        {
            return Executor.GetQuery<GetRolesQuery>().Process(q => q.Execute());
        }
    }
}
