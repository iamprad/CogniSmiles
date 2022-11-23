using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CogniSmiles.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SurgicalGuideWrittenDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImplantSite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImplantSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImplantDiameter = table.Column<int>(type: "int", nullable: true),
                    InmplantLength = table.Column<int>(type: "int", nullable: true),
                    DicomFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StlIosFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientStatus = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patient");
        }
    }
}
