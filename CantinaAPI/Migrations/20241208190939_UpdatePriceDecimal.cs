using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CantinaAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePriceDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Items",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "af759857-885c-43c6-a446-615834e783ae", "e638bdf9-f73a-43d8-81c0-a64dbc370042" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "69f3d070-f10a-4a03-8dae-51a8e333af79", "a48d6ed4-33e6-4fd4-9880-b94ee41f9baf" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Items",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2cc5377f-670b-4d5f-86d1-541d42d9722a", "afadafe7-55d2-4ab2-8b30-6892e932736a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "438b867e-a636-4b14-a3d1-a961c91a5a1f", "8a95412f-da44-4452-a561-e124c15fd757" });
        }
    }
}
