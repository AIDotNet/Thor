using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIDotNet.API.Service.Migrations.Sqlite.Master
{
    /// <inheritdoc />
    public partial class UpdateId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: 9999L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "82ea4ca6-3bc8-4881-b64e-d33fab18aa39");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Tokens",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "RedeemCodes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.InsertData(
                table: "Tokens",
                columns: new[] { "Id", "AccessedTime", "CreatedAt", "Creator", "DeletedAt", "Disabled", "ExpiredTime", "IsDelete", "Key", "Modifier", "Name", "RemainQuota", "UnlimitedExpired", "UnlimitedQuota", "UpdatedAt", "UsedQuota" },
                values: new object[] { "540aa3f2-b98e-479b-90be-86dbaae53cf7", null, new DateTime(2024, 5, 14, 23, 5, 38, 986, DateTimeKind.Local).AddTicks(1917), "e875f01b-b4db-4c58-b714-c338d7db8133", null, false, null, false, "sk-BBTEZjirc6sdgEbPlMtcif3Y1PL0RF2zj58bbe", null, "默认Token", 0L, true, true, null, 0L });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "e875f01b-b4db-4c58-b714-c338d7db8133", null, 0L, new DateTime(2024, 5, 14, 23, 5, 38, 986, DateTimeKind.Local).AddTicks(1558), null, null, "239573049@qq.com", false, false, null, "11c794f299490249f589b214b6752aad", "7f01ccf11d7e4bf29c3c89fddecd5080", 0L, 10000000L, "admin", null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: "540aa3f2-b98e-479b-90be-86dbaae53cf7");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "e875f01b-b4db-4c58-b714-c338d7db8133");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Tokens",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "RedeemCodes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.InsertData(
                table: "Tokens",
                columns: new[] { "Id", "AccessedTime", "CreatedAt", "Creator", "DeletedAt", "Disabled", "ExpiredTime", "IsDelete", "Key", "Modifier", "Name", "RemainQuota", "UnlimitedExpired", "UnlimitedQuota", "UpdatedAt", "UsedQuota" },
                values: new object[] { 9999L, null, new DateTime(2024, 5, 14, 23, 5, 39, 213, DateTimeKind.Local).AddTicks(5528), "82ea4ca6-3bc8-4881-b64e-d33fab18aa39", null, false, null, false, "sk-f4I2CkR0xGVXW6tpNB7timK7BwV8bbpm1KN8jk", null, "默认Token", 0L, true, true, null, 0L });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "82ea4ca6-3bc8-4881-b64e-d33fab18aa39", null, 0L, new DateTime(2024, 5, 14, 23, 5, 39, 214, DateTimeKind.Local).AddTicks(1045), null, null, "239573049@qq.com", false, false, null, "645203af27210c85c0ba0133f54e931b", "87874ef6916842a6988ddcf45a702550", 0L, 10000000L, "admin", null, "admin" });
        }
    }
}
