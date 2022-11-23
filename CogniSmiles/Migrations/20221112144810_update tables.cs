using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CogniSmiles.Migrations
{
    public partial class updatetables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Login");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Login",
                newName: "CreatedOn");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivatedOn",
                table: "Login",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Login",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Login",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivatedOn",
                table: "Login");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Login");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Login");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Login",
                newName: "UpdatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Login",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
