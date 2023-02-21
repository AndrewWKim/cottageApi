using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class ChangeTwoCountsToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardCreateTryCount",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PaymentTryCount",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "WayForPayRequestTryCount",
                table: "Clients",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WayForPayRequestTryCount",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "CardCreateTryCount",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentTryCount",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
