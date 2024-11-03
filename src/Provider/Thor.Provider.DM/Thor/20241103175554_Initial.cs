using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Thor.Provider.DM.Thor
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
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Order = table.Column<int>(type: "INT", nullable: false),
                    Type = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Address = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    ResponseTime = table.Column<long>(type: "BIGINT", nullable: true),
                    Key = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Models = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Other = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Disable = table.Column<bool>(type: "BIT", nullable: false),
                    Extension = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Quota = table.Column<int>(type: "INT", nullable: false),
                    RemainQuota = table.Column<long>(type: "BIGINT", nullable: false),
                    ControlAutomatically = table.Column<bool>(type: "BIT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    Modifier = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Creator = table.Column<string>(type: "NVARCHAR2(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModelManagers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "CHAR(36)", nullable: false),
                    Model = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Enable = table.Column<bool>(type: "BIT", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Available = table.Column<bool>(type: "BIT", nullable: false),
                    CompletionRate = table.Column<decimal>(type: "DECIMAL(29,4)", nullable: true),
                    PromptRate = table.Column<decimal>(type: "DECIMAL(29,4)", nullable: false),
                    AudioPromptRate = table.Column<decimal>(type: "DECIMAL(29,4)", nullable: true),
                    AudioOutputRate = table.Column<decimal>(type: "DECIMAL(29,4)", nullable: true),
                    QuotaType = table.Column<int>(type: "INT", nullable: false),
                    QuotaMax = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    Tags = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Icon = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    IsVersion2 = table.Column<bool>(type: "BIT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    Modifier = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Creator = table.Column<string>(type: "NVARCHAR2(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelManagers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductPurchaseRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ProductId = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Quantity = table.Column<int>(type: "INT", nullable: false),
                    RemainQuota = table.Column<long>(type: "BIGINT", nullable: false),
                    PurchaseTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Status = table.Column<int>(type: "INT", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    Modifier = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Creator = table.Column<string>(type: "NVARCHAR2(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPurchaseRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Price = table.Column<decimal>(type: "DECIMAL(29,4)", nullable: false),
                    RemainQuota = table.Column<long>(type: "BIGINT", nullable: false),
                    Stock = table.Column<int>(type: "INT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    Modifier = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Creator = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RateLimitModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    WhiteList = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    BlackList = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Enable = table.Column<bool>(type: "BIT", nullable: false),
                    Model = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Strategy = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Limit = table.Column<int>(type: "INT", nullable: false),
                    Value = table.Column<int>(type: "INT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    Modifier = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Creator = table.Column<string>(type: "NVARCHAR2(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RateLimitModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RedeemCodes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Code = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Quota = table.Column<long>(type: "BIGINT", nullable: false),
                    Disabled = table.Column<bool>(type: "BIT", nullable: false),
                    RedeemedTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    RedeemedUserId = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    RedeemedUserName = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    State = table.Column<int>(type: "INT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    Modifier = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Creator = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedeemCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Value = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Private = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Key = table.Column<string>(type: "NVARCHAR2(42)", maxLength: 42, nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    UsedQuota = table.Column<long>(type: "BIGINT", nullable: false),
                    UnlimitedQuota = table.Column<bool>(type: "BIT", nullable: false),
                    RemainQuota = table.Column<long>(type: "BIGINT", nullable: false),
                    AccessedTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    ExpiredTime = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    UnlimitedExpired = table.Column<bool>(type: "BIT", nullable: false),
                    Disabled = table.Column<bool>(type: "BIT", nullable: false),
                    LimitModels = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    WhiteIpList = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    IsDelete = table.Column<bool>(type: "BIT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    Modifier = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Creator = table.Column<string>(type: "NVARCHAR2(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    UserName = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    PasswordHas = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    Avatar = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    Role = table.Column<string>(type: "NVARCHAR2(32767)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "BIT", nullable: false),
                    IsDelete = table.Column<bool>(type: "BIT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    ConsumeToken = table.Column<long>(type: "BIGINT", nullable: false),
                    RequestCount = table.Column<long>(type: "BIGINT", nullable: false),
                    ResidualCredit = table.Column<long>(type: "BIGINT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    Modifier = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Creator = table.Column<string>(type: "NVARCHAR2(32767)", nullable: true)
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
                    { new Guid("04af4942-af97-4c57-bd3d-a3ac7cc37df4"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9933), null, "Text Davinci 003 文本模型", true, "OpenAI", false, "text-davinci-003", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("04f8cb16-5480-4adb-931a-13ed6ac42192"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9908), null, "GPT-4 0613 文本模型", true, "OpenAI", false, "gpt-4-0613", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("0d390490-11cd-4162-b8c7-5dec47cf6424"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9936), null, "Text Embedding Ada 002 嵌入模型", true, "OpenAI", false, "text-embedding-ada-002", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("0fc4eab1-ad4a-49ca-973d-efd92a620a02"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9933), null, "Text Davinci 002 文本模型", true, "OpenAI", false, "text-davinci-002", null, 10m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("10a2657d-ecc5-43a2-a69a-7138952d22ea"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9960), null, "DALL-E 3 图像生成模型", true, "OpenAI", false, "dall-e-3", null, 20m, null, 2, "[\"\\u56FE\\u7247\"]", null },
                    { new Guid("1339ff6c-3eda-40ec-ad7d-6a737e972642"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9906), null, "GPT-4 0125 预览文本模型", true, "OpenAI", false, "gpt-4-0125-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1400564e-ccb1-4144-b3e8-216cd8a7194d"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9955), null, "Claude 3 Sonnet 20240229 文本模型", true, "Claude", false, "claude-3-sonnet-20240229", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1729981d-dfef-4bcd-9fab-1cac974bc9ad"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9930), null, "Text Babbage 001 文本模型", true, "OpenAI", false, "text-babbage-001", null, 0.25m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("186de08c-8072-4df9-8e08-9310aadd12c5"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9922), null, "GPT-4o 文本模型", true, "OpenAI", false, "gpt-4o", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("1d94896d-21aa-4ba2-9b93-eaf5a3060670"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9891), null, "GPT-3.5 Turbo 0125 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0125", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1fd4ceb0-b295-4dcf-bb86-9d7a1c4cf620"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9913), null, "GPT-4 32k 0314 文本模型", true, "OpenAI", false, "gpt-4-32k-0314", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("211be490-817b-42ce-9140-80667cdf846b"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9957), null, "Claude Instant 1.2 文本模型", true, "Claude", false, "claude-instant-1.2", null, 0.4m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("222644ef-484c-41c9-a19f-f852a35c7905"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9914), null, "GPT-4 全部文本模型", true, "OpenAI", false, "gpt-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u8054\\u7F51\"]", null },
                    { new Guid("23cdbcd3-a397-4ad4-a852-487318fa4d6b"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9932), null, "Text Curie 001 文本模型", true, "OpenAI", false, "text-curie-001", null, 1m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("25ccba7f-2a3d-4548-a2aa-f05af313e76b"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9905), null, "GPT-3.5 Turbo Instruct 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-instruct", null, 0.001m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("28881fe9-a9ef-4fee-aed0-5497641fecfb"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9964), null, "GLM 4v 文本模型", true, "ChatGLM", false, "glm-4v", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("2a5a6d4b-6f74-4d45-91f9-216eb3f3ba43"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9941), null, "Whisper 1 语音识别模型", true, "OpenAI", false, "whisper-1", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("34f3e258-0813-46d3-b490-5bdf79458a09"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9926), null, "GPT-4o 2024-05-13 文本模型", true, "OpenAI", false, "gpt-4o-2024-05-13", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("3b7e09ec-c215-40f3-939a-28a72558c74a"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9946), null, "ChatGLM Lite 文本模型", true, "ChatGLM", false, "chatglm_lite", null, 0.1429m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("3fab0578-423d-4135-98bd-089ed55869b7"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9963), null, "GLM 4 文本模型", true, "ChatGLM", false, "glm-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("4134f81f-21f7-4008-a4d1-62efa7f5b159"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9903), null, "GPT-3.5 Turbo 16k 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("434a3d31-430b-412e-8b33-d028a303761b"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9954), null, "Claude 3.5 Sonnet 20240620 文本模型", true, "Claude", false, "claude-3-5-sonnet-20240620", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("4592a4cd-62de-4435-84da-eb509d834f5f"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9916), null, "Gemini 1.5 Pro 文本模型", true, "Google", false, "gemini-1.5-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("461e4e85-f13f-4088-a533-50bbe7f3002a"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9964), null, "GLM 4 全部文本模型", true, "ChatGLM", false, "glm-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("4dab6775-dccd-4593-95ce-95139c30a3ef"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9929), null, "Moonshot v1 8k 文本模型", true, "Moonshot", false, "moonshot-v1-8k", null, 1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("527afa26-69a0-4298-b190-aad70db93eec"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9962), null, "Embedding S1 v1 嵌入模型", true, null, false, "embedding_s1_v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("54c8071c-9c14-4bbb-829d-6c03d08720e7"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9939), null, "TTS 1 1106 语音合成模型", true, "OpenAI", false, "tts-1-1106", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("61bf488f-849b-4da8-9b4e-1f0cf68f40ca"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9918), null, "Gemini Pro 文本模型", true, "Google", false, "gemini-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("68136f5a-48ff-43b4-aa60-d0278bce387b"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9925), null, "GPT-4o Mini 2024-07-18 文本模型", true, "OpenAI", false, "gpt-4o-mini-2024-07-18", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("68464a6f-b753-4176-8b43-635b97cbacf3"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9906), null, "GPT-4 文本模型", true, "OpenAI", false, "gpt-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("68c043be-8d78-4aca-9f86-8fe266a7883f"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9921), null, "GPT-4 Turbo 预览文本模型", true, "OpenAI", false, "gpt-4-turbo-preview", null, 5m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("6a924293-87e8-48d6-a031-73d7e3bcb310"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9944), null, "通用文本模型 v3.5", true, "Spark", false, "generalv3.5", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("6dd73536-2232-40d1-906e-5d4df3c27368"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9914), null, "GPT-4 32k 0613 文本模型", true, "OpenAI", false, "gpt-4-32k-0613", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("6f80a5f4-827d-4dff-953a-d0b8b988a8ac"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9928), null, "Moonshot v1 128k 文本模型", true, "Moonshot", false, "moonshot-v1-128k", null, 5.06m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("70aac551-96a8-4b22-851d-881ec96bec0d"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9942), null, "通用文本模型", true, "Spark", false, "general", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("792d050a-1233-4721-a0c3-2e40da65de3b"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9925), null, "GPT-4o Mini 文本模型", true, "OpenAI", false, "gpt-4o-mini", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("7d51dadd-25be-4f51-8864-558dc012beb4"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9921), null, "GPT-4 视觉预览模型", true, "OpenAI", false, "gpt-4-vision-preview", null, 10m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("7fc5cc13-708e-4b25-a905-2f93c103ef3e"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9946), null, "4.0 超级文本模型", true, "Spark", false, "4.0Ultra", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("82038dd7-b1b8-499c-8a26-6d1bcca36e20"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9935), null, "Text Embedding 3 Small 嵌入模型", true, "OpenAI", false, "text-embedding-3-small", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("830551b9-838d-4ccd-8be9-d4f9e5c2e75a"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9955), null, "Claude 3 Opus 20240229 文本模型", true, "Claude", false, "claude-3-opus-20240229", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("84cafc64-08f4-4537-8c18-8d70587077a2"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9927), null, "GPT-4o 2024-08-06 文本模型", true, "OpenAI", false, "gpt-4o-2024-08-06", null, 1.25m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("90fecc18-96e9-4df6-80d5-a2c7a94c6a5b"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9962), null, "GLM 3 Turbo 文本模型", true, "ChatGLM", false, "glm-3-turbo", null, 0.355m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("9492c009-6684-4d49-90de-92109bd99f63"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9907), null, "GPT-4 0314 文本模型", true, "OpenAI", false, "gpt-4-0314", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("96627c20-05a5-4868-b85f-854399e67bcd"), null, null, true, 2m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9943), null, "通用文本模型 v3", true, "Spark", false, "generalv3", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("98040a2c-7e19-48c7-9682-5afd2195423a"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9940), null, "TTS 1 HD 1106 语音合成模型", true, "OpenAI", false, "tts-1-hd-1106", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("99c46d34-3f24-4cb1-b41f-eec0db24c1eb"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9892), null, "GPT-3.5 Turbo 0301 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0301", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("9b0b4215-8769-463d-8d18-20a4d8781ced"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9893), null, "GPT-3.5 Turbo 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0613", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("9cec3c66-fb48-46ca-a6b0-747d1715a28a"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9942), null, "Hunyuan Lite 文本模型", true, "Hunyuan", false, "hunyuan-lite", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("9ec683b1-6952-44bf-b77d-f8b14f380711"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9919), null, "Gemini Pro 视觉模型", true, "Google", false, "gemini-pro-vision", null, 2m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("9f6e940e-b444-47dc-8118-7934d0db1325"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9950), null, "Claude 2.0 文本模型", true, "Claude", false, "claude-2.0", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a3a763af-6ede-4f8d-bdb7-7efbe1550627"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9904), null, "GPT-3.5 Turbo 16k 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k-0613", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a54cd9eb-84dd-4284-9c81-c6075ce6f7f6"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9884), null, "GPT-3.5 Turbo 文本模型", true, "OpenAI", false, "gpt-3.5-turbo", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a5990f3a-b9e1-48bc-9032-c151efc2af6d"), null, null, true, 3m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9919), null, "Gemini 1.5 Flash 文本模型", true, "Google", false, "gemini-1.5-flash", null, 0.2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a7aef57d-c07b-466e-bc79-83ef58d60fd0"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9935), null, "Text Embedding 3 Large 嵌入模型", true, "OpenAI", false, "text-embedding-3-large", null, 0.13m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a99408fa-c15b-4db2-ad2b-71cc23f744dd"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9953), null, "Claude 3 Haiku 文本模型", true, "Claude", false, "claude-3-haiku", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ab8d0511-d7d3-4fa6-983e-fbee9fd5550e"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9961), null, "Embedding BERT 512 v1 嵌入模型", true, "OpenAI", false, "embedding-bert-512-v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("b199049c-c252-4786-af31-11c24e2434c6"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9948), null, "ChatGLM 标准文本模型", true, "ChatGLM", false, "chatglm_std", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b2c1530d-d09e-4583-997c-d597a101a1d0"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9949), null, "Claude 2 文本模型", true, "Claude", false, "claude-2", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b599b4f2-ab14-49e1-9f3d-7797760e8b28"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9950), null, "Claude 2.1 文本模型", true, "Claude", false, "claude-2.1", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b5b3476b-e9e6-4d4a-9a03-c131adc3a96e"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9939), null, "TTS 1 HD 语音合成模型", true, "OpenAI", false, "tts-1-hd", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("bd225d75-04a0-4e13-bccf-2f75d3804206"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9960), null, "Embedding 2 嵌入模型", true, "OpenAI", false, "embedding-2", null, 0.0355m, "", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("d0e05674-c8ec-4610-b749-7a368968f48c"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9934), null, "Text Davinci Edit 001 文本模型", true, "OpenAI", false, "text-davinci-edit-001", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("d31ddabb-7c1a-48fc-8892-bf0ef8b9c65e"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9928), null, "Moonshot v1 32k 文本模型", true, "Moonshot", false, "moonshot-v1-32k", null, 2m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("d6d0b96f-f8fc-41a6-9c44-872809e19c4d"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9920), null, "GPT-4 Turbo 2024-04-09 文本模型", true, "OpenAI", false, "gpt-4-turbo-2024-04-09", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("da5d3fdb-fdae-4344-be73-09a85f7cf686"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9915), null, "GPT-4 Turbo 文本模型", true, "OpenAI", false, "gpt-4-turbo", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("dade9a30-f7b6-4f5a-b49b-fe1db14d73bd"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9911), null, "GPT-4 1106 预览文本模型", true, "OpenAI", false, "gpt-4-1106-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("e19e608b-2b79-4808-b70f-e82daa2fde33"), null, null, true, 4m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9923), null, "ChatGPT 4o 最新文本模型", true, "OpenAI", false, "chatgpt-4o-latest", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("e528e73c-56b6-4837-9423-c3e3dbc72c85"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9947), null, "ChatGLM Pro 文本模型", true, "ChatGLM", false, "chatglm_pro", null, 0.7143m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("e65c11cc-3f85-4d18-b40a-ff677529b4c8"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9948), null, "ChatGLM Turbo 文本模型", true, "ChatGLM", false, "chatglm_turbo", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("e7bec2e7-ff7e-474e-89b3-87218f4156a6"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9903), null, "GPT-3.5 Turbo 1106 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-1106", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("e9a098fe-f478-4273-b892-88aaebc31f4a"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9912), null, "GPT-4 32k 文本模型", true, "OpenAI", false, "gpt-4-32k", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ec75c70b-0eda-4984-8cd3-3574d96146af"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9911), null, "GPT-4 1106 视觉预览模型", true, "OpenAI", false, "gpt-4-1106-vision-preview", null, 10m, "128K", 1, "[\"\\u89C6\\u89C9\",\"\\u6587\\u672C\"]", null },
                    { new Guid("f0069473-2a60-4ff9-8bf5-c270c7ca13d6"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9937), null, "TTS 1 语音合成模型", true, "OpenAI", false, "tts-1", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("f3074b13-1767-4500-9a5d-82a92d4e9490"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9957), null, "DALL-E 2 图像生成模型", true, "OpenAI", false, "dall-e-2", null, 8m, null, 2, "[\"\\u56FE\\u7247\"]", null },
                    { new Guid("f57939f6-b2b9-473b-a408-06ab05413d2b"), null, null, true, null, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9956), null, "Claude Instant 1 文本模型", true, "Claude", false, "claude-instant-1", null, 0.815m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("f5d0d654-0c44-47ad-b190-6779e61346fd"), null, null, true, 5m, new DateTime(2024, 11, 4, 1, 55, 54, 597, DateTimeKind.Local).AddTicks(9953), null, "Claude 3 Haiku 20240307 文本模型", true, "Claude", false, "claude-3-haiku-20240307", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null }
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
                values: new object[] { "2d42debc-c343-49e7-bca7-8a4a5fbdabcd", null, new DateTime(2024, 11, 4, 1, 55, 54, 595, DateTimeKind.Local).AddTicks(4615), "75208f65-a6e0-491d-91b4-a9ecc180a36f", null, false, null, false, "sk-ELFtn4XLhtFY99QPwLhMnOOLCEUfvAkFXPTjbe", "[]", null, "默认Token", 0L, true, true, null, 0L, "[]" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "75208f65-a6e0-491d-91b4-a9ecc180a36f", null, 0L, new DateTime(2024, 11, 4, 1, 55, 54, 595, DateTimeKind.Local).AddTicks(4171), null, null, "239573049@qq.com", false, false, null, "d8c46cab2a38f1656625e33498eca25e", "ee679168408e49eb9ed1259db5a1a422", 0L, 1000000000L, "admin", null, "admin" });

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
