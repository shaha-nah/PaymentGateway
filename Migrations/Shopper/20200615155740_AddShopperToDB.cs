using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway.Migrations.Shopper
{
    public partial class AddShopperToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shopper",
                columns: table => new
                {
                    shopperID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cardNumber = table.Column<string>(nullable: true),
                    credit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shopper", x => x.shopperID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shopper");
        }
    }
}
