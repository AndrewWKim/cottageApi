using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class AddedITNToCLient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillNumber",
                table: "CommunalBills");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "CommunalBills");

            migrationBuilder.AddColumn<string>(
                name: "BillNumber",
                table: "CottageBillings",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "CottageBillings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "CommunalBills",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ITN",
                table: "Clients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillNumber",
                table: "CottageBillings");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "CottageBillings");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CommunalBills");

            migrationBuilder.DropColumn(
                name: "ITN",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "BillNumber",
                table: "CommunalBills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "CommunalBills",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
