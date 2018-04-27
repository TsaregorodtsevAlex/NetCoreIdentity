using System;
using System.Linq.Expressions;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users.Dtos
{
    public class UserDto
    {
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

        public static Expression<Func<User, UserDto>> Map = user => new UserDto
        {
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            SecondName = user.SecondName,
            Position = user.Position,
            Inn = user.Inn,
            Birthdate = user.Birthdate,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            IsActive = user.IsActive
        };
    }
}