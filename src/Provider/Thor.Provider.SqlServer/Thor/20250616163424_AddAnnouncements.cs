﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Thor.Provider.SqlServer.Thor
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
                keyValue: new Guid("01eeedeb-1c80-449d-9a77-76e975f9ff78"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("05e86279-444b-4f77-a950-8ac669eb40b5"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("07f81b87-75f8-4550-b5cb-db0b8047a167"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("09ad340e-2a52-4be4-8b7d-cd7e66d41cfe"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0b026a6b-7ee0-4fb1-89ab-d8662de582f1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("102f0f4b-5b3a-493a-a03a-b04502b3c5b9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("11df0acb-be16-4c11-8eec-6518dc6c85d9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("138cc648-0be8-45d8-b55b-c5c2bc6f35aa"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("146c899e-a8c5-4a31-88c6-fc7058a3ab80"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("179825c7-3c6d-4485-8b86-a541d56956de"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1816e3da-e687-44eb-a847-601f56b8f0b8"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("18a3de73-57ed-4a36-8581-d63e4c1d296b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1e1269f3-231c-4418-b0d7-c3f5b9c78560"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1fe4c9c5-8457-4cfc-894c-e599a170ec3e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("206de9b5-e045-462c-9e39-1ffe405ffc0a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("209b8751-1a3b-435c-8963-800eb39e014c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("27956de8-b97b-4b93-99b1-b6066ee7b0be"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2d681dac-c2fe-4809-a162-8ee6f47c5089"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2e7b0cc8-4d6c-4de6-b0c7-4589aac92ca8"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("33156f30-cab2-4484-8331-0c3a332fb9f3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("341e094a-c67e-47bf-a6c1-0ee946b5fe59"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("371718a5-6d1e-49c4-845b-ec7017f145b6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("3d801782-7daa-4b70-8000-fd8b8df8b0f9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("409fbaf8-246c-4de6-974d-8192c9804fdb"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("41d9fd6b-e16a-4fb0-bd8f-9129e638baa7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("45273cda-ac7e-442a-9896-822eb69d2ace"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("460fc313-1f21-49a1-86f5-46b053aa2f60"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("478e8e77-979b-444d-ad65-3d49d67cfefc"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("531a101d-264f-4da4-bcfa-3221316e6547"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("53eaf2c8-0281-4eb9-979f-3d60c4cac542"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("571fa230-0ad9-4d19-b28b-653a5441de2b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("58a6f6b4-8a63-4330-85c9-d96248dfffcf"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("58b25505-40c2-4b3a-b344-255fa2e9bf15"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5efffaeb-0d78-4824-9b9b-dfa07b14613d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6c63980c-6529-4310-8d69-7ec231a614a2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("71800c7e-1913-476c-9e7c-13bf7311b48d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("72afeb42-5fa3-44c2-86b2-5e5db28a7365"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("74a1158e-3133-4c59-b11c-15f80c061e3f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7b121482-bdf3-4577-b61e-1ec6b92b6e79"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("87da8ea4-8c44-47b5-a5e6-dfb929a685b6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("892856e7-8dea-407f-93b9-02c9728b2f8c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8fd587d1-979c-499d-a6fd-ab10b2b728f9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9009bc1f-7acb-47e2-83b5-90dfa57e51bf"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9318b593-610c-485a-9644-a3050628eeb0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("986794d7-354c-47cd-bb00-23c9797ca67f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9a9c08db-2157-482c-8b29-f086152f6514"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a27d7691-2370-4a19-ab01-ae0149c4ce9c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a4e242b7-c52f-4ec5-b4ed-89ccb817141d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("acd9fc8b-03c9-4c20-a0df-430855d6b56f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ad0fe124-0e1e-4603-ab8b-3c04cd5b93d1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b000b042-ee25-4d21-b131-230e7f67a9e1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b0401c18-b7f2-44be-a508-aad885ab7520"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b1deed19-33b3-4ddb-b7f6-51c9cfc21cd1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b75e4c89-f8c6-4582-aef1-771adbf6197e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b7c2569d-d314-4217-9c2b-534f058ca12d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("bd6e97f3-d817-426c-9d73-aa33d0a49e89"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c20dcd9c-9248-4398-9f10-96eaf43713cd"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c3104d83-0107-4921-92f2-d1b1337bff69"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c38f933e-8f76-41e7-8a0a-ca4fb96e42a6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c3b94f12-33c4-455e-94c0-5bc570ce817e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c485d60e-40b0-4a99-8bc9-98d72902bd43"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("cd99fdf8-07bd-4964-9cce-f43d2e4f0043"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("cdc637ac-3d66-4d5a-adce-2c4d731020f6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d35543eb-7642-4047-adac-318dac91a135"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("de5db86f-6fae-4cb3-b17f-883f2f6a6ce2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e176a06d-5cd1-43c5-aad8-318076fe6694"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e2d21df5-7794-461d-be40-ceaeee1cb325"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e32f3311-2d76-482c-9b53-02073a5a9f8a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e83865f6-2db0-4578-bae2-fbc0c69f4477"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ecc9a3b4-e306-4504-973c-c3001f654197"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f632dab4-b8bb-49b9-aa81-88cb88a41f7f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f89dd699-72d6-49c7-b677-d5462be079cd"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("faf2b433-52ce-4796-b194-51bd9e3bda16"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fbba0c9b-1c58-44e9-93d3-024f0dde92cf"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fcf8d858-a576-4c5f-8317-d71d2aede17e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fda625d9-ee32-4c7e-b93c-bbc490f28c89"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ffc823b7-e912-4238-afa1-60b728ebd773"));

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Pinned = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ExpireTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Creator = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    { new Guid("00f7b0e9-41d8-450c-9c60-8ae5bc2faf32"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3285), null, "GPT-4o Mini 2024-07-18 文本模型", true, "{}", "OpenAI", false, "gpt-4o-mini-2024-07-18", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("01af2839-b8a1-49b3-811a-5e102baa995d"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3235), null, "GPT-3.5 Turbo 0613 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0613", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("05c0674d-4c8d-413c-a03a-a313ba4e36c5"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3335), null, "Embedding 2 嵌入模型", true, "{}", "OpenAI", false, "embedding-2", null, 0.0355m, "", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("0764b093-22cb-41f7-abea-990007c15fdd"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3339), null, "GLM 4 文本模型", true, "{}", "ChatGLM", false, "glm-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("0895255f-4280-4acc-b17a-6748b62d316a"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3263), null, "GPT-4 全部文本模型", true, "{}", "OpenAI", false, "gpt-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u8054\\u7F51\"]", "chat", null },
                    { new Guid("0e54dc38-37ed-4e38-9893-7ee0be00f4ce"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3309), null, "Whisper 1 语音识别模型", true, "{}", "OpenAI", false, "whisper-1", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "stt", null },
                    { new Guid("117a735f-dcc4-4007-9ed1-bbb445754421"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3281), null, "GPT-4o 文本模型", true, "{}", "OpenAI", false, "gpt-4o", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("16766eb1-96c6-4732-90cd-4c6673ff0554"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3309), null, "Hunyuan Lite 文本模型", true, "{}", "Hunyuan", false, "hunyuan-lite", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("17113c7d-30ea-473f-bc8f-089f0e85f8ed"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3269), null, "Gemini 1.5 Flash 文本模型", true, "{}", "Google", false, "gemini-1.5-flash", null, 0.2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("1c01ca85-050f-4410-bc9f-11eaaf8936d0"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3255), null, "GPT-4 1106 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-1106-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("1c83c5cc-cde7-4785-a9df-bcd3cf7d9988"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3316), null, "通用文本模型", true, "{}", "Spark", false, "general", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("1caef44d-8405-41de-8d48-fd292d09db99"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3280), null, "GPT-4 视觉预览模型", true, "{}", "OpenAI", false, "gpt-4-vision-preview", null, 10m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("20cc38db-3808-483a-8b9d-7aa922dd3a06"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3273), null, "GPT-4 Turbo 2024-04-09 文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo-2024-04-09", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("238ef6d3-5f14-4063-8c52-3099e1ca63bf"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3305), null, "TTS 1 1106 语音合成模型", true, "{}", "OpenAI", false, "tts-1-1106", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("256c445d-130a-4e11-85d4-4754f2b0d78c"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3334), null, "DALL-E 3 图像生成模型", true, "{}", "OpenAI", false, "dall-e-3", null, 20000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("27559ded-3730-4a2f-95e7-4ffb37fe4bbc"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3338), null, "GLM 3 Turbo 文本模型", true, "{}", "ChatGLM", false, "glm-3-turbo", null, 0.355m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("2b709dd6-34b8-4cad-8414-30d2b3a11c39"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3330), null, "Claude 3 Sonnet 20240229 文本模型", true, "{}", "Claude", false, "claude-3-sonnet-20240229", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("2d0ad485-f3b8-4172-a255-02e3610d9bab"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3339), null, "GLM 4 全部文本模型", true, "{}", "ChatGLM", false, "glm-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("33e99e88-46b6-49aa-8b81-910c259f246d"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3294), null, "Text Babbage 001 文本模型", true, "{}", "OpenAI", false, "text-babbage-001", null, 0.25m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("38b51a27-b304-4088-bea7-a03f76e8f3fb"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3236), null, "GPT-3.5 Turbo 16k 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-16k", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("3b1e56ad-d2f8-499f-b328-201532eaf343"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3320), null, "ChatGLM Pro 文本模型", true, "{}", "ChatGLM", false, "chatglm_pro", null, 0.7143m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("43eaf784-ae62-4ad6-8c9b-6fc5265b8dbf"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3296), null, "Text Davinci 003 文本模型", true, "{}", "OpenAI", false, "text-davinci-003", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("48abe2a1-bf67-4c45-8a01-50b1b7739cd7"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3340), null, "GLM 4v 文本模型", true, "{}", "ChatGLM", false, "glm-4v", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("4d489977-aa4f-48f8-b652-3b16d08be83d"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3292), null, "Moonshot v1 32k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-32k", null, 2m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("51b4a390-d82d-4a79-b5e0-65fb3da5b3aa"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3333), null, "DALL-E 2 图像生成模型", true, "{}", "OpenAI", false, "dall-e-2", null, 8000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("5260a718-9152-4a64-a1f5-98da0ab48ebf"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3295), null, "Text Davinci 002 文本模型", true, "{}", "OpenAI", false, "text-davinci-002", null, 10m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("5a86ee49-da81-45a9-840b-3c6557d09d74"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3298), null, "Text Davinci Edit 001 文本模型", true, "{}", "OpenAI", false, "text-davinci-edit-001", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("5b935cf3-2aaa-4fab-88a3-afc54ce7cb2d"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3324), null, "Claude 2 文本模型", true, "{}", "Claude", false, "claude-2", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("617a4ff4-c582-4ca3-b46e-1f713d901964"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3331), null, "Claude Instant 1 文本模型", true, "{}", "Claude", false, "claude-instant-1", null, 0.815m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("65b4cf71-7e31-493b-870a-9930cacaf38a"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3323), null, "ChatGLM 标准文本模型", true, "{}", "ChatGLM", false, "chatglm_std", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("660e22c6-a359-4a14-9d71-cbde03286f67"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3250), null, "GPT-4 文本模型", true, "{}", "OpenAI", false, "gpt-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("757f14b8-2da5-4009-9a55-6f7d2e7dc060"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3283), null, "GPT-4o Mini 文本模型", true, "{}", "OpenAI", false, "gpt-4o-mini", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("7d09f0d7-f6f8-48f3-b4fe-76a9ce0212c9"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3331), null, "Claude 3 Opus 20240229 文本模型", true, "{}", "Claude", false, "claude-3-opus-20240229", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("7ebcc1c8-ba73-4307-9c9c-a455c20ca2cb"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3266), null, "Gemini 1.5 Pro 文本模型", true, "{}", "Google", false, "gemini-1.5-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("88612241-2eea-42e9-ad5e-4cc6c36c6c37"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3300), null, "Text Embedding 3 Small 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-3-small", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("88c5dd5f-2107-47bb-a1c6-6b179d2ba05d"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3261), null, "GPT-4 32k 0314 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k-0314", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("8a69c08f-d2ce-41c0-8c06-428fa60eb187"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3249), null, "GPT-3.5 Turbo Instruct 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-instruct", null, 0.001m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("8ad35a76-8e9b-43b8-a110-d63b0a9e669c"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3238), null, "GPT-3.5 Turbo 16k 0613 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-16k-0613", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("8bb3838c-5dca-4ca8-980d-87e1f6db714f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3257), null, "GPT-4 32k 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("8e094a75-2dac-4e1b-93bf-33982a52c170"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3325), null, "Claude 2.0 文本模型", true, "{}", "Claude", false, "claude-2.0", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("938b3193-fe3c-4346-9799-79f2ca81bf12"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3306), null, "TTS 1 HD 语音合成模型", true, "{}", "OpenAI", false, "tts-1-hd", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("954de913-fe77-4472-ba78-1c3d214a16d5"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3267), null, "Gemini Pro 文本模型", true, "{}", "Google", false, "gemini-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("96cfd1e6-3609-4c26-a7e8-a2054a7de326"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3318), null, "4.0 超级文本模型", true, "{}", "Spark", false, "4.0Ultra", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("9853563d-39aa-4989-bfca-d3ac239ced4b"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3325), null, "Claude 2.1 文本模型", true, "{}", "Claude", false, "claude-2.1", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("9cffc495-547a-41bb-9964-46712a118993"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3231), null, "GPT-3.5 Turbo 0125 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0125", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("a0191355-03e3-4767-8f9f-2451fb5bcac7"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3290), null, "Moonshot v1 128k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-128k", null, 5.06m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("a491c4b5-f938-4549-a0a4-651cf4eb7f32"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3264), null, "GPT-4 Turbo 文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("a536b818-0950-4deb-8269-5cd398ef59c5"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3318), null, "通用文本模型 v3.5", true, "{}", "Spark", false, "generalv3.5", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("a62c5e7e-195f-40f4-9b6a-405b890507aa"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3256), null, "GPT-4 1106 视觉预览模型", true, "{}", "OpenAI", false, "gpt-4-1106-vision-preview", null, 10m, "128K", 1, "[\"\\u89C6\\u89C9\",\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("a906437f-9f57-477d-8c13-a71ea174db64"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3233), null, "GPT-3.5 Turbo 0301 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0301", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("ac63ff46-c4f6-416c-ac2c-fbef11290d14"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3252), null, "GPT-4 0125 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-0125-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("aec66ecc-599b-4c35-b842-98c4e1763fa4"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3262), null, "GPT-4 32k 0613 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k-0613", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("aef46351-e009-40ff-8acb-6b4e6baca392"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3295), null, "Text Curie 001 文本模型", true, "{}", "OpenAI", false, "text-curie-001", null, 1m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("af4dc6d6-0ec9-4148-beab-9590416f27ed"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3268), null, "Gemini Pro 视觉模型", true, "{}", "Google", false, "gemini-pro-vision", null, 2m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("b9e2eb75-8ed1-4e14-a29d-f5d64d87c2ac"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3274), null, "GPT-4 Turbo 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo-preview", null, 5m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("bc32ece1-fa27-4e3e-b715-8e14512dc457"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3307), null, "TTS 1 HD 1106 语音合成模型", true, "{}", "OpenAI", false, "tts-1-hd-1106", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("bc9fab50-fae7-45fe-9082-6db8f1a60733"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3323), null, "ChatGLM Turbo 文本模型", true, "{}", "ChatGLM", false, "chatglm_turbo", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("bed23d61-9526-48ba-95d4-8e2346d12520"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3302), null, "TTS 1 语音合成模型", true, "{}", "OpenAI", false, "tts-1", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("bf1eb103-f00e-48c0-a1db-2b2d2f203390"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3301), null, "Text Embedding Ada 002 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-ada-002", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("c188da00-fd06-44ed-b1f2-2a2b0227b45a"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3293), null, "Moonshot v1 8k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-8k", null, 1m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("c39e26cf-777d-4f8a-822c-f21a68ef483b"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3254), null, "GPT-4 0613 文本模型", true, "{}", "OpenAI", false, "gpt-4-0613", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("cb2e52ca-a4a2-4f4a-90bb-b687570e1379"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3334), null, "GPT Image 图片生成模型", true, "{}", "OpenAI", false, "gpt-image-1", null, 50000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("cc6b4817-8afb-4f7f-ba59-e3211b3c7166"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3327), null, "Claude 3 Haiku 文本模型", true, "{}", "Claude", false, "claude-3-haiku", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("ccbd58de-92a7-4476-a5d8-649f04b3f761"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3332), null, "Claude Instant 1.2 文本模型", true, "{}", "Claude", false, "claude-instant-1.2", null, 0.4m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("d405e652-a91e-4e69-97f3-09f1fd939fbf"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3253), null, "GPT-4 0314 文本模型", true, "{}", "OpenAI", false, "gpt-4-0314", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("d6684434-295f-48f5-ac07-7f56e1b77f4d"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3319), null, "ChatGLM Lite 文本模型", true, "{}", "ChatGLM", false, "chatglm_lite", null, 0.1429m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("e81d9a26-506b-4ae3-b8aa-13456da65bd2"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3026), null, "GPT-3.5 Turbo 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("eb3e033c-b97d-47bb-93e1-b20924df860e"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3282), null, "ChatGPT 4o 最新文本模型", true, "{}", "OpenAI", false, "chatgpt-4o-latest", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("ebb313a8-4b20-466d-90d4-355980b5fd60"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3289), null, "GPT-4o 2024-08-06 文本模型", true, "{}", "OpenAI", false, "gpt-4o-2024-08-06", null, 1.25m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("ece312b6-fb48-4553-8fdc-a3f39e261981"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3299), null, "Text Embedding 3 Large 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-3-large", null, 0.13m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("f1e8af73-604e-4270-881e-e7120d836319"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3235), null, "GPT-3.5 Turbo 1106 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-1106", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("f3b560aa-2fa7-49c9-81ea-684e8a336669"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3327), null, "Claude 3 Haiku 20240307 文本模型", true, "{}", "Claude", false, "claude-3-haiku-20240307", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("f4640912-e305-4d83-b78e-1bbc4eb53e3d"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3337), null, "Embedding S1 v1 嵌入模型", true, "{}", null, false, "embedding_s1_v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("f4fc4c30-f1a3-42b1-8ce3-c27e8e83bfcf"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3286), null, "GPT-4o 2024-05-13 文本模型", true, "{}", "OpenAI", false, "gpt-4o-2024-05-13", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("f9ab50f4-b943-4d5c-ab76-654cde31b4ff"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3328), null, "Claude 3.5 Sonnet 20240620 文本模型", true, "{}", "Claude", false, "claude-3-5-sonnet-20240620", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("ff2f85ae-d8bc-4356-b82b-7591015439dd"), null, null, null, true, null, null, null, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3337), null, "Embedding BERT 512 v1 嵌入模型", true, "{}", "OpenAI", false, "embedding-bert-512-v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("ff42c917-a096-42cd-88df-19212c30b99b"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 17, 0, 34, 23, 486, DateTimeKind.Local).AddTicks(3317), null, "通用文本模型 v3", true, "{}", "Spark", false, "generalv3", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null }
                });

            migrationBuilder.UpdateData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: "CA378C74-19E7-458A-918B-4DBB7AE1729D",
                columns: new[] { "CreatedAt", "Key" },
                values: new object[] { new DateTime(2025, 6, 17, 0, 34, 23, 443, DateTimeKind.Local).AddTicks(4379), "sk-AsRkUA8l49eK5ujcVYzFRzY2pbXuJ8iDPeygki" });

            migrationBuilder.UpdateData(
                table: "UserGroups",
                keyColumn: "Id",
                keyValue: new Guid("ca378c74-19e7-458a-918b-4dbb7ae17291"),
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 0, 34, 23, 444, DateTimeKind.Local).AddTicks(2429));

            migrationBuilder.UpdateData(
                table: "UserGroups",
                keyColumn: "Id",
                keyValue: new Guid("ca378c74-19e7-458a-918b-4dbb7ae1729d"),
                column: "CreatedAt",
                value: new DateTime(2025, 6, 17, 0, 34, 23, 444, DateTimeKind.Local).AddTicks(2062));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "CA378C74-19E7-458A-918B-4DBB7AE1729D",
                columns: new[] { "CreatedAt", "Password", "PasswordHas" },
                values: new object[] { new DateTime(2025, 6, 17, 0, 34, 23, 441, DateTimeKind.Local).AddTicks(9170), "5c8af64858d54a2d958093785ecc8893", "717c1f27b73441f78d515390ad4cd25b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("00f7b0e9-41d8-450c-9c60-8ae5bc2faf32"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("01af2839-b8a1-49b3-811a-5e102baa995d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("05c0674d-4c8d-413c-a03a-a313ba4e36c5"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0764b093-22cb-41f7-abea-990007c15fdd"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0895255f-4280-4acc-b17a-6748b62d316a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0e54dc38-37ed-4e38-9893-7ee0be00f4ce"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("117a735f-dcc4-4007-9ed1-bbb445754421"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("16766eb1-96c6-4732-90cd-4c6673ff0554"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("17113c7d-30ea-473f-bc8f-089f0e85f8ed"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1c01ca85-050f-4410-bc9f-11eaaf8936d0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1c83c5cc-cde7-4785-a9df-bcd3cf7d9988"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1caef44d-8405-41de-8d48-fd292d09db99"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("20cc38db-3808-483a-8b9d-7aa922dd3a06"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("238ef6d3-5f14-4063-8c52-3099e1ca63bf"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("256c445d-130a-4e11-85d4-4754f2b0d78c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("27559ded-3730-4a2f-95e7-4ffb37fe4bbc"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2b709dd6-34b8-4cad-8414-30d2b3a11c39"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2d0ad485-f3b8-4172-a255-02e3610d9bab"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("33e99e88-46b6-49aa-8b81-910c259f246d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("38b51a27-b304-4088-bea7-a03f76e8f3fb"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("3b1e56ad-d2f8-499f-b328-201532eaf343"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("43eaf784-ae62-4ad6-8c9b-6fc5265b8dbf"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("48abe2a1-bf67-4c45-8a01-50b1b7739cd7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("4d489977-aa4f-48f8-b652-3b16d08be83d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("51b4a390-d82d-4a79-b5e0-65fb3da5b3aa"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5260a718-9152-4a64-a1f5-98da0ab48ebf"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5a86ee49-da81-45a9-840b-3c6557d09d74"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5b935cf3-2aaa-4fab-88a3-afc54ce7cb2d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("617a4ff4-c582-4ca3-b46e-1f713d901964"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("65b4cf71-7e31-493b-870a-9930cacaf38a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("660e22c6-a359-4a14-9d71-cbde03286f67"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("757f14b8-2da5-4009-9a55-6f7d2e7dc060"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7d09f0d7-f6f8-48f3-b4fe-76a9ce0212c9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7ebcc1c8-ba73-4307-9c9c-a455c20ca2cb"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("88612241-2eea-42e9-ad5e-4cc6c36c6c37"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("88c5dd5f-2107-47bb-a1c6-6b179d2ba05d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8a69c08f-d2ce-41c0-8c06-428fa60eb187"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8ad35a76-8e9b-43b8-a110-d63b0a9e669c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8bb3838c-5dca-4ca8-980d-87e1f6db714f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8e094a75-2dac-4e1b-93bf-33982a52c170"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("938b3193-fe3c-4346-9799-79f2ca81bf12"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("954de913-fe77-4472-ba78-1c3d214a16d5"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("96cfd1e6-3609-4c26-a7e8-a2054a7de326"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9853563d-39aa-4989-bfca-d3ac239ced4b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9cffc495-547a-41bb-9964-46712a118993"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a0191355-03e3-4767-8f9f-2451fb5bcac7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a491c4b5-f938-4549-a0a4-651cf4eb7f32"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a536b818-0950-4deb-8269-5cd398ef59c5"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a62c5e7e-195f-40f4-9b6a-405b890507aa"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a906437f-9f57-477d-8c13-a71ea174db64"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ac63ff46-c4f6-416c-ac2c-fbef11290d14"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("aec66ecc-599b-4c35-b842-98c4e1763fa4"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("aef46351-e009-40ff-8acb-6b4e6baca392"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("af4dc6d6-0ec9-4148-beab-9590416f27ed"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b9e2eb75-8ed1-4e14-a29d-f5d64d87c2ac"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("bc32ece1-fa27-4e3e-b715-8e14512dc457"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("bc9fab50-fae7-45fe-9082-6db8f1a60733"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("bed23d61-9526-48ba-95d4-8e2346d12520"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("bf1eb103-f00e-48c0-a1db-2b2d2f203390"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c188da00-fd06-44ed-b1f2-2a2b0227b45a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c39e26cf-777d-4f8a-822c-f21a68ef483b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("cb2e52ca-a4a2-4f4a-90bb-b687570e1379"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("cc6b4817-8afb-4f7f-ba59-e3211b3c7166"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ccbd58de-92a7-4476-a5d8-649f04b3f761"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d405e652-a91e-4e69-97f3-09f1fd939fbf"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d6684434-295f-48f5-ac07-7f56e1b77f4d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e81d9a26-506b-4ae3-b8aa-13456da65bd2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("eb3e033c-b97d-47bb-93e1-b20924df860e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ebb313a8-4b20-466d-90d4-355980b5fd60"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ece312b6-fb48-4553-8fdc-a3f39e261981"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f1e8af73-604e-4270-881e-e7120d836319"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f3b560aa-2fa7-49c9-81ea-684e8a336669"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f4640912-e305-4d83-b78e-1bbc4eb53e3d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f4fc4c30-f1a3-42b1-8ce3-c27e8e83bfcf"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f9ab50f4-b943-4d5c-ab76-654cde31b4ff"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ff2f85ae-d8bc-4356-b82b-7591015439dd"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ff42c917-a096-42cd-88df-19212c30b99b"));

            migrationBuilder.InsertData(
                table: "ModelManagers",
                columns: new[] { "Id", "AudioCacheRate", "AudioOutputRate", "AudioPromptRate", "Available", "CacheHitRate", "CacheRate", "CompletionRate", "CreatedAt", "Creator", "Description", "Enable", "Extension", "Icon", "IsVersion2", "Model", "Modifier", "PromptRate", "QuotaMax", "QuotaType", "Tags", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("01eeedeb-1c80-449d-9a77-76e975f9ff78"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4506), null, "ChatGLM Lite 文本模型", true, "{}", "ChatGLM", false, "chatglm_lite", null, 0.1429m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("05e86279-444b-4f77-a950-8ac669eb40b5"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4425), null, "GPT-3.5 Turbo 0613 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0613", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("07f81b87-75f8-4550-b5cb-db0b8047a167"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4517), null, "Claude 3.5 Sonnet 20240620 文本模型", true, "{}", "Claude", false, "claude-3-5-sonnet-20240620", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("09ad340e-2a52-4be4-8b7d-cd7e66d41cfe"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4474), null, "GPT-4o Mini 文本模型", true, "{}", "OpenAI", false, "gpt-4o-mini", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("0b026a6b-7ee0-4fb1-89ab-d8662de582f1"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4526), null, "DALL-E 2 图像生成模型", true, "{}", "OpenAI", false, "dall-e-2", null, 8000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("102f0f4b-5b3a-493a-a03a-b04502b3c5b9"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4448), null, "GPT-4 32k 0314 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k-0314", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("11df0acb-be16-4c11-8eec-6518dc6c85d9"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4472), null, "GPT-4o 文本模型", true, "{}", "OpenAI", false, "gpt-4o", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("138cc648-0be8-45d8-b55b-c5c2bc6f35aa"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4522), null, "Claude Instant 1 文本模型", true, "{}", "Claude", false, "claude-instant-1", null, 0.815m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("146c899e-a8c5-4a31-88c6-fc7058a3ab80"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4503), null, "通用文本模型 v3", true, "{}", "Spark", false, "generalv3", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("179825c7-3c6d-4485-8b86-a541d56956de"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4517), null, "Claude 3 Haiku 20240307 文本模型", true, "{}", "Claude", false, "claude-3-haiku-20240307", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("1816e3da-e687-44eb-a847-601f56b8f0b8"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4514), null, "Claude 2.0 文本模型", true, "{}", "Claude", false, "claude-2.0", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("18a3de73-57ed-4a36-8581-d63e4c1d296b"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4537), null, "GLM 4 全部文本模型", true, "{}", "ChatGLM", false, "glm-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("1e1269f3-231c-4418-b0d7-c3f5b9c78560"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4487), null, "Text Curie 001 文本模型", true, "{}", "OpenAI", false, "text-curie-001", null, 1m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("1fe4c9c5-8457-4cfc-894c-e599a170ec3e"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4459), null, "Gemini Pro 视觉模型", true, "{}", "Google", false, "gemini-pro-vision", null, 2m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("206de9b5-e045-462c-9e39-1ffe405ffc0a"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4495), null, "Text Embedding Ada 002 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-ada-002", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("209b8751-1a3b-435c-8963-800eb39e014c"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4536), null, "GLM 4 文本模型", true, "{}", "ChatGLM", false, "glm-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("27956de8-b97b-4b93-99b1-b6066ee7b0be"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4460), null, "Gemini 1.5 Flash 文本模型", true, "{}", "Google", false, "gemini-1.5-flash", null, 0.2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("2d681dac-c2fe-4809-a162-8ee6f47c5089"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4471), null, "GPT-4 视觉预览模型", true, "{}", "OpenAI", false, "gpt-4-vision-preview", null, 10m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("2e7b0cc8-4d6c-4de6-b0c7-4589aac92ca8"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4515), null, "Claude 2.1 文本模型", true, "{}", "Claude", false, "claude-2.1", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("33156f30-cab2-4484-8331-0c3a332fb9f3"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4429), null, "GPT-3.5 Turbo 16k 0613 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-16k-0613", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("341e094a-c67e-47bf-a6c1-0ee946b5fe59"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4502), null, "Hunyuan Lite 文本模型", true, "{}", "Hunyuan", false, "hunyuan-lite", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("371718a5-6d1e-49c4-845b-ec7017f145b6"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4444), null, "GPT-4 1106 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-1106-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("3d801782-7daa-4b70-8000-fd8b8df8b0f9"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4505), null, "通用文本模型 v3.5", true, "{}", "Spark", false, "generalv3.5", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("409fbaf8-246c-4de6-974d-8192c9804fdb"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4489), null, "Text Davinci 002 文本模型", true, "{}", "OpenAI", false, "text-davinci-002", null, 10m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("41d9fd6b-e16a-4fb0-bd8f-9129e638baa7"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4521), null, "Claude 3 Opus 20240229 文本模型", true, "{}", "Claude", false, "claude-3-opus-20240229", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("45273cda-ac7e-442a-9896-822eb69d2ace"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4538), null, "GLM 4v 文本模型", true, "{}", "ChatGLM", false, "glm-4v", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("460fc313-1f21-49a1-86f5-46b053aa2f60"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4496), null, "TTS 1 1106 语音合成模型", true, "{}", "OpenAI", false, "tts-1-1106", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("478e8e77-979b-444d-ad65-3d49d67cfefc"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4497), null, "TTS 1 HD 语音合成模型", true, "{}", "OpenAI", false, "tts-1-hd", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("531a101d-264f-4da4-bcfa-3221316e6547"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4486), null, "Text Babbage 001 文本模型", true, "{}", "OpenAI", false, "text-babbage-001", null, 0.25m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("53eaf2c8-0281-4eb9-979f-3d60c4cac542"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4481), null, "Moonshot v1 128k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-128k", null, 5.06m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("571fa230-0ad9-4d19-b28b-653a5441de2b"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4489), null, "Text Davinci 003 文本模型", true, "{}", "OpenAI", false, "text-davinci-003", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("58a6f6b4-8a63-4330-85c9-d96248dfffcf"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4441), null, "GPT-4 0613 文本模型", true, "{}", "OpenAI", false, "gpt-4-0613", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("58b25505-40c2-4b3a-b344-255fa2e9bf15"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4464), null, "GPT-4 Turbo 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo-preview", null, 5m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", "chat", null },
                    { new Guid("5efffaeb-0d78-4824-9b9b-dfa07b14613d"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4427), null, "GPT-3.5 Turbo 1106 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-1106", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("6c63980c-6529-4310-8d69-7ec231a614a2"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4499), null, "Whisper 1 语音识别模型", true, "{}", "OpenAI", false, "whisper-1", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "stt", null },
                    { new Guid("71800c7e-1913-476c-9e7c-13bf7311b48d"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4434), null, "GPT-4 文本模型", true, "{}", "OpenAI", false, "gpt-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("72afeb42-5fa3-44c2-86b2-5e5db28a7365"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4458), null, "Gemini Pro 文本模型", true, "{}", "Google", false, "gemini-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("74a1158e-3133-4c59-b11c-15f80c061e3f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4490), null, "Text Davinci Edit 001 文本模型", true, "{}", "OpenAI", false, "text-davinci-edit-001", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("7b121482-bdf3-4577-b61e-1ec6b92b6e79"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4478), null, "GPT-4o 2024-08-06 文本模型", true, "{}", "OpenAI", false, "gpt-4o-2024-08-06", null, 1.25m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("87da8ea4-8c44-47b5-a5e6-dfb929a685b6"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4523), null, "Claude Instant 1.2 文本模型", true, "{}", "Claude", false, "claude-instant-1.2", null, 0.4m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("892856e7-8dea-407f-93b9-02c9728b2f8c"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4496), null, "TTS 1 语音合成模型", true, "{}", "OpenAI", false, "tts-1", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("8fd587d1-979c-499d-a6fd-ab10b2b728f9"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4461), null, "GPT-4 Turbo 2024-04-09 文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo-2024-04-09", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("9009bc1f-7acb-47e2-83b5-90dfa57e51bf"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4437), null, "GPT-4 0125 预览文本模型", true, "{}", "OpenAI", false, "gpt-4-0125-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("9318b593-610c-485a-9644-a3050628eeb0"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4475), null, "GPT-4o Mini 2024-07-18 文本模型", true, "{}", "OpenAI", false, "gpt-4o-mini-2024-07-18", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("986794d7-354c-47cd-bb00-23c9797ca67f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4494), null, "Text Embedding 3 Small 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-3-small", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("9a9c08db-2157-482c-8b29-f086152f6514"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4518), null, "Claude 3 Sonnet 20240229 文本模型", true, "{}", "Claude", false, "claude-3-sonnet-20240229", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("a27d7691-2370-4a19-ab01-ae0149c4ce9c"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4453), null, "GPT-4 全部文本模型", true, "{}", "OpenAI", false, "gpt-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u8054\\u7F51\"]", "chat", null },
                    { new Guid("a4e242b7-c52f-4ec5-b4ed-89ccb817141d"), null, null, null, true, null, null, 3m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4506), null, "4.0 超级文本模型", true, "{}", "Spark", false, "4.0Ultra", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("acd9fc8b-03c9-4c20-a0df-430855d6b56f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4528), null, "Embedding 2 嵌入模型", true, "{}", "OpenAI", false, "embedding-2", null, 0.0355m, "", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("ad0fe124-0e1e-4603-ab8b-3c04cd5b93d1"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4456), null, "Gemini 1.5 Pro 文本模型", true, "{}", "Google", false, "gemini-1.5-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("b000b042-ee25-4d21-b131-230e7f67a9e1"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4439), null, "GPT-4 0314 文本模型", true, "{}", "OpenAI", false, "gpt-4-0314", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("b0401c18-b7f2-44be-a508-aad885ab7520"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4483), null, "Moonshot v1 32k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-32k", null, 2m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("b1deed19-33b3-4ddb-b7f6-51c9cfc21cd1"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4534), null, "Embedding S1 v1 嵌入模型", true, "{}", null, false, "embedding_s1_v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("b75e4c89-f8c6-4582-aef1-771adbf6197e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4527), null, "GPT Image 图片生成模型", true, "{}", "OpenAI", false, "gpt-image-1", null, 50000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("b7c2569d-d314-4217-9c2b-534f058ca12d"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4513), null, "Claude 2 文本模型", true, "{}", "Claude", false, "claude-2", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("bd6e97f3-d817-426c-9d73-aa33d0a49e89"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4424), null, "GPT-3.5 Turbo 0301 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0301", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("c20dcd9c-9248-4398-9f10-96eaf43713cd"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4452), null, "GPT-4 32k 0613 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k-0613", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("c3104d83-0107-4921-92f2-d1b1337bff69"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4454), null, "GPT-4 Turbo 文本模型", true, "{}", "OpenAI", false, "gpt-4-turbo", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("c38f933e-8f76-41e7-8a0a-ca4fb96e42a6"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4535), null, "GLM 3 Turbo 文本模型", true, "{}", "ChatGLM", false, "glm-3-turbo", null, 0.355m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("c3b94f12-33c4-455e-94c0-5bc570ce817e"), null, null, null, true, null, null, 2m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4502), null, "通用文本模型", true, "{}", "Spark", false, "general", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("c485d60e-40b0-4a99-8bc9-98d72902bd43"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4428), null, "GPT-3.5 Turbo 16k 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-16k", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("cd99fdf8-07bd-4964-9cce-f43d2e4f0043"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4493), null, "Text Embedding 3 Large 嵌入模型", true, "{}", "OpenAI", false, "text-embedding-3-large", null, 0.13m, "8K", 1, "[\"\\u6587\\u672C\"]", "embedding", null },
                    { new Guid("cdc637ac-3d66-4d5a-adce-2c4d731020f6"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4527), null, "DALL-E 3 图像生成模型", true, "{}", "OpenAI", false, "dall-e-3", null, 20000m, null, 2, "[\"\\u56FE\\u7247\"]", "image", null },
                    { new Guid("d35543eb-7642-4047-adac-318dac91a135"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4498), null, "TTS 1 HD 1106 语音合成模型", true, "{}", "OpenAI", false, "tts-1-hd-1106", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", "tts", null },
                    { new Guid("de5db86f-6fae-4cb3-b17f-883f2f6a6ce2"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4421), null, "GPT-3.5 Turbo 0125 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-0125", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("e176a06d-5cd1-43c5-aad8-318076fe6694"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4512), null, "ChatGLM Turbo 文本模型", true, "{}", "ChatGLM", false, "chatglm_turbo", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("e2d21df5-7794-461d-be40-ceaeee1cb325"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4473), null, "ChatGPT 4o 最新文本模型", true, "{}", "OpenAI", false, "chatgpt-4o-latest", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("e32f3311-2d76-482c-9b53-02073a5a9f8a"), null, null, null, true, null, null, 4m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4477), null, "GPT-4o 2024-05-13 文本模型", true, "{}", "OpenAI", false, "gpt-4o-2024-05-13", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", "chat", null },
                    { new Guid("e83865f6-2db0-4578-bae2-fbc0c69f4477"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4446), null, "GPT-4 32k 文本模型", true, "{}", "OpenAI", false, "gpt-4-32k", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("ecc9a3b4-e306-4504-973c-c3001f654197"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4445), null, "GPT-4 1106 视觉预览模型", true, "{}", "OpenAI", false, "gpt-4-1106-vision-preview", null, 10m, "128K", 1, "[\"\\u89C6\\u89C9\",\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("f632dab4-b8bb-49b9-aa81-88cb88a41f7f"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4529), null, "Embedding BERT 512 v1 嵌入模型", true, "{}", "OpenAI", false, "embedding-bert-512-v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", "embedding", null },
                    { new Guid("f89dd699-72d6-49c7-b677-d5462be079cd"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4484), null, "Moonshot v1 8k 文本模型", true, "{}", "Moonshot", false, "moonshot-v1-8k", null, 1m, "8K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("faf2b433-52ce-4796-b194-51bd9e3bda16"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4430), null, "GPT-3.5 Turbo Instruct 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo-instruct", null, 0.001m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("fbba0c9b-1c58-44e9-93d3-024f0dde92cf"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4508), null, "ChatGLM 标准文本模型", true, "{}", "ChatGLM", false, "chatglm_std", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("fcf8d858-a576-4c5f-8317-d71d2aede17e"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4507), null, "ChatGLM Pro 文本模型", true, "{}", "ChatGLM", false, "chatglm_pro", null, 0.7143m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("fda625d9-ee32-4c7e-b93c-bbc490f28c89"), null, null, null, true, null, null, 5m, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4516), null, "Claude 3 Haiku 文本模型", true, "{}", "Claude", false, "claude-3-haiku", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null },
                    { new Guid("ffc823b7-e912-4238-afa1-60b728ebd773"), null, null, null, true, null, null, null, new DateTime(2025, 6, 16, 23, 28, 41, 597, DateTimeKind.Local).AddTicks(4095), null, "GPT-3.5 Turbo 文本模型", true, "{}", "OpenAI", false, "gpt-3.5-turbo", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", "chat", null }
                });

            migrationBuilder.UpdateData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: "CA378C74-19E7-458A-918B-4DBB7AE1729D",
                columns: new[] { "CreatedAt", "Key" },
                values: new object[] { new DateTime(2025, 6, 16, 23, 28, 41, 557, DateTimeKind.Local).AddTicks(5533), "sk-Hs8jmpL6pLjVCUoy6S9i4PxoAesMv393hcR90S" });

            migrationBuilder.UpdateData(
                table: "UserGroups",
                keyColumn: "Id",
                keyValue: new Guid("ca378c74-19e7-458a-918b-4dbb7ae17291"),
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 23, 28, 41, 558, DateTimeKind.Local).AddTicks(3693));

            migrationBuilder.UpdateData(
                table: "UserGroups",
                keyColumn: "Id",
                keyValue: new Guid("ca378c74-19e7-458a-918b-4dbb7ae1729d"),
                column: "CreatedAt",
                value: new DateTime(2025, 6, 16, 23, 28, 41, 558, DateTimeKind.Local).AddTicks(3317));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "CA378C74-19E7-458A-918B-4DBB7AE1729D",
                columns: new[] { "CreatedAt", "Password", "PasswordHas" },
                values: new object[] { new DateTime(2025, 6, 16, 23, 28, 41, 556, DateTimeKind.Local).AddTicks(171), "0983eaa1e7837e5589ac163d6692b37f", "e165f0cbf1c04355a8e64fab3f735ba5" });
        }
    }
}
