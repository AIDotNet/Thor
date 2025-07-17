using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thor.Provider.PostgreSql.Logger
{
    /// <inheritdoc />
    public partial class AddRequestLogger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestLogs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ChatLoggerId = table.Column<string>(type: "text", nullable: true),
                    RoutePath = table.Column<string>(type: "text", nullable: false),
                    RequestTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ResponseTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DurationMs = table.Column<long>(type: "bigint", nullable: false),
                    IsSuccess = table.Column<bool>(type: "boolean", nullable: false),
                    HttpStatusCode = table.Column<int>(type: "integer", nullable: false),
                    ClientIp = table.Column<string>(type: "text", nullable: true),
                    UserAgent = table.Column<string>(type: "text", nullable: true),
                    RequestHeaders = table.Column<string>(type: "text", nullable: true),
                    RequestBody = table.Column<string>(type: "text", nullable: true),
                    ResponseBody = table.Column<string>(type: "text", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Modifier = table.Column<string>(type: "text", nullable: true),
                    Creator = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestLogs_ChatLoggerId",
                table: "RequestLogs",
                column: "ChatLoggerId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestLogs_Creator",
                table: "RequestLogs",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_RequestLogs_RoutePath",
                table: "RequestLogs",
                column: "RoutePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestLogs");
        }
    }
}
