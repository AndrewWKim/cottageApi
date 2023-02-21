using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class SavingMeterDataRange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MeterDataBegin",
                table: "CommunalBills",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MeterDataEnd",
                table: "CommunalBills",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeterDataBegin",
                table: "CommunalBills");

            migrationBuilder.DropColumn(
                name: "MeterDataEnd",
                table: "CommunalBills");
        }
    }
}
