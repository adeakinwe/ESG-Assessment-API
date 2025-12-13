using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESG.Api.Migrations
{
    /// <inheritdoc />
    public partial class assessmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RESPONSETYPEID",
                table: "ESG_CHECKLIST_ASSESSMENT",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "RESPONSETYPEID",
                table: "ESG_CHECKLIST_ASSESSMENT",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
