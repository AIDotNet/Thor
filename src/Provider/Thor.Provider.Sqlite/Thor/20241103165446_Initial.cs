using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Thor.Provider.Thor
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
                    ControlAutomatically = table.Column<bool>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Channels", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "ModelManagers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Available = table.Column<bool>(type: "INTEGER", nullable: false),
                    CompletionRate = table.Column<decimal>(type: "TEXT", nullable: true),
                    PromptRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    AudioPromptRate = table.Column<decimal>(type: "TEXT", nullable: true),
                    AudioOutputRate = table.Column<decimal>(type: "TEXT", nullable: true),
                    QuotaType = table.Column<int>(type: "INTEGER", nullable: false),
                    QuotaMax = table.Column<string>(type: "TEXT", nullable: true),
                    Tags = table.Column<string>(type: "TEXT", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: true),
                    IsVersion2 = table.Column<bool>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_ModelManagers", x => x.Id); });

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
                constraints: table => { table.PrimaryKey("PK_ProductPurchaseRecords", x => x.Id); });

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
                constraints: table => { table.PrimaryKey("PK_Products", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "RateLimitModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    WhiteList = table.Column<string>(type: "TEXT", nullable: false),
                    BlackList = table.Column<string>(type: "TEXT", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Strategy = table.Column<string>(type: "TEXT", nullable: false),
                    Limit = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_RateLimitModels", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "RedeemCodes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Quota = table.Column<long>(type: "INTEGER", nullable: false),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    RedeemedTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RedeemedUserId = table.Column<string>(type: "TEXT", nullable: true),
                    RedeemedUserName = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_RedeemCodes", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Private = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Settings", x => x.Key); });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", maxLength: 42, nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    UsedQuota = table.Column<long>(type: "INTEGER", nullable: false),
                    UnlimitedQuota = table.Column<bool>(type: "INTEGER", nullable: false),
                    RemainQuota = table.Column<long>(type: "INTEGER", nullable: false),
                    AccessedTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExpiredTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UnlimitedExpired = table.Column<bool>(type: "INTEGER", nullable: false),
                    Disabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LimitModels = table.Column<string>(type: "TEXT", nullable: false),
                    WhiteIpList = table.Column<string>(type: "TEXT", nullable: false),
                    IsDelete = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Modifier = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Creator = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Tokens", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHas = table.Column<string>(type: "TEXT", nullable: false),
                    Avatar = table.Column<string>(type: "TEXT", nullable: true),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    IsDisabled = table.Column<bool>(type: "INTEGER", nullable: false),
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
                constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });

            migrationBuilder.InsertData(
                table: "ModelManagers",
                columns: new[]
                {
                    "Id", "AudioOutputRate", "AudioPromptRate", "Available", "CompletionRate", "CreatedAt", "Creator",
                    "Description", "Enable", "Icon", "IsVersion2", "Model", "Modifier", "PromptRate", "QuotaMax",
                    "QuotaType", "Tags", "UpdatedAt"
                },
                values: new object[,]
                {
                    {
                        new Guid("098771b5-016e-4678-a567-7f9a201535db"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7556), null,
                        "GPT-3.5 Turbo Instruct 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-instruct", null, 0.001m,
                        "128K", 1, "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("0a4df36c-cd02-46ef-9e17-680426ed2ec9"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7614), null,
                        "GLM 4 文本模型", true, "ChatGLM", false, "glm-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]",
                        null
                    },
                    {
                        new Guid("10a15cec-b840-445d-a3cc-4fd8a25f7e87"), null, null, true, 3m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7571), null,
                        "Gemini 1.5 Flash 文本模型", true, "Google", false, "gemini-1.5-flash", null, 0.2m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("1260ee88-b141-4cc5-aae5-98a7548358fe"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7609), null,
                        "DALL-E 3 图像生成模型", true, "OpenAI", false, "dall-e-3", null, 20m, null, 2,
                        "[\"\\u56FE\\u7247\"]", null
                    },
                    {
                        new Guid("139f47b8-fecb-4e87-b573-b211554dc4ce"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7588), null,
                        "TTS 1 语音合成模型", true, "OpenAI", false, "tts-1", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]",
                        null
                    },
                    {
                        new Guid("1bea154a-9ebb-4f25-9848-4fe742a6b826"), null, null, true, 2m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7567), null,
                        "Gemini 1.5 Pro 文本模型", true, "Google", false, "gemini-1.5-pro", null, 2m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("1c2d8edc-8612-4b46-bf43-d441aecd3a8d"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7558), null,
                        "GPT-4 0314 文本模型", true, "OpenAI", false, "gpt-4-0314", null, 15m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("1d072353-a8aa-407b-937a-862686cc2ddf"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7600), null,
                        "Claude 2 文本模型", true, "Claude", false, "claude-2", null, 7.5m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("22381ccf-3a5c-4a1a-896e-0631c2797d10"), null, null, true, 2m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7594), null, "通用文本模型 v3",
                        true, "Spark", false, "generalv3", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("2404a79a-4d64-4acc-9b06-8a8221aa7f67"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7592), null,
                        "Whisper 1 语音识别模型", true, "OpenAI", false, "whisper-1", null, 15m, null, 2,
                        "[\"\\u97F3\\u9891\"]", null
                    },
                    {
                        new Guid("243f113a-f6f2-40eb-9479-84198ebcbd0f"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7598), null,
                        "ChatGLM Pro 文本模型", true, "ChatGLM", false, "chatglm_pro", null, 0.7143m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("2472a22f-97b2-43fa-8d75-6fadb2ac952f"), null, null, true, 4m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7578), null,
                        "GPT-4o 2024-05-13 文本模型", true, "OpenAI", false, "gpt-4o-2024-05-13", null, 3m, "128K", 1,
                        "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null
                    },
                    {
                        new Guid("26f5424a-5399-4704-a043-aa35e455a5d5"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7610), null,
                        "Embedding 2 嵌入模型", true, "OpenAI", false, "embedding-2", null, 0.0355m, "", 1,
                        "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null
                    },
                    {
                        new Guid("2b6cca4d-306a-483d-94eb-3266d4fe5856"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7599), null,
                        "ChatGLM 标准文本模型", true, "ChatGLM", false, "chatglm_std", null, 0.3572m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("2c2b59f2-49a2-4222-8055-63807d3261e3"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7612), null,
                        "Embedding S1 v1 嵌入模型", true, null, false, "embedding_s1_v1", null, 0.1m, "128K", 1,
                        "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null
                    },
                    {
                        new Guid("2c7807ad-30ee-4398-af00-8a1342ba7ffc"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7564), null,
                        "GPT-4 32k 0314 文本模型", true, "OpenAI", false, "gpt-4-32k-0314", null, 30m, "32K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("31a89879-763e-4a7c-80e4-250ffcca6cea"), null, null, true, 5m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7602), null,
                        "Claude 3 Haiku 文本模型", true, "Claude", false, "claude-3-haiku", null, 0.5m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("31b05563-a34b-4e9e-ab69-92c4b8ce9b5a"), null, null, true, 4m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7575), null,
                        "GPT-4o Mini 2024-07-18 文本模型", true, "OpenAI", false, "gpt-4o-mini-2024-07-18", null, 0.07m,
                        "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null
                    },
                    {
                        new Guid("320b5399-3ec7-4e6c-8297-10ae88d170c5"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7565), null,
                        "GPT-4 32k 0613 文本模型", true, "OpenAI", false, "gpt-4-32k-0613", null, 30m, "32K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("32f9a31b-791e-44bf-9900-fa37de478e91"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7581), null,
                        "Text Babbage 001 文本模型", true, "OpenAI", false, "text-babbage-001", null, 0.25m, "8K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("3408987a-fbb0-4aa6-b767-a8a718290120"), null, null, true, 3m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7573), null,
                        "GPT-4o 文本模型", true, "OpenAI", false, "gpt-4o", null, 3m, "128K", 1,
                        "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null
                    },
                    {
                        new Guid("347678c1-064c-4b87-a08c-3bc649b66b21"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7566), null,
                        "GPT-4 Turbo 文本模型", true, "OpenAI", false, "gpt-4-turbo", null, 5m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("383b5635-b4d6-4dfc-9a3f-37ed6cfc883b"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7571), null,
                        "GPT-4 Turbo 2024-04-09 文本模型", true, "OpenAI", false, "gpt-4-turbo-2024-04-09", null, 5m,
                        "128K", 1, "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("3919a4a6-e984-4a2f-8be0-c9a498f4ba35"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7550), null,
                        "GPT-3.5 Turbo 1106 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-1106", null, 0.25m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("3b467e17-3d9b-4596-898a-ecbc705ccb27"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7585), null,
                        "Text Davinci 003 文本模型", true, "OpenAI", false, "text-davinci-003", null, 10m, "8K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("4384462d-b5c4-4d85-a7f5-f1325716bcf9"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7560), null,
                        "GPT-4 1106 预览文本模型", true, "OpenAI", false, "gpt-4-1106-preview", null, 5m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("449f9e8c-6593-4196-855c-6e213913dfc1"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7612), null,
                        "Embedding BERT 512 v1 嵌入模型", true, "OpenAI", false, "embedding-bert-512-v1", null, 0.1m,
                        "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null
                    },
                    {
                        new Guid("48f83c01-15d6-4760-91f6-4cdb3ce36708"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7614), null,
                        "GLM 4 全部文本模型", true, "ChatGLM", false, "glm-4-all", null, 30m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("49260289-66af-4d6e-8361-9fb543989a10"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7589), null,
                        "TTS 1 HD 语音合成模型", true, "OpenAI", false, "tts-1-hd", null, 15m, null, 2,
                        "[\"\\u97F3\\u9891\"]", null
                    },
                    {
                        new Guid("4b7a3517-b49a-4451-9fe8-c03d7a855b37"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7559), null,
                        "GPT-4 0613 文本模型", true, "OpenAI", false, "gpt-4-0613", null, 15m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("503e08dd-f8f2-424a-924c-2a5011078fd0"), null, null, true, 5m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7605), null,
                        "Claude 3.5 Sonnet 20240620 文本模型", true, "Claude", false, "claude-3-5-sonnet-20240620", null,
                        3m, "128K", 1, "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("53498da6-0fe6-47e5-b38c-917b7167f402"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7583), null,
                        "Text Davinci 002 文本模型", true, "OpenAI", false, "text-davinci-002", null, 10m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("54a8d048-ad0a-467e-a5f3-9cb8fe9de0c8"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7579), null,
                        "Moonshot v1 128k 文本模型", true, "Moonshot", false, "moonshot-v1-128k", null, 5.06m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("5c44361c-2766-4634-8fea-8e6cee83d518"), null, null, true, 4m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7578), null,
                        "GPT-4o 2024-08-06 文本模型", true, "OpenAI", false, "gpt-4o-2024-08-06", null, 1.25m, "128K", 1,
                        "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null
                    },
                    {
                        new Guid("6060f817-4e27-4d15-88cd-e8b27383a534"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7556), null,
                        "GPT-3.5 Turbo 16k 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k-0613", null, 0.75m,
                        "16K", 1, "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("62ba3a7e-1d5b-487f-9ab8-0297bf8cf59f"), null, null, true, 3m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7568), null,
                        "Gemini Pro 文本模型", true, "Google", false, "gemini-pro", null, 2m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("7f21d0bb-8767-4e2b-a1b9-2a947df0987d"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7601), null,
                        "Claude 2.0 文本模型", true, "Claude", false, "claude-2.0", null, 7.5m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("841034e0-f45b-4f35-9eaa-978e43973186"), null, null, true, 5m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7606), null,
                        "Claude 3 Sonnet 20240229 文本模型", true, "Claude", false, "claude-3-sonnet-20240229", null, 3m,
                        "128K", 1, "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("91574c71-ae93-432a-8cf9-a1c1d0f85122"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7580), null,
                        "Moonshot v1 8k 文本模型", true, "Moonshot", false, "moonshot-v1-8k", null, 1m, "8K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("96e57721-1b69-47a2-a596-ef00856d64c8"), null, null, true, 2m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7593), null, "通用文本模型",
                        true, "Spark", false, "general", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("9c5bb740-1e1b-447d-a0cd-ed521cfa0fe1"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7593), null,
                        "Hunyuan Lite 文本模型", true, "Hunyuan", false, "hunyuan-lite", null, 0.75m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("9c5f4ed6-721f-4fd9-b1b4-4670db54878a"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7566), null,
                        "GPT-4 全部文本模型", true, "OpenAI", false, "gpt-4-all", null, 30m, "128K", 1,
                        "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u8054\\u7F51\"]", null
                    },
                    {
                        new Guid("9e261636-55c4-46a3-a4a4-90a703e95b33"), null, null, true, 4m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7575), null,
                        "GPT-4o Mini 文本模型", true, "OpenAI", false, "gpt-4o-mini", null, 0.07m, "128K", 1,
                        "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null
                    },
                    {
                        new Guid("a394cf20-b349-4efc-b411-d8b98cdc50ae"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7541), null,
                        "GPT-3.5 Turbo 文本模型", true, "OpenAI", false, "gpt-3.5-turbo", null, 0.75m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("a65dbfc3-1125-434d-a19b-dc96f51c2b48"), null, null, true, 5m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7606), null,
                        "Claude 3 Opus 20240229 文本模型", true, "Claude", false, "claude-3-opus-20240229", null, 30m,
                        "128K", 1, "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("a7ac97b6-cfe4-4500-bf0d-81184da33940"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7589), null,
                        "TTS 1 1106 语音合成模型", true, "OpenAI", false, "tts-1-1106", null, 7.5m, null, 1,
                        "[\"\\u97F3\\u9891\"]", null
                    },
                    {
                        new Guid("ad405a9d-e3b3-487b-9c92-8ed7fe195188"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7601), null,
                        "Claude 2.1 文本模型", true, "Claude", false, "claude-2.1", null, 7.5m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("ad4f7f87-8612-414b-a94a-ff2c8c2c7ae9"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7561), null,
                        "GPT-4 1106 视觉预览模型", true, "OpenAI", false, "gpt-4-1106-vision-preview", null, 10m, "128K", 1,
                        "[\"\\u89C6\\u89C9\",\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("b58a6f38-64c1-45af-adb9-c35f14d5d5d7"), null, null, true, 5m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7603), null,
                        "Claude 3 Haiku 20240307 文本模型", true, "Claude", false, "claude-3-haiku-20240307", null, 0.5m,
                        "128K", 1, "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("b6b09e74-d747-4ee1-8c15-87ee67a5781c"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7607), null,
                        "Claude Instant 1 文本模型", true, "Claude", false, "claude-instant-1", null, 0.815m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("baaad077-bc70-4b3a-87af-5e2aa3768163"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7613), null,
                        "GLM 3 Turbo 文本模型", true, "ChatGLM", false, "glm-3-turbo", null, 0.355m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("bb2060a7-c210-4295-836d-22f0766be6ad"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7558), null,
                        "GPT-4 0125 预览文本模型", true, "OpenAI", false, "gpt-4-0125-preview", null, 5m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("bf1e74b8-6e6d-4af0-8910-b0df5eb95fbb"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7608), null,
                        "DALL-E 2 图像生成模型", true, "OpenAI", false, "dall-e-2", null, 8m, null, 2, "[\"\\u56FE\\u7247\"]",
                        null
                    },
                    {
                        new Guid("c2d0fd3c-c0ce-46f2-967b-d30e00f233d7"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7550), null,
                        "GPT-3.5 Turbo 16k 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k", null, 0.75m, "16K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("c32147b0-2afa-4187-8e38-612bdeab1865"), null, null, true, 4m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7574), null,
                        "ChatGPT 4o 最新文本模型", true, "OpenAI", false, "chatgpt-4o-latest", null, 3m, "128K", 1,
                        "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null
                    },
                    {
                        new Guid("c4e1fbc3-dc5f-4456-ab6c-f7ef0c8e85c4"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7563), null,
                        "GPT-4 32k 文本模型", true, "OpenAI", false, "gpt-4-32k", null, 30m, "32K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("c70c72c6-55ab-458f-801b-357a77895e03"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7572), null,
                        "GPT-4 Turbo 预览文本模型", true, "OpenAI", false, "gpt-4-turbo-preview", null, 5m, "8K", 1,
                        "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null
                    },
                    {
                        new Guid("c9676a5b-1921-4a0e-95d3-8823845fc360"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7557), null,
                        "GPT-4 文本模型", true, "OpenAI", false, "gpt-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("c9dc25bd-f509-439b-84b4-e9e3d57cbc55"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7587), null,
                        "Text Embedding Ada 002 嵌入模型", true, "OpenAI", false, "text-embedding-ada-002", null, 0.1m,
                        "8K", 1, "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("cf64b6e2-3203-48af-8290-3e758bf86438"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7548), null,
                        "GPT-3.5 Turbo 0301 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0301", null, 0.75m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("cf993a87-7b27-4def-9c4d-5ef961afd10f"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7586), null,
                        "Text Embedding 3 Large 嵌入模型", true, "OpenAI", false, "text-embedding-3-large", null, 0.13m,
                        "8K", 1, "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("d058266d-5b04-4040-9c3f-17dc451e18a5"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7585), null,
                        "Text Davinci Edit 001 文本模型", true, "OpenAI", false, "text-davinci-edit-001", null, 10m, "8K",
                        1, "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("d0dcfb78-0cc6-4727-a77d-1d0d80984b4a"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7580), null,
                        "Moonshot v1 32k 文本模型", true, "Moonshot", false, "moonshot-v1-32k", null, 2m, "32K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("d9b314f6-3982-4e9b-bf2e-682ec4027b2b"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7587), null,
                        "Text Embedding 3 Small 嵌入模型", true, "OpenAI", false, "text-embedding-3-small", null, 0.1m,
                        "8K", 1, "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("dd4f1052-8f12-4d2c-831c-e6b60a70de46"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7608), null,
                        "Claude Instant 1.2 文本模型", true, "Claude", false, "claude-instant-1.2", null, 0.4m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("ddf3e62b-63ee-437f-ad8c-e7a92b9f4017"), null, null, true, 3m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7568), null,
                        "Gemini Pro 视觉模型", true, "Google", false, "gemini-pro-vision", null, 2m, "128K", 1,
                        "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null
                    },
                    {
                        new Guid("de9d7abd-1c3d-40e5-a274-ef7eee329d4b"), null, null, true, 3m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7595), null,
                        "4.0 超级文本模型", true, "Spark", false, "4.0Ultra", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]",
                        null
                    },
                    {
                        new Guid("e4223225-2882-4362-927e-bd86b6908caa"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7547), null,
                        "GPT-3.5 Turbo 0125 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0125", null, 0.25m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("e87432d5-7a5a-4d87-ac0b-cdc8cf0f3d01"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7596), null,
                        "ChatGLM Lite 文本模型", true, "ChatGLM", false, "chatglm_lite", null, 0.1429m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("ebecbde8-97d0-4e73-8e11-efe9a63db88a"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7599), null,
                        "ChatGLM Turbo 文本模型", true, "ChatGLM", false, "chatglm_turbo", null, 0.3572m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("f02cbfb5-fa03-406c-af2e-0c832c879628"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7573), null,
                        "GPT-4 视觉预览模型", true, "OpenAI", false, "gpt-4-vision-preview", null, 10m, "8K", 1,
                        "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null
                    },
                    {
                        new Guid("f235aca0-ab50-4ce4-96b6-70cc1ad24f89"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7615), null,
                        "GLM 4v 文本模型", true, "ChatGLM", false, "glm-4v", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]",
                        null
                    },
                    {
                        new Guid("f2fdbfeb-88c7-487e-a86f-98cbbdf6cb1c"), null, null, true, 2m,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7595), null,
                        "通用文本模型 v3.5", true, "Spark", false, "generalv3.5", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]",
                        null
                    },
                    {
                        new Guid("f390e8a3-21a4-4b82-b489-a5d12732547b"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7582), null,
                        "Text Curie 001 文本模型", true, "OpenAI", false, "text-curie-001", null, 1m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("f93940fb-093a-4623-9d1a-ebe541286976"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7549), null,
                        "GPT-3.5 Turbo 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0613", null, 0.75m, "128K", 1,
                        "[\"\\u6587\\u672C\"]", null
                    },
                    {
                        new Guid("faa32d7e-ace6-42f9-8619-afa398ca4418"), null, null, true, null,
                        new DateTime(2024, 11, 4, 0, 54, 46, 431, DateTimeKind.Local).AddTicks(7591), null,
                        "TTS 1 HD 1106 语音合成模型", true, "OpenAI", false, "tts-1-hd-1106", null, 15m, null, 2,
                        "[\"\\u97F3\\u9891\"]", null
                    }
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
                    {
                        "Setting:OtherSetting:IndexContent", "首页内容", false,
                        "AI DotNet API 提供更强的兼容，将更多的AI平台接入到AI DotNet API中，让AI集成更加简单。"
                    },
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
                columns: new[]
                {
                    "Id", "AccessedTime", "CreatedAt", "Creator", "DeletedAt", "Disabled", "ExpiredTime", "IsDelete",
                    "Key", "LimitModels", "Modifier", "Name", "RemainQuota", "UnlimitedExpired", "UnlimitedQuota",
                    "UpdatedAt", "UsedQuota", "WhiteIpList"
                },
                values: new object[]
                {
                    "5fbd0afb-68a8-4fcd-87aa-c67cb8bfa517", null,
                    new DateTime(2024, 11, 4, 0, 54, 46, 429, DateTimeKind.Local).AddTicks(7940),
                    "2e1e05ba-586e-4578-a5e9-09000f9dbe70", null, false, null, false,
                    "sk-AWwIJaNkFB2cQlsHhVk2y1Dek9RbZI6bdeJX6e", "[]", null, "默认Token", 0L, true, true, null, 0L, "[]"
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[]
                {
                    "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete",
                    "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role",
                    "UpdatedAt", "UserName"
                },
                values: new object[]
                {
                    "2e1e05ba-586e-4578-a5e9-09000f9dbe70", null, 0L,
                    new DateTime(2024, 11, 4, 0, 54, 46, 429, DateTimeKind.Local).AddTicks(7468), null, null,
                    "239573049@qq.com", false, false, null, "a806333f66eeee6bff96c19531379948",
                    "489683420dc246b69637a4dd86e03a2e", 0L, 1000000000L, "admin", null, "admin"
                });

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