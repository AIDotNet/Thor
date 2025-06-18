# Thor项目构建指南

本文档介绍如何构建Thor项目的Release包，包括GitHub Actions自动化构建和本地手动构建两种方式。

## 🏗️ 构建特性

- ✅ **前后端集成**: 自动将前端构建产物集成到后端wwwroot目录
- ✅ **多架构支持**: 支持Windows、Linux、macOS的x64和ARM64架构
- ✅ **多部署模式**: 支持依赖框架(FD)和自包含(SC)两种部署模式
- ✅ **自动化流水线**: GitHub Actions自动构建和发布
- ✅ **Docker镜像**: 支持多架构Docker镜像构建

## 📋 前置要求

### 开发环境
- **.NET 9.0 SDK** - 后端项目构建
- **Node.js 20+** - 前端项目构建
- **npm** - 前端包管理器

### 可选工具
- **Docker** - 容器化部署
- **Git** - 版本控制

## 🚀 GitHub Actions自动构建

### 触发条件
自动构建会在以下情况触发：
- 推送到`main`分支
- 创建标签（`v*`格式）
- 提交Pull Request到`main`分支
- 手动触发（workflow_dispatch）

### 构建流程
1. **前端构建** - 构建React应用并生成静态文件
2. **后端构建** - 为多个平台和架构构建.NET应用
3. **Docker镜像** - 构建多架构Docker镜像
4. **发布Release** - 当推送标签时自动创建GitHub Release

### 构建产物
- **Release包**: 支持12种不同的平台/架构/部署模式组合
- **Docker镜像**: 支持linux/amd64和linux/arm64架构

### 常见问题解决
如果遇到Node.js缓存错误（`Some specified paths were not resolved`），我们已经优化了配置：
- 移除了对不存在的`package-lock.json`的依赖
- 简化了前端依赖安装流程
- 确保构建过程的稳定性

## 🔧 本地构建

### Windows用户
```cmd
# 运行Windows构建脚本
build.bat
```

### Linux/macOS用户
```bash
# 设置执行权限（首次运行）
chmod +x build.sh

# 运行构建脚本
./build.sh
```

### 构建步骤说明
1. **环境检查** - 验证必要工具是否安装
2. **前端构建** - 安装依赖并构建前端项目
3. **前端集成** - 将前端构建产物复制到后端wwwroot
4. **后端构建** - 还原包、编译项目
5. **多平台发布** - 生成各种平台和部署模式的Release包

## 📦 发布包说明

### 命名规则
- `Thor-{runtime}-{deployment}.{ext}`
- 例如：`Thor-win-x64-fd.zip`、`Thor-linux-x64-sc.tar.gz`

### 部署模式
- **fd (Framework Dependent)**: 依赖框架，需要目标机器安装.NET运行时
  - 优点：包体积小，启动快
  - 缺点：需要预装.NET运行时
  
- **sc (Self Contained)**: 自包含，包含完整的.NET运行时
  - 优点：无需预装.NET运行时，独立运行
  - 缺点：包体积大

### 支持的运行时
| 运行时 | 操作系统 | 架构 | 说明 |
|--------|----------|------|------|
| win-x64 | Windows | x64 | Windows 64位 |
| win-arm64 | Windows | ARM64 | Windows ARM64 |
| linux-x64 | Linux | x64 | Linux 64位 |
| linux-arm64 | Linux | ARM64 | Linux ARM64 |
| osx-x64 | macOS | x64 | macOS Intel |
| osx-arm64 | macOS | ARM64 | macOS Apple Silicon |

## 🐳 Docker部署

### 从Docker Hub拉取
```bash
# 拉取最新版本
docker pull aidotnet/thor:latest

# 运行容器
docker run -d -p 8080:8080 aidotnet/thor:latest
```

### 本地构建Docker镜像
```bash
# 构建镜像
docker build -f src/Thor.Service/Dockerfile -t thor:local .

# 运行容器
docker run -d -p 8080:8080 thor:local
```

## 🔧 配置说明

### 环境变量
可以通过环境变量配置应用行为，具体配置项请参考：
- `src/Thor.Service/appsettings.json` - 默认配置
- `src/Thor.Service/appsettings.Development.json` - 开发环境配置

### 数据库支持
Thor支持多种数据库：
- SQLite（默认）
- SQL Server
- PostgreSQL
- MySQL
- 达梦数据库

## 📝 开发说明

### 项目结构
```
Thor/
├── lobe/                     # React前端项目
├── src/
│   ├── Thor.Service/         # 主服务项目
│   ├── extensions/           # AI模型扩展
│   ├── framework/            # 框架组件
│   └── Provider/             # 数据库提供程序
├── .github/workflows/        # GitHub Actions配置
├── build.bat                 # Windows构建脚本
├── build.sh                  # Linux/macOS构建脚本
└── BUILD.md                  # 本文档
```

### 添加新的AI模型
1. 在`src/extensions/`下创建新的扩展项目
2. 实现相应的接口
3. 在主项目中注册服务

### 修改前端
1. 进入`lobe`目录
2. 运行`npm install`安装依赖
3. 运行`npm run dev`启动开发服务器
4. 修改代码后运行`npm run build`构建

## 🚀 发布流程

### 自动发布（推荐）
1. 创建并推送标签：
```bash
git tag v1.0.0
git push origin v1.0.0
```
2. GitHub Actions会自动构建并创建Release

### 手动发布
1. 运行本地构建脚本
2. 上传`publish/`目录下的压缩包到GitHub Release
3. 推送Docker镜像到Registry

## ❓ 常见问题

### Q: 构建失败，提示找不到.NET SDK
A: 请确保安装了.NET 9.0 SDK，可从[官网](https://dotnet.microsoft.com/download)下载

### Q: 前端构建失败
A: 确保Node.js版本为20+，删除`lobe/node_modules`目录后重新运行

### Q: Docker构建慢
A: 可以使用GitHub Actions的缓存机制，或在本地使用BuildKit

### Q: 如何只构建特定平台
A: 修改构建脚本中的`runtimes`数组，只保留需要的运行时

## 📞 支持

如有问题，请：
1. 查看[GitHub Issues](https://github.com/AIDotNet/Thor/issues)
2. 提交新的Issue描述问题
3. 联系项目维护者

---

**Thor项目** - 开源免费的AI模型聚合服务平台 