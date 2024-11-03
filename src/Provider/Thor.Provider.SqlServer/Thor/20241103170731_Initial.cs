using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Thor.Provider.SqlServer.Thor
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseTime = table.Column<long>(type: "bigint", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Models = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Other = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Disable = table.Column<bool>(type: "bit", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quota = table.Column<int>(type: "int", nullable: false),
                    RemainQuota = table.Column<long>(type: "bigint", nullable: false),
                    ControlAutomatically = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModelManagers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Enable = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false),
                    CompletionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PromptRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AudioPromptRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AudioOutputRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QuotaType = table.Column<int>(type: "int", nullable: false),
                    QuotaMax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVersion2 = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelManagers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductPurchaseRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RemainQuota = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPurchaseRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainQuota = table.Column<long>(type: "bigint", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RateLimitModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhiteList = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlackList = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enable = table.Column<bool>(type: "bit", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Strategy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Limit = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateLimitModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RedeemCodes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quota = table.Column<long>(type: "bigint", nullable: false),
                    Disabled = table.Column<bool>(type: "bit", nullable: false),
                    RedeemedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RedeemedUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RedeemedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedeemCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Private = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(42)", maxLength: 42, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsedQuota = table.Column<long>(type: "bigint", nullable: false),
                    UnlimitedQuota = table.Column<bool>(type: "bit", nullable: false),
                    RemainQuota = table.Column<long>(type: "bigint", nullable: false),
                    AccessedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiredTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UnlimitedExpired = table.Column<bool>(type: "bit", nullable: false),
                    Disabled = table.Column<bool>(type: "bit", nullable: false),
                    LimitModels = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhiteIpList = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConsumeToken = table.Column<long>(type: "bigint", nullable: false),
                    RequestCount = table.Column<long>(type: "bigint", nullable: false),
                    ResidualCredit = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ModelManagers",
                columns: new[] { "Id", "AudioOutputRate", "AudioPromptRate", "Available", "CompletionRate", "CreatedAt", "Creator", "Description", "Enable", "Icon", "IsVersion2", "Model", "Modifier", "PromptRate", "QuotaMax", "QuotaType", "Tags", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("03318043-202b-4055-80da-d2ba1a60b6d4"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9928), null, "TTS 1 HD 1106 语音合成模型", true, "OpenAI", false, "tts-1-hd-1106", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("070e5d79-013e-472a-8997-35805a81aed9"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9889), null, "GPT-3.5 Turbo 16k 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k-0613", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("0761caac-b83a-4e9e-8152-e3745a5ff32f"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9908), null, "GPT-4 视觉预览模型", true, "OpenAI", false, "gpt-4-vision-preview", null, 10m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("09e53f04-1dd3-4a43-bf3e-0fdf51b09bcf"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9934), null, "4.0 超级文本模型", true, "Spark", false, "4.0Ultra", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("0c1372ca-4222-4b53-8ca2-8339ed579cb1"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9925), null, "TTS 1 语音合成模型", true, "OpenAI", false, "tts-1", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("0d5a3743-5909-4778-abdb-26b32754ca28"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9940), null, "Claude 2.0 文本模型", true, "Claude", false, "claude-2.0", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1ac18888-e37f-4064-829a-c26f12c9b06e"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9950), null, "Embedding 2 嵌入模型", true, "OpenAI", false, "embedding-2", null, 0.0355m, "", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("1bd6747c-62f2-4260-bfb1-4543aab2cf3e"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9921), null, "Text Embedding 3 Large 嵌入模型", true, "OpenAI", false, "text-embedding-3-large", null, 0.13m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1da524d8-abdf-4e52-9811-88106176ad3f"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9925), null, "Text Embedding Ada 002 嵌入模型", true, "OpenAI", false, "text-embedding-ada-002", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1e57a14b-22d6-4e05-a0a1-dbf6c84dce1e"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9957), null, "GLM 4v 文本模型", true, "ChatGLM", false, "glm-4v", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("212427ae-70f6-4d05-8dd6-7f71609587e8"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9941), null, "Claude 2.1 文本模型", true, "Claude", false, "claude-2.1", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("240ddf1b-a155-4832-b087-4186152e98c4"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9890), null, "GPT-3.5 Turbo Instruct 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-instruct", null, 0.001m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("2870ae1e-8037-49e2-b458-c58aadad7ea7"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9898), null, "GPT-4 32k 0613 文本模型", true, "OpenAI", false, "gpt-4-32k-0613", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("2fa598d6-f04e-4cc8-9a1a-05b29f5585aa"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9908), null, "GPT-4o 文本模型", true, "OpenAI", false, "gpt-4o", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("2fd99c27-c0bb-4fdd-b3fb-93c5535451d1"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9950), null, "DALL-E 3 图像生成模型", true, "OpenAI", false, "dall-e-3", null, 20m, null, 2, "[\"\\u56FE\\u7247\"]", null },
                    { new Guid("3095994e-6d11-449e-a994-4cdcace92787"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9918), null, "Text Curie 001 文本模型", true, "OpenAI", false, "text-curie-001", null, 1m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("31b0e1bf-4999-4d3f-8dc4-94be1641b9ce"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9932), null, "通用文本模型 v3", true, "Spark", false, "generalv3", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("395114cb-b2a0-4b60-ba45-3b1af6a265b3"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9935), null, "ChatGLM Lite 文本模型", true, "ChatGLM", false, "chatglm_lite", null, 0.1429m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("3df6a9ac-52d8-4d0a-9f21-b1b5fc25f8e1"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9953), null, "GLM 3 Turbo 文本模型", true, "ChatGLM", false, "glm-3-turbo", null, 0.355m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("44d07a4b-096f-4029-8517-c6f2cdf482e9"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9956), null, "GLM 4 文本模型", true, "ChatGLM", false, "glm-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("4738871a-09d5-488c-91f7-9f38fcb91bee"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9904), null, "Gemini 1.5 Flash 文本模型", true, "Google", false, "gemini-1.5-flash", null, 0.2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("5301f6bc-5247-4e9a-a003-c7013e645b28"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9897), null, "GPT-4 32k 0314 文本模型", true, "OpenAI", false, "gpt-4-32k-0314", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("5c9e9e1d-818f-4539-9235-88c5d86a0117"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9901), null, "GPT-4 Turbo 文本模型", true, "OpenAI", false, "gpt-4-turbo", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("5f2cfb74-b846-4d62-b7d7-afea4f94384b"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9920), null, "Text Davinci 003 文本模型", true, "OpenAI", false, "text-davinci-003", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("60ce8881-916f-495c-8950-095c24e05a26"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9943), null, "Claude 3 Haiku 20240307 文本模型", true, "Claude", false, "claude-3-haiku-20240307", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("65ed613e-3c9f-41ba-aae9-eb229f7a78a9"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9928), null, "Whisper 1 语音识别模型", true, "OpenAI", false, "whisper-1", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("6b4b97c2-a619-4ef7-89ac-1ec06db681a8"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9917), null, "Text Babbage 001 文本模型", true, "OpenAI", false, "text-babbage-001", null, 0.25m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("6b840268-3045-479f-a6f2-9b26461e72e0"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9894), null, "GPT-4 0613 文本模型", true, "OpenAI", false, "gpt-4-0613", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("703997a5-a4df-4a6d-a5c6-bc085780015c"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9911), null, "GPT-4o Mini 2024-07-18 文本模型", true, "OpenAI", false, "gpt-4o-mini-2024-07-18", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("741b3154-fc6e-4332-9900-f3a5a49bd922"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9905), null, "GPT-4 Turbo 预览文本模型", true, "OpenAI", false, "gpt-4-turbo-preview", null, 5m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("7a65d216-27cd-4bb2-958b-7427652b0eba"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9895), null, "GPT-4 1106 预览文本模型", true, "OpenAI", false, "gpt-4-1106-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("7b80e23b-e32b-4ed1-97d1-298bc8a7e565"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9951), null, "Embedding BERT 512 v1 嵌入模型", true, "OpenAI", false, "embedding-bert-512-v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("7e921858-2d76-4f23-84ed-0d7b58e6862c"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9943), null, "Claude 3.5 Sonnet 20240620 文本模型", true, "Claude", false, "claude-3-5-sonnet-20240620", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("809b7c4b-b2ca-4aff-859f-697453f99c9d"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9927), null, "TTS 1 HD 语音合成模型", true, "OpenAI", false, "tts-1-hd", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("81c24c10-5b7b-49bb-931e-fa089dd6191c"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9909), null, "ChatGPT 4o 最新文本模型", true, "OpenAI", false, "chatgpt-4o-latest", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("81c3d16c-26a7-44b3-a892-0871eb635526"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9932), null, "通用文本模型", true, "Spark", false, "general", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("84cb999e-bd1e-4762-86b3-9778cec374d3"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9885), null, "GPT-3.5 Turbo 0125 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0125", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("878de74b-58ba-44ea-9bc6-11746fdccac2"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9957), null, "GLM 4 全部文本模型", true, "ChatGLM", false, "glm-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("8aef43fa-b9cc-4ca5-aab8-08f81132ec52"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9942), null, "Claude 3 Haiku 文本模型", true, "Claude", false, "claude-3-haiku", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("916fb7fa-fda2-4c7a-9d52-e5fd5d9775a8"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9912), null, "GPT-4o 2024-05-13 文本模型", true, "OpenAI", false, "gpt-4o-2024-05-13", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("92fc3864-fbc6-4c37-9202-bffb3b0fb427"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9905), null, "GPT-4 Turbo 2024-04-09 文本模型", true, "OpenAI", false, "gpt-4-turbo-2024-04-09", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("957b394a-5e00-40c0-a82b-8b5c766f027e"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9929), null, "Hunyuan Lite 文本模型", true, "Hunyuan", false, "hunyuan-lite", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("95ce16fa-6e4f-4dce-86a2-2bda382b4caa"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9944), null, "Claude 3 Sonnet 20240229 文本模型", true, "Claude", false, "claude-3-sonnet-20240229", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("99feedd5-92e2-4e02-ac9a-1a56d6d7dc6b"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9902), null, "Gemini 1.5 Pro 文本模型", true, "Google", false, "gemini-1.5-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a1ddb9db-49a9-4552-b15c-f62361ec0549"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9939), null, "Claude 2 文本模型", true, "Claude", false, "claude-2", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a214eeae-fcec-414c-93f8-d4d19d42e479"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9913), null, "Moonshot v1 128k 文本模型", true, "Moonshot", false, "moonshot-v1-128k", null, 5.06m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a36f3fbf-c911-4002-b615-645728379b1b"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9896), null, "GPT-4 32k 文本模型", true, "OpenAI", false, "gpt-4-32k", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b031c0f8-d94f-4b19-a823-314db62e7862"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9886), null, "GPT-3.5 Turbo 0301 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0301", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b20f2b7e-1ffa-4b01-b4fc-aff18423c196"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9893), null, "GPT-4 0314 文本模型", true, "OpenAI", false, "gpt-4-0314", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b315520b-3254-45ff-8dfa-837204e08595"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9936), null, "ChatGLM Pro 文本模型", true, "ChatGLM", false, "chatglm_pro", null, 0.7143m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b363a633-e27c-4b3a-b884-f8529bfefbb0"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9945), null, "Claude 3 Opus 20240229 文本模型", true, "Claude", false, "claude-3-opus-20240229", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b46e800f-8a81-49a3-84e3-7eae4903a0fe"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9949), null, "DALL-E 2 图像生成模型", true, "OpenAI", false, "dall-e-2", null, 8m, null, 2, "[\"\\u56FE\\u7247\"]", null },
                    { new Guid("b57f71fb-ce39-4cbd-9399-0fc96b399151"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9912), null, "GPT-4o 2024-08-06 文本模型", true, "OpenAI", false, "gpt-4o-2024-08-06", null, 1.25m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("b59eb207-afcc-464c-a5a6-fe94d3129af8"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9890), null, "GPT-4 文本模型", true, "OpenAI", false, "gpt-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b74de8b0-62bb-43b5-af6a-c7a3e5edf146"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9948), null, "Claude Instant 1.2 文本模型", true, "Claude", false, "claude-instant-1.2", null, 0.4m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b9298b20-b276-4b3e-8041-4a515b2bbb15"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9937), null, "ChatGLM Turbo 文本模型", true, "ChatGLM", false, "chatglm_turbo", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("bb2f07bf-2d81-49f2-9a0c-f3f365823e28"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9952), null, "Embedding S1 v1 嵌入模型", true, null, false, "embedding_s1_v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("ca4ba19a-3dde-4a6d-81ec-650f3c6040c0"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9887), null, "GPT-3.5 Turbo 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0613", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("cab85f36-636d-4b59-932d-57f8b3e22bb9"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9933), null, "通用文本模型 v3.5", true, "Spark", false, "generalv3.5", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("cb2a5856-0d6c-4fc7-ba26-1ea7882baec9"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9947), null, "Claude Instant 1 文本模型", true, "Claude", false, "claude-instant-1", null, 0.815m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("cc4f194e-7c1d-4a3f-97bf-861310f68b45"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9924), null, "Text Embedding 3 Small 嵌入模型", true, "OpenAI", false, "text-embedding-3-small", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("cf0638be-f9de-4e0f-8559-a4e1b48aabdf"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9926), null, "TTS 1 1106 语音合成模型", true, "OpenAI", false, "tts-1-1106", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("d4f0dcb0-b316-48c2-92eb-d0a08437c6f6"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9917), null, "Moonshot v1 8k 文本模型", true, "Moonshot", false, "moonshot-v1-8k", null, 1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("da9662ac-ac1f-45d6-b2d2-aef470f2fe8d"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9893), null, "GPT-4 0125 预览文本模型", true, "OpenAI", false, "gpt-4-0125-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("dc466fd5-f135-4fab-b17f-c9c98f0fc150"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9903), null, "Gemini Pro 视觉模型", true, "Google", false, "gemini-pro-vision", null, 2m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("de93fe4d-c648-49c1-b7de-fb97f0cb92cc"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9895), null, "GPT-4 1106 视觉预览模型", true, "OpenAI", false, "gpt-4-1106-vision-preview", null, 10m, "128K", 1, "[\"\\u89C6\\u89C9\",\"\\u6587\\u672C\"]", null },
                    { new Guid("e21f671d-7b95-4517-a755-383c3a7d3209"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9936), null, "ChatGLM 标准文本模型", true, "ChatGLM", false, "chatglm_std", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("e5898b1d-2cb6-4ba1-883e-91dc59effaaf"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9888), null, "GPT-3.5 Turbo 16k 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("e5d4a2e3-8adc-4252-9a78-a695c3b045eb"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9919), null, "Text Davinci 002 文本模型", true, "OpenAI", false, "text-davinci-002", null, 10m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("e65e12c8-0cef-494c-81f1-264df32613d7"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9873), null, "GPT-3.5 Turbo 文本模型", true, "OpenAI", false, "gpt-3.5-turbo", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("e993d85f-17cc-43c2-a222-aacb5cf0fb5b"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9888), null, "GPT-3.5 Turbo 1106 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-1106", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ec8c8827-d75c-49d1-9eb3-693f905bb9ae"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9900), null, "GPT-4 全部文本模型", true, "OpenAI", false, "gpt-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u8054\\u7F51\"]", null },
                    { new Guid("eccfeac0-a30d-4ec0-9e58-462c2c600d26"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9921), null, "Text Davinci Edit 001 文本模型", true, "OpenAI", false, "text-davinci-edit-001", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("f9495fb4-9c89-4635-a365-61eb4d8f8a5a"), null, null, true, null, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9916), null, "Moonshot v1 32k 文本模型", true, "Moonshot", false, "moonshot-v1-32k", null, 2m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("fbd36556-188c-4745-a2d0-a719bfc6a85c"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9902), null, "Gemini Pro 文本模型", true, "Google", false, "gemini-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("fc563669-cd00-4024-a394-4c1cc9961547"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 7, 31, 463, DateTimeKind.Local).AddTicks(9910), null, "GPT-4o Mini 文本模型", true, "OpenAI", false, "gpt-4o-mini", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null }
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
                values: new object[] { "af942a02-e112-4c14-9b69-edb8a81daded", null, new DateTime(2024, 11, 4, 1, 7, 31, 461, DateTimeKind.Local).AddTicks(5399), "e4149acd-b4e8-4f1d-ace6-991d8a88b184", null, false, null, false, "sk-XhpbIyhxN7rhaSvaxcmxuolvXdxQLEKQTLQBQb", "[]", null, "默认Token", 0L, true, true, null, 0L, "[]" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "e4149acd-b4e8-4f1d-ace6-991d8a88b184", null, 0L, new DateTime(2024, 11, 4, 1, 7, 31, 461, DateTimeKind.Local).AddTicks(4966), null, null, "239573049@qq.com", false, false, null, "4d3921f910219fa4cb5646661787a60c", "b300ada2387744d5b39cfc7905381f9c", 0L, 1000000000L, "admin", null, "admin" });

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
