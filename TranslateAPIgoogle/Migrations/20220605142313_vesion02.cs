using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TranslateAPIgoogle.Migrations
{
    public partial class vesion02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOrder",
                table: "orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOrder",
                table: "orders");
        }
    }
}
