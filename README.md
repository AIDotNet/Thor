<div align="center"><a name="readme-top"></a>

<img height="160" src="https://api.token-ai.cn/logo.png">

<h1>TokenAI</h1>

TokenAI打造企业级人工智能客服管理系统！

[![][github-contributors-shield]][github-contributors-link]
[![][github-forks-shield]][github-forks-link]
[![][github-stars-shield]][github-stars-link]
[![][github-issues-shield]][github-issues-link]
[![][github-license-shield]][github-license-link]

[Changelog](./CHANGELOG.md) · [Report Bug][github-issues-link] · [Request Feature][github-issues-link]

![](https://raw.githubusercontent.com/andreasbm/readme/master/assets/lines/rainbow.png)

</div>

[npm-release-shield]: https://img.shields.io/npm/v/@lobehub/chat?color=369eff&labelColor=ffcb47&logo=npm&logoColor=white&style=flat-square
[npm-release-link]: https://www.npmjs.com/package/@lobehub/chat
[github-releasedate-shield]: https://img.shields.io/github/release-date/AIDotNet/AIDotNet.API?color=8ae8ff&labelColor=ffcb47&style=flat-square
[github-releasedate-link]: https://github.com/AIDotNet/AIDotNet.API/releases
[github-action-test-shield]: https://img.shields.io/github/actions/workflow/status/AIDotNet/AIDotNet.API/test.yml?color=8ae8ff&label=test&labelColor=ffcb47&logo=githubactions&logoColor=white&style=flat-square
[github-action-test-link]: https://github.com/AIDotNet/AIDotNet.API/actions/workflows/test.yml
[github-action-release-shield]: https://img.shields.io/github/actions/workflow/status/AIDotNet/AIDotNet.API/release.yml?color=8ae8ff&label=release&labelColor=ffcb47&logo=githubactions&logoColor=white&style=flat-square
[github-action-release-link]: https://github.com/AIDotNet/AIDotNet.API/actions/workflows/release.yml
[github-contributors-shield]: https://img.shields.io/github/contributors/AIDotNet/AIDotNet.API?color=c4f042&labelColor=ffcb47&style=flat-square
[github-contributors-link]: https://github.com/AIDotNet/AIDotNet.API/graphs/contributors
[github-forks-shield]: https://img.shields.io/github/forks/AIDotNet/AIDotNet.API?color=8ae8ff&labelColor=ffcb47&style=flat-square
[github-forks-link]: https://github.com/AIDotNet/AIDotNet.API/network/members
[github-stars-shield]: https://img.shields.io/github/stars/AIDotNet/AIDotNet.API?color=ffcb47&labelColor=ffcb47&style=flat-square
[github-stars-link]: https://github.com/AIDotNet/AIDotNet.API/network/stargazers
[github-issues-shield]: https://img.shields.io/github/issues/AIDotNet/AIDotNet.API?color=ff80eb&labelColor=ffcb47&style=flat-square
[github-issues-link]: https://github.com/AIDotNet/AIDotNet.API/issues
[github-license-shield]: https://img.shields.io/github/license/AIDotNet/AIDotNet.API?color=white&labelColor=ffcb47&style=flat-square
[github-license-link]: https://github.com/AIDotNet/AIDotNet.API/blob/main/LICENSE

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
- [x] 腾讯混元大模型

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

