using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class RefactorIdeaRead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdeaReads_Users_UserId",
                table: "IdeaReads");

            migrationBuilder.DropIndex(
                name: "IX_IdeaReads_UserId",
                table: "IdeaReads");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_IdeaReads_UserId",
                table: "IdeaReads",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_IdeaReads_Users_UserId",
                table: "IdeaReads",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
