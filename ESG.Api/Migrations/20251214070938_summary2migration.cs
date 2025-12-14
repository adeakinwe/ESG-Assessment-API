using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESG.Api.Migrations
{
    /// <inheritdoc />
    public partial class summary2migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ESG_CHECKLIST_SUMMARY",
                columns: table => new
                {
                    SUMMARYID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LOANAPPLICATIONID = table.Column<int>(type: "int", nullable: false),
                    TOTALSCORE = table.Column<double>(type: "double", nullable: false),
                    TOTALWEIGHT = table.Column<double>(type: "double", nullable: false),
                    RATINGID = table.Column<int>(type: "int", nullable: false),
                    COMMENT_ = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CREATEDBY = table.Column<int>(type: "int", nullable: false),
                    DATETIMECREATED = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESG_CHECKLIST_SUMMARY", x => x.SUMMARYID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ESG_CHECKLIST_SUMMARY");
        }
    }
}
