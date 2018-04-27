using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NetCoreIdentity.DataAccess.Migrations
{
    public partial class Birthdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthdate",
                table: "Users",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthdate",
                table: "Users",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
