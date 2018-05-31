using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NetCoreIdentity.DataAccess.Migrations.DatabaseSeeding
{
    public partial class RolesSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder != null)
            {
                var tableName = typeof(Role).Name + "s";
                var tableFields = new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "IsDeleted" };

                migrationBuilder.InsertData(tableName, tableFields,
                    new object[] { Guid.NewGuid(), null, "Administrator", "Администратор", false });
                migrationBuilder.InsertData(tableName, tableFields,
                    new object[] { Guid.NewGuid(), null, "Chairman", "Председатель", false });
                migrationBuilder.InsertData(tableName, tableFields,
                    new object[] { Guid.NewGuid(), null, "Employee", "Экзаменующийся", false });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
