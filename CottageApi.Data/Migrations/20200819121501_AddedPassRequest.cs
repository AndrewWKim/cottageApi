using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class AddedPassRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PassRequests_Cars_CarId",
                table: "PassRequests");

            migrationBuilder.DropIndex(
                name: "IX_PassRequests_CarId",
                table: "PassRequests");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "PassRequests");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInfo",
                table: "PassRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarBrand",
                table: "PassRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarLicensePlate",
                table: "PassRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarModel",
                table: "PassRequests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "PassRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VisitorName",
                table: "PassRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PassRequests_ClientId",
                table: "PassRequests",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_PassRequests_Clients_ClientId",
                table: "PassRequests",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PassRequests_Clients_ClientId",
                table: "PassRequests");

            migrationBuilder.DropIndex(
                name: "IX_PassRequests_ClientId",
                table: "PassRequests");

            migrationBuilder.DropColumn(
                name: "AdditionalInfo",
                table: "PassRequests");

            migrationBuilder.DropColumn(
                name: "CarBrand",
                table: "PassRequests");

            migrationBuilder.DropColumn(
                name: "CarLicensePlate",
                table: "PassRequests");

            migrationBuilder.DropColumn(
                name: "CarModel",
                table: "PassRequests");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "PassRequests");

            migrationBuilder.DropColumn(
                name: "VisitorName",
                table: "PassRequests");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "PassRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PassRequests_CarId",
                table: "PassRequests",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_PassRequests_Cars_CarId",
                table: "PassRequests",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
