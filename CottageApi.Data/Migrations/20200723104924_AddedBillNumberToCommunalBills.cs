using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class AddedBillNumberToCommunalBills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BillNumber",
                table: "CommunalBills",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillNumber",
                table: "CommunalBills");
        }
    }
}
