using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CogniSmiles.Migrations
{
    public partial class addedcolumnpatientfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ScanTakenDate",
                table: "PatientFile",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScanTakenDate",
                table: "PatientFile");
        }
    }
}
