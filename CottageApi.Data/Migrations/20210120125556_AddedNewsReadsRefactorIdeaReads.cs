using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class AddedNewsReadsRefactorIdeaReads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdeaReads_Clients_ClientId",
                table: "IdeaReads");

            migrationBuilder.DropIndex(
                name: "IX_IdeaReads_ClientId",
                table: "IdeaReads");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "IdeaReads");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "IdeaReads",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "NewsReads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CottageNewsId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsReads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsReads_CottageNews_CottageNewsId",
                        column: x => x.CottageNewsId,
                        principalTable: "CottageNews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsReads_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdeaReads_UserId",
                table: "IdeaReads",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsReads_CottageNewsId",
                table: "NewsReads",
                column: "CottageNewsId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsReads_UserId",
                table: "NewsReads",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_IdeaReads_Users_UserId",
                table: "IdeaReads",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdeaReads_Users_UserId",
                table: "IdeaReads");

            migrationBuilder.DropTable(
                name: "NewsReads");

            migrationBuilder.DropIndex(
                name: "IX_IdeaReads_UserId",
                table: "IdeaReads");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "IdeaReads");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "IdeaReads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IdeaReads_ClientId",
                table: "IdeaReads",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_IdeaReads_Clients_ClientId",
                table: "IdeaReads",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
