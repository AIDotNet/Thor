using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Thor.Provider.PostgreSql.Thor
{
    /// <inheritdoc />
    public partial class UpdateModelMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("01df98df-b4ba-461b-a79a-f37e524fb8fa"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("07cd9d8e-62b6-4b1c-847f-3bfe86ca0b9f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("09af83d4-627b-482e-8629-194048050bf7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("09f145f1-69bf-42a6-91f3-ef99812cd6e9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0cf3d41d-91b4-47a6-91e4-c9d23a1f7800"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1060026a-03bf-42d6-89cf-a42cd67342d3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("147ff81f-c887-4ade-9b65-8aaac0f3862c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1689c35a-4a4a-4b36-877b-a82337a897ed"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("16fb9275-75a4-4b04-9ca9-98d4eeb7f392"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1ba7dbb7-df1f-4712-bf43-20329702d7dd"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1dc3f3f0-bd3b-49f1-8ef6-cf18e0297870"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1ed3d1bf-ae16-4b2d-ba17-77af61a4fd20"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1f88e433-3d74-478e-ad1f-681e3fadfa8e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("26e0fe6e-8f00-4ca9-8e12-a0d8155ad189"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2e21907e-040d-49ea-bea8-4c0bca3c518b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("326da002-9952-4e22-a30c-f35fc129e3cc"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("384e5bd9-bc99-4cd6-a4f4-e5f81e5be33f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("38a5ab77-036f-4cf4-8826-4f40841bb435"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("3ea81547-0ea8-4b16-b8f2-968e0ab87384"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("45c8fd31-b2d1-4912-97b0-6083ca8286be"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("4bf1813f-0a66-4b4a-aa2d-c81cac02b91d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("4d799f91-781d-48f8-8d26-7d5c9c6ea74a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("50af2f81-68b6-4685-ab23-3d3a3b733b3d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("52c39472-96da-43cc-bd1d-85bc3d8f638b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("53102626-efdb-4008-8cb4-741904d09094"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5c7a1119-e216-40bb-99e1-3b1a2b23a8be"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5cb662b9-1c22-4fbb-8ed6-7c1cf2eba5a8"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5eb5d921-e7b3-454f-8a45-b7c1535d5530"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5f98d35e-ca5b-40c8-9644-48ec589defea"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("62f5ade9-6d90-4087-9715-6b948e89730c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("63ea8004-e0ec-4a3a-9c9c-a36dcc3fa124"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("64746208-7e31-4ac6-9d0f-c9c71c4d6f57"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("66bea95e-76d7-4ffb-9c42-a7c7e489af4c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6d0402b4-a63f-4cf7-b41a-caf8eeb4ee4b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("72d88a5e-a1e1-4d7c-8516-b2e8da2b2c7e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("730f9616-15fa-4cc6-92d9-b88b520cfadd"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("744a501d-42e7-4083-b8f0-c7b477896281"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7886e959-8b9c-4321-8ae4-e95c6d72ca1a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("83ddb6b7-fb63-456a-9625-c2e0a4b83672"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("83fd25e2-c5e8-4b97-a2db-0ebc5a45cf2a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8bc43469-4a68-40cf-b8aa-dd801bdc14b9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8c50d2ef-3228-43ab-8ab6-476e6ee0a2a9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8cd5436f-8e2b-48dc-a91e-f3b54c5f16b1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8f798700-7e9d-4e24-81d5-5317e884356b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9b894d15-baa2-4db0-874f-eb749e66761a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9c91614f-01af-4903-bfbf-b17359bbc8d3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9e0d699d-1705-4701-83ef-4d9eb169315f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a0560099-f0ea-42c5-a482-10f5fd68f0aa"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a3dfd9ab-1835-4fcd-affe-9f2de6e25377"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a5670e89-67ab-4b25-beb5-0d05932ffaa7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a84853bb-958d-4a5c-a128-43ff677fe109"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b1c474d0-bde7-4ffd-ba8a-648aaa4d345a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b43a661f-912d-4796-b07b-9575bee03c01"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b79e730a-fe4f-41f0-ae35-3bc6b635ec1d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b8d4a4fb-14de-4783-9d9b-b26bd755564a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("bc13e726-486c-40df-bcf4-05c0c7d273dc"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("bccb2ceb-b0e4-459b-bc20-cce719eb7f36"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c05823d8-4697-4446-a95f-647caf9efa8e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c2973c1c-4c02-4dcc-9485-73dc64b51507"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c8abfaa0-fbd7-4d7e-a871-accce0af1f63"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c8b66ff1-93ac-4335-9cee-f65abaa8afe5"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d214bbff-f190-4064-8b3e-ff0337294fb1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d497ea2e-66e8-4cf8-ba7e-7b7125344502"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d5c67cb2-af24-4534-9eb1-723b2487882d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d689de96-a263-4499-97d0-7cd9ee7cc400"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("db7a8222-b62b-4ff0-bee0-7557386861a9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("dbc4d4e6-f7b6-4532-96ae-7c85e7313a88"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("dd2ed225-1428-4575-af64-2d031cb3b17d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("dde1cd9c-9464-4af5-b2bb-a6faabd31418"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("df0e3a76-a30a-4de5-9b4b-8b966277361f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e8c5278e-af09-4792-8db5-27721f62982f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ebee8cb5-059a-4af3-a677-5edc846cf216"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ec27d994-f4e2-42d1-a23d-a7859ee1e4a3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ef207545-f6ab-48d9-834c-3652f4d99555"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f10529c0-3e98-4a2a-bdea-bd68c010dfb1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fb4ef43a-8530-4df0-97ab-371e1fb32f6e"));

            migrationBuilder.DeleteData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: "e948a8e2-83b5-4ad6-aa00-ea28127fc1e0");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "f8856afe-18f5-4167-86d5-ee94e3879ca2");

            migrationBuilder.CreateTable(
                name: "ModelMaps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ModelId = table.Column<string>(type: "text", nullable: false),
                    ModelMapItems = table.Column<string>(type: "text", nullable: false),
                    Group = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Modifier = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Creator = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelMaps", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ModelManagers",
                columns: new[] { "Id", "AudioCacheRate", "AudioOutputRate", "AudioPromptRate", "Available", "CacheHitRate", "CacheRate", "CompletionRate", "CreatedAt", "Creator", "Description", "Enable", "Icon", "IsVersion2", "Model", "Modifier", "PromptRate", "QuotaMax", "QuotaType", "Tags", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("03ae1de9-949b-4bc2-9a93-619f3708e0ff"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8034), null, "GPT-4 Turbo 文本模型", true, "OpenAI", false, "gpt-4-turbo", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("0bcb8293-6f18-41d7-8a67-8ed87b6037b1"), null, null, null, true, null, null, 5m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8079), null, "Claude 3 Opus 20240229 文本模型", true, "Claude", false, "claude-3-opus-20240229", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("0c3a222a-db09-41e3-8a5f-ad85f09665a0"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8073), null, "ChatGLM Turbo 文本模型", true, "ChatGLM", false, "chatglm_turbo", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("108ef915-be27-495c-8226-7d718eb8b04f"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8075), null, "Claude 2.1 文本模型", true, "Claude", false, "claude-2.1", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1226b02d-a6b5-4cd2-8693-6138cca38a8e"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8069), null, "ChatGLM Lite 文本模型", true, "ChatGLM", false, "chatglm_lite", null, 0.1429m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("142a36b6-59a0-40ca-a52e-2a8c5d4b6e4b"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8059), null, "Text Embedding Ada 002 嵌入模型", true, "OpenAI", false, "text-embedding-ada-002", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("16c70f9c-1511-4929-9e22-408e8dde18ee"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8028), null, "GPT-4 1106 预览文本模型", true, "OpenAI", false, "gpt-4-1106-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1a052f51-02e6-4a1f-8876-470e65b6a060"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8061), null, "TTS 1 1106 语音合成模型", true, "OpenAI", false, "tts-1-1106", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("1ae4e068-58c1-48dd-b137-116566218e57"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8063), null, "TTS 1 HD 1106 语音合成模型", true, "OpenAI", false, "tts-1-hd-1106", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("1b93eacf-e114-4789-a93d-8d1c6bb1ce31"), null, null, null, true, null, null, 2m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8034), null, "Gemini 1.5 Pro 文本模型", true, "Google", false, "gemini-1.5-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("21a5d49c-c598-420d-afa7-ce623e81ed6c"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8047), null, "Moonshot v1 32k 文本模型", true, "Moonshot", false, "moonshot-v1-32k", null, 2m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("2618cdf4-694a-481a-9367-4906db6ad804"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8014), null, "GPT-3.5 Turbo 0125 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0125", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("28ae39d9-4fd6-4cb8-beca-031dfcc1aa6c"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8026), null, "GPT-4 0314 文本模型", true, "OpenAI", false, "gpt-4-0314", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("29013cf9-d0c4-48a0-a324-3b848fcc6f0d"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8051), null, "Text Davinci Edit 001 文本模型", true, "OpenAI", false, "text-davinci-edit-001", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("2bcedc03-771c-4087-ae3b-c168606fa384"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8020), null, "GPT-3.5 Turbo 16k 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("2c704f09-422d-4144-b4f3-959ab332b13e"), null, null, null, true, null, null, 4m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8043), null, "GPT-4o Mini 2024-07-18 文本模型", true, "OpenAI", false, "gpt-4o-mini-2024-07-18", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("2d721257-3096-4c0f-b7a2-9f45dc8c681b"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8037), null, "GPT-4 Turbo 2024-04-09 文本模型", true, "OpenAI", false, "gpt-4-turbo-2024-04-09", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("2d77a0ef-8835-4a42-86e7-11bd46380d7f"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8016), null, "GPT-3.5 Turbo 0301 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0301", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("2f925c57-32a4-4283-9a5e-ba1f0b6563c8"), null, null, null, true, null, null, 2m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8067), null, "通用文本模型 v3", true, "Spark", false, "generalv3", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("33046c9a-30b1-4993-b985-e1ef8de4cb02"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8029), null, "GPT-4 1106 视觉预览模型", true, "OpenAI", false, "gpt-4-1106-vision-preview", null, 10m, "128K", 1, "[\"\\u89C6\\u89C9\",\"\\u6587\\u672C\"]", null },
                    { new Guid("33d8caf5-e3e7-4a45-8723-237f74564b39"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8027), null, "GPT-4 0613 文本模型", true, "OpenAI", false, "gpt-4-0613", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("383edcbd-c28f-4c83-8dd6-6f2ecb7c4855"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8021), null, "GPT-3.5 Turbo 16k 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k-0613", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("3c2f3473-d58d-417b-98ff-573d2cbee275"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8071), null, "ChatGLM 标准文本模型", true, "ChatGLM", false, "chatglm_std", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("402c1a5f-4fe2-45c1-be98-fe740d441fa7"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8039), null, "GPT-4 Turbo 预览文本模型", true, "OpenAI", false, "gpt-4-turbo-preview", null, 5m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("453bb65c-b17b-423f-a659-b226beae253a"), null, null, null, true, null, null, 3m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8069), null, "4.0 超级文本模型", true, "Spark", false, "4.0Ultra", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("45ccb45f-d088-47b0-8428-05ecf30d1e0e"), null, null, null, true, null, null, 5m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8076), null, "Claude 3 Haiku 20240307 文本模型", true, "Claude", false, "claude-3-haiku-20240307", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("51765bbb-bbfa-40de-b243-6edcc71b5406"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8087), null, "GLM 4 全部文本模型", true, "ChatGLM", false, "glm-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("51c6076d-3c09-44ac-95bc-c7b5e948cb74"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8030), null, "GPT-4 32k 0314 文本模型", true, "OpenAI", false, "gpt-4-32k-0314", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("53258cbf-66b4-49d8-925d-7f9bc5dcf8b2"), null, null, null, true, null, null, 3m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8036), null, "Gemini 1.5 Flash 文本模型", true, "Google", false, "gemini-1.5-flash", null, 0.2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("5855d96c-53b6-4ab3-8aa5-3ed232145a20"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8020), null, "GPT-3.5 Turbo 1106 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-1106", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("59c09f18-29ca-4bbd-9bf4-f7b1a6a3234f"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8086), null, "GLM 3 Turbo 文本模型", true, "ChatGLM", false, "glm-3-turbo", null, 0.355m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("5d54cd7c-5291-4e7a-9e31-06ed65d2e774"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8082), null, "DALL-E 3 图像生成模型", true, "OpenAI", false, "dall-e-3", null, 20m, null, 2, "[\"\\u56FE\\u7247\"]", null },
                    { new Guid("68e6b95d-0629-4eac-a22a-2403de2b8256"), null, null, null, true, null, null, 4m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8044), null, "GPT-4o 2024-08-06 文本模型", true, "OpenAI", false, "gpt-4o-2024-08-06", null, 1.25m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("6f8a24d0-c4aa-4a34-ac9d-a6818f6e389b"), null, null, null, true, null, null, 2m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8068), null, "通用文本模型 v3.5", true, "Spark", false, "generalv3.5", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("74beb911-1264-46a2-b956-1e1e77ce2241"), null, null, null, true, null, null, 5m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8077), null, "Claude 3.5 Sonnet 20240620 文本模型", true, "Claude", false, "claude-3-5-sonnet-20240620", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("75618118-9b6a-47e4-b83e-e6a3da84f05c"), null, null, null, true, null, null, 3m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8035), null, "Gemini Pro 文本模型", true, "Google", false, "gemini-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("76d31933-b038-44ac-af64-40cfef0fc86a"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8023), null, "GPT-3.5 Turbo Instruct 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-instruct", null, 0.001m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("78fcca8c-bbad-42ca-936e-f81dff8e766c"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8046), null, "Moonshot v1 128k 文本模型", true, "Moonshot", false, "moonshot-v1-128k", null, 5.06m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("7a9b3273-949c-4f39-a63e-da339dd84159"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8083), null, "Embedding 2 嵌入模型", true, "OpenAI", false, "embedding-2", null, 0.0355m, "", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("7aee991d-3d5b-409e-b001-394e5ef397fb"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8087), null, "GLM 4 文本模型", true, "ChatGLM", false, "glm-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("81961d3e-e9ed-47bb-a81e-e95d2bd7c9e1"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8040), null, "GPT-4 视觉预览模型", true, "OpenAI", false, "gpt-4-vision-preview", null, 10m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("8951031b-619f-47a9-a482-7c2b71028e60"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8050), null, "Text Davinci 003 文本模型", true, "OpenAI", false, "text-davinci-003", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("8a919f96-0ec5-4b1e-a0fa-f2439ae446e7"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8032), null, "GPT-4 32k 0613 文本模型", true, "OpenAI", false, "gpt-4-32k-0613", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("8ab98e95-86de-452d-b155-55ac448d1e2d"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8073), null, "Claude 2 文本模型", true, "Claude", false, "claude-2", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("8b756a96-7229-4914-92c7-d743f5c1eda9"), null, null, null, true, null, null, 5m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8075), null, "Claude 3 Haiku 文本模型", true, "Claude", false, "claude-3-haiku", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("8f113570-8088-4d6f-8fb9-28bbe66cec2c"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(7756), null, "GPT-3.5 Turbo 文本模型", true, "OpenAI", false, "gpt-3.5-turbo", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("96949aab-e78a-4083-92e7-7138526511ca"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8088), null, "GLM 4v 文本模型", true, "ChatGLM", false, "glm-4v", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("986b40c0-067c-4f19-8ca4-6816d5a3416b"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8048), null, "Moonshot v1 8k 文本模型", true, "Moonshot", false, "moonshot-v1-8k", null, 1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("98908468-228b-4864-a65d-6639f357e841"), null, null, null, true, null, null, 2m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8066), null, "通用文本模型", true, "Spark", false, "general", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("9ad2f7b8-ab3d-4d39-a937-5eca1d165d3e"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8019), null, "GPT-3.5 Turbo 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0613", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("9f2ae5f0-7829-49c1-b699-d08a588bfee4"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8050), null, "Text Davinci 002 文本模型", true, "OpenAI", false, "text-davinci-002", null, 10m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a42ca1c9-ec78-4418-b617-b4ff152e34f3"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8048), null, "Text Babbage 001 文本模型", true, "OpenAI", false, "text-babbage-001", null, 0.25m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a51b4041-ba0e-4070-8c37-b4d24c8be2af"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8060), null, "TTS 1 语音合成模型", true, "OpenAI", false, "tts-1", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("a7eb5c40-0f6c-4c09-b18c-f9b154d194d7"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8084), null, "Embedding S1 v1 嵌入模型", true, null, false, "embedding_s1_v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("ab2deacb-c68b-444e-a8db-8e74c78247e9"), null, null, null, true, null, null, 3m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8041), null, "GPT-4o 文本模型", true, "OpenAI", false, "gpt-4o", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("b4451282-77b8-4bd6-bb3b-0b7417a37cdf"), null, null, null, true, null, null, 3m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8036), null, "Gemini Pro 视觉模型", true, "Google", false, "gemini-pro-vision", null, 2m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("b57ce632-b1ee-4586-ac87-c9dc5b3ec8c1"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8084), null, "Embedding BERT 512 v1 嵌入模型", true, "OpenAI", false, "embedding-bert-512-v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("c30d3c4c-6f74-4ecb-9f2a-9cdb6eb2a31c"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8025), null, "GPT-4 文本模型", true, "OpenAI", false, "gpt-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("c5a3ebe3-df78-4dc2-bbbd-ccee13a83030"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8065), null, "Hunyuan Lite 文本模型", true, "Hunyuan", false, "hunyuan-lite", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("c690c3f0-f6f7-4236-af3c-033f1535181c"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8082), null, "DALL-E 2 图像生成模型", true, "OpenAI", false, "dall-e-2", null, 8m, null, 2, "[\"\\u56FE\\u7247\"]", null },
                    { new Guid("cb1e5768-0d12-4259-be2e-d8708a84eaea"), null, null, null, true, null, null, 5m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8077), null, "Claude 3 Sonnet 20240229 文本模型", true, "Claude", false, "claude-3-sonnet-20240229", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("cb5b6f2f-1907-4313-add3-67715433068c"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8074), null, "Claude 2.0 文本模型", true, "Claude", false, "claude-2.0", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ce96b674-5d64-408c-8118-1a06cecf4032"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8053), null, "Text Embedding 3 Large 嵌入模型", true, "OpenAI", false, "text-embedding-3-large", null, 0.13m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("cea7412b-b084-4c04-8744-7b2c3b32d4c7"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8081), null, "Claude Instant 1.2 文本模型", true, "Claude", false, "claude-instant-1.2", null, 0.4m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("d0664031-d444-458e-a627-b3bf2bb44a85"), null, null, null, true, null, null, 4m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8044), null, "GPT-4o 2024-05-13 文本模型", true, "OpenAI", false, "gpt-4o-2024-05-13", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("d4be69a2-1ed7-46f7-8d11-8767ebf5d68a"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8030), null, "GPT-4 32k 文本模型", true, "OpenAI", false, "gpt-4-32k", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("d6dafdc7-10cc-496d-9073-0474679f99b6"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8063), null, "Whisper 1 语音识别模型", true, "OpenAI", false, "whisper-1", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("d8d65faa-7a36-4fbf-bc30-9447d2401db6"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8026), null, "GPT-4 0125 预览文本模型", true, "OpenAI", false, "gpt-4-0125-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("e1b32085-062f-4933-ba8d-05a8f2584006"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8080), null, "Claude Instant 1 文本模型", true, "Claude", false, "claude-instant-1", null, 0.815m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("e4174c46-7094-4e34-a068-65b3c1d87d46"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8049), null, "Text Curie 001 文本模型", true, "OpenAI", false, "text-curie-001", null, 1m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ee4edee7-c37a-4ce3-bb95-6685dd5e6513"), null, null, null, true, null, null, 4m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8042), null, "GPT-4o Mini 文本模型", true, "OpenAI", false, "gpt-4o-mini", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("f38124c0-6d11-4921-a82e-0f134f900d97"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8070), null, "ChatGLM Pro 文本模型", true, "ChatGLM", false, "chatglm_pro", null, 0.7143m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("f3d28db9-1555-40c5-9943-a192ae5d0797"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8033), null, "GPT-4 全部文本模型", true, "OpenAI", false, "gpt-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u8054\\u7F51\"]", null },
                    { new Guid("f5a327c5-b83a-4f02-bcfc-b6eeebe1bfd8"), null, null, null, true, null, null, 4m, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8041), null, "ChatGPT 4o 最新文本模型", true, "OpenAI", false, "chatgpt-4o-latest", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("fc4559ae-6b71-4dcf-a20c-7f226028b8f8"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8061), null, "TTS 1 HD 语音合成模型", true, "OpenAI", false, "tts-1-hd", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("fd03f942-cd1c-4072-8d10-8a4bb2559251"), null, null, null, true, null, null, null, new DateTime(2025, 3, 10, 2, 26, 46, 777, DateTimeKind.Local).AddTicks(8054), null, "Text Embedding 3 Small 嵌入模型", true, "OpenAI", false, "text-embedding-3-small", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", null }
                });

            migrationBuilder.InsertData(
                table: "Tokens",
                columns: new[] { "Id", "AccessedTime", "CreatedAt", "Creator", "DeletedAt", "Disabled", "ExpiredTime", "IsDelete", "Key", "LimitModels", "Modifier", "Name", "RemainQuota", "UnlimitedExpired", "UnlimitedQuota", "UpdatedAt", "UsedQuota", "WhiteIpList" },
                values: new object[] { "cc72b900-0392-47cf-b242-d0171c7adab7", null, new DateTime(2025, 3, 10, 2, 26, 46, 726, DateTimeKind.Local).AddTicks(2709), "eb84af52-d119-42cc-b9f7-62a920d36539", null, false, null, false, "sk-0TIlbXSvkFFJeS0gl8iM2X1ZfZTSr3IL4isO5R", "[]", null, "默认Token", 0L, true, true, null, 0L, "[]" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "Groups", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "eb84af52-d119-42cc-b9f7-62a920d36539", null, 0L, new DateTime(2025, 3, 10, 2, 26, 46, 724, DateTimeKind.Local).AddTicks(8637), null, null, "239573049@qq.com", "[\"admin\"]", false, false, null, "4be25ee95c675c3090d42587375eed44", "4031e50ba10749aab3c488a3269f4632", 0L, 1000000000L, "admin", null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_ModelMaps_ModelId",
                table: "ModelMaps",
                column: "ModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModelMaps");

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("03ae1de9-949b-4bc2-9a93-619f3708e0ff"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0bcb8293-6f18-41d7-8a67-8ed87b6037b1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0c3a222a-db09-41e3-8a5f-ad85f09665a0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("108ef915-be27-495c-8226-7d718eb8b04f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1226b02d-a6b5-4cd2-8693-6138cca38a8e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("142a36b6-59a0-40ca-a52e-2a8c5d4b6e4b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("16c70f9c-1511-4929-9e22-408e8dde18ee"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1a052f51-02e6-4a1f-8876-470e65b6a060"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1ae4e068-58c1-48dd-b137-116566218e57"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1b93eacf-e114-4789-a93d-8d1c6bb1ce31"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("21a5d49c-c598-420d-afa7-ce623e81ed6c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2618cdf4-694a-481a-9367-4906db6ad804"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("28ae39d9-4fd6-4cb8-beca-031dfcc1aa6c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("29013cf9-d0c4-48a0-a324-3b848fcc6f0d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2bcedc03-771c-4087-ae3b-c168606fa384"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2c704f09-422d-4144-b4f3-959ab332b13e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2d721257-3096-4c0f-b7a2-9f45dc8c681b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2d77a0ef-8835-4a42-86e7-11bd46380d7f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2f925c57-32a4-4283-9a5e-ba1f0b6563c8"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("33046c9a-30b1-4993-b985-e1ef8de4cb02"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("33d8caf5-e3e7-4a45-8723-237f74564b39"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("383edcbd-c28f-4c83-8dd6-6f2ecb7c4855"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("3c2f3473-d58d-417b-98ff-573d2cbee275"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("402c1a5f-4fe2-45c1-be98-fe740d441fa7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("453bb65c-b17b-423f-a659-b226beae253a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("45ccb45f-d088-47b0-8428-05ecf30d1e0e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("51765bbb-bbfa-40de-b243-6edcc71b5406"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("51c6076d-3c09-44ac-95bc-c7b5e948cb74"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("53258cbf-66b4-49d8-925d-7f9bc5dcf8b2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5855d96c-53b6-4ab3-8aa5-3ed232145a20"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("59c09f18-29ca-4bbd-9bf4-f7b1a6a3234f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5d54cd7c-5291-4e7a-9e31-06ed65d2e774"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("68e6b95d-0629-4eac-a22a-2403de2b8256"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6f8a24d0-c4aa-4a34-ac9d-a6818f6e389b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("74beb911-1264-46a2-b956-1e1e77ce2241"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("75618118-9b6a-47e4-b83e-e6a3da84f05c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("76d31933-b038-44ac-af64-40cfef0fc86a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("78fcca8c-bbad-42ca-936e-f81dff8e766c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7a9b3273-949c-4f39-a63e-da339dd84159"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7aee991d-3d5b-409e-b001-394e5ef397fb"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("81961d3e-e9ed-47bb-a81e-e95d2bd7c9e1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8951031b-619f-47a9-a482-7c2b71028e60"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8a919f96-0ec5-4b1e-a0fa-f2439ae446e7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8ab98e95-86de-452d-b155-55ac448d1e2d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8b756a96-7229-4914-92c7-d743f5c1eda9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8f113570-8088-4d6f-8fb9-28bbe66cec2c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("96949aab-e78a-4083-92e7-7138526511ca"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("986b40c0-067c-4f19-8ca4-6816d5a3416b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("98908468-228b-4864-a65d-6639f357e841"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9ad2f7b8-ab3d-4d39-a937-5eca1d165d3e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9f2ae5f0-7829-49c1-b699-d08a588bfee4"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a42ca1c9-ec78-4418-b617-b4ff152e34f3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a51b4041-ba0e-4070-8c37-b4d24c8be2af"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a7eb5c40-0f6c-4c09-b18c-f9b154d194d7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ab2deacb-c68b-444e-a8db-8e74c78247e9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b4451282-77b8-4bd6-bb3b-0b7417a37cdf"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b57ce632-b1ee-4586-ac87-c9dc5b3ec8c1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c30d3c4c-6f74-4ecb-9f2a-9cdb6eb2a31c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c5a3ebe3-df78-4dc2-bbbd-ccee13a83030"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c690c3f0-f6f7-4236-af3c-033f1535181c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("cb1e5768-0d12-4259-be2e-d8708a84eaea"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("cb5b6f2f-1907-4313-add3-67715433068c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ce96b674-5d64-408c-8118-1a06cecf4032"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("cea7412b-b084-4c04-8744-7b2c3b32d4c7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d0664031-d444-458e-a627-b3bf2bb44a85"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d4be69a2-1ed7-46f7-8d11-8767ebf5d68a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d6dafdc7-10cc-496d-9073-0474679f99b6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d8d65faa-7a36-4fbf-bc30-9447d2401db6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e1b32085-062f-4933-ba8d-05a8f2584006"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e4174c46-7094-4e34-a068-65b3c1d87d46"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ee4edee7-c37a-4ce3-bb95-6685dd5e6513"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f38124c0-6d11-4921-a82e-0f134f900d97"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f3d28db9-1555-40c5-9943-a192ae5d0797"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f5a327c5-b83a-4f02-bcfc-b6eeebe1bfd8"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fc4559ae-6b71-4dcf-a20c-7f226028b8f8"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fd03f942-cd1c-4072-8d10-8a4bb2559251"));

            migrationBuilder.DeleteData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: "cc72b900-0392-47cf-b242-d0171c7adab7");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "eb84af52-d119-42cc-b9f7-62a920d36539");

            migrationBuilder.InsertData(
                table: "ModelManagers",
                columns: new[] { "Id", "AudioCacheRate", "AudioOutputRate", "AudioPromptRate", "Available", "CacheHitRate", "CacheRate", "CompletionRate", "CreatedAt", "Creator", "Description", "Enable", "Icon", "IsVersion2", "Model", "Modifier", "PromptRate", "QuotaMax", "QuotaType", "Tags", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("01df98df-b4ba-461b-a79a-f37e524fb8fa"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3198), null, "Moonshot v1 128k 文本模型", true, "Moonshot", false, "moonshot-v1-128k", null, 5.06m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("07cd9d8e-62b6-4b1c-847f-3bfe86ca0b9f"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3161), null, "GPT-3.5 Turbo 16k 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("09af83d4-627b-482e-8629-194048050bf7"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3237), null, "GLM 4 文本模型", true, "ChatGLM", false, "glm-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("09f145f1-69bf-42a6-91f3-ef99812cd6e9"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3221), null, "ChatGLM 标准文本模型", true, "ChatGLM", false, "chatglm_std", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("0cf3d41d-91b4-47a6-91e4-c9d23a1f7800"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3171), null, "GPT-4 0125 预览文本模型", true, "OpenAI", false, "gpt-4-0125-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1060026a-03bf-42d6-89cf-a42cd67342d3"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3212), null, "TTS 1 HD 1106 语音合成模型", true, "OpenAI", false, "tts-1-hd-1106", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("147ff81f-c887-4ade-9b65-8aaac0f3862c"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3220), null, "ChatGLM Pro 文本模型", true, "ChatGLM", false, "chatglm_pro", null, 0.7143m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1689c35a-4a4a-4b36-877b-a82337a897ed"), null, null, null, true, null, null, 5m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3229), null, "Claude 3 Opus 20240229 文本模型", true, "Claude", false, "claude-3-opus-20240229", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("16fb9275-75a4-4b04-9ca9-98d4eeb7f392"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3213), null, "Whisper 1 语音识别模型", true, "OpenAI", false, "whisper-1", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("1ba7dbb7-df1f-4712-bf43-20329702d7dd"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3175), null, "GPT-4 1106 视觉预览模型", true, "OpenAI", false, "gpt-4-1106-vision-preview", null, 10m, "128K", 1, "[\"\\u89C6\\u89C9\",\"\\u6587\\u672C\"]", null },
                    { new Guid("1dc3f3f0-bd3b-49f1-8ef6-cf18e0297870"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3181), null, "GPT-4 Turbo 文本模型", true, "OpenAI", false, "gpt-4-turbo", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1ed3d1bf-ae16-4b2d-ba17-77af61a4fd20"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3159), null, "GPT-3.5 Turbo 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0613", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1f88e433-3d74-478e-ad1f-681e3fadfa8e"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3238), null, "GLM 4v 文本模型", true, "ChatGLM", false, "glm-4v", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("26e0fe6e-8f00-4ca9-8e12-a0d8155ad189"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3178), null, "GPT-4 32k 0613 文本模型", true, "OpenAI", false, "gpt-4-32k-0613", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("2e21907e-040d-49ea-bea8-4c0bca3c518b"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3214), null, "Hunyuan Lite 文本模型", true, "Hunyuan", false, "hunyuan-lite", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("326da002-9952-4e22-a30c-f35fc129e3cc"), null, null, null, true, null, null, 4m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3197), null, "GPT-4o 2024-08-06 文本模型", true, "OpenAI", false, "gpt-4o-2024-08-06", null, 1.25m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("384e5bd9-bc99-4cd6-a4f4-e5f81e5be33f"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3210), null, "TTS 1 HD 语音合成模型", true, "OpenAI", false, "tts-1-hd", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("38a5ab77-036f-4cf4-8826-4f40841bb435"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3206), null, "Text Embedding 3 Large 嵌入模型", true, "OpenAI", false, "text-embedding-3-large", null, 0.13m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("3ea81547-0ea8-4b16-b8f2-968e0ab87384"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3206), null, "Text Embedding 3 Small 嵌入模型", true, "OpenAI", false, "text-embedding-3-small", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("45c8fd31-b2d1-4912-97b0-6083ca8286be"), null, null, null, true, null, null, 4m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3192), null, "ChatGPT 4o 最新文本模型", true, "OpenAI", false, "chatgpt-4o-latest", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("4bf1813f-0a66-4b4a-aa2d-c81cac02b91d"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3222), null, "ChatGLM Turbo 文本模型", true, "ChatGLM", false, "chatglm_turbo", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("4d799f91-781d-48f8-8d26-7d5c9c6ea74a"), null, null, null, true, null, null, 3m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3182), null, "Gemini Pro 文本模型", true, "Google", false, "gemini-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("50af2f81-68b6-4685-ab23-3d3a3b733b3d"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3201), null, "Text Babbage 001 文本模型", true, "OpenAI", false, "text-babbage-001", null, 0.25m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("52c39472-96da-43cc-bd1d-85bc3d8f638b"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3170), null, "GPT-4 文本模型", true, "OpenAI", false, "gpt-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("53102626-efdb-4008-8cb4-741904d09094"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3199), null, "Moonshot v1 8k 文本模型", true, "Moonshot", false, "moonshot-v1-8k", null, 1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("5c7a1119-e216-40bb-99e1-3b1a2b23a8be"), null, null, null, true, null, null, 2m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3215), null, "通用文本模型", true, "Spark", false, "general", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("5cb662b9-1c22-4fbb-8ed6-7c1cf2eba5a8"), null, null, null, true, null, null, 5m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3224), null, "Claude 3 Haiku 文本模型", true, "Claude", false, "claude-3-haiku", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("5eb5d921-e7b3-454f-8a45-b7c1535d5530"), null, null, null, true, null, null, 3m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3192), null, "GPT-4o 文本模型", true, "OpenAI", false, "gpt-4o", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("5f98d35e-ca5b-40c8-9644-48ec589defea"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3232), null, "Embedding 2 嵌入模型", true, "OpenAI", false, "embedding-2", null, 0.0355m, "", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("62f5ade9-6d90-4087-9715-6b948e89730c"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3160), null, "GPT-3.5 Turbo 1106 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-1106", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("63ea8004-e0ec-4a3a-9c9c-a36dcc3fa124"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3223), null, "Claude 2.0 文本模型", true, "Claude", false, "claude-2.0", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("64746208-7e31-4ac6-9d0f-c9c71c4d6f57"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3151), null, "GPT-3.5 Turbo 0125 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0125", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("66bea95e-76d7-4ffb-9c42-a7c7e489af4c"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3236), null, "GLM 3 Turbo 文本模型", true, "ChatGLM", false, "glm-3-turbo", null, 0.355m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("6d0402b4-a63f-4cf7-b41a-caf8eeb4ee4b"), null, null, null, true, null, null, 2m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3181), null, "Gemini 1.5 Pro 文本模型", true, "Google", false, "gemini-1.5-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("72d88a5e-a1e1-4d7c-8516-b2e8da2b2c7e"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3177), null, "GPT-4 32k 文本模型", true, "OpenAI", false, "gpt-4-32k", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("730f9616-15fa-4cc6-92d9-b88b520cfadd"), null, null, null, true, null, null, 3m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3185), null, "Gemini 1.5 Flash 文本模型", true, "Google", false, "gemini-1.5-flash", null, 0.2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("744a501d-42e7-4083-b8f0-c7b477896281"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3185), null, "GPT-4 Turbo 2024-04-09 文本模型", true, "OpenAI", false, "gpt-4-turbo-2024-04-09", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("7886e959-8b9c-4321-8ae4-e95c6d72ca1a"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3168), null, "GPT-3.5 Turbo 16k 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k-0613", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("83ddb6b7-fb63-456a-9625-c2e0a4b83672"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3204), null, "Text Davinci 003 文本模型", true, "OpenAI", false, "text-davinci-003", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("83fd25e2-c5e8-4b97-a2db-0ebc5a45cf2a"), null, null, null, true, null, null, 2m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3216), null, "通用文本模型 v3.5", true, "Spark", false, "generalv3.5", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("8bc43469-4a68-40cf-b8aa-dd801bdc14b9"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3230), null, "Claude Instant 1.2 文本模型", true, "Claude", false, "claude-instant-1.2", null, 0.4m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("8c50d2ef-3228-43ab-8ab6-476e6ee0a2a9"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3234), null, "Embedding BERT 512 v1 嵌入模型", true, "OpenAI", false, "embedding-bert-512-v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("8cd5436f-8e2b-48dc-a91e-f3b54c5f16b1"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3199), null, "Moonshot v1 32k 文本模型", true, "Moonshot", false, "moonshot-v1-32k", null, 2m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("8f798700-7e9d-4e24-81d5-5317e884356b"), null, null, null, true, null, null, 5m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3227), null, "Claude 3.5 Sonnet 20240620 文本模型", true, "Claude", false, "claude-3-5-sonnet-20240620", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("9b894d15-baa2-4db0-874f-eb749e66761a"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3186), null, "GPT-4 Turbo 预览文本模型", true, "OpenAI", false, "gpt-4-turbo-preview", null, 5m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("9c91614f-01af-4903-bfbf-b17359bbc8d3"), null, null, null, true, null, null, 5m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3228), null, "Claude 3 Sonnet 20240229 文本模型", true, "Claude", false, "claude-3-sonnet-20240229", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("9e0d699d-1705-4701-83ef-4d9eb169315f"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3208), null, "Text Embedding Ada 002 嵌入模型", true, "OpenAI", false, "text-embedding-ada-002", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a0560099-f0ea-42c5-a482-10f5fd68f0aa"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3235), null, "Embedding S1 v1 嵌入模型", true, null, false, "embedding_s1_v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("a3dfd9ab-1835-4fcd-affe-9f2de6e25377"), null, null, null, true, null, null, 4m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3196), null, "GPT-4o 2024-05-13 文本模型", true, "OpenAI", false, "gpt-4o-2024-05-13", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("a5670e89-67ab-4b25-beb5-0d05932ffaa7"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3191), null, "GPT-4 视觉预览模型", true, "OpenAI", false, "gpt-4-vision-preview", null, 10m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("a84853bb-958d-4a5c-a128-43ff677fe109"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3232), null, "DALL-E 3 图像生成模型", true, "OpenAI", false, "dall-e-3", null, 20m, null, 2, "[\"\\u56FE\\u7247\"]", null },
                    { new Guid("b1c474d0-bde7-4ffd-ba8a-648aaa4d345a"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3231), null, "DALL-E 2 图像生成模型", true, "OpenAI", false, "dall-e-2", null, 8m, null, 2, "[\"\\u56FE\\u7247\"]", null },
                    { new Guid("b43a661f-912d-4796-b07b-9575bee03c01"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3217), null, "ChatGLM Lite 文本模型", true, "ChatGLM", false, "chatglm_lite", null, 0.1429m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b79e730a-fe4f-41f0-ae35-3bc6b635ec1d"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3205), null, "Text Davinci Edit 001 文本模型", true, "OpenAI", false, "text-davinci-edit-001", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b8d4a4fb-14de-4783-9d9b-b26bd755564a"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3171), null, "GPT-4 0314 文本模型", true, "OpenAI", false, "gpt-4-0314", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("bc13e726-486c-40df-bcf4-05c0c7d273dc"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3179), null, "GPT-4 全部文本模型", true, "OpenAI", false, "gpt-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u8054\\u7F51\"]", null },
                    { new Guid("bccb2ceb-b0e4-459b-bc20-cce719eb7f36"), null, null, null, true, null, null, 5m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3225), null, "Claude 3 Haiku 20240307 文本模型", true, "Claude", false, "claude-3-haiku-20240307", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("c05823d8-4697-4446-a95f-647caf9efa8e"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3224), null, "Claude 2.1 文本模型", true, "Claude", false, "claude-2.1", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("c2973c1c-4c02-4dcc-9485-73dc64b51507"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3209), null, "TTS 1 1106 语音合成模型", true, "OpenAI", false, "tts-1-1106", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("c8abfaa0-fbd7-4d7e-a871-accce0af1f63"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3173), null, "GPT-4 1106 预览文本模型", true, "OpenAI", false, "gpt-4-1106-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("c8b66ff1-93ac-4335-9cee-f65abaa8afe5"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3201), null, "Text Curie 001 文本模型", true, "OpenAI", false, "text-curie-001", null, 1m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("d214bbff-f190-4064-8b3e-ff0337294fb1"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3177), null, "GPT-4 32k 0314 文本模型", true, "OpenAI", false, "gpt-4-32k-0314", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("d497ea2e-66e8-4cf8-ba7e-7b7125344502"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3158), null, "GPT-3.5 Turbo 0301 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0301", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("d5c67cb2-af24-4534-9eb1-723b2487882d"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3230), null, "Claude Instant 1 文本模型", true, "Claude", false, "claude-instant-1", null, 0.815m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("d689de96-a263-4499-97d0-7cd9ee7cc400"), null, null, null, true, null, null, 3m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3183), null, "Gemini Pro 视觉模型", true, "Google", false, "gemini-pro-vision", null, 2m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("db7a8222-b62b-4ff0-bee0-7557386861a9"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3222), null, "Claude 2 文本模型", true, "Claude", false, "claude-2", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("dbc4d4e6-f7b6-4532-96ae-7c85e7313a88"), null, null, null, true, null, null, 4m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3193), null, "GPT-4o Mini 文本模型", true, "OpenAI", false, "gpt-4o-mini", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("dd2ed225-1428-4575-af64-2d031cb3b17d"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3209), null, "TTS 1 语音合成模型", true, "OpenAI", false, "tts-1", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("dde1cd9c-9464-4af5-b2bb-a6faabd31418"), null, null, null, true, null, null, 2m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3215), null, "通用文本模型 v3", true, "Spark", false, "generalv3", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("df0e3a76-a30a-4de5-9b4b-8b966277361f"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3238), null, "GLM 4 全部文本模型", true, "ChatGLM", false, "glm-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("e8c5278e-af09-4792-8db5-27721f62982f"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3202), null, "Text Davinci 002 文本模型", true, "OpenAI", false, "text-davinci-002", null, 10m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ebee8cb5-059a-4af3-a677-5edc846cf216"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3173), null, "GPT-4 0613 文本模型", true, "OpenAI", false, "gpt-4-0613", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ec27d994-f4e2-42d1-a23d-a7859ee1e4a3"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(2789), null, "GPT-3.5 Turbo 文本模型", true, "OpenAI", false, "gpt-3.5-turbo", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ef207545-f6ab-48d9-834c-3652f4d99555"), null, null, null, true, null, null, null, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3169), null, "GPT-3.5 Turbo Instruct 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-instruct", null, 0.001m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("f10529c0-3e98-4a2a-bdea-bd68c010dfb1"), null, null, null, true, null, null, 3m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3217), null, "4.0 超级文本模型", true, "Spark", false, "4.0Ultra", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("fb4ef43a-8530-4df0-97ab-371e1fb32f6e"), null, null, null, true, null, null, 4m, new DateTime(2025, 3, 4, 2, 15, 4, 199, DateTimeKind.Local).AddTicks(3194), null, "GPT-4o Mini 2024-07-18 文本模型", true, "OpenAI", false, "gpt-4o-mini-2024-07-18", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null }
                });

            migrationBuilder.InsertData(
                table: "Tokens",
                columns: new[] { "Id", "AccessedTime", "CreatedAt", "Creator", "DeletedAt", "Disabled", "ExpiredTime", "IsDelete", "Key", "LimitModels", "Modifier", "Name", "RemainQuota", "UnlimitedExpired", "UnlimitedQuota", "UpdatedAt", "UsedQuota", "WhiteIpList" },
                values: new object[] { "e948a8e2-83b5-4ad6-aa00-ea28127fc1e0", null, new DateTime(2025, 3, 4, 2, 15, 4, 150, DateTimeKind.Local).AddTicks(5307), "f8856afe-18f5-4167-86d5-ee94e3879ca2", null, false, null, false, "sk-6gX2joc6fywfE79aAdFZC7sLolkHBxBeDOT1M6", "[]", null, "默认Token", 0L, true, true, null, 0L, "[]" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "Groups", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "f8856afe-18f5-4167-86d5-ee94e3879ca2", null, 0L, new DateTime(2025, 3, 4, 2, 15, 4, 148, DateTimeKind.Local).AddTicks(8338), null, null, "239573049@qq.com", "[\"admin\"]", false, false, null, "ab57db29ea8fd80efbc69ad18d54b2e0", "09ec79245f834ad18f5e40957aee17d2", 0L, 1000000000L, "admin", null, "admin" });
        }
    }
}
