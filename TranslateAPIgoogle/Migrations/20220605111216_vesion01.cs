using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TranslateAPIgoogle.Migrations
{
    public partial class vesion01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_users_AddressID",
                table: "users",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_orders_UserID",
                table: "orders",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_UserID",
                table: "orders",
                column: "UserID",
                principalTable: "users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_addresses_AddressID",
                table: "users",
                column: "AddressID",
                principalTable: "addresses",
                principalColumn: "AddressID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_UserID",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_users_addresses_AddressID",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_AddressID",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_orders_UserID",
                table: "orders");
        }
    }
}
