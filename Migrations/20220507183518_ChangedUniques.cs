using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class ChangedUniques : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_UserName",
                table: "Users");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Brands_Name",
                table: "Brands");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_Email",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Name",
                table: "Brands",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Brands_Name",
                table: "Brands");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_UserName",
                table: "Users",
                column: "UserName");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Brands_Name",
                table: "Brands",
                column: "Name");
        }
    }
}
