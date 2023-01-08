using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CogniSmiles.Migrations
{
    public partial class DoctorTableChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddedDate",
                table: "Doctor",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateComment",
                table: "Doctor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Doctor",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedDate",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "UpdateComment",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Doctor");
        }
    }
}
