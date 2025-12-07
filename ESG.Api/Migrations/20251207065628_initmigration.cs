using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESG.Api.Migrations
{
    /// <inheritdoc />
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    public partial class initmigration : Migration
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CUSTOMER",
                columns: table => new
                {
                    CUSTOMERID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CUSTOMERCODE = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FIRSTNAME = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LASTNAME = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GENDER = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SECTOR = table.Column<int>(type: "int", nullable: false),
                    ADDRESS = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CREATEDBY = table.Column<int>(type: "int", nullable: false),
                    DATETIMECREATED = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUSTOMER", x => x.CUSTOMERID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ESG_CHECKLIST_DETAIL",
                columns: table => new
                {
                    CHECKLISTDETAILID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CHECKLISTITEMID = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ESG_CHECKLIST_DETAIL", x => x.CHECKLISTDETAILID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ESG_CHECKLIST_ITEM",
                columns: table => new
                {
                    CHECKLISTITEMID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CHECKLISTITEM = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RESPONSETYPEID = table.Column<int>(type: "int", nullable: false),
                    ISINUSE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    WEIGHT = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESG_CHECKLIST_ITEM", x => x.CHECKLISTITEMID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ESG_CHECKLIST_ITEM_SCORE",
                columns: table => new
                {
                    SCOREID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CHECKLISTITEMID = table.Column<int>(type: "int", nullable: false),
                    RESPONSETYPEID = table.Column<int>(type: "int", nullable: false),
                    SCORE = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESG_CHECKLIST_ITEM_SCORE", x => x.SCOREID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ESG_CHECKLIST_RESPONSE",
                columns: table => new
                {
                    RESPONSETYPEID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NAME = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESG_CHECKLIST_RESPONSE", x => x.RESPONSETYPEID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LOAN_APPLICATION",
                columns: table => new
                {
                    LOANAPPLICATIONID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CUSTOMERID = table.Column<int>(type: "int", nullable: false),
                    PRODUCTID = table.Column<int>(type: "int", nullable: false),
                    AMOUNT = table.Column<double>(type: "double", nullable: false),
                    TENOR = table.Column<int>(type: "int", nullable: false),
                    INTERESTRATE = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    LOANPURPOSE = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    APPLICATIONDATE = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CREATEDBY = table.Column<int>(type: "int", nullable: false),
                    DATETIMECREATED = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_APPLICATION", x => x.LOANAPPLICATIONID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CUSTOMER");

            migrationBuilder.DropTable(
                name: "ESG_CHECKLIST_DETAIL");

            migrationBuilder.DropTable(
                name: "ESG_CHECKLIST_ITEM");

            migrationBuilder.DropTable(
                name: "ESG_CHECKLIST_ITEM_SCORE");

            migrationBuilder.DropTable(
                name: "ESG_CHECKLIST_RESPONSE");

            migrationBuilder.DropTable(
                name: "LOAN_APPLICATION");
        }
    }
}
