using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESG.Api.Migrations
{
    /// <inheritdoc />
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    public partial class assessmentmigration : Migration
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ESG_CHECKLIST_DETAIL");

            migrationBuilder.AddColumn<string>(
                name: "CATEGORY",
                table: "ESG_CHECKLIST_ITEM",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ESG_CHECKLIST_ASSESSMENT",
                columns: table => new
                {
                    CHECKLISTDETAILID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CHECKLISTITEMID = table.Column<int>(type: "int", nullable: false),
                    LOANAPPLICATIONID = table.Column<int>(type: "int", nullable: false),
                    RESPONSETYPEID = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SCORE = table.Column<int>(type: "int", nullable: false),
                    WEIGHT = table.Column<int>(type: "int", nullable: false),
                    COMMENT_ = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CREATEDBY = table.Column<int>(type: "int", nullable: false),
                    DATETIMECREATED = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESG_CHECKLIST_ASSESSMENT", x => x.CHECKLISTDETAILID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ESG_CHECKLIST_ASSESSMENT");

            migrationBuilder.DropColumn(
                name: "CATEGORY",
                table: "ESG_CHECKLIST_ITEM");

            migrationBuilder.CreateTable(
                name: "ESG_CHECKLIST_DETAIL",
                columns: table => new
                {
                    CHECKLISTDETAILID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CHECKLISTITEMID = table.Column<int>(type: "int", nullable: false),
                    COMMENT_ = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CREATEDBY = table.Column<int>(type: "int", nullable: false),
                    DATETIMECREATED = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LOANAPPLICATIONID = table.Column<int>(type: "int", nullable: false),
                    RESPONSETYPEID = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SCORE = table.Column<int>(type: "int", nullable: false),
                    WEIGHT = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESG_CHECKLIST_DETAIL", x => x.CHECKLISTDETAILID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
