using System;

namespace NetCoreIdentityHttpClient.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Account { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SecondName { get; set; }
        public string Position { get; set; }
        public string Inn { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public RoleDto Role { get; set; }
    }
}
