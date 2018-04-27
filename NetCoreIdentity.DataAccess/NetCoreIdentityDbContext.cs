using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetCoreDataAccess;

namespace NetCoreIdentity.DataAccess
{
    public class NetCoreIdentityDbContext: DbContext
    {
        public NetCoreIdentityDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
    }

    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SecondName { get; set; }
        public string Position { get; set; }
        public string Inn { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserClaim : IdentityUserClaim<Guid>
    {
    }
}
