using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIDotNet.API.Service.Migrations
{
    /// <inheritdoc />
    public partial class controlAutomatically : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "f9c067cf-7afa-4712-be30-5024cc47aa67");

            migrationBuilder.AddColumn<bool>(
                name: "ControlAutomatically",
                table: "Channels",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "f4910916-d4df-4c39-beed-6e7deb1f5166", null, 0L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "239573049@qq.com", false, false, null, "13e11eb515bcfb009ca59f3f54efcdf9", "d285e92b00e74014b533b12dabb93227", 0L, 10000000L, "admin", null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "f4910916-d4df-4c39-beed-6e7deb1f5166");

            migrationBuilder.DropColumn(
                name: "ControlAutomatically",
                table: "Channels");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "f9c067cf-7afa-4712-be30-5024cc47aa67", null, 0L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "239573049@qq.com", false, false, null, "7d7e935de87264488810a310c85f9cf3", "f33e061c46fb4175b1dd929f677dae27", 0L, 10000000L, "admin", null, "admin" });
        }
    }
}
