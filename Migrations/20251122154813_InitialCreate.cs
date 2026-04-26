using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoadDefect.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentAreaId = table.Column<int>(type: "int", nullable: true),
                    GeoJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Areas_Areas_ParentAreaId",
                        column: x => x.ParentAreaId,
                        principalTable: "Areas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoadSegments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    StartLat = table.Column<double>(type: "float", nullable: false),
                    StartLng = table.Column<double>(type: "float", nullable: false),
                    EndLat = table.Column<double>(type: "float", nullable: false),
                    EndLng = table.Column<double>(type: "float", nullable: false),
                    FunctionalClass = table.Column<int>(type: "int", nullable: false),
                    TrafficImportance = table.Column<int>(type: "int", nullable: false),
                    AreaId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadSegments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoadSegments_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoadSegments_Areas_AreaId1",
                        column: x => x.AreaId1,
                        principalTable: "Areas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Defects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefectType = table.Column<int>(type: "int", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReportedByUserId = table.Column<int>(type: "int", nullable: true),
                    AssignedEngineerId = table.Column<int>(type: "int", nullable: true),
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    RoadSegmentId = table.Column<int>(type: "int", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AreaId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Defects_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Defects_Areas_AreaId1",
                        column: x => x.AreaId1,
                        principalTable: "Areas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Defects_RoadSegments_RoadSegmentId",
                        column: x => x.RoadSegmentId,
                        principalTable: "RoadSegments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Defects_Users_AssignedEngineerId",
                        column: x => x.AssignedEngineerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Defects_Users_ReportedByUserId",
                        column: x => x.ReportedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DefectImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefectId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBeforeRepair = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefectImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefectImages_Defects_DefectId",
                        column: x => x.DefectId,
                        principalTable: "Defects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefectId = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    AssignedToUserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Defects_DefectId",
                        column: x => x.DefectId,
                        principalTable: "Defects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Users_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderUpdates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkOrderId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrderUpdates_Users_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkOrderUpdates_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Areas_ParentAreaId",
                table: "Areas",
                column: "ParentAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_DefectImages_DefectId",
                table: "DefectImages",
                column: "DefectId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_AreaId",
                table: "Defects",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_AreaId1",
                table: "Defects",
                column: "AreaId1");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_AssignedEngineerId",
                table: "Defects",
                column: "AssignedEngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_ReportedByUserId",
                table: "Defects",
                column: "ReportedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_RoadSegmentId",
                table: "Defects",
                column: "RoadSegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RoadSegments_AreaId",
                table: "RoadSegments",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_RoadSegments_AreaId1",
                table: "RoadSegments",
                column: "AreaId1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AreaId",
                table: "Users",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_AssignedToUserId",
                table: "WorkOrders",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_CreatedByUserId",
                table: "WorkOrders",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_DefectId",
                table: "WorkOrders",
                column: "DefectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderUpdates_UpdatedByUserId",
                table: "WorkOrderUpdates",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderUpdates_WorkOrderId",
                table: "WorkOrderUpdates",
                column: "WorkOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefectImages");

            migrationBuilder.DropTable(
                name: "WorkOrderUpdates");

            migrationBuilder.DropTable(
                name: "WorkOrders");

            migrationBuilder.DropTable(
                name: "Defects");

            migrationBuilder.DropTable(
                name: "RoadSegments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Areas");
        }
    }
}
