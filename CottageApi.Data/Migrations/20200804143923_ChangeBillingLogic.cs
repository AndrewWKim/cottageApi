using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class ChangeBillingLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunalBills_CommunalTypes_CommunalTypeId",
                table: "CommunalBills");

            migrationBuilder.DropTable(
                name: "CommunalTypes");

            migrationBuilder.DropIndex(
                name: "IX_CommunalBills_CommunalTypeId",
                table: "CommunalBills");

            migrationBuilder.DropColumn(
                name: "BillNumber",
                table: "CottageBillings");

            migrationBuilder.DropColumn(
                name: "CommunalTypeId",
                table: "CommunalBills");

            migrationBuilder.AddColumn<string>(
                name: "BillNumber",
                table: "CommunalBills",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BillingDate",
                table: "CommunalBills",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CommunalType",
                table: "CommunalBills",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Identifier1C",
                table: "CommunalBills",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillNumber",
                table: "CommunalBills");

            migrationBuilder.DropColumn(
                name: "BillingDate",
                table: "CommunalBills");

            migrationBuilder.DropColumn(
                name: "CommunalType",
                table: "CommunalBills");

            migrationBuilder.DropColumn(
                name: "Identifier1C",
                table: "CommunalBills");

            migrationBuilder.AddColumn<string>(
                name: "BillNumber",
                table: "CottageBillings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommunalTypeId",
                table: "CommunalBills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CommunalTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunalTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunalBills_CommunalTypeId",
                table: "CommunalBills",
                column: "CommunalTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunalBills_CommunalTypes_CommunalTypeId",
                table: "CommunalBills",
                column: "CommunalTypeId",
                principalTable: "CommunalTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
