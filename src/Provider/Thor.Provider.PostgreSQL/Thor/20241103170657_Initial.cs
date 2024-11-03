using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Thor.Provider.PostgreSQL.Thor
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
                    Id = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    ResponseTime = table.Column<long>(type: "bigint", nullable: true),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Models = table.Column<string>(type: "text", nullable: false),
                    Other = table.Column<string>(type: "text", nullable: false),
                    Disable = table.Column<bool>(type: "boolean", nullable: false),
                    Extension = table.Column<string>(type: "text", nullable: false),
                    Quota = table.Column<int>(type: "integer", nullable: false),
                    RemainQuota = table.Column<long>(type: "bigint", nullable: false),
                    ControlAutomatically = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modifier = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Creator = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModelManagers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Available = table.Column<bool>(type: "boolean", nullable: false),
                    CompletionRate = table.Column<decimal>(type: "numeric", nullable: true),
                    PromptRate = table.Column<decimal>(type: "numeric", nullable: false),
                    AudioPromptRate = table.Column<decimal>(type: "numeric", nullable: true),
                    AudioOutputRate = table.Column<decimal>(type: "numeric", nullable: true),
                    QuotaType = table.Column<int>(type: "integer", nullable: false),
                    QuotaMax = table.Column<string>(type: "text", nullable: true),
                    Tags = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    IsVersion2 = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modifier = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Creator = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelManagers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductPurchaseRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    RemainQuota = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modifier = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Creator = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPurchaseRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    RemainQuota = table.Column<long>(type: "bigint", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modifier = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Creator = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RateLimitModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    WhiteList = table.Column<string>(type: "text", nullable: false),
                    BlackList = table.Column<string>(type: "text", nullable: false),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Strategy = table.Column<string>(type: "text", nullable: false),
                    Limit = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modifier = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Creator = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateLimitModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RedeemCodes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Quota = table.Column<long>(type: "bigint", nullable: false),
                    Disabled = table.Column<bool>(type: "boolean", nullable: false),
                    RedeemedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RedeemedUserId = table.Column<string>(type: "text", nullable: true),
                    RedeemedUserName = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modifier = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Creator = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedeemCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Private = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Key = table.Column<string>(type: "character varying(42)", maxLength: 42, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UsedQuota = table.Column<long>(type: "bigint", nullable: false),
                    UnlimitedQuota = table.Column<bool>(type: "boolean", nullable: false),
                    RemainQuota = table.Column<long>(type: "bigint", nullable: false),
                    AccessedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiredTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UnlimitedExpired = table.Column<bool>(type: "boolean", nullable: false),
                    Disabled = table.Column<bool>(type: "boolean", nullable: false),
                    LimitModels = table.Column<string>(type: "text", nullable: false),
                    WhiteIpList = table.Column<string>(type: "text", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modifier = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Creator = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PasswordHas = table.Column<string>(type: "text", nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConsumeToken = table.Column<long>(type: "bigint", nullable: false),
                    RequestCount = table.Column<long>(type: "bigint", nullable: false),
                    ResidualCredit = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modifier = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Creator = table.Column<string>(type: "text", nullable: true)
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
                    { new Guid("009d872e-7ded-48ed-acd7-6418f987fd4f"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7709), null, "Embedding BERT 512 v1 嵌入模型", true, "OpenAI", false, "embedding-bert-512-v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("01ff3f29-c00c-45bc-b377-c573100728b3"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7674), null, "GPT-4o 2024-05-13 文本模型", true, "OpenAI", false, "gpt-4o-2024-05-13", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("0296b929-ce40-4df0-a458-2964e0797a59"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7687), null, "TTS 1 HD 语音合成模型", true, "OpenAI", false, "tts-1-hd", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("04d0bb5b-e3d1-4623-b248-f6e451f5687a"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7702), null, "Claude 3 Sonnet 20240229 文本模型", true, "Claude", false, "claude-3-sonnet-20240229", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("054ba477-0ecf-4112-bf95-c5b65ad9c92f"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7680), null, "Text Curie 001 文本模型", true, "OpenAI", false, "text-curie-001", null, 1m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("056cd3ed-ba1c-4c2c-81f6-11f807dac4b0"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7666), null, "Gemini Pro 视觉模型", true, "Google", false, "gemini-pro-vision", null, 2m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("07a1527a-9c98-4433-80d0-df44b75d84db"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7688), null, "TTS 1 HD 1106 语音合成模型", true, "OpenAI", false, "tts-1-hd-1106", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("08397cab-7c87-4ab1-94f0-adab822ee3de"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7680), null, "Text Davinci 002 文本模型", true, "OpenAI", false, "text-davinci-002", null, 10m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("0e739669-2332-4eff-8fb4-c4eff028d0d2"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7698), null, "Claude 2.1 文本模型", true, "Claude", false, "claude-2.1", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("10222f21-57b4-4f16-9f8f-8cd707cc63f4"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7663), null, "GPT-4 Turbo 文本模型", true, "OpenAI", false, "gpt-4-turbo", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("138bc817-0215-413f-96d5-7a75192a3a7d"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7678), null, "Text Babbage 001 文本模型", true, "OpenAI", false, "text-babbage-001", null, 0.25m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("15d71b93-d34c-43a9-9565-bb8d0da5ddf2"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7685), null, "TTS 1 语音合成模型", true, "OpenAI", false, "tts-1", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("1aed309d-8653-415d-9ef3-f4bb59532ae5"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7667), null, "Gemini 1.5 Flash 文本模型", true, "Google", false, "gemini-1.5-flash", null, 0.2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("24d33436-0441-49fd-b526-a3a16c64742d"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7711), null, "GLM 4 全部文本模型", true, "ChatGLM", false, "glm-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("27273083-1c87-449b-93f6-3130a3f7e1d9"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7669), null, "GPT-4 视觉预览模型", true, "OpenAI", false, "gpt-4-vision-preview", null, 10m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("28f3ab06-6c12-4691-8c69-b09733010306"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7654), null, "GPT-4 文本模型", true, "OpenAI", false, "gpt-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("2c88843e-833b-4e34-9879-4a622f204db5"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7658), null, "GPT-4 1106 预览文本模型", true, "OpenAI", false, "gpt-4-1106-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("36e1ae6c-2d64-4850-872d-fe0c2c0128c8"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7660), null, "GPT-4 32k 文本模型", true, "OpenAI", false, "gpt-4-32k", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("40989633-c854-4abe-86ff-38c44e09dba7"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7668), null, "GPT-4 Turbo 2024-04-09 文本模型", true, "OpenAI", false, "gpt-4-turbo-2024-04-09", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("413489f4-ec1d-42be-b6ff-4108dd43754c"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7654), null, "GPT-4 0125 预览文本模型", true, "OpenAI", false, "gpt-4-0125-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("4451bcc0-09d9-46db-b03e-233bd9cbf934"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7691), null, "通用文本模型 v3", true, "Spark", false, "generalv3", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("45247d28-25e2-4d88-8558-f6db624f81d1"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7677), null, "Moonshot v1 8k 文本模型", true, "Moonshot", false, "moonshot-v1-8k", null, 1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("47892d02-9985-4691-b99b-7d88b9bba2ed"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7711), null, "GLM 4 文本模型", true, "ChatGLM", false, "glm-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("4f7c05b0-ee5e-48da-954d-9937dea54ad4"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7676), null, "Moonshot v1 128k 文本模型", true, "Moonshot", false, "moonshot-v1-128k", null, 5.06m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("52c559d9-ccfc-4a76-af0f-a03dc8e3a954"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7708), null, "Embedding 2 嵌入模型", true, "OpenAI", false, "embedding-2", null, 0.0355m, "", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("52d8d182-d91d-478e-b8cd-97650071d54f"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7656), null, "GPT-4 0613 文本模型", true, "OpenAI", false, "gpt-4-0613", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("54accf82-ce76-4b22-88ae-1bbf51ea8158"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7673), null, "GPT-4o Mini 文本模型", true, "OpenAI", false, "gpt-4o-mini", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("55d9c543-d795-4d2f-af5f-a316acbf7a68"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7704), null, "Claude Instant 1.2 文本模型", true, "Claude", false, "claude-instant-1.2", null, 0.4m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("5a0d1043-ea45-499e-bdee-59641b03033b"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7693), null, "4.0 超级文本模型", true, "Spark", false, "4.0Ultra", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("5d5c417a-ff35-4e09-b420-69071a3d3310"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7689), null, "Whisper 1 语音识别模型", true, "OpenAI", false, "whisper-1", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("5d8c5449-e6f7-4b16-b7ae-e56d525a8356"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7676), null, "Moonshot v1 32k 文本模型", true, "Moonshot", false, "moonshot-v1-32k", null, 2m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("5e7b1222-1d8e-4c67-90bd-372197d04a2c"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7624), null, "GPT-3.5 Turbo 文本模型", true, "OpenAI", false, "gpt-3.5-turbo", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("6bb25f74-2b9c-463c-a0a4-2765f547f49c"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7673), null, "GPT-4o Mini 2024-07-18 文本模型", true, "OpenAI", false, "gpt-4o-mini-2024-07-18", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("726fac2a-d0df-4217-94bd-99944bddf0b0"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7702), null, "Claude 3.5 Sonnet 20240620 文本模型", true, "Claude", false, "claude-3-5-sonnet-20240620", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("75cd2bdd-4771-4a06-a440-b8cb0bf9ac4c"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7652), null, "GPT-3.5 Turbo 16k 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("76745063-1422-4107-b5d9-27acb8cdc173"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7662), null, "GPT-4 全部文本模型", true, "OpenAI", false, "gpt-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u8054\\u7F51\"]", null },
                    { new Guid("7b05e674-304c-41c1-8899-18f4784a0cd1"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7695), null, "ChatGLM Pro 文本模型", true, "ChatGLM", false, "chatglm_pro", null, 0.7143m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("7b34f0ac-65bc-42b9-bd10-0c151c59d5ee"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7653), null, "GPT-3.5 Turbo Instruct 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-instruct", null, 0.001m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("7d4ac93b-200d-4f87-a85c-d6212c19baf1"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7683), null, "Text Embedding 3 Small 嵌入模型", true, "OpenAI", false, "text-embedding-3-small", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("849fe8df-1344-4e1f-b276-484e8ee23466"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7661), null, "GPT-4 32k 0613 文本模型", true, "OpenAI", false, "gpt-4-32k-0613", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("86b0bcfc-4220-4942-a5c9-0396347c051a"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7670), null, "GPT-4o 文本模型", true, "OpenAI", false, "gpt-4o", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("89aea156-24ee-49d5-bb31-a6b2fe1b3a9c"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7712), null, "GLM 4v 文本模型", true, "ChatGLM", false, "glm-4v", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("8eb31887-8365-46c4-882e-9a22280650fd"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7703), null, "Claude 3 Opus 20240229 文本模型", true, "Claude", false, "claude-3-opus-20240229", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("8fa47d18-a994-4bbc-ba76-a071c462f661"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7661), null, "GPT-4 32k 0314 文本模型", true, "OpenAI", false, "gpt-4-32k-0314", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("907bcf12-0996-442d-b81d-c53b364b8179"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7682), null, "Text Davinci Edit 001 文本模型", true, "OpenAI", false, "text-davinci-edit-001", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("9462a339-2ecd-4394-a4c7-aef816fd39f6"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7697), null, "Claude 2 文本模型", true, "Claude", false, "claude-2", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("998ef7a5-2292-4c66-8e6f-77ee1a08c248"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7696), null, "ChatGLM 标准文本模型", true, "ChatGLM", false, "chatglm_std", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("9a44324a-997e-4358-aec4-4e789192acd3"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7634), null, "GPT-3.5 Turbo 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0613", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("9a5e224c-4cfe-4058-b09e-6252cc1bdf74"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7707), null, "DALL-E 3 图像生成模型", true, "OpenAI", false, "dall-e-3", null, 20m, null, 2, "[\"\\u56FE\\u7247\"]", null },
                    { new Guid("9c0fbf7b-ff78-47ae-a3cf-be4902a6a9f1"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7691), null, "通用文本模型 v3.5", true, "Spark", false, "generalv3.5", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("9d3b34f9-74ea-4a2a-892e-19995f5ec2d3"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7682), null, "Text Embedding 3 Large 嵌入模型", true, "OpenAI", false, "text-embedding-3-large", null, 0.13m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a646afce-8641-4ae1-902b-cd59b73fed01"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7710), null, "GLM 3 Turbo 文本模型", true, "ChatGLM", false, "glm-3-turbo", null, 0.355m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a762c0d7-2a43-4305-8cbe-ed87822398ac"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7696), null, "ChatGLM Turbo 文本模型", true, "ChatGLM", false, "chatglm_turbo", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a87ab7db-ad64-406c-a8aa-f38c2dd7e06d"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7675), null, "GPT-4o 2024-08-06 文本模型", true, "OpenAI", false, "gpt-4o-2024-08-06", null, 1.25m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("ac0625c8-bb9a-46ea-9de5-d0e010a04cf8"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7704), null, "Claude Instant 1 文本模型", true, "Claude", false, "claude-instant-1", null, 0.815m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b1e879f6-c0e5-4c36-b8be-fbabb86b14eb"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7652), null, "GPT-3.5 Turbo 16k 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k-0613", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b252e18d-b400-4cc5-93e2-fb7a25202ec0"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7666), null, "Gemini Pro 文本模型", true, "Google", false, "gemini-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b61711ff-19ec-4ad0-b2c8-925a94d13e05"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7664), null, "Gemini 1.5 Pro 文本模型", true, "Google", false, "gemini-1.5-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b7048614-36df-455d-8617-2d209e4c34ab"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7701), null, "Claude 3 Haiku 20240307 文本模型", true, "Claude", false, "claude-3-haiku-20240307", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b8d0d3aa-df8f-4089-8ad4-8d753e1fd882"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7669), null, "GPT-4 Turbo 预览文本模型", true, "OpenAI", false, "gpt-4-turbo-preview", null, 5m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("ba85c2ee-f1c9-4fe4-a7dc-8cbca1b271f2"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7655), null, "GPT-4 0314 文本模型", true, "OpenAI", false, "gpt-4-0314", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("c36364e6-9b2b-4476-bcd1-98d7ee2de279"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7659), null, "GPT-4 1106 视觉预览模型", true, "OpenAI", false, "gpt-4-1106-vision-preview", null, 10m, "128K", 1, "[\"\\u89C6\\u89C9\",\"\\u6587\\u672C\"]", null },
                    { new Guid("c73549ec-a493-4560-be90-6794bfaf6ccd"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7671), null, "ChatGPT 4o 最新文本模型", true, "OpenAI", false, "chatgpt-4o-latest", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("c81b1a2e-d13d-4a58-87da-a5e8360cbc60"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7684), null, "Text Embedding Ada 002 嵌入模型", true, "OpenAI", false, "text-embedding-ada-002", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("c985873d-6b29-403e-be38-2faef58aa10f"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7705), null, "DALL-E 2 图像生成模型", true, "OpenAI", false, "dall-e-2", null, 8m, null, 2, "[\"\\u56FE\\u7247\"]", null },
                    { new Guid("ca529992-c50b-4bb2-8798-1fa8ac0984b4"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7633), null, "GPT-3.5 Turbo 0125 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0125", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("d13fe35b-d975-443c-8a70-0176ce8fdd36"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7651), null, "GPT-3.5 Turbo 1106 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-1106", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("d5a7955b-c07f-4d5b-acb5-b2cfbacfac34"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7694), null, "ChatGLM Lite 文本模型", true, "ChatGLM", false, "chatglm_lite", null, 0.1429m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("d8f649f7-e1bb-4cdf-a6a0-ca410f1d370d"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7634), null, "GPT-3.5 Turbo 0301 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0301", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ddd9ba02-73ad-4d46-8deb-bdadc736efc6"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7690), null, "通用文本模型", true, "Spark", false, "general", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("e615c373-cb1a-4cc7-9e1c-b9ce5aa3eb70"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7689), null, "Hunyuan Lite 文本模型", true, "Hunyuan", false, "hunyuan-lite", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("f3f191e1-d1a9-4439-b9d0-b6ddbab52b95"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7709), null, "Embedding S1 v1 嵌入模型", true, null, false, "embedding_s1_v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("f8fd18cc-754c-4e2b-9a75-851f312c24b4"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7681), null, "Text Davinci 003 文本模型", true, "OpenAI", false, "text-davinci-003", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("fafb1e1c-eb43-4142-8a1c-8ee8ccaab503"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7698), null, "Claude 2.0 文本模型", true, "Claude", false, "claude-2.0", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("fe9cbb87-250f-4157-878b-6b1687cdd345"), null, null, true, null, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7687), null, "TTS 1 1106 语音合成模型", true, "OpenAI", false, "tts-1-1106", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("ff88870b-a15e-455c-be79-65d47a358da3"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 6, 57, 354, DateTimeKind.Local).AddTicks(7700), null, "Claude 3 Haiku 文本模型", true, "Claude", false, "claude-3-haiku", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null }
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
                values: new object[] { "324ed167-a7b8-4fe1-8dca-f608f6ff93ec", null, new DateTime(2024, 11, 4, 1, 6, 57, 352, DateTimeKind.Local).AddTicks(3391), "a0581ea1-af3a-4d78-8501-b832f6017b98", null, false, null, false, "sk-aAtCeKnf0XMvVCb8ATfSyafvMheD25cs6bwYdH", "[]", null, "默认Token", 0L, true, true, null, 0L, "[]" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "a0581ea1-af3a-4d78-8501-b832f6017b98", null, 0L, new DateTime(2024, 11, 4, 1, 6, 57, 352, DateTimeKind.Local).AddTicks(2940), null, null, "239573049@qq.com", false, false, null, "659a41980932418aeb4b1245ba6a792a", "0519a14e45b343a2ae8b38fcf9c18a06", 0L, 1000000000L, "admin", null, "admin" });

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
