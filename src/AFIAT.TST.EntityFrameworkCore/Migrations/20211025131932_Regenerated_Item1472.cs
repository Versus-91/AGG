using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AFIAT.TST.Migrations
{
    public partial class Regenerated_Item1472 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Items",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Items",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Items",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "Items",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "Items");
        }
    }
}
