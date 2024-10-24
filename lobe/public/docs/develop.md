
# 部署Thor服务

## 使用docker部署Thor服务

确认服务器已经安装docker

推荐使用docker-compose部署Thor服务

### 下载docker-compose.yml文件

```shell
wget https://raw.githubusercontent.com/AIDotNet/Thor/refs/heads/main/docker-compose-rabbitmq.yml -O docker-compose.yml
```

然后我们的docker-compose.yml下载完成以后我们就可以直接使用指令进行部署

```shell
docker-compose up -d
```

然后我们就可以访问默认的端口`18080` 如果你是在本地部署的话，那么你可以直接访问`http://localhost:18080` 如果你是在服务器上部署的话，那么你可以直接访问`http://你的服务器ip:18080`

然后我们就可以看到Thor的首页。

![Thor首页](./images/1729791466907.jpg)

点击前往控制台

然后输入默认的用户名`admin` 密码`admin` 进入控制台

登录成功以后会进入到Thor的控制台面板

![Thor控制台](./images/1729791537950.jpg)
