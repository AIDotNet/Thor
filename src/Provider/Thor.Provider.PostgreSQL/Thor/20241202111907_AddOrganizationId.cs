using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Thor.Provider.PostgreSQL.Thor
{
    /// <inheritdoc />
    public partial class AddOrganizationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("009d872e-7ded-48ed-acd7-6418f987fd4f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("01ff3f29-c00c-45bc-b377-c573100728b3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0296b929-ce40-4df0-a458-2964e0797a59"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("04d0bb5b-e3d1-4623-b248-f6e451f5687a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("054ba477-0ecf-4112-bf95-c5b65ad9c92f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("056cd3ed-ba1c-4c2c-81f6-11f807dac4b0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("07a1527a-9c98-4433-80d0-df44b75d84db"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("08397cab-7c87-4ab1-94f0-adab822ee3de"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0e739669-2332-4eff-8fb4-c4eff028d0d2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("10222f21-57b4-4f16-9f8f-8cd707cc63f4"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("138bc817-0215-413f-96d5-7a75192a3a7d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("15d71b93-d34c-43a9-9565-bb8d0da5ddf2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1aed309d-8653-415d-9ef3-f4bb59532ae5"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("24d33436-0441-49fd-b526-a3a16c64742d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("27273083-1c87-449b-93f6-3130a3f7e1d9"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("28f3ab06-6c12-4691-8c69-b09733010306"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2c88843e-833b-4e34-9879-4a622f204db5"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("36e1ae6c-2d64-4850-872d-fe0c2c0128c8"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("40989633-c854-4abe-86ff-38c44e09dba7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("413489f4-ec1d-42be-b6ff-4108dd43754c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("4451bcc0-09d9-46db-b03e-233bd9cbf934"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("45247d28-25e2-4d88-8558-f6db624f81d1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("47892d02-9985-4691-b99b-7d88b9bba2ed"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("4f7c05b0-ee5e-48da-954d-9937dea54ad4"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("52c559d9-ccfc-4a76-af0f-a03dc8e3a954"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("52d8d182-d91d-478e-b8cd-97650071d54f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("54accf82-ce76-4b22-88ae-1bbf51ea8158"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("55d9c543-d795-4d2f-af5f-a316acbf7a68"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5a0d1043-ea45-499e-bdee-59641b03033b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5d5c417a-ff35-4e09-b420-69071a3d3310"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5d8c5449-e6f7-4b16-b7ae-e56d525a8356"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5e7b1222-1d8e-4c67-90bd-372197d04a2c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6bb25f74-2b9c-463c-a0a4-2765f547f49c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("726fac2a-d0df-4217-94bd-99944bddf0b0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("75cd2bdd-4771-4a06-a440-b8cb0bf9ac4c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("76745063-1422-4107-b5d9-27acb8cdc173"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7b05e674-304c-41c1-8899-18f4784a0cd1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7b34f0ac-65bc-42b9-bd10-0c151c59d5ee"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7d4ac93b-200d-4f87-a85c-d6212c19baf1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("849fe8df-1344-4e1f-b276-484e8ee23466"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("86b0bcfc-4220-4942-a5c9-0396347c051a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("89aea156-24ee-49d5-bb31-a6b2fe1b3a9c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8eb31887-8365-46c4-882e-9a22280650fd"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8fa47d18-a994-4bbc-ba76-a071c462f661"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("907bcf12-0996-442d-b81d-c53b364b8179"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9462a339-2ecd-4394-a4c7-aef816fd39f6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("998ef7a5-2292-4c66-8e6f-77ee1a08c248"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9a44324a-997e-4358-aec4-4e789192acd3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9a5e224c-4cfe-4058-b09e-6252cc1bdf74"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9c0fbf7b-ff78-47ae-a3cf-be4902a6a9f1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9d3b34f9-74ea-4a2a-892e-19995f5ec2d3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a646afce-8641-4ae1-902b-cd59b73fed01"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a762c0d7-2a43-4305-8cbe-ed87822398ac"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a87ab7db-ad64-406c-a8aa-f38c2dd7e06d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ac0625c8-bb9a-46ea-9de5-d0e010a04cf8"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b1e879f6-c0e5-4c36-b8be-fbabb86b14eb"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b252e18d-b400-4cc5-93e2-fb7a25202ec0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b61711ff-19ec-4ad0-b2c8-925a94d13e05"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b7048614-36df-455d-8617-2d209e4c34ab"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b8d0d3aa-df8f-4089-8ad4-8d753e1fd882"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ba85c2ee-f1c9-4fe4-a7dc-8cbca1b271f2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c36364e6-9b2b-4476-bcd1-98d7ee2de279"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c73549ec-a493-4560-be90-6794bfaf6ccd"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c81b1a2e-d13d-4a58-87da-a5e8360cbc60"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c985873d-6b29-403e-be38-2faef58aa10f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ca529992-c50b-4bb2-8798-1fa8ac0984b4"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d13fe35b-d975-443c-8a70-0176ce8fdd36"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d5a7955b-c07f-4d5b-acb5-b2cfbacfac34"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d8f649f7-e1bb-4cdf-a6a0-ca410f1d370d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ddd9ba02-73ad-4d46-8deb-bdadc736efc6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e615c373-cb1a-4cc7-9e1c-b9ce5aa3eb70"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f3f191e1-d1a9-4439-b9d0-b6ddbab52b95"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f8fd18cc-754c-4e2b-9a75-851f312c24b4"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fafb1e1c-eb43-4142-8a1c-8ee8ccaab503"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fe9cbb87-250f-4157-878b-6b1687cdd345"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ff88870b-a15e-455c-be79-65d47a358da3"));

            migrationBuilder.DeleteData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: "324ed167-a7b8-4fe1-8dca-f608f6ff93ec");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "a0581ea1-af3a-4d78-8501-b832f6017b98");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "Users",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tokens",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiredTime",
                table: "Tokens",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "Tokens",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Tokens",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccessedTime",
                table: "Tokens",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "RedeemCodes",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RedeemedTime",
                table: "RedeemCodes",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RedeemCodes",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "RateLimitModels",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RateLimitModels",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Products",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductPurchaseRecords",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchaseTime",
                table: "ProductPurchaseRecords",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductPurchaseRecords",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ModelManagers",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ModelManagers",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Channels",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Channels",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.InsertData(
                table: "ModelManagers",
                columns: new[] { "Id", "AudioOutputRate", "AudioPromptRate", "Available", "CompletionRate", "CreatedAt", "Creator", "Description", "Enable", "Icon", "IsVersion2", "Model", "Modifier", "PromptRate", "QuotaMax", "QuotaType", "Tags", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0035e738-69fb-425e-a012-7ec23c5c660a"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8111), null, "Claude 2 文本模型", true, "Claude", false, "claude-2", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("00fde2a8-aacc-43da-ab0e-fe39282e5429"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8103), null, "Hunyuan Lite 文本模型", true, "Hunyuan", false, "hunyuan-lite", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("051e980f-53f3-4ea1-8bd8-a02d0b43012b"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8084), null, "GPT-4 视觉预览模型", true, "OpenAI", false, "gpt-4-vision-preview", null, 10m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("0884e4c8-e739-457e-822d-eda46fd7a758"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8101), null, "TTS 1 HD 语音合成模型", true, "OpenAI", false, "tts-1-hd", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("0948b812-b153-403a-8aa6-a9c4185ec4ed"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8056), null, "GPT-3.5 Turbo 0125 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0125", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("0df7ecdd-2dc2-4c78-9fb2-45267556ae6f"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8096), null, "Text Davinci Edit 001 文本模型", true, "OpenAI", false, "text-davinci-edit-001", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("0e9a9984-35df-4e36-a343-804a9c09df4f"), null, null, true, 3m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8108), null, "4.0 超级文本模型", true, "Spark", false, "4.0Ultra", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("10266cc1-f01a-434b-ac68-90487b200e8e"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8067), null, "GPT-3.5 Turbo 1106 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-1106", null, 0.25m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1ceb3e41-ec34-487a-b938-42c6bc5e9f2e"), null, null, true, 5m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8115), null, "Claude 3 Haiku 文本模型", true, "Claude", false, "claude-3-haiku", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("1dc4ee80-2149-4814-8f2d-eefc159c1a14"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8077), null, "GPT-4 全部文本模型", true, "OpenAI", false, "gpt-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u8054\\u7F51\"]", null },
                    { new Guid("216c9c24-2c78-46db-bcce-f52754b2535f"), null, null, true, 3m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8082), null, "Gemini Pro 视觉模型", true, "Google", false, "gemini-pro-vision", null, 2m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("2a3f405e-3912-46fd-9f82-cf25166ea2d1"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8097), null, "Text Embedding 3 Small 嵌入模型", true, "OpenAI", false, "text-embedding-3-small", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("2e01620e-30f8-4412-b69c-b5781d5decc2"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8100), null, "TTS 1 语音合成模型", true, "OpenAI", false, "tts-1", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("3026704d-6b0d-49f2-b5a1-713ce3760d41"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8112), null, "Claude 2.0 文本模型", true, "Claude", false, "claude-2.0", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("346df93d-624d-41bf-a489-c3e729bc6611"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8066), null, "GPT-3.5 Turbo 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0613", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("38428fb2-bd9a-4780-9bf4-48e3177b22be"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8058), null, "GPT-3.5 Turbo 0301 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-0301", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("444238b9-08de-46e5-ac21-f1091acd67d5"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8074), null, "GPT-4 1106 预览文本模型", true, "OpenAI", false, "gpt-4-1106-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("45130c80-0df9-4584-9018-cc4d9212f808"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8071), null, "GPT-4 0314 文本模型", true, "OpenAI", false, "gpt-4-0314", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("524dae06-bc27-41a3-a814-6cff2b936807"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8111), null, "ChatGLM Turbo 文本模型", true, "ChatGLM", false, "chatglm_turbo", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("529d51e8-eadd-4393-a5d7-c2cdbfaf49c0"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8120), null, "DALL-E 2 图像生成模型", true, "OpenAI", false, "dall-e-2", null, 8m, null, 2, "[\"\\u56FE\\u7247\"]", null },
                    { new Guid("5559764e-3682-4f8d-b40d-d4cba4ca72b6"), null, null, true, 2m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8105), null, "通用文本模型 v3", true, "Spark", false, "generalv3", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("55df6def-740e-4c02-8e30-3a5025f58838"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8118), null, "Claude Instant 1 文本模型", true, "Claude", false, "claude-instant-1", null, 0.815m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("57ac8770-7a93-4a16-9dd8-9edddb5d9e3e"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8122), null, "Embedding 2 嵌入模型", true, "OpenAI", false, "embedding-2", null, 0.0355m, "", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("5b0d06a7-2d37-4d41-8d47-f8d6071429ce"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8076), null, "GPT-4 32k 0314 文本模型", true, "OpenAI", false, "gpt-4-32k-0314", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("5c1ff8d4-5dfc-48fa-a6c5-c87cf803e203"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8091), null, "Moonshot v1 32k 文本模型", true, "Moonshot", false, "moonshot-v1-32k", null, 2m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("689fb848-598c-480c-86ad-f9af5f057e09"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8078), null, "GPT-4 Turbo 文本模型", true, "OpenAI", false, "gpt-4-turbo", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("6a3713a8-b12f-4a9e-a4b7-8c7b8577d99f"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8122), null, "Embedding BERT 512 v1 嵌入模型", true, "OpenAI", false, "embedding-bert-512-v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("6b5dcebb-ac03-42a4-b01f-6da2b82cff8d"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(7713), null, "GPT-3.5 Turbo 文本模型", true, "OpenAI", false, "gpt-3.5-turbo", null, 0.75m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("6b963874-eaa0-478e-bdcb-c9b0b50e47fb"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8124), null, "GLM 4 文本模型", true, "ChatGLM", false, "glm-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("6eb271e5-b35a-4994-9dc3-32d148e224ce"), null, null, true, 3m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8085), null, "GPT-4o 文本模型", true, "OpenAI", false, "gpt-4o", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("70922425-989a-4813-b1fb-6e762cdc83d4"), null, null, true, 5m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8116), null, "Claude 3 Sonnet 20240229 文本模型", true, "Claude", false, "claude-3-sonnet-20240229", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("73411891-d9de-4e0c-9871-b3bd6c18e8f1"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8127), null, "GLM 4v 文本模型", true, "ChatGLM", false, "glm-4v", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("76f9e362-a8d2-456f-aa69-04cb39137b50"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8103), null, "Whisper 1 语音识别模型", true, "OpenAI", false, "whisper-1", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("795d9b26-2dcb-4a1f-a52b-3dc1f1372d0b"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8123), null, "Embedding S1 v1 嵌入模型", true, null, false, "embedding_s1_v1", null, 0.1m, "128K", 1, "[\"\\u5D4C\\u5165\\u6A21\\u578B\"]", null },
                    { new Guid("7a0671ce-e2df-4787-a3e9-9a084ffe02ef"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8095), null, "Text Davinci 002 文本模型", true, "OpenAI", false, "text-davinci-002", null, 10m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("7b4d5454-7da0-4951-af69-c46590b45ebf"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8101), null, "TTS 1 1106 语音合成模型", true, "OpenAI", false, "tts-1-1106", null, 7.5m, null, 1, "[\"\\u97F3\\u9891\"]", null },
                    { new Guid("7d4f983b-3388-41df-ab2a-52e71832c2cc"), null, null, true, 5m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8116), null, "Claude 3.5 Sonnet 20240620 文本模型", true, "Claude", false, "claude-3-5-sonnet-20240620", null, 3m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("7dcd7e5c-6383-4b3e-acac-cec0fb8e05eb"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8077), null, "GPT-4 32k 0613 文本模型", true, "OpenAI", false, "gpt-4-32k-0613", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("83c71e9b-6904-4530-8048-fab87d6c9044"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8094), null, "Text Babbage 001 文本模型", true, "OpenAI", false, "text-babbage-001", null, 0.25m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("854f87e2-1e65-4b2f-ad0d-5b5d564e3fe7"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8073), null, "GPT-4 0613 文本模型", true, "OpenAI", false, "gpt-4-0613", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("87fd644e-94da-4424-b8ca-c8654250b4be"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8096), null, "Text Davinci 003 文本模型", true, "OpenAI", false, "text-davinci-003", null, 10m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("889dfefd-fe1e-4cac-9ecf-6fadfd8e3f37"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8070), null, "GPT-4 0125 预览文本模型", true, "OpenAI", false, "gpt-4-0125-preview", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("8923e0fa-731f-478e-a2cc-ace5f17f40a5"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8094), null, "Text Curie 001 文本模型", true, "OpenAI", false, "text-curie-001", null, 1m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("8a13358f-9b78-48b6-9a8d-d49636886cb6"), null, null, true, 4m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8089), null, "GPT-4o 2024-05-13 文本模型", true, "OpenAI", false, "gpt-4o-2024-05-13", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("9253f313-9060-4ba7-b603-43042645c9c7"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8114), null, "Claude 2.1 文本模型", true, "Claude", false, "claude-2.1", null, 7.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("92d2e421-7781-42da-b33d-8bb69d4f1071"), null, null, true, 2m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8104), null, "通用文本模型", true, "Spark", false, "general", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("977e8578-add0-4738-a7fd-78ab35fec145"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8075), null, "GPT-4 1106 视觉预览模型", true, "OpenAI", false, "gpt-4-1106-vision-preview", null, 10m, "128K", 1, "[\"\\u89C6\\u89C9\",\"\\u6587\\u672C\"]", null },
                    { new Guid("99dac9cb-63ee-4909-8694-4bdffa498f30"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8118), null, "Claude Instant 1.2 文本模型", true, "Claude", false, "claude-instant-1.2", null, 0.4m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a493dc4d-08ce-4796-8db2-fd079450de4b"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8125), null, "GLM 4 全部文本模型", true, "ChatGLM", false, "glm-4-all", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a4d075d0-15d1-48a3-be93-6545d46fe91c"), null, null, true, 4m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8087), null, "GPT-4o Mini 文本模型", true, "OpenAI", false, "gpt-4o-mini", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("a84a5213-805e-4c96-8499-3cef0d5e739a"), null, null, true, 2m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8080), null, "Gemini 1.5 Pro 文本模型", true, "Google", false, "gemini-1.5-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("a8afbd4e-6c2b-43af-aad3-b9abed9b2ec4"), null, null, true, 2m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8107), null, "通用文本模型 v3.5", true, "Spark", false, "generalv3.5", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b3b6d38d-a4ce-4bbf-a229-98112fa51a3f"), null, null, true, 3m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8081), null, "Gemini Pro 文本模型", true, "Google", false, "gemini-pro", null, 2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("b99eaa52-3a52-4388-89ab-9d677479ec62"), null, null, true, 5m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8117), null, "Claude 3 Opus 20240229 文本模型", true, "Claude", false, "claude-3-opus-20240229", null, 30m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("baba9620-00fb-46b7-b284-c37069529490"), null, null, true, 4m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8089), null, "GPT-4o 2024-08-06 文本模型", true, "OpenAI", false, "gpt-4o-2024-08-06", null, 1.25m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("c05e6858-45fa-4f26-9e58-ae2bdf62bcbb"), null, null, true, 4m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8087), null, "ChatGPT 4o 最新文本模型", true, "OpenAI", false, "chatgpt-4o-latest", null, 3m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("c27deab3-6d0b-45cd-a3c9-5a1f02aa12ee"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8110), null, "ChatGLM 标准文本模型", true, "ChatGLM", false, "chatglm_std", null, 0.3572m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("c58a7d79-cd53-424b-bc6e-89e0924c258b"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8075), null, "GPT-4 32k 文本模型", true, "OpenAI", false, "gpt-4-32k", null, 30m, "32K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ca895013-db68-4139-a7cc-92f17584bd36"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8098), null, "Text Embedding Ada 002 嵌入模型", true, "OpenAI", false, "text-embedding-ada-002", null, 0.1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("cac15953-d2f2-43a6-8050-3852fe836ff6"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8109), null, "ChatGLM Pro 文本模型", true, "ChatGLM", false, "chatglm_pro", null, 0.7143m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("cdb2b93a-dedd-460f-b46e-8ab38952e65a"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8097), null, "Text Embedding 3 Large 嵌入模型", true, "OpenAI", false, "text-embedding-3-large", null, 0.13m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("d15aec92-7930-4055-858c-f565686d6f40"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8091), null, "Moonshot v1 8k 文本模型", true, "Moonshot", false, "moonshot-v1-8k", null, 1m, "8K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("d815f09d-860a-428e-8b28-81d4fa7346c6"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8067), null, "GPT-3.5 Turbo 16k 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("d8a7afa8-59ce-4aa0-9931-2014a02cf0c2"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8083), null, "GPT-4 Turbo 2024-04-09 文本模型", true, "OpenAI", false, "gpt-4-turbo-2024-04-09", null, 5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("da2f3f9c-8c38-474f-bddc-e52a107fb895"), null, null, true, 3m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8082), null, "Gemini 1.5 Flash 文本模型", true, "Google", false, "gemini-1.5-flash", null, 0.2m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ddb35baa-aea1-49ca-b733-4865faf3e511"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8084), null, "GPT-4 Turbo 预览文本模型", true, "OpenAI", false, "gpt-4-turbo-preview", null, 5m, "8K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\"]", null },
                    { new Guid("e8952eb6-e4de-4ba5-ba76-3dbf6d04540e"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8123), null, "GLM 3 Turbo 文本模型", true, "ChatGLM", false, "glm-3-turbo", null, 0.355m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("e9989910-7bc0-4414-8edf-6cad428a2fbe"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8069), null, "GPT-4 文本模型", true, "OpenAI", false, "gpt-4", null, 15m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ed63adcb-e255-4e58-bde4-346a0d0eb0d4"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8068), null, "GPT-3.5 Turbo 16k 0613 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-16k-0613", null, 0.75m, "16K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("ef9d781e-35b1-4ddf-adb4-a8317043042e"), null, null, true, 5m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8115), null, "Claude 3 Haiku 20240307 文本模型", true, "Claude", false, "claude-3-haiku-20240307", null, 0.5m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("f11d6eaf-2dc0-471f-a0a3-a997dc972903"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8108), null, "ChatGLM Lite 文本模型", true, "ChatGLM", false, "chatglm_lite", null, 0.1429m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("f12ab79e-28f6-426e-9b02-a355334098b3"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8069), null, "GPT-3.5 Turbo Instruct 文本模型", true, "OpenAI", false, "gpt-3.5-turbo-instruct", null, 0.001m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("f57c4cf9-4028-49fc-a71d-0505bf222369"), null, null, true, 4m, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8088), null, "GPT-4o Mini 2024-07-18 文本模型", true, "OpenAI", false, "gpt-4o-mini-2024-07-18", null, 0.07m, "128K", 1, "[\"\\u6587\\u672C\",\"\\u89C6\\u89C9\",\"\\u97F3\\u9891\"]", null },
                    { new Guid("f5fe8dbd-d3c5-45b7-9cfd-6e714ea8988c"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8090), null, "Moonshot v1 128k 文本模型", true, "Moonshot", false, "moonshot-v1-128k", null, 5.06m, "128K", 1, "[\"\\u6587\\u672C\"]", null },
                    { new Guid("fad3a017-22b3-461b-9910-1118104ab494"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8121), null, "DALL-E 3 图像生成模型", true, "OpenAI", false, "dall-e-3", null, 20m, null, 2, "[\"\\u56FE\\u7247\"]", null },
                    { new Guid("fefb72fe-d340-4da5-976b-c9c0e79ac5f0"), null, null, true, null, new DateTime(2024, 12, 2, 19, 19, 6, 694, DateTimeKind.Local).AddTicks(8102), null, "TTS 1 HD 1106 语音合成模型", true, "OpenAI", false, "tts-1-hd-1106", null, 15m, null, 2, "[\"\\u97F3\\u9891\"]", null }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Key", "Description", "Private", "Value" },
                values: new object[,]
                {
                    { "Setting:SystemSetting:CasdoorClientId", "Casdoor Client Id", true, "" },
                    { "Setting:SystemSetting:CasdoorClientSecret", "Casdoor Client Secret", false, "" },
                    { "Setting:SystemSetting:CasdoorEndipoint", "Casdoor 自定义端点", true, "" },
                    { "Setting:SystemSetting:EnableCasdoorAuth", "启用Casdoor 授权", true, "false" }
                });

            migrationBuilder.InsertData(
                table: "Tokens",
                columns: new[] { "Id", "AccessedTime", "CreatedAt", "Creator", "DeletedAt", "Disabled", "ExpiredTime", "IsDelete", "Key", "LimitModels", "Modifier", "Name", "RemainQuota", "UnlimitedExpired", "UnlimitedQuota", "UpdatedAt", "UsedQuota", "WhiteIpList" },
                values: new object[] { "7a90ff66-c00f-4d28-a666-6349d2abef2d", null, new DateTime(2024, 12, 2, 19, 19, 6, 667, DateTimeKind.Local).AddTicks(5127), "260e76d0-2355-4db2-97b7-5616e7a01560", null, false, null, false, "sk-Cx47l1BOABEs9rvqFj50Ss9dALRuQ4tonQwmYJ", "[]", null, "默认Token", 0L, true, true, null, 0L, "[]" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "260e76d0-2355-4db2-97b7-5616e7a01560", null, 0L, new DateTime(2024, 12, 2, 19, 19, 6, 665, DateTimeKind.Local).AddTicks(9342), null, null, "239573049@qq.com", false, false, null, "cadc12e5cf0a26e9df6f649d93b1e578", "673d96237a3d4dd1b3cba3ce9289a7e1", 0L, 1000000000L, "admin", null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0035e738-69fb-425e-a012-7ec23c5c660a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("00fde2a8-aacc-43da-ab0e-fe39282e5429"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("051e980f-53f3-4ea1-8bd8-a02d0b43012b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0884e4c8-e739-457e-822d-eda46fd7a758"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0948b812-b153-403a-8aa6-a9c4185ec4ed"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0df7ecdd-2dc2-4c78-9fb2-45267556ae6f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("0e9a9984-35df-4e36-a343-804a9c09df4f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("10266cc1-f01a-434b-ac68-90487b200e8e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1ceb3e41-ec34-487a-b938-42c6bc5e9f2e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("1dc4ee80-2149-4814-8f2d-eefc159c1a14"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("216c9c24-2c78-46db-bcce-f52754b2535f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2a3f405e-3912-46fd-9f82-cf25166ea2d1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("2e01620e-30f8-4412-b69c-b5781d5decc2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("3026704d-6b0d-49f2-b5a1-713ce3760d41"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("346df93d-624d-41bf-a489-c3e729bc6611"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("38428fb2-bd9a-4780-9bf4-48e3177b22be"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("444238b9-08de-46e5-ac21-f1091acd67d5"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("45130c80-0df9-4584-9018-cc4d9212f808"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("524dae06-bc27-41a3-a814-6cff2b936807"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("529d51e8-eadd-4393-a5d7-c2cdbfaf49c0"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5559764e-3682-4f8d-b40d-d4cba4ca72b6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("55df6def-740e-4c02-8e30-3a5025f58838"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("57ac8770-7a93-4a16-9dd8-9edddb5d9e3e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5b0d06a7-2d37-4d41-8d47-f8d6071429ce"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("5c1ff8d4-5dfc-48fa-a6c5-c87cf803e203"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("689fb848-598c-480c-86ad-f9af5f057e09"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6a3713a8-b12f-4a9e-a4b7-8c7b8577d99f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6b5dcebb-ac03-42a4-b01f-6da2b82cff8d"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6b963874-eaa0-478e-bdcb-c9b0b50e47fb"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("6eb271e5-b35a-4994-9dc3-32d148e224ce"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("70922425-989a-4813-b1fb-6e762cdc83d4"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("73411891-d9de-4e0c-9871-b3bd6c18e8f1"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("76f9e362-a8d2-456f-aa69-04cb39137b50"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("795d9b26-2dcb-4a1f-a52b-3dc1f1372d0b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7a0671ce-e2df-4787-a3e9-9a084ffe02ef"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7b4d5454-7da0-4951-af69-c46590b45ebf"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7d4f983b-3388-41df-ab2a-52e71832c2cc"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("7dcd7e5c-6383-4b3e-acac-cec0fb8e05eb"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("83c71e9b-6904-4530-8048-fab87d6c9044"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("854f87e2-1e65-4b2f-ad0d-5b5d564e3fe7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("87fd644e-94da-4424-b8ca-c8654250b4be"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("889dfefd-fe1e-4cac-9ecf-6fadfd8e3f37"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8923e0fa-731f-478e-a2cc-ace5f17f40a5"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("8a13358f-9b78-48b6-9a8d-d49636886cb6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("9253f313-9060-4ba7-b603-43042645c9c7"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("92d2e421-7781-42da-b33d-8bb69d4f1071"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("977e8578-add0-4738-a7fd-78ab35fec145"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("99dac9cb-63ee-4909-8694-4bdffa498f30"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a493dc4d-08ce-4796-8db2-fd079450de4b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a4d075d0-15d1-48a3-be93-6545d46fe91c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a84a5213-805e-4c96-8499-3cef0d5e739a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("a8afbd4e-6c2b-43af-aad3-b9abed9b2ec4"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b3b6d38d-a4ce-4bbf-a229-98112fa51a3f"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("b99eaa52-3a52-4388-89ab-9d677479ec62"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("baba9620-00fb-46b7-b284-c37069529490"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c05e6858-45fa-4f26-9e58-ae2bdf62bcbb"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c27deab3-6d0b-45cd-a3c9-5a1f02aa12ee"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("c58a7d79-cd53-424b-bc6e-89e0924c258b"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ca895013-db68-4139-a7cc-92f17584bd36"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("cac15953-d2f2-43a6-8050-3852fe836ff6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("cdb2b93a-dedd-460f-b46e-8ab38952e65a"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d15aec92-7930-4055-858c-f565686d6f40"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d815f09d-860a-428e-8b28-81d4fa7346c6"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("d8a7afa8-59ce-4aa0-9931-2014a02cf0c2"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("da2f3f9c-8c38-474f-bddc-e52a107fb895"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ddb35baa-aea1-49ca-b733-4865faf3e511"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e8952eb6-e4de-4ba5-ba76-3dbf6d04540e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("e9989910-7bc0-4414-8edf-6cad428a2fbe"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ed63adcb-e255-4e58-bde4-346a0d0eb0d4"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("ef9d781e-35b1-4ddf-adb4-a8317043042e"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f11d6eaf-2dc0-471f-a0a3-a997dc972903"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f12ab79e-28f6-426e-9b02-a355334098b3"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f57c4cf9-4028-49fc-a71d-0505bf222369"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("f5fe8dbd-d3c5-45b7-9cfd-6e714ea8988c"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fad3a017-22b3-461b-9910-1118104ab494"));

            migrationBuilder.DeleteData(
                table: "ModelManagers",
                keyColumn: "Id",
                keyValue: new Guid("fefb72fe-d340-4da5-976b-c9c0e79ac5f0"));

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:SystemSetting:CasdoorClientId");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:SystemSetting:CasdoorClientSecret");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:SystemSetting:CasdoorEndipoint");

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Key",
                keyValue: "Setting:SystemSetting:EnableCasdoorAuth");

            migrationBuilder.DeleteData(
                table: "Tokens",
                keyColumn: "Id",
                keyValue: "7a90ff66-c00f-4d28-a666-6349d2abef2d");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "260e76d0-2355-4db2-97b7-5616e7a01560");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tokens",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiredTime",
                table: "Tokens",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "Tokens",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Tokens",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AccessedTime",
                table: "Tokens",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "RedeemCodes",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RedeemedTime",
                table: "RedeemCodes",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RedeemCodes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "RateLimitModels",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RateLimitModels",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductPurchaseRecords",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchaseTime",
                table: "ProductPurchaseRecords",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductPurchaseRecords",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ModelManagers",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ModelManagers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Channels",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Channels",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

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
                table: "Tokens",
                columns: new[] { "Id", "AccessedTime", "CreatedAt", "Creator", "DeletedAt", "Disabled", "ExpiredTime", "IsDelete", "Key", "LimitModels", "Modifier", "Name", "RemainQuota", "UnlimitedExpired", "UnlimitedQuota", "UpdatedAt", "UsedQuota", "WhiteIpList" },
                values: new object[] { "324ed167-a7b8-4fe1-8dca-f608f6ff93ec", null, new DateTime(2024, 11, 4, 1, 6, 57, 352, DateTimeKind.Local).AddTicks(3391), "a0581ea1-af3a-4d78-8501-b832f6017b98", null, false, null, false, "sk-aAtCeKnf0XMvVCb8ATfSyafvMheD25cs6bwYdH", "[]", null, "默认Token", 0L, true, true, null, 0L, "[]" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "ConsumeToken", "CreatedAt", "Creator", "DeletedAt", "Email", "IsDelete", "IsDisabled", "Modifier", "Password", "PasswordHas", "RequestCount", "ResidualCredit", "Role", "UpdatedAt", "UserName" },
                values: new object[] { "a0581ea1-af3a-4d78-8501-b832f6017b98", null, 0L, new DateTime(2024, 11, 4, 1, 6, 57, 352, DateTimeKind.Local).AddTicks(2940), null, null, "239573049@qq.com", false, false, null, "659a41980932418aeb4b1245ba6a792a", "0519a14e45b343a2ae8b38fcf9c18a06", 0L, 1000000000L, "admin", null, "admin" });
        }
    }
}
