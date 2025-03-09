@echo off
set MIGRATION_NAME=UpdateModelMap

set DBType=sqllite
dotnet ef migrations add --project src\Provider\Thor.Provider.Sqlite\Thor.Provider.Sqlite.csproj --startup-project src\Thor.Service\Thor.Service.csproj --context Thor.Provider.SqliteThorContext --configuration Debug %MIGRATION_NAME%   --output-dir Thor

set DBType=sqlserver
dotnet ef migrations add --project src\Provider\Thor.Provider.SqlServer\Thor.Provider.SqlServer.csproj --startup-project src\Thor.Service\Thor.Service.csproj --context Thor.Provider.SqlServerThorContext --configuration Debug %MIGRATION_NAME%   --output-dir Thor

set DBType=postgresql
dotnet ef migrations add --project src\Provider\Thor.Provider.PostgreSql\Thor.Provider.PostgreSql.csproj --startup-project src\Thor.Service\Thor.Service.csproj --context Thor.Provider.PostgreSqlThorContext --configuration Debug %MIGRATION_NAME%   --output-dir Thor

set DBType=mysql
dotnet ef migrations add --project src\Provider\Thor.Provider.MySql\Thor.Provider.MySql.csproj --startup-project src\Thor.Service\Thor.Service.csproj --context Thor.Provider.MySqlThorContext --configuration Debug %MIGRATION_NAME%   --output-dir Thor

set DBType=dm
dotnet ef migrations add --project src\Provider\Thor.Provider.DaMeng\Thor.Provider.DaMeng.csproj --startup-project src\Thor.Service\Thor.Service.csproj --context Thor.Provider.DaMengThorContext --configuration Debug %MIGRATION_NAME%   --output-dir Thor
