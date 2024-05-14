using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIDotNet.API.Service.Migrations.Master
{
    /// <inheritdoc />
    public partial class AddSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "72e8703c-6dea-466c-82aa-7e0b85d04618");

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:GeneralSetting:ModelPromptRate",
                column: "Value",
                value: "{ \"360GPT_S2_V9\": 0.8572, \"BLOOMZ-7B\": 0.284, \"Baichuan2-53B\": 1.42, \"Baichuan2-Turbo\": 0.568, \"Baichuan2-Turbo-192k\": 1.136, \"ChatPro\": 7.1, \"ChatStd\": 0.71, \"ERNIE-3.5-4K-0205\": 0.852, \"ERNIE-3.5-8K\": 0.852, \"ERNIE-3.5-8K-0205\": 1.704, \"ERNIE-3.5-8K-1222\": 0.852, \"ERNIE-4.0-8K\": 8.52, \"ERNIE-Bot\": 0.8572, \"ERNIE-Bot-4\": 8.572, \"ERNIE-Bot-8K\": 1.704, \"ERNIE-Bot-8k\": 1.704, \"ERNIE-Bot-turbo\": 0.5715, \"ERNIE-Lite-8K-0308\": 0.213, \"ERNIE-Lite-8K-0922\": 0.568, \"ERNIE-Speed-128K\": 0.284, \"ERNIE-Speed-8K\": 0.284, \"ERNIE-Tiny-8K\": 0.071, \"Embedding-V1\": 0.1429, \"PaLM-2\": 1, \"SparkDesk\": 1.2858, \"SparkDesk-v1.1\": 1.2858, \"SparkDesk-v2.1\": 1.2858, \"SparkDesk-v3.1\": 1.2858, \"SparkDesk-v3.5\": 1.2858, \"abab5.5-chat\": 1.065, \"abab5.5s-chat\": 0.355, \"abab6-chat\": 7.1, \"ada\": 10, \"ali-stable-diffusion-v1.5\": 8, \"ali-stable-diffusion-xl\": 8, \"babbage\": 10, \"babbage-002\": 0.2, \"bge-large-8k\": 0.142, \"bge-large-en\": 0.142, \"bge-large-zh\": 0.142, \"chatglm_lite\": 0.1429, \"chatglm_pro\": 0.7143, \"chatglm_std\": 0.3572, \"chatglm_turbo\": 0.3572, \"claude-2\": 5.51, \"claude-2.0\": 5.51, \"claude-2.1\": 5.51, \"claude-3-haiku-20240307\": 0.125, \"claude-3-opus-20240229\": 15, \"claude-3-sonnet-20240229\": 5, \"claude-instant-1\": 0.815, \"claude-instant-1.2\": 0.4, \"code-davinci-edit-001\": 10, \"cogview-3\": 17.75, \"curie\": 10, \"dall-e-2\": 8, \"dall-e-3\": 20, \"davinci\": 10, \"davinci-002\": 1, \"embedding-2\": 0.0355, \"embedding-bert-512-v1\": 0.0715, \"embedding_s1_v1\": 0.0715, \"gemini-1.0-pro-001\": 1, \"gemini-1.0-pro-vision-001\": 1, \"gemini-1.5-pro\": 1, \"gemini-pro\": 1, \"gemini-pro-vision\": 1, \"gemma-7b-it\": 0.05, \"glm-3-turbo\": 0.355, \"glm-4\": 7.1, \"glm-4v\": 7.1, \"gpt-3.5-turbo\": 0.25, \"gpt-3.5-turbo-0125\": 0.25, \"gpt-3.5-turbo-0301\": 0.75, \"gpt-3.5-turbo-0613\": 0.75, \"gpt-3.5-turbo-1106\": 0.75, \"gpt-3.5-turbo-16k\": 1.5, \"gpt-3.5-turbo-16k-0613\": 1.5, \"gpt-3.5-turbo-instruct\": 0.75, \"gpt-4\": 15, \"gpt-4-0125-preview\": 5, \"gpt-4-0314\": 15, \"gpt-4-0613\": 15, \"gpt-4-1106-preview\": 5, \"gpt-4-turbo-2024-04-09\": 5, \"gpt-4-32k\": 30, \"gpt-4-32k-0314\": 30, \"gpt-4-32k-0613\": 30, \"gpt-4-all\": 15, \"gpt-4-gizmo-*\": 15, \"gpt-4-turbo-preview\": 5, \"gpt-4-vision-preview\": 5, \"hunyuan\": 7.143, \"llama2-70b-4096\": 0.35, \"llama2-7b-2048\": 0.05, \"mistral-embed\": 0.05, \"mistral-large-latest\": 4, \"mistral-medium-latest\": 1.35, \"mistral-small-latest\": 1, \"mixtral-8x7b-32768\": 0.135, \"moonshot-v1-128k\": 4.26, \"moonshot-v1-32k\": 1.704, \"moonshot-v1-8k\": 0.852, \"open-mistral-7b\": 0.125, \"open-mixtral-8x7b\": 0.35, \"qwen-max\": 1.4286, \"qwen-max-longcontext\": 1.4286, \"qwen-plus\": 1.4286, \"qwen-turbo\": 0.5715, \"search-serper\": 0.00001, \"semantic_similarity_s1_v1\": 0.0715, \"step-1-200k\": 10.65, \"step-1-32k\": 1.704, \"step-1v-32k\": 1.704, \"tao-8k\": 0.142, \"text-ada-001\": 0.2, \"text-babbage-001\": 0.25, \"text-curie-001\": 1, \"text-davinci-002\": 10, \"text-davinci-003\": 10, \"text-davinci-edit-001\": 10, \"text-embedding-3-large\": 0.065, \"text-embedding-3-small\": 0.5, \"text-embedding-ada-002\": 0.1, \"text-embedding-v1\": 0.05, \"text-moderation-latest\": 0.1, \"text-moderation-stable\": 0.1, \"text-search-ada-doc-001\": 10, \"tts-1\": 7.5, \"tts-1-1106\": 7.5, \"tts-1-hd\": 15, \"tts-1-hd-1106\": 15, \"wanx-v1\": 8, \"whisper-1\": 15, \"yi-34b-chat-0205\": 0.1775, \"yi-34b-chat-200k\": 0.852, \"yi-vl-plus\": 0.426, \"llama3:8b\": 1, \"llama3:70b\": 1 }");

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Key", "Description", "Private", "Value" },
                values: new object[] { "Setting:GeneralSetting:VidolLink", "Vidol 链接", false, "" });

            migrationBuilder.UpdateData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: 9999L,
                columns: new[] { "Creator", "Key" },
                values: new object[] { "82ea4ca6-3bc8-4881-b64e-d33fab18aa39", "sk-f4I2CkR0xGVXW6tpNB7timK7BwV8bbpm1KN8jk" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "82ea4ca6-3bc8-4881-b64e-d33fab18aa39", null, 0L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "239573049@qq.com", false, false, null, "645203af27210c85c0ba0133f54e931b", "87874ef6916842a6988ddcf45a702550", 0L, 10000000L, "admin", null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:GeneralSetting:VidolLink");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "82ea4ca6-3bc8-4881-b64e-d33fab18aa39");

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:GeneralSetting:ModelPromptRate",
                column: "Value",
                value: "{ \"360GPT_S2_V9\": 0.8572, \"BLOOMZ-7B\": 0.284, \"Baichuan2-53B\": 1.42, \"Baichuan2-Turbo\": 0.568, \"Baichuan2-Turbo-192k\": 1.136, \"ChatPro\": 7.1, \"ChatStd\": 0.71, \"ERNIE-3.5-4K-0205\": 0.852, \"ERNIE-3.5-8K\": 0.852, \"ERNIE-3.5-8K-0205\": 1.704, \"ERNIE-3.5-8K-1222\": 0.852, \"ERNIE-4.0-8K\": 8.52, \"ERNIE-Bot\": 0.8572, \"ERNIE-Bot-4\": 8.572, \"ERNIE-Bot-8K\": 1.704, \"ERNIE-Bot-8k\": 1.704, \"ERNIE-Bot-turbo\": 0.5715, \"ERNIE-Lite-8K-0308\": 0.213, \"ERNIE-Lite-8K-0922\": 0.568, \"ERNIE-Speed-128K\": 0.284, \"ERNIE-Speed-8K\": 0.284, \"ERNIE-Tiny-8K\": 0.071, \"Embedding-V1\": 0.1429, \"PaLM-2\": 1, \"SparkDesk\": 1.2858, \"SparkDesk-v1.1\": 1.2858, \"SparkDesk-v2.1\": 1.2858, \"SparkDesk-v3.1\": 1.2858, \"SparkDesk-v3.5\": 1.2858, \"abab5.5-chat\": 1.065, \"abab5.5s-chat\": 0.355, \"abab6-chat\": 7.1, \"ada\": 10, \"ali-stable-diffusion-v1.5\": 8, \"ali-stable-diffusion-xl\": 8, \"babbage\": 10, \"babbage-002\": 0.2, \"bge-large-8k\": 0.142, \"bge-large-en\": 0.142, \"bge-large-zh\": 0.142, \"chatglm_lite\": 0.1429, \"chatglm_pro\": 0.7143, \"chatglm_std\": 0.3572, \"chatglm_turbo\": 0.3572, \"claude-2\": 5.51, \"claude-2.0\": 5.51, \"claude-2.1\": 5.51, \"claude-3-haiku-20240307\": 0.125, \"claude-3-opus-20240229\": 15, \"claude-3-sonnet-20240229\": 5, \"claude-instant-1\": 0.815, \"claude-instant-1.2\": 0.4, \"code-davinci-edit-001\": 10, \"cogview-3\": 17.75, \"curie\": 10, \"dall-e-2\": 8, \"dall-e-3\": 20, \"davinci\": 10, \"davinci-002\": 1, \"embedding-2\": 0.0355, \"embedding-bert-512-v1\": 0.0715, \"embedding_s1_v1\": 0.0715, \"gemini-1.0-pro-001\": 1, \"gemini-1.0-pro-vision-001\": 1, \"gemini-1.5-pro\": 1, \"gemini-pro\": 1, \"gemini-pro-vision\": 1, \"gemma-7b-it\": 0.05, \"glm-3-turbo\": 0.355, \"glm-4\": 7.1, \"glm-4v\": 7.1, \"gpt-3.5-turbo\": 0.25, \"gpt-3.5-turbo-0125\": 0.25, \"gpt-3.5-turbo-0301\": 0.75, \"gpt-3.5-turbo-0613\": 0.75, \"gpt-3.5-turbo-1106\": 0.75, \"gpt-3.5-turbo-16k\": 1.5, \"gpt-3.5-turbo-16k-0613\": 1.5, \"gpt-3.5-turbo-instruct\": 0.75, \"gpt-4\": 15, \"gpt-4-0125-preview\": 5, \"gpt-4-0314\": 15, \"gpt-4-0613\": 15, \"gpt-4-1106-preview\": 5, \"gpt-4-turbo-2024-04-09\": 5, \"gpt-4-32k\": 30, \"gpt-4-32k-0314\": 30, \"gpt-4-32k-0613\": 30, \"gpt-4-all\": 15, \"gpt-4-gizmo-*\": 15, \"gpt-4-turbo-preview\": 5, \"gpt-4-vision-preview\": 5, \"hunyuan\": 7.143, \"llama2-70b-4096\": 0.35, \"llama2-7b-2048\": 0.05, \"mistral-embed\": 0.05, \"mistral-large-latest\": 4, \"mistral-medium-latest\": 1.35, \"mistral-small-latest\": 1, \"mixtral-8x7b-32768\": 0.135, \"moonshot-v1-128k\": 4.26, \"moonshot-v1-32k\": 1.704, \"moonshot-v1-8k\": 0.852, \"open-mistral-7b\": 0.125, \"open-mixtral-8x7b\": 0.35, \"qwen-max\": 1.4286, \"qwen-max-longcontext\": 1.4286, \"qwen-plus\": 1.4286, \"qwen-turbo\": 0.5715, \"search-serper\": 0.00001, \"semantic_similarity_s1_v1\": 0.0715, \"step-1-200k\": 10.65, \"step-1-32k\": 1.704, \"step-1v-32k\": 1.704, \"tao-8k\": 0.142, \"text-ada-001\": 0.2, \"text-babbage-001\": 0.25, \"text-curie-001\": 1, \"text-davinci-002\": 10, \"text-davinci-003\": 10, \"text-davinci-edit-001\": 10, \"text-embedding-3-large\": 0.065, \"text-embedding-3-small\": 0.5, \"text-embedding-ada-002\": 0.1, \"text-embedding-v1\": 0.05, \"text-moderation-latest\": 0.1, \"text-moderation-stable\": 0.1, \"text-search-ada-doc-001\": 10, \"tts-1\": 7.5, \"tts-1-1106\": 7.5, \"tts-1-hd\": 15, \"tts-1-hd-1106\": 15, \"wanx-v1\": 8, \"whisper-1\": 15, \"yi-34b-chat-0205\": 0.1775, \"yi-34b-chat-200k\": 0.852, \"yi-vl-plus\": 0.426 }");

            migrationBuilder.UpdateData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: 9999L,
                columns: new[] { "Creator", "Key" },
                values: new object[] { "72e8703c-6dea-466c-82aa-7e0b85d04618", "sk-gkT0IZkGclTdAqZVZuOhgCEtCjapsQl37UiS3z" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "72e8703c-6dea-466c-82aa-7e0b85d04618", null, 0L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "239573049@qq.com", false, false, null, "654b09708742ad11ad1eee4e43fd84f1", "9d80d1d43cf14a17bcf052a2da79e5cf", 0L, 10000000L, "admin", null, "admin" });
        }
    }
}
