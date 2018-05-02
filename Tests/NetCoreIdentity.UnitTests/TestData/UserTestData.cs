using System;
using NetCoreIdentity.BusinessLogic.Users.Dtos;

namespace NetCoreIdentity.UnitTests.TestData
{
    public class UserTestData
    {
        public static UserDto TestUserDto = new UserDto
        {
            FirstName = "Alex",
            MiddleName = "Alexovich",
            SecondName = "Alexov",
            PhoneNumber = "+791345878965",
            Email = "test@test.kz",
            Address = "Test address",
            Birthdate = DateTime.Now,
            Inn = "1312312313",
            Position = "Director"
        };
    }
}