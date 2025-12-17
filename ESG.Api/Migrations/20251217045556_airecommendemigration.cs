using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESG.Api.Migrations
{
    /// <inheritdoc />
    public partial class airecommendemigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DATETIMEUPDATED",
                table: "ESG_AI_RECOMMENDATION",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LASTUPDATEDBY",
                table: "ESG_AI_RECOMMENDATION",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DATETIMEUPDATED",
                table: "ESG_AI_RECOMMENDATION");

            migrationBuilder.DropColumn(
                name: "LASTUPDATEDBY",
                table: "ESG_AI_RECOMMENDATION");
        }
    }
}
