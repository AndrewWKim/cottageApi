using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class FIxedPivdenniyEffortCardPRoperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardExpMonth",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardExpYear",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardMask",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Cards");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "PivdenniyPaymentEfforts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CardPan",
                table: "Cards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardToken",
                table: "Cards",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "PivdenniyPaymentEfforts");

            migrationBuilder.DropColumn(
                name: "CardPan",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardToken",
                table: "Cards");

            migrationBuilder.AddColumn<string>(
                name: "CardExpMonth",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardExpYear",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardMask",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
