using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Thor.Provider.MySql.Thor
{
    /// <inheritdoc />
    public partial class AddAnnouncements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("045b9916-95ad-486c-abdd-b6448e6c4e26"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("05cdfdf0-7ca2-4dfb-9daa-8d5e172490ef"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("06b49e8b-41f7-4b25-99c8-db728506c8e7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0e17eb12-00e5-43cf-b30f-0a306fb282ac"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("10105fcb-2563-4dfe-a12d-85b787aa6e68"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("11e679c2-4d46-4a0e-8355-2c906f3b7940"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("11fa4873-818d-4bde-9149-f92ef02a625d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("14fd03c1-8219-4300-b8d3-ea43b145a2fe"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("178b0d59-1b17-4f56-a0e2-da0920f7ad36"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("17c1f7bd-8736-4dcd-95ac-34fe32cbd298"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("199a4a54-78e9-4a9a-9745-6d70214a34f2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1c1fe844-2d2d-4e59-93e7-df87134b4ba7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1c38ac7a-74b0-4911-ade0-cd1d4f439407"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1e91aa6b-56f4-49e3-a5e7-71cf6d76fc5f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("23304565-a367-4ed6-8625-6af23e9fddd5"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2460ceac-14e6-4599-9145-9bd4b6409844"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("24644101-63d0-4e29-ab3d-af3eb03b964b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("27a23e1d-6c8d-4ab0-8754-f48adc66dd4f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("28e6cc8b-0bd5-440d-ab7f-5cbefe8a8481"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("38f7483d-30b6-42b9-bb31-37a6ebb9bbf6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("39d1439b-7baa-4cd2-98c8-2ec6928951a2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("420d2949-738b-4bba-8b79-20ff6d74ec4e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("423abcff-04d2-4e43-a027-0b03387cfaf3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("43914fc3-eb3e-415a-952b-249db3c7e8d3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("47f4e75e-85b5-44ce-9798-4fe3d9e885f3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("506f7d2a-0b56-44bb-bf6f-5214375d65c4"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("520b605e-38f7-4503-9ff7-de579ebcffdb"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("555d37bf-84b6-4b3f-8b53-1114caf8158c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5628e544-317c-4424-89ae-e3f63bb2370b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("58fdcf2e-407a-4075-a5c4-512a265fa60c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5d8acea9-b9b7-464c-95e9-f222ff65b656"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("605b055f-eaa8-4549-a221-aa98f4873e4c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("63548300-1928-4571-a62d-d24360b5d3d8"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6398bfac-a889-4b8f-8ea6-0f7cc5015d2a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("643075d6-c723-4989-b15d-74d650313682"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6a9fb149-96e4-4c45-9abc-30a53f7df89a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6d24edbf-d1e6-492a-a585-ae761625da7f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("71ffd7b0-f284-4836-8ce9-330ae5c9d387"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("774536a1-5e6d-434b-b878-c07b8bc8ef99"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("777bc64b-4f62-4780-9250-88f5c0561d45"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("780ab4fd-6bd0-4f0c-9e64-4d09933766c9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("79d48680-1f23-4d0e-b39d-1878783fb7f9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7b36dd72-0439-4467-84e5-4d8a84a2729b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7d24e581-1175-493c-a2a3-b56f73177c1a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7ef50946-e8c6-481c-bb27-2fc7187a3c46"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("857da218-fade-4bb7-961d-688091bb7879"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("89dcbccd-bd9e-48df-9176-495051dbb622"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8aa29990-5dc9-45f4-b7e3-de9ad8812e84"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8b9fe7d2-52ac-44a1-a2b6-abec89daa2d0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9c1aad0d-0a54-48f7-9151-56f2f1e0adf4"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a3c5780b-d71f-4a57-8783-af4b2cdb673c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a5bc978e-8e9e-4ad8-bb9c-40d3ba569956"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b1401c04-c5e7-4423-9435-3d0f8f6537de"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b33b4c70-9efe-4fb7-a311-ffcb6f439a13"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b6f6db90-b908-418d-aaee-f36f38335c27"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b7b1335c-33f0-4c4b-af11-bd17c0938735"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b90da15c-e614-494f-bed2-0851808c6702"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("bcbdc906-9748-4e94-8abc-d0c92f1c2860"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c01358a7-cb73-49b4-a1a1-1847a1991262"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("caaf7531-7afc-4653-ad04-248c94a4d8a2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ccc96dfa-3b4a-4fbe-8d81-aba8dc7daef8"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d2509342-3877-4ca6-b7fe-894217b54791"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d41324de-ed36-49fe-bcf6-874263c32dc5"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d4ec56d0-f7d7-4840-b97b-553bf7e9c6a7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d8694f43-4568-4f15-be1e-753bc66b8fea"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e0f43ebd-84b6-4212-8bfa-eb2d72da95e6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e45c52f2-cd86-4577-b114-cf277ba159e2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e513abed-4088-4c1c-a086-865cfa62cd1c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e625ec04-5661-4be6-8c89-613b8730da82"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e8810623-b845-4abf-8595-9ea14144df80"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f236f5d9-0149-4ada-b952-aa535a9b7e43"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f5da1519-2684-4d7e-be38-9f9e916fcbe5"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f71ef040-036f-41cb-92ec-5c33e5e0b326"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f7dd376d-4f16-4855-ba7b-c93c069e1f09"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fb58d77a-dc58-440b-befa-a6bb26d95c86"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fce93319-c34a-4693-90da-0e24ab703513"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fedb5ce5-acca-416f-a908-9e25971d3b8d"));

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Pinned = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ExpireTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Modifier = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Creator = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "ModelManagers",
                columns: new[] { "Id", "AudioCacheRate", "AudioOutputRate", "AudioPromptRate", "Available", "CacheHitRate", "CacheRate", "CompletionRate", "CreatedAt", "Creator", "Description", "Enable", "Extension", "Icon", "IsVersion2", "Model", "Modifier", "PromptRate", "QuotaMax", "QuotaType", "Tags", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("013398c8-c960-4402-9e4b-5640e49751a3"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9304), null, "GPT-4 视觉预览模型", true, "{}", "OpenAI", false, "gpt-4-vision-preview", null, 10m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("01d840e0-86e6-4486-9bdc-d0bfcc7a2d86"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9324), null, "Text Embedding Ada 002 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-ada-002", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("0291b1e7-05ee-40e5-aaca-424c2cb7e952"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9353), null, "GLM 4 全部文本模型", true, "{}", "ChatGLM", false, "glm-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("03e9906d-0673-4f8e-97fb-d917ab5504ef"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9325), null, "TTS 1 语音合成模型", true, "{}", "OpenAI", false, "tts-1", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("0b81daa5-8bd2-4ec9-8faa-3ab3b5308331"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9296), null, "GPT-4 Turbo 2024-04-09 文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo-2024-04-09", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("0ba6e600-5a6c-4e9b-96d5-71e38a488c26"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9308), null, "GPT-4o Mini 文本模型", true, "{}", "OpenAI", false, "gpt-4o-mini", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("0c342a97-5bdf-4658-b842-320ca0dcafcc"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9348), null, "Embedding 2 嵌入模型", true, "{}", "OpenAI", false, "embedding-2", null, 0.0355m, "", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("14fccf76-77ef-4c50-bab7-6beed0e801b6"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9278), null, "GPT-4 0314 文本模型", true, "{}", "OpenAI", false, "gpt-4-0314", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("1a0f4722-5c35-4ca4-bdb3-609bf51db1f8"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9326), null, "TTS 1 HD 语音合成模型", true, "{}", "OpenAI", false, "tts-1-hd", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("1fa08607-a2f3-4b45-9cd2-640c268fed83"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9279), null, "GPT-4 0613 文本模型", true, "{}", "OpenAI", false, "gpt-4-0613", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("21cb733d-3ae0-472c-b4b3-d46041a6e42c"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9335), null, "ChatGLM Pro 文本模型", true, "{}", "ChatGLM", false, "chatglm_pro", null, 0.7143m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("21fc3bb1-9818-462a-9212-cb5ac24676f1"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9331), null, "通用文本模型", true, "{}", "Spark", false, "general", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("23883819-cf8e-4196-816c-10a5dbf577ce"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9269), null, "GPT-3.5 Turbo 0613 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0613", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("24610688-1852-4e33-9ed1-f35f2abe6f02"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9354), null, "GLM 4v 文本模型", true, "{}", "ChatGLM", false, "glm-4v", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("24feda79-966c-41b0-a14f-91c4243f5586"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9342), null, "Claude 3 Sonnet 20240229 文本模型", true, "{}", "Claude", false, "claude-3-sonnet-20240229", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("26ccb343-a569-42d8-9913-dfe01aec892e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9287), null, "GPT-4 32k 0613 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k-0613", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("278c2eca-747d-4e31-8bae-b0cbf2ddc3ea"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9283), null, "GPT-4 32k 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("2efea6ca-e647-444f-ac82-b244daacc8fa"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9339), null, "Claude 2.1 文本模型", true, "{}", "Claude", false, "claude-2.1", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("33c4797e-8ade-49a9-8485-279fcb117de0"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9315), null, "Moonshot v1 128k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-128k", null, 5.06m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("36802ce4-1bd0-4a26-9519-8092e754ecaf"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9316), null, "Moonshot v1 32k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-32k", null, 2m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("3701c784-01ae-4371-b48e-6b7d027f6190"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9325), null, "TTS 1 1106 语音合成模型", true, "{}", "OpenAI", false, "tts-1-1106", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("474f228c-40f4-4ba3-b5e0-fd42fa261778"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9068), null, "GPT-3.5 Turbo 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("4d3834bb-3e15-4d2c-bbb0-a754e8314e86"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9306), null, "ChatGPT 4o 最新文本模型", true, "{}", "OpenAI", false, "chatgpt-4o-latest", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("5a76a8af-fe28-4d09-87f6-21f22334ce2e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9282), null, "GPT-4 1106 视觉预览模型", true, "{}", "OpenAI", false, "gpt-4-1106-vision-preview", null, 10m, "128K", 1, "[\"\\u89C6\\u89C9\",\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("5a8ce9ef-5001-4ddc-8872-d2ff7450b5e2"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9318), null, "Text Curie 001 文本模型", true, "{}", "OpenAI", false, "text-curie-001", null, 1m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("5c1eb6b6-6827-4c3e-adf3-3f82b86393a4"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9319), null, "Text Davinci 002 文本模型", true, "{}", "OpenAI", false, "text-davinci-002", null, 10m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("5fd530ac-6704-4526-a8dc-83ce571656b0"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9347), null, "DALL-E 3 图像生成模型", true, "{}", "OpenAI", false, "dall-e-3", null, 20000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("638c887b-1ebb-4d19-90f6-71c384815b7a"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9332), null, "通用文本模型 v3", true, "{}", "Spark", false, "generalv3", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("640cf7ef-8163-4d79-a4a2-e0ee28baef60"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9345), null, "Claude Instant 1.2 文本模型", true, "{}", "Claude", false, "claude-instant-1.2", null, 0.4m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("6a3e1334-2f07-4009-8677-9fd5af06adc9"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9268), null, "GPT-3.5 Turbo 0301 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0301", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("6ae8935c-5e37-471a-a196-db0a0ef75fd4"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9337), null, "ChatGLM Turbo 文本模型", true, "{}", "ChatGLM", false, "chatglm_turbo", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("6e52d2c8-7f9e-48e1-83a2-0837b772b327"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9345), null, "Claude Instant 1 文本模型", true, "{}", "Claude", false, "claude-instant-1", null, 0.815m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("77f11618-b97d-4210-bb85-edf30502212b"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9292), null, "Gemini Pro 文本模型", true, "{}", "Google", false, "gemini-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("7ac13c53-bfb1-46ec-a590-8a75ca48ea61"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9291), null, "Gemini 1.5 Pro 文本模型", true, "{}", "Google", false, "gemini-1.5-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("7de38e79-c4eb-4a00-b399-5660edd356f4"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9333), null, "4.0 超级文本模型", true, "{}", "Spark", false, "4.0Ultra", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("7e3d9997-30c6-4073-9d57-2e3e9c948aa8"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9339), null, "Claude 2.0 文本模型", true, "{}", "Claude", false, "claude-2.0", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("87242937-38bd-4599-b3e8-e0fdb97ccb8e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9347), null, "GPT Image 图片生成模型", true, "{}", "OpenAI", false, "gpt-image-1", null, 50000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("893bdce0-0cc6-4bfa-805b-1c0053033ed0"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9352), null, "GLM 4 文本模型", true, "{}", "ChatGLM", false, "glm-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("8bef9725-88e0-4b2f-8234-eb326011d8cf"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9288), null, "GPT-4 全部文本模型", true, "{}", "OpenAI", false, "gpt-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u8054\\u7F51\"]", "chat", null },
                    { new Guid("8c53944c-6359-4db0-9302-1bac5198a017"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9349), null, "Embedding BERT 512 v1 嵌入模型", true, "{}", "OpenAI", false, "embedding-bert-512-v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("8ff1ff49-f619-4e94-9cf1-4ce2cc3a20dc"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9338), null, "Claude 2 文本模型", true, "{}", "Claude", false, "claude-2", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("90fd1529-b36d-44da-b58a-feb5efa78d6f"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9294), null, "Gemini 1.5 Flash 文本模型", true, "{}", "Google", false, "gemini-1.5-flash", null, 0.2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("92af6e95-e587-4b00-b53c-298b744a7797"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9266), null, "GPT-3.5 Turbo 0125 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0125", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("96cd6c68-35e2-449c-be9c-08b7f49cd636"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9328), null, "Whisper 1 语音识别模型", true, "{}", "OpenAI", false, "whisper-1", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "stt", null },
                    { new Guid("97e599d6-f506-41df-a1ad-10859588b991"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9277), null, "GPT-4 0125 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-0125-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("986266c0-a592-45b8-9335-d7a6e2cc496f"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9313), null, "GPT-4o 2024-08-06 文本模型", true, "{}", "OpenAI", false, "gpt-4o-2024-08-06", null, 1.25m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("987f084c-7256-4bee-8171-e7863e051f02"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9327), null, "TTS 1 HD 1106 语音合成模型", true, "{}", "OpenAI", false, "tts-1-hd-1106", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("9a5ef7c0-9a02-48e2-82f7-f6520dfe2398"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9276), null, "GPT-4 文本模型", true, "{}", "OpenAI", false, "gpt-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("a10b7dd6-a754-4f1d-b733-aad7d83e9f44"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9273), null, "GPT-3.5 Turbo Instruct 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-instruct", null, 0.001m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("a43269e0-8162-4942-8f39-59508250eb25"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9346), null, "DALL-E 2 图像生成模型", true, "{}", "OpenAI", false, "dall-e-2", null, 8000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("a4db6b8d-b9f1-4678-ae70-5b4716a44663"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9322), null, "Text Embedding 3 Large 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-3-large", null, 0.13m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("a8232267-a5bc-4821-b10d-a811d6c1ffd6"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9351), null, "Embedding S1 v1 嵌入模型", true, "{}", null, false, "embedding_s1_v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("ac029b08-15bd-4a90-94ef-82bc93e8fbbf"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9317), null, "Moonshot v1 8k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-8k", null, 1m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("ae9b18f5-692f-49a4-ab6e-e5e57fb89611"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9305), null, "GPT-4o 文本模型", true, "{}", "OpenAI", false, "gpt-4o", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("b142e8aa-5cc4-4ed2-8bee-9ddb598e73fe"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9323), null, "Text Embedding 3 Small 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-3-small", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("b4ff9dbd-5afa-4b64-958c-3e4338f9d26d"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9317), null, "Text Babbage 001 文本模型", true, "{}", "OpenAI", false, "text-babbage-001", null, 0.25m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("b53a4a6c-e516-496c-abdf-09e801091816"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9335), null, "ChatGLM 标准文本模型", true, "{}", "ChatGLM", false, "chatglm_std", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("b82fc1ea-d672-489f-93b4-a2d1a7aa696f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9271), null, "GPT-3.5 Turbo 16k 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-16k", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("ba246850-1b4f-4992-89a0-66b052f485d1"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9272), null, "GPT-3.5 Turbo 16k 0613 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-16k-0613", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("ba7175a3-b606-4467-917f-81dac3f53b1a"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9309), null, "GPT-4o Mini 2024-07-18 文本模型", true, "{}", "OpenAI", false, "gpt-4o-mini-2024-07-18", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("bd638459-673d-41d5-98ba-5bf79fc4ef56"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9319), null, "Text Davinci 003 文本模型", true, "{}", "OpenAI", false, "text-davinci-003", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("bda46a34-d5cf-4b6c-bdb5-a7cecd90aeac"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9334), null, "ChatGLM Lite 文本模型", true, "{}", "ChatGLM", false, "chatglm_lite", null, 0.1429m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("c36385c4-af0a-4659-b599-57e9cc67f5db"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9270), null, "GPT-3.5 Turbo 1106 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-1106", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("c5382438-a05a-4d76-8af4-1d7d8e87aaa7"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9341), null, "Claude 3.5 Sonnet 20240620 文本模型", true, "{}", "Claude", false, "claude-3-5-sonnet-20240620", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("cb6ebd7a-f2bf-4867-96de-e090a5e0a0de"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9340), null, "Claude 3 Haiku 文本模型", true, "{}", "Claude", false, "claude-3-haiku", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("d539dbaa-4655-4a38-9fda-9be272beb0fe"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9293), null, "Gemini Pro 视觉模型", true, "{}", "Google", false, "gemini-pro-vision", null, 2m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("dbe1350a-c14e-4093-89a0-fa59de230459"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9341), null, "Claude 3 Haiku 20240307 文本模型", true, "{}", "Claude", false, "claude-3-haiku-20240307", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("dd2e5c8a-dd95-46bd-a4a9-b734aeaec0e8"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9352), null, "GLM 3 Turbo 文本模型", true, "{}", "ChatGLM", false, "glm-3-turbo", null, 0.355m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("e0966dfa-80c6-4fe4-8840-290ae5887c7f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9298), null, "GPT-4 Turbo 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo-preview", null, 5m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("e3e607b0-e341-46f8-8d1b-1378aa7a6c14"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9284), null, "GPT-4 32k 0314 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k-0314", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("e491ea9c-5daa-4beb-b780-8c11b29d1022"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9344), null, "Claude 3 Opus 20240229 文本模型", true, "{}", "Claude", false, "claude-3-opus-20240229", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("edf46748-cac8-4faa-88e6-58b4973ca5a5"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9333), null, "通用文本模型 v3.5", true, "{}", "Spark", false, "generalv3.5", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("eec29049-963d-437e-97be-f8f25a9e32df"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9312), null, "GPT-4o 2024-05-13 文本模型", true, "{}", "OpenAI", false, "gpt-4o-2024-05-13", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("f764ddba-df99-44d5-bed2-5c6a81eb57e0"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9290), null, "GPT-4 Turbo 文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("f805f89d-3848-4a8d-9eb4-63fd3aa5e603"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9331), null, "Hunyuan Lite 文本模型", true, "{}", "Hunyuan", false, "hunyuan-lite", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("fb04c6a5-d68a-4360-b2ed-2a46e50ed399"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9281), null, "GPT-4 1106 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-1106-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("fe4cfc2b-9e95-4db3-87ce-18c777ec5b46"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 39, 669, DateTimeKind.Local).AddTicks(9320), null, "Text Davinci Edit 001 文本模型", true, "{}", "OpenAI", false, "text-davinci-edit-001", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null }
                });

            migrationBuilder.UpdateData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: "CA378C74-19E7-458A-918B-4DBB7AE1729D",
                columns: new[] { "CreatedAt", "Key" },
                values: new object[] { new DateTime(2025, 6, 17, 0, 34, 39, 630, DateTimeKind.Local).AddTicks(9194), "sk-O21Dc6aSGADFDUSs9S10kXSlljXfbwjOboabqL" });

            migrationBuilder.UpdateData(
                table: "UserGroups",
                keyColumn: "Id",
                keyValue: new Guid("ca378c74-19e7-458a-918b-4dbb7ae17291"),
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 0, 34, 39, 631, DateTimeKind.Local).AddTicks(7054));

            migrationBuilder.UpdateData(
                table: "UserGroups",
                keyColumn: "Id",
                keyValue: new Guid("ca378c74-19e7-458a-918b-4dbb7ae1729d"),
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 0, 34, 39, 631, DateTimeKind.Local).AddTicks(6654));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "CA378C74-19E7-458A-918B-4DBB7AE1729D",
                columns: new[] { "CreatedAt", "Password", "PasswordHas" },
                values: new object[] { new DateTime(2025, 6, 17, 0, 34, 39, 628, DateTimeKind.Local).AddTicks(8011), "68eabde51f53a742d6014a72777fc8df", "de1822e5300c4907bffb558d7707e4c8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("013398c8-c960-4402-9e4b-5640e49751a3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("01d840e0-86e6-4486-9bdc-d0bfcc7a2d86"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0291b1e7-05ee-40e5-aaca-424c2cb7e952"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("03e9906d-0673-4f8e-97fb-d917ab5504ef"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0b81daa5-8bd2-4ec9-8faa-3ab3b5308331"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0ba6e600-5a6c-4e9b-96d5-71e38a488c26"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0c342a97-5bdf-4658-b842-320ca0dcafcc"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("14fccf76-77ef-4c50-bab7-6beed0e801b6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1a0f4722-5c35-4ca4-bdb3-609bf51db1f8"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1fa08607-a2f3-4b45-9cd2-640c268fed83"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("21cb733d-3ae0-472c-b4b3-d46041a6e42c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("21fc3bb1-9818-462a-9212-cb5ac24676f1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("23883819-cf8e-4196-816c-10a5dbf577ce"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("24610688-1852-4e33-9ed1-f35f2abe6f02"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("24feda79-966c-41b0-a14f-91c4243f5586"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("26ccb343-a569-42d8-9913-dfe01aec892e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("278c2eca-747d-4e31-8bae-b0cbf2ddc3ea"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2efea6ca-e647-444f-ac82-b244daacc8fa"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("33c4797e-8ade-49a9-8485-279fcb117de0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("36802ce4-1bd0-4a26-9519-8092e754ecaf"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("3701c784-01ae-4371-b48e-6b7d027f6190"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("474f228c-40f4-4ba3-b5e0-fd42fa261778"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("4d3834bb-3e15-4d2c-bbb0-a754e8314e86"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5a76a8af-fe28-4d09-87f6-21f22334ce2e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5a8ce9ef-5001-4ddc-8872-d2ff7450b5e2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5c1eb6b6-6827-4c3e-adf3-3f82b86393a4"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5fd530ac-6704-4526-a8dc-83ce571656b0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("638c887b-1ebb-4d19-90f6-71c384815b7a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("640cf7ef-8163-4d79-a4a2-e0ee28baef60"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6a3e1334-2f07-4009-8677-9fd5af06adc9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6ae8935c-5e37-471a-a196-db0a0ef75fd4"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6e52d2c8-7f9e-48e1-83a2-0837b772b327"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("77f11618-b97d-4210-bb85-edf30502212b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7ac13c53-bfb1-46ec-a590-8a75ca48ea61"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7de38e79-c4eb-4a00-b399-5660edd356f4"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7e3d9997-30c6-4073-9d57-2e3e9c948aa8"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("87242937-38bd-4599-b3e8-e0fdb97ccb8e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("893bdce0-0cc6-4bfa-805b-1c0053033ed0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8bef9725-88e0-4b2f-8234-eb326011d8cf"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8c53944c-6359-4db0-9302-1bac5198a017"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8ff1ff49-f619-4e94-9cf1-4ce2cc3a20dc"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("90fd1529-b36d-44da-b58a-feb5efa78d6f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("92af6e95-e587-4b00-b53c-298b744a7797"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("96cd6c68-35e2-449c-be9c-08b7f49cd636"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("97e599d6-f506-41df-a1ad-10859588b991"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("986266c0-a592-45b8-9335-d7a6e2cc496f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("987f084c-7256-4bee-8171-e7863e051f02"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9a5ef7c0-9a02-48e2-82f7-f6520dfe2398"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a10b7dd6-a754-4f1d-b733-aad7d83e9f44"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a43269e0-8162-4942-8f39-59508250eb25"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a4db6b8d-b9f1-4678-ae70-5b4716a44663"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a8232267-a5bc-4821-b10d-a811d6c1ffd6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ac029b08-15bd-4a90-94ef-82bc93e8fbbf"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ae9b18f5-692f-49a4-ab6e-e5e57fb89611"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b142e8aa-5cc4-4ed2-8bee-9ddb598e73fe"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b4ff9dbd-5afa-4b64-958c-3e4338f9d26d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b53a4a6c-e516-496c-abdf-09e801091816"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b82fc1ea-d672-489f-93b4-a2d1a7aa696f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ba246850-1b4f-4992-89a0-66b052f485d1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ba7175a3-b606-4467-917f-81dac3f53b1a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("bd638459-673d-41d5-98ba-5bf79fc4ef56"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("bda46a34-d5cf-4b6c-bdb5-a7cecd90aeac"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c36385c4-af0a-4659-b599-57e9cc67f5db"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c5382438-a05a-4d76-8af4-1d7d8e87aaa7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("cb6ebd7a-f2bf-4867-96de-e090a5e0a0de"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d539dbaa-4655-4a38-9fda-9be272beb0fe"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("dbe1350a-c14e-4093-89a0-fa59de230459"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("dd2e5c8a-dd95-46bd-a4a9-b734aeaec0e8"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e0966dfa-80c6-4fe4-8840-290ae5887c7f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e3e607b0-e341-46f8-8d1b-1378aa7a6c14"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e491ea9c-5daa-4beb-b780-8c11b29d1022"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("edf46748-cac8-4faa-88e6-58b4973ca5a5"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("eec29049-963d-437e-97be-f8f25a9e32df"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f764ddba-df99-44d5-bed2-5c6a81eb57e0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f805f89d-3848-4a8d-9eb4-63fd3aa5e603"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fb04c6a5-d68a-4360-b2ed-2a46e50ed399"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fe4cfc2b-9e95-4db3-87ce-18c777ec5b46"));

            migrationBuilder.InsertData(
                table: "ModelManagers",
                columns: new[] { "Id", "AudioCacheRate", "AudioOutputRate", "AudioPromptRate", "Available", "CacheHitRate", "CacheRate", "CompletionRate", "CreatedAt", "Creator", "Description", "Enable", "Extension", "Icon", "IsVersion2", "Model", "Modifier", "PromptRate", "QuotaMax", "QuotaType", "Tags", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("045b9916-95ad-486c-abdd-b6448e6c4e26"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8437), null, "GPT-4 Turbo 文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("05cdfdf0-7ca2-4dfb-9daa-8d5e172490ef"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8491), null, "GLM 4 全部文本模型", true, "{}", "ChatGLM", false, "glm-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("06b49e8b-41f7-4b25-99c8-db728506c8e7"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8443), null, "Gemini 1.5 Flash 文本模型", true, "{}", "Google", false, "gemini-1.5-flash", null, 0.2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("0e17eb12-00e5-43cf-b30f-0a306fb282ac"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8424), null, "GPT-4 0125 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-0125-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("10105fcb-2563-4dfe-a12d-85b787aa6e68"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8429), null, "GPT-4 1106 视觉预览模型", true, "{}", "OpenAI", false, "gpt-4-1106-vision-preview", null, 10m, "128K", 1, "[\"\\u89C6\\u89C9\",\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("11e679c2-4d46-4a0e-8355-2c906f3b7940"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8468), null, "TTS 1 HD 语音合成模型", true, "{}", "OpenAI", false, "tts-1-hd", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("11fa4873-818d-4bde-9149-f92ef02a625d"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8492), null, "GLM 4v 文本模型", true, "{}", "ChatGLM", false, "glm-4v", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("14fd03c1-8219-4300-b8d3-ea43b145a2fe"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8461), null, "Text Curie 001 文本模型", true, "{}", "OpenAI", false, "text-curie-001", null, 1m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("178b0d59-1b17-4f56-a0e2-da0920f7ad36"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8449), null, "GPT-4 视觉预览模型", true, "{}", "OpenAI", false, "gpt-4-vision-preview", null, 10m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("17c1f7bd-8736-4dcd-95ac-34fe32cbd298"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8484), null, "Claude Instant 1 文本模型", true, "{}", "Claude", false, "claude-instant-1", null, 0.815m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("199a4a54-78e9-4a9a-9745-6d70214a34f2"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8222), null, "GPT-3.5 Turbo 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("1c1fe844-2d2d-4e59-93e7-df87134b4ba7"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8459), null, "Moonshot v1 32k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-32k", null, 2m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("1c38ac7a-74b0-4911-ade0-cd1d4f439407"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8432), null, "GPT-4 32k 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("1e91aa6b-56f4-49e3-a5e7-71cf6d76fc5f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8490), null, "GLM 4 文本模型", true, "{}", "ChatGLM", false, "glm-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("23304565-a367-4ed6-8625-6af23e9fddd5"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8457), null, "GPT-4o 2024-05-13 文本模型", true, "{}", "OpenAI", false, "gpt-4o-2024-05-13", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("2460ceac-14e6-4599-9145-9bd4b6409844"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8484), null, "Claude Instant 1.2 文本模型", true, "{}", "Claude", false, "claude-instant-1.2", null, 0.4m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("24644101-63d0-4e29-ab3d-af3eb03b964b"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8412), null, "GPT-3.5 Turbo 16k 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-16k", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("27a23e1d-6c8d-4ab0-8754-f48adc66dd4f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8460), null, "Text Babbage 001 文本模型", true, "{}", "OpenAI", false, "text-babbage-001", null, 0.25m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("28e6cc8b-0bd5-440d-ab7f-5cbefe8a8481"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8411), null, "GPT-3.5 Turbo 1106 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-1106", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("38f7483d-30b6-42b9-bb31-37a6ebb9bbf6"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8453), null, "GPT-4o Mini 2024-07-18 文本模型", true, "{}", "OpenAI", false, "gpt-4o-mini-2024-07-18", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("39d1439b-7baa-4cd2-98c8-2ec6928951a2"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8473), null, "通用文本模型 v3.5", true, "{}", "Spark", false, "generalv3.5", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("420d2949-738b-4bba-8b79-20ff6d74ec4e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8425), null, "GPT-4 0314 文本模型", true, "{}", "OpenAI", false, "gpt-4-0314", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("423abcff-04d2-4e43-a027-0b03387cfaf3"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8460), null, "Moonshot v1 8k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-8k", null, 1m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("43914fc3-eb3e-415a-952b-249db3c7e8d3"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8486), null, "DALL-E 3 图像生成模型", true, "{}", "OpenAI", false, "dall-e-3", null, 20000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("47f4e75e-85b5-44ce-9798-4fe3d9e885f3"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8485), null, "DALL-E 2 图像生成模型", true, "{}", "OpenAI", false, "dall-e-2", null, 8000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("506f7d2a-0b56-44bb-bf6f-5214375d65c4"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8480), null, "Claude 3 Haiku 20240307 文本模型", true, "{}", "Claude", false, "claude-3-haiku-20240307", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("520b605e-38f7-4503-9ff7-de579ebcffdb"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8489), null, "Embedding S1 v1 嵌入模型", true, "{}", null, false, "embedding_s1_v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("555d37bf-84b6-4b3f-8b53-1114caf8158c"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8478), null, "Claude 2.0 文本模型", true, "{}", "Claude", false, "claude-2.0", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("5628e544-317c-4424-89ae-e3f63bb2370b"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8458), null, "Moonshot v1 128k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-128k", null, 5.06m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("58fdcf2e-407a-4075-a5c4-512a265fa60c"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8439), null, "Gemini Pro 文本模型", true, "{}", "Google", false, "gemini-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("5d8acea9-b9b7-464c-95e9-f222ff65b656"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8488), null, "Embedding 2 嵌入模型", true, "{}", "OpenAI", false, "embedding-2", null, 0.0355m, "", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("605b055f-eaa8-4549-a221-aa98f4873e4c"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8483), null, "Claude 3 Sonnet 20240229 文本模型", true, "{}", "Claude", false, "claude-3-sonnet-20240229", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("63548300-1928-4571-a62d-d24360b5d3d8"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8477), null, "ChatGLM Turbo 文本模型", true, "{}", "ChatGLM", false, "chatglm_turbo", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("6398bfac-a889-4b8f-8ea6-0f7cc5015d2a"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8462), null, "Text Davinci 002 文本模型", true, "{}", "OpenAI", false, "text-davinci-002", null, 10m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("643075d6-c723-4989-b15d-74d650313682"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8470), null, "TTS 1 HD 1106 语音合成模型", true, "{}", "OpenAI", false, "tts-1-hd-1106", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("6a9fb149-96e4-4c45-9abc-30a53f7df89a"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8490), null, "GLM 3 Turbo 文本模型", true, "{}", "ChatGLM", false, "glm-3-turbo", null, 0.355m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("6d24edbf-d1e6-492a-a585-ae761625da7f"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8452), null, "GPT-4o Mini 文本模型", true, "{}", "OpenAI", false, "gpt-4o-mini", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("71ffd7b0-f284-4836-8ce9-330ae5c9d387"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8465), null, "Text Embedding 3 Large 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-3-large", null, 0.13m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("774536a1-5e6d-434b-b878-c07b8bc8ef99"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8409), null, "GPT-3.5 Turbo 0301 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0301", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("777bc64b-4f62-4780-9250-88f5c0561d45"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8428), null, "GPT-4 1106 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-1106-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("780ab4fd-6bd0-4f0c-9e64-4d09933766c9"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8472), null, "通用文本模型 v3", true, "{}", "Spark", false, "generalv3", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("79d48680-1f23-4d0e-b39d-1878783fb7f9"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8451), null, "ChatGPT 4o 最新文本模型", true, "{}", "OpenAI", false, "chatgpt-4o-latest", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("7b36dd72-0439-4467-84e5-4d8a84a2729b"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8438), null, "Gemini 1.5 Pro 文本模型", true, "{}", "Google", false, "gemini-1.5-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("7d24e581-1175-493c-a2a3-b56f73177c1a"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8436), null, "GPT-4 全部文本模型", true, "{}", "OpenAI", false, "gpt-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u8054\\u7F51\"]", "chat", null },
                    { new Guid("7ef50946-e8c6-481c-bb27-2fc7187a3c46"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8483), null, "Claude 3 Opus 20240229 文本模型", true, "{}", "Claude", false, "claude-3-opus-20240229", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("857da218-fade-4bb7-961d-688091bb7879"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8450), null, "GPT-4o 文本模型", true, "{}", "OpenAI", false, "gpt-4o", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("89dcbccd-bd9e-48df-9176-495051dbb622"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8472), null, "通用文本模型", true, "{}", "Spark", false, "general", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("8aa29990-5dc9-45f4-b7e3-de9ad8812e84"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8445), null, "GPT-4 Turbo 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo-preview", null, 5m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("8b9fe7d2-52ac-44a1-a2b6-abec89daa2d0"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8480), null, "Claude 3 Haiku 文本模型", true, "{}", "Claude", false, "claude-3-haiku", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("9c1aad0d-0a54-48f7-9151-56f2f1e0adf4"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8422), null, "GPT-3.5 Turbo Instruct 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-instruct", null, 0.001m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("a3c5780b-d71f-4a57-8783-af4b2cdb673c"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8440), null, "Gemini Pro 视觉模型", true, "{}", "Google", false, "gemini-pro-vision", null, 2m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("a5bc978e-8e9e-4ad8-bb9c-40d3ba569956"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8474), null, "4.0 超级文本模型", true, "{}", "Spark", false, "4.0Ultra", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("b1401c04-c5e7-4423-9435-3d0f8f6537de"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8467), null, "TTS 1 1106 语音合成模型", true, "{}", "OpenAI", false, "tts-1-1106", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("b33b4c70-9efe-4fb7-a311-ffcb6f439a13"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8476), null, "ChatGLM Pro 文本模型", true, "{}", "ChatGLM", false, "chatglm_pro", null, 0.7143m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("b6f6db90-b908-418d-aaee-f36f38335c27"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8465), null, "Text Embedding 3 Small 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-3-small", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("b7b1335c-33f0-4c4b-af11-bd17c0938735"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8476), null, "ChatGLM 标准文本模型", true, "{}", "ChatGLM", false, "chatglm_std", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("b90da15c-e614-494f-bed2-0851808c6702"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8423), null, "GPT-4 文本模型", true, "{}", "OpenAI", false, "gpt-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("bcbdc906-9748-4e94-8abc-d0c92f1c2860"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8410), null, "GPT-3.5 Turbo 0613 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0613", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("c01358a7-cb73-49b4-a1a1-1847a1991262"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8464), null, "Text Davinci Edit 001 文本模型", true, "{}", "OpenAI", false, "text-davinci-edit-001", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("caaf7531-7afc-4653-ad04-248c94a4d8a2"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8486), null, "GPT Image 图片生成模型", true, "{}", "OpenAI", false, "gpt-image-1", null, 50000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("ccc96dfa-3b4a-4fbe-8d81-aba8dc7daef8"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8479), null, "Claude 2.1 文本模型", true, "{}", "Claude", false, "claude-2.1", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("d2509342-3877-4ca6-b7fe-894217b54791"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8466), null, "Text Embedding Ada 002 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-ada-002", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("d41324de-ed36-49fe-bcf6-874263c32dc5"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8478), null, "Claude 2 文本模型", true, "{}", "Claude", false, "claude-2", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("d4ec56d0-f7d7-4840-b97b-553bf7e9c6a7"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8408), null, "GPT-3.5 Turbo 0125 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0125", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("d8694f43-4568-4f15-be1e-753bc66b8fea"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8470), null, "Whisper 1 语音识别模型", true, "{}", "OpenAI", false, "whisper-1", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "stt", null },
                    { new Guid("e0f43ebd-84b6-4212-8bfa-eb2d72da95e6"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8434), null, "GPT-4 32k 0314 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k-0314", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("e45c52f2-cd86-4577-b114-cf277ba159e2"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8471), null, "Hunyuan Lite 文本模型", true, "{}", "Hunyuan", false, "hunyuan-lite", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("e513abed-4088-4c1c-a086-865cfa62cd1c"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8489), null, "Embedding BERT 512 v1 嵌入模型", true, "{}", "OpenAI", false, "embedding-bert-512-v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("e625ec04-5661-4be6-8c89-613b8730da82"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8466), null, "TTS 1 语音合成模型", true, "{}", "OpenAI", false, "tts-1", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("e8810623-b845-4abf-8595-9ea14144df80"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8474), null, "ChatGLM Lite 文本模型", true, "{}", "ChatGLM", false, "chatglm_lite", null, 0.1429m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("f236f5d9-0149-4ada-b952-aa535a9b7e43"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8458), null, "GPT-4o 2024-08-06 文本模型", true, "{}", "OpenAI", false, "gpt-4o-2024-08-06", null, 1.25m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("f5da1519-2684-4d7e-be38-9f9e916fcbe5"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8482), null, "Claude 3.5 Sonnet 20240620 文本模型", true, "{}", "Claude", false, "claude-3-5-sonnet-20240620", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("f71ef040-036f-41cb-92ec-5c33e5e0b326"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8435), null, "GPT-4 32k 0613 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k-0613", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("f7dd376d-4f16-4855-ba7b-c93c069e1f09"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8463), null, "Text Davinci 003 文本模型", true, "{}", "OpenAI", false, "text-davinci-003", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("fb58d77a-dc58-440b-befa-a6bb26d95c86"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8421), null, "GPT-3.5 Turbo 16k 0613 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-16k-0613", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("fce93319-c34a-4693-90da-0e24ab703513"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8426), null, "GPT-4 0613 文本模型", true, "{}", "OpenAI", false, "gpt-4-0613", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("fedb5ce5-acca-416f-a908-9e25971d3b8d"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 57, 244, DateTimeKind.Local).AddTicks(8444), null, "GPT-4 Turbo 2024-04-09 文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo-2024-04-09", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null }
                });

            migrationBuilder.UpdateData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: "CA378C74-19E7-458A-918B-4DBB7AE1729D",
                columns: new[] { "CreatedAt", "Key" },
                values: new object[] { new DateTime(2025, 6, 16, 23, 28, 57, 207, DateTimeKind.Local).AddTicks(3681), "sk-XcpQLm8ePsdt8AIAhkvG0QMqLUG2lrDbUVtDZA" });

            migrationBuilder.UpdateData(
                table: "UserGroups",
                keyColumn: "Id",
                keyValue: new Guid("ca378c74-19e7-458a-918b-4dbb7ae17291"),
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 23, 28, 57, 208, DateTimeKind.Local).AddTicks(1598));

            migrationBuilder.UpdateData(
                table: "UserGroups",
                keyColumn: "Id",
                keyValue: new Guid("ca378c74-19e7-458a-918b-4dbb7ae1729d"),
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 23, 28, 57, 208, DateTimeKind.Local).AddTicks(1251));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "CA378C74-19E7-458A-918B-4DBB7AE1729D",
                columns: new[] { "CreatedAt", "Password", "PasswordHas" },
                values: new object[] { new DateTime(2025, 6, 16, 23, 28, 57, 205, DateTimeKind.Local).AddTicks(3500), "cea387cd7ab16b6a6ab3e585ce973dc2", "4e7a5d4dd4e9402ea63e1a2bfd5d9bc8" });
        }
    }
}
