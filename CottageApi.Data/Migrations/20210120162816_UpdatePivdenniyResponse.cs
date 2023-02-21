using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class UpdatePivdenniyResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "PivdenniyPaymentResponses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HostCode",
                table: "PivdenniyPaymentResponses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "PivdenniyPaymentResponses");

            migrationBuilder.DropColumn(
                name: "HostCode",
                table: "PivdenniyPaymentResponses");
        }
    }
}
