using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AFIAT.TST.Migrations
{
    public partial class Regenerated_Category9802 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Categories",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "Categories",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "Categories");
        }
    }
}
