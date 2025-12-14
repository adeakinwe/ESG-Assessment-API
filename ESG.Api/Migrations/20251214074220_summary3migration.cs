using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESG.Api.Migrations
{
    /// <inheritdoc />
    public partial class summary3migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AVGSCORE",
                table: "ESG_CHECKLIST_SUMMARY",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AVGSCORE",
                table: "ESG_CHECKLIST_SUMMARY");
        }
    }
}
