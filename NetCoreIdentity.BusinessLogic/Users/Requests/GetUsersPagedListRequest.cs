using System;
using NetCoreDataAccess.BaseRequests;

namespace NetCoreIdentity.BusinessLogic.Users.Requests
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

        public bool HasRequestFirstName => string.IsNullOrEmpty(FirstName) == false;
        public bool HasRequestMiddleName => string.IsNullOrEmpty(SecondName) == false;
        public bool HasRequestSecondName => string.IsNullOrEmpty(FirstName) == false;
        public bool HasRequestPosition => string.IsNullOrEmpty(Position) == false;
        public bool HasRequestInn => string.IsNullOrEmpty(Inn) == false;
        public bool HasRequestBirthdate => Birthdate.HasValue;
        public bool HasRequestAddress => string.IsNullOrEmpty(Address) == false;
        public bool HasRequestIsActive => IsActive.HasValue;
    }
}