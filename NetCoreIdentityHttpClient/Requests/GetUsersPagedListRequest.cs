using System;
using NetCoreDataAccess.BaseRequests;

namespace NetCoreIdentityHttpClient.Requests
{
    public class GetUsersPagedListRequest : PagedListRequest
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SecondName { get; set; }
        public string Position { get; set; }
        public string Inn { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Address { get; set; }
        public bool? IsActive { get; set; }
        public Guid? RoleId { get; set; }
    }
}