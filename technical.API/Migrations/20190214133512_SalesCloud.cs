using Microsoft.EntityFrameworkCore.Migrations;

namespace technical.API.Migrations
{
    public partial class SalesCloud : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sells",
                columns: table => new
                {
                    SellerId = table.Column<int>(nullable: false),
                    AssetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sells", x => new { x.SellerId, x.AssetId });
                    table.ForeignKey(
                        name: "FK_Sells_Users_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sells_Users_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sells_AssetId",
                table: "Sells",
                column: "AssetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sells");
        }
    }
}
