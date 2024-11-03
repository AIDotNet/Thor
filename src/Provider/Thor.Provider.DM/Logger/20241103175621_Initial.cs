using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thor.Provider.DM.Logger
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Loggers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Type = table.Column<int>(type: "INT", nullable: false),
                    Content = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    PromptTokens = table.Column<int>(type: "INT", nullable: false),
                    CompletionTokens = table.Column<int>(type: "INT", nullable: false),
                    Quota = table.Column<long>(type: "BIGINT", nullable: false),
                    ModelName = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    TokenName = table.Column<string>(type: "NVARCHAR2(450)", nullable: true),
                    UserName = table.Column<string>(type: "NVARCHAR2(450)", nullable: true),
                    UserId = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    ChannelId = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    TotalTime = table.Column<int>(type: "INT", nullable: false),
                    Stream = table.Column<bool>(type: "BIT", nullable: false),
                    ChannelName = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    IP = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    UserAgent = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    Modifier = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Creator = table.Column<string>(type: "NVARCHAR2(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loggers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModelStatisticsNumbers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Year = table.Column<int>(type: "INT", nullable: false),
                    Month = table.Column<int>(type: "INT", nullable: false),
                    Day = table.Column<int>(type: "INT", nullable: false),
                    ModelName = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Quota = table.Column<int>(type: "INT", nullable: false),
                    TokenUsed = table.Column<int>(type: "INT", nullable: false),
                    Count = table.Column<int>(type: "INT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    Modifier = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Creator = table.Column<string>(type: "NVARCHAR2(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelStatisticsNumbers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatisticsConsumesNumbers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Year = table.Column<int>(type: "INT", nullable: false),
                    Month = table.Column<int>(type: "INT", nullable: false),
                    Day = table.Column<int>(type: "INT", nullable: false),
                    Number = table.Column<long>(type: "BIGINT", nullable: false),
                    Type = table.Column<int>(type: "INT", nullable: false),
                    Value = table.Column<long>(type: "BIGINT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    Modifier = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Creator = table.Column<string>(type: "NVARCHAR2(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticsConsumesNumbers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loggers_Creator",
                table: "Loggers",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_Loggers_ModelName",
                table: "Loggers",
                column: "ModelName");

            migrationBuilder.CreateIndex(
                name: "IX_Loggers_TokenName",
                table: "Loggers",
                column: "TokenName");

            migrationBuilder.CreateIndex(
                name: "IX_Loggers_UserName",
                table: "Loggers",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_ModelStatisticsNumbers_Creator",
                table: "ModelStatisticsNumbers",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_ModelStatisticsNumbers_Day",
                table: "ModelStatisticsNumbers",
                column: "Day");

            migrationBuilder.CreateIndex(
                name: "IX_ModelStatisticsNumbers_ModelName",
                table: "ModelStatisticsNumbers",
                column: "ModelName");

            migrationBuilder.CreateIndex(
                name: "IX_ModelStatisticsNumbers_Month",
                table: "ModelStatisticsNumbers",
                column: "Month");

            migrationBuilder.CreateIndex(
                name: "IX_ModelStatisticsNumbers_Year",
                table: "ModelStatisticsNumbers",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_StatisticsConsumesNumbers_Creator",
                table: "StatisticsConsumesNumbers",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_StatisticsConsumesNumbers_Day",
                table: "StatisticsConsumesNumbers",
                column: "Day");

            migrationBuilder.CreateIndex(
                name: "IX_StatisticsConsumesNumbers_Month",
                table: "StatisticsConsumesNumbers",
                column: "Month");

            migrationBuilder.CreateIndex(
                name: "IX_StatisticsConsumesNumbers_Year",
                table: "StatisticsConsumesNumbers",
                column: "Year");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loggers");

            migrationBuilder.DropTable(
                name: "ModelStatisticsNumbers");

            migrationBuilder.DropTable(
                name: "StatisticsConsumesNumbers");
        }
    }
}
