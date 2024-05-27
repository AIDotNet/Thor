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

```mermaid
graph LR
    A(用户)
    A --->|使用 AIDotNet.API 分发的 key 进行请求| B(AIDotNet.API)
    B -->|中继请求| C(OpenAI)
    B -->|中继请求| D(Azure)
    B -->|中继请求| E(其他 OpenAI API 格式下游渠道)
    B -->|中继并修改请求体和返回体| F(非 OpenAI API 格式下游渠道)
```

## 简单使用

默认账号密码
admin admin

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

### Sqlite构建

创建`docker-compose.yml`文件，内容如下：

```yaml
version: '3.8'

services:
	ai-dotnet-api-service:
		image: hejiale010426/ai-dotnet-api-service:latest
		container_name: ai-dotnet-api-service
		ports:
		  - 8080:8080
		volumes:
		  - ./data:/data
		environment:
		  - TZ=Asia/Shanghai
		  - DBType=sqlite
		  - ConnectionString=data source=/data/token.db
		  - LoggerConnectionString=data source=/data/logger.db
```

docker run版本
```shell
docker run -d -p 8080:8080 --name ai-dotnet-api-service -v $(pwd)/data:/data -e TZ=Asia/Shanghai -e DBType=sqlite -e ConnectionString=data source=/data/token.db -e LoggerConnectionString=data source=/data/logger.db hejiale010426/ai-dotnet-api-service:latest
```


执行以下命令启动服务
```shell
sudo docker-compose up -d
```
然后访问 http://localhost:8080 即可看到服务启动成功。

### PostgreSql构建

创建`docker-compose.yml`文件，内容如下：

```yaml
version: '3.8'

services:
	ai-dotnet-api-service:
		image: hejiale010426/ai-dotnet-api-service:latest
		container_name: ai-dotnet-api-service
		ports:
		  - 8080:8080
		volumes:
		  - ./data:/data
		environment:
		  - TZ=Asia/Shanghai
		  - DBType=postgresql
		  - "ConnectionString=Host=127.0.0.1;Port=5432;Database=token;Username=token;Password=dd666666"
		  - "ConnectionString=Host=127.0.0.1;Port=5432;Database=logger;Username=token;Password=dd666666"
```

docker run版本
```shell
docker run -d \
  --name ai-dotnet-api-service \
  -p 8080:8080 \
  -v $(pwd)/data:/data \
  -e TZ=Asia/Shanghai \
  -e DBType=postgresql \
  -e "ConnectionString=Host=127.0.0.1;Port=5432;Database=token;Username=token;Password=dd666666" \
  -e "ConnectionString=Host=127.0.0.1;Port=5432;Database=logger;Username=token;Password=dd666666" \
  hejiale010426/ai-dotnet-api-service:latest
```


执行以下命令启动服务
```shell
sudo docker-compose up -d
```
然后访问 http://localhost:8080 即可看到服务启动成功。

### SqlServer构建

创建`docker-compose.yml`文件，内容如下：

```yaml
version: '3.8'

services:
	ai-dotnet-api-service:
		image: hejiale010426/ai-dotnet-api-service:latest
		container_name: ai-dotnet-api-service
		ports:
		  - 8080:8080
		volumes:
		  - ./data:/data
		environment:
		  - TZ=Asia/Shanghai
		  - DBType=sqlserver
		  - "ConnectionString=Server=127.0.0.1;Database=token;User Id=sa;Password=dd666666;"
		  - "ConnectionString=Server=127.0.0.1;Database=logger;User Id=sa;Password=dd666666;"
```

docker run版本
```shell
docker run -d \
  --name ai-dotnet-api-service \
  -p 8080:8080 \
  -v $(pwd)/data:/data \
  -e TZ=Asia/Shanghai \
  -e DBType=sqlserver \
  -e "ConnectionString=Server=127.0.0.1;Database=token;User Id=sa;Password=dd666666;" \
  -e "ConnectionString=Server=127.0.0.1;Database=logger;User Id=sa;Password=dd666666;" \
  hejiale010426/ai-dotnet-api-service:latest
```


执行以下命令启动服务
```shell
sudo docker-compose up -d
```
然后访问 http://localhost:8080 即可看到服务启动成功。

### MySql构建

创建`docker-compose.yml`文件，内容如下：

```yaml
version: '3.8'

services:
	ai-dotnet-api-service:
		image: hejiale010426/ai-dotnet-api-service:latest
		container_name: ai-dotnet-api-service
		ports:
		  - 8080:8080
		volumes:
		  - ./data:/data
		environment:
		  - TZ=Asia/Shanghai
		  - DBType=mysql
		  - "ConnectionString=mysql://root:dd666666@localhost:3306/token"
		  - "ConnectionString=mysql://root:dd666666@localhost:3306/logger"
```

docker run版本
```shell
docker run -d \
  --name ai-dotnet-api-service \
  -p 8080:8080 \
  -v $(pwd)/data:/data \
  -e TZ=Asia/Shanghai \
  -e DBType=mysql \
  -e "ConnectionString=mysql://root:dd666666@localhost:3306/token" \
  -e "ConnectionString=mysql://root:dd666666@localhost:3306/logger" \
  hejiale010426/ai-dotnet-api-service:latest
```


执行以下命令启动服务
```shell
sudo docker-compose up -d
```
然后访问 http://localhost:8080 即可看到服务启动成功。
