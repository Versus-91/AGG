using Microsoft.EntityFrameworkCore.Migrations;

namespace AFIAT.TST.Migrations
{
    public partial class Regenerated_Post3040 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostTypesId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostTypesId",
                table: "Posts",
                column: "PostTypesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_PostTypeses_PostTypesId",
                table: "Posts",
                column: "PostTypesId",
                principalTable: "PostTypeses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PostTypeses_PostTypesId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PostTypesId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostTypesId",
                table: "Posts");
        }
    }
}
