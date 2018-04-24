using System;
using NetCoreIdentity.DataAccess;
using NetCoreIdentity.DataAccess.Enums;

namespace NetCoreIdentity.Controllers.Account.Users
{
    public class UserRegistrationModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SecondName { get; set; }

        public GenderType Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }

        public User ToUser =>
            new User
            {
                UserName = FirstName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                PasswordHash = Password,
                AccessFailedCount = 5,
                EmailConfirmed = true,
                IsActive = true,
                Id = Guid.NewGuid(),
                NormalizedEmail = Email,
                NormalizedUserName = FirstName,
                PhoneNumberConfirmed = true
            };
    }
}