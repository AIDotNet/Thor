#!/bin/bash

# 设置构建参数
CONFIGURATION="Release"
PROJECT_PATH="src/Thor.Service/Thor.Service.csproj"
FRONTEND_PATH="lobe"
OUTPUT_DIR="publish"

# 颜色定义
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}========================================"
echo -e "Thor项目自动化构建脚本"
echo -e "========================================${NC}"
echo

# 检查是否安装了必要的工具
echo -e "${YELLOW}检查必要工具...${NC}"

if ! command -v dotnet &> /dev/null; then
    echo -e "${RED}[错误] 未找到.NET SDK，请先安装.NET 9.0 SDK${NC}"
    exit 1
fi

if ! command -v node &> /dev/null; then
    echo -e "${RED}[错误] 未找到Node.js，请先安装Node.js${NC}"
    exit 1
fi

if ! command -v npm &> /dev/null; then
    echo -e "${RED}[错误] 未找到npm，请先安装npm${NC}"
    exit 1
fi

echo -e "${GREEN}[✓] 工具检查完成${NC}"
echo

# 清理旧的构建产物
if [ -d "$OUTPUT_DIR" ]; then
    echo -e "${YELLOW}清理旧的构建产物...${NC}"
    rm -rf "$OUTPUT_DIR"
fi
mkdir -p "$OUTPUT_DIR"

# 构建前端
echo -e "${BLUE}========================================"
echo -e "1. 构建前端项目"
echo -e "========================================${NC}"

cd "$FRONTEND_PATH"

if [ ! -d "node_modules" ]; then
    echo -e "${YELLOW}安装前端依赖...${NC}"
    npm install --force
    if [ $? -ne 0 ]; then
        echo -e "${RED}[错误] 前端依赖安装失败${NC}"
        cd ..
        exit 1
    fi
else
    echo -e "${GREEN}前端依赖已存在，跳过安装${NC}"
fi

echo -e "${YELLOW}构建前端...${NC}"
npm run build
if [ $? -ne 0 ]; then
    echo -e "${RED}[错误] 前端构建失败${NC}"
    cd ..
    exit 1
fi

cd ..
echo -e "${GREEN}[✓] 前端构建完成${NC}"

# 复制前端构建产物到后端wwwroot
echo
echo -e "${YELLOW}复制前端构建产物到后端...${NC}"
if [ -d "src/Thor.Service/wwwroot" ]; then
    rm -rf "src/Thor.Service/wwwroot"
fi
mkdir -p "src/Thor.Service/wwwroot"
cp -r "$FRONTEND_PATH/dist/"* "src/Thor.Service/wwwroot/"
echo -e "${GREEN}[✓] 前端集成完成${NC}"

# 构建后端
echo
echo -e "${BLUE}========================================"
echo -e "2. 构建后端项目"
echo -e "========================================${NC}"

# 还原NuGet包
echo -e "${YELLOW}还原NuGet包...${NC}"
dotnet restore "$PROJECT_PATH"
if [ $? -ne 0 ]; then
    echo -e "${RED}[错误] NuGet包还原失败${NC}"
    exit 1
fi

# 构建项目
echo -e "${YELLOW}构建项目...${NC}"
dotnet build "$PROJECT_PATH" --configuration "$CONFIGURATION" --no-restore
if [ $? -ne 0 ]; then
    echo -e "${RED}[错误] 项目构建失败${NC}"
    exit 1
fi

echo -e "${GREEN}[✓] 后端构建完成${NC}"

# 发布不同版本
echo
echo -e "${BLUE}========================================"
echo -e "3. 发布应用程序"
echo -e "========================================${NC}"

# 定义要构建的运行时和模式
runtimes=("win-x64" "win-arm64" "linux-x64" "linux-arm64" "osx-x64" "osx-arm64")
deployments=("framework-dependent" "self-contained")

for runtime in "${runtimes[@]}"; do
    for deployment in "${deployments[@]}"; do
        echo
        echo -e "${YELLOW}发布 $runtime ($deployment)...${NC}"
        
        if [ "$deployment" == "framework-dependent" ]; then
            SELF_CONTAINED="false"
            SUFFIX="fd"
        else
            SELF_CONTAINED="true"
            SUFFIX="sc"
        fi
        
        OUTPUT_PATH="$OUTPUT_DIR/$runtime-$SUFFIX"
        
        if [ "$deployment" == "framework-dependent" ]; then
            dotnet publish "$PROJECT_PATH" \
                --configuration "$CONFIGURATION" \
                --runtime "$runtime" \
                --no-build \
                --output "$OUTPUT_PATH" \
                /p:PublishSingleFile=true \
                /p:PublishReadyToRun=true
        else
            dotnet publish "$PROJECT_PATH" \
                --configuration "$CONFIGURATION" \
                --runtime "$runtime" \
                --self-contained true \
                --no-build \
                --output "$OUTPUT_PATH" \
                /p:PublishSingleFile=true \
                /p:PublishReadyToRun=true \
                /p:IncludeNativeLibrariesForSelfExtract=true
        fi
        
        if [ $? -ne 0 ]; then
            echo -e "${RED}[错误] $runtime ($deployment) 发布失败${NC}"
        else
            echo -e "${GREEN}[✓] $runtime ($deployment) 发布完成${NC}"
            
            # 创建压缩包
            PACKAGE_NAME="Thor-$runtime-$SUFFIX.tar.gz"
            echo -e "${YELLOW}创建压缩包: $PACKAGE_NAME${NC}"
            tar -czf "$OUTPUT_DIR/$PACKAGE_NAME" -C "$OUTPUT_PATH" .
        fi
    done
done

echo
echo -e "${BLUE}========================================"
echo -e "构建完成！"
echo -e "========================================${NC}"
echo -e "${GREEN}构建产物位置: $OUTPUT_DIR${NC}"
echo
echo -e "${YELLOW}发布包说明:${NC}"
echo -e "- fd = Framework Dependent (依赖框架)"
echo -e "- sc = Self Contained (自包含)"
echo
echo -e "${YELLOW}支持的运行时:${NC}"
echo -e "- win-x64: Windows x64"
echo -e "- win-arm64: Windows ARM64"
echo -e "- linux-x64: Linux x64"
echo -e "- linux-arm64: Linux ARM64"
echo -e "- osx-x64: macOS x64"
echo -e "- osx-arm64: macOS ARM64"
echo
echo -e "${GREEN}生成的发布包:${NC}"
ls -la "$OUTPUT_DIR"/*.tar.gz 2>/dev/null || echo "没有找到压缩包文件"
echo

# 使脚本可执行
chmod +x "$0" 