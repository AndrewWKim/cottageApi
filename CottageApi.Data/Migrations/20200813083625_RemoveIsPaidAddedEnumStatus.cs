using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class RemoveIsPaidAddedEnumStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "CommunalBills");

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "CommunalBills",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "CommunalBills");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "CommunalBills",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
