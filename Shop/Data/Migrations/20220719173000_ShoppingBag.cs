using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.Data.Migrations
{
    public partial class ShoppingBag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingBag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingBag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingBag_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemShoppingBag",
                columns: table => new
                {
                    ItemsId = table.Column<int>(type: "int", nullable: false),
                    ShoppingBagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemShoppingBag", x => new { x.ItemsId, x.ShoppingBagsId });
                    table.ForeignKey(
                        name: "FK_ItemShoppingBag_Items_ItemsId",
                        column: x => x.ItemsId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemShoppingBag_ShoppingBag_ShoppingBagsId",
                        column: x => x.ShoppingBagsId,
                        principalTable: "ShoppingBag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemShoppingBag_ShoppingBagsId",
                table: "ItemShoppingBag",
                column: "ShoppingBagsId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingBag_ProfileId",
                table: "ShoppingBag",
                column: "ProfileId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemShoppingBag");

            migrationBuilder.DropTable(
                name: "ShoppingBag");
        }
    }
}
