@echo off
set MIGRATION_NAME=AddTracing

set DBType=sqllite
dotnet ef migrations add --project src\Provider\Thor.Provider.Sqlite\Thor.Provider.Sqlite.csproj --startup-project src\Thor.Service\Thor.Service.csproj --context Thor.Provider.SqliteLoggerContext --configuration Debug %MIGRATION_NAME%   --output-dir Logger

set DBType=sqlserver
dotnet ef migrations add --project src\Provider\Thor.Provider.SqlServer\Thor.Provider.SqlServer.csproj --startup-project src\Thor.Service\Thor.Service.csproj --context Thor.Provider.SqlServerLoggerContext --configuration Debug %MIGRATION_NAME%   --output-dir Logger

set DBType=postgresql
dotnet ef migrations add --project src\Provider\Thor.Provider.PostgreSql\Thor.Provider.PostgreSql.csproj --startup-project src\Thor.Service\Thor.Service.csproj --context Thor.Provider.PostgreSQLLoggerContext --configuration Debug %MIGRATION_NAME%   --output-dir Logger

set DBType=mysql
dotnet ef migrations add --project src\Provider\Thor.Provider.MySql\Thor.Provider.MySql.csproj --startup-project src\Thor.Service\Thor.Service.csproj --context Thor.Provider.MySqlLoggerContext --configuration Debug %MIGRATION_NAME%   --output-dir Logger

set DBType=dm
dotnet ef migrations add --project src\Provider\Thor.Provider.DaMeng\Thor.Provider.DaMeng.csproj --startup-project src\Thor.Service\Thor.Service.csproj --context Thor.Provider.DMLoggerContext --configuration Debug %MIGRATION_NAME%   --output-dir Logger
