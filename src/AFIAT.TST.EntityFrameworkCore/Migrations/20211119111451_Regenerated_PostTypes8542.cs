using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AFIAT.TST.Migrations
{
    public partial class Regenerated_PostTypes8542 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "PostTypeses",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "PostTypeses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "PostTypeses",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "PostTypeses",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "PostTypeses");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "PostTypeses");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "PostTypeses");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "PostTypeses");
        }
    }
}
