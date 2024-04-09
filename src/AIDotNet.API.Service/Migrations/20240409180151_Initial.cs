using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIDotNet.API.Service.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    ResponseTime = table.Column<long>(type: "INTEGER", nullable: true),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Models = table.Column<string>(type: "TEXT", nullable: false),
                    Other = table.Column<string>(type: "TEXT", nullable: false),
                    Disable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Extension = table.Column<string>(type: "TEXT", nullable: false),
                    Quota = table.Column<int>(type: "INTEGER", nullable: false),
                    RemainQuota = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Loggers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    PromptTokens = table.Column<int>(type: "INTEGER", nullable: false),
                    CompletionTokens = table.Column<int>(type: "INTEGER", nullable: false),
                    Quota = table.Column<int>(type: "INTEGER", nullable: false),
                    ModelName = table.Column<string>(type: "TEXT", nullable: false),
                    TokenName = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    ChannelId = table.Column<string>(type: "TEXT", nullable: true),
                    ChannelName = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loggers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", maxLength: 42, nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    UsedQuota = table.Column<long>(type: "INTEGER", nullable: false),
                    UnlimitedQuota = table.Column<bool>(type: "INTEGER", nullable: false),
                    RemainQuota = table.Column<long>(type: "INTEGER", nullable: false),
                    AccessedTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExpiredTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UnlimitedExpired = table.Column<bool>(type: "INTEGER", nullable: false),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDelete = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHas = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    IsDelete = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ConsumeToken = table.Column<long>(type: "INTEGER", nullable: false),
                    RequestCount = table.Column<long>(type: "INTEGER", nullable: false),
                    ResidualCredit = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "5dac91d8-0ed4-4727-a534-e6f9c52434e3", 0L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "239573049@qq.com", false, null, "76c359997c2ec8ac655aeb7a7917304b", "37747d67812549bcae99032d020efd3f", 0L, 0L, "admin", null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Channels_Creator",
                table: "Channels",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_Name",
                table: "Channels",
                column: "Name");

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
                name: "IX_Tokens_Creator",
                table: "Tokens",
                column: "Creator");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "Loggers");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
