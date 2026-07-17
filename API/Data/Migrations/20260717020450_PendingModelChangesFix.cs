using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class PendingModelChangesFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-id",
                column: "ConcurrencyStamp",
                value: "d7869c66-677c-4a7f-b7f6-a92ee50197ab");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-id",
                column: "ConcurrencyStamp",
                value: "d7869c66-677c-4a7c-b7f6-a92ee50197ab");
        }
    }
}
