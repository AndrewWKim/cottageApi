using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class ChangeBillingEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingDate",
                table: "CommunalBills");

            migrationBuilder.DropColumn(
                name: "Identifier1C",
                table: "CommunalBills");

            migrationBuilder.AddColumn<string>(
                name: "BillGUID",
                table: "CommunalBills",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillGUID",
                table: "CommunalBills");

            migrationBuilder.AddColumn<DateTime>(
                name: "BillingDate",
                table: "CommunalBills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Identifier1C",
                table: "CommunalBills",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
