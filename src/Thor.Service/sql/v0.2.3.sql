alter table "ModelManagers"
    add "AudioPromptRate" numeric;

alter table "ModelManagers"
    add "AudioOutputRate" numeric;

alter table "ModelManagers"
    add "IsVersion2" boolean not null default false;


INSERT INTO "ModelManagers" ("Id", "Model", "Enable", "Description", "Available", "CompletionRate", "PromptRate", "AudioPromptRate", "AudioOutputRate", "QuotaType", "QuotaMax", "Tags", "Icon", "IsVersion2", "UpdatedAt", "Modifier", "CreatedAt", "Creator") VALUES ('808BFB96-E162-479C-83AD-D90B7ED3A464', 'gpt-4o-realtime-preview-2024-10-01', true, 'gpt-4o-realtime-preview-2024-10-01 实时模型', true, '10', '2.5', '50', '100', 1, '128K', '["\u6587\u672C","\u89C6\u89C9","\u97F3\u9891"]', 'OpenAI', 1, null, null, '2024-10-30 23:58:08.740816', null);
INSERT INTO "ModelManagers" ("Id", "Model", "Enable", "Description", "Available", "CompletionRate", "PromptRate", "AudioPromptRate", "AudioOutputRate", "QuotaType", "QuotaMax", "Tags", "Icon", "IsVersion2", "UpdatedAt", "Modifier", "CreatedAt", "Creator") VALUES ('CD474EA3-CE84-473C-8C99-17ABB028012D', 'gpt-4o-realtime-preview', true, 'gpt-4o-realtime 实时接口模型', true, '10.0', '2.5', '50.0', '100.0', 1, '128K', '["\u6587\u672C","\u5B9E\u65F6"]', 'OpenAI', 1, null, null, '2024-10-31 00:33:43.8559299', 'fd8404e1-ffa7-431b-8827-b6b617330247');
