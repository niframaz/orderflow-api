using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMoneyValueObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "PriceAmount");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "OrderItems",
                newName: "UnitPriceAmount");

            migrationBuilder.AddColumn<string>(
                name: "PriceCurrency",
                table: "Products",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UnitPriceCurrency",
                table: "OrderItems",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceCurrency",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitPriceCurrency",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "PriceAmount",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "UnitPriceAmount",
                table: "OrderItems",
                newName: "UnitPrice");
        }
    }
}
