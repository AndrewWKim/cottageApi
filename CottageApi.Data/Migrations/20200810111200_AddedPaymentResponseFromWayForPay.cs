using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class AddedPaymentResponseFromWayForPay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentResponses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MerchantAccount = table.Column<string>(nullable: true),
                    OrderReference = table.Column<string>(nullable: true),
                    MerchantSignature = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    Currency = table.Column<string>(nullable: true),
                    AuthCode = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<string>(nullable: true),
                    ProcessingDate = table.Column<string>(nullable: true),
                    CardPan = table.Column<string>(nullable: true),
                    CardType = table.Column<string>(nullable: true),
                    IssuerBankCountry = table.Column<string>(nullable: true),
                    IssuerBankName = table.Column<string>(nullable: true),
                    RecToken = table.Column<string>(nullable: true),
                    TransactionStatus = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    ReasonCode = table.Column<int>(nullable: false),
                    Fee = table.Column<double>(nullable: false),
                    PaymentSystem = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentResponses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentResponses");
        }
    }
}
