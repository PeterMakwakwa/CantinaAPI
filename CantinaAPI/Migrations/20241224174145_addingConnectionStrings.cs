using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CantinaAPI.Migrations
{
    /// <inheritdoc />
    public partial class addingConnectionStrings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "48e14680-1a81-4a05-b1f1-b3335b3cf703", "3ec1e8bc-2b26-4164-aaa2-e218f9bd5156" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "5e75d4a4-8929-4ef6-9122-f70941eaeb50", "d808676d-4d07-4981-9264-107d7da8e5a3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2a29fff5-7da0-4942-89c5-6a472a6172cf", "ec7c6afb-92bf-408f-8197-66edc3f1ad2f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6fa0ee65-45f7-43f2-80ba-be965df917d0", "73f253c8-23f7-4a04-9564-04b401bcf0cd" });
        }
    }
}
