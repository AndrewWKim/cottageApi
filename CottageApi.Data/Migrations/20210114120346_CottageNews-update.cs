using Microsoft.EntityFrameworkCore.Migrations;

namespace CottageApi.Data.Migrations
{
    public partial class CottageNewsupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "CottageNews");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CottageNews",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "CottageNews");

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "CottageNews",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
