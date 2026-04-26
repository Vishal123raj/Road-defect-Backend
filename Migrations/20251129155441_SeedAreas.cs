using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RoadDefect.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedAreas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "GeoJson", "Name", "ParentAreaId" },
                values: new object[,]
                {
                    { 1, null, "Area 1", null },
                    { 2, null, "Area 2", null },
                    { 3, null, "Area 3", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
