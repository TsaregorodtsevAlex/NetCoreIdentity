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
        public bool IsActive { get; set; }
    }

    public class UserClaim : IdentityUserClaim<Guid>
    {
    }
}
