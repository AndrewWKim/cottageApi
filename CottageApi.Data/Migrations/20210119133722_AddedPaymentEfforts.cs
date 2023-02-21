using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class AddedPaymentEfforts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "PivdenniyPaymentResponses",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PivdenniyPaymentEfforts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(nullable: false),
                    CommunalBillId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PivdenniyPaymentEfforts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PivdenniyPaymentEfforts_CommunalBills_CommunalBillId",
                        column: x => x.CommunalBillId,
                        principalTable: "CommunalBills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PivdenniyPaymentEfforts_CommunalBillId",
                table: "PivdenniyPaymentEfforts",
                column: "CommunalBillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PivdenniyPaymentEfforts");

            migrationBuilder.AlterColumn<string>(
                name: "OrderID",
                table: "PivdenniyPaymentResponses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
