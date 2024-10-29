alter table "Tokens"
    add "LimitModels"TEXT  null;

alter table "Tokens"
    add "WhiteIpList" TEXT  null;

-- 需要给 表提供默认值
update  "Tokens"
set "LimitModels" = '[]', "WhiteIpList" = '[]'
where "LimitModels" is null or "LimitModels" is null;