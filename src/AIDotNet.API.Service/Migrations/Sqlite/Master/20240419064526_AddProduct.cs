using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AIDotNet.API.Service.Migrations.Master
{
    /// <inheritdoc />
    public partial class AddProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "4cde8057-c272-4e2a-91b7-878032e4a2d0");

            migrationBuilder.CreateTable(
                name: "ProductPurchaseRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ProductId = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    RemainQuota = table.Column<long>(type: "INTEGER", nullable: false),
                    PurchaseTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPurchaseRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    RemainQuota = table.Column<long>(type: "INTEGER", nullable: false),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Key", "Description", "Private", "Value" },
                values: new object[,]
                {
                    { "Setting:GeneralSetting:AlipayAppCertPath", "支付宝AppCertPath", true, "" },
                    { "Setting:GeneralSetting:AlipayAppId", "支付宝应用APPID", true, "" },
                    { "Setting:GeneralSetting:AlipayNotifyUrl", "支付宝支付回调地址", false, "https://您的服务器地址/" },
                    { "Setting:GeneralSetting:AlipayPrivateKey", "支付宝应用私钥", true, "" },
                    { "Setting:GeneralSetting:AlipayPublicCertPath", "支付宝公钥证书路径", true, "" },
                    { "Setting:GeneralSetting:AlipayPublicKey", "支付宝公钥", true, "" },
                    { "Setting:GeneralSetting:AlipayRootCertPath", "支付宝AlipayRootCertPath", true, "" }
                });

            migrationBuilder.InsertData(
                table: "Tokens",
                columns: new[] { "Id", "AccessedTime", "CreatedAt", "Creator", "DeletedAt", "Disabled", "ExpiredTime", "IsDelete", "Key", "Modifier", "Name", "RemainQuota", "UnlimitedExpired", "UnlimitedQuota", "UpdatedAt", "UsedQuota" },
                values: new object[] { 9999L, null, DateTime.Now, "72e8703c-6dea-466c-82aa-7e0b85d04618", null, false, null, false, "sk-gkT0IZkGclTdAqZVZuOhgCEtCjapsQl37UiS3z", null, "默认Token", 0L, true, true, null, 0L });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "72e8703c-6dea-466c-82aa-7e0b85d04618", null, 0L, DateTime.Now, null, null, "239573049@qq.com", false, false, null, "654b09708742ad11ad1eee4e43fd84f1", "9d80d1d43cf14a17bcf052a2da79e5cf", 0L, 10000000L, "admin", null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPurchaseRecords_Creator",
                table: "ProductPurchaseRecords",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPurchaseRecords_UserId",
                table: "ProductPurchaseRecords",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPurchaseRecords");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:GeneralSetting:AlipayAppCertPath");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:GeneralSetting:AlipayAppId");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:GeneralSetting:AlipayNotifyUrl");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:GeneralSetting:AlipayPrivateKey");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:GeneralSetting:AlipayPublicCertPath");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:GeneralSetting:AlipayPublicKey");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:GeneralSetting:AlipayRootCertPath");

            migrationBuilder.DeleteData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: 9999L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "72e8703c-6dea-466c-82aa-7e0b85d04618");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "4cde8057-c272-4e2a-91b7-878032e4a2d0", null, 0L, DateTime.Now, null, null, "239573049@qq.com", false, false, null, "b73f077db2980fe2765fc3b136babedb", "e9a07d959d174ee6953b7777833dd23b", 0L, 10000000L, "admin", null, "admin" });
        }
    }
}
