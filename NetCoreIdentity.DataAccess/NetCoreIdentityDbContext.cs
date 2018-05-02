using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreDataAccess.Interfaces;

namespace NetCoreIdentity.DataAccess
{
    public class NetCoreIdentityDbContext : DbContext
    {
        public NetCoreIdentityDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserRoleClaim> UserRoleClaims { get; set; }
        public DbSet<Role> Roles { get; set; }
    }

    public class User : IdentityUser<Guid>, IDeletable
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SecondName { get; set; }
        public string Position { get; set; }
        public string Inn { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public List<UserRole> UserRoles { get; set; }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }
    }

    public class UserClaim : IdentityUserClaim<Guid>, IDeletable
    {
        public bool IsDeleted { get; set; }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }
    }

    public class UserRole : IdentityUserRole<Guid>, IDeletable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }

        public bool IsDeleted { get; set; }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }
    }

    public class UserRoleClaim : IdentityRoleClaim<Guid>, IDeletable
    {
        public bool IsDeleted { get; set; }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }
    }

    public class Role : IdentityRole<Guid>, IDeletable
    {
        public bool IsDeleted { get; set; }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }
    }
}