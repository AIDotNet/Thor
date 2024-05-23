# AIDotNet API 

AIDotNet API 提供了大部分的AI模型兼容OpenAI的接口格式，并且将所有模型的实现单独成类库打包成SDK使用，可快速使用入门，也可以使用AIDotNet API的服务部署成独立的AI中转服务，
在AIDotNet API中提供了基本的用户管理和权限管理，并且支持多模型转换，以便提供给服务OpenAI的API风格。

## 功能实现

- [x] 支持用户管理
- [x] 支持渠道管理
- [x] 支持token管理
- [x] 提供数据统计预览
- [x] 支持日志查看
- [x] 支持系统设置
- [x] 支持接入外部Chat链接
- [x] 支持支付宝购买账号余额

# AI大模型支持列表

- [x] OpenAI （支持function）
- [x] 星火大模型（支持function）
- [x] Claudia
- [x] 智谱AI
- [x] Ollama
- [x] 通义千问（阿里云）   
- [x] AzureOpenAI（支持function）

# 支持数据库

- [x] SqlServer 配置类型[sqlserver,mssql]
- [x] PostgreSql 配置类型[postgresql,pgsql]
- [x] Sqlite 配置类型[sqlite,默认]
- [x] MySql 配置类型[mysql]

修改`appsettings.json`的`ConnectionStrings:DBType`配置项即可切换数据库类型。请注意切换数据库不会迁移数据。

## 简单使用

### 环境变量

- DBType
	sqlite | [postgresql,pgsql] | [sqlserver,mssql] | mysql
- ConnectionString 
	主数据库连接字符串
- LoggerConnectionString
	日志数据连接字符串


使用docker compose启动服务：

```yaml
version: '3.8'

services:
  ai-dotnet-api-service:
    image: hejiale010426/ai-dotnet-api-service:latest
    container_name: ai-dotnet-api-service
    networks:
      - gateway
    volumes:
      - ./data:/data
    environment:
      - TZ=Asia/Shanghai
      - DBType=sqlite # sqlite | [postgresql,pgsql] | [sqlserver,mssql] | mysql
      - ConnectionString=data source=/data/token.db
      - LoggerConnectionString=data source=/data/logger.db
```

使用docker run启动服务

```sh
docker run --name ai-dotnet-api-service --network=gateway -v $PWD/data:/data -e TZ=Asia/Shanghai -e DBType=sqlite -e ConnectionString="data source=/data/token.db" -e LoggerConnectionString="data source=/data/logger.db" hejiale010426/ai-dotnet-api-service:latest
```

