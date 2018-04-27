using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NetCoreIdentity.DataAccess.Migrations
{
    public partial class ExtendUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Inn",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondName",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Inn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecondName",
                table: "Users");
        }
    }
}
