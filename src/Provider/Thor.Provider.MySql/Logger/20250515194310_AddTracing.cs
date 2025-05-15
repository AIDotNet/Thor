using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thor.Provider.MySql.Logger
{
    /// <inheritdoc />
    public partial class AddTracing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Loggers",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccess",
                table: "Loggers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "Loggers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OpenAIProject",
                table: "Loggers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                table: "Loggers",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ServiceId",
                table: "Loggers",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Loggers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tracings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TraceId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChatLoggerId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Duration = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Depth = table.Column<int>(type: "int", nullable: false),
                    ServiceName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Attributes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Children = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Modifier = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creator = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracings", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Loggers_OrganizationId",
                table: "Loggers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loggers_ServiceId",
                table: "Loggers",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Loggers_UserId",
                table: "Loggers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracings_ChatLoggerId",
                table: "Tracings",
                column: "ChatLoggerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracings_Creator",
                table: "Tracings",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_Tracings_TraceId",
                table: "Tracings",
                column: "TraceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tracings");

            migrationBuilder.DropIndex(
                name: "IX_Loggers_OrganizationId",
                table: "Loggers");

            migrationBuilder.DropIndex(
                name: "IX_Loggers_ServiceId",
                table: "Loggers");

            migrationBuilder.DropIndex(
                name: "IX_Loggers_UserId",
                table: "Loggers");

            migrationBuilder.DropColumn(
                name: "IsSuccess",
                table: "Loggers");

            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "Loggers");

            migrationBuilder.DropColumn(
                name: "OpenAIProject",
                table: "Loggers");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Loggers");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Loggers");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Loggers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Loggers",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
