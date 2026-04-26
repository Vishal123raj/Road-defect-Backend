using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RoadDefect.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RoadSegments",
                columns: new[] { "Id", "AreaId", "AreaId1", "EndLat", "EndLng", "FunctionalClass", "Name", "StartLat", "StartLng", "TrafficImportance" },
                values: new object[,]
                {
                    { 1, 1, null, 20.600000000000001, 78.969999999999999, 1, "Main Highway - Section 1", 20.593699999999998, 78.962900000000005, 3 },
                    { 2, 2, null, 20.510000000000002, 78.915000000000006, 2, "City Ring Road - East", 20.5, 78.900000000000006, 2 },
                    { 3, 3, null, 20.552499999999998, 78.952500000000001, 3, "Market Street Lane", 20.550000000000001, 78.950000000000003, 1 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AreaId", "CreatedAt", "Email", "IsActive", "Name", "PasswordHash", "Phone", "Role" },
                values: new object[] { 99, null, new DateTime(2025, 11, 29, 16, 8, 27, 987, DateTimeKind.Utc).AddTicks(2899), "admin@roaddefect.com", true, "Super Admin", "E86F78A8A3CAF0B60D8E74E5942AA6D86DC150CD3C03338AEF25B7D2D7E3ACC7", "9999999999", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoadSegments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RoadSegments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RoadSegments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 99);
        }
    }
}
