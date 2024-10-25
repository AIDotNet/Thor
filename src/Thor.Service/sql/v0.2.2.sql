alter table Tokens
    add LimitModels TEXT not null;

alter table Tokens
    add WhiteIpList TEXT not null;

-- 需要给 表提供默认值
update from Tokens
set limitModels = '[]', WhiteIpList = '[]'
where limitModels is null or WhiteIpList is null;