using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class AddedCardTypeAndCardCreateResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardType",
                table: "Cards",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CardCreateResponses",
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
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<string>(nullable: true),
                    ProcessingDate = table.Column<string>(nullable: true),
                    CardPan = table.Column<string>(nullable: true),
                    CardType = table.Column<string>(nullable: true),
                    IssuerBankCountry = table.Column<string>(nullable: true),
                    TransactionStatus = table.Column<string>(nullable: true),
                    Fee = table.Column<double>(nullable: false),
                    ReasonCode = table.Column<int>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    RecToken = table.Column<string>(nullable: true),
                    PaymentSystem = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardCreateResponses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardCreateResponses");

            migrationBuilder.DropColumn(
                name: "CardType",
                table: "Cards");
        }
    }
}
