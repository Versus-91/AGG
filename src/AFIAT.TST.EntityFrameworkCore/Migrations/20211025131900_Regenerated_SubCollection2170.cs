using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AFIAT.TST.Migrations
{
    public partial class Regenerated_SubCollection2170 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "SubCollections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "SubCollections",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "SubCollections",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "SubCollections",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "SubCollections");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "SubCollections");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "SubCollections");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "SubCollections");
        }
    }
}
