using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class AddedPivdenniyPaymentResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PivdenniyPaymentResponses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseTime = table.Column<string>(nullable: true),
                    ProxyPan = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    ApprovalCode = table.Column<string>(nullable: true),
                    MerchantID = table.Column<string>(nullable: true),
                    OrderID = table.Column<string>(nullable: true),
                    Signature = table.Column<string>(nullable: true),
                    Rrn = table.Column<string>(nullable: true),
                    XID = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    SD = table.Column<string>(nullable: true),
                    TranCode = table.Column<string>(nullable: true),
                    TerminalID = table.Column<string>(nullable: true),
                    TotalAmount = table.Column<string>(nullable: true),
                    Delay = table.Column<string>(nullable: true),
                    UPCToken = table.Column<string>(nullable: true),
                    UPCTokenExp = table.Column<string>(nullable: true),
                    ResponseAction = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PivdenniyPaymentResponses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PivdenniyPaymentResponses");
        }
    }
}
