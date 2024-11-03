using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Thor.Provider.MySql.Thor
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResponseTime = table.Column<long>(type: "bigint", nullable: true),
                    Key = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Models = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Other = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Disable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Extension = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quota = table.Column<int>(type: "int", nullable: false),
                    RemainQuota = table.Column<long>(type: "bigint", nullable: false),
                    ControlAutomatically = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Modifier = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creator = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ModelManagers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Model = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Enable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Available = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CompletionRate = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    PromptRate = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    AudioPromptRate = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    AudioOutputRate = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    QuotaType = table.Column<int>(type: "int", nullable: false),
                    QuotaMax = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tags = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsVersion2 = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Modifier = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creator = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelManagers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductPurchaseRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RemainQuota = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
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
                    table.PrimaryKey("PK_ProductPurchaseRecords", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    RemainQuota = table.Column<long>(type: "bigint", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Modifier = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creator = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RateLimitModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WhiteList = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BlackList = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Enable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Model = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Strategy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Limit = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Modifier = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creator = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateLimitModels", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RedeemCodes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quota = table.Column<long>(type: "bigint", nullable: false),
                    Disabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RedeemedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RedeemedUserId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RedeemedUserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Modifier = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creator = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedeemCodes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Private = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Key);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Key = table.Column<string>(type: "varchar(42)", maxLength: 42, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsedQuota = table.Column<long>(type: "bigint", nullable: false),
                    UnlimitedQuota = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RemainQuota = table.Column<long>(type: "bigint", nullable: false),
                    AccessedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExpiredTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UnlimitedExpired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Disabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LimitModels = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WhiteIpList = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDelete = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Modifier = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creator = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHas = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Avatar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDisabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDelete = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ConsumeToken = table.Column<long>(type: "bigint", nullable: false),
                    RequestCount = table.Column<long>(type: "bigint", nullable: false),
                    ResidualCredit = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Modifier = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Creator = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "ModelManagers",
                columns: new[] { "Id", "AudioOutputRate", "AudioPromptRate", "Available", "CompletionRate", "CreatedAt", "Creator", "Description", "Enable", "Icon", "IsVersion2", "Model", "Modifier", "PromptRate", "QuotaMax", "QuotaType", "Tags", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00105c70-9652-470e-a623-22bab05b5f02"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3098), null, "GPT-4o 2024-05-13 文本模型", true, "OpenAI", false, "gpt-4o-2024-05-13", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("01055496-d99c-44a7-a943-532313bc38c6"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3120), null, "Claude 2 文本模型", true, "Claude", false, "claude-2", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("05b0ce4c-5bfd-4da7-94db-b93a09380c60"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3133), null, "Embedding S1 v1 嵌入模型", true, null, false, "embedding_s1_v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("09afb809-dc02-4313-9ffe-f33339037a4a"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3115), null, "4.0 超级文本模型", true, "Spark", false, "4.0Ultra", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("09f68f3d-d9f6-47d7-808c-ebc1bc7cf41b"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3127), null, "Claude 3 Opus 20240229 文本模型", true, "Claude", false, "claude-3-opus-20240229", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("0b95d208-9d9a-4b2f-be45-d9f50826397d"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3125), null, "Claude 3.5 Sonnet 20240620 文本模型", true, "Claude", false, "claude-3-5-sonnet-20240620", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("0c06b145-11aa-4127-a19e-4c5df3b2f350"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3111), null, "TTS 1 HD 1106 语音合成模型", true, "OpenAI", false, "tts-1-hd-1106", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("0c0b1680-1625-46ce-8a31-e70a32398c23"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3080), null, "GPT-4 1106 预览文本模型", true, "OpenAI", false, "gpt-4-1106-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("0d104d2f-83c8-4b90-bf5e-7aeeb42f1784"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3119), null, "ChatGLM 标准文本模型", true, "ChatGLM", false, "chatglm_std", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1d0f59a8-e201-4b59-b730-30d9e7190b1e"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3107), null, "Text Embedding 3 Small 嵌入模型", true, "OpenAI", false, "text-embedding-3-small", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1e468141-a1d2-4707-b6e0-8be35e6766d6"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3068), null, "GPT-3.5 Turbo 0301 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0301", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1f700432-5574-4608-948e-1af811711cb1"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3099), null, "GPT-4o 2024-08-06 文本模型", true, "OpenAI", false, "gpt-4o-2024-08-06", null, 1.25m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("2179cf82-24be-4f47-80c3-4132d637ec1a"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3107), null, "Text Embedding Ada 002 嵌入模型", true, "OpenAI", false, "text-embedding-ada-002", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("23b3d340-3c34-469b-8770-5820be231dcd"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3069), null, "GPT-3.5 Turbo 1106 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-1106", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("27fdf7e2-d13c-419d-ab07-ddfcf4b5e7e5"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3077), null, "GPT-3.5 Turbo Instruct 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-instruct", null, 0.001m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("2998aa4c-390a-4a53-8e34-69ec56b355d3"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3123), null, "Claude 3 Haiku 文本模型", true, "Claude", false, "claude-3-haiku", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("2eb94136-a534-4092-8453-010be6ae9d2f"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3091), null, "Gemini 1.5 Flash 文本模型", true, "Google", false, "gemini-1.5-flash", null, 0.2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("31c07c48-ae98-4950-9a00-e2cb8c4221c4"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3129), null, "DALL-E 2 图像生成模型", true, "OpenAI", false, "dall-e-2", null, 8m, null, 2, "[\"\\u56FE\\u7247\"]", null },
                    { new Guid("326a4a3b-79e4-44bd-a293-0ecb5dede286"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3087), null, "GPT-4 Turbo 文本模型", true, "OpenAI", false, "gpt-4-turbo", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("354ac804-8052-453e-b14f-4b47853d1c81"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3121), null, "Claude 2.0 文本模型", true, "Claude", false, "claude-2.0", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("35c63d74-17f3-4506-9c25-03dff8371d4c"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3112), null, "Whisper 1 语音识别模型", true, "OpenAI", false, "whisper-1", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("36232a2d-9b65-41ab-9838-f1cde410f282"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3111), null, "TTS 1 HD 语音合成模型", true, "OpenAI", false, "tts-1-hd", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("36606e19-8ab0-4877-83b1-a685a9d176f1"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3113), null, "通用文本模型", true, "Spark", false, "general", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("3ae20bee-0c28-40f1-8118-918797c86127"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3085), null, "GPT-4 32k 0613 文本模型", true, "OpenAI", false, "gpt-4-32k-0613", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("3fd8aed6-b066-497e-95ba-eb618b3c1182"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3092), null, "GPT-4 Turbo 2024-04-09 文本模型", true, "OpenAI", false, "gpt-4-turbo-2024-04-09", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("41821418-9b1d-4c09-9542-50feb4a3254e"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3069), null, "GPT-3.5 Turbo 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0613", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("483cd97c-9e06-4616-8df3-0bc82abeb1ac"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3118), null, "ChatGLM Pro 文本模型", true, "ChatGLM", false, "chatglm_pro", null, 0.7143m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("516108e9-8271-4aa4-b8c1-ba60401eb54b"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3086), null, "GPT-4 全部文本模型", true, "OpenAI", false, "gpt-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u8054\\u7F51\"]", null },
                    { new Guid("53597617-f9e7-4b5b-a54a-3159f6d75cd0"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3134), null, "GLM 4 文本模型", true, "ChatGLM", false, "glm-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("5be5c17e-677d-454f-bad4-ed821f576ee8"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3113), null, "Hunyuan Lite 文本模型", true, "Hunyuan", false, "hunyuan-lite", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("6008568a-1a3a-467a-9f2d-8887fd8dfb4f"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3092), null, "GPT-4 Turbo 预览文本模型", true, "OpenAI", false, "gpt-4-turbo-preview", null, 5m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("63c6cfab-0d32-4291-96fa-545134c197e0"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3075), null, "GPT-3.5 Turbo 16k 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("66bad5d5-9e2f-46eb-8c85-627fd976adc8"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3094), null, "GPT-4o 文本模型", true, "OpenAI", false, "gpt-4o", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("686d4f27-bb2a-4d46-97e0-0d66bb7369b5"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3104), null, "Text Davinci 002 文本模型", true, "OpenAI", false, "text-davinci-002", null, 10m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("69ee54f0-5b3c-4a73-baa7-41747247e033"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3108), null, "TTS 1 语音合成模型", true, "OpenAI", false, "tts-1", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("6b10fd74-2d5d-4e94-ad96-53f21675585a"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3097), null, "GPT-4o Mini 2024-07-18 文本模型", true, "OpenAI", false, "gpt-4o-mini-2024-07-18", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("738869bc-ed9e-4dd7-8a5e-e5922b25f090"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3101), null, "Text Babbage 001 文本模型", true, "OpenAI", false, "text-babbage-001", null, 0.25m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("75b1af94-1abc-4cbe-8834-01979c21d6f4"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3132), null, "Embedding BERT 512 v1 嵌入模型", true, "OpenAI", false, "embedding-bert-512-v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("89eefafa-c974-44a4-948c-0811bec3ff64"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3099), null, "Moonshot v1 128k 文本模型", true, "Moonshot", false, "moonshot-v1-128k", null, 5.06m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("8afe2211-c556-4f50-9e96-46aecee76d04"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3105), null, "Text Davinci Edit 001 文本模型", true, "OpenAI", false, "text-davinci-edit-001", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("971ecd91-0225-43e3-90c9-ba4b5367f462"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3115), null, "通用文本模型 v3.5", true, "Spark", false, "generalv3.5", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("9a4bab1d-dc71-4ec8-ac15-e4b1be472015"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3122), null, "Claude 2.1 文本模型", true, "Claude", false, "claude-2.1", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("9f50e39a-e0e5-4a91-923e-41033e4aa5da"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3083), null, "GPT-4 32k 文本模型", true, "OpenAI", false, "gpt-4-32k", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("9fc6554f-57fc-46c6-a217-4ba6c79ee227"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3128), null, "Claude Instant 1.2 文本模型", true, "Claude", false, "claude-instant-1.2", null, 0.4m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a4a4e243-3329-43e0-b6ca-8aced90cc600"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3131), null, "Embedding 2 嵌入模型", true, "OpenAI", false, "embedding-2", null, 0.0355m, "", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("a5e1621c-b824-410c-af93-174d2f22e9f9"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3124), null, "Claude 3 Haiku 20240307 文本模型", true, "Claude", false, "claude-3-haiku-20240307", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a9a56ec8-cd1e-486f-b32b-814a745fc743"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3090), null, "Gemini Pro 视觉模型", true, "Google", false, "gemini-pro-vision", null, 2m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("ade0d6df-630d-400a-a79e-31b0925de7ad"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3100), null, "Moonshot v1 32k 文本模型", true, "Moonshot", false, "moonshot-v1-32k", null, 2m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("adea217b-3c8f-40c0-884d-b0e04433adda"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3080), null, "GPT-4 0613 文本模型", true, "OpenAI", false, "gpt-4-0613", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ae3c4629-3fb0-4e10-8018-e807e05506d2"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3120), null, "ChatGLM Turbo 文本模型", true, "ChatGLM", false, "chatglm_turbo", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("aebf6bf7-5eec-4705-9ab8-3ba42f390fcb"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3114), null, "通用文本模型 v3", true, "Spark", false, "generalv3", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b3a99204-27b7-4ff5-a3c9-4e1515d0ce78"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3079), null, "GPT-4 0314 文本模型", true, "OpenAI", false, "gpt-4-0314", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b4b46d7c-1f25-499a-89aa-f0526dfaee73"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3084), null, "GPT-4 32k 0314 文本模型", true, "OpenAI", false, "gpt-4-32k-0314", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b8eff6eb-4c87-404e-a843-4da5d8f1f0ad"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3093), null, "GPT-4 视觉预览模型", true, "OpenAI", false, "gpt-4-vision-preview", null, 10m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("ba3d714f-0b08-4532-bed5-0db2f3d46222"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3087), null, "Gemini 1.5 Pro 文本模型", true, "Google", false, "gemini-1.5-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("be496236-6d7e-4712-8c97-fc7051b82a49"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3060), null, "GPT-3.5 Turbo 文本模型", true, "OpenAI", false, "gpt-3.5-turbo", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("c3f1a9b3-c3da-4517-a036-e80576116d4d"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3102), null, "Text Curie 001 文本模型", true, "OpenAI", false, "text-curie-001", null, 1m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("c50237ee-e1dc-45c6-b898-2e7ee8a45d16"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3109), null, "TTS 1 1106 语音合成模型", true, "OpenAI", false, "tts-1-1106", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("c5742a17-71e7-4959-b960-85e1e26137c4"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3101), null, "Moonshot v1 8k 文本模型", true, "Moonshot", false, "moonshot-v1-8k", null, 1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("c6c7c467-3a83-4ca7-81b1-53cd49eff24b"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3105), null, "Text Davinci 003 文本模型", true, "OpenAI", false, "text-davinci-003", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("c731dd4e-f290-45be-bf2f-a276d55e6a58"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3106), null, "Text Embedding 3 Large 嵌入模型", true, "OpenAI", false, "text-embedding-3-large", null, 0.13m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("c9dc0078-78d9-40d4-9ddd-eb7c557f214c"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3067), null, "GPT-3.5 Turbo 0125 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0125", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("caf27f4e-381b-41b5-a6df-d0ba6ac567ab"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3078), null, "GPT-4 文本模型", true, "OpenAI", false, "gpt-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("cdd9ba3a-9682-4e21-9754-8608a7f4c6eb"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3094), null, "ChatGPT 4o 最新文本模型", true, "OpenAI", false, "chatgpt-4o-latest", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("cf999306-7099-43ce-ab1f-a82f21571582"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3127), null, "Claude Instant 1 文本模型", true, "Claude", false, "claude-instant-1", null, 0.815m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("d42d8377-9401-4ea9-84fc-dbeeafa25679"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3135), null, "GLM 4v 文本模型", true, "ChatGLM", false, "glm-4v", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("dcd96138-c000-48c8-8c5d-e33e7f046270"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3118), null, "ChatGLM Lite 文本模型", true, "ChatGLM", false, "chatglm_lite", null, 0.1429m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("e8d88669-f375-4e57-9d92-4a7d72350310"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3088), null, "Gemini Pro 文本模型", true, "Google", false, "gemini-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("e989ece9-2c93-4f3e-b9bd-5040a210de1e"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3078), null, "GPT-4 0125 预览文本模型", true, "OpenAI", false, "gpt-4-0125-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ea63b44c-6d5c-4846-8f8b-b6201d21063f"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3126), null, "Claude 3 Sonnet 20240229 文本模型", true, "Claude", false, "claude-3-sonnet-20240229", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("edd3593d-8b67-4b9f-888e-7b5964f92162"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3135), null, "GLM 4 全部文本模型", true, "ChatGLM", false, "glm-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("f1b2e874-d24a-48a0-8720-b7b2e1808e33"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3095), null, "GPT-4o Mini 文本模型", true, "OpenAI", false, "gpt-4o-mini", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("f59ab4e5-6a4b-4e01-911c-ff94e8236b7b"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3129), null, "DALL-E 3 图像生成模型", true, "OpenAI", false, "dall-e-3", null, 20m, null, 2, "[\"\\u56FE\\u7247\"]", null },
                    { new Guid("f8f560cf-c288-4377-8865-847383c9643e"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3133), null, "GLM 3 Turbo 文本模型", true, "ChatGLM", false, "glm-3-turbo", null, 0.355m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("fcb1d536-7bb8-47d3-8774-580108f175cf"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3076), null, "GPT-3.5 Turbo 16k 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k-0613", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ff867b12-70aa-4ebc-bfdb-685e8195a48c"), null, null, true, null, new DateTime(2024, 11, 4, 1, 9, 21, 295, DateTimeKind.Local).AddTicks(3083), null, "GPT-4 1106 视觉预览模型", true, "OpenAI", false, "gpt-4-1106-vision-preview", null, 10m, "128K", 1, "[\"\\u89C6\\u89C9\",\"\\u6587\\u672C\"]", null }
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
                    { "Setting:GeneralSetting:AlipayRootCertPath", "支付宝AlipayRootCertPath", true, "" },
                    { "Setting:GeneralSetting:AutoDisableChannel", "自动禁用异常渠道", true, "false" },
                    { "Setting:GeneralSetting:ChatLink", "对话链接", false, "" },
                    { "Setting:GeneralSetting:CheckInterval", "检测间隔 (分钟)", true, "60" },
                    { "Setting:GeneralSetting:EnableAutoCheckChannel", "启用自动检测渠道策略", true, "false" },
                    { "Setting:GeneralSetting:EnableClearLog", "启用定时清理日志", true, "true" },
                    { "Setting:GeneralSetting:IntervalDays", "间隔天数", true, "90" },
                    { "Setting:GeneralSetting:InviteQuota", "邀请奖励额度", true, "100000" },
                    { "Setting:GeneralSetting:NewUserQuota", "新用户初始额度", true, "100000" },
                    { "Setting:GeneralSetting:RechargeAddress", "充值地址", false, "" },
                    { "Setting:GeneralSetting:RequestQuota", "请求预扣额度", true, "2000" },
                    { "Setting:GeneralSetting:VidolLink", "Vidol 链接", false, "" },
                    { "Setting:OtherSetting:IndexContent", "首页内容", false, "AI DotNet API 提供更强的兼容，将更多的AI平台接入到AI DotNet API中，让AI集成更加简单。" },
                    { "Setting:OtherSetting:WebLogo", "网站Logo地址", false, "/logo.png" },
                    { "Setting:OtherSetting:WebTitle", "网站标题", false, "AIDtoNet API" },
                    { "Setting:SystemSetting:EmailAddress", "邮箱地址", true, "" },
                    { "Setting:SystemSetting:EmailPassword", "邮箱密码", true, "" },
                    { "Setting:SystemSetting:EnableEmailRegister", "启用邮箱验证注册", false, "false" },
                    { "Setting:SystemSetting:EnableGiteeLogin", "允许Gitee登录", false, "true" },
                    { "Setting:SystemSetting:EnableGithubLogin", "允许Github登录", false, "true" },
                    { "Setting:SystemSetting:EnableRegister", "启用账号注册", false, "true" },
                    { "Setting:SystemSetting:GiteeClientId", "Gitee Client Id", false, "" },
                    { "Setting:SystemSetting:GiteeClientSecret", "Gitee Client Secret", true, "" },
                    { "Setting:SystemSetting:GiteeRedirectUri", "Gitee redirect_uri", false, "" },
                    { "Setting:SystemSetting:GithubClientId", "Github Client Id", false, "" },
                    { "Setting:SystemSetting:GithubClientSecret", "Github Client Secret", true, "" },
                    { "Setting:SystemSetting:ServerAddress", "服务器地址", false, "" },
                    { "Setting:SystemSetting:SmtpAddress", "SMTP地址", true, "" }
                });

            migrationBuilder.InsertData(
                table: "Tokens",
                columns: new[] { "Id", "AccessedTime", "CreatedAt", "Creator", "DeletedAt", "Disabled", "ExpiredTime", "IsDelete", "Key", "LimitModels", "Modifier", "Name", "RemainQuota", "UnlimitedExpired", "UnlimitedQuota", "UpdatedAt", "UsedQuota", "WhiteIpList" },
                values: new object[] { "e1c39826-54d3-42ba-aebb-09b894ab3cb3", null, new DateTime(2024, 11, 4, 1, 9, 21, 293, DateTimeKind.Local).AddTicks(3846), "c68dd459-d9e3-4955-93b7-337255d8e0f4", null, false, null, false, "sk-Qg6M1YwLj9C2Irz6EJTrxybdRyqqm4hOBKYged", "[]", null, "默认Token", 0L, true, true, null, 0L, "[]" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "c68dd459-d9e3-4955-93b7-337255d8e0f4", null, 0L, new DateTime(2024, 11, 4, 1, 9, 21, 293, DateTimeKind.Local).AddTicks(3432), null, null, "239573049@qq.com", false, false, null, "da5828612309b64239dd2479ab6dec27", "66004663165d46f38baf659d91908f7f", 0L, 1000000000L, "admin", null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Channels_Creator",
                table: "Channels",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_Name",
                table: "Channels",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ModelManagers_Creator",
                table: "ModelManagers",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_ModelManagers_Model",
                table: "ModelManagers",
                column: "Model");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPurchaseRecords_Creator",
                table: "ProductPurchaseRecords",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPurchaseRecords_UserId",
                table: "ProductPurchaseRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RateLimitModels_Creator",
                table: "RateLimitModels",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_RateLimitModels_Model",
                table: "RateLimitModels",
                column: "Model");

            migrationBuilder.CreateIndex(
                name: "IX_RedeemCodes_Code",
                table: "RedeemCodes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_Creator",
                table: "Tokens",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_Key",
                table: "Tokens",
                column: "Key");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "ModelManagers");

            migrationBuilder.DropTable(
                name: "ProductPurchaseRecords");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "RateLimitModels");

            migrationBuilder.DropTable(
                name: "RedeemCodes");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
