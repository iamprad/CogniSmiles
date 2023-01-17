using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CogniSmiles.Migrations
{
    public partial class renamecolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SurgicalGuideWrittenDate",
                table: "Patient",
                newName: "SurgicalGuideReturnDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SurgicalGuideReturnDate",
                table: "Patient",
                newName: "SurgicalGuideWrittenDate");
        }
    }
}
