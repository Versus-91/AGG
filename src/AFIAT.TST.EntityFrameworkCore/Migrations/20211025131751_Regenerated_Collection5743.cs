using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AFIAT.TST.Migrations
{
    public partial class Regenerated_Collection5743 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Collections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Collections",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Collections",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "Collections",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "Collections");
        }
    }
}
