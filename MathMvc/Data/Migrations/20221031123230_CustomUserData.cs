using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathMvc.Data.Migrations
{
    public partial class CustomUserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberResolvedAccounts",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberUnresolvedAccounts",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberResolvedAccounts",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NumberUnresolvedAccounts",
                table: "AspNetUsers");
        }
    }
}
