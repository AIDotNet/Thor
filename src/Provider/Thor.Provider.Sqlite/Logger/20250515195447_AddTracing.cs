﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thor.Provider.Logger
{
    /// <inheritdoc />
    public partial class AddTracing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuccess",
                table: "Loggers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "Loggers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OpenAIProject",
                table: "Loggers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceId",
                table: "Loggers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Loggers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tracings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    TraceId = table.Column<string>(type: "TEXT", nullable: false),
                    ChatLoggerId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Duration = table.Column<long>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ErrorMessage = table.Column<string>(type: "TEXT", nullable: true),
                    Depth = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceName = table.Column<string>(type: "TEXT", nullable: false),
                    Attributes = table.Column<string>(type: "TEXT", nullable: true),
                    Children = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracings", x => x.Id);
                });

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
                name: "ServiceId",
                table: "Loggers");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Loggers");
        }
    }
}
