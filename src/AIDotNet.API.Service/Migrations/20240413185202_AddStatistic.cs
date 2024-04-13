using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIDotNet.API.Service.Migrations
{
    /// <inheritdoc />
    public partial class AddStatistic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "f4910916-d4df-4c39-beed-6e7deb1f5166");

            migrationBuilder.CreateTable(
                name: "ModelStatisticsNumbers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Month = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Day = table.Column<ushort>(type: "INTEGER", nullable: false),
                    ModelName = table.Column<string>(type: "TEXT", nullable: false),
                    Quota = table.Column<int>(type: "INTEGER", nullable: false),
                    TokenUsed = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelStatisticsNumbers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatisticsConsumesNumbers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Month = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Day = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Number = table.Column<long>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticsConsumesNumbers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:GeneralSetting:EnableClearLog",
                column: "Value",
                value: "true");

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:GeneralSetting:IntervalDays",
                column: "Value",
                value: "90");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "5c5fa814-d56b-4a6b-ab4e-92af426f1881", null, 0L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "239573049@qq.com", false, false, null, "18506eb13b7691d38913aa6af135d395", "aa6f2db494724b82b9f8076a6c696a56", 0L, 10000000L, "admin", null, "admin" });

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
                name: "ModelStatisticsNumbers");

            migrationBuilder.DropTable(
                name: "StatisticsConsumesNumbers");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5c5fa814-d56b-4a6b-ab4e-92af426f1881");

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:GeneralSetting:EnableClearLog",
                column: "Value",
                value: "false");

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:GeneralSetting:IntervalDays",
                column: "Value",
                value: "7");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "f4910916-d4df-4c39-beed-6e7deb1f5166", null, 0L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "239573049@qq.com", false, false, null, "13e11eb515bcfb009ca59f3f54efcdf9", "d285e92b00e74014b533b12dabb93227", 0L, 10000000L, "admin", null, "admin" });
        }
    }
}
