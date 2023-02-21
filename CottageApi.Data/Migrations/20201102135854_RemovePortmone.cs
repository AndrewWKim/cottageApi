using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class RemovePortmone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardCreateResponses");

            migrationBuilder.DropTable(
                name: "PaymentResponses");

            migrationBuilder.DropColumn(
                name: "WayForPayRequestTryCount",
                table: "Clients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WayForPayRequestTryCount",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CardCreateResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    AuthCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardPan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fee = table.Column<double>(type: "float", nullable: false),
                    IssuerBankCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MerchantAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MerchantSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessingDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonCode = table.Column<int>(type: "int", nullable: false),
                    RecToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardCreateResponses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    AuthCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardPan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fee = table.Column<double>(type: "float", nullable: false),
                    IssuerBankCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuerBankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MerchantAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MerchantSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessingDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonCode = table.Column<int>(type: "int", nullable: false),
                    RecToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentResponses", x => x.Id);
                });
        }
    }
}
