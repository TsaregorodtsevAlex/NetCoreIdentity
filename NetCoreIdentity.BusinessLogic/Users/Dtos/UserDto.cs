using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NetCoreIdentity.BusinessLogic.Roles.Dtos;
using NetCoreIdentity.DataAccess;

namespace NetCoreIdentity.BusinessLogic.Users.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
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

        public static Func<User, UserDto> MapFromUser = user => new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            SecondName = user.SecondName,
            Position = user.Position,
            Inn = user.Inn,
            Birthdate = user.Birthdate,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            IsActive = user.IsActive,
            IsDeleted = user.IsDeleted,

            Role = user.UserRoles.Where(r => r.IsDeleted == false).Select(RoleDto.Map).FirstOrDefault()
        };

        public static Expression<Func<User, UserDto>> Map = user => new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            SecondName = user.SecondName,
            Position = user.Position,
            Inn = user.Inn,
            Birthdate = user.Birthdate,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            IsActive = user.IsActive,
            IsDeleted = user.IsDeleted,

            Role = user.UserRoles.Where(r => r.IsDeleted == false).Select(RoleDto.Map).FirstOrDefault()
        };

        public void UpdateUser(User user)
        {
            user.FirstName = FirstName;
            user.MiddleName = MiddleName;
            user.SecondName = SecondName;
            user.Position = Position;
            user.Inn = Inn;
            user.Birthdate = Birthdate;
            user.Address = Address;
            user.PhoneNumber = PhoneNumber;
            user.Email = Email;
            user.IsActive = IsActive;
            user.IsDeleted = IsDeleted;

            var userRole = user.UserRoles.FirstOrDefault(r => r.RoleId == Role.Id && r.IsDeleted == false);
            if (userRole != null)
            {
                return;
            }

            var activeUserRoles = user.UserRoles.Where(r => r.UserId == user.Id && r.IsDeleted == false);
            foreach (var activeUserRole in activeUserRoles)
            {
                activeUserRole.MarkAsDeleted();
            }

            var newUserRole = Role.ToUserRole();

            if (user.UserRoles == null)
            {
                user.UserRoles = new List<UserRole>();
            }

            user.UserRoles.Add(newUserRole);
        }

        public User ToUser()
        {
            var user = new User
            {
                FirstName = FirstName,
                MiddleName = MiddleName,
                SecondName = SecondName,
                Position = Position,
                Inn = Inn,
                Birthdate = Birthdate,
                Address = Address,
                PhoneNumber = PhoneNumber,
                Email = Email,
                IsActive = IsActive,
                IsDeleted = IsDeleted,
                UserRoles = new List<UserRole>()
            };

            if (Role != null)
            {
                user.UserRoles.Add(Role.ToUserRole());
            }

            return user;
        }

        public string UserFullName => $"{SecondName} {FirstName} {MiddleName}";
    }
}
