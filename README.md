<div align="center"><a name="readme-top"></a>

<img height="160" src="https://github.com/AIDotNet/Thor/assets/61819790/a3eb7557-0ff4-4edd-a472-e5a0a745616d">

<h1>Thor(雷神托尔)</h1>

Thor(雷神托尔),一个AI大模型管理网关。

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

# Thor(雷神托尔)

Thor(雷神托尔) 是一款强大的人工智能模型管理工具，其主要目的是为了实现多种AI模型的统一管理和使用。通过Thor(雷神托尔)，用户可以轻松地管理和使用众多AI模型，而且Thor(雷神托尔)兼容OpenAI的接口格式，使得使用更加方便。

Thor(雷神托尔)提供了丰富的功能：

1. 管理功能：支持用户管理，渠道管理以及token管理，简化了管理流程。
2. 数据统计预览：可以清晰地看到各种数据的统计情况，帮助用户更好地了解使用情况。
3. 日志查看：支持日志查看，方便用户跟踪和解决问题。
4. 系统设置：可以根据需要进行各种系统设置。
5. 外部Chat链接接入：支持接入外部Chat链接，提升交互体验。
6. 支付宝购买账号余额：提供支付宝购买账号余额的功能，方便用户进行充值。

此外，Thor(雷神托尔)还支持多种AI大模型，包括OpenAI、星火大模型、Claudia、智谱AI、Ollama、通义千问（阿里云）、AzureOpenAI以及腾讯混元大模型，满足了用户对各种AI模型的需求。

Thor(雷神托尔)还支持多种数据库，包括SqlServer、PostgreSql、Sqlite以及MySql，用户可以根据自己的需要选择合适的数据库。

## 功能实现

- [x] 支持用户管理
- [x] 支持渠道管理
- [x] 支持token管理
- [x] 提供数据统计预览
- [x] 支持日志查看
- [x] 支持系统设置
- [x] 支持接入外部Chat链接
- [x] 支持支付宝购买账号余额
- [x] 支持Rabbit消费日志（默认情况下使用本地事件） 

# AI大模型支持列表

- [x] OpenAI （支持function）
- [x] Kimi（月之暗面）（支持function）
- [x] 星火大模型（支持function）
- [x] Claudia (非Stream支持function)
- [x] 智谱AI (支持function)
- [x] AzureOpenAI（支持function）
- [x] Ollama
- [x] 通义千问（阿里云）
- [x] 腾讯混元大模型
- [x] 支持百度大模型(ErnieBot)
- [x] Gitee AI

# 支持数据库

- [x] SqlServer 配置类型[sqlserver,mssql]
- [x] PostgreSql 配置类型[postgresql,pgsql]
- [x] Sqlite 配置类型[sqlite,默认]
- [x] MySql 配置类型[mysql]

修改`appsettings.json`的`ConnectionStrings:DBType`配置项即可切换数据库类型。请注意切换数据库不会迁移数据。

```mermaid
graph LR
    A(用户)
    A --->|使用 Thor 分发的 key 进行请求| B(Thor)
    B -->|中继请求| C(OpenAI)
    B -->|中继请求| D(Azure)
    B -->|中继请求| E(其他 OpenAI API 格式下游渠道)
    B -->|中继并修改请求体和返回体| F(非 OpenAI API 格式下游渠道)
```

## 简单使用

默认账号密码
admin admin

### 注意事项
需要注意的是，如果克隆项目后，项目根目录缺少`data`时，需要手动创建，`docker compose up` 时，需要挂载本地目录`data`。

### 环境变量

- DBType
  sqlite | [postgresql,pgsql] | [sqlserver,mssql] | mysql
- ConnectionString
  主数据库连接字符串
- LoggerConnectionString
  日志数据连接字符串
- CACHE_TYPE
  缓存类型 Memory|Redis
- CACHE_CONNECTION_STRING
  缓存连接字符串 如果是Redis则为Redis连接字符串，Memory则为空
- HttpClientPoolSize
  HttpClient连接池大小

使用`docker compose`启动服务：

```yaml
version: '3.8'

services:
  ai-dotnet-api-service:
    image: registry.token-ai.cn/thor:latest
    ports:
      - 18080:8080
    container_name: ai-dotnet-api-service
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
docker run -d -p 18080:8080 --name ai-dotnet-api-service --network=gateway -v $PWD/data:/data -e Theme=lobe -e TZ=Asia/Shanghai -e DBType=sqlite -e ConnectionString="data source=/data/token.db" -e LoggerConnectionString="data source=/data/logger.db" registry.token-ai.cn/thor:latest
```

### Sqlite构建
`docker compose`版本

项目根目录创建`docker-compose.yml`文件，内容如下：

```yaml
version: '3.8'

services:
  ai-dotnet-api-service:
    image: registry.token-ai.cn/thor:latest
    container_name: ai-dotnet-api-service
    ports:
      - 18080:8080
    volumes:
      - ./data:/data
    environment:
      - TZ=Asia/Shanghai
      - DBType=sqlite
      - ConnectionString=data source=/data/token.db
      - LoggerConnectionString=data source=/data/logger.db
```
执行如下命令打包镜像
```shell
sudo docker compose build
```

执行以下命令启动服务

```shell
sudo docker compose up -d
```


docker run版本

```shell
docker run -d -p 18080:8080 --name ai-dotnet-api-service -v $(pwd)/data:/data -e Theme=lobe -e TZ=Asia/Shanghai -e DBType=sqlite -e ConnectionString=data source=/data/token.db -e LoggerConnectionString=data source=/data/logger.db registry.token-ai.cn/thor:latest
```

然后访问 http://localhost:18080 即可看到服务启动成功。

### PostgreSql构建
`docker compose`版本

项目根目录创建`docker-compose.yml`文件，内容如下：

```yaml
version: '3.8'

services:
  ai-dotnet-api-service:
    image: registry.token-ai.cn/thor:latest
    container_name: ai-dotnet-api-service
    ports:
      - 18080:8080
    volumes:
      - ./data:/data
    environment:
      - TZ=Asia/Shanghai
      - DBType=postgresql
      - "ConnectionString=Host=127.0.0.1;Port=5432;Database=token;Username=token;Password=dd666666"
      - "ConnectionString=Host=127.0.0.1;Port=5432;Database=logger;Username=token;Password=dd666666"
```

执行如下命令打包镜像
```shell
sudo docker compose build
```

执行以下命令启动服务

```shell
sudo docker compose up -d
```

docker run版本

```shell
docker run -d \
  --name ai-dotnet-api-service \
  -p 18080:8080 \
  -v $(pwd)/data:/data \
  -e TZ=Asia/Shanghai \
  -e DBType=postgresql \
  -e "ConnectionString=Host=127.0.0.1;Port=5432;Database=token;Username=token;Password=dd666666" \
  -e "ConnectionString=Host=127.0.0.1;Port=5432;Database=logger;Username=token;Password=dd666666" \
  registry.token-ai.cn/thor:latest
```

然后访问 http://localhost:18080 即可看到服务启动成功。

### SqlServer构建
`docker compose`版本

项目根目录创建`docker-compose.yml`文件，内容如下：

```yaml
version: '3.8'

services:
  ai-dotnet-api-service:
    image: registry.token-ai.cn/thor:latest
    container_name: ai-dotnet-api-service
    ports:
      - 18080:8080
    volumes:
      - ./data:/data
    environment:
      - TZ=Asia/Shanghai
      - DBType=sqlserver
      - "ConnectionString=Server=127.0.0.1;Database=token;User Id=sa;Password=dd666666;"
      - "ConnectionString=Server=127.0.0.1;Database=logger;User Id=sa;Password=dd666666;"
```

执行如下命令打包镜像
```shell
sudo docker compose build
```

执行以下命令启动服务

```shell
sudo docker compose up -d
```

docker run版本

```shell
docker run -d \
  --name ai-dotnet-api-service \
  -p 18080:8080 \
  -v $(pwd)/data:/data \
  -e TZ=Asia/Shanghai \
  -e DBType=sqlserver \
  -e "ConnectionString=Server=127.0.0.1;Database=token;User Id=sa;Password=dd666666;" \
  -e "ConnectionString=Server=127.0.0.1;Database=logger;User Id=sa;Password=dd666666;" \
  registry.token-ai.cn/thor:latest
```

然后访问 http://localhost:18080 即可看到服务启动成功。

### MySql构建
`docker compose`版本

项目根目录创建`docker-compose.yml`文件，内容如下：

```yaml
version: '3.8'

services:
  ai-dotnet-api-service:
    image: registry.token-ai.cn/thor:latest
    container_name: ai-dotnet-api-service
    ports:
      - 18080:8080
    volumes:
      - ./data:/data
    environment:
      - TZ=Asia/Shanghai
      - DBType=mysql
      - "ConnectionString=mysql://root:dd666666@localhost:3306/token"
      - "ConnectionString=mysql://root:dd666666@localhost:3306/logger"
```

执行如下命令打包镜像
```shell
sudo docker compose build
```

执行以下命令启动服务

```shell
sudo docker compose up -d
```
docker run版本

```shell
docker run -d \
  --name ai-dotnet-api-service \
  -p 18080:8080 \
  -v $(pwd)/data:/data \
  -e TZ=Asia/Shanghai \
  -e DBType=mysql \
  -e "ConnectionString=mysql://root:dd666666@localhost:3306/token" \
  -e "ConnectionString=mysql://root:dd666666@localhost:3306/logger" \
  registry.token-ai.cn/thor:latest
```

然后访问 http://localhost:18080 即可看到服务启动成功。
