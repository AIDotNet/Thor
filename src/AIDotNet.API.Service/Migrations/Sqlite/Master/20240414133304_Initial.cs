using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AIDotNet.API.Service.Migrations.Master
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
                name: "RedeemCodes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                table: "Settings",
                columns: new[] { "Key", "Description", "Private", "Value" },
                values: new object[,]
                {
                    { "Setting:GeneralSetting:AutoDisableChannel", "自动禁用异常渠道", true, "false" },
                    { "Setting:GeneralSetting:ChatLink", "对话链接", false, "" },
                    { "Setting:GeneralSetting:CheckInterval", "检测间隔 (分钟)", true, "60" },
                    { "Setting:GeneralSetting:EnableAutoCheckChannel", "启用自动检测渠道策略", true, "false" },
                    { "Setting:GeneralSetting:EnableClearLog", "启用定时清理日志", true, "true" },
                    { "Setting:GeneralSetting:IntervalDays", "间隔天数", true, "90" },
                    { "Setting:GeneralSetting:InviteQuota", "邀请奖励额度", true, "100000" },
                    { "Setting:GeneralSetting:ModelCompletionRate", "模型倍率Completion", true, "{}" },
                    {
                        "Setting:GeneralSetting:ModelPromptRate", "模型倍率Prompt", true,
                        "{ \"360GPT_S2_V9\": 0.8572, \"BLOOMZ-7B\": 0.284, \"Baichuan2-53B\": 1.42, \"Baichuan2-Turbo\": 0.568, \"Baichuan2-Turbo-192k\": 1.136, \"ChatPro\": 7.1, \"ChatStd\": 0.71, \"ERNIE-3.5-4K-0205\": 0.852, \"ERNIE-3.5-8K\": 0.852, \"ERNIE-3.5-8K-0205\": 1.704, \"ERNIE-3.5-8K-1222\": 0.852, \"ERNIE-4.0-8K\": 8.52, \"ERNIE-Bot\": 0.8572, \"ERNIE-Bot-4\": 8.572, \"ERNIE-Bot-8K\": 1.704, \"ERNIE-Bot-8k\": 1.704, \"ERNIE-Bot-turbo\": 0.5715, \"ERNIE-Lite-8K-0308\": 0.213, \"ERNIE-Lite-8K-0922\": 0.568, \"ERNIE-Speed-128K\": 0.284, \"ERNIE-Speed-8K\": 0.284, \"ERNIE-Tiny-8K\": 0.071, \"Embedding-V1\": 0.1429, \"PaLM-2\": 1, \"SparkDesk\": 1.2858, \"SparkDesk-v1.1\": 1.2858, \"SparkDesk-v2.1\": 1.2858, \"SparkDesk-v3.1\": 1.2858, \"SparkDesk-v3.5\": 1.2858, \"abab5.5-chat\": 1.065, \"abab5.5s-chat\": 0.355, \"abab6-chat\": 7.1, \"ada\": 10, \"ali-stable-diffusion-v1.5\": 8, \"ali-stable-diffusion-xl\": 8, \"babbage\": 10, \"babbage-002\": 0.2, \"bge-large-8k\": 0.142, \"bge-large-en\": 0.142, \"bge-large-zh\": 0.142, \"chatglm_lite\": 0.1429, \"chatglm_pro\": 0.7143, \"chatglm_std\": 0.3572, \"chatglm_turbo\": 0.3572, \"claude-2\": 5.51, \"claude-2.0\": 5.51, \"claude-2.1\": 5.51, \"claude-3-haiku-20240307\": 0.125, \"claude-3-opus-20240229\": 15, \"claude-3-sonnet-20240229\": 5, \"claude-instant-1\": 0.815, \"claude-instant-1.2\": 0.4, \"code-davinci-edit-001\": 10, \"cogview-3\": 17.75, \"curie\": 10, \"dall-e-2\": 8, \"dall-e-3\": 20, \"davinci\": 10, \"davinci-002\": 1, \"embedding-2\": 0.0355, \"embedding-bert-512-v1\": 0.0715, \"embedding_s1_v1\": 0.0715, \"gemini-1.0-pro-001\": 1, \"gemini-1.0-pro-vision-001\": 1, \"gemini-1.5-pro\": 1, \"gemini-pro\": 1, \"gemini-pro-vision\": 1, \"gemma-7b-it\": 0.05, \"glm-3-turbo\": 0.355, \"glm-4\": 7.1, \"glm-4v\": 7.1, \"gpt-3.5-turbo\": 0.25, \"gpt-3.5-turbo-0125\": 0.25, \"gpt-3.5-turbo-0301\": 0.75, \"gpt-3.5-turbo-0613\": 0.75, \"gpt-3.5-turbo-1106\": 0.75, \"gpt-3.5-turbo-16k\": 1.5, \"gpt-3.5-turbo-16k-0613\": 1.5, \"gpt-3.5-turbo-instruct\": 0.75, \"gpt-4\": 15, \"gpt-4-0125-preview\": 5, \"gpt-4-0314\": 15, \"gpt-4-0613\": 15, \"gpt-4-1106-preview\": 5, \"gpt-4-turbo-2024-04-09\": 5, \"gpt-4-32k\": 30, \"gpt-4-32k-0314\": 30, \"gpt-4-32k-0613\": 30, \"gpt-4-all\": 15, \"gpt-4-gizmo-*\": 15, \"gpt-4-turbo-preview\": 5, \"gpt-4-vision-preview\": 5, \"hunyuan\": 7.143, \"llama2-70b-4096\": 0.35, \"llama2-7b-2048\": 0.05, \"mistral-embed\": 0.05, \"mistral-large-latest\": 4, \"mistral-medium-latest\": 1.35, \"mistral-small-latest\": 1, \"mixtral-8x7b-32768\": 0.135, \"moonshot-v1-128k\": 4.26, \"moonshot-v1-32k\": 1.704, \"moonshot-v1-8k\": 0.852, \"open-mistral-7b\": 0.125, \"open-mixtral-8x7b\": 0.35, \"qwen-max\": 1.4286, \"qwen-max-longcontext\": 1.4286, \"qwen-plus\": 1.4286, \"qwen-turbo\": 0.5715, \"search-serper\": 0.00001, \"semantic_similarity_s1_v1\": 0.0715, \"step-1-200k\": 10.65, \"step-1-32k\": 1.704, \"step-1v-32k\": 1.704, \"tao-8k\": 0.142, \"text-ada-001\": 0.2, \"text-babbage-001\": 0.25, \"text-curie-001\": 1, \"text-davinci-002\": 10, \"text-davinci-003\": 10, \"text-davinci-edit-001\": 10, \"text-embedding-3-large\": 0.065, \"text-embedding-3-small\": 0.5, \"text-embedding-ada-002\": 0.1, \"text-embedding-v1\": 0.05, \"text-moderation-latest\": 0.1, \"text-moderation-stable\": 0.1, \"text-search-ada-doc-001\": 10, \"tts-1\": 7.5, \"tts-1-1106\": 7.5, \"tts-1-hd\": 15, \"tts-1-hd-1106\": 15, \"wanx-v1\": 8, \"whisper-1\": 15, \"yi-34b-chat-0205\": 0.1775, \"yi-34b-chat-200k\": 0.852, \"yi-vl-plus\": 0.426 }"
                    },
                    { "Setting:GeneralSetting:NewUserQuota", "新用户初始额度", true, "100000" },
                    { "Setting:GeneralSetting:RechargeAddress", "充值地址", false, "" },
                    { "Setting:GeneralSetting:RequestQuota", "请求预扣额度", true, "2000" },
                    {
                        "Setting:OtherSetting:IndexContent", "首页内容", false,
                        "AI DotNet API 提供更强的兼容，将更多的AI平台接入到AI DotNet API中，让AI集成更加简单。"
                    },
                    { "Setting:OtherSetting:WebLogo", "网站Logo地址", false, "/logo.png" },
                    { "Setting:OtherSetting:WebTitle", "网站标题", false, "AIDtoNet API" },
                    { "Setting:SystemSetting:EnableGithubLogin", "允许Github登录", false, "true" },
                    { "Setting:SystemSetting:EnableRegister", "启用账号注册", false, "true" },
                    { "Setting:SystemSetting:GithubClientId", "Github Client Id", false, "" },
                    { "Setting:SystemSetting:GithubClientSecret", "Github Client Secret", true, "" },
                    { "Setting:SystemSetting:ServerAddress", "服务器地址", false, "" }
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
                    "4cde8057-c272-4e2a-91b7-878032e4a2d0", null, 0L, DateTime.Now, null, null, "239573049@qq.com",
                    false, false, null, "b73f077db2980fe2765fc3b136babedb", "e9a07d959d174ee6953b7777833dd23b", 0L,
                    10000000L, "admin", null, "admin"
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
                name: "IX_RedeemCodes_Code",
                table: "RedeemCodes",
                column: "Code");

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