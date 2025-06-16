using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Thor.Provider.PostgreSql.Thor
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
                keyValue: new Guid("02fe749d-fb1c-44d3-bcf5-70c01ebde4dd"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("069c18e1-fab5-4478-83c9-1b51a381a3b0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0821f8e1-3472-4644-aa43-633ac0ca384c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0c3d6e95-554a-46bf-bfa0-b636826a7408"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0d959c9f-1f20-45b3-9e78-a6e73a3fce3c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0f2b4c39-42e0-4218-94e8-f1c134549844"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0f373f85-2f66-4f3e-83fa-c351466aade7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0f760d1d-d1c4-46aa-9b28-2ad00d0940c6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0fc91d41-8183-4cde-abe2-26cbeaf5e435"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1695c1b2-7f7e-49b6-bcbd-bc5e69fa01c2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("16baa5cb-f725-4b59-a24f-d85cdd8a829c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1b5c8dc1-4683-42a2-8d31-1fba24b46e69"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("205ccb7f-86b9-4d75-a32d-c45b6d05a8e6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("222c0845-2286-415e-84a1-14dcf21e1f98"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("24b6d503-601e-4a20-b266-99be2893c988"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("257b43db-598a-4ccf-9974-ab4db80b2b08"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("36e8fa2c-9534-450d-8bca-f0b82a8a9725"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("3aa426ac-ab40-4d86-a5bb-b4a9037d1568"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("40201e08-96b8-4199-81f1-623ff50a4bd0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("42ca4448-2105-4ee1-aa9e-a985a3211366"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("432158ff-0ca7-48fa-82fd-04f6934d761a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("442395cc-7d95-4f2d-aaf6-99c9aaa49465"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("487948c4-c6f3-4497-bbaa-bca8f1c6a741"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("4a27ba7a-1b38-4449-8ffc-1f960674aae6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("4d9027f1-bcf8-4865-8b10-3a4a88560816"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("514cb636-4b3c-4974-9b42-8220e1ef6d4f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("53ce3fe8-eb30-47c2-ab29-7af75d73caa2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("559cbe3e-1557-432e-a564-72cb52a92c84"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("57115f6e-8a11-454f-9964-3a3cf1a2819f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5768f792-fff1-4aa7-bb47-da246e1f4e89"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5d8a7841-4905-419f-9e97-e45fcd3c189a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5f8692f2-43a5-4e29-b05a-2a0fb60f4c67"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6581e4be-9f37-45a6-82ac-9c311dd35378"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6762c6b6-669a-49ff-8633-012943243aa9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6790d690-e012-4047-adf7-da23264f131e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6ad59a64-410c-483d-8eda-42627369ef4f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6ed8cf46-db23-4c46-810d-8688553ff69c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6ee4e4dd-969f-4c5e-ae37-c8fc152cc8e9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("713ae685-f224-4729-9f56-c2574b1418c2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7181a045-2fdf-47f0-bd79-bf08d48e908f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7380550d-5adb-413c-9546-613d8f66c3be"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("76952b5e-fce9-464d-906f-913ae65bc492"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7aa9bde1-88be-45cf-879a-6e39f462bf43"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7d8b3e2f-da1e-4e26-aec2-68b177f1281b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("82cde53a-f8b9-412e-9649-667678ec648f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8314e562-5f77-4009-8faa-b88ad2d328b3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8683931a-033d-4ee1-9343-cf2277cebbe7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("88b71541-d8a7-47e8-aac9-9b86cd440da3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("88bb1a9c-3b90-437d-a6ed-1156b5d35605"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8deec91d-454f-467c-b851-95d1df5bac2b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("90092b4e-6fb3-45ff-8e5d-bdfc84cd873d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("941c6ce4-de6e-43d5-a9f0-4f9762edff7e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9a85b661-c80d-45ed-898a-17f2cc117b39"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9fde0911-80de-4a70-bf2c-ccb5d509500c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a04ac203-bf03-43a0-bf62-9b5d38a9a6e6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a4e67089-7b58-4a55-8271-5bb37354d080"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a563822f-e200-4ef1-a69a-b49683b0a540"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a625134e-95a0-4448-a3e0-b4ee59ce582e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b0fde7d3-971d-4f92-99b0-1b1d3c962f03"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b5f4fda6-b985-4492-896f-353a4e6c916a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c099f571-b505-4133-a3fc-6638b29405de"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c2a7d198-613a-46b2-a0ce-14daa163026b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ca9ab267-8104-4aee-9e55-11813cc10701"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ccb60337-73f1-4185-8012-149260a75190"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d0fce706-31b3-4e66-b873-cebb6aced50b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d130581d-3d55-412c-81d8-34410458874f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d3cddb9c-0f8d-4f1e-9e0c-75deab5a057d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d4988f3e-4645-4a22-a558-82f3a9caa2b3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("df093ad5-9073-45a9-a88f-e9cda6fb6df7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e23167c4-633a-4354-8090-1211152a708a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e3797af3-31a0-4175-9f90-c0e22dec8b67"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("eb2616da-35fd-495c-8ec7-8270f07f04ce"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("eb86249e-ff70-4166-bd6f-52b2e900089e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f72fc6ca-4f32-4bf4-833d-2904665bac39"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fb1d14a8-307d-4758-8605-b9d489329790"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fce23472-7161-4e08-a212-c830027db637"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fd3f1cdf-1804-4cbe-9cf6-ce7009fb4688"));

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    Pinned = table.Column<bool>(type: "boolean", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    ExpireTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Modifier = table.Column<string>(type: "text", nullable: true),
                    Creator = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ModelManagers",
                columns: new[] { "Id", "AudioCacheRate", "AudioOutputRate", "AudioPromptRate", "Available", "CacheHitRate", "CacheRate", "CompletionRate", "CreatedAt", "Creator", "Description", "Enable", "Extension", "Icon", "IsVersion2", "Model", "Modifier", "PromptRate", "QuotaMax", "QuotaType", "Tags", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0276dd97-8095-4a45-b868-aa0f288418fe"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7127), null, "Hunyuan Lite 文本模型", true, "{}", "Hunyuan", false, "hunyuan-lite", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("03a2769a-a594-4dd8-a970-15b276ecccfe"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7108), null, "Moonshot v1 8k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-8k", null, 1m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("03a46d7e-27ba-4762-9788-1da2aca75967"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7111), null, "Text Davinci 002 文本模型", true, "{}", "OpenAI", false, "text-davinci-002", null, 10m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("04631e67-2bd4-445f-919f-c4e80d30f82b"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7112), null, "Text Davinci 003 文本模型", true, "{}", "OpenAI", false, "text-davinci-003", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("048a191c-137f-4541-89a6-b0e4a91af23d"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7110), null, "Text Curie 001 文本模型", true, "{}", "OpenAI", false, "text-curie-001", null, 1m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("060f0a37-f8e2-4679-95fe-12b56d80f998"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7133), null, "通用文本模型 v3", true, "{}", "Spark", false, "generalv3", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("063ceaf6-5ee4-433a-bf1f-065627e66176"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7116), null, "Text Embedding 3 Large 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-3-large", null, 0.13m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("086c8ef5-67b2-48bd-91ef-905e42e6e5d6"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7101), null, "GPT-4o Mini 文本模型", true, "{}", "OpenAI", false, "gpt-4o-mini", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("0ddbb380-a835-4885-9d7c-558497419192"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7107), null, "Moonshot v1 32k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-32k", null, 2m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("110e18f2-10c9-453a-abd3-138c7bf52ecd"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7076), null, "GPT-4 0613 文本模型", true, "{}", "OpenAI", false, "gpt-4-0613", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("1353b36b-ed6a-4521-82db-fe3295865ff6"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7152), null, "Claude Instant 1 文本模型", true, "{}", "Claude", false, "claude-instant-1", null, 0.815m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("14bdec8c-1406-4c7b-97d1-df2f3c2c11ad"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7057), null, "GPT-3.5 Turbo 0301 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0301", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("19c83ce7-c55a-4e6a-9484-17730f779ba8"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7135), null, "4.0 超级文本模型", true, "{}", "Spark", false, "4.0Ultra", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("1f26381a-4d25-449c-bd2a-ee18efc17135"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7133), null, "通用文本模型", true, "{}", "Spark", false, "general", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("24d86cf3-d4c1-44ce-8a41-632a09f38e63"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7109), null, "Text Babbage 001 文本模型", true, "{}", "OpenAI", false, "text-babbage-001", null, 0.25m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("2762f24f-7061-4698-898c-81f390d4b513"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7119), null, "Text Embedding Ada 002 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-ada-002", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("2a949d6e-4d04-4464-ba77-2af1a2b4984e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7137), null, "ChatGLM Pro 文本模型", true, "{}", "ChatGLM", false, "chatglm_pro", null, 0.7143m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("2c63af81-2725-461c-bde0-a9180f8eb05a"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7115), null, "Text Davinci Edit 001 文本模型", true, "{}", "OpenAI", false, "text-davinci-edit-001", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("2f63fa6e-c1f3-4abe-8a7c-6d171dd22eb2"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7087), null, "Gemini Pro 文本模型", true, "{}", "Google", false, "gemini-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("3d4d5fe0-dd0e-4820-a5bd-973eec593f20"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7078), null, "GPT-4 32k 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("4134c33f-c4cd-46a9-bfb2-91cdb5cab7b5"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7144), null, "Claude 2.1 文本模型", true, "{}", "Claude", false, "claude-2.1", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("4a3b3a9c-a667-4286-bc60-c6b986828469"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(6787), null, "GPT-3.5 Turbo 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("4aab15bb-ffd9-4c60-88e3-5a85a62e6ec4"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7078), null, "GPT-4 1106 视觉预览模型", true, "{}", "OpenAI", false, "gpt-4-1106-vision-preview", null, 10m, "128K", 1, "[\"\\u89C6\\u89C9\",\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("4b5d0cc6-b1b6-4159-8e0a-f6c195e33c73"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7142), null, "Claude 2 文本模型", true, "{}", "Claude", false, "claude-2", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("4cbc2914-1d7c-421a-b1e7-8d3d5f644c55"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7055), null, "GPT-3.5 Turbo 0125 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0125", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("4d845b4e-7c99-49d3-a874-27d468b5907e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7098), null, "GPT-4 视觉预览模型", true, "{}", "OpenAI", false, "gpt-4-vision-preview", null, 10m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("50be52cb-c6fd-4aee-9394-4834bdaae8a1"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7105), null, "GPT-4o 2024-08-06 文本模型", true, "{}", "OpenAI", false, "gpt-4o-2024-08-06", null, 1.25m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("540dda43-b158-4f85-9b56-f55f4b6ba857"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7151), null, "Claude 3 Opus 20240229 文本模型", true, "{}", "Claude", false, "claude-3-opus-20240229", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("59acc495-0fcc-4816-9b04-58404a88da19"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7143), null, "Claude 2.0 文本模型", true, "{}", "Claude", false, "claude-2.0", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("5d0de8bd-7649-4405-9156-de1560f32ff0"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7103), null, "GPT-4o 2024-05-13 文本模型", true, "{}", "OpenAI", false, "gpt-4o-2024-05-13", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("5fdb0203-2647-4b83-85db-e01c0f786e01"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7122), null, "TTS 1 HD 语音合成模型", true, "{}", "OpenAI", false, "tts-1-hd", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("672be378-d247-4ffc-b7b0-ae5d99ec6d12"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7126), null, "Whisper 1 语音识别模型", true, "{}", "OpenAI", false, "whisper-1", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "stt", null },
                    { new Guid("700c38bc-ea29-41bc-84b0-926e501ea85e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7107), null, "Moonshot v1 128k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-128k", null, 5.06m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("7022c92c-cede-4283-b565-cfe44345fb2a"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7154), null, "DALL-E 2 图像生成模型", true, "{}", "OpenAI", false, "dall-e-2", null, 8000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("76ebe013-8ecd-4309-851a-8a69dfc956c3"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7100), null, "ChatGPT 4o 最新文本模型", true, "{}", "OpenAI", false, "chatgpt-4o-latest", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("7896ce4c-ab11-4ab4-8a4c-d8c6f8304037"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7155), null, "DALL-E 3 图像生成模型", true, "{}", "OpenAI", false, "dall-e-3", null, 20000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("78c90229-fae2-489f-b999-294383d2ee8e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7123), null, "TTS 1 HD 1106 语音合成模型", true, "{}", "OpenAI", false, "tts-1-hd-1106", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("7b8ad5c8-7a43-4507-a23b-533f76d070e7"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7083), null, "GPT-4 32k 0613 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k-0613", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("8102fd3a-eccb-46e0-a4e1-35b2ca4e0cbb"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7093), null, "GPT-4 Turbo 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo-preview", null, 5m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("875e2602-7c1f-4de6-a458-2211f1406304"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7147), null, "Claude 3.5 Sonnet 20240620 文本模型", true, "{}", "Claude", false, "claude-3-5-sonnet-20240620", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("88a330d9-31cc-4893-a6b0-0e34b57f0d73"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7134), null, "通用文本模型 v3.5", true, "{}", "Spark", false, "generalv3.5", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("8c3d8e56-4eb1-4eee-ac57-eb96a37a0f1a"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7156), null, "Embedding 2 嵌入模型", true, "{}", "OpenAI", false, "embedding-2", null, 0.0355m, "", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("902f43df-ab15-41e6-9c4a-ae7be1bd46a6"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7062), null, "GPT-3.5 Turbo 16k 0613 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-16k-0613", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("9179c744-e468-4067-b50f-9023b8524cc3"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7099), null, "GPT-4o 文本模型", true, "{}", "OpenAI", false, "gpt-4o", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("943c28fd-9692-474d-9fce-fabb1d21ca42"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7076), null, "GPT-4 1106 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-1106-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("962690d7-aad0-4993-b509-5cdb11f7f2ae"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7058), null, "GPT-3.5 Turbo 0613 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0613", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("96a2cd63-6890-4784-8e86-05331cbbbdb0"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7145), null, "Claude 3 Haiku 文本模型", true, "{}", "Claude", false, "claude-3-haiku", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("9880569c-76c6-46bf-8a91-825de8da61bd"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7161), null, "GLM 3 Turbo 文本模型", true, "{}", "ChatGLM", false, "glm-3-turbo", null, 0.355m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("9c150d7c-da34-4256-acd9-46681f145f98"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7159), null, "Embedding BERT 512 v1 嵌入模型", true, "{}", "OpenAI", false, "embedding-bert-512-v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("9f0ac1a2-2058-4f70-ac03-624f60a4708e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7117), null, "Text Embedding 3 Small 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-3-small", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("ae286606-9fe4-46cb-a9ee-dbefe002292f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7156), null, "GPT Image 图片生成模型", true, "{}", "OpenAI", false, "gpt-image-1", null, 50000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("b0edab4d-a6d7-4daf-9018-00ee36da67c9"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7164), null, "GLM 4v 文本模型", true, "{}", "ChatGLM", false, "glm-4v", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("b2f45aa3-4647-4fd1-b7cf-431abffaa40e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7061), null, "GPT-3.5 Turbo 16k 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-16k", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("b3464248-61f9-490e-ac33-fdcd596d2652"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7102), null, "GPT-4o Mini 2024-07-18 文本模型", true, "{}", "OpenAI", false, "gpt-4o-mini-2024-07-18", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("bb86bdd8-eca1-40c5-8707-695f1602b0f6"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7136), null, "ChatGLM Lite 文本模型", true, "{}", "ChatGLM", false, "chatglm_lite", null, 0.1429m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("bbd55770-23b0-405d-b353-4d2d1a7c83a0"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7121), null, "TTS 1 1106 语音合成模型", true, "{}", "OpenAI", false, "tts-1-1106", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("bcd77057-021b-44fc-adc5-9d6db8af4c9c"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7150), null, "Claude 3 Sonnet 20240229 文本模型", true, "{}", "Claude", false, "claude-3-sonnet-20240229", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("c0b8ede1-38a2-465a-b713-0e8dc1112949"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7074), null, "GPT-4 0314 文本模型", true, "{}", "OpenAI", false, "gpt-4-0314", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("c290ab8b-fcf5-483c-9e9d-bf9acc528635"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7085), null, "GPT-4 Turbo 文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("c6e26ec0-043f-4104-82bb-6efc3e1f1339"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7121), null, "TTS 1 语音合成模型", true, "{}", "OpenAI", false, "tts-1", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("c78f5062-065f-4025-baad-937d3e1fd6d5"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7090), null, "Gemini 1.5 Flash 文本模型", true, "{}", "Google", false, "gemini-1.5-flash", null, 0.2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("c90ff4e8-259d-4e0c-8205-8f9dadd36660"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7163), null, "GLM 4 文本模型", true, "{}", "ChatGLM", false, "glm-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("c9e0847f-f798-40ad-9c59-fb3d82ee37c3"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7140), null, "ChatGLM 标准文本模型", true, "{}", "ChatGLM", false, "chatglm_std", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("ce7acd9e-d660-4cd7-b950-99438ee4e52c"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7082), null, "GPT-4 32k 0314 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k-0314", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("cfb2137d-3646-4435-ad12-795deb005311"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7088), null, "Gemini Pro 视觉模型", true, "{}", "Google", false, "gemini-pro-vision", null, 2m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("d512ccd4-338f-48ff-a673-f70b804969a0"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7084), null, "GPT-4 全部文本模型", true, "{}", "OpenAI", false, "gpt-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u8054\\u7F51\"]", "chat", null },
                    { new Guid("db24331c-8176-4b88-b0e8-fa51a511134b"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7092), null, "GPT-4 Turbo 2024-04-09 文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo-2024-04-09", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("e3572588-ce05-412b-92c8-fdc0f45783b1"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7087), null, "Gemini 1.5 Pro 文本模型", true, "{}", "Google", false, "gemini-1.5-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("e7d4f358-c62d-46c0-bc8e-1b784135480f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7071), null, "GPT-3.5 Turbo Instruct 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-instruct", null, 0.001m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("e7f05467-82dd-47b9-87a7-d7dcae7c3166"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7160), null, "Embedding S1 v1 嵌入模型", true, "{}", null, false, "embedding_s1_v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("ed041e45-dea8-4bd7-8a95-f5b2e2b9f85e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7073), null, "GPT-4 0125 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-0125-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("f11fb4aa-ad04-4b11-85e2-990387e0a6b8"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7163), null, "GLM 4 全部文本模型", true, "{}", "ChatGLM", false, "glm-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("f1324fd0-776d-4b41-9526-34014f2970a3"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7153), null, "Claude Instant 1.2 文本模型", true, "{}", "Claude", false, "claude-instant-1.2", null, 0.4m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("f7598752-b278-4ec6-898f-6eb975b5a663"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7072), null, "GPT-4 文本模型", true, "{}", "OpenAI", false, "gpt-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("fa6ca869-1f31-483f-a653-09388b690721"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7146), null, "Claude 3 Haiku 20240307 文本模型", true, "{}", "Claude", false, "claude-3-haiku-20240307", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("fb3ff766-bff6-4d74-918d-3992a6c93213"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7060), null, "GPT-3.5 Turbo 1106 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-1106", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("fd5987d1-27c5-443a-86ff-a8c52dcc5c20"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 31, 17, DateTimeKind.Local).AddTicks(7141), null, "ChatGLM Turbo 文本模型", true, "{}", "ChatGLM", false, "chatglm_turbo", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null }
                });

            migrationBuilder.UpdateData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: "CA378C74-19E7-458A-918B-4DBB7AE1729D",
                columns: new[] { "CreatedAt", "Key" },
                values: new object[] { new DateTime(2025, 6, 17, 0, 34, 30, 976, DateTimeKind.Local).AddTicks(422), "sk-pNml6ZDAJIriN07CmgvutYqYptZiydMgInvQeY" });

            migrationBuilder.UpdateData(
                table: "UserGroups",
                keyColumn: "Id",
                keyValue: new Guid("ca378c74-19e7-458a-918b-4dbb7ae17291"),
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 0, 34, 30, 976, DateTimeKind.Local).AddTicks(8974));

            migrationBuilder.UpdateData(
                table: "UserGroups",
                keyColumn: "Id",
                keyValue: new Guid("ca378c74-19e7-458a-918b-4dbb7ae1729d"),
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 0, 34, 30, 976, DateTimeKind.Local).AddTicks(8599));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "CA378C74-19E7-458A-918B-4DBB7AE1729D",
                columns: new[] { "CreatedAt", "Password", "PasswordHas" },
                values: new object[] { new DateTime(2025, 6, 17, 0, 34, 30, 974, DateTimeKind.Local).AddTicks(4939), "b5fa33fbbae404b85ba0d16b9a10141a", "8712ef5ad2674958be8a7ffc8e47d225" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0276dd97-8095-4a45-b868-aa0f288418fe"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("03a2769a-a594-4dd8-a970-15b276ecccfe"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("03a46d7e-27ba-4762-9788-1da2aca75967"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("04631e67-2bd4-445f-919f-c4e80d30f82b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("048a191c-137f-4541-89a6-b0e4a91af23d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("060f0a37-f8e2-4679-95fe-12b56d80f998"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("063ceaf6-5ee4-433a-bf1f-065627e66176"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("086c8ef5-67b2-48bd-91ef-905e42e6e5d6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0ddbb380-a835-4885-9d7c-558497419192"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("110e18f2-10c9-453a-abd3-138c7bf52ecd"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1353b36b-ed6a-4521-82db-fe3295865ff6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("14bdec8c-1406-4c7b-97d1-df2f3c2c11ad"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("19c83ce7-c55a-4e6a-9484-17730f779ba8"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1f26381a-4d25-449c-bd2a-ee18efc17135"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("24d86cf3-d4c1-44ce-8a41-632a09f38e63"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2762f24f-7061-4698-898c-81f390d4b513"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2a949d6e-4d04-4464-ba77-2af1a2b4984e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2c63af81-2725-461c-bde0-a9180f8eb05a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2f63fa6e-c1f3-4abe-8a7c-6d171dd22eb2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("3d4d5fe0-dd0e-4820-a5bd-973eec593f20"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("4134c33f-c4cd-46a9-bfb2-91cdb5cab7b5"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("4a3b3a9c-a667-4286-bc60-c6b986828469"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("4aab15bb-ffd9-4c60-88e3-5a85a62e6ec4"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("4b5d0cc6-b1b6-4159-8e0a-f6c195e33c73"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("4cbc2914-1d7c-421a-b1e7-8d3d5f644c55"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("4d845b4e-7c99-49d3-a874-27d468b5907e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("50be52cb-c6fd-4aee-9394-4834bdaae8a1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("540dda43-b158-4f85-9b56-f55f4b6ba857"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("59acc495-0fcc-4816-9b04-58404a88da19"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5d0de8bd-7649-4405-9156-de1560f32ff0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5fdb0203-2647-4b83-85db-e01c0f786e01"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("672be378-d247-4ffc-b7b0-ae5d99ec6d12"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("700c38bc-ea29-41bc-84b0-926e501ea85e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7022c92c-cede-4283-b565-cfe44345fb2a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("76ebe013-8ecd-4309-851a-8a69dfc956c3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7896ce4c-ab11-4ab4-8a4c-d8c6f8304037"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("78c90229-fae2-489f-b999-294383d2ee8e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7b8ad5c8-7a43-4507-a23b-533f76d070e7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8102fd3a-eccb-46e0-a4e1-35b2ca4e0cbb"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("875e2602-7c1f-4de6-a458-2211f1406304"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("88a330d9-31cc-4893-a6b0-0e34b57f0d73"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8c3d8e56-4eb1-4eee-ac57-eb96a37a0f1a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("902f43df-ab15-41e6-9c4a-ae7be1bd46a6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9179c744-e468-4067-b50f-9023b8524cc3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("943c28fd-9692-474d-9fce-fabb1d21ca42"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("962690d7-aad0-4993-b509-5cdb11f7f2ae"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("96a2cd63-6890-4784-8e86-05331cbbbdb0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9880569c-76c6-46bf-8a91-825de8da61bd"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9c150d7c-da34-4256-acd9-46681f145f98"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9f0ac1a2-2058-4f70-ac03-624f60a4708e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ae286606-9fe4-46cb-a9ee-dbefe002292f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b0edab4d-a6d7-4daf-9018-00ee36da67c9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b2f45aa3-4647-4fd1-b7cf-431abffaa40e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b3464248-61f9-490e-ac33-fdcd596d2652"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("bb86bdd8-eca1-40c5-8707-695f1602b0f6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("bbd55770-23b0-405d-b353-4d2d1a7c83a0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("bcd77057-021b-44fc-adc5-9d6db8af4c9c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c0b8ede1-38a2-465a-b713-0e8dc1112949"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c290ab8b-fcf5-483c-9e9d-bf9acc528635"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c6e26ec0-043f-4104-82bb-6efc3e1f1339"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c78f5062-065f-4025-baad-937d3e1fd6d5"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c90ff4e8-259d-4e0c-8205-8f9dadd36660"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c9e0847f-f798-40ad-9c59-fb3d82ee37c3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ce7acd9e-d660-4cd7-b950-99438ee4e52c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("cfb2137d-3646-4435-ad12-795deb005311"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d512ccd4-338f-48ff-a673-f70b804969a0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("db24331c-8176-4b88-b0e8-fa51a511134b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e3572588-ce05-412b-92c8-fdc0f45783b1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e7d4f358-c62d-46c0-bc8e-1b784135480f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e7f05467-82dd-47b9-87a7-d7dcae7c3166"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ed041e45-dea8-4bd7-8a95-f5b2e2b9f85e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f11fb4aa-ad04-4b11-85e2-990387e0a6b8"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f1324fd0-776d-4b41-9526-34014f2970a3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f7598752-b278-4ec6-898f-6eb975b5a663"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fa6ca869-1f31-483f-a653-09388b690721"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fb3ff766-bff6-4d74-918d-3992a6c93213"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fd5987d1-27c5-443a-86ff-a8c52dcc5c20"));

            migrationBuilder.InsertData(
                table: "ModelManagers",
                columns: new[] { "Id", "AudioCacheRate", "AudioOutputRate", "AudioPromptRate", "Available", "CacheHitRate", "CacheRate", "CompletionRate", "CreatedAt", "Creator", "Description", "Enable", "Extension", "Icon", "IsVersion2", "Model", "Modifier", "PromptRate", "QuotaMax", "QuotaType", "Tags", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("02fe749d-fb1c-44d3-bcf5-70c01ebde4dd"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2102), null, "通用文本模型 v3", true, "{}", "Spark", false, "generalv3", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("069c18e1-fab5-4478-83c9-1b51a381a3b0"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2101), null, "Hunyuan Lite 文本模型", true, "{}", "Hunyuan", false, "hunyuan-lite", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("0821f8e1-3472-4644-aa43-633ac0ca384c"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2101), null, "通用文本模型", true, "{}", "Spark", false, "general", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("0c3d6e95-554a-46bf-bfa0-b636826a7408"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2054), null, "GPT-4 1106 视觉预览模型", true, "{}", "OpenAI", false, "gpt-4-1106-vision-preview", null, 10m, "128K", 1, "[\"\\u89C6\\u89C9\",\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("0d959c9f-1f20-45b3-9e78-a6e73a3fce3c"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2110), null, "Claude 3 Haiku 文本模型", true, "{}", "Claude", false, "claude-3-haiku", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("0f2b4c39-42e0-4218-94e8-f1c134549844"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2118), null, "GPT Image 图片生成模型", true, "{}", "OpenAI", false, "gpt-image-1", null, 50000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("0f373f85-2f66-4f3e-83fa-c351466aade7"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2111), null, "Claude 3 Haiku 20240307 文本模型", true, "{}", "Claude", false, "claude-3-haiku-20240307", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("0f760d1d-d1c4-46aa-9b28-2ad00d0940c6"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2107), null, "ChatGLM Turbo 文本模型", true, "{}", "ChatGLM", false, "chatglm_turbo", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("0fc91d41-8183-4cde-abe2-26cbeaf5e435"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2112), null, "Claude 3.5 Sonnet 20240620 文本模型", true, "{}", "Claude", false, "claude-3-5-sonnet-20240620", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("1695c1b2-7f7e-49b6-bcbd-bc5e69fa01c2"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2122), null, "GLM 3 Turbo 文本模型", true, "{}", "ChatGLM", false, "glm-3-turbo", null, 0.355m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("16baa5cb-f725-4b59-a24f-d85cdd8a829c"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2075), null, "GPT-4 视觉预览模型", true, "{}", "OpenAI", false, "gpt-4-vision-preview", null, 10m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("1b5c8dc1-4683-42a2-8d31-1fba24b46e69"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2069), null, "GPT-4 Turbo 2024-04-09 文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo-2024-04-09", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("205ccb7f-86b9-4d75-a32d-c45b6d05a8e6"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2061), null, "GPT-4 全部文本模型", true, "{}", "OpenAI", false, "gpt-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u8054\\u7F51\"]", "chat", null },
                    { new Guid("222c0845-2286-415e-84a1-14dcf21e1f98"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2080), null, "GPT-4o 2024-05-13 文本模型", true, "{}", "OpenAI", false, "gpt-4o-2024-05-13", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("24b6d503-601e-4a20-b266-99be2893c988"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2114), null, "Claude 3 Opus 20240229 文本模型", true, "{}", "Claude", false, "claude-3-opus-20240229", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("257b43db-598a-4ccf-9974-ab4db80b2b08"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2092), null, "Text Davinci Edit 001 文本模型", true, "{}", "OpenAI", false, "text-davinci-edit-001", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("36e8fa2c-9534-450d-8bca-f0b82a8a9725"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2118), null, "Embedding 2 嵌入模型", true, "{}", "OpenAI", false, "embedding-2", null, 0.0355m, "", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("3aa426ac-ab40-4d86-a5bb-b4a9037d1568"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2117), null, "DALL-E 3 图像生成模型", true, "{}", "OpenAI", false, "dall-e-3", null, 20000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("40201e08-96b8-4199-81f1-623ff50a4bd0"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2059), null, "GPT-4 32k 0314 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k-0314", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("42ca4448-2105-4ee1-aa9e-a985a3211366"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2062), null, "GPT-4 Turbo 文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("432158ff-0ca7-48fa-82fd-04f6934d761a"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2116), null, "DALL-E 2 图像生成模型", true, "{}", "OpenAI", false, "dall-e-2", null, 8000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("442395cc-7d95-4f2d-aaf6-99c9aaa49465"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2114), null, "Claude 3 Sonnet 20240229 文本模型", true, "{}", "Claude", false, "claude-3-sonnet-20240229", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("487948c4-c6f3-4497-bbaa-bca8f1c6a741"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2123), null, "GLM 4 全部文本模型", true, "{}", "ChatGLM", false, "glm-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("4a27ba7a-1b38-4449-8ffc-1f960674aae6"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2085), null, "Moonshot v1 8k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-8k", null, 1m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("4d9027f1-bcf8-4865-8b10-3a4a88560816"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2048), null, "GPT-3.5 Turbo Instruct 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-instruct", null, 0.001m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("514cb636-4b3c-4974-9b42-8220e1ef6d4f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2104), null, "ChatGLM Lite 文本模型", true, "{}", "ChatGLM", false, "chatglm_lite", null, 0.1429m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("53ce3fe8-eb30-47c2-ab29-7af75d73caa2"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2084), null, "Moonshot v1 32k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-32k", null, 2m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("559cbe3e-1557-432e-a564-72cb52a92c84"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2087), null, "Text Davinci 002 文本模型", true, "{}", "OpenAI", false, "text-davinci-002", null, 10m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("57115f6e-8a11-454f-9964-3a3cf1a2819f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2088), null, "Text Davinci 003 文本模型", true, "{}", "OpenAI", false, "text-davinci-003", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("5768f792-fff1-4aa7-bb47-da246e1f4e89"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2097), null, "TTS 1 HD 语音合成模型", true, "{}", "OpenAI", false, "tts-1-hd", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("5d8a7841-4905-419f-9e97-e45fcd3c189a"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2096), null, "TTS 1 语音合成模型", true, "{}", "OpenAI", false, "tts-1", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("5f8692f2-43a5-4e29-b05a-2a0fb60f4c67"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2037), null, "GPT-3.5 Turbo 16k 0613 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-16k-0613", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("6581e4be-9f37-45a6-82ac-9c311dd35378"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2107), null, "ChatGLM 标准文本模型", true, "{}", "ChatGLM", false, "chatglm_std", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("6762c6b6-669a-49ff-8633-012943243aa9"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2051), null, "GPT-4 0314 文本模型", true, "{}", "OpenAI", false, "gpt-4-0314", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("6790d690-e012-4047-adf7-da23264f131e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2115), null, "Claude Instant 1 文本模型", true, "{}", "Claude", false, "claude-instant-1", null, 0.815m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("6ad59a64-410c-483d-8eda-42627369ef4f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2085), null, "Text Babbage 001 文本模型", true, "{}", "OpenAI", false, "text-babbage-001", null, 0.25m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("6ed8cf46-db23-4c46-810d-8688553ff69c"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2105), null, "ChatGLM Pro 文本模型", true, "{}", "ChatGLM", false, "chatglm_pro", null, 0.7143m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("6ee4e4dd-969f-4c5e-ae37-c8fc152cc8e9"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2100), null, "Whisper 1 语音识别模型", true, "{}", "OpenAI", false, "whisper-1", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "stt", null },
                    { new Guid("713ae685-f224-4729-9f56-c2574b1418c2"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2064), null, "Gemini 1.5 Pro 文本模型", true, "{}", "Google", false, "gemini-1.5-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("7181a045-2fdf-47f0-bd79-bf08d48e908f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(1834), null, "GPT-3.5 Turbo 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("7380550d-5adb-413c-9546-613d8f66c3be"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2052), null, "GPT-4 0613 文本模型", true, "{}", "OpenAI", false, "gpt-4-0613", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("76952b5e-fce9-464d-906f-913ae65bc492"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2078), null, "GPT-4o Mini 2024-07-18 文本模型", true, "{}", "OpenAI", false, "gpt-4o-mini-2024-07-18", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("7aa9bde1-88be-45cf-879a-6e39f462bf43"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2116), null, "Claude Instant 1.2 文本模型", true, "{}", "Claude", false, "claude-instant-1.2", null, 0.4m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("7d8b3e2f-da1e-4e26-aec2-68b177f1281b"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2095), null, "Text Embedding Ada 002 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-ada-002", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("82cde53a-f8b9-412e-9649-667678ec648f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2124), null, "GLM 4v 文本模型", true, "{}", "ChatGLM", false, "glm-4v", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("8314e562-5f77-4009-8faa-b88ad2d328b3"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2065), null, "Gemini Pro 文本模型", true, "{}", "Google", false, "gemini-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("8683931a-033d-4ee1-9343-cf2277cebbe7"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2071), null, "GPT-4 Turbo 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo-preview", null, 5m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("88b71541-d8a7-47e8-aac9-9b86cd440da3"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2121), null, "Embedding S1 v1 嵌入模型", true, "{}", null, false, "embedding_s1_v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("88bb1a9c-3b90-437d-a6ed-1156b5d35605"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2060), null, "GPT-4 32k 0613 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k-0613", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("8deec91d-454f-467c-b851-95d1df5bac2b"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2066), null, "Gemini 1.5 Flash 文本模型", true, "{}", "Google", false, "gemini-1.5-flash", null, 0.2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("90092b4e-6fb3-45ff-8e5d-bdfc84cd873d"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2076), null, "GPT-4o 文本模型", true, "{}", "OpenAI", false, "gpt-4o", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("941c6ce4-de6e-43d5-a9f0-4f9762edff7e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2086), null, "Text Curie 001 文本模型", true, "{}", "OpenAI", false, "text-curie-001", null, 1m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("9a85b661-c80d-45ed-898a-17f2cc117b39"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2108), null, "Claude 2 文本模型", true, "{}", "Claude", false, "claude-2", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("9fde0911-80de-4a70-bf2c-ccb5d509500c"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2036), null, "GPT-3.5 Turbo 16k 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-16k", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("a04ac203-bf03-43a0-bf62-9b5d38a9a6e6"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2033), null, "GPT-3.5 Turbo 0301 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0301", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("a4e67089-7b58-4a55-8271-5bb37354d080"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2077), null, "ChatGPT 4o 最新文本模型", true, "{}", "OpenAI", false, "chatgpt-4o-latest", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("a563822f-e200-4ef1-a69a-b49683b0a540"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2053), null, "GPT-4 1106 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-1106-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("a625134e-95a0-4448-a3e0-b4ee59ce582e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2110), null, "Claude 2.1 文本模型", true, "{}", "Claude", false, "claude-2.1", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("b0fde7d3-971d-4f92-99b0-1b1d3c962f03"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2065), null, "Gemini Pro 视觉模型", true, "{}", "Google", false, "gemini-pro-vision", null, 2m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("b5f4fda6-b985-4492-896f-353a4e6c916a"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2103), null, "通用文本模型 v3.5", true, "{}", "Spark", false, "generalv3.5", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("c099f571-b505-4133-a3fc-6638b29405de"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2098), null, "TTS 1 HD 1106 语音合成模型", true, "{}", "OpenAI", false, "tts-1-hd-1106", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("c2a7d198-613a-46b2-a0ce-14daa163026b"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2082), null, "GPT-4o 2024-08-06 文本模型", true, "{}", "OpenAI", false, "gpt-4o-2024-08-06", null, 1.25m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("ca9ab267-8104-4aee-9e55-11813cc10701"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2078), null, "GPT-4o Mini 文本模型", true, "{}", "OpenAI", false, "gpt-4o-mini", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("ccb60337-73f1-4185-8012-149260a75190"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2096), null, "TTS 1 1106 语音合成模型", true, "{}", "OpenAI", false, "tts-1-1106", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("d0fce706-31b3-4e66-b873-cebb6aced50b"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2050), null, "GPT-4 0125 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-0125-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("d130581d-3d55-412c-81d8-34410458874f"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2103), null, "4.0 超级文本模型", true, "{}", "Spark", false, "4.0Ultra", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("d3cddb9c-0f8d-4f1e-9e0c-75deab5a057d"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2035), null, "GPT-3.5 Turbo 1106 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-1106", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("d4988f3e-4645-4a22-a558-82f3a9caa2b3"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2094), null, "Text Embedding 3 Small 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-3-small", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("df093ad5-9073-45a9-a88f-e9cda6fb6df7"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2034), null, "GPT-3.5 Turbo 0613 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0613", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("e23167c4-633a-4354-8090-1211152a708a"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2121), null, "Embedding BERT 512 v1 嵌入模型", true, "{}", "OpenAI", false, "embedding-bert-512-v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("e3797af3-31a0-4175-9f90-c0e22dec8b67"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2109), null, "Claude 2.0 文本模型", true, "{}", "Claude", false, "claude-2.0", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("eb2616da-35fd-495c-8ec7-8270f07f04ce"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2083), null, "Moonshot v1 128k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-128k", null, 5.06m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("eb86249e-ff70-4166-bd6f-52b2e900089e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2123), null, "GLM 4 文本模型", true, "{}", "ChatGLM", false, "glm-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("f72fc6ca-4f32-4bf4-833d-2904665bac39"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2031), null, "GPT-3.5 Turbo 0125 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0125", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("fb1d14a8-307d-4758-8605-b9d489329790"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2049), null, "GPT-4 文本模型", true, "{}", "OpenAI", false, "gpt-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("fce23472-7161-4e08-a212-c830027db637"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2055), null, "GPT-4 32k 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("fd3f1cdf-1804-4cbe-9cf6-ce7009fb4688"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 50, 622, DateTimeKind.Local).AddTicks(2093), null, "Text Embedding 3 Large 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-3-large", null, 0.13m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null }
                });

            migrationBuilder.UpdateData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: "CA378C74-19E7-458A-918B-4DBB7AE1729D",
                columns: new[] { "CreatedAt", "Key" },
                values: new object[] { new DateTime(2025, 6, 16, 23, 28, 50, 585, DateTimeKind.Local).AddTicks(6465), "sk-mXINTo74ZbB7c7mdgVIRyhTcuHzMncuOOta3Ko" });

            migrationBuilder.UpdateData(
                table: "UserGroups",
                keyColumn: "Id",
                keyValue: new Guid("ca378c74-19e7-458a-918b-4dbb7ae17291"),
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 23, 28, 50, 586, DateTimeKind.Local).AddTicks(4494));

            migrationBuilder.UpdateData(
                table: "UserGroups",
                keyColumn: "Id",
                keyValue: new Guid("ca378c74-19e7-458a-918b-4dbb7ae1729d"),
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 23, 28, 50, 586, DateTimeKind.Local).AddTicks(4148));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "CA378C74-19E7-458A-918B-4DBB7AE1729D",
                columns: new[] { "CreatedAt", "Password", "PasswordHas" },
                values: new object[] { new DateTime(2025, 6, 16, 23, 28, 50, 583, DateTimeKind.Local).AddTicks(3571), "59b164d6f4d7973759f03ce883719d75", "458bdb1704454a598c2b0d08efc7f069" });
        }
    }
}
