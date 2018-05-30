using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace NetCoreIdentity.DataAccess.Migrations.DatabaseSeeding
{
    [DbContext(typeof(NetCoreIdentityDbContext))]
    [Migration("RolesSeeding")]
    public class RolesSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
            {
                var tableName = typeof(Role).Name + "s";
                var tableFields = new[] { "Id", "Name", "NormalizedName" };

                migrationBuilder.InsertData(tableName, tableFields, new object[] { Guid.NewGuid(), "Administrator", "Администратор" });
                migrationBuilder.InsertData(tableName, tableFields, new object[] { Guid.NewGuid(), "Chairman", "Председатель" });
                migrationBuilder.InsertData(tableName, tableFields, new object[] { Guid.NewGuid(), "Employee", "Экзаменующийся" });
            }
        }
    }
}
