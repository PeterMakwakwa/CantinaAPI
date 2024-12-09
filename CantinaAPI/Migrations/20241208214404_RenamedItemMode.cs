using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CantinaAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenamedItemMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6543009c-beaf-43f0-9052-3e9d9f029d71", "f74546dc-61f9-4bad-98b4-9c7be8684fab" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2-id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "14cefe59-7a72-45fb-a40c-153e06463c50", "03afc3bc-e8f8-4b2c-aaea-721013fbf3d8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
